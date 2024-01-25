namespace TeicsoftSpectacleCards.scripts.battle.target;

public interface Target {
    public int MaxHealth { get; set; }
    public int Health { get; set; }
    public int DefenseLower { get; set; }
    public int DefenseUpper { get; set; }
    void Damage(int damage, Utils.PositionEnum position = Utils.PositionEnum.Upper) { }

    void Stun(int stun) { }

    public void ModifyBlock(int change, Utils.PositionEnum position) { }

    public void Heal(int amount) { }

    private void DirectDamage(int damage) { }
}
