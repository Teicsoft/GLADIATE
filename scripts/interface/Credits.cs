using System.Collections.Generic;
using GLADIATE.scripts.audio;
using Godot;

namespace GLADIATE.scripts;

public partial class Credits : Control
{
    private List<string> _lines;
    private ScrollContainer _scroll;
    private VBoxContainer _vbox;
    private BoxContainer _box;
    private BoxContainer _box2;
    private Label _creditslabel;
    private AnimationPlayer animation;

    private autoloads.SceneLoader sceneLoader;

    public override void _Ready()
    {
        _lines = Readfile();
        _vbox = GetNode<VBoxContainer>("ColorRect/MarginContainer/ScrollContainer/VBoxContainer");


        
        sceneLoader = GetNode<autoloads.SceneLoader>("/root/SceneLoader");

        var audioEngine = GetNode<AudioEngine>("/root/audio_engine");
        
        sceneLoader.i = 0;
        sceneLoader.Health = 0;
        sceneLoader.SpectaclePoints = 0;

        Label label = GetNode<Label>("BoxContainer/VBoxContainer2/creditslabel");

        foreach(string line in _lines)  { 
        label.Text+= line+"\n";
         }


        label.SizeFlagsHorizontal = SizeFlags.Fill;
        label.SizeFlagsVertical = SizeFlags.ExpandFill;
        label.HorizontalAlignment = HorizontalAlignment.Center;
        label.AddThemeFontSizeOverride("font_size", 50);
    
        animation=GetNode<AnimationPlayer>("AnimationPlayer");
        animation.Play("Credit roll");


    }

    public override void _Process(double delta){
                if (animation.CurrentAnimationPosition==(20))
        {
            sceneLoader = GetNode<autoloads.SceneLoader>("/root/SceneLoader");
            sceneLoader.GoToScene("res://scenes/sub/TeicsoftLogo.tscn");
        }
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


}

