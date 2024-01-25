using System;
using System.Collections.Generic;
using Godot;
using TeicsoftSpectacleCards.scripts.battle.card;
using TeicsoftSpectacleCards.scripts.battle.target;
using TeicsoftSpectacleCards.scripts.XmlParsing;

namespace TeicsoftSpectacleCards.scripts.battle;

public class GameState {

    public Player Player;
    public int Multiplier { get; set; }
    public int SpectaclePoints { get; set; }

    public List<Card> ComboStack { get; set; }

    // changed this back to Card objects, as we use spectacle points in the combo processing. Easier than tracking separately.

    private List<Combo> AllCombos { get; set; }

    public List<Enemy> Enemies = new();
    private int SelectedEnemyIndex = -1;
    public Hand Hand;
    public Deck<CardSleeve> Deck;

    // Constructor
    public GameState() {
        AllCombos = ComboXmlParser.ParseAllCombos(); // Retrieve a list of all combos as model objects
        Player = new Player(12, 0, 0);
        ComboStack = new List<Card>();
        Multiplier = 1; // 1 is lowest possible value
        SpectaclePoints = 0;
    }

    public void EndTurn() {
        GD.Print(SpectaclePoints);
        GD.Print(Player.Health);
        ProcessCombo(null);
        foreach (Enemy enemy in Enemies) {
            Card card = enemy.DrawCard();
            card.Play(this, Player, enemy);
            enemy.TakeCardIntoDiscard(card);
        }

        GD.Print(" ==== ====  END TURN  ==== ====");
        StartTurn();
    }

    public void StartTurn() {
        GD.Print(" ==== ==== START TURN ==== ====");
        GD.Print(SpectaclePoints);
        GD.Print(Player.Health);
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

        // find a matching combo if it exists, returns null if no match
        Combo matchingCombo = ComboCompare();
        if (matchingCombo != null) {
            GD.Print("C-C-COMBO!!!");
            GD.Print(matchingCombo.Name);
            ProcessCombo(matchingCombo);
        }
    }

    private void ProcessCombo(Combo combo) {
        combo?.Play(this);
        ProcessMultiplier(combo?.CardList.Count ?? 0);
        ProcessSpectaclePoints(combo?.SpectaclePoints ?? 0);
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

        Multiplier = Math.Max(Multiplier + comboMult, 1);
    }

    public void ProcessSpectaclePoints(int spectaclePoints) {
        foreach (Card card in ComboStack) { spectaclePoints += card.SpectaclePoints; }

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

    public target.Enemy GetSelectedEnemy() {
        return SelectedEnemyIndex != -1 ? Enemies[SelectedEnemyIndex] : null;
    }

    public void SelectEnemy(Enemy enemy) {
        int enemyIndex = Enemies.IndexOf(enemy);
        SelectedEnemyIndex = SelectedEnemyIndex != enemyIndex ? enemyIndex : -1;
    }

    // ****

    public override string ToString() {
        return $"ComboMultiplier: {Multiplier}," + $"SpectaclePoints: {SpectaclePoints}," +
               $"ComboStack: {ComboStack}," + $"AllCombos: {AllCombos}";
    }
}
