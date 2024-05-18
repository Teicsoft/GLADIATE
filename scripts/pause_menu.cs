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
        _sceneLoader.i = 0;
        _sceneLoader.Health = 0;
        _sceneLoader.SpectaclePoints = 0;
        
        _sceneLoader.GoToScene("res://scenes/title_screen.tscn");
        
    }

}
