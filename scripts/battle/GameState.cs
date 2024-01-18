using System.Collections.Generic;
using Godot;
using Godot.NativeInterop;
using TeicsoftSpectacleCards.scripts.XmlParsing;
using TeicsoftSpectacleCards.scripts.XmlParsing.models;

namespace TeicsoftSpectacleCards.scripts.battle;

public class GameState
{
    public int ComboMultiplier { get; set; }
    public int SpectaclePoints { get; set; }
    public int MaxPlayerHealth { get; set; }
    public int PlayerHealth { get; set; }

    public List<string> CardStack { get; set; } 
    private List<ComboModel> Combos { get; set; }

    // Constructor
    public GameState()
    {
        this.Combos = ParseAllCombos();

        CardStack = new List<string>();

        ComboMultiplier = 1;
        ComboCompare();
        SpectaclePoints = 0;

        MaxPlayerHealth = 100; // adjust this as needed, or base on some other check
        PlayerHealth = MaxPlayerHealth;
    }
    
    // Stack Methods
    public void PushCardStack(string id)
    {
        this.CardStack.Add(id);
    }

    public void CleanCardStack()
    {
        this.CardStack = new List<string>();

    }
    
    public bool IsPlayerAlive()
    {
        return PlayerHealth > 0;
    }

    public void AdjustPlayerHealth(int amount)
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
    
    

    public void ComboCalulation(int comboAdjustmentAmount)
    {
        // Placeholder for combo math
        
        if (comboAdjustmentAmount <= 1)
        {
            comboAdjustmentAmount = 1;
        }
        
        this.ComboMultiplier += comboAdjustmentAmount;
    }

    public int BlunderCount(ComboModel comboModel, List<string> CardStack)
    {
        return CardStack.Count - comboModel.CardList.Count;
    }

    
    // Check for combo matches
    public ComboModel ComboCompare()
    {

        foreach (ComboModel combo in Combos)
        {
            int i = combo.CardList.Count -1;

            bool match = true;
            while (i>=0)
            {
                if (CardStack[-i] != combo.CardList[-i].Id)
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
    
    
    // Retrieve a list of all combos as model objects
    public List<ComboModel> ParseAllCombos()
    {
        string comboFilePath = "res://data/combos/";
        
        // using DirAccess dir = DirAccess.Open("res://data/combos/");
        string[] dir = DirAccess.GetFilesAt(comboFilePath);

        List<ComboModel> comboModels = new List<ComboModel>();
        foreach (string file in dir)
        {
            ComboModel combo =  ComboXmlParser.ParseComboFromXml(comboFilePath + file);
            comboModels.Add(combo);
        }

        return comboModels;
    }
}