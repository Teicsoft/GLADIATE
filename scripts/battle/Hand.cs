#nullable enable
using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class Hand : Path2D {
    private List<CardSleeve> cards = new();

    private int selectedCardIndex = -1;
    private PathFollow2D handCardLocation;
    public Discard discard;

    public override void _Ready() {
        handCardLocation = GetNode<PathFollow2D>("HandCardLocation");
    }

    public new void AddCards(List<CardSleeve> cardSleeves) {
        foreach (CardSleeve cardSleeve in cardSleeves)
        {
            AddCard(cardSleeve);
        }

    }

    private new void AddCard(CardSleeve cardSleeve) {
        cards.Add(cardSleeve);
        AddChild(cardSleeve);
        cardSleeve.CardSelected += SelectCard;
        UpdateCardPositions();
    }

    public CardSleeve GetSelectedCard() {
        return selectedCardIndex != -1 ? cards[selectedCardIndex] : null;
    }

    public void Discard() { Discard(cards[selectedCardIndex]); }

    public void Discard(CardSleeve cardSleeve) {
        cardSleeve.CardSelected -= SelectCard;
        discard.AddCard(cardSleeve);
        cards.RemoveAt(selectedCardIndex);
        selectedCardIndex = -1;
        RemoveChild(cardSleeve);
        UpdateCardPositions();
    }

    private void UpdateCardPositions() {
        float locationRatio = 1f / (cards.Count + 1);
        for (int i = 0; i < cards.Count; i++) {
            CardSleeve cardSleeve = cards[i];
            handCardLocation.ProgressRatio = (i + 1) * locationRatio;
            Vector2 cardPosition = handCardLocation.Position;
            if (i == selectedCardIndex) { cardPosition.Y -= 100; }

            cardSleeve.Position = cardPosition;
            cardSleeve.Rotation = handCardLocation.Rotation;
        }
    }

    private void SelectCard(CardSleeve cardSleeve) {
        int cardIndex = cards.IndexOf(cardSleeve);
        selectedCardIndex = selectedCardIndex != cardIndex ? cardIndex : -1;
        UpdateCardPositions();
    }
}
