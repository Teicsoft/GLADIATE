using Godot;
using System.Collections.Generic;
using System.Linq;
using TeicsoftSpectacleCards.scripts.battle;
using TeicsoftSpectacleCards.scripts.XmlParsing;
using TeicsoftSpectacleCards.scripts.XmlParsing.models;

public partial class Battle : Node2D {

    [Export] private PackedScene CardScene;
    [Export] private PackedScene EnemyScene;
    private Hand Hand;
    private Deck Deck;
    private Discard Discard;
    private PathFollow2D EnemiesLocation;
    private GameState GameState;

    public override void _Ready() {
        ModelTesting();

        GameState = new GameState();
        Hand = GetNode<Hand>("Hand");
        Discard = new Discard();
        Deck = new Deck(Discard);
        Hand = GetNode<Hand>("Hand");
        Hand.Discard = Discard;
        EnemiesLocation = GetNode<PathFollow2D>("Enemies/EnemiesLocation");
        GameState.Hand = Hand;
        GameState.Deck = Deck;

        List<Card> initialDeck = new();

        foreach (int i in Enumerable.Range(0, 6)) {
            Card card = CardScene.Instantiate<Card>();


            card.TestSetup((int)(1 + GD.Randi() % 4), true, new Color(1, 1, 1));
            initialDeck.Add(card);
        }

        foreach (int i in Enumerable.Range(0, 3)) {
            Card card = CardScene.Instantiate<Card>();
            card.TestSetup((int)(1 + GD.Randi() % 4), false, new Color(1, 0.5f, 0.5f));
            initialDeck.Add(card);
        }

        Card lastCard = CardScene.Instantiate<Card>();
        lastCard.TestSetup(15, true, new Color(0, 0, 0));
        initialDeck.Add(lastCard);

        Deck.AddCards(initialDeck);
        Deck.Shuffle();


        float locationRatio = 1f / 2;
        foreach (int i in Enumerable.Range(0, 3)) {
            Enemy enemy = EnemyScene.Instantiate<Enemy>();
            enemy.EnemySelected += GameState.SelectEnemy;
            GameState.Enemies.Add(enemy);
            EnemiesLocation.ProgressRatio = i * locationRatio;
            enemy.Position = EnemiesLocation.Position;
            AddChild(enemy);
        }
    }

    public override void _Process(double delta) { }

    private void OnPlayButtonPressed() {
        GameState.PlaySelectedCard();
    }

    private void OnDeckPressed() {
        GameState.Draw();
    }

    private void ModelTesting() {
        //This is a test to see if the card factory works, feel free to remove it
        Card modelCard = CardXmlParser.ParseCardsFromXml("res://data/cards/card_template.xml");
        GD.Print("\n CardModelTest: " + modelCard + "\n");

        //This is a test to see if the combo parsing works, feel free to remove it
        ComboModel combo = ComboXmlParser.ParseComboFromXml("res://data/combos/combo_template.xml");
        GD.Print("\n ComboModelTest: " + combo + ": ");
        foreach (Card card in combo.CardList) { GD.Print(card + "\n"); }

        GD.Print("\n");

        // //This is a test to see if the Deck parsing works, feel free to remove it
        Deck deck = DeckXmlParser.ParseDeckFromXml("res://data/decks/deck_template.xml");
        GD.Print("\n DeckModelTest" + deck + ": ");
        foreach (Card card in deck.Cards) { GD.Print(card + "\n"); }

        GD.Print("\n");

        // //This is a test to see if the GameState works, feel free to remove it
        GameState gameState = new GameState();
        GD.Print("\n GameStateTest: ");

        for (int i = 0; i < 5; i++) {
            gameState.ComboCheck(modelCard);
            GD.Print(gameState);
        }

        GD.Print("\n");
    }
}
