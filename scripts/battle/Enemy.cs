using System;
using Godot;

public partial class Enemy : Node2D {
    [Signal]
    public delegate void EnemySelectedEventHandler(Enemy enemy);

    private ColorRect Rect;
    private Button SelectButton;
    private int CurrentHealth = 12;
    private int MaxHealth = 12;
    private int DefenseLower = 0;
    private int DefenseUpper = 1;

    public override void _Ready() {
        SelectButton = GetNode<Button>("SelectButton");
        Rect = GetNode<ColorRect>("ColorRect");
        Rect.Color = new Color(0, 1, 0);
    }

    public void Damage(int damage, PositionEnum position = PositionEnum.Upper) {
        bool blocked = false;
        switch (position) {
            case PositionEnum.Upper:
                if (DefenseUpper > 0) {
                    blocked = true;
                    DefenseUpper--;
                }

                break;
            case PositionEnum.Lower:
                if (DefenseLower > 0) {
                    blocked = true;
                    DefenseLower--;
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
        CurrentHealth = Math.Max(0, CurrentHealth - damage);
        if (CurrentHealth == 0) { ChangeColour(new Color(0, 0, 0)); } else {
            float healthRatio = (float)CurrentHealth / MaxHealth;
            ChangeColour(new Color(1f - healthRatio, healthRatio, 0));
        }
    }

    public override void _Process(double delta) { }

    public void ChangeColour(Color color) {
        Rect.Color = color;
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
