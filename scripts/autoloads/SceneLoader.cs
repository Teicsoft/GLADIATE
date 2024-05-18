using System.Collections.Generic;
using GLADIATE.scripts.audio;
using GLADIATE.scripts.XmlParsing;
using Godot;

namespace GLADIATE.scripts.autoloads;

public partial class SceneLoader : Godot.Node
{
    private AudioEngine _audioEngine;
    public Node CurrentScene { get; set; }
    public List<Dictionary<string, dynamic>> Battles;
    public int i { get; set; }
    public int SpectaclePoints { get; set; }
    public string DeckSelected { get; set; }
    public int Health { get; set; }
    public const string PreBossBattleId = "battle_6";
    public const string BossBattleId = "battle_7";


    public override void _Ready()
    {
        _audioEngine = GetNode<AudioEngine>("/root/audio_engine");

        i = 6;

        Viewport root = GetTree().Root;
        CurrentScene = root.GetChild(root.GetChildCount() - 1);
        Battles = BattleXmlParser.ParseAllBattles();
        DeckSelected = "deck_Player1";
        SpectaclePoints = 0;
    }

    public void GoToScene(string path)
    {
        CallDeferred(MethodName.DeferredGotoScene, path);
    }

    public void DeferredGotoScene(string path)
    {
        CurrentScene.Free();
        var nextScene = GD.Load<PackedScene>(path);
        CurrentScene = nextScene.Instantiate();
        GetTree().Root.AddChild(CurrentScene);
        GetTree().CurrentScene = CurrentScene;
    }

    public void GoToNextBattle()
    {
        if (i < Battles.Count)
        {
            GD.Print("GotoNextBattle");
            CallDeferred("DeferredGotoScene", "res://scenes/battle/Battle.tscn");
        }
        else
        {
            GD.Print("GotoVictory");
            CallDeferred("DeferredGotoScene", "res://scenes/sub/Victory.tscn");
            _audioEngine.PlayMusic("fuck_around_and_find_out_2_electric_boogaloo.mp3");
        }
    }


    public Dictionary<string, dynamic> GetCurrentBattleData()
    {
        return Battles[i];
    }
}
