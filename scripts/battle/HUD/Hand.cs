using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class Hand : Path2D {

    private List<Card> _hand = new();

    private PathFollow2D handCardLocation;

    public override void _Ready() {
        handCardLocation = GetNode<PathFollow2D>("HandCardLocation");
    }

    public override void _Process(double delta) { }

    public void AddCard(Card card) {
        _hand.Add(card);
        AddChild(card);
        card.Pressed += UpdateCardPositions;
        UpdateCardPositions();
    }

    public void PlaySelectedCards(Enemy enemy) {
        List<Card> selectedCards = _hand.Where(card => card.Selected).ToList();
        foreach (Card card in selectedCards) { card.Target(enemy); }

        DiscardSelectedCards();
    }

    private void DiscardSelectedCards() {
        List<Card> selectedCards = _hand.Where(card => card.Selected).ToList();
        _hand.RemoveAll(card => card.Selected);
        foreach (Card card in selectedCards) { card.QueueFree(); }

        UpdateCardPositions();
    }

    private void UpdateCardPositions() {
        float locationRatio = 1f / (_hand.Count + 1);
        for (int i = 0; i < _hand.Count; i++) {
            Card card = _hand[i];
            handCardLocation.ProgressRatio = (i + 1) * locationRatio;
            Vector2 cardPosition = handCardLocation.Position;
            if (card.Selected) { cardPosition.Y -= 100; }

            card.Position = cardPosition;
        }
    }
}
