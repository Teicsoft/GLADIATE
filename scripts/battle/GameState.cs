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

    public static List<string> CardStack { get; set; } 
    
    
    private List<ComboModel> Combos { get; set; }


    public GameState()
    {
        this.Combos = ParseAllCombos();
        GD.Print(Combos);
    }

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