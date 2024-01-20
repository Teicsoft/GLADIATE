using Godot;
using System.Collections.Generic;

public partial class Card : Node2D {
    [Signal]
    public delegate void CardSelectedEventHandler(Card card);

    public Color color { get; set; }
    public Button selectButton;

    public string id { get; set; } // card_id

    public bool targetRequired { get; set; }
    public Enemy.ModifierEnum modifier { get; set; }
    public Enemy.PositionEnum targetPosition { get; set; }

    // main stats
    public int attack { get; set; }
    public int defenseLower { get; set; }
    public int defenseUpper { get; set; }
    public int health { get; set; }
    public int draw { get; set; }
    public int discard { get; set; }
    public int spectaclePoints { get; set; }

    //text
    public string name { get; set; }
    public string description { get; set; }
    public string lore { get; set; }
    public string tooltip { get; set; }

    //design
    public string imagePath { get; set; } //path to image
    public string animationPath { get; set; } //path to animation
    public string soundPath { get; set; } //path to sound

    //cache
    public Texture image { get; set; }
    public Animation animation { get; set; }
    public AudioStream sound { get; set; }

    // I'm using this to initialize the card in place of a constructor,
    // which I can't use because Godot doesn't like them with nodes supposedly
    public virtual Card Initialize(string id) {
        this.id = id;
        return this;
    }

    public virtual Card Initialize(string id, bool targetRequired = true, int attack = 0, int defenseLower = 0,
        int defenseUpper = 0, int health = 0, int draw = 0, int discard = 0, int spectaclePoints = 0, string name = "",
        string description = "", string lore = "", string tooltip = "", string imagePath = "",
        string animationPath = "", string soundPath = "") {
        this.id = id;
        this.targetRequired = targetRequired;

        this.attack = attack;
        this.defenseLower = defenseLower;
        this.defenseUpper = defenseUpper;
        this.health = health;
        this.draw = draw;
        this.discard = discard;
        this.spectaclePoints = spectaclePoints;

        this.name = name;
        this.description = description;
        this.lore = lore;
        this.tooltip = tooltip;

        this.imagePath = imagePath;
        this.animationPath = animationPath;
        this.soundPath = soundPath;
        return this;
    }

    public override void _Ready() {
        selectButton = GetNode<Button>("SelectButton");
        selectButton.Text = attack.ToString();
        selectButton.AddThemeColorOverride("font_color", this.color);
    }

    public override void _Process(double delta) { }

    public void TestSetup(int newAttack, bool targetRequired, Color color) {
        attack = newAttack;
        this.targetRequired = targetRequired;
        this.color = color;
    }

    public void Play( /*GameState gameState,*/ Enemy targetedEnemy, List<Enemy> allEnemies) {
        if (attack != 0) {
            if (targetRequired) { targetedEnemy.Damage(attack); } else {
                foreach (Enemy enemy in allEnemies) { enemy.Damage(attack); }
            }
        }

        if (defenseLower != 0) {
            // gameState.ModifyPlayerDefenseLower(DefenseLower);
        }

        if (defenseUpper != 0) {
            // gameState.ModifyPlayerDefenseUpper(DefenseUpper);
        }

        if (health != 0) {
            // gameState.ModifyPlayerHealth(Health);
        }

        if (draw > 0) {
            // swalsh TODO: Emit Event?
            // gameState.DrawCards(Health);
        }

        if (discard > 0) {
            // swalsh TODO: Emit Event?
            // swalsh TODO: Choice Discard by default, I think, but still needs an interface etc.
            // gameState.DiscardCards(Health);
        }

        if (spectaclePoints > 0) {
            // gameState.AddSpectaclePoints(SpectaclePoints);
        }

        // gameState.AddToCombo(id);
    }

    private void OnPress() {
        EmitSignal(SignalName.CardSelected, this);
    }

    public void LoadAssets() {
        LoadTexture();
        LoadAnimation();
        LoadSound();
    }

    private void LoadTexture() {
        image = (Texture)GD.Load(imagePath);
    }

    private void LoadAnimation() {
        animation = (Animation)GD.Load(animationPath);
    }

    private void LoadSound() {
        sound = (AudioStream)GD.Load(soundPath);
    }

    public Card Clone()
    {
        Card card = new Card();

        card.Initialize(id, targetRequired, attack, defenseLower, defenseUpper, health, draw, discard, spectaclePoints,
            name, description, lore, tooltip, imagePath, animationPath, soundPath);
        card.color = new Color(this.color.R, this.color.G, this.color.B);
        return card;
    }
}
