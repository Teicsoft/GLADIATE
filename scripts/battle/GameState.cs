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

    //Constructor
    public GameState()
    {
        this.Combos = ParseAllCombos();

        CardStack = new List<string>();

        ComboCompare();
    }

    
    //Add card ID to "Stack"
    public void PushCardStack(string id)
    {
        this.CardStack.Add(id);
    }

    public void CleanCardStack()
    {
        this.CardStack = new List<string>();
    }


    public bool ComboCompare()
    {

        foreach (ComboModel Combo in Combos)
        {
            int i = Combo.CardList.Count -1;

            bool match = true;
            while (i>=0)
            {
                if (CardStack[-i] != Combo.CardList[-i].Id)
                {
                    match = false;
                    break;
                }
                i--;
            }
            
            if (match)
            {
                return true;
            }
        }

        return false;
    }
    
    
    //Retrieve a list of all combos as model objects
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