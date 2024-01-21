#nullable enable
using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class Hand : Path2D {
    private List<Card> Cards = new();

    private int SelectedCardIndex = -1;
    private PathFollow2D HandCardLocation;
    public Discard Discard;

    public override void _Ready() {
        HandCardLocation = GetNode<PathFollow2D>("HandCardLocation");
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

    public Card GetSelectedCard() {
        return SelectedCardIndex != -1 ? Cards[SelectedCardIndex] : null;
    }

    public void DiscardSelectedCard() { DiscardCard(Cards[SelectedCardIndex]); }

    public void DiscardCard(Card card) {
        card.CardSelected -= SelectCard;
        Discard.AddCard(card);
        Cards.RemoveAt(SelectedCardIndex);
        SelectedCardIndex = -1;
        RemoveChild(card);
        UpdateCardPositions();
    }

    private void UpdateCardPositions() {
        float locationRatio = 1f / (Cards.Count + 1);
        for (int i = 0; i < Cards.Count; i++) {
            Card card = Cards[i];
            HandCardLocation.ProgressRatio = (i + 1) * locationRatio;
            Vector2 cardPosition = HandCardLocation.Position;
            if (i == SelectedCardIndex) { cardPosition.Y -= 100; }

            card.Position = cardPosition;
            card.Rotation = HandCardLocation.Rotation;
        }
    }

    private void SelectCard(Card card) {
        int cardIndex = Cards.IndexOf(card);
        SelectedCardIndex = SelectedCardIndex != cardIndex ? cardIndex : -1;
        UpdateCardPositions();
    }
}
