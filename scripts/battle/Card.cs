using Godot;
using System;
using System.Collections.Generic;

public partial class Card : Button {
    [Signal]
    public delegate void CardSelectedEventHandler(Card card);

    public Color Color { get; set; }

    public override void _Ready() { }

    public override void _Process(double delta) { }

    public void Play( /*GameState gameState,*/ Enemy enemy, List<Enemy> allEnemies) {
        if (Color.Equals(new Color(0.0f, 1.0f, 0.0f))) {
            foreach (Enemy e in allEnemies) { e.ChangeColour(Color); }
        } else {
            enemy.ChangeColour(Color);
        }
    }

    public bool RequiresTarget() {
        return !Color.Equals(new Color(0.0f, 1.0f, 0.0f));
    }

    private void OnPress() {
        EmitSignal(SignalName.CardSelected, this);
    }
}
