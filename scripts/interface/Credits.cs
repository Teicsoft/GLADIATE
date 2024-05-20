using System.Collections.Generic;
using System.Linq;
using GLADIATE.scripts.audio;
using Godot;

namespace GLADIATE.scripts;

public partial class Credits : Control {
    private ScrollContainer _scroll;
    private VBoxContainer _vbox;
    private BoxContainer _box;
    private BoxContainer _box2;
    private Label _creditsLabel;
    private AnimationPlayer _animation;
    private autoloads.SceneLoader _sceneLoader;

    public override void _Ready() {
        _vbox = GetNode<VBoxContainer>("ColorRect/MarginContainer/ScrollContainer/VBoxContainer");
    
        _sceneLoader = GetNode<autoloads.SceneLoader>("/root/SceneLoader");
        _sceneLoader.i = 0;
        _sceneLoader.Health = 0;
        _sceneLoader.SpectaclePoints = 0;

        Label label = GetNode<Label>("BoxContainer/VBoxContainer2/creditslabel");
        foreach (string line in ReadFile()) { label.Text += line + "\n"; }
        label.SizeFlagsHorizontal = SizeFlags.Fill;
        label.SizeFlagsVertical = SizeFlags.ExpandFill;
        label.HorizontalAlignment = HorizontalAlignment.Center;
        label.AddThemeFontSizeOverride("font_size", 50);

        _animation = GetNode<AnimationPlayer>("AnimationPlayer");
        _animation.Play("Credit roll");
    }

    public override void _Process(double delta) {
        if (_animation.CurrentAnimationPosition == 20) {
            _sceneLoader = GetNode<autoloads.SceneLoader>("/root/SceneLoader");
            _sceneLoader.GoToScene("res://scenes/sub/TeicsoftLogo.tscn");
        }
    }

    private List<string> ReadFile() {
        return FileAccess.Open("res://data/credits.txt", FileAccess.ModeFlags.Read).GetAsText().Split("\n").ToList();
    }

    private void LoadTexture(string path) {
        TextureRect textureRect = new();
        textureRect.Texture = GD.Load<Texture2D>(path);

        textureRect.StretchMode = TextureRect.StretchModeEnum.KeepAspect;
        textureRect.SizeFlagsHorizontal = SizeFlags.Fill;

        CenterContainer centerContainer = new();
        centerContainer.AddChild(textureRect);
        _vbox.AddChild(centerContainer);
    }
}
