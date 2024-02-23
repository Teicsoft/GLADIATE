using System;
using GLADIATE.scripts.battle.target;
using Godot;

namespace GLADIATE.scripts.battle.card;

public class Card {
    public string Id { get; set; } // card_id
    public string CardType { get; set; } // card_type
    public bool TargetRequired { get; set; }
    public Utils.ModifierEnum Modifier { get; set; }
    public Utils.PositionEnum TargetPosition { get; set; }

    // main stats
    public int Attack { get; set; }
    public int DefenseLower { get; set; }
    public int DefenseUpper { get; set; }
    public int Health { get; set; }
    public int CardDraw { get; set; }
    public int Discard { get; set; }
    public int SpectaclePoints { get; set; }

    //text
    public string CardName { get; set; }
    public string Description { get; set; }
    public string Lore { get; set; }
    public string Tooltip { get; set; }

    //design
    public string ImagePath { get; set; } //path to image
    public string AnimationPath { get; set; } //path to animation
    public string SoundPath { get; set; } //path to sound

    public Card Initialize(
        string cardType, string id, Utils.ModifierEnum modifier, Utils.PositionEnum position,
        bool targetRequired = true, int attack = 0, int defenseLower = 0, int defenseUpper = 0, int health = 0,
        int draw = 0, int discard = 0, int spectaclePoints = 0, string name = "", string description = "",
        string lore = "", string tooltip = "", string imagePath = "", string animationPath = "", string soundPath = ""
    ) {
        CardType = cardType;
        Id = id;
        Modifier = modifier;
        TargetPosition = position;

        TargetRequired = targetRequired;

        Attack = attack;
        DefenseLower = defenseLower;
        DefenseUpper = defenseUpper;
        Health = health;
        CardDraw = draw;
        Discard = discard;
        SpectaclePoints = spectaclePoints;

        CardName = name;
        Description = description;
        Lore = lore;
        Tooltip = tooltip;

        ImagePath = imagePath;
        AnimationPath = animationPath;
        SoundPath = soundPath;

        return this;
    }

    public virtual void Play(GameState gameState, ITarget target, ITarget player) {
        // not the player player, they who played this card
        GD.Print(player.Name + " played " + CardName);
        if (Attack != 0) {
            Utils.CounterCheck(gameState, target, player);
            int modifiedAttack = (int)Math.Floor(Attack * Utils.GetDamageMultiplier(player));
            if (TargetRequired || player is Enemy) { target.Damage(modifiedAttack, TargetPosition); } else {
                foreach (Enemy enemy in gameState.Enemies) { enemy.Damage(modifiedAttack, TargetPosition); }
            }
        }

        if (DefenseLower != 0) { player.ModifyBlock(DefenseLower, Utils.PositionEnum.Lower); }

        if (DefenseUpper != 0) { player.ModifyBlock(DefenseUpper, Utils.PositionEnum.Upper); }

        if (Health != 0) { player.Heal(Health); }

        if (player is not Enemy) {
            if (CardDraw > 0) { gameState.Draw(CardDraw); }

            if (Discard > 0) { gameState.Discards += Discard; }

            if (SpectaclePoints > 0) { gameState.SpectacleBuffer += SpectaclePoints; }
        }
    }

    public virtual bool IsPlayable(ITarget target) { return !TargetRequired || (TargetRequired && target != null); }

    public Card Clone() {
        Card card = CardFactory.ConstructCard(Id);

        card.Initialize(
            CardType, Id, Modifier, TargetPosition, TargetRequired, Attack, DefenseLower, DefenseUpper, Health,
            CardDraw, Discard, SpectaclePoints, CardName, Description, Lore, Tooltip, ImagePath, AnimationPath,
            SoundPath
        );
        return card;
    }

    public override string ToString() {
        return
            $"Card: {CardName} ({Id}), {Attack} attack, {DefenseLower}-{DefenseUpper} defense, {Health} health, {CardDraw} draw, {Discard} discard, {SpectaclePoints} spectacle points";
    }
}
