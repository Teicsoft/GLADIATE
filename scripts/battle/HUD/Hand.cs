using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class Hand : Path2D {

    private List<Card> _hand = new();

    private PathFollow2D _handCardLocation;
    [Export] private Discard _discard;

    public override void _Ready() {
        _handCardLocation = GetNode<PathFollow2D>("HandCardLocation");
    }

    public override void _Process(double delta) { }

    public void AddCards(List<Card> cards) {
        foreach (Card card in cards) {
            _hand.Add(card);
            AddChild(card);
            card.Pressed += UpdateCardPositions;
            UpdateCardPositions();
        }
    }

    public void PlaySelectedCards(Enemy enemy) {
        List<Card> selectedCards = _hand.Where(card => card.Selected).ToList();
        foreach (Card card in selectedCards) { card.Target(enemy); }

        DiscardSelectedCards();
    }

    private void DiscardSelectedCards() {
        List<Card> selectedCards = _hand.Where(card => card.Selected).ToList();
        _discard.AddCards(selectedCards);
        foreach (Card card in selectedCards) {
            card.Selected = false;
            card.Pressed -= UpdateCardPositions;
            RemoveChild(card);
        }
        _hand.RemoveAll(selectedCards.Contains);
        UpdateCardPositions();
    }

    private void UpdateCardPositions() {
        float locationRatio = 1f / (_hand.Count + 1);
        for (int i = 0; i < _hand.Count; i++) {
            Card card = _hand[i];
            _handCardLocation.ProgressRatio = (i + 1) * locationRatio;
            Vector2 cardPosition = _handCardLocation.Position;
            if (card.Selected) { cardPosition.Y -= 100; }

            card.Position = cardPosition;
        }
    }
}
