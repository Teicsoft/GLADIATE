using System;
using System.Collections.Generic;
using Godot;

namespace GLADIATE.scripts.battle.target;

public partial class Player : Node2D, ITarget {
    public event EventHandler PlayerHealthChangedCustomEvent;
    public event EventHandler PlayerDefenseLowerChangedCustomEvent;
    public event EventHandler PlayerDefenseUpperChangedCustomEvent;

    public string Name { get; set; }
    public int MaxHealth { get; set; }
    public HashSet<Utils.StatusEnum> Statuses { get; set; } = new();
    public Utils.ModifierEnum Modifier { get; set; } = Utils.ModifierEnum.None;
    private int _health;
    private int _defenseLower = 0;
    private int _defenseUpper = 1;

    public int Health {
        get => _health;
        set {
            _health = value;
            PlayerHealthChangedCustomEvent?.Invoke(this, EventArgs.Empty);
        }
    }

    public int DefenseLower {
        get => _defenseLower;
        set {
            _defenseLower = value;
            PlayerDefenseLowerChangedCustomEvent?.Invoke(this, EventArgs.Empty);
        }
    }

    public int DefenseUpper {
        get => _defenseUpper;
        set {
            _defenseUpper = value;
            PlayerDefenseUpperChangedCustomEvent?.Invoke(this, EventArgs.Empty);
        }
    }

    public Player(int maxHealth, int defenseLower, int defenseUpper) {
        MaxHealth = maxHealth;
        Health = maxHealth;
        DefenseLower = defenseLower;
        DefenseUpper = defenseUpper;
        Name = "Player";
    }

    public void Damage(int damage, Utils.PositionEnum position) {
        if (!CheckBlock(position)) { DirectDamage(damage); }
    }

    public bool CheckBlock(Utils.PositionEnum position) {
        switch (position) {
            case Utils.PositionEnum.Upper:
                if (DefenseUpper > 0) {
                    DefenseUpper--;
                    return true;
                }

                break;
            case Utils.PositionEnum.Lower:
                if (DefenseLower > 0) {
                    DefenseLower--;
                    return true;
                }

                break;
        }
        return false;
    }

    public void DirectDamage(int damage) { Health = Math.Max(0, Health - damage); }

    public void Ground(Utils.PositionEnum position) {
        if (!CheckBlock(position)) { Modifier = Utils.ModifierEnum.Grounded; }
    }

    public void Juggle() { Modifier = Utils.ModifierEnum.Juggled; }

    public void Grapple(Utils.PositionEnum position) {
        if (!CheckBlock(position)) { Modifier = Utils.ModifierEnum.Grappled; }
    }

    public void Heal(int amount) { Health = Math.Min(MaxHealth, Health + Math.Abs(amount)); }

    public void Stun() {
        if (DefenseUpper > 0 || DefenseLower > 0) {
            DefenseUpper = 0;
            DefenseLower = 0;
        } else {
            // TODO: Figure this out.
            // End turn immediately?
        }
    }

    public bool IsStunned() { return Statuses.Remove(Utils.StatusEnum.Stunned); }

    public void ModifyBlock(int change, Utils.PositionEnum position) {
        switch (position) {
            case Utils.PositionEnum.Upper:
                DefenseUpper = Math.Max(DefenseUpper + change, 0);
                break;
            case Utils.PositionEnum.Lower:
                DefenseLower = Math.Max(DefenseLower + change, 0);
                break;
        }
    }
}
