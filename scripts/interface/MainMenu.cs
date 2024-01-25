using Godot;
using System.Collections.Generic;
using TeicsoftSpectacleCards.scripts.audio;
using TeicsoftSpectacleCards.scripts.autoloads;

public partial class MainMenu : Control
{
    private AudioEngine audioEngine;

    // this dictionary is used to declare what audio is used by this scene, so it can be preloaded in AudioEngine
    Dictionary<string, AudioEngine.AudioType> AudioDeclaration = new Dictionary<string, AudioEngine.AudioType>()
    {
        {"Shop_loop_audio.wav", AudioEngine.AudioType.Music},
        {"venividivichy.wav", AudioEngine.AudioType.Music},
        {"testsound1.ogg", AudioEngine.AudioType.SoundFx}
    };
    
    private void _on_ready()
    {
        audioEngine = GetNode<AudioEngine>("/root/audio_engine");
        audioEngine.PreloadAudio(AudioDeclaration);
        StartMusic();
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
    
    private void StartMusic()
    {
        audioEngine.PlayMusic("venividivichy.wav");

    }
}
