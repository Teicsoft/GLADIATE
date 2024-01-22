using Godot;
using System;

public partial class AudioEngine : Node
{
    Node MusicPlayer;
    Node SoundPlayer;
    
    public AudioStreamPlayer CurrentMusicStream { get; set; }
    public AudioStreamPlayer QueuedMusicStream { get; set; }
    
    public AudioStreamPlayer CurrentSoundEffectStream { get; set; }
    public AudioStreamPlayer QueuedSoundEffectStream { get; set; }
    
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        MusicPlayer = GetNode("MusicPlayer");
        SoundPlayer = GetNode("SoundEffects");
    }

    public void PlayMusic()
    {
        MusicPlayer.GetNode<AudioStreamPlayer>("MainMenu").Play();
    }
    
    

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
    }
}
