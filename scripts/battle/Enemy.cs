using System;
using Godot;

public partial class Enemy : Node2D {
    [Signal]
    public delegate void EnemySelectedEventHandler(Enemy enemy);

    private ColorRect rect;
    private Button selectButton;
    private int currentHealth = 12;
    private int maxHealth = 12;
    private int defenseLower = 0;
    private int defenseUpper = 1;

    public override void _Ready() {
        selectButton = GetNode<Button>("SelectButton");
        rect = GetNode<ColorRect>("ColorRect");
        rect.Color = new Color(0, 1, 0);
    }

    public void Damage(int damage, PositionEnum position = PositionEnum.Upper) {
        bool blocked = false;
        switch (position) {
            case PositionEnum.Upper:
                if (defenseUpper > 0) {
                    blocked = true;
                    defenseUpper--;
                }

                break;
            case PositionEnum.Lower:
                if (defenseLower > 0) {
                    blocked = true;
                    defenseLower--;
                }

                break;
        }

        if (!blocked) { DirectDamage(damage); }
    }

    public void Stun(int stun) {
        bool blocked = false;
            if ((defenseUpper > 0) || (defenseLower > 0)) 
            {
                blocked = true;
                defenseUpper = 0;
                defenseLower = 0;                
            }

            //else { Lose turn }
        
    }

    private void DirectDamage(int damage) {
        currentHealth = Math.Max(0, currentHealth - damage);
        if (currentHealth == 0) { ChangeColour(new Color(0, 0, 0)); } else {
            float healthRatio = (float)currentHealth / maxHealth;
            ChangeColour(new Color(1f - healthRatio, healthRatio, 0));
        }
    }

    public override void _Process(double delta) { }

    public void ChangeColour(Color color) {
        rect.Color = color;
    }

    private void OnPress() {
        EmitSignal(SignalName.EnemySelected, this);
    }

    public enum ModifierEnum {
        Grappled,
        Grounded,
        Juggled,
        None
    }

    public enum PositionEnum {
        Upper,
        Lower,
        None
    }
}
