using Godot;
using System;
using TeicsoftSpectacleCards.scripts.autoloads;

public partial class GameOver : Control
{
    private void OnTimerTimeout()
    {
        var sceneLoader = GetNode<SceneLoader>("/root/scene_loader");
        sceneLoader.GoToScene("res://scenes/title_screen.tscn");
    }
}
