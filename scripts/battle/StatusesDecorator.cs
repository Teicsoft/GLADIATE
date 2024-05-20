using System;
using System.Collections.Generic;
using System.Linq;
using GLADIATE.scripts.battle.target;

namespace GLADIATE.scripts.battle;

public class StatusesDecorator : HashSet<Utils.StatusEnum> {
    public ITarget Target { get; set; }

    private HashSet<Utils.StatusEnum> _statuses = new();

    public event EventHandler PlayerStatusesChangedCustomEvent;

    public new bool Add(Utils.StatusEnum item) {
        bool result = _statuses.Add(item);
        switch (Target) {
            case Enemy enemy: enemy.UpdateStatusesToolTip();
                break;
            case Player: PlayerStatusesChangedCustomEvent?.Invoke(this, EventArgs.Empty);
                break;
        }
        return result;
    }

    public new bool Remove(Utils.StatusEnum item) {
        bool result = _statuses.Remove(item);
        switch (Target) {
            case Enemy enemy: enemy.UpdateStatusesToolTip();
                break;
            case Player: PlayerStatusesChangedCustomEvent?.Invoke(this, EventArgs.Empty);
                break;
        }
        return result;
    }

    public new int Count() { return _statuses.Count(); }

    public new bool Contains(Utils.StatusEnum item) { return _statuses.Contains(item); }

    public new Enumerator GetEnumerator() { return _statuses.GetEnumerator(); }
}
