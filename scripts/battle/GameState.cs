using System;
using System.Collections.Generic;
using TeicsoftSpectacleCards.scripts.XmlParsing;
using TeicsoftSpectacleCards.scripts.XmlParsing.models;

namespace TeicsoftSpectacleCards.scripts.battle;

public class GameState
{
    public int ComboMultiplier { get; set; }
    public int SpectaclePoints { get; set; }
    public int MaxPlayerHealth { get; set; }
    public int PlayerHealth { get; set; }
    private int DefenseLower { get; set; }
    private int DefenseUpper { get; set; }
    public List<Card> CardStack { get; set; } // changed this back to Card objects, as we use spectacle points in the combo processing. Easier than tracking separately.
    private List<ComboModel> AllCombos { get; set; }

    // Constructor
    public GameState()
    {
        AllCombos = ComboXmlParser.ParseAllCombos(); // Retrieve a list of all combos as model objects

        CardStack = new List<Card>();
        ComboMultiplier = 1; // 1 is lowest possible value
        SpectaclePoints = 0;
        MaxPlayerHealth = 100; // adjust this as needed, or base on some other check
        DefenseLower = 0;
        DefenseUpper = 0;
        PlayerHealth = MaxPlayerHealth;
        
    }
    
    // Stack Methods
    public void PushCardStack(Card card)
    {
        CardStack.Add(card);
    }

    public void CleanCardStack()
    {
        CardStack = new List<Card>();

    }
    
    // Player Health Methods
    public bool IsPlayerAlive()
    {
        return PlayerHealth > 0;
    }
    
    public void Damage(int damage, PositionEnum position = PositionEnum.Upper) {
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

        if (!blocked) { DirectDamage(damage); }
    }
    
    private void DirectDamage(int damage) {
        PlayerHealth = Math.Max(0, PlayerHealth - damage);
        if (PlayerHealth == 0) { EndRound(); } else {
            float healthRatio = (float)PlayerHealth / MaxPlayerHealth;
        }
    }
    
    public void HealPlayer(int amount)
    {
        PlayerHealth += Math.Abs(amount);

        if (PlayerHealth > MaxPlayerHealth)
        {
            PlayerHealth = MaxPlayerHealth;
        }
        
        if (!IsPlayerAlive())
        {
            //end round on loss
        }
    }

    // Combo Methods

    public void GamestateCardPlayed(Card card) //largely based on Cath's python code
    {
        PushCardStack(card);
        
        // find a matching combo if it exists, returns null if no match
        ComboModel matchingCombo = ComboCompare();
        
        
        if (matchingCombo != null)
        {
            ProcessCombo(matchingCombo.CardList.Count);
            SpectaclePointsGain(matchingCombo);
            CleanCardStack();
        }
    }
    
    public void EndRound()
    {
        ProcessCombo(0);
        CleanCardStack();
    }
    
    // Check for combo matches
    public ComboModel ComboCompare()
    {
        foreach (ComboModel combo in AllCombos)
        {
            int count = combo.CardList.Count;
            if (CardStack.Count < count) { continue; }
            
            bool match = true;
            for (int i = 0; i < count; i++)
            {
                if (CardStack[CardStack.Count -1 -i].id != combo.CardList[count-1-i].id)
                {
                    match = false;
                    break;
                }
            }
            
            if (match) { return combo; }
        }
        
        return null;
    }
    
    public void ProcessCombo(int number) // overloaded to allow int input for end round
    {
        //calculate combo multiplier adjustment
        
        int comboCount = number;
        int comboValue = (int)Math.Floor(Math.Pow(2, comboCount-1));
        int blunderCount = CardStack.Count - comboCount;
        int blunderValue = (int)Math.Floor(Math.Pow(2, blunderCount-1));
        
        int multiplierAdjustment = comboValue - blunderValue;
        
        //adjust combo multiplier
        ComboMultiplier += multiplierAdjustment;
        if (ComboMultiplier < 1) { ComboMultiplier = 1; }
        
        //resolve other combo effects
    }

    public int SpectaclePointsGain(ComboModel matchingCombo)
    {
        int roundSpectaclePoint = 0;
        
        //calculate round spectacle points
        foreach (Card card in CardStack)
        {
            roundSpectaclePoint += card.spectaclePoints;
        }
        
        roundSpectaclePoint += matchingCombo.SpectaclePoints;
        
        // should be redundant based on design of multiplier,
        // but just in case uses absolute value of turnSpectaclePoints to prevent negative values
        return SpectaclePoints += Math.Abs(roundSpectaclePoint * ComboMultiplier);
    }
    
    public override string ToString()
    {
        return $"ComboMultiplier: {ComboMultiplier}, SpectaclePoints: {SpectaclePoints}, MaxPlayerHealth: {MaxPlayerHealth}, PlayerHealth: {PlayerHealth}, CardStack: {CardStack}, AllCombos: {AllCombos}";
    }
    
    
    public enum PositionEnum {
        Upper,
        Lower,
        None
    }
}