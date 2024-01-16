using Godot;
using System;
using System.Collections.Generic;

public partial class Discard {
    public List<Card> Cards = new();

    public void AddCard(Card card) { // Adds cards to TOP of deck, highest index on top.
        Cards.Add(card);
    }

    public void AddCards(List<Card> cards) { // Adds cards to TOP of deck, highest index on top.
        Cards.AddRange(cards);
    }

    public bool IsEmpty() {
        return Cards.Count == 0;
    }

    public List<Card> GetCards(bool clear = true) {
        List<Card> output = new(Cards);
        if (clear) { Cards.Clear(); }

        return output;
    }
}
