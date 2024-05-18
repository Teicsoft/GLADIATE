using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GLADIATE.scripts.audio;
using GLADIATE.scripts.autoloads;
using GLADIATE.scripts.battle.card;
using GLADIATE.scripts.battle.target;
using GLADIATE.scripts.XmlParsing;
using Godot;
using Godot.Collections;
using Array = Godot.Collections.Array;

namespace GLADIATE.scripts.battle;

public partial class Battle : Control {
    [Signal] public delegate void BattleLostEventHandler();
    [Signal] public delegate void BattleWonEventHandler(Player player);

    [Export] private PackedScene _cardScene;
    [Export] private PackedScene _enemyScene;
    private GameState _gameState;

    private List<TextureRect> _comboArts = new();

    private List<Enemy> _allEnemies;
    private System.Collections.Generic.Dictionary<string, List<string>> _allDecks;

    public string Id { get; set; }
    public string BattleName { get; set; }
    public string Music { get; set; }
    
    AudioEngine _audioEngine;
    autoloads.SceneLoader _sceneLoader;

    public override void _Ready() {
        _audioEngine = GetNode<AudioEngine>("/root/audio_engine");
        _sceneLoader = GetNode<autoloads.SceneLoader>("/root/SceneLoader");
        
        _allEnemies = EnemyXmlParser.ParseAllEnemies();
        _allDecks = DeckXmlParser.ParseAllDecks();
        _allDecks.TryGetValue(_sceneLoader.DeckSelected, out List<string> playerCardIds);

        System.Collections.Generic.Dictionary<string, dynamic> battleData = _sceneLoader.GetCurrentBattleData();
        Id = battleData["battle_id"];
        BattleName = battleData["battle_name"];
        Music = battleData["music"];
        List<Enemy> enemies = CreateEnemies((List<string>)battleData["enemies"]);
        
        InitialiseGameState(playerCardIds, enemies);
        InitialiseHud();

        ComboGlossary comboGlossary = GetNode<ComboGlossary>("HUD/ComboGlossary");
        comboGlossary.Initialize(_gameState.Hand.Deck, _gameState.AllCombos);
        
        GetNode<Label>("HUD/VsLabel").Text = BattleName;
        
        if (Id == SceneLoader.BossBattleId) {
            _audioEngine.PlayMusic("Menu_music.wav");
            GD.Print("Boss Battle");
            
            GetNode<ColorRect>("Background/TextureRect/ColorRect").Show();
        }
        GD.Print(" ==== ==== START GAME ==== ====");
        
        
        //UIScaling();
        UIScaling();
    }

    public override void _Process(double delta) {
        if (SceneLoader.BossBattleId == Id) {
            foreach (Enemy enemy in _gameState.Enemies) {
                PathFollow2D enemyPathFollow2D = enemy.EnemyPath2D.GetNode<PathFollow2D>("EnemyLocation");
                
                if (enemyPathFollow2D.ProgressRatio <= 0.7f) {
                    enemyPathFollow2D.ProgressRatio += 0.5f * (float)delta;

                    enemy.Position = enemyPathFollow2D.Position + new Vector2(960, 540);
                    
                }
                else if (enemyPathFollow2D.ProgressRatio > 0.7f && enemyPathFollow2D.ProgressRatio <= 1.0f) {
                    if (GD.Randi() % 100 == 0)
                    {
                        enemyPathFollow2D.ProgressRatio += (float)GD.RandRange(-0.3f, 0.3f) * (float)delta*5;
                        enemy.Position = enemyPathFollow2D.Position + new Vector2(960, 540);
                    }
                }
                else {
                    enemyPathFollow2D.ProgressRatio = 1.0f;
                    enemy.Position = enemyPathFollow2D.Position + new Vector2(960, 540);
                }
            }
        }
        
        //UIScaling();
        UIScaling();
    }

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
        _gameState.Player.Statuses.PlayerStatusesChangedCustomEvent += UpdateStatusesToolTip;


