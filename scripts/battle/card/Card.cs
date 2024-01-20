using Godot;
using System.Collections.Generic;
using TeicsoftSpectacleCards.scripts.battle;

public partial class Card : Node2D {
    [Signal]
    public delegate void CardSelectedEventHandler(Card card);

    public Color color { get; set; }
    public Button selectButton;

    public string Id { get; set; } // card_id

    public bool TargetRequired { get; set; }
    public Enemy.ModifierEnum Modifier { get; set; }
    public Enemy.PositionEnum TargetPosition { get; set; }

    // main stats
    public int Attack { get; set; }
    public int DefenseLower { get; set; }
    public int DefenseUpper { get; set; }
    public int Health { get; set; }
    public int CardDraw { get; set; }
    public int Discard { get; set; }
    public int SpectaclePoints { get; set; }

    //text
    public string CardName { get; set; }
    public string Description { get; set; }
    public string Lore { get; set; }
    public string Tooltip { get; set; }

    //design
    public string ImagePath { get; set; } //path to image
    public string AnimationPath { get; set; } //path to animation
    public string SoundPath { get; set; } //path to sound

    //cache
    public Texture Image { get; set; }
    public Animation Animation { get; set; }
    public AudioStream Sound { get; set; }

    // I'm using this to initialize the card in place of a constructor,
    // which I can't use because Godot doesn't like them with nodes supposedly
    public virtual Card Initialize(string id) {
        this.Id = id;
        return this;
    }

    public virtual Card Initialize(string id, bool targetRequired = true, int attack = 0, int defenseLower = 0,
        int defenseUpper = 0, int health = 0, int draw = 0, int discard = 0, int spectaclePoints = 0, string name = "",
        string description = "", string lore = "", string tooltip = "", string imagePath = "",
        string animationPath = "", string soundPath = "") {
        this.Id = id;
        this.TargetRequired = targetRequired;

        this.Attack = attack;
        this.DefenseLower = defenseLower;
        this.DefenseUpper = defenseUpper;
        this.Health = health;
        this.CardDraw = draw;
        this.Discard = discard;
        this.SpectaclePoints = spectaclePoints;

        this.CardName = name;
        this.Description = description;
        this.Lore = lore;
        this.Tooltip = tooltip;

        this.ImagePath = imagePath;
        this.AnimationPath = animationPath;
        this.SoundPath = soundPath;
        return this;
        
    }

    public override void _Ready() {
        selectButton = GetNode<Button>("SelectButton");
        selectButton.Text = Attack.ToString();
        selectButton.AddThemeColorOverride("font_color", this.color);
    }

    public override void _Process(double delta) { }

    public void TestSetup(int newAttack, bool targetRequired, Color color) {
        Attack = newAttack;
        this.TargetRequired = targetRequired;
        this.color = color;
    }

    public void Play( GameState gameState) {
        if (Attack != 0) {
            if (TargetRequired) { gameState.GetSelectedEnemy().Damage(Attack); } else {
                foreach (Enemy enemy in gameState.enemies) { enemy.Damage(Attack); }
            }
        }

        if (DefenseLower != 0) {
            // gameState.ModifyPlayerDefenseLower(DefenseLower);
        }

        if (DefenseUpper != 0) {
            // gameState.ModifyPlayerDefenseUpper(DefenseUpper);
        }

        if (Health != 0) {
            gameState.HealPlayer(Health);
        }

        if (CardDraw > 0) {
            gameState.Draw(CardDraw);
        }

        if (Discard > 0) {
            // swalsh TODO: Emit Event?
            // swalsh TODO: Choice Discard by default, I think, but still needs an interface etc.
            // gameState.DiscardCards(Health);
        }

        gameState.ComboCheck(this);
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
        Image = (Texture)GD.Load(ImagePath);
    }

    private void LoadAnimation() {
        Animation = (Animation)GD.Load(AnimationPath);
    }

    private void LoadSound() {
        Sound = (AudioStream)GD.Load(SoundPath);
    }

    public Card Clone()
    {
        Card card = new Card();

        card.Initialize(Id, TargetRequired, Attack, DefenseLower, DefenseUpper, Health, CardDraw, Discard, SpectaclePoints,
            CardName, Description, Lore, Tooltip, ImagePath, AnimationPath, SoundPath);
        card.color = new Color(this.color.R, this.color.G, this.color.B);
        return card;
    }
    
    public override string ToString()
    {
        return $"Card: {CardName} ({Id}), {Attack} attack, {DefenseLower}-{DefenseUpper} defense, {Health} health, {CardDraw} draw, {Discard} discard, {SpectaclePoints} spectacle points";
    }
}
