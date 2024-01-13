using Godot;
using System;
using Godot.Collections;

public partial class Card : Button {

    [Signal]
    public delegate void CardPressedEventHandler();

    private static readonly Color[] Colors = {
        new(1.0f, 0.0f, 0.0f),
        new(1.0f, 1.0f, 0.0f),
        new(0.0f, 1.0f, 0.0f),
        new(0.0f, 1.0f, 1.0f),
        new(0.0f, 0.0f, 1.0f),
        new(1.0f, 0.0f, 1.0f),
        new(1.0f, 1.0f, 1.0f),
        new(0.0f, 0.0f, 0.0f),
    };

    public Color Color;

    public bool Selected = false;

    public override void _Ready() {
        this.ToggleMode = true;
        Color = Colors[GD.Randi() % Colors.Length];
        AddThemeColorOverride("font_color", Color);
    }

    public override void _Process(double delta) { }

    public void Target(Enemy enemy) {
        enemy.ChangeColour(Color);
    }

    private void OnToggle(bool toggled_on) {
        Selected = toggled_on;
    }

}
