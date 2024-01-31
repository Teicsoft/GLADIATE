using System;
using Godot;
using System.Collections.Generic;
using System.Linq;
using TeicsoftSpectacleCards.scripts.battle;
using TeicsoftSpectacleCards.scripts.battle.card;
using TeicsoftSpectacleCards.scripts.battle.target;
using TeicsoftSpectacleCards.scripts.XmlParsing;
using Utils = TeicsoftSpectacleCards.scripts.battle.Utils;

public partial class Battle : Node2D {
    [Signal] public delegate void BattleLostEventHandler();
    [Signal] public delegate void BattleWonEventHandler(Player player);

    [Export] private PackedScene _cardScene;
    [Export] private PackedScene _enemyScene;
    private GameState _gameState;

    private List<TextureRect> _comboArts = new();

    private List<Enemy> _allEnemies = EnemyXmlParser.ParseAllEnemies();
    private Dictionary<string, List<string>> _allDecks = DeckXmlParser.ParseAllDecks();

    public override void _Ready() {
        // TODO: Change to accepting a player deck.
        _allDecks.TryGetValue("deck_player", out List<string> playerCardIds);
        // TODO: This battle's enemy IDs go here
        List<string> enemyIds = new();
        List<Enemy> enemies = CreateEnemies(enemyIds);
        InitialiseGameState(playerCardIds, enemies);
        InitialiseHud();


        GD.Print(" ==== ==== START GAME ==== ====");
    }

    public override void _Process(double delta) { }

    private void InitialiseGameState(List<string> playerCardIds, List<Enemy> enemies) {
        Hand hand = GetNode<Hand>("Hand");
        hand.InitialiseDeck(playerCardIds);
        _gameState = new GameState(hand, enemies);
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

    private List<Enemy> CreateEnemies(List<string> enemyIds) {
        int idsCount = enemyIds.Count;
        List<Enemy> enemies = new();
        for (int i = 0; i < idsCount; i++) {
            Enemy enemy = _enemyScene.Instantiate<Enemy>();
            _allEnemies.First(e => e.Id == enemyIds[i]).CloneTo(enemy);
            switch (GD.Randi() % 2) {
                case 0:
                    enemy.Color = new Color(0.5f, 0, 0);
                    break;
                case 1:
                    enemy.Color = new Color(0, 0.5f, 0);
                    break;
                case 2:
                    enemy.Color = new Color(0, 0, 0.5f);
                    break;
            }
            enemy.Deck = GetEnemyDeck(enemy.DeckId);
            enemy.Position = GetEnemyPosition(i, idsCount);
            enemy.EnemySelected += MoveSelectedIndicator;
            AddChild(enemy);
            enemies.Add(enemy);
        }
        return enemies;
    }

    private Deck<Card> GetEnemyDeck(string deckId) {
        Deck<Card> enemyDeck = new(new());

        // _allDecks.TryGetValue("deck_enemy", out List<string> enemyCardIds);
        _allDecks.TryGetValue(deckId, out List<string> enemyCardIds);
        enemyDeck.AddCards(enemyCardIds.Select(CardPrototypes.CloneCard).ToList());
        enemyDeck.Shuffle();
        return enemyDeck;
    }

    private Vector2 GetEnemyPosition(int index, int count) {
        PathFollow2D enemiesLocation = GetNode<PathFollow2D>("Enemies/EnemiesLocation");
        enemiesLocation.ProgressRatio = (float)index / (count - 1);
        return enemiesLocation.Position;
    }

    private void InitialiseHud() {
        OnPlayerHealthChanged();
        OnPlayerDefenseUpperChanged();
        OnPlayerDefenseLowerChanged();
        OnMultiplierChanged();
        OnSpectacleChanged();
    }

    private void OnPlayButtonPressed() { _gameState.PlaySelectedCard(); }
    private void EndTurn() { _gameState.EndTurn(); }
    private void WinBattle(object sender, EventArgs eventArgs) { EmitSignal(SignalName.BattleWon, _gameState.Player); }

    private void MoveSelectedIndicator(Enemy enemy) {
        GetNode<ColorRect>("HUD/SelectedIndicator").Position =
            enemy.Position != GetNode<ColorRect>("HUD/SelectedIndicator").Position
                ? enemy.Position
                : new Vector2(-100, 520);
    }

    private void OnPlayerHealthChanged() {
        Player playerObject = _gameState.Player;
        if (playerObject.Health <= 0) { EmitSignal(SignalName.BattleLost); }
        GetNode<Label>("HUD/PlayerHealthDisplay").Text = playerObject.Health + "/" + playerObject.MaxHealth;
        GetNode<ProgressBar>("HUD/PlayerHealthProgressBar").Ratio =
            (double)playerObject.Health / playerObject.MaxHealth;
    }

    private void OnPlayerDefenseUpperChanged() {
        GetNode<ColorRect>("HUD/PlayerUpperBlockRect").Color =
            new Color(0, 0, _gameState.Player.DefenseUpper > 0 ? 1 : 0);
        GetNode<Label>("HUD/PlayerUpperBlockDisplay").Text = _gameState.Player.DefenseUpper.ToString();
    }

    private void OnPlayerDefenseLowerChanged() {
        GetNode<ColorRect>("HUD/PlayerLowerBlockRect").Color =
            new Color(0, 0, _gameState.Player.DefenseLower > 0 ? 1 : 0);
        GetNode<Label>("HUD/PlayerLowerBlockDisplay").Text = _gameState.Player.DefenseLower.ToString();
    }

    private void OnMultiplierChanged() {
        GetNode<Label>("HUD/MultiplierDisplay").Text = _gameState.Multiplier.ToString();
    }

    private void OnSpectacleChanged() {
        GetNode<Label>("HUD/SpectacleDisplay").Text = _gameState.SpectaclePoints.ToString();
    }

    private void OnDiscardStateChanged() {
        GetNode<Label>("HUD/DiscardDisplay").Text =
            _gameState.Discards == 0 ? "" : $"You must discard {_gameState.Discards} cards.";
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
                TextureRect art = Utils.LoadCardArt(_gameState.ComboStack[i]);
                art.Position = comboStackLocation.Position;
                _comboArts.Add(art);
                AddChild(art);
            }
        }
    }

    private void OnPlayerHealthChanged(object sender, EventArgs e) { OnPlayerHealthChanged(); }
    private void OnPlayerDefenseUpperChanged(object sender, EventArgs e) { OnPlayerDefenseUpperChanged(); }
    private void OnPlayerDefenseLowerChanged(object sender, EventArgs e) { OnPlayerDefenseLowerChanged(); }
    private void OnMultiplierChanged(object sender, EventArgs e) { OnMultiplierChanged(); }
    private void OnSpectacleChanged(object sender, EventArgs e) { OnSpectacleChanged(); }
    private void OnDiscardStateChanged(object sender, EventArgs e) { OnDiscardStateChanged(); }
}
