using Godot;
using System.Collections.Generic;
using System.Linq;
using TeicsoftSpectacleCards.scripts.battle;
using TeicsoftSpectacleCards.scripts.battle.card;
using TeicsoftSpectacleCards.scripts.XmlParsing;
using TeicsoftSpectacleCards.scripts.XmlParsing.models;

public partial class Battle : Node2D {

    [Export] private PackedScene cardScene;
    [Export] private PackedScene enemyScene;
    private Hand hand;
    private Deck deck;
    private Discard discard;
    private PathFollow2D enemiesLocation;
    private GameState gameState;

    public override void _Ready() {
        ModelTesting();
        
        deck = DeckXmlParser.ParseDeckFromXml("res://data/decks/deck_template.xml");
        gameState = new GameState();
        hand = GetNode<Hand>("Hand");
        discard = new Discard();
        deck.Discard = discard;
        hand.discard = discard;
        enemiesLocation = GetNode<PathFollow2D>("Enemies/EnemiesLocation");
        gameState.hand = hand;
        gameState.deck = deck;
        

        // List<CardSleeve> initialDeck = new();
        //
        //
        // foreach (int i in Enumerable.Range(0, 6)) {
        //     CardSleeve cardSleeve = cardScene.Instantiate<CardSleeve>();
        //
        //
        //     cardSleeve.TestSetup((int)(1 + GD.Randi() % 4), true, new Color(1, 1, 1));
        //     initialDeck.Add(cardSleeve);
        // }
        //
        // foreach (int i in Enumerable.Range(0, 3)) {
        //     CardSleeve cardSleeve = cardScene.Instantiate<CardSleeve>();
        //     cardSleeve.TestSetup((int)(1 + GD.Randi() % 4), false, new Color(1, 0.5f, 0.5f));
        //     initialDeck.Add(cardSleeve);
        // }
        //
        // CardSleeve lastCardSleeve = cardScene.Instantiate<CardSleeve>();
        // lastCardSleeve.TestSetup(15, true, new Color(0, 0, 0));
        // initialDeck.Add(lastCardSleeve);

        // deck.AddCards(initialDeck);
        deck.Shuffle();


        float locationRatio = 1f / 2;
        foreach (int i in Enumerable.Range(0, 3)) {
            Enemy enemy = enemyScene.Instantiate<Enemy>();
            enemy.EnemySelected += gameState.SelectEnemy;
            gameState.enemies.Add(enemy);
            enemiesLocation.ProgressRatio = i * locationRatio;
            enemy.Position = enemiesLocation.Position;
            AddChild(enemy);
        }
    }

    public override void _Process(double delta) { }

    private void OnPlayButtonPressed() {
        gameState.PlaySelectedCard();
    }

    private void OnDeckPressed() {
        gameState.Draw();
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
        foreach (CardSleeve cardSleeve in deck.CardSleeves)
        {
            GD.Print(cardSleeve.Card + "\n");
        }

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
