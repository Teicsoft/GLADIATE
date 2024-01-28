using System;
using Godot;
using TeicsoftSpectacleCards.scripts.battle.card;

namespace TeicsoftSpectacleCards.scripts.battle.target;

public partial class Enemy : Node2D,
    ITarget {
    [Signal]
    public delegate void EnemySelectedEventHandler(Enemy enemy);

    private ColorRect _rect;
    private Button _selectButton;

    public string Name { get; set; }
    public int MaxHealth { get; set; } = 12;
    private int _health = 12;

    public int Health {
        get => _health;
        set {
            _health = value;
            UpdateHealthBar();
        }
    }

    private int _defenseLower = 0;

    public int DefenseLower {
        get => _defenseLower;
        set {
            _defenseLower = value;
            UpdateDefenseLowerDisplay();
        }
    }

    private int _defenseUpper = 1;

    public int DefenseUpper {
        get => _defenseUpper;
        set {
            _defenseUpper = value;
            UpdateDefenseUpperDisplay();
        }
    }

    public Deck<Card> Deck;

    private Discard<Card> _discard;
    private Label _healthDisplay;
    private ProgressBar _healthProgressBar;
    private ColorRect _upperBlockRect;
    private Label _upperBlockDisplay;
    private ColorRect _lowerBlockRect;
    private Label _lowerBlockDisplay;
    public Color Color;

    public Discard<Card> Discard {
        get => _discard;
        set {
            _discard = value;
            Deck.Discard = value;
        }
    }

    public override void _Ready() {
        _selectButton = GetNode<Button>("SelectButton");
        _rect = GetNode<ColorRect>("ColorRect");
        _rect.Color = Color;
        _healthDisplay = GetNode<Label>("HealthDisplay");
        _healthDisplay.Text = MaxHealth + "/" + MaxHealth;
        _healthProgressBar = GetNode<ProgressBar>("HealthProgressBar");
        _healthProgressBar.Ratio = 1;
        _upperBlockRect = GetNode<ColorRect>("UpperBlockRect");
        _upperBlockDisplay = GetNode<Label>("UpperBlockDisplay");
        _lowerBlockRect = GetNode<ColorRect>("LowerBlockRect");
        _lowerBlockDisplay = GetNode<Label>("LowerBlockDisplay");
        UpdateHealthBar();
        UpdateDefenseUpperDisplay();
        UpdateDefenseLowerDisplay();
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
        GD.Print("position");
        GD.Print(position);
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
        GD.Print("blocked");
        GD.Print(blocked);

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

    private void UpdateHealthBar() {
        _healthDisplay.Text = Health + "/" + MaxHealth;
        _healthProgressBar.Ratio = (double)Health / MaxHealth;
    }

    private void UpdateDefenseUpperDisplay() {
        _upperBlockRect.Color = new Color(0, 0, DefenseUpper > 0 ? 1 : 0);
        _upperBlockDisplay.Text = DefenseUpper.ToString();
    }
    private void UpdateDefenseLowerDisplay() {
        _lowerBlockRect.Color = new Color(0, 0, DefenseLower > 0 ? 1 : 0);
        _lowerBlockDisplay.Text = DefenseLower.ToString();
    }
}
