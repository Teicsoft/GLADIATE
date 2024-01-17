using Godot;
using System;
using System.Collections.Generic;

public partial class Card : Button {
    [Signal]
    public delegate void CardSelectedEventHandler(Card card);

    public Color color { get; set; }

    public override void _Ready() { }

    public override void _Process(double delta) { }

    public void Play( /*GameState gameState,*/ Enemy enemy, List<Enemy> allEnemies) {
        if (color.Equals(new Color(0.0f, 1.0f, 0.0f))) {
            foreach (Enemy e in allEnemies) { e.ChangeColour(color); }
        } else {
            enemy.ChangeColour(color);
        }
    }

    public bool RequiresTarget() {
        return !color.Equals(new Color(0.0f, 1.0f, 0.0f));
    }

    private void OnPress() {
        EmitSignal(SignalName.CardSelected, this);
    }
}
