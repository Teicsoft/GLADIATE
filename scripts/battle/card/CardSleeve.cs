using Godot;
using TeicsoftSpectacleCards.scripts.battle;
using TeicsoftSpectacleCards.scripts.battle.card;

public partial class CardSleeve : Control {
    [Signal] public delegate void CardSelectedEventHandler(CardSleeve cardSleeve);

    public Card Card { get; set; }
    public Button SelectButton;
    private TextureRect _art;
    private Label _name;
    private Label _description;

    //cache
    public Animation Animation { get; set; }
    public AudioStream Sound { get; set; }

    public override void _Ready() {
        SelectButton = GetNode<Button>("SelectButton");
        _art = GetNode<TextureRect>("Art");
        _name = GetNode<Label>("Name");
        _description = GetNode<Label>("Description");

        _name.Text = Card.CardName;
        _description.Text = Card.Description;
        Utils.LoadCardArt(Card, _art);

        TextureRect cardType = GetNode<TextureRect>("Background/CardTypeIndicator");
        Texture cardTypeTexture = (Texture)GD.Load($"res://assets/images/Cards/Type Icons/{Card.CardType}.png");
        cardType.Texture = (Texture2D)cardTypeTexture;

        Label SpectaclePoints = GetNode<Label>("Background/SpectaclePoints");
        SpectaclePoints.Text = Card.SpectaclePoints.ToString();
    }

    public override void _Process(double delta) { }

    private void OnPress() { EmitSignal(SignalName.CardSelected, this); }

    public void LoadAssets() {
        Utils.LoadCardArt(Card, _art);
        LoadAnimation();
        LoadSound();
    }

    private void LoadAnimation() { Animation = (Animation)GD.Load(Card.AnimationPath); }

    private void LoadSound() { Sound = (AudioStream)GD.Load(Card.SoundPath); }

    public override string ToString() { return $"CardSleve: " + Card.ToString(); }
}
