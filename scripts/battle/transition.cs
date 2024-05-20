using Godot;
using System;
using GLADIATE.scripts.autoloads;

public partial class transition : ColorRect
{
    private SceneLoader _sceneLoader;
    
    
    public override void _Ready()
    {
        _sceneLoader = GetNode<SceneLoader>("/root/SceneLoader");
    }
    
    private void _on_continue_button_pressed()
    {
        _sceneLoader.GoToNextBattle();
    }
    

    private void _on_retreat_button_pressed()
    {
        _sceneLoader.i = 0;
        _sceneLoader.Health = 0;
        _sceneLoader.SpectaclePoints = 0;
        
        _sceneLoader.GoToScene("res://scenes/title_screen.tscn");
    }
}
