using System;
using System.Collections.Generic;
using System.Linq;
using GLADIATE.scripts.battle.target;

namespace GLADIATE.scripts.battle;

public class StatusesDecorator : HashSet<Utils.StatusEnum>
{
    public Enemy Enemy { get; set; }
    public Player Player { get; set; }
    
    private HashSet<Utils.StatusEnum> _statuses;
    
    public event EventHandler PlayerStatusesChangedCustomEvent;

    public StatusesDecorator()
    {
        _statuses = new();
    }
    
    
    public new bool Add(Utils.StatusEnum item)
    {
        bool result = _statuses.Add(item);
        if (Enemy != null) { Enemy.UpdateStatusesToolTip(); }
        if (Player != null) { PlayerStatusesChangedCustomEvent?.Invoke(this, EventArgs.Empty); }
        return result;
    }
    
    
    public new bool Remove(Utils.StatusEnum item)
    {
        bool result = false;
        if (_statuses.Contains(item)) { result = _statuses.Remove(item); }
        if (Enemy != null) { Enemy.UpdateStatusesToolTip(); }
        if (Player != null) { PlayerStatusesChangedCustomEvent?.Invoke(this, EventArgs.Empty); }
        return result;
    }

    public new int Count(){ return _statuses.Count(); }

    public new bool Contains(Utils.StatusEnum item){ return _statuses.Contains(item); }

    public new Enumerator GetEnumerator(){ return _statuses.GetEnumerator(); }
}
