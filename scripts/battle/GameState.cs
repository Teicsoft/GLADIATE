using System;
using System.Collections.Generic;
using System.Linq;
using GLADIATE.scripts.battle.card;
using GLADIATE.scripts.battle.target;
using GLADIATE.scripts.XmlParsing;
using Godot;

namespace GLADIATE.scripts.battle;

public class GameState {
    public event EventHandler MultiplierChangedCustomEvent;
    public event EventHandler SpectacleChangedCustomEvent;
    public event EventHandler DiscardStateChangedCustomEvent;
    public event EventHandler AllEnemiesDefeatedCustomEvent;
    public event EventHandler ComboStackChangedCustomEvent;
    public event EventHandler<ComboEventArgs> ComboPlayedCustomEvent;
    public event EventHandler SelectedEnemyIndexChangedCustomEvent;

    public List<Combo> AllCombos;
    public Player Player;
    public Hand Hand;
    public List<Enemy> Enemies;
    public List<Card> ComboStack;
    public int SpectacleBuffer;
    private int _multiplier;
    private int _spectaclePoints;
    private int _discards;
    private int _turnStartEnemyCount;
    private int _selectedEnemyIndex = -1;
    public int TurnDamageCount;

    public int Discards {
        get => _discards;
        set {
            if (value > 0) { StartDiscarding(); }

            if (value == 0) { StopDiscarding(); }

            _discards = value;
            DiscardStateChangedCustomEvent?.Invoke(this, EventArgs.Empty);
        }
    }

    public int Multiplier {
        get => _multiplier;
        set {
            Utils.DirectionEventArgs args = Utils.CheckDirection(_multiplier, value);
            _multiplier = value;
            MultiplierChangedCustomEvent?.Invoke(this, args);
        }
    }

    public int SpectaclePoints {
        get => _spectaclePoints;
        set {
            Utils.DirectionEventArgs args = Utils.CheckDirection(_spectaclePoints, value);
            _spectaclePoints = value;
            SpectacleChangedCustomEvent?.Invoke(this, args);
        }
    }

    // Constructor
    public GameState(Hand hand, List<Enemy> enemies) {
        Player = new Player(500, 0, 0);
        Player.PlayerDamageTakenCustomEvent += PlayerDamageTaken;
        AllCombos = ComboXmlParser.ParseAllCombos(); // Retrieve a list of all combos as model objects
        ComboStack = new List<Card>();
        Multiplier = 1; // 1 is lowest possible value
        SpectaclePoints = 0;
        TurnDamageCount = 0;
        Hand = hand;
        Enemies = enemies;
        Enemies.ForEach(enemy => enemy.EnemySelected += SelectEnemy);
    }

    public void StartTurn() {
        GD.Print(" ==== ==== START TURN ==== ====");
        _turnStartEnemyCount = GetAliveEnemies().Count;
        SpectacleBuffer = 0;
        if (Player.IsStunned()) { EndTurn(); } else { Draw(); }
    }

    public void Draw(int n = 1) { Hand.DrawCards(n); }

    public void PlaySelectedCard() {
        CardSleeve cardSleeve = Hand.GetSelectedCard();
        Enemy selectedEnemy = GetSelectedEnemy();
        if (cardSleeve != null && cardSleeve.Card.IsPlayable(selectedEnemy)) {
            cardSleeve.Card.Play(this, selectedEnemy, Player);
            if (Player.Statuses.Contains(Utils.StatusEnum.MoveShouted)) { SpectacleBuffer += 10; }
            ComboCheck(cardSleeve.Card);
            Player.Statuses.Remove(Utils.StatusEnum.OpenedRecklessly);
            Hand.DiscardCard();
            DeselectDeadEnemy();
            HideDeadEnemies();
            if (Hand.Cards.Count == 0) { EndTurn(); }
        }
    }

    public void ComboCheck(Card card) { // largely based on Cath's python code
        PushCardStack(card);

        // find a matching combo if it exists, returns null if no match
        Combo matchingCombo = ComboCompare();
        if (matchingCombo != null) {
            ComboPlayedCustomEvent?.Invoke(this, new ComboEventArgs(matchingCombo));
            ProcessCombo(matchingCombo);
        } else { ComboStackChangedCustomEvent?.Invoke(this, EventArgs.Empty); }
    }

    public void PushCardStack(Card card) { ComboStack.Add(card); }

    public Combo ComboCompare() {
        foreach (Combo combo in AllCombos) {
            if (ComboStack.Count < combo.CardList.Count) { continue; }
            bool match = true;
            for (int i = 1; i <= combo.CardList.Count; i++) {
                Card playedCard = ComboStack[^i];
                Card comboCard = combo.CardList[^i];
                if (playedCard.CardType == "Block" && comboCard.Id == "card_FullBlock") { continue; }
                if (playedCard.Id != comboCard.Id) {
                    match = false;
                    break;
                }
            }

            if (match) { return combo; }
        }

        return null;
    }

    private void ProcessCombo(Combo combo) {
        ProcessMultiplier(combo?.CardList.Count ?? 0);
        combo?.Play(this);
        ShowOffCheck();

        SpectaclePoints += Math.Abs(SpectacleBuffer * Multiplier);
        SpectacleBuffer = 0;

        ComboStack.Clear();
        ComboStackChangedCustomEvent?.Invoke(this, EventArgs.Empty);
    }

