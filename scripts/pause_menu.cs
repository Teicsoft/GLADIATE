using Godot;
using System;
using GLADIATE.scripts.audio;
using GLADIATE.scripts.autoloads;

public partial class pause_menu : Control
{

    SceneLoader _sceneLoader;
    AudioEngine _audioEngine;
    
    public override void _Ready()
    {
        _sceneLoader = GetNode<SceneLoader>("/root/SceneLoader");
        _audioEngine = GetNode<AudioEngine>("/root/audio_engine");
    }
    
    private void _on_quit_button_pressed()
    {
        GetTree().Paused = false;

        
        _sceneLoader.i = 0;
        _sceneLoader.Health = 0;
        _sceneLoader.SpectaclePoints = 0;
        
        _sceneLoader.GoToScene("res://scenes/title_screen.tscn");
    }
    
    public override void _UnhandledInput(InputEvent @event)
    {
        if (@event is InputEventKey eventKey)       
            if (eventKey.IsReleased() && eventKey.Keycode == Key.Escape)
            {
                if (!GetTree().Paused){
                    Show();
                    GetTree().Paused = true;
                }
                else {
                    Hide();
                    GetTree().Paused = false;
                }
            }
    }
    
    private void _on_resume_button_pressed()
    {
        Hide();
        GetTree().Paused = false;
    }

}
