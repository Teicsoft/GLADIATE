using System;
using System.Collections.Generic;
using Godot;
using TeicsoftSpectacleCards.scripts.battle.card;
using TeicsoftSpectacleCards.scripts.XmlParsing;

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

    private List<Combo> AllCombos { get; set; }

    public List<Enemy> Enemies = new();
    private int SelectedEnemyIndex = -1;
    public Hand Hand;
    public Deck Deck;

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

    // Player Methods
    // ****
    public void DamagePlayer(int damage, Utils.PositionEnum position = Utils.PositionEnum.Upper) {
        bool blocked = false;
        switch (position) {
            case Utils.PositionEnum.Upper:
                if (DefenseUpper > 0) {
                    blocked = true;
                    DefenseUpper--;
                }

                break;
            case Utils.PositionEnum.Lower:
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

    public void HealPlayer(int amount) {
        PlayerHealth = Math.Min(PlayerMaxHealth, PlayerHealth + Math.Abs(amount));
        if (PlayerHealth == 0) { EndRound(); }
    }

    public void StunPlayer(int stun, Utils.PositionEnum position = Utils.PositionEnum.Upper) {
        bool blocked = false;
        if ((DefenseUpper > 0) || (DefenseLower > 0)) {
            blocked = true;
            DefenseUpper = 0;
            DefenseLower = 0;
        }
    }

    public void ModifyPlayerBlock(int change, Utils.PositionEnum position) {
        switch (position) {
            case Utils.PositionEnum.Upper:
                DefenseUpper += change;
                break;
            case Utils.PositionEnum.Lower:
                DefenseLower += change;
                break;
        }
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
        if (matchingCombo != null) { ProcessCombo(matchingCombo); }
    }

    private void ProcessCombo(Combo matchingCombo) {
        matchingCombo?.Play(this);
        ProcessMultiplier(matchingCombo?.CardList.Count ?? 0);
        ProcessSpectaclePoints(matchingCombo?.SpectaclePoints ?? 0);
        // Combo().Play(); Like Card.Play, do all the gameplay stuff here.
        ComboStack.Clear();
    }

    public void EndRound() {
        ProcessCombo(null);
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
            cardSleeve.Card.Play(this);
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
        return SelectedEnemyIndex != -1 ? Enemies[SelectedEnemyIndex] : null;
    }

    public void SelectEnemy(Enemy enemy) {
        int enemyIndex = Enemies.IndexOf(enemy);
        SelectedEnemyIndex = SelectedEnemyIndex != enemyIndex ? enemyIndex : -1;
    }

    // ****

    public override string ToString() {
        return $"ComboMultiplier: {Multiplier}," + $"SpectaclePoints: {SpectaclePoints}," +
               $"MaxPlayerHealth: {PlayerMaxHealth}," + $"PlayerHealth: {PlayerHealth}," +
               $"ComboStack: {ComboStack}," + $"AllCombos: {AllCombos}";
    }
}
