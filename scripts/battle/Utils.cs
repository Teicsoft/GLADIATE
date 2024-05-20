using System;
using GLADIATE.scripts.battle.card;
using GLADIATE.scripts.battle.target;
using Godot;

namespace GLADIATE.scripts.battle;

public static class Utils {
    public static TextureRect LoadCardArt(Card card) { return LoadCardArt(card, new()); }

    public static TextureRect LoadCardArt(Card card, TextureRect art) {
        Texture2D texture = (Texture2D)GD.Load(card.ImagePath);
        art.Texture = texture;

        float ratio = 176 / texture.GetSize().X;
        art.Scale = new Vector2(ratio, ratio);
        return art;
    }

    public static void RemoveEndTurnStatuses(ITarget target) {
        target.Statuses.Remove(StatusEnum.MoveShouted);
        target.Statuses.Remove(StatusEnum.TattooRevealed);
        target.Statuses.Remove(StatusEnum.Countering);
        target.Statuses.Remove(StatusEnum.DoubleDamage);
        if (target is Player && target.Statuses.Remove(StatusEnum.GetScars)) {
            target.ModifyBlock(1, PositionEnum.Upper);
            target.ModifyBlock(1, PositionEnum.Lower);
        }
        if (!(target.Statuses.Remove(StatusEnum.StayJuggled) && target.Modifier == ModifierEnum.Juggled)) {
            target.Modifier = ModifierEnum.None;
        }
    }

    public static int CalculateDamage(ITarget player, int attack) {
        float damageMultiplier = 1f;
        if (player.Statuses.Contains(StatusEnum.TattooRevealed)) { damageMultiplier *= 3f / 4; }
        if (player.Statuses.Contains(StatusEnum.DoubleDamage)) { damageMultiplier *= 2; }
        return (int)Math.Floor(attack * damageMultiplier);
    }

    private static void CounterCheck(GameState gameState, ITarget target, ITarget player) {
        if (target.Statuses.Contains(StatusEnum.Countering)) {
            player.DirectDamage(5);
            if (player is Enemy) { gameState.SpectaclePoints += 10 * gameState.Multiplier; }
        }
    }

    public static void DoAttack(GameState gameState, ITarget t, ITarget p, int attack, PositionEnum targetPosition) {
        CounterCheck(gameState, t, p);
        t.Damage(CalculateDamage(p, attack), targetPosition);
    }

    public enum ModifierEnum { Grappled, Grounded, Juggled, None }

    public enum PositionEnum { Upper, Lower, None }

    public enum StatusEnum {
        Stunned,
        TattooRevealed,
        Countering,
        CrowdPleased,
        MoveShouted,
        JustShowedOff,
        ShowedOff,
        DoubleDamage,
        StayJuggled,
        GetScars,
        OpenedRecklessly,
    }

    public class DirectionEventArgs : EventArgs {
        public string Direction { get; set; }
    }

    public static DirectionEventArgs CheckDirection(int oldValue, int newValue) {
        DirectionEventArgs args = new DirectionEventArgs();
        if (oldValue > newValue) { args.Direction = "down"; } else if
            (oldValue < newValue) { args.Direction = "up"; } else { args.Direction = "none"; }
        return args;
    }

    public class DamageEventArgs : EventArgs {
        public int Damage { get; set; }
    }

    public static PositionEnum GetRandomPosition() {
        Array values = Enum.GetValues(typeof(PositionEnum));
        return (PositionEnum)values.GetValue(GD.Randi() % values.Length);
    }
}
