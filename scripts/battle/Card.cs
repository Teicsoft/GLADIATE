using Godot;
using System;
using System.Collections.Generic;

public partial class Card : Node2D {
    [Signal]
    public delegate void CardSelectedEventHandler(Card card);

    public Color color { get; set; }
    public Button selectButton;

    public override void _Ready() {
        selectButton = GetNode<Button>("SelectButton");
        selectButton.AddThemeColorOverride("font_color", this.color);
    }

    public override void _Process(double delta) { }

    public void Play( /*GameState gameState,*/ Enemy enemy, List<Enemy> allEnemies) {
        if (color.Equals(new Color(0.0f, 1.0f, 0.0f))) {
            foreach (Enemy e in allEnemies) { e.ChangeColour(color); }
        } else { enemy.ChangeColour(color); }
    }

    public bool RequiresTarget() {
        return !color.Equals(new Color(0.0f, 1.0f, 0.0f));
    }

    public void ChangeColor(Color color) {
        this.color = color;
    }

    private void OnPress() {
        EmitSignal(SignalName.CardSelected, this);
    }
}
