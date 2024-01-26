using Godot;
using System.Collections.Generic;
using System.Linq;
using TeicsoftSpectacleCards.scripts.battle;
using TeicsoftSpectacleCards.scripts.battle.card;
using TeicsoftSpectacleCards.scripts.battle.target;
using TeicsoftSpectacleCards.scripts.XmlParsing;

public partial class Battle : Node2D {

    [Export] private PackedScene cardScene;
    [Export] private PackedScene enemyScene;
    private Hand hand;
    private Deck<CardSleeve> deck;
    private Discard<CardSleeve> discard;
    private PathFollow2D enemiesLocation;
    private GameState gameState;

    public override void _Ready() {
        ModelTesting();

        Dictionary<string, List<string>> decks = DeckXmlParser.ParseAllDecks();
        List<string> playerCardIds;
        decks.TryGetValue("deck_player", out playerCardIds);
        deck = new Deck<CardSleeve>();
        deck.AddCards(Deck<CardSleeve>.SleeveCards(playerCardIds.Select(CardPrototypes.CloneCard).ToList()));
        gameState = new GameState();
        hand = GetNode<Hand>("Hand");
        discard = new Discard<CardSleeve>();
        deck.Discard = discard;
        hand.discard = discard;
        enemiesLocation = GetNode<PathFollow2D>("Enemies/EnemiesLocation");
        gameState.Hand = hand;
        gameState.Deck = deck;
        deck.Shuffle();
        gameState.Draw(4);
        
        StyleBoxFlat styleBoxFlat = new StyleBoxFlat();
        styleBoxFlat.CornerRadiusTopLeft = 10;
        styleBoxFlat.CornerRadiusTopRight = 10;
        styleBoxFlat.CornerRadiusBottomRight = 10;
        styleBoxFlat.CornerRadiusBottomLeft = 10;
        GetNode<TextureButton>("HUD/Deck").DrawStyleBox(styleBoxFlat, default);



        List<string> enemyCardIds;
        decks.TryGetValue("deck_enemy", out enemyCardIds);
        float locationRatio = 1f / 2;
        foreach (int i in Enumerable.Range(0, 3)) {
            Enemy enemy = enemyScene.Instantiate<Enemy>();
            Deck<Card> enemyDeck = new();
            enemyDeck.AddCards(enemyCardIds.Select(CardPrototypes.CloneCard).ToList());
            enemyDeck.Shuffle();
            enemy.Deck = enemyDeck;
            enemy.Discard = new();
            enemy.EnemySelected += gameState.SelectEnemy;
            gameState.Enemies.Add(enemy);
            enemiesLocation.ProgressRatio = i * locationRatio;
            enemy.Position = enemiesLocation.Position;
            AddChild(enemy);
        }

        GD.Print(" ==== ==== START GAME ==== ====");
        GD.Print(gameState.SpectaclePoints);
        GD.Print(gameState.Player.Health);
    }

    public override void _Process(double delta) { }

    private void OnPlayButtonPressed() {
        gameState.PlaySelectedCard();
    }

    private void OnDeckPressed() {
        gameState.Draw();
    }

    private void EndTurn() {
        gameState.EndTurn();
    }

    private void ModelTesting() {
        //This is a test to see if the card factory works, feel free to remove it
        Card modelCard = CardXmlParser.ParseCardsFromXml("res://data/cards/card_template.xml");
        GD.Print("\n CardModelTest: " + modelCard + "\n");

        //This is a test to see if the combo parsing works, feel free to remove it
        Combo combo = ComboXmlParser.ParseComboFromXml("res://data/combos/combo_template.xml");
        GD.Print("\n ComboModelTest: " + combo + ": ");
        foreach (Card card in combo.CardList) { GD.Print(card + "\n"); }

        GD.Print("\n");

        // //This is a test to see if the Deck parsing works, feel free to remove it
        // Deck<CardSleeve> deck = DeckXmlParser.ParseDeckFromXml("res://data/decks/deck_template.xml");
        // GD.Print("\n DeckModelTest" + deck + ": ");
        // foreach (CardSleeve card in deck.Cards) { GD.Print(card.Card + "\n"); }
        //
        // GD.Print("\n");

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
