using Godot;

namespace GLADIATE.scenes.sub;

public partial class TeicogLogo : Control
{
    private void _OnTcTimerTimeout()
    {
        var sceneLoader = GetNode<scripts.autoloads.SceneLoader>("/root/SceneLoader");
        sceneLoader.GoToScene("res://scenes/title_screen.tscn");
    }
}
