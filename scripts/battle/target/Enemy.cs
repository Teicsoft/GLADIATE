using System;
using Godot;
using TeicsoftSpectacleCards.scripts.battle.card;

namespace TeicsoftSpectacleCards.scripts.battle.target;

public partial class Enemy : Node2D,
    Target {
    [Signal]
    public delegate void EnemySelectedEventHandler(Enemy enemy);

    private ColorRect Rect;
    private Button SelectButton;

    public int MaxHealth { get; set; } = 12;
    public int Health { get; set; } = 12;
    public int DefenseLower { get; set; } = 0;
    public int DefenseUpper { get; set; } = 1;

    public Deck<Card> Deck;

    private Discard<Card> _discard;

    public Discard<Card> Discard {
        get => _discard;
        set {
            _discard = value;
            Deck.Discard = value;
        }
    }

    public override void _Ready() {
        SelectButton = GetNode<Button>("SelectButton");
        Rect = GetNode<ColorRect>("ColorRect");
        Rect.Color = new Color(0, 1, 0);
    }

    public override void _Process(double delta) { }

    private void OnPress() {
        EmitSignal(SignalName.EnemySelected, this);
    }

    public Card DrawCard() {
        return Deck.DrawCards(1)[0];
    }

    public void TakeCardIntoDiscard(Card card) {
        Discard.AddCard(card);
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

    public void Stun(int stun) {
        if (DefenseUpper > 0 || DefenseLower > 0) {
            DefenseUpper = 0;
            DefenseLower = 0;
        }

        //else { Lose turn }
    }

    private void DirectDamage(int damage) {
        Health = Math.Max(0, Health - damage);
        HealthColorCheck();
    }

    private void HealthColorCheck() {
        float blue = (DefenseUpper > 0 ? 0.5f : 0f) + (DefenseLower > 0 ? 0.5f : 0f);
        if (Health == 0) { ChangeColour(new Color(0, 0, blue)); }
        else {
            float healthRatio = (float)Health / MaxHealth;
            ChangeColour(new Color(1f - healthRatio, healthRatio, blue));
        }
    }

    public void ChangeColour(Color color) {
        Rect.Color = color;
    }
}
