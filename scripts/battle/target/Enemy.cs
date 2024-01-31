using System;
using System.Collections.Generic;
using System.Linq;
using Godot;
using TeicsoftSpectacleCards.scripts.battle.card;

namespace TeicsoftSpectacleCards.scripts.battle.target;

public partial class Enemy : Node2D, ITarget {
    [Signal] public delegate void EnemySelectedEventHandler(Enemy enemy);

    public string Id { get; set; }
    public string Name { get; set; }
    public string Image { get; set; }
    public string SoundEffect { get; set; }
    public string Lore { get; set; }
    public string DeckId { get; set; }

    public Utils.ModifierEnum Modifier { get; set; } = Utils.ModifierEnum.None;
    public Color Color;
    public Deck<Card> Deck;
    private int _health;
    private int _defenseUpper = 1;
    private int _defenseLower = 0;
    public int MaxHealth { get; set; }
    public HashSet<Utils.StatusEnum> Statuses { get; set; } = new();

    public int Health {
        get => _health;
        set {
            _health = value;
            UpdateHealthBar();
        }
    }

    public int DefenseLower {
        get => _defenseLower;
        set {
            _defenseLower = value;
            UpdateDefenseLowerDisplay();
        }
    }

    public int DefenseUpper {
        get => _defenseUpper;
        set {
            _defenseUpper = value;
            UpdateDefenseUpperDisplay();
        }
    }

    public void InitializeEnemy(
        string id, string name, string image, string soundEffect, string lore, string deckId, int maxHealth,
        int defenseUpper, int defenseLower
    ) {
        Id = id;
        Name = name;
        Image = image;
        SoundEffect = soundEffect;
        Lore = lore;
        DeckId = deckId;
        MaxHealth = maxHealth;
        _health = maxHealth;
        _defenseUpper = defenseUpper;
        _defenseLower = defenseLower;
    }

    public void CloneTo(Enemy enemy) {
        enemy.Id = Id;
        enemy.Name = Name;
        enemy.Image = Image;
        enemy.SoundEffect = SoundEffect;
        enemy.Lore = Lore;
        enemy.DeckId = DeckId;
        enemy.MaxHealth = MaxHealth;
        enemy._health = MaxHealth;
        enemy._defenseUpper = DefenseUpper;
        enemy._defenseLower = DefenseLower;
    }

    public override void _Ready() {
        GetNode<ColorRect>("ColorRect").Color = Color;
        UpdateHealthBar();
        UpdateDefenseUpperDisplay();
        UpdateDefenseLowerDisplay();
    }

    public override void _Process(double delta) { }

    private void OnPress() { EmitSignal(SignalName.EnemySelected, this); }

    public Card DrawCard() { return Deck.DrawCards(1)[0]; }

    public void TakeCardIntoDiscard(Card card) { Deck.Discard.AddCard(card); }

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

    public void Stun() {
        if (DefenseUpper > 0 || DefenseLower > 0) {
            DefenseUpper = 0;
            DefenseLower = 0;
        } else { Statuses.Add(Utils.StatusEnum.Stunned); }
    }

    public bool IsStunned() { return Statuses.Remove(Utils.StatusEnum.Stunned); }

    public void DirectDamage(int damage) { Health = Math.Max(0, Health - damage); }

    public void Ground(Utils.PositionEnum position) {
        if (!CheckBlock(position)) { Modifier = Utils.ModifierEnum.Grounded; }
    }

    public void Juggle() { Modifier = Utils.ModifierEnum.Juggled; }

    public void Grapple(Utils.PositionEnum position) {
        if (!CheckBlock(position)) { Modifier = Utils.ModifierEnum.Grappled; }
    }

    public void ModifyBlock(int change, Utils.PositionEnum position) {
        switch (position) {
            case Utils.PositionEnum.Upper:
                DefenseUpper = Math.Max(DefenseUpper + change,0);
                break;
            case Utils.PositionEnum.Lower:
                DefenseLower = Math.Max(DefenseLower + change,0);
                break;
        }
    }

    private void UpdateHealthBar() {
        GetNode<Label>("HealthDisplay").Text = Health + "/" + MaxHealth;
        GetNode<ProgressBar>("HealthProgressBar").Ratio = (double)Health / MaxHealth;
    }

    private void UpdateDefenseUpperDisplay() {
        GetNode<ColorRect>("UpperBlockRect").Color = new Color(0, 0, DefenseUpper > 0 ? 1 : 0);
        GetNode<Label>("UpperBlockDisplay").Text = DefenseUpper.ToString();
    }

    private void UpdateDefenseLowerDisplay() {
        GetNode<ColorRect>("LowerBlockRect").Color = new Color(0, 0, DefenseLower > 0 ? 1 : 0);
        GetNode<Label>("LowerBlockDisplay").Text = DefenseLower.ToString();
    }

    public override string ToString() {
        return
            $"{nameof(Id)}: {Id}, {nameof(Name)}: {Name}, {nameof(Image)}: {Image}, {nameof(SoundEffect)}: {SoundEffect}, {nameof(Lore)}: {Lore}, {nameof(DeckId)}: {DeckId}, {nameof(Modifier)}: {Modifier}, {nameof(Color)}: {Color}, {nameof(Deck)}: {Deck}, {nameof(_health)}: {_health}, {nameof(_defenseUpper)}: {_defenseUpper}, {nameof(_defenseLower)}: {_defenseLower}, {nameof(MaxHealth)}: {MaxHealth}, {nameof(Statuses)}: {Statuses}";
    }
}
