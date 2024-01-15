using Godot;

namespace TeicsoftSpectacleCards.scripts.customresource;

public class CardModel
{
    public string Id { get; set;} // card_id

    public Target TargetType { get; set;}
    public Type CardType { get; set;}
    public Modifier[] Modifiers { get; set;}
    
    // main stats
    public int Attack { get; set;}
    public int DefenseLower { get; set;}
    public int DefenseUpper { get; set;}
    public int Health { get; set;}
    public int Draw { get; set;}
    public int SpectaclePoints { get; set;}
    
    //text
    public string Name { get; set;}
    public string Description { get; set;}
    public string Lore { get; set;}
    public string ToolTip { get; set;}
    
        
    //design
    public string ImagePath { get; set;} //path to image
    public string AnimationPath { get; set;} //path to animation
    public string SoundPath { get; set;} //path to sound
    
    //cache
    public Texture Image { get; set;}
    public Animation Animation { get; set;}
    public AudioStream Sound { get; set;}
    
    
    //constructor
    public CardModel(string id)
    {
        this.Id = id;
    }
    
    public CardModel(
        string id, 
        int attack, int defenseLower, int defenseUpper, int health, int draw, int spectaclePoints, 
        string name, string description, string lore, string tooltip,
        string imagePath, string animationPath, string soundPath
        )
    {
        this.Id = id;
        this.Attack = attack;
        this.DefenseLower = defenseLower;
        this.DefenseUpper = defenseUpper;
        this.Health = health;
        this.Draw = draw;
        this.SpectaclePoints = spectaclePoints;
        
        this.Name = name;
        this.Description = description;
        this.Lore = lore;
        this.ToolTip = tooltip;
        
        this.ImagePath = imagePath;
        this.AnimationPath = animationPath;
        this.SoundPath = soundPath;
    }
    
    public string ToString()
    {
        return $"{nameof(Id)}: {Id}, {nameof(TargetType)}: {TargetType}, {nameof(CardType)}: {CardType}, {nameof(Modifiers)}: {Modifiers}, {nameof(Attack)}: {Attack}, {nameof(DefenseLower)}: {DefenseLower}, {nameof(DefenseUpper)}: {DefenseUpper}, {nameof(Health)}: {Health}, {nameof(Draw)}: {Draw}, {nameof(SpectaclePoints)}: {SpectaclePoints}, {nameof(Name)}: {Name}, {nameof(Description)}: {Description}, {nameof(Lore)}: {Lore}, {nameof(ToolTip)}: {ToolTip}, {nameof(ImagePath)}: {ImagePath}, {nameof(AnimationPath)}: {AnimationPath}, {nameof(SoundPath)}: {SoundPath}, {nameof(Image)}: {Image}, {nameof(Animation)}: {Animation}, {nameof(Sound)}: {Sound}";
    }


    public void LoadAssets()
    {
        LoadTexture();
        LoadAnimation();
        LoadSound();
    }
    
    private void LoadTexture()
    {
        Image = (Texture) GD.Load(ImagePath);
    }
    
    private void LoadAnimation()
    {
        Animation = (Animation) GD.Load(AnimationPath);
    }
    
    private void LoadSound()
    {
        Sound = (AudioStream) GD.Load(SoundPath);
    }
    
    //enums
    public enum Type
    {
        Attack, 
        Modifier, 
        Block, 
        Draw
    }
    
    public enum Target
    {
        Self, 
        SingleEnemy, 
        AllEnemies, 
        Everyone
    }
    
    public enum Modifier
    {
        Grappled,
        Grounded,
        Juggled
    }
}