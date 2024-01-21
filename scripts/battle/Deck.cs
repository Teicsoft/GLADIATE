using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public class Deck {
    public string Id { get; set; }
    public string Name { get; set; }
    public UsedBy Owner { get; set; }

    public Discard Discard {get; set; }
    public List<Card> Cards;

    public Deck() {
        Cards = new();
    }

    public Deck(Discard discard) {
        this.Discard = discard;
        Cards = new();
    }

    public Deck Initialize(string id, string name, UsedBy usedBy, List<Card> cardList)
    {
        this.Id = id;
        this.Name = name;
        this.Owner = usedBy;
        this.Cards = cardList;
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
        Cards.Add(card);
    }

    public void AddCards(List<Card> cards) {
        // Adds cards to TOP of deck, highest index on top.
        this.Cards.AddRange(cards);
    }

    public bool IsEmpty() {
        return Cards.Count == 0;
    }

    public void Shuffle() {
        Cards = Shuffle(Cards);
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
        if (Discard.IsEmpty()) { return new(); }

        AddCards(Discard.GetCards());
        Shuffle();
        return DrawCards(amount);
    }

    public override string ToString()
    {
        return
            $"{nameof(Id)}: {Id}, {nameof(Name)}: {Name}, {nameof(Owner)}: {Owner}, {nameof(Cards)}: {Cards}";
    }

    public enum UsedBy
    {
        Player,
        Enemy
    }
}
