namespace TeicsoftSpectacleCards.scripts.battle.card;

public partial class ModifierCard : Card
{
    public override string ToString()
    {
        return this.GetType().Name + base.ToString();
    }
}