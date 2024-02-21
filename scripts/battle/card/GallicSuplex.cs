using GLADIATE.scripts.battle.target;

namespace GLADIATE.scripts.battle.card;

public class GallicSuplex : Card {
    public override bool IsPlayable(ITarget target) { return target is { Modifier: Utils.ModifierEnum.Grappled }; }
}
