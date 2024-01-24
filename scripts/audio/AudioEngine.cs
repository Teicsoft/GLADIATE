using System.Threading.Tasks;
using Godot;
using TeicsoftSpectacleCards.scripts.customresource;

namespace TeicsoftSpectacleCards.scripts.audio;

public partial class AudioEngine : Node
{
    // two channels for music, to allow for crossfading
    private AudioStreamPlayer _musicPlayer1;
    private AudioStreamPlayer _musicPlayer2;
    
    // two channels for sound effects, to allow for multiple sounds at once
    private AudioStreamPlayer _soundPlayer1;
    private AudioStreamPlayer _soundPlayer2;

    // one channel for voice acting as there should only be one voice line at a time for clarity
    private AudioStreamPlayer _voicePlayer;


    private const string MusicFolderName = "audio/music/";

    
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        _musicPlayer1 = GetNode<AudioStreamPlayer>("MusicPlayer1");
        _musicPlayer2 = GetNode<AudioStreamPlayer>("MusicPlayer2");
        _soundPlayer1 = GetNode<AudioStreamPlayer>("SoundFxPlayer1");
        _soundPlayer2 = GetNode<AudioStreamPlayer>("SoundFxPlayer2");
        _voicePlayer = GetNode<AudioStreamPlayer>("VoicePlayer");
    }

    public void PlayMusic(string musicFileName)
    {
        string localPath = ResourceGrabber.GetAssetPath(musicFileName, MusicFolderName);
        AudioStream audioStream = (AudioStream)ResourceLoader.Load(localPath);
        
        if (!_musicPlayer1.Playing && !_musicPlayer2.Playing) // if nothing is playing, play on channel 1
        {
            _musicPlayer1.Stream = audioStream;
            _musicPlayer1.Play();
        }
        
        else if (!_musicPlayer1.Playing) // if channel 1 is not playing, but 2 is, cross fade to channel 1
        {
            _musicPlayer1.Stream = audioStream;
            _= FadeInMusic(_musicPlayer1); // assigning Task to discard to make rider happy, as I'm not awaiting async
            if (_musicPlayer2.Playing) { _ = FadeOutTrack(_musicPlayer2); }
            
        }
        else if (!_musicPlayer2.Playing) // if channel 1 is playing, but not 2, cross fade to channel 2
        {
            _musicPlayer2.Stream = audioStream;
            _= FadeInMusic(_musicPlayer2);
            _= FadeOutTrack(_musicPlayer1);
        }
        else // if both channels are playing, just stop all music and play on channel 1
        {
            _musicPlayer1.Stop();
            _musicPlayer2.Stop();
            _musicPlayer1.Stream = audioStream;
            _musicPlayer1.Play();
        }
    }

    private async Task FadeOutTrack(AudioStreamPlayer player) // -80 is the lowest volume, 0 is the highest
    {
        float vol = player.VolumeDb;
        while (vol > -80)
        {
            vol -= 0.1f;
            player.VolumeDb = vol;

            await Task.Delay(30);
            if (vol <= -80)
            {
                player.Stop();
            }
        }
    }
    
    public async Task FadeAllTracks()
    {
        await FadeOutTrack(_musicPlayer1);
        await FadeOutTrack(_musicPlayer2);
    }

    private async Task FadeInMusic(AudioStreamPlayer player)
    {
        float vol = player.VolumeDb;
        vol -= 80f;
        player.VolumeDb = vol;
        player.Play();
        
        while (vol < 0)
        {
            vol += 1f;
            player.VolumeDb = vol;
            await Task.Delay(30);
            if (vol >= 0)
            {
                break;
            }
        }
    }
    
    public void StopMusic()
    {
        if (_musicPlayer1.Playing) { _musicPlayer1.Stop(); }
        if (_musicPlayer2.Playing) { _musicPlayer2.Stop(); }
    }
}