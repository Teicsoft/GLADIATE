using Godot;

public partial class Enemy : Node2D {

	[Signal]
	public delegate void EnemySelectedEventHandler(Enemy enemy);

	private ColorRect rect;
	private Button selectButton;

	public override void _Ready() {
		selectButton = GetNode<Button>("SelectButton");
		rect = GetNode<ColorRect>("ColorRect");
	}

	public override void _Process(double delta) { }

	public void ChangeColour(Color color) {
		rect.Color = color;
	}

	private void OnPress() {
		EmitSignal(SignalName.EnemySelected, this);
	}

}
