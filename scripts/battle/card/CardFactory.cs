using System;

namespace TeicsoftSpectacleCards.scripts.battle.card;

public static class CardFactory
{
    public static Card MakeBlankCard(string cardId)
    {
        Card card = new Card();
        card.Id = cardId;
        return card;
    }

    public static Card MakeCard(
        string type, string cardId, Card.ModifierEnum modifier, Card.PositionEnum position,
        int attack, int defenseLower, int defenseUpper, int health, int draw, int discard, int spectaclePoints,
        string name, string description, string lore, string tooltip, string
            imagePath, string animationPath, string soundPath)
    {
        Card card;
        switch (type)
        {
            case "Attack":
                card = new AttackCard();
                return card.Initialize(
                    cardId, modifier, position,
                    attack, defenseLower, defenseUpper, health, draw, discard, spectaclePoints,
                    name, description, lore, tooltip,
                    imagePath, animationPath, soundPath);

            case "Draw":
                card = new DrawCard();
                return card.Initialize(
                    cardId, modifier, position,
                    attack, defenseLower, defenseUpper, health, draw, discard, spectaclePoints,
                    name, description, lore, tooltip,
                    imagePath, animationPath, soundPath
                );
            case "Block":
                card = new BlockCard();
                return card.Initialize(
                    cardId, modifier, position,
                    attack, defenseLower, defenseUpper, health, draw, discard, spectaclePoints,
                    name, description, lore, tooltip,
                    imagePath, animationPath, soundPath
                );
            case "Modifier":
                card = new ModifierCard();
                return card.Initialize(
                    cardId, modifier, position,
                    attack, defenseLower, defenseUpper, health, draw, discard, spectaclePoints,
                    name, description, lore, tooltip,
                    imagePath, animationPath, soundPath
                );

            default:
                throw new ArgumentException("Invalid card type", nameof(type));
        }
    }
}