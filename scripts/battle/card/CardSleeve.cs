using Godot;
using TeicsoftSpectacleCards.scripts.battle;
using TeicsoftSpectacleCards.scripts.battle.card;

public partial class CardSleeve : Node2D {
    [Signal]
    public delegate void CardSelectedEventHandler(CardSleeve cardSleeve);

    public Card Card { get; set; }
    public Button SelectButton;
    
    //cache
    public Texture Image { get; set; }
    public Animation Animation { get; set; }
    public AudioStream Sound { get; set; }

    
    public override void _Ready() {
        SelectButton = GetNode<Button>("SelectButton");
        SelectButton.Text = Card.Attack.ToString();
        SelectButton.AddThemeColorOverride("font_color", Card.color);
    }

    public override void _Process(double delta) { }

    public void TestSetup(int newAttack, bool targetRequired, Color color) {
        Card.Attack = newAttack;
        Card.TargetRequired = targetRequired;
        Card.color = color;
    }

    public void Play(GameState gameState) {
        if (Card.Attack != 0) {
            if (Card.TargetRequired) { gameState.GetSelectedEnemy().Damage(Card.Attack); } else {
                foreach (Enemy enemy in gameState.enemies) { enemy.Damage(Card.Attack); }
            }
        }

        if (Card.DefenseLower != 0) {
            // gameState.ModifyPlayerDefenseLower(DefenseLower);
        }

        if (Card.DefenseUpper != 0) {
            // gameState.ModifyPlayerDefenseUpper(DefenseUpper);
        }

        if (Card.Health != 0) {
            gameState.HealPlayer(Card.Health);
        }

        if (Card.CardDraw > 0) {
            gameState.Draw(Card.CardDraw);
        }

        if (Card.Discard > 0) {
            // swalsh TODO: Emit Event?
            // swalsh TODO: Choice Discard by default, I think, but still needs an interface etc.
            // gameState.DiscardCards(Health);
        }

        gameState.ComboCheck(Card);
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
        Image = (Texture)GD.Load(Card.ImagePath);
    }

    private void LoadAnimation() {
        Animation = (Animation)GD.Load(Card.AnimationPath);
    }

    private void LoadSound() {
        Sound = (AudioStream)GD.Load(Card.SoundPath);
    }



    public override string ToString()
    {
        return $"CardSleve: " + Card.ToString();
    }
}
