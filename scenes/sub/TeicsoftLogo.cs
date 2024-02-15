using Godot;
using TeicsoftSpectacleCards.scripts.autoloads;

namespace TeicsoftSpectacleCards.scenes.sub;

public partial class TeicsoftLogo : Control
{
    private void _OnTsTimerTimeout()
    {
        var sceneLoader = GetNode<scripts.autoloads.SceneLoader>("/root/scene_loader");
        sceneLoader.GoToScene("res://scenes/sub/TeicogLogo.tscn");
    }
}
