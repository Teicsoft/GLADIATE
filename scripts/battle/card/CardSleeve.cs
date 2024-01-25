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
        SelectButton.Text = Card.CardName;
        SelectButton.AddThemeColorOverride("font_color", Card.color);
    }

    public override void _Process(double delta) { }
    
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
