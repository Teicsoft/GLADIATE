using Godot;
using System;
using System.Collections.Generic;

public class Discard<T> {
    public List<T> Cards = new();

    public void AddCard(T card) {
        Cards.Add(card);
    }

    public void AddCards(List<T> cards) {
        Cards.AddRange(cards);
    }

    public bool IsEmpty() {
        return Cards.Count == 0;
    }

    public List<T> GetCards(bool clear = true) {
        List<T> output = new(Cards);
        if (clear) { Cards.Clear(); }

        return output;
    }
}
