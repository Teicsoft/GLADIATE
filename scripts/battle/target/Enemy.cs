using System;
using System.Collections.Generic;
using System.Linq;
using Godot;
using TeicsoftSpectacleCards.scripts.battle.card;

namespace TeicsoftSpectacleCards.scripts.battle.target;

public partial class Enemy : Node2D, ITarget {
	[Signal] public delegate void EnemySelectedEventHandler(Enemy enemy);

	public string Name { get; set; }
	public Utils.ModifierEnum Modifier { get; set; } = Utils.ModifierEnum.None;
	public Color Color;
	public Deck<Card> Deck;
	private Discard<Card> _discard;
	private int _health = 12;
	private int _defenseUpper = 1;
	private int _defenseLower = 0;
	public int MaxHealth { get; set; } = 12;
	public List<Utils.StatusEnum> Statuses { get; set; } = new();

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
		} else if (stun > 0) {
			foreach (int _ in Enumerable.Range(0, stun)) { Statuses.Add(Utils.StatusEnum.Stunned); }
		}
	}

	private void DirectDamage(int damage) { Health = Math.Max(0, Health - damage); }

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

	public bool IsStunned() {
		if (Statuses.Contains(Utils.StatusEnum.Stunned)) {
			GD.Print($"{Name} was stunned this turn");
			Statuses.Remove(Utils.StatusEnum.Stunned);
			return true;
		}

		return false;
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
}
