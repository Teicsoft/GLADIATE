using Godot;
using System;
using System.Collections.Generic;

public partial class Card : Button {
    [Signal]
    public delegate void CardSelectedEventHandler(Card card);

    // public delegate void PlayerAffectedEventHandler(Action<Player> action);

    public event EventHandler<SingleEnemyAffectedEventArgs> RaiseSingleEnemyAffectedEvent;

    public event EventHandler<MultiEnemyAffectedEventArgs> RaiseMultiEnemyAffectedEvent;

    public Color Color { get; set; }

    public override void _Ready() { }

    public override void _Process(double delta) { }

    public void Play() {
        if (!Color.Equals(new Color(0.0f, 1.0f, 0.0f))) {
            RaiseSingleEnemyAffectedEvent?.Invoke(this, new SingleEnemyAffectedEventArgs(SingleEnemyEffect));
        } else {
            RaiseMultiEnemyAffectedEvent?.Invoke(this, new MultiEnemyAffectedEventArgs(MultiEnemyEffect));
        }
    }

    public void SingleEnemyEffect(Enemy enemy) {
        enemy.ChangeColour(Color);
    }

    public void MultiEnemyEffect(List<Enemy> enemies) {
        foreach (Enemy enemy in enemies) { enemy.ChangeColour(Color); }
    }

    private void OnPress() {
        EmitSignal(SignalName.CardSelected, this);
    }
}

public class SingleEnemyAffectedEventArgs : EventArgs {
    public SingleEnemyAffectedEventArgs(Action<Enemy> action) {
        Action = action;
    }

    public Action<Enemy> Action { get; set; }
}

public class MultiEnemyAffectedEventArgs : EventArgs {
    public MultiEnemyAffectedEventArgs(Action<List<Enemy>> action) {
        Action = action;
    }

    public Action<List<Enemy>> Action { get; set; }
}
