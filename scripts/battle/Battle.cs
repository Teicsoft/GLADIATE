using System;
using Godot;
using System.Collections.Generic;
using System.Linq;
using TeicsoftSpectacleCards.scripts.battle;
using TeicsoftSpectacleCards.scripts.battle.card;
using TeicsoftSpectacleCards.scripts.battle.target;
using TeicsoftSpectacleCards.scripts.XmlParsing;

public partial class Battle : Node2D {

    [Export] private PackedScene _cardScene;
    [Export] private PackedScene _enemyScene;
    private Hand _hand;
    private Deck<CardSleeve> _deck;
    private Discard<CardSleeve> _discard;
    private PathFollow2D _enemiesLocation;
    private GameState _gameState;
    private ProgressBar _playerHealthProgressBar;
    private Label _multiplierDisplay;
    private Label _spectacleDisplay;
    private Label _playerHealthDisplay;
    private ColorRect _selectedIndicator;
    private Label _playerDefenseLowerDisplay;
    private ColorRect _playerDefenseLowerRect;
    private Label _playerDefenseUpperDisplay;
    private ColorRect _playerDefenseUpperRect;

    const int ENEMY_COUNT = 3;
    private List<Tuple<string, Color>> _enemyDeets =new() {
        new Tuple<string, Color>("Red", new Color(0.5f, 0, 0)),
        new Tuple<string, Color>("Green", new Color(0, 0.5f, 0)),
        new Tuple<string, Color>("Blue", new Color(0, 0, 0.5f)),
    };

    public override void _Ready() {
        ModelTesting();

        Dictionary<string, List<string>> decks = DeckXmlParser.ParseAllDecks();
        List<string> playerCardIds;
        decks.TryGetValue("deck_player", out playerCardIds);
        _discard = new Discard<CardSleeve>();
        _deck = new Deck<CardSleeve>();
        _deck.Discard = _discard;
        _deck.AddCards(Deck<CardSleeve>.SleeveCards(playerCardIds.Select(CardPrototypes.CloneCard).ToList()));
        _hand = GetNode<Hand>("Hand");
        _hand.discard = _discard;
        _gameState = new GameState();
        _gameState.Hand = _hand;
        _gameState.Deck = _deck;
        _gameState.Player.PlayerHealthChangedCustomEvent += OnPlayerHealthChanged;
        _gameState.Player.PlayerDefenseLowerChangedCustomEvent += OnPlayerDefenseLowerChanged;
        _gameState.Player.PlayerDefenseUpperChangedCustomEvent += OnPlayerDefenseUpperChanged;
        _gameState.MultiplierChangedCustomEvent += OnMultiplierChanged;
        _gameState.SpectacleChangedCustomEvent += OnSpectacleChanged;
        _deck.Shuffle();
        _gameState.Draw(4);
        InitialiseHud();

        _enemiesLocation = GetNode<PathFollow2D>("Enemies/EnemiesLocation");
        decks.TryGetValue("deck_enemy", out List<string> enemyCardIds);
        foreach (int i in Enumerable.Range(0, ENEMY_COUNT)) {
            Enemy enemy = CreateEnemy(GetEnemyDeck(enemyCardIds), i);
            AddChild(enemy);
            _gameState.Enemies.Add(enemy);
        }

        GD.Print(" ==== ==== START GAME ==== ====");
    }

