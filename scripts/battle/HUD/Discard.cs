using Godot;
using System;
using System.Collections.Generic;

public partial class Discard : ColorRect {

    private List<Card> _discardPile = new();
    public override void _Ready() { }

    public override void _Process(double delta) { }
    public bool IsEmpty() {
        return _discardPile.Count == 0;
    }

    public void AddCards(List<Card> cards) {
        _discardPile.AddRange(cards);
    }

    public List<Card> GetCards(bool clear = true) {
        List<Card> output = new(_discardPile);
        if (clear) { _discardPile.Clear(); }
        return output;
    }
}
