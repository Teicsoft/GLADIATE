using System;
using Godot;
using System.Collections.Generic;
using System.Linq;
using TeicsoftSpectacleCards.scripts.battle;
using TeicsoftSpectacleCards.scripts.battle.card;
using TeicsoftSpectacleCards.scripts.battle.target;
using TeicsoftSpectacleCards.scripts.XmlParsing;

public partial class Battle : Node2D {
    [Signal] public delegate void BattleLostEventHandler();
    [Signal] public delegate void BattleWonEventHandler(Player player);

    [Export] private PackedScene _cardScene;
    [Export] private PackedScene _enemyScene;
    private GameState _gameState;

    private List<TextureRect> _comboArts = new();

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

        decks.TryGetValue("deck_enemy", out List<string> enemyCardIds);
        foreach (int i in Enumerable.Range(0, ENEMY_COUNT)) {
            Enemy enemy = CreateEnemy(GetEnemyDeck(enemyCardIds), i);
            AddChild(enemy);
            _gameState.Enemies.Add(enemy);
        }

        GD.Print(" ==== ==== START GAME ==== ====");
    }

    public override void _Process(double delta) { }

    private void InitialiseGameState(List<string> playerCardIds) {
        Hand hand = GetNode<Hand>("Hand");
        hand.InitialiseDeck(playerCardIds);
        _gameState = new GameState(hand);
        _gameState.Player.PlayerHealthChangedCustomEvent += OnPlayerHealthChanged;
        _gameState.Player.PlayerDefenseLowerChangedCustomEvent += OnPlayerDefenseLowerChanged;
        _gameState.Player.PlayerDefenseUpperChangedCustomEvent += OnPlayerDefenseUpperChanged;
        _gameState.MultiplierChangedCustomEvent += OnMultiplierChanged;
        _gameState.SpectacleChangedCustomEvent += OnSpectacleChanged;
        _gameState.DiscardStateChangedCustomEvent += OnDiscardStateChanged;
        _gameState.ComboStackChangedCustomEvent += OnComboStackChanged;
        _gameState.AllEnemiesDefeatedCustomEvent += WinBattle;
        _gameState.Draw(4);
    }

    private void InitialiseHud() {
        OnPlayerHealthChanged();
        OnPlayerDefenseUpperChanged();
        OnPlayerDefenseLowerChanged();
        OnMultiplierChanged();
        OnSpectacleChanged();
    }

    private Enemy CreateEnemy(Deck<Card> enemyDeck, int i) {
        PathFollow2D enemiesLocation = GetNode<PathFollow2D>("Enemies/EnemiesLocation");
        Enemy enemy = _enemyScene.Instantiate<Enemy>();
        enemy.Name = _enemyDeets[i].Item1;
        enemy.Color = _enemyDeets[i].Item2;
        enemiesLocation.ProgressRatio = (float)i / (ENEMY_COUNT - 1);
        enemy.Position = enemiesLocation.Position;
        enemy.Deck = enemyDeck;
        enemy.EnemySelected += _gameState.SelectEnemy;
        enemy.EnemySelected += MoveSelectedIndicator;
        return enemy;
    }

    private static Deck<Card> GetEnemyDeck(List<string> enemyCardIds) {
        Deck<Card> enemyDeck = new(new ());
        enemyDeck.AddCards(enemyCardIds.Select(CardPrototypes.CloneCard).ToList());
        enemyDeck.Shuffle();
        return enemyDeck;
    }

    private void MoveSelectedIndicator(Enemy enemy) {
        GetNode<ColorRect>("HUD/SelectedIndicator").Position =
            enemy.Position != GetNode<ColorRect>("HUD/SelectedIndicator").Position ? enemy.Position : new Vector2(-100, 520);
    }

    private void OnPlayerHealthChanged(object sender, EventArgs e) { OnPlayerHealthChanged(); }

    private void OnPlayerHealthChanged() {
        Player playerObject = _gameState.Player;
        if (playerObject.Health <= 0) { EmitSignal(SignalName.BattleLost); }
        GetNode<Label>("HUD/PlayerHealthDisplay").Text = playerObject.Health + "/" + playerObject.MaxHealth;
        GetNode<ProgressBar>("HUD/PlayerHealthProgressBar").Ratio = (double)playerObject.Health / playerObject.MaxHealth;
    }

    private void OnPlayerDefenseUpperChanged(object sender, EventArgs e) { OnPlayerDefenseUpperChanged(); }

    private void OnPlayerDefenseUpperChanged() {
        GetNode<ColorRect>("HUD/PlayerUpperBlockRect").Color = new Color(0, 0, _gameState.Player.DefenseUpper > 0 ? 1 : 0);
        GetNode<Label>("HUD/PlayerUpperBlockDisplay").Text = _gameState.Player.DefenseUpper.ToString();
    }

    private void OnPlayerDefenseLowerChanged(object sender, EventArgs e) { OnPlayerDefenseLowerChanged(); }

    private void OnPlayerDefenseLowerChanged() {
        GetNode<ColorRect>("HUD/PlayerLowerBlockRect").Color = new Color(0, 0, _gameState.Player.DefenseLower > 0 ? 1 : 0);
        GetNode<Label>("HUD/PlayerLowerBlockDisplay").Text = _gameState.Player.DefenseLower.ToString();
    }

    private void OnMultiplierChanged(object sender, EventArgs e) { OnMultiplierChanged(); }

    private void OnMultiplierChanged() { GetNode<Label>("HUD/MultiplierDisplay").Text = _gameState.Multiplier.ToString(); }

    private void OnSpectacleChanged(object sender, EventArgs e) { OnSpectacleChanged(); }

    private void OnSpectacleChanged() { GetNode<Label>("HUD/SpectacleDisplay").Text = _gameState.SpectaclePoints.ToString(); }

    private void OnDiscardStateChanged(object sender, EventArgs e) { OnDiscardStateChanged(); }

    private void OnDiscardStateChanged() {
        string discardText = _gameState.Discards == 0 ? "" : $"You must discard {_gameState.Discards} cards.";
        GetNode<Label>("HUD/DiscardDisplay").Text = discardText;
    }

    private void OnComboStackChanged(object sender, EventArgs e) {
        PathFollow2D comboStackLocation = GetNode<PathFollow2D>("ComboStack/ComboStackLocation");

        _comboArts.ForEach(art => art.QueueFree());
        _comboArts.Clear();

        int stackSize = _gameState.ComboStack.Count;
        if (stackSize != 0) {
            float stepSize = 1f / Math.Max(stackSize, 7);
            for (int i = 0; i < stackSize; i++) {
                comboStackLocation.ProgressRatio = i * stepSize;
                TextureRect art = LoadArt(_gameState.ComboStack[i]);
                art.Position = comboStackLocation.Position;
                _comboArts.Add(art);
                AddChild(art);
            }
        }
    }

    private TextureRect LoadArt(Card card) {
        TextureRect art = new();
        Texture2D texture = (Texture2D)GD.Load(card.ImagePath);
        art.Texture = texture;

        float ratio = 160 / texture.GetSize().X;
        art.Scale = new Vector2(ratio, ratio);
        return art;
    }

    private void OnPlayButtonPressed() { _gameState.PlaySelectedCard(); }

    private void OnDeckPressed() { _gameState.Draw(); }

    private void EndTurn() { _gameState.EndTurn(); }

    private void WinBattle(object sender, EventArgs eventArgs) { EmitSignal(SignalName.BattleWon, _gameState.Player); }
}
