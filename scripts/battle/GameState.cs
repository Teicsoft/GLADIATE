using System;
using System.Collections.Generic;
using System.Linq;
using Godot;
using TeicsoftSpectacleCards.scripts.battle.card;
using TeicsoftSpectacleCards.scripts.battle.target;
using TeicsoftSpectacleCards.scripts.XmlParsing;

namespace TeicsoftSpectacleCards.scripts.battle;

public class GameState {
    public event EventHandler MultiplierChangedCustomEvent;
    public event EventHandler SpectacleChangedCustomEvent;
    public event EventHandler DiscardStateChangedCustomEvent;
    public event EventHandler AllEnemiesDefeatedCustomEvent;
    public event EventHandler ComboStackChangedCustomEvent;

    private List<Combo> AllCombos;
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
            _multiplier = value;
            MultiplierChangedCustomEvent?.Invoke(this, EventArgs.Empty);
        }
    }

    public int SpectaclePoints {
        get => _spectaclePoints;
        set {
            _spectaclePoints = value;
            SpectacleChangedCustomEvent?.Invoke(this, EventArgs.Empty);
        }
    }

    // Constructor
    public GameState(Hand hand, List<Enemy> enemies) {
        AllCombos = ComboXmlParser.ParseAllCombos(); // Retrieve a list of all combos as model objects
        Player = new Player(100, 0, 0);
        ComboStack = new List<Card>();
        Multiplier = 1; // 1 is lowest possible value
        SpectaclePoints = 0;
        Hand = hand;
        Enemies = enemies;
        Enemies.ForEach(enemy => enemy.EnemySelected += SelectEnemy);
    }

    public void StartTurn() {
        GD.Print(" ==== ==== START TURN ==== ====");
        _turnStartEnemyCount = Enemies.FindAll(enemy => enemy.Health > 0).Count;
        SpectacleBuffer = 0;
        Draw();
    }

    public void Draw(int n = 1) { Hand.DrawCards(n); }

    public void PlaySelectedCard() {
        CardSleeve cardSleeve = Hand.GetSelectedCard();
        if (cardSleeve != null && !(cardSleeve.Card.TargetRequired && GetSelectedEnemy() == null)) {
            cardSleeve.Card.Play(this, GetSelectedEnemy(), Player);
            if (Player.Statuses.Contains(Utils.StatusEnum.MoveShouted)) {
                SpectacleBuffer += 10;
            }
            ComboCheck(cardSleeve.Card);
            Hand.DiscardCard();
        }
    }

    public void ComboCheck(Card card) { // largely based on Cath's python code
        PushCardStack(card);

        // find a matching combo if it exists, returns null if no match
        Combo matchingCombo = ComboCompare();
        if (matchingCombo != null) {
            GD.Print("C-C-COMBO!!!");
            GD.Print("Playing Combo: " + matchingCombo.Name);
            ProcessCombo(matchingCombo);
        } else { ComboStackChangedCustomEvent?.Invoke(this, EventArgs.Empty); }
    }

    public void PushCardStack(Card card) { ComboStack.Add(card); }

    public Combo ComboCompare() {
        foreach (Combo combo in AllCombos) {
            int count = combo.CardList.Count;
            if (ComboStack.Count < count) { continue; }

            bool match = true;
            for (int i = 1; i <= count; i++) {
                if (ComboStack[^i].Id != combo.CardList[^i].Id) {
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

        // Safe to do, even if MoveShouted not in the Set.

        ProcessCombo(null);
        List<Enemy> aliveEnemies = Enemies.FindAll(enemy => enemy.Health > 0);
        CrowdPleasedCheck(aliveEnemies.Count);
        if (aliveEnemies.Count == 0) { AllEnemiesDefeatedCustomEvent?.Invoke(this, EventArgs.Empty); } else {
            foreach (Enemy enemy in aliveEnemies) {
                if (enemy.IsStunned()) {
                    // Update HUD
                    continue;
                }
                Card card = enemy.DrawCard();
                card.Play(this, Player, enemy);
                enemy.TakeCardIntoDiscard(card);
                Utils.RemoveEndTurnStatuses(enemy);
            }
        }
        Utils.RemoveEndTurnStatuses(Player);

        GD.Print(" ==== ====  END TURN  ==== ====");
        StartTurn();
    }

    private void CrowdPleasedCheck(int aliveEnemiesCount) {
        if (Player.Statuses.Remove(Utils.StatusEnum.CrowdPleased) && aliveEnemiesCount > _turnStartEnemyCount) {
            int enemiesDefeated = _turnStartEnemyCount - aliveEnemiesCount;
            SpectaclePoints += (enemiesDefeated * 20) * Multiplier;
            Draw(enemiesDefeated*2);
        }
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
    }

    // Enemy methods
    public Enemy GetSelectedEnemy() { return _selectedEnemyIndex != -1 ? Enemies[_selectedEnemyIndex] : null; }

    public void SelectEnemy(Enemy enemy) {
        int enemyIndex = Enemies.IndexOf(enemy);
        _selectedEnemyIndex = _selectedEnemyIndex != enemyIndex ? enemyIndex : -1;
    }

    public override string ToString() {
        return $"ComboMultiplier: {Multiplier}," + $"SpectaclePoints: {SpectaclePoints}," +
               $"ComboStack: {ComboStack}," + $"AllCombos: {AllCombos}";
    }
}
