namespace TeicsoftSpectacleCards.scripts.customresource.Cards;

public class AttackCardModel: CardModel
{
    public AttackCardModel(
        string id, Modifier modifier,
        int attack, int defenseLower, int defenseUpper, int health, int draw, int spectaclePoints, 
        string name, string description, string lore, string tooltip, string 
            imagePath, string animationPath, string soundPath
        ) : base(
        id, modifier,
        attack, defenseLower, defenseUpper, health, draw, spectaclePoints, 
        name, description, lore, tooltip, 
        imagePath, animationPath, soundPath
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

    public override string ToString()
    {
        return  this.GetType() + base.ToString();
    }
}