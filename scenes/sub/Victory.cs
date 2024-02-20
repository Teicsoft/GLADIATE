using Godot;
using System;
using GLADIATE.scripts.audio;
using GLADIATE.scripts.autoloads;

public partial class Victory : Control
{
    private AudioEngine audioEngine;
    private GLADIATE.scripts.autoloads.SceneLoader sceneLoader;
    
    Label label;
    
    
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        audioEngine = GetNode<AudioEngine>("/root/audio_engine");
        sceneLoader = GetNode<GLADIATE.scripts.autoloads.SceneLoader>("/root/SceneLoader");
        
        label = GetNode<Label>("ColorRect/ColorRect/VBoxContainer/Spectacle Points");
        label.Text = sceneLoader.SpectaclePoints + " Spectacle Points!";
    }
    
    private void OnTimerTimeout()
    {
        
        
        sceneLoader.GoToScene("res://scenes/main/Credits.tscn");
        audioEngine.PlayMusic("Shop_loop_audio.wav");
    }
    
}
