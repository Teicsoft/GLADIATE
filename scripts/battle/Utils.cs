using System;
using System.Collections.Generic;
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
        if (target is Player && target.Statuses.Remove(StatusEnum.GetScars)) {
            target.ModifyBlock(1, PositionEnum.Upper);
            target.ModifyBlock(1, PositionEnum.Lower);
        }
        if (!(target.Statuses.Remove(StatusEnum.StayJuggled) && target.Modifier == ModifierEnum.Juggled)) {
            target.Modifier = ModifierEnum.None;
        }
    }

    public static void CounterCheck(GameState gameState, ITarget target, ITarget player) {
        if (target.Statuses.Contains(StatusEnum.Countering)) {
            player.DirectDamage(5);
            if (player is Enemy) { gameState.SpectaclePoints += 10 * gameState.Multiplier; }
        }
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

    public static PositionEnum GetRandomPosition() {
        Array values = Enum.GetValues(typeof(PositionEnum));
        return (PositionEnum)values.GetValue(GD.Randi() % values.Length);
    }
    
    public class StatusesDecorator : HashSet<Utils.StatusEnum>
    {
        protected HashSet<Utils.StatusEnum> _Statuses;
        public StatusesDecorator(HashSet<Utils.StatusEnum> statuses) { _Statuses = statuses; }

        public StatusesDecorator()
        {
            _Statuses = new HashSet<Utils.StatusEnum>();
        }
        
        public new bool Add(StatusEnum item)
        {
            GD.Print("Add: " + item);
            return _Statuses.Add(item);
        }
    }
}
