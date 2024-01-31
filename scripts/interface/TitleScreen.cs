using Godot;
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
        {"Menu_music.wav", AudioEngine.AudioType.Music},
        {"venividivichy.wav", AudioEngine.AudioType.Music},
        {"whatdidtheromanseverdoforme.wav", AudioEngine.AudioType.Music},
        {"testsound1.ogg", AudioEngine.AudioType.SoundFx}
    };
    
    
    public override void _Ready()
    {
        audioEngine = GetNode<AudioEngine>("/root/audio_engine");
        audioEngine.PreloadAudio(AudioDeclaration);
        
        this.Set("mouse_filter", 0);
    }
    
    private void OnStartMusicTimeout()
    {
        audioEngine.PlayMusic("Menu_music.wav");
    }

    private void OnInstructionTimerTimeout()
    {
        Label label = (Label)GetNode("Label");
        if (label.Visible) { label.Hide(); }
        else { label.Show(); }
    }

    private void OnGuiInput(InputEvent @event)
    {
        if (@event is InputEventMouseButton mouseButton)
        {
            if (mouseButton.Pressed)
            {
                var sceneLoader = GetNode<SceneLoader>("/root/scene_loader");
                sceneLoader.GoToScene("res://scenes/Lore.tscn");
                audioEngine.PlayMusic("whatdidtheromanseverdoforme.wav");
            }
        }
    }
}
