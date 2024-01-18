using Godot;
using System;
using System.Collections.Generic;

public class Discard
{
    public List<Card> cards = new();

    public void AddCard(Card card)
    {
        cards.Add(card);
    }

    public void AddCards(List<Card> cards)
    {
        this.cards.AddRange(cards);
    }

    public bool IsEmpty()
    {
        return cards.Count == 0;
    }

    public List<Card> GetCards(bool clear = true)
    {
        List<Card> output = new(cards);
        if (clear)
        {
            cards.Clear();
        }

        return output;
    }
}