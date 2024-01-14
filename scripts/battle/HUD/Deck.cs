using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class Deck : Button {

    [Signal]
    public delegate void DeckEmptyEventHandler(int drawAmount = 0);

    private List<Card> _deck = new();

    public override void _Ready() { }

    public override void _Process(double delta) { }

    public bool IsEmpty() {
        return _deck.Count == 0;
    }

    public void Shuffle() {
        _deck = Shuffle(_deck);
    }

    public static List<Card> Shuffle(List<Card> input) {
        List<Card> deck = new(input);
        GD.Print(" deck.Count: "+deck.Count);
        for (int i = deck.Count-1; i > 1; i--) {
            int j = (int)(GD.Randi() % i + 1);
            (deck[i], deck[j]) = (deck[j], deck[i]);
            GD.Print(" i: "+i);
            GD.Print(" j: "+j);
        }

        return deck;
    }

    public void AddCards(List<Card> cards) { // Adds cards to TOP of deck, highest index on top.
        GD.Print("ADDING "+cards.Count+" CARDS");
        _deck.AddRange(cards);
        GD.Print(" _deck.Count: "+_deck.Count);
    }

    public List<Card> DrawCard(int amount = 1) {
        if (_deck.Count == 0) {
            EmitSignal(SignalName.DeckEmpty, amount);
            return new();
        }

        List<Card> draw = new();
        if (amount >= _deck.Count) {
            draw.AddRange(_deck);
            amount -= _deck.Count;
            _deck.Clear();
            if (amount > 0) { EmitSignal(SignalName.DeckEmpty, amount); }
        } else if (amount > 0 && _deck.Count > 0) {
            draw.Add(_deck[^1]);
            _deck.RemoveAt(_deck.Count - 1);
            draw.AddRange(DrawCard(amount - 1));
        }
        return draw;
    }
}
