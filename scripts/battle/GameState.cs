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
    public event EventHandler<IntEventArgs> DiscardStateChangedCustomEvent;
    public Player Player;
    private int _multiplier;

    private int _discardsRemaining;

    public int DiscardsRemaining {
        get => _discardsRemaining;
        set {
            _discardsRemaining = value;
            if (value > 0) { StartDiscarding(); }
            else if (value == 0) { StopDiscarding(); }
            DiscardStateChangedCustomEvent?.Invoke(this,new IntEventArgs(value));
        }
    }

    public int Multiplier {
        get => _multiplier;
        set {
            _multiplier = value;
            MultiplierChangedCustomEvent?.Invoke(this, EventArgs.Empty);
        }
    }

    private int _spectaclePoints;

    public int SpectaclePoints {
        get => _spectaclePoints;
        set {
            _spectaclePoints = value;
            SpectacleChangedCustomEvent?.Invoke(this, EventArgs.Empty);
        }
    }

    public List<Card> ComboStack { get; set; }

    // changed this back to Card objects, as we use spectacle points in the combo processing. Easier than tracking separately.

    private List<Combo> AllCombos { get; set; }

    public List<Enemy> Enemies = new();
    private int _selectedEnemyIndex = -1;
    public Hand Hand;
    public Deck<CardSleeve> Deck;

    // Constructor
    public GameState() {
        AllCombos = ComboXmlParser.ParseAllCombos(); // Retrieve a list of all combos as model objects
        Player = new Player(100, 0, 0);
        ComboStack = new List<Card>();
        Multiplier = 1; // 1 is lowest possible value
        SpectaclePoints = 0;
    }

    public void EndTurn() {
        if (DiscardsRemaining > 0) return;
        ProcessCombo(null);
        foreach (Enemy enemy in Enemies.FindAll(enemy => enemy.Health > 0)) {
            Card card = enemy.DrawCard();
            card.Play(this, Player, enemy);
            enemy.TakeCardIntoDiscard(card);
        }

        GD.Print(" ==== ====  END TURN  ==== ====");
        StartTurn();
    }

    public void StartTurn() {
        GD.Print(" ==== ==== START TURN ==== ====");
        Draw();
    }

    // Player Methods
    // ****
    public void DamagePlayer(int damage, Utils.PositionEnum position = Utils.PositionEnum.Upper) {
        Player.Damage(damage, position);
    }

    public void HealPlayer(int amount) { Player.Heal(amount); }

    public void StunPlayer(int stun, Utils.PositionEnum position = Utils.PositionEnum.Upper) { Player.Stun(stun); }

    public void ModifyPlayerBlock(int change, Utils.PositionEnum position) { Player.ModifyBlock(change, position); }

    // ****

    // Combo Methods
    // ****
    public void PushCardStack(Card card) { ComboStack.Add(card); }

    public void ComboCheck(Card card) { // largely based on Cath's python code
        PushCardStack(card);

        // find a matching combo if it exists, returns null if no match
        Combo matchingCombo = ComboCompare();
        if (matchingCombo != null) {
            GD.Print("C-C-COMBO!!!");
            GD.Print("Playing Combo: " + matchingCombo);
            ProcessCombo(matchingCombo);
        }
    }

    private void ProcessCombo(Combo combo) {
        int spectaclePoints = ComboStack.Sum(card => card.SpectaclePoints) + (combo?.SpectaclePoints ?? 0);
        ProcessMultiplier(combo?.CardList.Count ?? 0);

        combo?.Play(this);

        SpectaclePoints += Math.Abs(spectaclePoints * Multiplier);

        ComboStack.Clear();
    }

    // Check for combo matches
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

    public void ProcessMultiplier(int comboLength) {
        int comboValue = (int)Math.Floor(Math.Pow(2, comboLength - 1));

        int blunders = ComboStack.Count - comboLength;
        int blunderValue = (int)Math.Floor(Math.Pow(2, blunders - 1));

        Multiplier = Math.Max(Multiplier + (comboValue - blunderValue), 1);
    }

    // ****

    // Hand methods

    public void PlaySelectedCard() {
        CardSleeve cardSleeve = Hand.GetSelectedCard();
        if (cardSleeve != null && !(cardSleeve.Card.TargetRequired && GetSelectedEnemy() == null)) {
            cardSleeve.Card.Play(this, GetSelectedEnemy(), Player);
            Hand.DiscardCard();
        }
    }

    public void Draw(int n = 1) { Hand.AddCards(Deck.DrawCards(n)); }

    public void StartDiscarding() {
        foreach (CardSleeve sleeve in Hand.Cards) { sleeve.CardSelected += SelectedDiscard; }
    }

    public void StopDiscarding() {
        foreach (CardSleeve sleeve in Hand.Cards) { sleeve.CardSelected -= SelectedDiscard; }
    }

    private void SelectedDiscard(CardSleeve cardSleeve) {
        DiscardsRemaining--;
        Hand.DiscardCard(cardSleeve);
    }

    // ****

    // Enemy methods
    // ****

    public Enemy GetSelectedEnemy() { return _selectedEnemyIndex != -1 ? Enemies[_selectedEnemyIndex] : null; }

    public void SelectEnemy(Enemy enemy) {
        int enemyIndex = Enemies.IndexOf(enemy);
        _selectedEnemyIndex = _selectedEnemyIndex != enemyIndex ? enemyIndex : -1;
    }

    // ****

    public override string ToString() {
        return $"ComboMultiplier: {Multiplier}," + $"SpectaclePoints: {SpectaclePoints}," +
               $"ComboStack: {ComboStack}," + $"AllCombos: {AllCombos}";
    }
}

public class IntEventArgs : EventArgs {
    public IntEventArgs(int n) {
        N = n;
    }

    public int N { get; set; }
}
