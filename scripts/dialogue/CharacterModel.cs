using Godot;

namespace TeicsoftSpectacleCards.scripts.dialogue;

public class CharacterModel
{
    public string id { get; set;}
    public string char_name { get; set;}
    public string font_ref { get; set;}
    public string color_hex { get; set;}
    public string sprite_ref { get; set;}
    public string default_animation { get; set;}
    // public string[] animations { get; set;}

    public CharacterModel(string id, string charName, string fontRef, string colorHex, string spriteRef, string defaultAnimation) //string[] animations)
    {
        this.id = id;
        char_name = charName;
        font_ref = fontRef;
        color_hex = colorHex;
        sprite_ref = spriteRef;
        default_animation = defaultAnimation;
        // this.animations = animations;
    }

    public override string ToString()
    {
        return $"{nameof(id)}: {id}, {nameof(char_name)}: {char_name}, {nameof(font_ref)}: {font_ref}, {nameof(color_hex)}: {color_hex}, {nameof(sprite_ref)}: {sprite_ref}, {nameof(default_animation)}: {default_animation}"; //, {nameof(animations)}: {animations}"; 
    }
}