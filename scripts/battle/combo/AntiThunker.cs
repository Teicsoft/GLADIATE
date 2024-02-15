namespace GLADIATE.scripts.battle;

public class AntiThunker : Combo { // misspalt intenshunally
    public override void Play(GameState gameState) {
        base.Play(gameState);
        Hand hand = gameState.Hand;

        // TODO
        // foreach (CardSleeve sleeve in hand.Cards.Where(sleeve => sleeve.Card.CardType == "BLOCK")) {
        //     hand.DiscardCard(sleeve);
        // }
        gameState.EndTurn();
    }
}
