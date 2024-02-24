#nullable enable
using Godot;
using System.Collections.Generic;
using System.Linq;
using GLADIATE.scripts.battle.card;

public partial class Hand : Path2D {
    private const int MAX_HAND_SIZE = 10;
    public List<CardSleeve> Cards = new();

    private int _selectedCardIndex = -1;
    public Deck<CardSleeve> Deck;
    public Discard<CardSleeve> Discard;

    public override void _Ready() {
        Discard = new Discard<CardSleeve>();
        Deck = new Deck<CardSleeve>(Discard);
    }

    public void DrawCards(int n) {
        AddCards(Deck.DrawCards(n + Cards.Count > MAX_HAND_SIZE ? MAX_HAND_SIZE - Cards.Count : n));
    }

    public void InitialiseDeck(List<string> cardIds) {
        Deck.AddCards(Deck<CardSleeve>.SleeveCards(cardIds.Select(CardFactory.CloneCard).ToList()));
        Deck.Shuffle();
    }

    public void AddCards(List<CardSleeve> cardSleeves) {
        if (cardSleeves.Count > 0) {
            foreach (CardSleeve cardSleeve in cardSleeves) { AddCard(cardSleeve); }
        }
    }

    private void AddCard(CardSleeve cardSleeve) {
        Cards.Add(cardSleeve);
        AddChild(cardSleeve);
        cardSleeve.CardSelected += SelectCard;
        UpdateCardPositions();
    }

    public CardSleeve GetSelectedCard() { return _selectedCardIndex != -1 ? Cards[_selectedCardIndex] : null; }

    public void DiscardCard() { DiscardCard(Cards[_selectedCardIndex]); }

    public void DiscardCard(CardSleeve cardSleeve) {
        cardSleeve.CardSelected -= SelectCard;
        Discard.AddCard(cardSleeve);
        if (_selectedCardIndex == -1) { Cards.Remove(cardSleeve); } else {
            Cards.RemoveAt(_selectedCardIndex);
            _selectedCardIndex = -1;
        }
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
