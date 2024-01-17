using Godot;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using TeicsoftSpectacleCards.scripts.customresource;
using TeicsoftSpectacleCards.scripts.customresource.Cards;
using TeicsoftSpectacleCards.scripts.customresource.combos;
using TeicsoftSpectacleCards.scripts.customresource.deck;
using TeicsoftSpectacleCards.scripts.XmlParsing;

public partial class Battle : Node2D {

	private static readonly List<Color> Colors = new() {
		new(1.0f, 0.0f, 0.0f),
		new(1.0f, 0.0f, 0.0f),
		new(1.0f, 0.0f, 0.0f),
		new(1.0f, 0.0f, 0.0f),
		new(0.0f, 1.0f, 0.0f),
		new(0.0f, 1.0f, 0.0f),
		new(0.0f, 0.0f, 1.0f),
		new(0.0f, 0.0f, 1.0f),
	};

	private static readonly Color[] Palette = {
		new(1.0f, 0.0f, 0.0f),
		new(1.0f, 1.0f, 0.0f),
		new(0.0f, 1.0f, 0.0f),
		new(0.0f, 1.0f, 1.0f),
		new(0.0f, 0.0f, 1.0f),
		new(1.0f, 0.0f, 1.0f),
		new(1.0f, 1.0f, 1.0f),
		new(0.0f, 0.0f, 0.0f),
	};

	[Export] private PackedScene _cardScene;
	[Export] private PackedScene _enemyScene;
	private Hand _hand;
	private Deck _deck;
	private Discard _discard;
	private PathFollow2D _enemiesLocation;
	private List<Enemy> _enemies = new();
	private int selectedEnemyIndex = -1;

	public override void _Ready() {
		//This is a test to see if the card factory works, feel free to remove it
		CardModel modelCard = CardXmlParser.ParseCardsFromXml("res://data/cards/card_template.xml");
		GD.Print(modelCard.ToString() + "\n");

		//This is a test to see if the combo parsing works, feel free to remove it
		ComboModel combo = ComboXmlParser.ParseComboFromXml("res://data/combos/combo_template.xml");
		GD.Print(combo.ToString()+ "\n");

		//
		// //This is a test to see if the Deck parsing works, feel free to remove it
		DeckModel deck = DeckXmlParser.ParseDeckFromXml("res://data/decks/deck_template.xml");
		
		_hand = GetNode<Hand>("Hand");
		_deck = new Deck();
		_discard = new Discard();
		_enemiesLocation = GetNode<PathFollow2D>("Enemies/EnemiesLocation");
		_hand.Discard = _discard;
		_deck.Discard = _discard;
		List<Card> initialDeck = new();
		foreach (Color color in Colors) {
			Card card = _cardScene.Instantiate<Card>();
			card.Color = color;
			card.AddThemeColorOverride("font_color", card.Color);
			initialDeck.Add(card);
		}

		Card lastCard = _cardScene.Instantiate<Card>();
		lastCard.Color = Palette[GD.Randi() % Palette.Length];
		lastCard.AddThemeColorOverride("font_color", lastCard.Color);
		initialDeck.Add(lastCard);

		_deck.AddCards(initialDeck);
		_deck.Shuffle();


		float locationRatio = 1f / 2;
		foreach (int i in Enumerable.Range(0, 3)) {
			Enemy enemy = _enemyScene.Instantiate<Enemy>();
			enemy.EnemySelected += SelectEnemy;
			_enemies.Add(enemy);
			_enemiesLocation.ProgressRatio = i * locationRatio;
			enemy.Position = _enemiesLocation.Position;
			AddChild(enemy);
		}
	}

	public override void _Process(double delta) { }

	public void PlaySelectedCard() {
		Card card = _hand.GetSelectedCard();
		if (card != null && !(card.RequiresTarget() && GetSelectedEnemy() == null)) {
			card.Play(GetSelectedEnemy(), _enemies);
			_hand.DiscardSelectedCard();
		}
	}

	private Enemy GetSelectedEnemy() {
		return selectedEnemyIndex != -1 ? _enemies[selectedEnemyIndex] : null;
	}

	private void SelectEnemy(Enemy enemy) {
		selectedEnemyIndex = _enemies.IndexOf(enemy);
	}

	private void OnPlayButtonPressed() {
		PlaySelectedCard();
	}

	private bool EnemySelected() {
		return selectedEnemyIndex != -1;
	}

	private void OnDeckPressed() {
		_hand.AddCards(_deck.DrawCard());
	}
}
