using Godot;
using System;
using GLADIATE.scripts.autoloads;

public partial class GameOver : Control
{
    private void OnTimerTimeout()
    {
        var sceneLoader = GetNode<GLADIATE.scripts.autoloads.SceneLoader>("/root/SceneLoader");
        sceneLoader.GoToScene("res://scenes/title_screen.tscn");
        sceneLoader.i = 0;
        sceneLoader.SpectaclePoints = 0;
        sceneLoader.Health = 0;
    }
}

