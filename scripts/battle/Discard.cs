using Godot;
using System;
using System.Collections.Generic;

public class Discard {
    public List<Card> Cards = new();

    public void AddCard(Card card) {
        Cards.Add(card);
    }

    public void AddCards(List<Card> cards) {
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
