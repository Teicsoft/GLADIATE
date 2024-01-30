using Godot;
using TeicsoftSpectacleCards.scripts.battle.target;

namespace TeicsoftSpectacleCards.scripts.battle.card;

public class Card {

    public Color color { get; set; }

    public string Id { get; set; } // card_id
    public bool TargetRequired { get; set; }
    public Utils.ModifierEnum Modifier { get; set; }
    public Utils.PositionEnum TargetPosition { get; set; }

    // main stats
    public int Attack { get; set; }
    public int DefenseLower { get; set; }
    public int DefenseUpper { get; set; }
    public int Health { get; set; }
    public int CardDraw { get; set; }
    public int Discard { get; set; }
    public int SpectaclePoints { get; set; }

    //text
    public string CardName { get; set; }
    public string Description { get; set; }
    public string Lore { get; set; }
    public string Tooltip { get; set; }

    //design
    public string ImagePath { get; set; } //path to image
    public string AnimationPath { get; set; } //path to animation
    public string SoundPath { get; set; } //path to sound

    public Card Initialize(string id, Utils.ModifierEnum modifier, Utils.PositionEnum position,
        bool targetRequired = true, int attack = 0, int defenseLower = 0, int defenseUpper = 0, int health = 0,
        int draw = 0, int discard = 0, int spectaclePoints = 0, string name = "", string description = "",
        string lore = "", string tooltip = "", string imagePath = "", string animationPath = "",
        string soundPath = "") {
        Id = id;
        Modifier = modifier;
        TargetPosition = position;

        TargetRequired = targetRequired;

        Attack = attack;
        DefenseLower = defenseLower;
        DefenseUpper = defenseUpper;
        Health = health;
        CardDraw = draw;
        Discard = discard;
        SpectaclePoints = spectaclePoints;

        CardName = name;
        Description = description;
        Lore = lore;
        Tooltip = tooltip;

        ImagePath = imagePath;
        AnimationPath = animationPath;
        SoundPath = soundPath;

        // todo this section is just for testing, remove later
        // Colour is a test feature, to help with debugging
        color = new Color(1, 1, 1);

        return this;
    }

    public virtual void Play(GameState gameState, ITarget target, ITarget player) {
        // not the player player, they who played this card
        GD.Print(player.Name + " played " + CardName);
        if (Attack != 0) {
            if (TargetRequired || player is Enemy) { target.Damage(Attack, TargetPosition); }
            else {
                foreach (Enemy enemy in gameState.Enemies) { enemy.Damage(Attack, TargetPosition); }
            }
        }

        if (DefenseLower != 0) { player.ModifyBlock(DefenseLower, Utils.PositionEnum.Lower); }

        if (DefenseUpper != 0) { player.ModifyBlock(DefenseUpper, Utils.PositionEnum.Upper); }

        if (Health != 0) { player.Heal(Health); }

        if (player is not Enemy) {
            if (CardDraw > 0) { gameState.Draw(CardDraw); }

            if (Discard > 0) {
                // swalsh TODO: Emit Event?
                // swalsh TODO: Choice Discard by default, I think, but still needs an interface etc.
                // gameState.DiscardCards(Health);
            }

            gameState.ComboCheck(this);
        }
    }

    public virtual Card Clone() {
        Card card = new Card();

        card.Initialize(Id, Modifier, TargetPosition, TargetRequired, Attack, DefenseLower, DefenseUpper, Health,
            CardDraw, Discard, SpectaclePoints, CardName, Description, Lore, Tooltip, ImagePath, AnimationPath,
            SoundPath);
        card.color = new Color(color.R, color.G, color.B);
        return card;
    }

    public override string ToString() {
        return
            $"Card: {CardName} ({Id}), {Attack} attack, {DefenseLower}-{DefenseUpper} defense, {Health} health, {CardDraw} draw, {Discard} discard, {SpectaclePoints} spectacle points";
    }
}
