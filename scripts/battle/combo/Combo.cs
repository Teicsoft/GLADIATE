using System.Collections.Generic;
using GLADIATE.scripts.battle.card;
using GLADIATE.scripts.battle.target;
using Godot;

namespace GLADIATE.scripts.battle;

public class Combo {
    public string Id { get; set; }

    public List<Card> CardList;

    public Utils.ModifierEnum Modifier { get; set; }

    public Utils.PositionEnum Position { get; set; }

    // main stats
    public int Attack { get; set; }
    public int DefenseLower { get; set; }
    public int DefenseUpper { get; set; }
    public int Health { get; set; }
    public int CardDraw { get; set; }
    public int Discard { get; set; }
    public int SpectaclePoints { get; set; }

    //text
    public string Name { get; set; }
    public string Description { get; set; }
    public string Lore { get; set; }
    public string OnscreenText { get; set; }

    //design
    public string ImagePath { get; set; } //path to image
    public string CharAnimationPath { get; set; } //path to animation
    public string StageAnimationPath { get; set; } //path to animation
    public string SoundPath { get; set; } //path to sound

    //cache
    public Texture Image { get; set; }
    public Animation CharAnimation { get; set; }
    public Animation StageAnimation { get; set; }
    public AudioStream Sound { get; set; }

    public void Initialize(
        string id, List<Card> cardList, Utils.ModifierEnum modifier, Utils.PositionEnum position, int attack,
        int defenseLower, int defenseUpper, int health, int cardDraw, int discard, int spectaclePoints, string name,
        string description, string lore, string onscreenText, string imagePath, string charAnimationPath,
        string stageAnimationPath, string soundPath
    ) {
        Id = id;
        CardList = cardList;
        Modifier = modifier;
        Position = position;

        Attack = attack;
        DefenseLower = defenseLower;
        DefenseUpper = defenseUpper;
        Health = health;
        CardDraw = cardDraw;
        Discard = discard;
        SpectaclePoints = spectaclePoints;

        Name = name;
        Description = description;
        Lore = lore;
        OnscreenText = onscreenText;

        ImagePath = imagePath;
        CharAnimationPath = charAnimationPath;
        StageAnimationPath = stageAnimationPath;
        SoundPath = soundPath;
    }

    public virtual void Play(GameState gameState) {
        if (Attack != 0) {
            // if the last combo card required a target, then we apply the combo damage to that target.
            if (CardList[^1].TargetRequired) {
                Enemy selectedEnemy = gameState.GetSelectedEnemy();
                GD.Print(" **** " + "Hit " + selectedEnemy.Name + " for " + Attack);
                selectedEnemy.Damage(Attack, Position);
            }

            // Otherwise, we can't assume that an enemy is selected, so damage all of them.
            else {
                foreach (Enemy enemy in gameState.Enemies) {
                    GD.Print(" **** " + "Hit " + enemy.Name + " for " + Attack);
                    enemy.Damage(Attack, Position);
                }
            }
        }

        if (DefenseLower != 0) {
            GD.Print(" **** " + "Modified Lower Defense by " + DefenseLower);
            gameState.Player.ModifyBlock(DefenseLower, Utils.PositionEnum.Lower);
        }

        if (DefenseUpper != 0) {
            GD.Print(" **** " + "Modified Upper Defense by " + DefenseLower);
            gameState.Player.ModifyBlock(DefenseUpper, Utils.PositionEnum.Upper);
        }

        if (Health != 0) {
            GD.Print(" **** " + "Healed for " + Health);
            gameState.Player.Heal(Health);
        }

        if (CardDraw > 0) {
            GD.Print(" **** " + "Drawing " + CardDraw + " Cards");
            gameState.Draw(CardDraw);
        }

        if (Discard > 0) {
            GD.Print(" **** " + "Discarding " + Discard + " Cards");
            gameState.Discards += Discard;
        }

        if (SpectaclePoints > 0) {
            GD.Print(" **** " + "Adding " + SpectaclePoints + " SP to buffer");
            gameState.SpectacleBuffer += SpectaclePoints;
        }
    }

    public void LoadAssets() {
        LoadTexture();
        LoadCharAnimation();
        LoadStageAnimation();
        LoadSound();
    }

    private void LoadTexture() { Image = (Texture)GD.Load(ImagePath); }

    private void LoadCharAnimation() { CharAnimation = (Animation)GD.Load(CharAnimationPath); }

    private void LoadStageAnimation() { StageAnimation = (Animation)GD.Load(StageAnimationPath); }

    private void LoadSound() { Sound = (AudioStream)GD.Load(SoundPath); }
}
