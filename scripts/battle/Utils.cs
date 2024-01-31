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
        if (player.Statuses.Contains(StatusEnum.TattooRevealed)) { damageMultiplier *= 3f / 4; }
        if (player.Statuses.Contains(StatusEnum.DoubleDamage)) { damageMultiplier *= 2; }
        return damageMultiplier;
    }

    public static void RemoveEndTurnStatuses(ITarget target) {
        target.Statuses.Remove(StatusEnum.MoveShouted);
        target.Statuses.Remove(StatusEnum.TattooRevealed);
        target.Statuses.Remove(StatusEnum.Countering);
        target.Statuses.Remove(StatusEnum.DoubleDamage);
    }

    public static void CounterCheck(GameState gameState, ITarget target, ITarget player) {
        if (target.Statuses.Contains(StatusEnum.Countering)) {
            player.DirectDamage(5);
            if (player is Enemy) { gameState.SpectaclePoints += 10 * gameState.Multiplier; }
        }
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
