using Godot;
using GLADIATE.scripts.audio;
using FileAccess = Godot.FileAccess;

public partial class Lore : Control
{
    private AudioEngine audioEngine;

    Label loreText;

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
        var sceneLoader = GetNode<GLADIATE.scripts.autoloads.SceneLoader>("/root/SceneLoader");
        sceneLoader.GoToScene("res://scenes/main/Deck Select.tscn");
        audioEngine.PlayMusic("fuckaroundandfindout.wav");
    }
}
