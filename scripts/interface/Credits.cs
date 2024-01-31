using Godot;
using System;
using System.Collections.Generic;
using TeicsoftSpectacleCards.scripts.audio;
using TeicsoftSpectacleCards.scripts.autoloads;

public partial class Credits : Control
{
    private List<string> lines;
    private ScrollContainer scroll;
    private VBoxContainer vbox;
    private int i = 0;
    private Timer timer;
    
    
    public override void _Ready()
    {
        lines = readfile();
        vbox = GetNode<VBoxContainer>("ColorRect/MarginContainer/ScrollContainer/VBoxContainer");
        scroll = GetNode<ScrollContainer>("ColorRect/MarginContainer/ScrollContainer");
        timer = GetNode<Timer>("Timer");

        vbox.AddChild(new Label());
        loadTexture("res://assets/sprites/Dave/Eagle.png");
        loadTexture("res://assets/sprites/Dave/Gladiate.png");
        
        var audioEngine = GetNode<AudioEngine>("/root/audio_engine");
        audioEngine.PlayMusic("Shop_loop_audio.wav");
    }

    public List<string> readfile()
    {
        List<string> lines = new List<string>();
        using var file = FileAccess.Open("res://data/credits.txt", FileAccess.ModeFlags.Read);
        
        foreach (string line in file.GetAsText().Split("\n"))
        {
            lines.Add(line);
        }
        return lines;
    }

    public void loadTexture(string path)
    {
        TextureRect textureRect = new TextureRect();
        textureRect.Texture = GD.Load<Texture2D>(path);

        textureRect.StretchMode = TextureRect.StretchModeEnum.KeepAspect;
        textureRect.SizeFlagsHorizontal = SizeFlags.Fill;
        
        CenterContainer centerContainer = new CenterContainer();
        centerContainer.AddChild(textureRect);
        vbox.AddChild(centerContainer);
    }
    
    private void OnTimerTimeout()
    {
        Label label  = new Label();

        if (i == 0)
        {
            vbox.AddChild(new Label());
        }
        
        if (i >= lines.Count)
        {
            label.Set("text", "\n");
        }
        else
        {
            label.Set("text", lines[i] );
        }

        label.SizeFlagsHorizontal = SizeFlags.Fill;
        label.SizeFlagsVertical = SizeFlags.ExpandFill;
        label.HorizontalAlignment = HorizontalAlignment.Center;
        label.AddThemeFontSizeOverride("font_size", 75);
        vbox.AddChild(label);
        
        
        if (i == (lines.Count + 33))
        {
            var sceneLoader = GetNode<SceneLoader>("/root/scene_loader");
            sceneLoader.GoToScene("res://scenes/sub/TeicsoftLogo.tscn");
        }


        scroll.ScrollVertical = (int)scroll.GetVScrollBar().MaxValue;
        i++;
    }
    
}
