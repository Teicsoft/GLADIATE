using System;

public class SingleEnemyAffectedEventArgs : EventArgs {
    public SingleEnemyAffectedEventArgs(Action<Enemy> action) {
        Action = action;
    }

    public Action<Enemy> Action { get; set; }
}
