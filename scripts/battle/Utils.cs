using Godot;
using TeicsoftSpectacleCards.scripts.battle.card;
using TeicsoftSpectacleCards.scripts.battle.target;

namespace TeicsoftSpectacleCards.scripts.battle;

public static class Utils {

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

    public static float GetDamageMultiplier(ITarget player) {
        float damageMultiplier = 1f;
        if (player.Statuses.Contains(Utils.StatusEnum.TattooRevealed)) { damageMultiplier *= 3f / 4; }
        if (player.Statuses.Contains(Utils.StatusEnum.DoubleDamage)) { damageMultiplier *= 2; }
        return damageMultiplier;
    }
    

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
        TattooRevealed,
        Countering,
        CrowdPleased,
        MoveShouted,
        JustShowedOff,
        ShowedOff,
        DoubleDamage,
    }
}
