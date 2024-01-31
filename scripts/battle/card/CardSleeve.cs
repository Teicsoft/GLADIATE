using Godot;
using TeicsoftSpectacleCards.scripts.battle;
using TeicsoftSpectacleCards.scripts.battle.card;

public partial class CardSleeve : Control {
	[Signal]
	public delegate void CardSelectedEventHandler(CardSleeve cardSleeve);

	public Card Card { get; set; }
	public Button SelectButton;
	private ColorRect _background;
	private TextureRect _art;
	private Label _name;
	private Label _description;

	//cache
	public Animation Animation { get; set; }
	public AudioStream Sound { get; set; }

	public override void _Ready() {
		SelectButton = GetNode<Button>("SelectButton");
		_background = GetNode<ColorRect>("Background");
		_art = GetNode<TextureRect>("Art");
		_name = GetNode<Label>("Name");
		_description = GetNode<Label>("Description");

		_name.Text = Card.CardName;
		_description.Text = Card.Description;
		LoadImage();
	}

	public override void _Process(double delta) { }

	private void OnPress() {
		EmitSignal(SignalName.CardSelected, this);
	}

	public void LoadAssets() {
		Utils.LoadCardArt(Card, _art);
		LoadAnimation();
		LoadSound();
	}

	private void LoadImage() {
		Texture2D texture = (Texture2D)GD.Load(Card.ImagePath);
		_art.Texture = texture;

		float ratio = 160 /texture.GetSize().X;
		_art.Scale = new Vector2(ratio, ratio);
	}

	private void LoadAnimation() {
		Animation = (Animation)GD.Load(Card.AnimationPath);
	}

	private void LoadSound() {
		Sound = (AudioStream)GD.Load(Card.SoundPath);
	}

	public override string ToString() {
		return $"CardSleve: " + Card.ToString();
	}
}
