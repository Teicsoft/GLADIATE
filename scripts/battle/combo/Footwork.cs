using System.Linq;
using GLADIATE.scripts.battle.card;
using GLADIATE.scripts.battle.target;

namespace GLADIATE.scripts.battle;

public class Footwork : Combo {
    public override void Play(GameState gameState) {
        Enemy selectedEnemy = gameState.GetSelectedEnemy();
        foreach (Card card in CardList.Where(card => card.Attack > 0)) {
            int modifiedAttack = (int)(card.Attack * Utils.GetDamageMultiplier(gameState.Player));
            selectedEnemy.Damage(modifiedAttack, card.TargetPosition);
            selectedEnemy.Damage(modifiedAttack, card.TargetPosition);
            selectedEnemy.Damage(modifiedAttack, card.TargetPosition);
        }
        base.Play(gameState);
    }
}
