using Godot;
using System;
using TeicsoftSpectacleCards.scripts.autoloads;

public partial class MainMenu : Control
{

    private void _on_start_game_button_pressed()
    {
        var sceneLoader = GetNode<SceneLoader>("/root/scene_loader");
        sceneLoader.GoToScene("res://scenes/battle/Battle.tscn");
    }
        
    private void _on_dialogue_button_pressed()
    {
        var sceneLoader = GetNode<SceneLoader>("/root/scene_loader");
        sceneLoader.GoToScene("res://scenes/main/dialogue_display.tscn");
    }


    private void _on_settings_button_pressed()
    {
        // Replace with function body.
    }


    private void _on_exit_button_pressed()
    {
        GetTree().Quit();
    }
}






