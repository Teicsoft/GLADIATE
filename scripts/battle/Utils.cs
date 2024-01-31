using Godot;
using TeicsoftSpectacleCards.scripts.battle.card;

namespace TeicsoftSpectacleCards.scripts.battle;

public static class Utils {
    

    public enum ModifierEnum {
        Grappled,
        Grounded,
        Juggled,
        None
    }

    public enum PositionEnum {
        Upper,
        Lower,
        None
    }

    public enum StatusEnum {
        Stunned,
    }

    public static TextureRect LoadCardArt(Card card) { 
        return LoadCardArt(card, new());
    }

    public static TextureRect LoadCardArt(Card card, TextureRect art) {
        Texture2D texture = (Texture2D)GD.Load(card.ImagePath);
        art.Texture = texture;

        float ratio = 176 / texture.GetSize().X;
        art.Scale = new Vector2(ratio, ratio);
        return art;
    }
}
