using System.Collections.Generic;
using Godot;
using TeicsoftSpectacleCards.scripts.XmlParsing;

namespace TeicsoftSpectacleCards.scripts.autoloads;

public partial class SceneLoader : Node
{
    public Node CurrentScene { get; set; }
    public List<Dictionary<string, dynamic>> battles;
    private int _i = 1;

    public override void _Ready()
    {
        Viewport root = GetTree().Root;
        CurrentScene = root.GetChild(root.GetChildCount() - 1);
        battles = BattleXmlParser.ParseAllBattles();
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
        if (_i < battles.Count)
        {
            DeferredGotoScene("res://scenes/battle/Battle.tscn");
            _i++;
        }
        else
        {
            DeferredGotoScene("res://scenes/main/Credits.tscn");
        }
    }
    
    public Dictionary<string, dynamic> getCurrentBattleData() { return battles[_i]; }
}
