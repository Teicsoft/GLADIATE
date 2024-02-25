using GLADIATE.scripts.audio;
using Godot;

namespace GLADIATE.scenes.sub;

public partial class Victory : Control
{
    private AudioEngine _audioEngine;
    private scripts.autoloads.SceneLoader _sceneLoader;
    
    public override void _Ready()
    {
        _audioEngine = GetNode<AudioEngine>("/root/audio_engine");
        _sceneLoader = GetNode<GLADIATE.scripts.autoloads.SceneLoader>("/root/SceneLoader");
        
        Label label = GetNode<Label>("ColorRect/ColorRect/VBoxContainer/Spectacle Points");
        label.Text = _sceneLoader.SpectaclePoints + " Spectacle Points!";
        
        SaveData.WriteScoretoJson( _sceneLoader.DeckSelected, _sceneLoader.SpectaclePoints);
    }
    
    private void OnTimerTimeout()
    {
        _sceneLoader.GoToScene("res://scenes/main/Credits.tscn");
        _audioEngine.PlayMusic("Shop_loop_audio.wav");
    }
    
}