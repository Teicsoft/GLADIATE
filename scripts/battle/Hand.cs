#nullable enable
using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class Hand : Path2D {
    public List<CardSleeve> Cards = new();

    private int _selectedCardIndex = -1;
    public Discard<CardSleeve> Discard;

    public override void _Ready() { }

    public new void AddCards(List<CardSleeve> cardSleeves) {
        if (cardSleeves.Count > 0) {
            foreach (CardSleeve cardSleeve in cardSleeves) { AddCard(cardSleeve); }
        }
    }

    private new void AddCard(CardSleeve cardSleeve) {
        Cards.Add(cardSleeve);
        AddChild(cardSleeve);
        cardSleeve.CardSelected += SelectCard;
        UpdateCardPositions();
    }

    public CardSleeve GetSelectedCard() {
        return _selectedCardIndex != -1 ? Cards[_selectedCardIndex] : null;
    }

    public void DiscardCard() {
        DiscardCard(Cards[_selectedCardIndex]);
    }

    public void DiscardCard(CardSleeve cardSleeve) {
        cardSleeve.CardSelected -= SelectCard;
        Discard.AddCard(cardSleeve);
        Cards.RemoveAt(_selectedCardIndex);
        _selectedCardIndex = -1;
        RemoveChild(cardSleeve);
        UpdateCardPositions();
    }

    private void UpdateCardPositions() {
        PathFollow2D handCardLocation = GetNode<PathFollow2D>("HandCardLocation");
        for (int i = 0; i < Cards.Count; i++) {
            CardSleeve cardSleeve = Cards[i];
            handCardLocation.ProgressRatio = (i + 1f) / (Cards.Count + 1f);
            Vector2 cardPosition = handCardLocation.Position;
            if (i == _selectedCardIndex) { cardPosition.Y -= 100; }
            cardSleeve.Position = cardPosition;
            cardSleeve.Rotation = handCardLocation.Rotation;
        }
    }

    private void SelectCard(CardSleeve cardSleeve) {
        int cardIndex = Cards.IndexOf(cardSleeve);
        _selectedCardIndex = _selectedCardIndex != cardIndex ? cardIndex : -1;
        UpdateCardPositions();
    }
}
