using Godot;

namespace TeicsoftSpectacleCards.scripts.autoloads;

public partial class SceneLoader : Node
{
    public Node CurrentScene { get; set; }

    public override void _Ready()
    {
        Viewport root = GetTree().Root;
        CurrentScene = root.GetChild(root.GetChildCount() - 1);
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

}
