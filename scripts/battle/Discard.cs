using Godot;
using System;
using System.Collections.Generic;

public class Discard {
    public List<CardSleeve> cards = new();

    public void AddCard(CardSleeve cardSleeve) {
        cards.Add(cardSleeve);
    }

    public void AddCards(List<CardSleeve> cards) {
        this.cards.AddRange(cards);
    }

    public bool IsEmpty() {
        return cards.Count == 0;
    }

    public List<CardSleeve> GetCards(bool clear = true) {
        List<CardSleeve> output = new(cards);
        if (clear) { cards.Clear(); }

        return output;
    }
}
