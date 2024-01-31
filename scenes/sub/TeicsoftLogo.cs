using Godot;
using TeicsoftSpectacleCards.scripts.autoloads;

namespace TeicsoftSpectacleCards.scenes.sub;

public partial class TeicsoftLogo : Control
{
    private void _on_ts_timer_timeout()
    {
        var sceneLoader = GetNode<SceneLoader>("/root/scene_loader");
        sceneLoader.GoToScene("res://scenes/sub/TeicogLogo.tscn");
    }

}
