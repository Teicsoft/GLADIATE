using System;

namespace TeicsoftSpectacleCards.scripts.customresource.Cards;

public static class CardFactory
{
    public static CardModel MakeCard(
        string type, string cardId, CardModel.ModifierEnum modifier, CardModel.PositionEnum position,
        int attack, int defenseLower, int defenseUpper, int health, int draw, int discard, int spectaclePoints, 
        string name, string description, string lore, string tooltip, string 
            imagePath, string animationPath, string soundPath)
    {
        switch (type)
        {
            case"Attack":
                return new AttackCardModel(
                    cardId, modifier, position, 
                    attack, defenseLower, defenseUpper, health, draw, discard, spectaclePoints,
                    name, description, lore, tooltip,
                    imagePath, animationPath, soundPath
                    );
            case "Draw":
                return new DrawCardModel(
                    cardId, modifier, position,
                    attack, defenseLower, defenseUpper, health, draw, discard, spectaclePoints,
                    name, description, lore, tooltip,
                    imagePath, animationPath, soundPath
                    );
            case "Block":
                return new BlockCardModel(
                    cardId, modifier, position, 
                    attack, defenseLower, defenseUpper, health, draw, discard, spectaclePoints,
                    name, description, lore, tooltip,
                    imagePath, animationPath, soundPath
                    );
            case "Modifier":
                return new ModifierCardModel(
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