using System;
using Godot;
using System.Collections.Generic;
using GLADIATE.scripts.battle.card;

public class Deck<T> {
    public event EventHandler DeckShuffledCustomEvent;

    public string Id { get; set; }
    public string Name { get; set; }

    public Discard<T> Discard { get; set; }
    public List<T> Cards = new();

    public Deck(Discard<T> discard) {
        Discard = discard;
    }

    public static List<CardSleeve> SleeveCards(List<Card> cards) {
        PackedScene cardScene = GD.Load<PackedScene>("res://scenes/battle/Card.tscn");

        List<CardSleeve> sleevedCards = new();

        foreach (Card card in cards) {
            CardSleeve cardSleeve = cardScene.Instantiate<CardSleeve>();
            cardSleeve.Card = card;
            sleevedCards.Add(cardSleeve);
        }

        return sleevedCards;
    }

    public static List<T> Shuffle(List<T> input) {
        List<T> deck = new(input);
        for (int i = deck.Count - 1; i > 1; i--) {
            int j = (int)(GD.Randi() % i + 1);
            (deck[i], deck[j]) = (deck[j], deck[i]);
        }

        return deck;
    }

    public void AddCard(T card) {
        // Adds cards to TOP of deck, highest index on top.
        Cards.Add(card);
    }

    public void AddCards(List<T> cards) {
        // Adds cards to TOP of deck, highest index on top.
        Cards.AddRange(cards);
    }

    public bool IsEmpty() { return Cards.Count == 0; }

    public void Shuffle() {
        Cards = Shuffle(Cards);
        DeckShuffledCustomEvent?.Invoke(this, EventArgs.Empty);
    }

    public List<T> DrawCards(int amount) {
        List<T> draw = new();
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

    private List<T> OnDeckEmptied(int amount) {
        if (Discard.IsEmpty()) { return new(); }
        AddCards(Discard.GetCards());
        Shuffle();
        return DrawCards(amount);
    }

    public override string ToString() {
        return $"{nameof(Id)}: {Id}, {nameof(Name)}: {Name}, {nameof(Cards)}: {Cards}";
    }
}
