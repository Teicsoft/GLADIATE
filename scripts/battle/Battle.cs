using System;
using System.Collections.Generic;
using System.Linq;
using GLADIATE.scripts.audio;
using GLADIATE.scripts.battle.card;
using GLADIATE.scripts.battle.target;
using GLADIATE.scripts.XmlParsing;
using Godot;

namespace GLADIATE.scripts.battle;

public partial class Battle : Node2D {
    [Signal] public delegate void BattleLostEventHandler();
    [Signal] public delegate void BattleWonEventHandler(Player player);

    [Export] private PackedScene _cardScene;
    [Export] private PackedScene _enemyScene;
    private GameState _gameState;

    private List<TextureRect> _comboArts = new();

    private List<Enemy> _allEnemies;
    private Dictionary<string, List<string>> _allDecks;

    public string Id { get; set; }
    public string BattleName { get; set; }
    public string Music { get; set; }

    AudioEngine audioEngine;
    autoloads.SceneLoader sceneLoader;

    public override void _Ready() {
        audioEngine = GetNode<AudioEngine>("/root/audio_engine");
        sceneLoader = GetNode<autoloads.SceneLoader>("/root/SceneLoader");

        _allEnemies = EnemyXmlParser.ParseAllEnemies();
        _allDecks = DeckXmlParser.ParseAllDecks();
        _allDecks.TryGetValue(sceneLoader.DeckSelected, out List<string> playerCardIds);

        Dictionary<string, dynamic> battleData = sceneLoader.GetCurrentBattleData();
        Id = battleData["battle_id"];
        BattleName = battleData["battle_name"];
        Music = battleData["music"];
        List<Enemy> enemies = CreateEnemies((List<string>)battleData["enemies"]);

        InitialiseGameState(playerCardIds, enemies);
        InitialiseHud();
        
        ComboGlossary comboGlossary = GetNode<ComboGlossary>("HUD/ComboGlossary");
        comboGlossary.Initialize(_gameState.Hand.Deck);

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

        foreach (Enemy enemy in _gameState.Enemies) {
            enemy.EnemyHealthChangedCustomEvent += OnEnemyHealthChanged;
            enemy.EnemyDefenseLowerChangedCustomEvent += OnEnemyDefenseLowerChanged;
            enemy.EnemyDefenseUpperChangedCustomEvent += OnEnemyDefenseUpperChanged;
        }

        _gameState.SelectedEnemyIndexChangedCustomEvent += MoveSelectedIndicator;
        _gameState.MultiplierChangedCustomEvent += OnMultiplierChanged;
        _gameState.SpectacleChangedCustomEvent += OnSpectacleChanged;
        _gameState.DiscardStateChangedCustomEvent += OnDiscardStateChanged;
        _gameState.ComboStackChangedCustomEvent += OnComboStackChanged;
        _gameState.AllEnemiesDefeatedCustomEvent += WinBattle;
        _gameState.ComboPlayedCustomEvent += DisplayPlayedCombo;
        _gameState.Hand.Deck.DeckShuffledCustomEvent += OnDeckShuffled;
        _gameState.Player.PlayerModifierChangedCustomEvent += OnPlayerMofifierChanged;


        _gameState.Draw(4);
        if (sceneLoader.Health != 0) { _gameState.Player.Health = sceneLoader.Health; }
        sceneLoader.i += 1;
        if (sceneLoader.SpectaclePoints != 0) { _gameState.SpectaclePoints = sceneLoader.SpectaclePoints; }
    }

    private void OnPlayerMofifierChanged(object sender, EventArgs e) {
        TextureRect PlayerModifierIcon = GetNode<TextureRect>("HUD/PlayerModifierIcon");
        if (_gameState.Player.Modifier == Utils.ModifierEnum.None) { PlayerModifierIcon.Visible = false; } else {
            PlayerModifierIcon.Texture =
                (Texture2D)GD.Load($"res://assets/images/ModifierIcons/{_gameState.Player.Modifier}.png");
            PlayerModifierIcon.Visible = true;
        }
    }

    private List<Enemy> CreateEnemies(List<string> enemyIds) {
        int idsCount = enemyIds.Count;
        List<Enemy> enemies = new();
        for (int i = 0; i < idsCount; i++) {
            Enemy enemy = _enemyScene.Instantiate<Enemy>();
            _allEnemies.First(e => e.Id == enemyIds[i]).CloneTo(enemy);

            AssignRandomColorDEBUG(enemy);
            enemy.Deck = GetEnemyDeck(enemy.DeckId);
            enemy.Position = GetEnemyPosition(i, idsCount);
            enemy.EnemySelected += MoveSelectedIndicator;
            
            if (GD.Randi() % 2 == 0) {
                GD.Print(i.ToString());
                enemy.GetNode<Sprite2D>("EnemySprite").FlipH = false;
            }
            else
            {
                GD.Print(i.ToString());
            }
            
            AddChild(enemy);
            enemies.Add(enemy);
        }
        return enemies;
    }

