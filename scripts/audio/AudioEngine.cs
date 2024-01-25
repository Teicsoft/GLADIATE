using System.Collections.Generic;
using System.Threading.Tasks;
using Godot;
using TeicsoftSpectacleCards.scripts.customresource;

namespace TeicsoftSpectacleCards.scripts.audio;

public partial class AudioEngine : Node
{
    private Dictionary<string, AudioStream> preLoadedAudio;
    
    // two channels for music, to allow for cross fading
    private AudioStreamPlayer _musicPlayer1;
    private AudioStreamPlayer _musicPlayer2;
    
    // two channels for sound effects, to allow for multiple sounds at once
    private AudioStreamPlayer _soundFxPlayer1;
    private AudioStreamPlayer _soundFxPlayer2;

    // one channel for voice acting as there should only be one voice line at a time for clarity
    private AudioStreamPlayer _voiceLinePlayer;


    private const string MusicFolderName = "audio/music/";
    private const string SoundFxFolderName = "audio/sfx/";
    private const string VoiceLineFolderName = "audio/voice/";

    
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        _musicPlayer1 = GetNode<AudioStreamPlayer>("MusicPlayer1");
        _musicPlayer2 = GetNode<AudioStreamPlayer>("MusicPlayer2");
        _soundFxPlayer1 = GetNode<AudioStreamPlayer>("SoundFxPlayer1");
        _soundFxPlayer2 = GetNode<AudioStreamPlayer>("SoundFxPlayer2");
        _voiceLinePlayer = GetNode<AudioStreamPlayer>("VoicePlayer");
    }

    // Music Methods //
    public void PlayMusic(string musicFileName)
    {
        string localPath = ResourceGrabber.GetAssetPath(musicFileName, MusicFolderName);
        AudioStream audioStream = (AudioStream)ResourceLoader.Load(localPath);
        
        if (!_musicPlayer1.Playing && !_musicPlayer2.Playing) // if nothing is playing, play on channel 1
        {
            _musicPlayer1.Stream = audioStream;
            _musicPlayer1.VolumeDb = 0;
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
    
    public async Task FadeAllTracks()
    {
        await FadeOutTrack(_musicPlayer1);
        await FadeOutTrack(_musicPlayer2);
    }
    
    public void StopAllTracks()
    {
        if (_musicPlayer1.Playing) { _musicPlayer1.Stop(); }
        if (_musicPlayer2.Playing) { _musicPlayer2.Stop(); }
    }
    
    // Sound Effect Methods //

    public void PlaySoundFx(string soundFxFileName)
    {
        string localPath = ResourceGrabber.GetAssetPath(soundFxFileName, SoundFxFolderName);
        AudioStream audioStream = (AudioStream)ResourceLoader.Load(localPath);

        if (!_soundFxPlayer1.Playing) // if nothing is playing, play on channel 1
        {
            _soundFxPlayer1.Stream = audioStream;
            _soundFxPlayer1.Play();
        }
        else if (!_soundFxPlayer2.Playing) // if channel 1 playing, play on 2
        {
            _soundFxPlayer2.Stream = audioStream;
            _soundFxPlayer2.Play();
        }
        //otherwise, do nothing, 2 sound effects at once is enough
        //TODO: add queueing system for sfx with short backoff timer
        // to allow for a second effect to be played if close enough to the end of the first line
    }
    
    public void StopSoundFx()
    {
        _soundFxPlayer1.Stop();
        _soundFxPlayer2.Stop();
    }
    
    
    // Voice Acting Methods //
    public void PlayVoiceLine(string voiceLineFileName)
    {
        string localPath = ResourceGrabber.GetAssetPath(voiceLineFileName, VoiceLineFolderName);
        AudioStream audioStream = (AudioStream)ResourceLoader.Load(localPath);
        
        if (!_voiceLinePlayer.Playing) // if nothing is playing, play voice line
        {
            _voiceLinePlayer.Stream = audioStream;
            _voiceLinePlayer.Play();
        }
        //otherwise, do nothing, 1 line at a time for clarity
        //TODO: add queueing system for voice lines with short backoff timer
        // to allow for a second line to be played if close enough to the end of the first line
    }
    
    
    // General Methods //
    public void StopAllAudio()
    {
        StopAllTracks();
        StopSoundFx();
        _voiceLinePlayer.Stop();
    }

    public void PreloadAudio(List<string> fileNames)
    {
        Dictionary<string, AudioStream> preLoadedAudio = new Dictionary<string, AudioStream>();

        foreach (string file in fileNames)
        {
            string localPath = ResourceGrabber.GetAssetPath(file, MusicFolderName);
            AudioStream audioStream = (AudioStream)ResourceLoader.Load(localPath);
            
            preLoadedAudio.Add(file, audioStream);
        }

        this.preLoadedAudio = preLoadedAudio;
    }
}

//todo it would be worth preloading audio files used by a scene when the scene is loaded, to avoid stuttering when playing audio for the first time