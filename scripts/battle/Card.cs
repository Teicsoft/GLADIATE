using Godot;
using System;

public partial class Card : Button {
    [Signal]
    public delegate void CardSelectedEventHandler(Card card);

    // public delegate void PlayerAffectedEventHandler(Action<Enemy> action);

    public event EventHandler<SingleEnemyAffectedEventArgs> RaiseSingleEnemyAffectedEvent;

    // public delegate void MultiEnemyAffectedEventHandler(Action<List<Enemy>> action);

    public Color Color { get; set; }

    public override void _Ready() { }

    public override void _Process(double delta) { }

    public void Play() {
        EventHandler<SingleEnemyAffectedEventArgs> raiseEvent = RaiseSingleEnemyAffectedEvent;
        if (raiseEvent != null) { raiseEvent(this, new SingleEnemyAffectedEventArgs(Affect)); }
    }

    public void Affect(Enemy enemy) {
        enemy.ChangeColour(Color);
    }

    private void OnPress() {
        EmitSignal(SignalName.CardSelected, this);
    }
}
