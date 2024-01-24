using Godot;
using System;
using TeicsoftSpectacleCards.scripts.autoloads;

public partial class MainMenu : Control
{
    private TeicsoftSpectacleCards.scripts.audio.AudioEngine audioEngine;
    
    private void _on_ready()
    {
        audioEngine = GetNode<TeicsoftSpectacleCards.scripts.audio.AudioEngine>("/root/audio_engine");
        
        StartMusic();
    }
    
    private void _on_start_game_button_pressed()
    {
        var sceneLoader = GetNode<SceneLoader>("/root/scene_loader");
        sceneLoader.GoToScene("res://scenes/battle/Battle.tscn");
        audioEngine.StopAllTracks();
    }
        
    private void _on_dialogue_button_pressed()
    {
        var sceneLoader = GetNode<SceneLoader>("/root/scene_loader");
        sceneLoader.GoToScene("res://scenes/main/dialogue_display.tscn");
    }


    private void _on_settings_button_pressed()
    {
        // Replace with function body.
        audioEngine.PlayMusic("Shop_loop_audio.wav"); //for testing audio engine, remove when setting main menu functionality
    }


    private void _on_exit_button_pressed()
    {
        GetTree().Quit();
    }
    
    private void StartMusic()
    {
        audioEngine.PlayMusic("venividivichy.wav");

    }
}
