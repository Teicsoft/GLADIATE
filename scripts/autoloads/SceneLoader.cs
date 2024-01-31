using System.Collections.Generic;
using Godot;
using TeicsoftSpectacleCards.scripts.audio;
using TeicsoftSpectacleCards.scripts.XmlParsing;

namespace TeicsoftSpectacleCards.scripts.autoloads;

public partial class SceneLoader : Node
{
    private AudioEngine audioEngine;
    public Node CurrentScene { get; set; }
    public List<Dictionary<string, dynamic>> battles;
    public int i {get; set;}
    public int SpectaclePoints { get; set; }
    public string deckSelected { get; set; }
    public int health { get; set; }

    public override void _Ready()
    {
        audioEngine = GetNode<AudioEngine>("/root/audio_engine");

        i = 0;
        
        Viewport root = GetTree().Root;
        CurrentScene = root.GetChild(root.GetChildCount() - 1);
        battles = BattleXmlParser.ParseAllBattles();
        deckSelected = "deck_Player1";
        SpectaclePoints = 0;
    }
    
    public void GoToScene(string path)
    {
        CallDeferred(MethodName.DeferredGotoScene, path);
    }
    
    public void DeferredGotoScene(string path)
    {
        CurrentScene.Free();
        var nextScene = (PackedScene) GD.Load<PackedScene>(path);
        CurrentScene = nextScene.Instantiate();
        GetTree().Root.AddChild(CurrentScene);
        GetTree().CurrentScene = CurrentScene;
    }

    public void GoToNextBattle()
    { 
        if (i < battles.Count)
        {
            GD.Print("GotoNextBattle");
            CallDeferred("DeferredGotoScene", "res://scenes/battle/Battle.tscn");
        }
        else
        {
            GD.Print("GotoVictory");
            CallDeferred("DeferredGotoScene", "res://scenes/sub/Victory.tscn");
            audioEngine.PlayMusic("fuck_around_and_find_out_2_electric_boogaloo.mp3");
        }
    }
    
    
    public Dictionary<string, dynamic> getCurrentBattleData() { return battles[i]; }
}
