#nullable enable
using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class Hand : Path2D {
    private List<Card> cards = new();

    private int selectedCardIndex = -1;
    private PathFollow2D handCardLocation;
    public Discard discard;

    public override void _Ready() {
        handCardLocation = GetNode<PathFollow2D>("HandCardLocation");
    }

    public new void AddCards(List<Card> cards) {
        foreach (Card card in cards) { AddCard(card); }
    }

    private new void AddCard(Card card) {
        cards.Add(card);
        AddChild(card);
        card.CardSelected += SelectCard;
        UpdateCardPositions();
    }

    public Card GetSelectedCard() {
        return selectedCardIndex != -1 ? cards[selectedCardIndex] : null;
    }

    public void Discard() { Discard(cards[selectedCardIndex]); }

    public void Discard(Card card) {
        card.CardSelected -= SelectCard;
        discard.AddCard(card);
        cards.RemoveAt(selectedCardIndex);
        selectedCardIndex = -1;
        RemoveChild(card);
        UpdateCardPositions();
    }

    private void UpdateCardPositions() {
        float locationRatio = 1f / (cards.Count + 1);
        for (int i = 0; i < cards.Count; i++) {
            Card card = cards[i];
            handCardLocation.ProgressRatio = (i + 1) * locationRatio;
            Vector2 cardPosition = handCardLocation.Position;
            if (i == selectedCardIndex) { cardPosition.Y -= 100; }

            card.Position = cardPosition;
            card.Rotation = handCardLocation.Rotation;
        }
    }

    private void SelectCard(Card card) {
        int cardIndex = cards.IndexOf(card);
        selectedCardIndex = selectedCardIndex != cardIndex ? cardIndex : -1;
        UpdateCardPositions();
    }
}
