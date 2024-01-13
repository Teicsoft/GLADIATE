using Godot;
using System;

public partial class HUD : CanvasLayer {

    [Export] public PackedScene CardScene { get; set; }
    [Export] public Hand hand { get; set; }

    public override void _Ready() { }

    public override void _Process(double delta) { }

    private void OnDeckPressed() {
        Card card = CardScene.Instantiate<Card>();
        hand.AddCard(card);
    }
}
