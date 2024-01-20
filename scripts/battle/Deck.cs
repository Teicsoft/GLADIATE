using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public class Deck {
    public string Id { get; set; }
    public string Name { get; set; }
    public UsedBy Owner { get; set; }

    private Discard discard;
    public List<Card> cards;

    public Deck() {
        cards = new();
    }

    public Deck(Discard discard) {
        this.discard = discard;
        cards = new();
    }

    public Deck Initialize(string id, string name, UsedBy usedBy, List<Card> cardList)
    {
        this.Id = id;
        this.Name = name;
        this.Owner = usedBy;
        this.cards = cardList;
        return this;
    }

    public static List<Card> Shuffle(List<Card> input) {
        List<Card> deck = new(input);
        for (int i = deck.Count - 1; i > 1; i--) {
            int j = (int)(GD.Randi() % i + 1);
            (deck[i], deck[j]) = (deck[j], deck[i]);
        }

        return deck;
    }

    public void AddCard(Card card) {
        // Adds cards to TOP of deck, highest index on top.
        cards.Add(card);
    }

    public void AddCards(List<Card> cards) {
        // Adds cards to TOP of deck, highest index on top.
        this.cards.AddRange(cards);
    }

    public bool IsEmpty() {
        return cards.Count == 0;
    }

    public void Shuffle() {
        cards = Shuffle(cards);
    }

    public List<Card> DrawCards(int amount) {
        List<Card> draw = new();
        if (amount > 0) {
            if (cards.Count == 0) { return OnDeckEmptied(amount); }

            if (cards.Count > 0) {
                draw.Add(cards[^1]);
                cards.RemoveAt(cards.Count - 1);
                draw.AddRange(DrawCards(amount - 1));
            }
        }

        return draw;
    }

    private List<Card> OnDeckEmptied(int amount) {
        if (discard.IsEmpty()) { return new(); }

        AddCards(discard.GetCards());
        Shuffle();
        return DrawCards(amount);
    }

    public override string ToString()
    {
        return
            $"{nameof(Id)}: {Id}, {nameof(Name)}: {Name}, {nameof(Owner)}: {Owner}, {nameof(cards)}: {cards}";
    }

    public enum UsedBy
    {
        Player,
        Enemy
    }
}
