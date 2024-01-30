using System;
using Godot;
using System.Collections.Generic;
using System.Linq;
using TeicsoftSpectacleCards.scripts.battle;
using TeicsoftSpectacleCards.scripts.battle.card;
using TeicsoftSpectacleCards.scripts.battle.target;
using TeicsoftSpectacleCards.scripts.XmlParsing;

public partial class Battle : Node2D {
    [Signal] public delegate void GameOverEventHandler();

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

    private List<Tuple<string, Color>> _enemyDeets = new() {
        new Tuple<string, Color>("Red", new Color(0.5f, 0, 0)),
        new Tuple<string, Color>("Green", new Color(0, 0.5f, 0)),
        new Tuple<string, Color>("Blue", new Color(0, 0, 0.5f)),
    };

    public override void _Ready() {
        Dictionary<string, List<string>> decks = DeckXmlParser.ParseAllDecks();
        decks.TryGetValue("deck_player", out List<string> playerCardIds);
        InitialiseGameState(playerCardIds);
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

    private void InitialiseGameState(List<string> playerCardIds) {
        _deck = new Deck<CardSleeve>();
        _deck.Discard = _discard;
        _deck.AddCards(Deck<CardSleeve>.SleeveCards(playerCardIds.Select(CardPrototypes.CloneCard).ToList()));
        _hand = GetNode<Hand>("Hand");
        _discard = new Discard<CardSleeve>();
        _hand.Discard = _discard;
        _gameState = new GameState();
        _gameState.Hand = _hand;
        _gameState.Deck = _deck;
        _gameState.Player.PlayerHealthChangedCustomEvent += OnPlayerHealthChanged;
        _gameState.Player.PlayerDefenseLowerChangedCustomEvent += OnPlayerDefenseLowerChanged;
        _gameState.Player.PlayerDefenseUpperChangedCustomEvent += OnPlayerDefenseUpperChanged;
        _gameState.MultiplierChangedCustomEvent += OnMultiplierChanged;
        _gameState.SpectacleChangedCustomEvent += OnSpectacleChanged;
        _gameState.DiscardStateChangedCustomEvent += OnDiscardStateChanged;
        _deck.Shuffle();
        _gameState.Draw(4);
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
        _spectacleDisplay = GetNode<Label>("HUD/SpectacleDisplay");
        _multiplierDisplay = GetNode<Label>("HUD/MultiplierDisplay");
        _selectedIndicator = GetNode<ColorRect>("HUD/SelectedIndicator");
    }

    public override void _Process(double delta) { }

    private Enemy CreateEnemy(Deck<Card> enemyDeck, int i) {
        Enemy enemy = _enemyScene.Instantiate<Enemy>();
        enemy.Name = _enemyDeets[i].Item1;
        enemy.Color = _enemyDeets[i].Item2;
        _enemiesLocation.ProgressRatio = (float)i / (ENEMY_COUNT - 1);
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
        if (playerObject.Health <= 0) { EmitSignal(SignalName.GameOver); }

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

    private void OnDiscardStateChanged(object sender, IntEventArgs e) {
        GetNode<Label>("HUD/DiscardDisplay").Text = e.N == 0 ? "" : $"You must discard {e.N.ToString()} cards.";
    }

    private void OnPlayButtonPressed() { _gameState.PlaySelectedCard(); }

    private void OnDeckPressed() { _gameState.Draw(); }

    private void EndTurn() { _gameState.EndTurn(); }
}
