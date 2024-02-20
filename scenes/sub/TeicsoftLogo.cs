using Godot;
using GLADIATE.scripts.autoloads;

namespace GLADIATE.scenes.sub;

public partial class TeicsoftLogo : Control
{
    private void _OnTsTimerTimeout()
    {
        var sceneLoader = GetNode<scripts.autoloads.SceneLoader>("/root/SceneLoader");
        sceneLoader.GoToScene("res://scenes/sub/TeicogLogo.tscn");
    }
}
