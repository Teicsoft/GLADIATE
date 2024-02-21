namespace GLADIATE.scripts.battle.card;

public partial class BlockCard : Card
{
    public override string ToString()
    {
        return this.GetType().Name + base.ToString();
    }
}