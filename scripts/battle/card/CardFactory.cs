using System;
using System.Collections.Generic;
using Godot;

namespace TeicsoftSpectacleCards.scripts.battle.card;

public static class CardFactory {

    private static Dictionary<string, Func<Card>> TypeDictionary = new() {
        { "card_05", () => new StunCard() },
    };

    public static Card MakeBlankCard(string cardId) {
        Card card = new Card();
        card.Id = cardId;
        return card;
    }

    public static Card MakeCard(string type, string cardId, Utils.ModifierEnum modifier, Utils.PositionEnum position,
        int attack, int defenseLower, int defenseUpper, int health, int draw, int discard, int spectaclePoints,
        string name, string description, string lore, string tooltip, string imagePath, string animationPath,
        string soundPath,bool targetRequired) {
        Card card = TypeDictionary.GetValueOrDefault(cardId, () => new Card()).Invoke();

        return card.Initialize(cardId, targetRequired, attack, defenseLower, defenseUpper, health, draw, discard, spectaclePoints,
            name, description, lore, tooltip, imagePath, animationPath, soundPath);
    }
}
