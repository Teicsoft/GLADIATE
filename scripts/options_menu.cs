using Godot;
using System;
using GLADIATE.scripts.audio;
using GLADIATE.scripts.autoloads;

public partial class options_menu : Control
{
    
    SceneLoader _sceneLoader;
    AudioEngine _audioEngine;


    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        _sceneLoader = GetNode<SceneLoader>("/root/SceneLoader");
        _audioEngine = GetNode<AudioEngine>("/root/audio_engine");
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
        Vector2 originalViewportSize = new Vector2(1920, 1080);
        Vector2 currentViewportSize = GetViewport().GetVisibleRect().Size;
        
        Vector2 scaleFactor = GetViewport().GetVisibleRect().Size / originalViewportSize;
        Vector2 offsetFactor = originalViewportSize - currentViewportSize;

        Scale = scaleFactor;
    }
    
    private void _on_texture_button_pressed()
    {
        OS.ShellOpen("https://github.com/Teicsoft/GLADIATE");
    }
    
    private void _on_credits_button_pressed()
    {
        _sceneLoader.GoToScene("res://scenes/main/Credits.tscn");
        _audioEngine.PlayMusic("Shop_loop_audio.wav");
    }
    
    private void _on_options_button_pressed()
    {
        Show();
    }
    
    private void _on_back_button_pressed()
    {
        Hide();
    }   
}
