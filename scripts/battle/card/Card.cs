using Godot;
using System.Collections.Generic;

public partial class Card : Node2D
{
    [Signal]
    public delegate void CardSelectedEventHandler(Card card);

    public Color color { get; set; }
    public Button selectButton;


    public string Id { get; set; } // card_id

    public bool TargetRequired { get; set; }
    public ModifierEnum Modifier { get; set; }
    public PositionEnum TargetPosition { get; set; }


    // main stats
    public int Attack { get; set; }
    public int DefenseLower { get; set; }
    public int DefenseUpper { get; set; }
    public int Health { get; set; }
    public int Draw { get; set; }
    public int Discard { get; set; }
    public int SpectaclePoints { get; set; }

    //text
    public string Name { get; set; }
    public string Description { get; set; }
    public string Lore { get; set; }
    public string ToolTip { get; set; }


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
    public virtual Card Initialize(string id)
    {
        this.Id = id;
        return this;
    }

    public virtual Card Initialize(
        string id, ModifierEnum modifier, PositionEnum position,
        int attack, int defenseLower, int defenseUpper, int health, int draw, int discard, int spectaclePoints,
        string name, string description, string lore, string tooltip,
        string imagePath, string animationPath, string soundPath
    )
    {
        this.Id = id;
        this.Modifier = modifier;
        this.TargetPosition = TargetPosition;
        this.Attack = attack;

        this.DefenseLower = defenseLower;
        this.DefenseUpper = defenseUpper;
        this.Health = health;
        this.Draw = draw;
        this.Discard = discard;
        this.SpectaclePoints = spectaclePoints;

        this.Name = name;
        this.Description = description;
        this.Lore = lore;
        this.ToolTip = tooltip;

        this.ImagePath = imagePath;
        this.AnimationPath = animationPath;
        this.SoundPath = soundPath;
        return this;
    }

    public override void _Ready()
    {
        selectButton = GetNode<Button>("SelectButton");
        selectButton.AddThemeColorOverride("font_color", this.color);
    }

    public override void _Process(double delta)
    {
    }

    public void Play( /*GameState gameState,*/ Enemy enemy, List<Enemy> allEnemies)
    {
        if (color.Equals(new Color(0.0f, 1.0f, 0.0f)))
        {
            foreach (Enemy e in allEnemies)
            {
                e.ChangeColour(color);
            }
        }
        else
        {
            enemy.ChangeColour(color);
        }
    }

    public bool RequiresTarget()
    {
        return !color.Equals(new Color(0.0f, 1.0f, 0.0f));
    }

    public void ChangeColor(Color color)
    {
        this.color = color;
    }

    private void OnPress()
    {
        EmitSignal(SignalName.CardSelected, this);
    }

    public void LoadAssets()
    {
        LoadTexture();
        LoadAnimation();
        LoadSound();
    }

    private void LoadTexture()
    {
        Image = (Texture)GD.Load(ImagePath);
    }

    private void LoadAnimation()
    {
        Animation = (Animation)GD.Load(AnimationPath);
    }

    private void LoadSound()
    {
        Sound = (AudioStream)GD.Load(SoundPath);
    }

    public enum ModifierEnum
    {
        Grappled,
        Grounded,
        Juggled,
        None
    }

    public enum PositionEnum
    {
        Upper,
        Lower,
        Both,
        None
    }
}
