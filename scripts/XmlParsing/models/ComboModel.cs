using System.Collections.Generic;
using Godot;
using TeicsoftSpectacleCards.scripts.battle.card;

namespace TeicsoftSpectacleCards.scripts.XmlParsing.models;

public class ComboModel
{
    public string Id { get; set; }

    public List<Card> CardList;

    public bool TargetRequired { get; set; }

    public ModifierEnum Modifier { get; set; }

    public PositionEnum Position { get; set; }

    // main stats
    public int Attack { get; set; }
    public int DefenseLower { get; set; }
    public int DefenseUpper { get; set; }
    public int Health { get; set; }
    public int Draw { get; set; }
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

    public ComboModel(
        string id, List<Card> cardList, ModifierEnum modifier, PositionEnum position,
        int attack, int defenseLower, int defenseUpper, int health, int draw, int discard, int spectaclePoints,
        string name, string description, string lore, string onscreenText,
        string imagePath, string charAnimationPath, string stageAnimationPath, string soundPath
    )
    {
        this.Id = id;
        this.CardList = cardList;
        this.Modifier = modifier;
        this.Position = position;

        this.Attack = attack;
        this.DefenseLower = defenseLower;
        this.DefenseUpper = defenseUpper;
        this.Health = health;
        this.Draw = draw;
        this.Discard = discard;
        this.SpectaclePoints = spectaclePoints;

        this.Name = name;
        this.Description = description;
        this.Lore = lore;
        this.OnscreenText = onscreenText;

        this.ImagePath = imagePath;
        this.CharAnimationPath = charAnimationPath;
        this.StageAnimationPath = stageAnimationPath;
        this.SoundPath = soundPath;
    }

    public void LoadAssets()
    {
        LoadTexture();
        LoadCharAnimation();
        LoadStageAnimation();
        LoadSound();
    }

    private void LoadTexture()
    {
        Image = (Texture)GD.Load(ImagePath);
    }

    private void LoadCharAnimation()
    {
        CharAnimation = (Animation)GD.Load(CharAnimationPath);
    }

    private void LoadStageAnimation()
    {
        StageAnimation = (Animation)GD.Load(StageAnimationPath);
    }

    private void LoadSound()
    {
        Sound = (AudioStream)GD.Load(SoundPath);
    }

    public enum ModifierEnum
    {
        Grappled,
        Grounded,
        Juggled,
        None
    }

    public enum PositionEnum
    {
        Upper,
        Lower,
        Both,
        None
    }
}