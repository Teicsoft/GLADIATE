using Godot;
using System.Collections.Generic;
using GLADIATE.scripts.audio;
using GLADIATE.scripts.autoloads;


public partial class TitleScreen : Control
{
    private AudioEngine audioEngine;

    // this dictionary is used to declare what audio is used by this scene, so it can be preloaded in AudioEngine
    Dictionary<string, AudioEngine.AudioType> AudioDeclaration = new Dictionary<string, AudioEngine.AudioType>()
    {
        { "Shop_loop_audio.wav", AudioEngine.AudioType.Music },
        { "Menu_music.wav", AudioEngine.AudioType.Music },
        { "venividivichy.wav", AudioEngine.AudioType.Music },
        { "whatdidtheromanseverdoforme.wav", AudioEngine.AudioType.Music },
        { "Lil_tune.wav", AudioEngine.AudioType.Music },
        { "fuckaroundandfindout.wav", AudioEngine.AudioType.Music },
        { "fuck_around_and_find_out_2_electric_boogaloo.mp3", AudioEngine.AudioType.Music },
        { "testsound1.ogg", AudioEngine.AudioType.SoundFx },
        { "victory-jingle.wav", AudioEngine.AudioType.SoundFx },
        { "menu-accept.wav", AudioEngine.AudioType.SoundFx },
        { "drawn-card.ogg", AudioEngine.AudioType.SoundFx },
        { "male-hurt-1.ogg", AudioEngine.AudioType.SoundFx },
        { "male-hurt-2.ogg", AudioEngine.AudioType.SoundFx },
        { "male-hurt-3.ogg", AudioEngine.AudioType.SoundFx },
        { "male-hurt-4.ogg", AudioEngine.AudioType.SoundFx }
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
        if (label.Visible)
        {
            label.Hide();
        }
        else
        {
            label.Show();
        }
    }

    private void _on_start_button_pressed()
    {
        audioEngine.PlaySoundFx("menu-accept.wav");
        var sceneLoader = GetNode<SceneLoader>("/root/SceneLoader");
        sceneLoader.GoToScene("res://scenes/Lore.tscn");
        audioEngine.PlayMusic("whatdidtheromanseverdoforme.wav");
    }
    
    private void _on_quit_button_pressed()
    {
        GetTree().Quit();
    }
}
