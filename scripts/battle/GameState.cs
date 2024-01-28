using System;
using System.Collections.Generic;
using Godot;
using TeicsoftSpectacleCards.scripts.battle.card;
using TeicsoftSpectacleCards.scripts.battle.target;
using TeicsoftSpectacleCards.scripts.XmlParsing;

namespace TeicsoftSpectacleCards.scripts.battle;

public class GameState {

    public event EventHandler MultiplierChangedCustomEvent;
    public event EventHandler SpectacleChangedCustomEvent;
    public Player Player;
    private int _multiplier;

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

    public void HealPlayer(int amount) {
        Player.Heal(amount);
    }

    public void StunPlayer(int stun, Utils.PositionEnum position = Utils.PositionEnum.Upper) {
        Player.Stun(stun);
    }

    public void ModifyPlayerBlock(int change, Utils.PositionEnum position) {
        Player.ModifyBlock(change, position);
    }

    // ****

    // Combo Methods
    // ****
    public void PushCardStack(Card card) {
        ComboStack.Add(card);
    }

    public void ComboCheck(Card card) { // largely based on Cath's python code
        PushCardStack(card);
        GD.Print("Combo Stack Size: " + ComboStack.Count);

        // find a matching combo if it exists, returns null if no match
        Combo matchingCombo = ComboCompare();
        if (matchingCombo != null) {
            GD.Print("C-C-COMBO!!!");
            ProcessCombo(matchingCombo);
        }
    }

    private void ProcessCombo(Combo combo) {
        GD.Print("Playing Combo: " + (combo?.Name ?? "null"));
        combo?.Play(this);
        GD.Print("Processing Multiplier");
        ProcessMultiplier(combo?.CardList.Count ?? 0);
        GD.Print("Processing Spectacle");
        ProcessSpectaclePoints(combo?.SpectaclePoints ?? 0);
        GD.Print("Clearing Combo Stack");
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
        int blunders = ComboStack.Count - comboLength;
        int comboValue = (int)Math.Floor(Math.Pow(2, comboLength - 1));
        int blunderValue = (int)Math.Floor(Math.Pow(2, blunders - 1));
        int comboMult = comboValue - blunderValue;
        GD.Print("Total ComboStack Size: " + ComboStack.Count);
        GD.Print("Blunders: " + blunders);
        GD.Print("comboValue: " + comboValue);
        GD.Print("blunderValue: " + blunderValue);
        GD.Print("Adding " + comboMult + " to Multiplier");

        Multiplier = Math.Max(Multiplier + comboMult, 1);
    }

    public void ProcessSpectaclePoints(int spectaclePoints) {
        foreach (Card card in ComboStack) { spectaclePoints += card.SpectaclePoints; }

        GD.Print("Base Combo Spectacle Points: " + spectaclePoints);

        SpectaclePoints += Math.Abs(spectaclePoints * Multiplier);
    }

    // ****

    // Hand methods

    public void PlaySelectedCard() {
        CardSleeve cardSleeve = Hand.GetSelectedCard();
        if (cardSleeve != null && !(cardSleeve.Card.TargetRequired && GetSelectedEnemy() == null)) {
            cardSleeve.Card.Play(this, GetSelectedEnemy(), Player);
            Hand.Discard();
        }
    }

    public void Draw(int n = 1) {
        Hand.AddCards(Deck.DrawCards(n));
    }

    // ****

    // Enemy methods
    // ****

    public Enemy GetSelectedEnemy() {
        return _selectedEnemyIndex != -1 ? Enemies[_selectedEnemyIndex] : null;
    }

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
