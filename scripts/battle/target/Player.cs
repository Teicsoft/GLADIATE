using System;

namespace TeicsoftSpectacleCards.scripts.battle.target;

public class Player : ITarget {

    public event EventHandler PlayerHealthChangedCustomEvent;
    public event EventHandler PlayerDefenseLowerChangedCustomEvent;
    public event EventHandler PlayerDefenseUpperChangedCustomEvent;

    public string Name { get; set; }
    public int MaxHealth { get; set; }
    private int _health;

    public int Health {
        get => _health;
        set {
            _health = value;
            PlayerHealthChangedCustomEvent?.Invoke(this, EventArgs.Empty);
        }
    }

    private int _defenseLower = 0;

    public int DefenseLower {
        get => _defenseLower;
        set {
            _defenseLower = value;
            PlayerDefenseLowerChangedCustomEvent?.Invoke(this, EventArgs.Empty);
        }
    }

    private int _defenseUpper = 1;

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

    public void Damage(int damage, Utils.PositionEnum position = Utils.PositionEnum.Upper) {
        bool blocked = false;
        switch (position) {
            case Utils.PositionEnum.Upper:
                if (DefenseUpper > 0) {
                    blocked = true;
                    DefenseUpper--;
                }

                break;
            case Utils.PositionEnum.Lower:
                if (DefenseLower > 0) {
                    blocked = true;
                    DefenseLower--;
                }

                break;
        }

        if (!blocked) { DirectDamage(damage); }
    }

    private void DirectDamage(int damage) {
        Health = Math.Max(0, Health - damage);
    }

    public void Heal(int amount) {
        Health = Math.Min(MaxHealth, Health + Math.Abs(amount));
    }

    public void Stun(int stun, Utils.PositionEnum position = Utils.PositionEnum.Upper) {
        bool blocked = false;
        if ((DefenseUpper > 0) || (DefenseLower > 0)) {
            blocked = true;
            DefenseUpper = 0;
            DefenseLower = 0;
        }
    }

    public void ModifyBlock(int change, Utils.PositionEnum position) {
        switch (position) {
            case Utils.PositionEnum.Upper:
                DefenseUpper += change;
                break;
            case Utils.PositionEnum.Lower:
                DefenseLower += change;
                break;
        }
    }
}
