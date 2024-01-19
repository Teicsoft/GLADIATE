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
    public List<Card> CardStack { get; set; } // changed this back to Card objects, as we use spectacle points in the combo processing. Easier than tracking separately.
    private List<ComboModel> AllCombos { get; set; }

    // Constructor
    public GameState()
    {
        this.AllCombos = ComboXmlParser.ParseAllCombos(); // Retrieve a list of all combos as model objects

        CardStack = new List<Card>();

        ComboMultiplier = 1;
        ComboCompare();
        SpectaclePoints = 0;

        MaxPlayerHealth = 100; // adjust this as needed, or base on some other check
        PlayerHealth = MaxPlayerHealth;
    }
    
    // Stack Methods
    public void PushCardStack(Card card)
    {
        this.CardStack.Add(card);
    }

    public void CleanCardStack()
    {
        this.CardStack = new List<Card>();

    }
    
    // Player Health Methods
    public bool IsPlayerAlive()
    {
        return PlayerHealth > 0;
    }

    public void AdjustHealth(int amount)
    {
        PlayerHealth += amount;

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

    public void ProcessCombo() //largely based on Cath's python code
    {
        int roundSpectaclePoint = 0;
        
        // find a matching combo if it exists, returns null if no match
        ComboModel matchingCombo = ComboCompare();
        
        //calculate round spectacle points
        foreach (Card card in CardStack)
        {
            roundSpectaclePoint += card.spectaclePoints;
        }

        //calculate combo multiplier adjustment
        int comboCount = 0;
        if (matchingCombo != null)
        {
            roundSpectaclePoint += matchingCombo.SpectaclePoints;

            comboCount = matchingCombo.CardList.Count;
        }
        int comboValue = (int)Math.Floor(Math.Pow(2, comboCount-1));
        int blunderCount = CardStack.Count - comboCount;
        int blunderValue = (int)Math.Floor(Math.Pow(2, blunderCount-1));
        int multiplierAdjustment = comboValue - blunderValue;
        
        //adjust combo multiplier
        ComboMultiplier += multiplierAdjustment;
        if (ComboMultiplier < 1) { ComboMultiplier = 1; }

        if (matchingCombo != null)
        {
            SpectaclePointsGain(roundSpectaclePoint);
        }
        
        CleanCardStack();
        
        //resolve other combo effects
    }
    

    public int SpectaclePointsGain(int turnSpectaclePoints) 
    {
        // should be redundant based on design of multiplier,
        // but just in case uses absolute value of turnSpectaclePoints to prevent negative values
        return SpectaclePoints += Math.Abs(turnSpectaclePoints * ComboMultiplier);
    }

    
    // Check for combo matches
    public ComboModel ComboCompare()
    {
        foreach (ComboModel combo in AllCombos)
        {
            int i = combo.CardList.Count -1;

            bool match = true;
            while (i>=0)
            {
                if (CardStack[-i].id != combo.CardList[-i].id)
                {
                    match = false;
                    break;
                }
                i--;
            }
            if (match) { return combo; }
        }

        return null;
    }
}