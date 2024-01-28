﻿using Godot;
using TeicsoftSpectacleCards.scripts.battle.target;

namespace TeicsoftSpectacleCards.scripts.battle.card;

public class StunCard : Card {

    public override void Play(GameState gameState, ITarget target, ITarget player) {
        GD.Print("STUN!");
        target.Stun(1);
        base.Play(gameState, target, player);
    }

    public override Card Clone() {
        Card card = new StunCard();

        card.Initialize(Id,Modifier,TargetPosition, TargetRequired, Attack, DefenseLower, DefenseUpper, Health, CardDraw, Discard,
            SpectaclePoints, CardName, Description, Lore, Tooltip, ImagePath, AnimationPath, SoundPath);
        return card;
    }

}
