using Godot;
using System.Collections.Generic;
using System.Threading.Tasks;
using TeicsoftSpectacleCards.scripts.audio;
using TeicsoftSpectacleCards.scripts.autoloads;

public partial class MainMenu : Control
{
    private AudioEngine audioEngine;

    
    private void _on_ready()
    {
        audioEngine = GetNode<AudioEngine>("/root/audio_engine");
        
        //this is ugly, but I'm just doing this for speed while designing. Will come back later and clean up
        AnimatedSprite2D goon = GetNode<AnimatedSprite2D>("GoonSquad/Goon");
        goon.Animation = "Idle";
        goon.Play();
        AnimatedSprite2D goon2 = GetNode<AnimatedSprite2D>("GoonSquad/Goon2");
        goon2.Animation = "Idle";
        goon2.Frame = 10;
        goon2.Play();
        AnimatedSprite2D remus = GetNode<AnimatedSprite2D>("BG/Remus");
        remus.Play();
        
        
    }
    
    private void _on_start_game_button_pressed()
    {
        var sceneLoader = GetNode<SceneLoader>("/root/scene_loader");
        sceneLoader.GoToScene("res://scenes/battle/Battle.tscn");
        audioEngine.StopAllTracks();
        audioEngine.DestroyPreloadedAudio();
    }
        
    private void _on_dialogue_button_pressed()
    {
        var sceneLoader = GetNode<SceneLoader>("/root/scene_loader");
        sceneLoader.GoToScene("res://scenes/main/dialogue_display.tscn");
        audioEngine.StopAllTracks();
        audioEngine.DestroyPreloadedAudio();

    }


    private void _on_settings_button_pressed()
    {
        // Replace with function body.
        // audioEngine.PlayMusic("Shop_loop_audio.wav"); //for testing audio engine, remove when setting main menu functionality
        audioEngine.PlaySoundFx("testsound1.ogg"); //for testing audio engine, remove when setting main menu functionality
    }


    private void _on_exit_button_pressed()
    {
        GetTree().Quit();
    }
    
}
