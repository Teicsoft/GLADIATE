using Godot;
using System;
using System.IO;
using TeicsoftSpectacleCards.scripts.audio;
using TeicsoftSpectacleCards.scripts.autoloads;
using FileAccess = Godot.FileAccess;

public partial class Lore : Control
{
    private AudioEngine audioEngine;
    Label loreText;
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        audioEngine = GetNode<AudioEngine>("/root/audio_engine");
        
        loreText = GetNode<Label>("ColorRect/Label");
        FileAccess file = FileAccess.Open("res://assets/writing/openingTextCrawl.txt", FileAccess.ModeFlags.Read);
        var content = file.GetAsText();
        GD.Print(content);
        loreText.Text = content;
    }
    
    private void OnButtonPressed()
    {
        var sceneLoader = GetNode<SceneLoader>("/root/scene_loader");
        sceneLoader.GoToScene("res://scenes/main/Deck Select.tscn");
        audioEngine.PlayMusic("fuckaroundandfindout.wav");
    }
}


