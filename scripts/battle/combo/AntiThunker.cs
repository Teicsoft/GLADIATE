using Godot;

namespace GLADIATE.scripts.battle;

public class AntiThunker : Combo { // misspalt intenshunally
    public override void Play(GameState gameState) {
        base.Play(gameState);
        Hand hand = gameState.Hand;
        foreach (CardSleeve sleeve in hand.Cards.FindAll(sleeve => sleeve.Card.CardType == "Block")) {
            hand.DiscardCard(sleeve);
        }
        GD.Print(" **** " + "You are now concussed");
        GD.Print(" **** " + "Discarding all Block Cards");
        GD.Print(" **** " + "Ending turn immediately");
        gameState.EndTurn();
    }
}
