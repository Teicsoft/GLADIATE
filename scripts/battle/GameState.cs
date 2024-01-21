using System;
using System.Collections.Generic;
using Godot;
using TeicsoftSpectacleCards.scripts.battle.card;
using TeicsoftSpectacleCards.scripts.XmlParsing;
using TeicsoftSpectacleCards.scripts.XmlParsing.models;

namespace TeicsoftSpectacleCards.scripts.battle;

public class GameState {
    public int Multiplier { get; set; }
    public int SpectaclePoints { get; set; }
    public int PlayerMaxHealth { get; set; }
    public int PlayerHealth { get; set; }
    private int DefenseLower { get; set; }
    private int DefenseUpper { get; set; }

    public List<Card> ComboStack { get; set; }

    // changed this back to Card objects, as we use spectacle points in the combo processing. Easier than tracking separately.

    private List<ComboModel> AllCombos { get; set; }

    public List<Enemy> enemies = new();
    private int selectedEnemyIndex = -1;
    public Hand hand;
    public Deck deck;

    // Constructor
    public GameState() {
        AllCombos = ComboXmlParser.ParseAllCombos(); // Retrieve a list of all combos as model objects

        ComboStack = new List<Card>();
        Multiplier = 1; // 1 is lowest possible value
        SpectaclePoints = 0;
        PlayerMaxHealth = 100; // adjust this as needed, or base on some other check
        DefenseLower = 0;
        DefenseUpper = 0;
        PlayerHealth = PlayerMaxHealth;
    }
    // Player Health Methods
// ****
    public void DamagePlayer(int damage, PositionEnum position = PositionEnum.Upper) {
        bool blocked = false;
        switch (position) {
            case PositionEnum.Upper:
                if (DefenseUpper > 0) {
                    blocked = true;
                    DefenseUpper--;
                }

                break;
            case PositionEnum.Lower:
                if (DefenseLower > 0) {
                    blocked = true;
                    DefenseLower--;
                }

                break;
        }

        if (!blocked) { DirectDamagePlayer(damage); }
    }

    private void DirectDamagePlayer(int damage) {
        PlayerHealth = Math.Max(0, PlayerHealth - damage);
        if (PlayerHealth == 0) { EndRound(); }
    }
    // ****

    public void HealPlayer(int amount) {
        PlayerHealth = Math.Min(PlayerMaxHealth, PlayerHealth + Math.Abs(amount));
        if (PlayerHealth == 0) { EndRound(); }
    }

    // Combo Methods
    // ****
    public void PushCardStack(Card card) {
        ComboStack.Add(card);
    }
    public void ComboCheck(Card card) { // largely based on Cath's python code
        PushCardStack(card);

        // find a matching combo if it exists, returns null if no match
        ComboModel matchingCombo = ComboCompare();
        if (matchingCombo != null) { ProcessCombo(matchingCombo); }
    }

    private void ProcessCombo(ComboModel matchingCombo) {
        ProcessMultiplier(matchingCombo?.CardList.Count ?? 0);
        ProcessSpectaclePoints(matchingCombo?.SpectaclePoints ?? 0);

        // Combo().Play(); Like Card.Play, do all the gameplay stuff here.
        ComboStack.Clear();
    }

    public void EndRound() {
        ProcessCombo(null);
    }

    // Check for combo matches
    public ComboModel ComboCompare() {
        foreach (ComboModel combo in AllCombos) {
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
        CardSleeve cardSleeve = hand.GetSelectedCard();
        if (cardSleeve != null && !(cardSleeve.Card.TargetRequired && GetSelectedEnemy() == null)) {
            cardSleeve.Card.Play(this);
            hand.Discard();
        }
    }

    public void Draw(int n = 1) {
        hand.AddCards(deck.DrawCards(n));
    }
    // ****


    // Enemy methods
    // ****

    public Enemy GetSelectedEnemy() {
        return selectedEnemyIndex != -1 ? enemies[selectedEnemyIndex] : null;
    }

    public void SelectEnemy(Enemy enemy) {
        int enemyIndex = enemies.IndexOf(enemy);
        selectedEnemyIndex = selectedEnemyIndex != enemyIndex ? enemyIndex : -1;
    }
    // ****

    public override string ToString() {
        return
            $"ComboMultiplier: {Multiplier}," +
            $"SpectaclePoints: {SpectaclePoints}," +
            $"MaxPlayerHealth: {PlayerMaxHealth}," +
            $"PlayerHealth: {PlayerHealth}," +
            $"ComboStack: {ComboStack}," +
            $"AllCombos: {AllCombos}";
    }

    public enum PositionEnum {
        Upper,
        Lower,
        None
    }
}