    private static void AssignRandomColorDEBUG(Enemy enemy) {
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

    private void WinBattle(object sender, EventArgs eventArgs) {
        EmitSignal(Battle.SignalName.BattleWon, _gameState.Player);

        audioEngine.PlaySoundFx("victory-jingle.wav");
        GD.Print(" ==== ====  WIN BATTLE  ==== ====");
        sceneLoader.SpectaclePoints += _gameState.SpectaclePoints;
        sceneLoader.Health = _gameState.Player.Health;
        sceneLoader.GoToNextBattle();
    }

    private void MoveSelectedIndicator(Enemy enemy) {
        GetNode<ColorRect>("HUD/SelectedIndicator").Position =
            _gameState.GetSelectedEnemy()?.Position ?? new Vector2(-100, -100);
    }

    private void MoveSelectedIndicator(object sender, EventArgs e) {
        GetNode<ColorRect>("HUD/SelectedIndicator").Position =
            _gameState.GetSelectedEnemy()?.Position ?? new Vector2(-100, -100);
    }

    private void OnPlayerHealthChanged() {
        Player playerObject = _gameState.Player;
        if (playerObject.Health <= 0) {
            EmitSignal(Battle.SignalName.BattleLost);

            sceneLoader = GetNode<autoloads.SceneLoader>("/root/SceneLoader");
            audioEngine.PlayMusic("Lil_tune.wav");
            sceneLoader.GoToScene("res://scenes/sub/GameOver.tscn");
        }
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

    private void OnEnemyHealthChanged(object sender, EventArgs e) {
        //todo Particle effect control here
        Utils.DirectionEventArgs directionEventArgs = (Utils.DirectionEventArgs)e;

        // GD.Print("Enemy health went " + directionEventArgs.Direction);
    }

    private void OnEnemyDefenseUpperChanged(object sender, EventArgs e) {
        //todo Particle effect control here
        Utils.DirectionEventArgs directionEventArgs = (Utils.DirectionEventArgs)e;

        // GD.Print("Enemy Upper Defense value went " + directionEventArgs.Direction);
    }

    private void OnEnemyDefenseLowerChanged(object sender, EventArgs e) {
        //todo Particle effect control here
        Utils.DirectionEventArgs directionEventArgs = (Utils.DirectionEventArgs)e;

        // GD.Print("Enemy Lower Defense value went " + directionEventArgs.Direction);
    }

    private void OnMultiplierChanged() {
        GetNode<Label>("HUD/MultiplierDisplay").Text = _gameState.Multiplier.ToString();
    }

    private void OnSpectacleChanged() {
        GetNode<Label>("HUD/SpectacleDisplay").Text = _gameState.SpectaclePoints.ToString();
    }

    private void OnDiscardStateChanged() {
        GetNode<Label>("HUD/DiscardDisplay").Text = _gameState.Discards == 0
            ? ""
            : $"You must discard {_gameState.Discards} card{(_gameState.Discards == 1 ? "" : "s")}.";
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

    private void DisplayPlayedCombo(object sender, ComboEventArgs e) {
        GetNode<Label>("HUD/ComboDisplay").Text = $"C-C-C-COMBO!!! {e.Combo.Name}!";
        GetNode<Timer>("HUD/ComboDisplay/ComboDisplayTimer").Start();
    }

    private void OnComboDisplayTimeout() { GetNode<Label>("HUD/ComboDisplay").Text = ""; }

    private void OnPlayerHealthChanged(object sender, EventArgs e) {
        Utils.DirectionEventArgs directionEventArgs = (Utils.DirectionEventArgs)e;

        // GD.Print("Player health went " + directionEventArgs.Direction);

        OnPlayerHealthChanged();
    }

    private void OnPlayerDefenseUpperChanged(object sender, EventArgs e) {
        Utils.DirectionEventArgs directionEventArgs = (Utils.DirectionEventArgs)e;

        // GD.Print("Player Upper Defense value went " + directionEventArgs.Direction);

        OnPlayerDefenseUpperChanged();
    }

    private void OnPlayerDefenseLowerChanged(object sender, EventArgs e) {
        Utils.DirectionEventArgs directionEventArgs = (Utils.DirectionEventArgs)e;

        // GD.Print("Player Lower Defense value went " + directionEventArgs.Direction);

        OnPlayerDefenseLowerChanged();
    }

    private void OnMultiplierChanged(object sender, EventArgs e) {
        Utils.DirectionEventArgs directionEventArgs = (Utils.DirectionEventArgs)e;

        // GD.Print("Multiplier value went " + directionEventArgs.Direction);

        OnMultiplierChanged();
    }

    private void OnSpectacleChanged(object sender, EventArgs e) {
        Utils.DirectionEventArgs directionEventArgs = (Utils.DirectionEventArgs)e;

        // GD.Print("Spectacle Points value went " + directionEventArgs.Direction);

        OnSpectacleChanged();
    }

    private void OnDiscardStateChanged(object sender, EventArgs e) { OnDiscardStateChanged(); }
    private void OnDeckShuffled(object sender, EventArgs e) { GD.Print(" Deck Shuffled "); }
    public override string ToString() { return $"Battle: {BattleName}({Id})"; }
}