    private void InitialiseHud() {
        _playerHealthDisplay = GetNode<Label>("HUD/PlayerHealthDisplay");
        _playerHealthDisplay.Text = _gameState.Player.MaxHealth + "/" + _gameState.Player.MaxHealth;
        _playerHealthProgressBar = GetNode<ProgressBar>("HUD/PlayerHealthProgressBar");
        _playerHealthProgressBar.Ratio = 1;
        _playerDefenseLowerDisplay = GetNode<Label>("HUD/PlayerLowerBlockDisplay");
        _playerDefenseLowerRect = GetNode<ColorRect>("HUD/PlayerLowerBlockRect");
        _playerDefenseLowerRect.Color = new Color(0, 0, _gameState.Player.DefenseLower > 0 ? 1 : 0);
        _playerDefenseLowerDisplay.Text = _gameState.Player.DefenseLower.ToString();
        _playerDefenseUpperDisplay = GetNode<Label>("HUD/PlayerUpperBlockDisplay");
        _playerDefenseUpperRect = GetNode<ColorRect>("HUD/PlayerUpperBlockRect");
        _playerDefenseUpperRect.Color = new Color(0, 0, _gameState.Player.DefenseUpper > 0 ? 1 : 0);
        _playerDefenseUpperDisplay.Text = _gameState.Player.DefenseUpper.ToString();
        _playerHealthProgressBar = GetNode<ProgressBar>("HUD/PlayerHealthProgressBar");
        _playerHealthProgressBar.Ratio = 1;
        _playerHealthDisplay = GetNode<Label>("HUD/PlayerHealthDisplay");
        _playerHealthDisplay.Text = _gameState.Player.MaxHealth + "/" + _gameState.Player.MaxHealth;
        _playerHealthProgressBar = GetNode<ProgressBar>("HUD/PlayerHealthProgressBar");
        _playerHealthProgressBar.Ratio = 1;
        _spectacleDisplay = GetNode<Label>("HUD/SpectacleDisplay");
        _multiplierDisplay = GetNode<Label>("HUD/MultiplierDisplay");
        _selectedIndicator = GetNode<ColorRect>("HUD/SelectedIndicator");
    }

    public override void _Process(double delta) { }

    private Enemy CreateEnemy(Deck<Card> enemyDeck, int i) {
        float progressRatio = (float)i / (ENEMY_COUNT - 1);
        Enemy enemy = _enemyScene.Instantiate<Enemy>();
        enemy.Name = _enemyDeets[i].Item1;
        enemy.Color = _enemyDeets[i].Item2;
        _enemiesLocation.ProgressRatio = progressRatio;
        enemy.Position = _enemiesLocation.Position;
        enemy.Deck = enemyDeck;
        enemy.Discard = new();
        enemy.EnemySelected += _gameState.SelectEnemy;
        enemy.EnemySelected += MoveSelectedIndicator;
        return enemy;
    }

    private static Deck<Card> GetEnemyDeck(List<string> enemyCardIds) {
        Deck<Card> enemyDeck = new();
        enemyDeck.AddCards(enemyCardIds.Select(CardPrototypes.CloneCard).ToList());
        enemyDeck.Shuffle();
        return enemyDeck;
    }

    private void MoveSelectedIndicator(Enemy enemy) {
        _selectedIndicator.Position =
            enemy.Position != _selectedIndicator.Position ? enemy.Position : new Vector2(-100, 520);
    }

    private void OnPlayerHealthChanged(object sender, EventArgs e) {
        Player playerObject = _gameState.Player;
        _playerHealthDisplay.Text = playerObject.Health + "/" + playerObject.MaxHealth;
        _playerHealthProgressBar.Ratio = (double)playerObject.Health / playerObject.MaxHealth;
    }

    private void OnPlayerDefenseUpperChanged(object sender, EventArgs e) {
        _playerDefenseUpperRect.Color = new Color(0, 0, _gameState.Player.DefenseUpper > 0 ? 1 : 0);
        _playerDefenseUpperDisplay.Text = _gameState.Player.DefenseUpper.ToString();
    }

    private void OnPlayerDefenseLowerChanged(object sender, EventArgs e) {
        _playerDefenseLowerRect.Color = new Color(0, 0, _gameState.Player.DefenseLower > 0 ? 1 : 0);
        _playerDefenseLowerDisplay.Text = _gameState.Player.DefenseLower.ToString();
    }

    private void OnMultiplierChanged(object sender, EventArgs e) {
        _multiplierDisplay.Text = _gameState.Multiplier.ToString();
    }

    private void OnSpectacleChanged(object sender, EventArgs e) {
        _spectacleDisplay.Text = _gameState.SpectaclePoints.ToString();
    }

    private void OnPlayButtonPressed() {
        _gameState.PlaySelectedCard();
    }

    private void OnDeckPressed() {
        _gameState.Draw();
    }

    private void EndTurn() {
        _gameState.EndTurn();
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
