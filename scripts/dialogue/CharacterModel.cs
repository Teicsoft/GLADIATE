namespace TeicsoftSpectacleCards.scripts.dialogue;

public class CharacterModel
{
    public string Id { get; set;}
    public string CharName { get; set;}
    public string FontRef { get; set;}
    public string ColorHex { get; set;}
    public string SpriteRef { get; set;}
    public string DefaultAnimation { get; set;}
    public string[] Animations { get; set;}

    public CharacterModel(string id, string charName, string fontRef, string colorHex, string spriteRef, string defaultAnimation, string[] animations)
    {
        this.Id = id;
        CharName = charName;
        FontRef = fontRef;
        ColorHex = colorHex;
        SpriteRef = spriteRef;
        DefaultAnimation = defaultAnimation;
        this.Animations = animations;
    }

    public override string ToString()
    {
        return $"{nameof(Id)}: {Id}, {nameof(CharName)}: {CharName}, {nameof(FontRef)}: {FontRef}, {nameof(ColorHex)}: {ColorHex}, {nameof(SpriteRef)}: {SpriteRef}, {nameof(DefaultAnimation)}: {DefaultAnimation}, {nameof(Animations)}: {Animations}"; 
    }
}