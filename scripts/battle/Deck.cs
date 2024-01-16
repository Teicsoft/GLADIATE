using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class Deck : Node2D {

    public Discard Discard;
    public List<Card> Cards = new();

    public static List<Card> Shuffle(List<Card> input) {
        List<Card> deck = new(input);
        for (int i = deck.Count - 1; i > 1; i--) {
            int j = (int)(GD.Randi() % i + 1);
            (deck[i], deck[j]) = (deck[j], deck[i]);
        }

        return deck;
    }

    public void AddCard(Card card) { // Adds cards to TOP of deck, highest index on top.
        Cards.Add(card);
    }

    public void AddCards(List<Card> cards) { // Adds cards to TOP of deck, highest index on top.
        Cards.AddRange(cards);
    }

    public bool IsEmpty() {
        return Cards.Count == 0;
    }

    public void Shuffle() {
        Cards = Shuffle(Cards);
    }

    public List<Card> DrawCard() {
        return DrawCards(1);
    }

    public List<Card> DrawCards(int amount) {
        List<Card> draw = new();
        if (amount > 0) {
            if (Cards.Count == 0) { return OnDeckEmptied(amount); }

            if (Cards.Count > 0) {
                draw.Add(Cards[^1]);
                Cards.RemoveAt(Cards.Count - 1);
                draw.AddRange(DrawCards(amount - 1));
            }
        }

        return draw;
    }

    private List<Card> OnDeckEmptied(int amount) {
        if (!Discard.IsEmpty()) {
            AddCards(Discard.GetCards());
            Shuffle();
            return DrawCards(amount);
        }

        return new();
    }
}