    private void ShowOffCheck() {
        if (Player.Statuses.Contains(Utils.StatusEnum.JustShowedOff)) {
            Player.Statuses.Remove(Utils.StatusEnum.JustShowedOff);
            Player.Statuses.Add(Utils.StatusEnum.ShowedOff);
        } else if (Player.Statuses.Contains(Utils.StatusEnum.ShowedOff)) {
            Player.Statuses.Remove(Utils.StatusEnum.ShowedOff);
            SpectacleBuffer *= 2;
        }
    }

    public void ProcessMultiplier(int comboLength) {
        int comboValue = (int)Math.Floor(Math.Pow(2, comboLength - 1));

        int blunders = ComboStack.Count - comboLength;
        int blunderValue = (int)Math.Floor(Math.Pow(2, blunders - 1));

        Multiplier = Math.Max(Multiplier + (comboValue - blunderValue), 1);
    }

    public void EndTurn() {
        if (Discards > 0) {
            if (Hand.Cards.Count == 0) { Discards = 0; } else { return; }
        }

        TurnDamageCount = 0;
        ProcessCombo(null);
        CrowdPleasedCheck(GetAliveEnemies().Count);
        if (GetAliveEnemies().Count == 0) { AllEnemiesDefeatedCustomEvent?.Invoke(this, EventArgs.Empty); } else {
            TakeEnemyTurns(GetAliveEnemies());
        }
        Utils.RemoveEndTurnStatuses(Player);

        DeselectDeadEnemy();
        HideDeadEnemies();

        GD.Print(" ==== ====  END TURN  ==== ====");
        StartTurn();
    }

    private void TakeEnemyTurns(List<Enemy> aliveEnemies) {
        foreach (Enemy enemy in aliveEnemies) {
            if (enemy.IsStunned()) {
                // Update HUD
                continue;
            }
            Label cardPlayedLabel = enemy.GetNode<Label>("HealthBar/CardPlayed");
            Timer cardPlayedTimer = enemy.GetNode<Timer>("HealthBar/CardPlayedTimer");

            Card card = enemy.DrawCard();
            card.Play(this, Player, enemy);
            enemy.TakeCardIntoDiscard(card);
            Utils.RemoveEndTurnStatuses(enemy);

            cardPlayedLabel.Text = card.CardName;
            cardPlayedLabel.Visible = true;


            if (enemy.BossHealthBar != null) {
                Label bossCardPlayedLabel = enemy.BossHealthBar.GetNode<Label>("MarginContainer/Control/CardPlayed");
                bossCardPlayedLabel.Text = card.CardName;
                bossCardPlayedLabel.Visible = true;
            }

            cardPlayedTimer.Start();
        }
    }

    private void CrowdPleasedCheck(int aliveEnemiesCount) {
        if (Player.Statuses.Remove(Utils.StatusEnum.CrowdPleased) && aliveEnemiesCount < _turnStartEnemyCount) {
            int enemiesDefeated = _turnStartEnemyCount - aliveEnemiesCount;
            Draw(enemiesDefeated * 2);
            SpectaclePoints += (enemiesDefeated * 20) * Multiplier;
        }
    }

    private void PlayerDamageTaken(object sender, EventArgs args) {
        TurnDamageCount += ((Utils.DamageEventArgs)args).Damage;
    }

    public void StartDiscarding() {
        foreach (CardSleeve sleeve in Hand.Cards) {
            sleeve.CardSelected -= SelectedDiscard;
            sleeve.CardSelected += SelectedDiscard;
        }
    }

    public void StopDiscarding() {
        foreach (CardSleeve sleeve in Hand.Cards) { sleeve.CardSelected -= SelectedDiscard; }

        foreach (CardSleeve sleeve in Hand.Deck.Cards) { sleeve.CardSelected -= SelectedDiscard; }

        foreach (CardSleeve sleeve in Hand.Discard.Cards) { sleeve.CardSelected -= SelectedDiscard; }
    }

    private void SelectedDiscard(CardSleeve sleeve) {
        Discards--;
        Hand.DiscardCard(sleeve);
        if (Hand.Cards.Count == 0) { EndTurn(); }
    }

    // Enemy methods
    public Enemy GetSelectedEnemy() { return _selectedEnemyIndex != -1 ? Enemies[_selectedEnemyIndex] : null; }

    public void SelectEnemy(Enemy enemy) {
        if (enemy.Health > 0) {
            int enemyIndex = Enemies.IndexOf(enemy);
            _selectedEnemyIndex = _selectedEnemyIndex != enemyIndex ? enemyIndex : -1;
            SelectedEnemyIndexChangedCustomEvent?.Invoke(this, EventArgs.Empty);
        }
    }

    public List<Enemy> GetAliveEnemies() { return Enemies.FindAll(e => e.Health > 0); }

    public List<Enemy> GetDeadEnemies() { return Enemies.FindAll(e => e.Health <= 0); }

    private void DeselectDeadEnemy() {
        if ((GetSelectedEnemy()?.Health ?? -1) <= 0) { _selectedEnemyIndex = -1; }
        SelectedEnemyIndexChangedCustomEvent?.Invoke(this, EventArgs.Empty);
    }

    private void HideDeadEnemies() {
        foreach (Enemy deadEnemy in GetDeadEnemies()) {
            if (deadEnemy.Id != "enemy_Goon") { deadEnemy.Visible = false; }
        }
    }

    public override string ToString() {
        return $"ComboMultiplier: {Multiplier}," + $"SpectaclePoints: {SpectaclePoints}," +
               $"ComboStack: {ComboStack}," + $"AllCombos: {AllCombos}";
    }
}

public class ComboEventArgs : EventArgs {
    public Combo Combo;
    public ComboEventArgs(Combo combo) { Combo = combo; }
}
