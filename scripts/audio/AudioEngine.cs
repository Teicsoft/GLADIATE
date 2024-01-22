using Godot;
using System;
using TeicsoftSpectacleCards.scripts.customresource;

public partial class AudioEngine : Node
{
    AudioStreamPlayer MusicPlayer;
    AudioStreamPlayer SoundPlayer;
    
    public AudioStream CurrentMusicStream { get; set; }
    public AudioStream QueuedMusicStream { get; set; }
    
    public AudioStream CurrentSoundEffectStream { get; set; }
    public AudioStream QueuedSoundEffectStream { get; set; }
    
    string musicFolderName = "audio/music/";

    
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        MusicPlayer = GetNode<AudioStreamPlayer>("MusicPlayer");
        SoundPlayer = GetNode<AudioStreamPlayer>("SoundFxPlayer");
    }

    public void PlayMusic(string musicFileName)
    {
        string localPath = ResourceGrabber.GetAssetPath(musicFileName, musicFolderName);
        AudioStream audioStream = (AudioStream)ResourceLoader.Load(localPath);
        MusicPlayer.Stream = audioStream;
        
        MusicPlayer.Play();
    }
    
    

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
        
    }
}
