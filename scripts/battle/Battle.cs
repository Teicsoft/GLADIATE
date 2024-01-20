using Godot;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using TeicsoftSpectacleCards.scripts.battle;
using TeicsoftSpectacleCards.scripts.customresource;
using TeicsoftSpectacleCards.scripts.XmlParsing;
using TeicsoftSpectacleCards.scripts.XmlParsing.models;

public partial class Battle : Node2D {

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
        ModelTesting();
        
        hand = GetNode<Hand>("Hand");
        deck = new Deck();
        discard = new Discard();
        enemiesLocation = GetNode<PathFollow2D>("Enemies/EnemiesLocation");
        hand.discard = discard;
        deck.discard = discard;
        List<Card> initialDeck = new();
        foreach (int i in Enumerable.Range(0, 6)) {
            Card card = cardScene.Instantiate<Card>();
            
            
            card.TestSetup((int)(1 + GD.Randi() % 4), true, new Color(1, 1, 1));
            initialDeck.Add(card);
        }

        foreach (int i in Enumerable.Range(0, 3)) {
            Card card = cardScene.Instantiate<Card>();
            card.TestSetup((int)(1 + GD.Randi() % 4), false, new Color(1, 0.5f, 0.5f));
            initialDeck.Add(card);
        }

        Card lastCard = cardScene.Instantiate<Card>();
        lastCard.TestSetup(15, true, new Color(0, 0, 0));
        initialDeck.Add(lastCard);

        deck.AddCards(initialDeck);
        deck.Shuffle();


        float locationRatio = 1f / 2;
        foreach (int i in Enumerable.Range(0, 3)) {
            Enemy enemy = enemyScene.Instantiate<Enemy>();
            enemy.EnemySelected += SelectEnemy;
            enemies.Add(enemy);
            enemiesLocation.ProgressRatio = i * locationRatio;
            enemy.Position = enemiesLocation.Position;
            AddChild(enemy);
        }
    }

    public override void _Process(double delta) { }

    public void PlaySelectedCard() {
        Card card = hand.GetSelectedCard();
        if (card != null && !(card.targetRequired && GetSelectedEnemy() == null)) {
            card.Play(GetSelectedEnemy(), enemies);
            hand.DiscardSelectedCard();
        }
    }

    private Enemy GetSelectedEnemy() {
        return selectedEnemyIndex != -1 ? enemies[selectedEnemyIndex] : null;
    }

    private void SelectEnemy(Enemy enemy) {
        int enemyIndex = enemies.IndexOf(enemy);
        selectedEnemyIndex = selectedEnemyIndex != enemyIndex ? enemyIndex : -1;
    }

    private void OnPlayButtonPressed() {
        PlaySelectedCard();
    }

    private void OnDeckPressed() {
        hand.AddCards(deck.DrawCard());
    }

    
    private void ModelTesting()
    {
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
        foreach (Card card in deck.cards) { GD.Print(card + "\n"); }
        GD.Print("\n");

        // //This is a test to see if the GameState works, feel free to remove it
        GameState gameState = new GameState();
        GD.Print("\n GameStateTest: ");

        for (int i = 0; i < 5; i++)
        {
            gameState.GamestateCardPlayed(modelCard);
            GD.Print(gameState);
        }
        GD.Print("\n");
    }
}
