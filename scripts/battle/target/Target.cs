using System;
using System.Collections.Generic;

namespace TeicsoftSpectacleCards.scripts.battle.target;

public interface ITarget {
    public string Name { get; set; }
    public int MaxHealth { get; set; }
    public int Health { get; set; }
    public int DefenseLower { get; set; }
    public int DefenseUpper { get; set; }
    HashSet<Utils.StatusEnum> Statuses { get; set; }
    public Utils.ModifierEnum Modifier { get; set; }
    void Damage(int damage, Utils.PositionEnum position = Utils.PositionEnum.Upper) { }

    void Stun() { }

    public bool CheckBlock(Utils.PositionEnum position) { throw new NotImplementedException();}

    public void ModifyBlock(int change, Utils.PositionEnum position) { }

    public void Heal(int amount) { }

    public void DirectDamage(int damage) { }
    public void Grapple(Utils.PositionEnum position);
    public void Ground(Utils.PositionEnum position);
    public void Juggle();
}
