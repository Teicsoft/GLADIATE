using Godot;

namespace TeicsoftSpectacleCards.scripts.battle.card;

public class StunCard:Card {

    public override void Play(GameState gameState) {
        base.Play(gameState);
        
        GD.Print("STUN!");
        
        gameState.GetSelectedEnemy().Stun(1);

        gameState.ComboCheck(this);
    }

    public override Card Clone() {
        Card card = new StunCard();

        card.Initialize(Id, TargetRequired, Attack, DefenseLower, DefenseUpper, Health, CardDraw, Discard,
            SpectaclePoints, CardName, Description, Lore, Tooltip, ImagePath, AnimationPath, SoundPath);
        return card;
    }

}
