using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class HUD : CanvasLayer {

    private static readonly List<Color> Colors = new() {
        new(1.0f, 0.0f, 0.0f),
        new(1.0f, 0.0f, 0.0f),
        new(1.0f, 0.0f, 0.0f),
        new(1.0f, 0.0f, 0.0f),
        new(0.0f, 1.0f, 0.0f),
        new(0.0f, 1.0f, 0.0f),
        new(0.0f, 0.0f, 1.0f),
        new(0.0f, 0.0f, 1.0f),
    };

    private static readonly Color[] Palette = {
        new(1.0f, 0.0f, 0.0f),
        new(1.0f, 1.0f, 0.0f),
        new(0.0f, 1.0f, 0.0f),
        new(0.0f, 1.0f, 1.0f),
        new(0.0f, 0.0f, 1.0f),
        new(1.0f, 0.0f, 1.0f),
        new(1.0f, 1.0f, 1.0f),
        new(0.0f, 0.0f, 0.0f),
    };

    [Export] private PackedScene _cardScene;
    private Hand _hand;
    private Deck _deck;
    private Discard _discard;

    public override void _Ready() {
        _hand = GetNode<Hand>("Hand");
        _deck = GetNode<Deck>("Deck");
        _discard = GetNode<Discard>("Discard");
        List<Card> initialDeck = new();
        foreach (Color color in Colors) {
            Card card = _cardScene.Instantiate<Card>();
            card.Color = color;
            card.AddThemeColorOverride("font_color", card.Color);
            initialDeck.Add(card);
        }

        Card lastCard = _cardScene.Instantiate<Card>();
        lastCard.Color = Palette[GD.Randi() % Palette.Length];
        lastCard.AddThemeColorOverride("font_color", lastCard.Color);
        initialDeck.Add(lastCard);

        _deck.AddCards(initialDeck);
        _deck.Shuffle();
    }

    public override void _Process(double delta) { }

    private void OnDeckPressed() {
        _hand.AddCards(_deck.DrawCard());
    }

    private void OnDeckEmptied(int drawAmount) {
        if (!_discard.IsEmpty()) {
            _deck.AddCards(_discard.GetCards());
            _deck.Shuffle();
            if (drawAmount > 0 && !_deck.IsEmpty()) { _hand.AddCards(_deck.DrawCard(drawAmount)); }
        }
    }
}
