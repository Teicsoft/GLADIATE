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

    public Color Color { get; set; }

    public bool Selected = false;

    public override void _Ready() {
    }

    public override void _Process(double delta) { }

    public void Target(Enemy enemy) {
        enemy.ChangeColour(Color);
    }

    private void OnPress() {
        Selected = !Selected;
    }
}
