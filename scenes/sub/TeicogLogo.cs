using Godot;
using GLADIATE.scripts.autoloads;

namespace GLADIATE.scenes.sub;

public partial class TeicogLogo : Control
{
    private void _OnTcTimerTimeout()
    {
        var sceneLoader = GetNode<scripts.autoloads.SceneLoader>("/root/scene_loader");
        sceneLoader.GoToScene("res://scenes/title_screen.tscn");
    }
}
