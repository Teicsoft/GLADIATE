using Godot;
using System;
using System.Collections.Generic;

public partial class Enemy : Node2D {

    [Signal]
    public delegate void EnemyAttackedEventHandler(Enemy enemy);

    private ColorRect rect;

    public override void _Ready() {
        rect = GetNode<ColorRect>("ColorRect");
    }

    public override void _Process(double delta) { }

    public void ChangeColour(Color color) {
        rect.Color = color;
    }

    private void OnAttackButtonPressed() {
        EmitSignal(SignalName.EnemyAttacked, this);
    }

}
