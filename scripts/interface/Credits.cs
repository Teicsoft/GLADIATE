using System.Collections.Generic;
using Godot;
using TeicsoftSpectacleCards.scripts.audio;
using TeicsoftSpectacleCards.scripts.autoloads;

namespace TeicsoftSpectacleCards.scripts;

public partial class Credits : Control
{
    private List<string> _lines;
    private ScrollContainer _scroll;
    private VBoxContainer _vbox;
    private int counter = 0;
    private Timer _timer;

    private SceneLoader sceneLoader;

    public override void _Ready()
    {
        _lines = Readfile();
        _vbox = GetNode<VBoxContainer>("ColorRect/MarginContainer/ScrollContainer/VBoxContainer");
        _scroll = GetNode<ScrollContainer>("ColorRect/MarginContainer/ScrollContainer");
        _timer = GetNode<Timer>("Timer");

        _vbox.AddChild(new Label());
        LoadTexture("res://assets/sprites/Dave/Eagle.png");
        LoadTexture("res://assets/sprites/Dave/Gladiate.png");
        
        sceneLoader = GetNode<SceneLoader>("/root/scene_loader");

        var audioEngine = GetNode<AudioEngine>("/root/audio_engine");
        
        sceneLoader.i = 0;
        sceneLoader.Health = 0;
        sceneLoader.SpectaclePoints = 0;
    }

    public List<string> Readfile()
    {
        List<string> lines = new List<string>();
        using var file = FileAccess.Open("res://data/credits.txt", FileAccess.ModeFlags.Read);

        foreach (string line in file.GetAsText().Split("\n"))
        {
            lines.Add(line);
        }

        return lines;
    }

    public void LoadTexture(string path)
    {
        TextureRect textureRect = new TextureRect();
        textureRect.Texture = GD.Load<Texture2D>(path);

        textureRect.StretchMode = TextureRect.StretchModeEnum.KeepAspect;
        textureRect.SizeFlagsHorizontal = SizeFlags.Fill;

        CenterContainer centerContainer = new CenterContainer();
        centerContainer.AddChild(textureRect);
        _vbox.AddChild(centerContainer);
    }

    private void _OnTimerTimeout()
    {
        Label label = new Label();

        if (counter == 0)
        {
            _vbox.AddChild(new Label());
        }

        if (counter >= _lines.Count)
        {
            label.Set("text", "\n");
        }
        else
        {
            label.Set("text", _lines[counter]);
        }

        label.SizeFlagsHorizontal = SizeFlags.Fill;
        label.SizeFlagsVertical = SizeFlags.ExpandFill;
        label.HorizontalAlignment = HorizontalAlignment.Center;
        label.AddThemeFontSizeOverride("font_size", 75);
        _vbox.AddChild(label);


        if (counter == (_lines.Count + 15))
        {
            sceneLoader = GetNode<SceneLoader>("/root/scene_loader");
            sceneLoader.GoToScene("res://scenes/sub/TeicsoftLogo.tscn");
        }


        _scroll.ScrollVertical = (int)_scroll.GetVScrollBar().MaxValue;
        counter++;
    }
}