        _gameState.Draw(4);
        if (_sceneLoader.Health != 0) { _gameState.Player.Health = _sceneLoader.Health; }
        _sceneLoader.i += 1;
        if (_sceneLoader.SpectaclePoints != 0) { _gameState.SpectaclePoints = _sceneLoader.SpectaclePoints; }
    }
    
    public void UpdateStatusesToolTip(object sender, EventArgs e)
    {
           
        TextureRect statusIndicator = GetNode<TextureRect>("HUD/StatusIndicator");
        string statusString = "";
        
        foreach (Utils.StatusEnum status in _gameState.Player.Statuses) { statusString += status + "\n"; }
        statusIndicator.TooltipText = statusString;
        if (statusString.Length > 0) {statusIndicator.Show();} else { statusIndicator.Hide();}
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
            
            Path2D enemyPath2d = GetEnemyPosition(i, idsCount);
            PathFollow2D enemyPathFollow2d = enemyPath2d.GetNode<PathFollow2D>("EnemyLocation");
            enemy.Position = enemyPathFollow2d.Position;
            enemy.EnemyPath2D = enemyPath2d;
            enemy.EnemySelected += MoveSelectedIndicator;

            if (GD.Randi() % 2 == 0) { enemy.GetNode<Sprite2D>("EnemySprite").FlipH = false; }

            if (Id == SceneLoader.PreBossBattleId && enemy.Id == "enemy_Goon") {
                enemy.GetNode<Sprite2D>("EnemySprite").Hide();
                AnimatedSprite2D goon = enemy.GetNode<AnimatedSprite2D>("Goon");
                goon.Show();
                goon.Play("Idle");
                enemy.Scale = new Vector2(1.7f, 1.7f);
                enemy.GetNode<Label>("HealthBar/CardPlayed").Position = new Vector2(100, 0);
            }
            
            AddChild(enemy);
            enemies.Add(enemy);
        }
        
        
        if (Id == SceneLoader.BossBattleId) {
            GetNode<PanelContainer>("BossHealthBarsPanel").Visible = true;
            
            foreach (Enemy enemy in enemies) {
                enemy.GetNode<Control>("HealthBar").Visible = false;
                
                PanelContainer bossScene = GD.Load<PackedScene>("res://scenes/battle/boss_health_bars.tscn").Instantiate<PanelContainer>();
                GetNode<VBoxContainer>("BossHealthBarsPanel/BossHealthBarsVBoxContainer").AddChild(bossScene);
                enemy.BossHealthBar = bossScene;
            }
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
        _allDecks.TryGetValue(deckId, out List<string> enemyCardIds);
        enemyDeck.AddCards(enemyCardIds.Select(CardFactory.CloneCard).ToList());
        enemyDeck.Shuffle();
        return enemyDeck;
    }

    private Path2D GetEnemyPosition(int index, int count) {
        if (Id == SceneLoader.BossBattleId)
        {
            GetNode<PanelContainer>("BossHealthBarsPanel").Visible = true;
            
            Path2D BossPath2D = GetNode<Path2D>("BossNode/BossBattle" + index);
            PathFollow2D bossLocation = BossPath2D.GetNode<PathFollow2D>("EnemyLocation");

            bossLocation.ProgressRatio = 0;
            bossLocation.Transform = BossPath2D.Transform;
            
            return BossPath2D;
        }
        Path2D enemyNode = GetNode<Path2D>("Enemies");
        PathFollow2D enemyLocation = enemyNode.GetNode<PathFollow2D>("EnemyLocation");
        enemyLocation.ProgressRatio = (float)index / (count - 1);
        return enemyNode;
    }

    private void InitialiseHud() {
        OnPlayerHealthChanged();
        OnPlayerDefenseUpperChanged();
        OnPlayerDefenseLowerChanged();
        OnMultiplierChanged();
        OnSpectacleChanged();
    }

    private void OnPlayButtonPressed() { 
        
        _gameState.PlaySelectedCard(); 
        _audioEngine.PlaySoundFx("drawn-card.ogg");
    }


    private void EndTurn() { _gameState.EndTurn(); }

    private void WinBattle(object sender, EventArgs eventArgs) {
        EmitSignal(Battle.SignalName.BattleWon, _gameState.Player);

        if (Id != SceneLoader.PreBossBattleId)
        {_audioEngine.PlaySoundFx("victory-jingle.wav"); }
        else {
            _audioEngine.PlaySoundFx("male-hurt-1.ogg");
            Task.Delay(10).Wait();
            _audioEngine.PlaySoundFx("male-hurt-2.ogg");
            Task.Delay(10).Wait();
            _audioEngine.PlaySoundFx("male-hurt-3.ogg");
            Task.Delay(10).Wait();
            _audioEngine.PlaySoundFx("male-hurt-4.ogg");
            Task.Delay(10).Wait();
        }

        GD.Print(" ==== ====  WIN BATTLE  ==== ====");
        _sceneLoader.SpectaclePoints += _gameState.SpectaclePoints;
        _sceneLoader.Health = _gameState.Player.Health;
        _sceneLoader.GoToNextBattle();
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

            _sceneLoader = GetNode<autoloads.SceneLoader>("/root/SceneLoader");
            _audioEngine.PlayMusic("Lil_tune.wav");
            _sceneLoader.GoToScene("res://scenes/sub/GameOver.tscn");
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
        GetNode<Label>("HUD/Sp_combo Background/HBoxContainer/MultiplierDisplay").Text = _gameState.Multiplier.ToString();
    }

    private void OnSpectacleChanged() {
        GetNode<Label>("HUD/Sp_combo Background/HBoxContainer2/SpectacleDisplay").Text = _gameState.SpectaclePoints.ToString();
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
    
    
    /// 
    // This is a quick and dirty patch to give us a scaling UI, also see HUD.cs. 
    // Ideally I wouldn't want to define all of the UI elements in code, but just want to make the change fast
    /// 
    

    
    private void UIScaling() {

        Vector2 originalViewportSize = new Vector2(1920, 1080);
        Vector2 currentViewportSize = GetViewport().GetVisibleRect().Size;
        
        Vector2 scaleFactor = GetViewport().GetVisibleRect().Size / originalViewportSize;
        Vector2 offsetFactor = originalViewportSize - currentViewportSize;

        Path2D hand = GetNode<Path2D>("Hand");
        hand.Scale = scaleFactor;
        // hand.Position = new Vector2(hand.Position.X - offsetFactor.X, hand.Position.Y - offsetFactor.Y);
        Path2D enemies = GetNode<Path2D>("Enemies");
        enemies.Scale = scaleFactor;
        Path2D comboStack = GetNode<Path2D>("ComboStack");
        comboStack.Scale = scaleFactor;
        comboStack.Position = new Vector2(comboStack.Position.X - offsetFactor.X, comboStack.Position.Y - offsetFactor.Y);
        // Path2D bossNode = GetNode<Path2D>("BossNode");
        // bossNode.Scale = scaleFactor;
        
    }
    
}
