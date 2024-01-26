using Godot;
using System;
using System.Collections.Generic;
using TeicsoftSpectacleCards.scripts.audio;
using TeicsoftSpectacleCards.scripts.autoloads;


public partial class TitleScreen : Control
{
    private AudioEngine audioEngine;
    // this dictionary is used to declare what audio is used by this scene, so it can be preloaded in AudioEngine
    Dictionary<string, AudioEngine.AudioType> AudioDeclaration = new Dictionary<string, AudioEngine.AudioType>()
    {
        {"Shop_loop_audio.wav", AudioEngine.AudioType.Music},
        {"venividivichy.wav", AudioEngine.AudioType.Music},
        {"testsound1.ogg", AudioEngine.AudioType.SoundFx}
    };
    
    
    public override void _Ready()
    {
        audioEngine = GetNode<AudioEngine>("/root/audio_engine");
        audioEngine.PreloadAudio(AudioDeclaration);
        AnimatedSprite2D goon = GetNode<AnimatedSprite2D>("Container/Goon");
        goon.Animation = "Idle";
        goon.Play();
    }
    
    private void _on_timer_timeout()
    {
        var sceneLoader = GetNode<SceneLoader>("/root/scene_loader");
        sceneLoader.GoToScene("res://scenes/main/main_menu.tscn");
    }
    
    private void _on_start_music_timeout()
    {
        audioEngine.PlayMusic("venividivichy.wav");
    }
}
