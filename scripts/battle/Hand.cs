#nullable enable
using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class Hand : Path2D {

    private List<Card> Cards = new();

    private int selectedCardIndex = -1;
    private PathFollow2D _handCardLocation;
    public Discard Discard;

    public override void _Ready() {
        _handCardLocation = GetNode<PathFollow2D>("HandCardLocation");
    }

    public new void AddCards(List<Card> cards) {
        foreach (Card card in cards) { AddCard(card); }
    }

    private new void AddCard(Card card) {
        Cards.Add(card);
        AddChild(card);
        card.CardSelected += SelectCard;
        UpdateCardPositions();
    }

    public Card? GetSelectedCard() {
        return selectedCardIndex != -1 ? Cards[selectedCardIndex] : null;
    }

    public void DiscardSelectedCard() {
        Card card = Cards[selectedCardIndex];
        card.CardSelected -= SelectCard;
        Discard.AddCard(card);
        RemoveChild(card);
        Cards.RemoveAt(selectedCardIndex);
        selectedCardIndex = -1;
        UpdateCardPositions();
    }

    private void UpdateCardPositions() {
        float locationRatio = 1f / (Cards.Count + 1);
        for (int i = 0; i < Cards.Count; i++) {
            Card card = Cards[i];
            _handCardLocation.ProgressRatio = (i + 1) * locationRatio;
            Vector2 cardPosition = _handCardLocation.Position;
            if (i == selectedCardIndex) { cardPosition.Y -= 100; }

            card.Position = cardPosition;
        }
    }

    private void SelectCard(Card card) {
        selectedCardIndex = Cards.IndexOf(card);
        UpdateCardPositions();
    }
}
