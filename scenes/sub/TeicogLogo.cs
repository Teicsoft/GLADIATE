using Godot;
using TeicsoftSpectacleCards.scripts.autoloads;

namespace TeicsoftSpectacleCards.scenes.sub;

public partial class TeicogLogo : Control
{
    private void OnTcTimerTimeout()
    {
        var sceneLoader = GetNode<SceneLoader>("/root/scene_loader");
        sceneLoader.GoToScene("res://scenes/title_screen.tscn");
    }

}
