using Godot;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using TeicsoftSpectacleCards.scripts.customresource;
using TeicsoftSpectacleCards.scripts.XmlParsing;
using TeicsoftSpectacleCards.scripts.XmlParsing.models;

public partial class Battle : Node2D
{
    private static readonly List<Color> COLORS = new()
    {
        new(1.0f, 0.0f, 0.0f),
        new(1.0f, 0.0f, 0.0f),
        new(1.0f, 0.0f, 0.0f),
        new(1.0f, 0.0f, 0.0f),
        new(0.0f, 1.0f, 0.0f),
        new(0.0f, 1.0f, 0.0f),
        new(0.0f, 0.0f, 1.0f),
        new(0.0f, 0.0f, 1.0f),
    };

    private static readonly Color[] PALETTE =
    {
        new(1.0f, 0.0f, 0.0f),
        new(1.0f, 1.0f, 0.0f),
        new(0.0f, 1.0f, 0.0f),
        new(0.0f, 1.0f, 1.0f),
        new(0.0f, 0.0f, 1.0f),
        new(1.0f, 0.0f, 1.0f),
        new(1.0f, 1.0f, 1.0f),
        new(0.0f, 0.0f, 0.0f),
    };

    [Export] private PackedScene cardScene;
    [Export] private PackedScene enemyScene;
    private Hand hand;
    private Deck deck;
    private Discard discard;
    private PathFollow2D enemiesLocation;
    private List<Enemy> enemies = new();
    private int selectedEnemyIndex = -1;

    public override void _Ready()
    {
        //This is a test to see if the card factory works, feel free to remove it
        Card modelCard = CardXmlParser.ParseCardsFromXml("res://data/cards/card_template.xml");
        GD.Print(modelCard + "\n");

        //This is a test to see if the combo parsing works, feel free to remove it
        ComboModel combo = ComboXmlParser.ParseComboFromXml("res://data/combos/combo_template.xml");
        GD.Print(combo + "\n");

        // //This is a test to see if the Deck parsing works, feel free to remove it
        DeckModel deckModel = DeckXmlParser.ParseDeckFromXml("res://data/decks/deck_template.xml");
        GD.Print(deckModel + "\n");
        hand = GetNode<Hand>("Hand");
        deck = new Deck();
        discard = new Discard();
        enemiesLocation = GetNode<PathFollow2D>("Enemies/EnemiesLocation");
        hand.discard = discard;
        deck.discard = discard;
        List<Card> initialDeck = new();
        foreach (Color color in COLORS)
        {
            Card card = cardScene.Instantiate<Card>();
            card.ChangeColor(color);
            initialDeck.Add(card);
        }

        Card lastCard = cardScene.Instantiate<Card>();
        lastCard.ChangeColor(PALETTE[GD.Randi() % PALETTE.Length]);
        initialDeck.Add(lastCard);

        deck.AddCards(initialDeck);
        deck.Shuffle();


        float locationRatio = 1f / 2;
        foreach (int i in Enumerable.Range(0, 3))
        {
            Enemy enemy = enemyScene.Instantiate<Enemy>();
            enemy.EnemySelected += SelectEnemy;
            enemies.Add(enemy);
            enemiesLocation.ProgressRatio = i * locationRatio;
            enemy.Position = enemiesLocation.Position;
            AddChild(enemy);
        }
    }

    public override void _Process(double delta)
    {
    }

    public void PlaySelectedCard()
    {
        Card card = hand.GetSelectedCard();
        if (card != null && !(card.RequiresTarget() && GetSelectedEnemy() == null))
        {
            card.Play(GetSelectedEnemy(), enemies);
            hand.DiscardSelectedCard();
        }
    }

    private Enemy GetSelectedEnemy()
    {
        return selectedEnemyIndex != -1 ? enemies[selectedEnemyIndex] : null;
    }

    private void SelectEnemy(Enemy enemy)
    {
        int enemyIndex = enemies.IndexOf(enemy);
        selectedEnemyIndex = selectedEnemyIndex != enemyIndex ? enemyIndex : -1;
    }

    private void OnPlayButtonPressed()
    {
        PlaySelectedCard();
    }

    private void OnDeckPressed()
    {
        hand.AddCards(deck.DrawCard());
    }
}