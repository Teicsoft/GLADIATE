using System;
using Godot;

namespace TeicsoftSpectacleCards.scripts.battle.card;

public static class CardFactory
{
    private static PackedScene scene = (PackedScene)ResourceLoader.Load("res://scenes/battle/Card.tscn");
    
    public static Card MakeBlankCard(string cardId) {
        Card card = new Card();
        card.Id = cardId;
        return card;
    }

    public static Card MakeCard(string type, string cardId, Enemy.ModifierEnum modifier, Enemy.PositionEnum position,
        int attack, int defenseLower, int defenseUpper, int health, int draw, int discard, int spectaclePoints,
        string name, string description, string lore, string tooltip, string imagePath, string animationPath,
        string soundPath) {
        Card Card;
        switch (type) {
            case "Attack":
                Card = new AttackCard();
                return Card.Initialize(cardId, true, attack, defenseLower, defenseUpper, health, draw, discard,
                    spectaclePoints, name, description, lore, tooltip, imagePath, animationPath, soundPath);

            case "Draw":
                Card = new DrawCard();
                return Card.Initialize(cardId, true, attack, defenseLower, defenseUpper, health, draw, discard,
                    spectaclePoints, name, description, lore, tooltip, imagePath, animationPath, soundPath);
            case "Block":
                Card = new BlockCard();
                return Card.Initialize(cardId, true, attack, defenseLower, defenseUpper, health, draw, discard,
                    spectaclePoints, name, description, lore, tooltip, imagePath, animationPath, soundPath);
            case "Modifier":
                Card = new ModifierCard();
                return Card.Initialize(cardId, true, attack, defenseLower, defenseUpper, health, draw, discard,
                    spectaclePoints, name, description, lore, tooltip, imagePath, animationPath, soundPath);

            default:
                throw new ArgumentException("Invalid card type", nameof(type));
        }
    }
}
