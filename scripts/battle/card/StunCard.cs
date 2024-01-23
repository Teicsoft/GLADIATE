namespace TeicsoftSpectacleCards.scripts.battle.card;

public class StunCard:Card {

    public override void Play(GameState gameState) {
        base.Play(gameState);
        
        gameState.GetSelectedEnemy().Stun(1);

        gameState.ComboCheck(this);
    }
    
}
