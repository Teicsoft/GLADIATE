using GLADIATE.scripts.battle.target;

namespace GLADIATE.scripts.battle.card;

public partial class ModifierCard : Card
{
    public override void Play(GameState gameState, ITarget target, ITarget player) {
        switch (Modifier) {
            case Utils.ModifierEnum.Grappled:
                target.Grapple(TargetPosition);
                break;
            case Utils.ModifierEnum.Grounded:
                target.Ground(TargetPosition);
                break;
            case Utils.ModifierEnum.Juggled:
                target.Juggle();
                break;
        }
        base.Play(gameState, target, player);
    }
}