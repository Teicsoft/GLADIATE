using System;
using System.Collections.Generic;
using Godot;

namespace GLADIATE.scripts.battle.target;

public partial class Player : Node2D, ITarget {
    public event EventHandler PlayerHealthChangedCustomEvent;
    public event EventHandler PlayerDefenseLowerChangedCustomEvent;
    public event EventHandler PlayerDefenseUpperChangedCustomEvent;
    public event EventHandler PlayerModifierChangedCustomEvent;

    public string Name { get; set; }
    public int MaxHealth { get; set; }
    public Utils.StatusesDecorator Statuses { get; set; } = new();

    private Utils.ModifierEnum _modifier = Utils.ModifierEnum.None;
    public Utils.ModifierEnum Modifier {
        get => _modifier;
        set {
            _modifier = value;
            PlayerModifierChangedCustomEvent?.Invoke(this, EventArgs.Empty);
        }
    }

    private int _health;
    private int _defenseLower = 0;
    private int _defenseUpper = 1;

    public int Health {
        get => _health;
        set {
            Utils.DirectionEventArgs args = Utils.CheckDirection(_health, value);
            _health = value;
            PlayerHealthChangedCustomEvent?.Invoke(this, args);
        }
    }

    public int DefenseLower {
        get => _defenseLower;
        set {
            Utils.DirectionEventArgs args = Utils.CheckDirection(_defenseLower, value);
            _defenseLower = value;
            PlayerDefenseLowerChangedCustomEvent?.Invoke(this, args);
        }
    }

    public int DefenseUpper {
        get => _defenseUpper;
        set {
            Utils.DirectionEventArgs args = Utils.CheckDirection(_defenseUpper, value);
            _defenseUpper = value;
            PlayerDefenseUpperChangedCustomEvent?.Invoke(this, args);
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
        }
        else
        {
            GD.Print("Trying to stun player");
            Statuses.Add(Utils.StatusEnum.Stunned);
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
