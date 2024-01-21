using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using TeicsoftSpectacleCards.scripts.battle.card;

public class Deck {
    public string Id { get; set; }
    public string Name { get; set; }
    public UsedBy Owner { get; set; }
    
    public Discard Discard {get; set; }
    public List<CardSleeve> CardSleeves;

    public Deck() {
        CardSleeves = new();
    }
    
    public Deck(Discard discard) {
        this.Discard = discard;
        CardSleeves = new();
    }
    
    public Deck Initialize(string id, string name, UsedBy usedBy, List<CardSleeve> cardList)
    {
        this.Id = id;
        this.Name = name;
        this.Owner = usedBy;
        this.CardSleeves = cardList;
        return this;
    }

    public static List<CardSleeve> Shuffle(List<CardSleeve> input) {
        List<CardSleeve> deck = new(input);
        for (int i = deck.Count - 1; i > 1; i--) {
            int j = (int)(GD.Randi() % i + 1);
            (deck[i], deck[j]) = (deck[j], deck[i]);
        }

        return deck;
    }

    public void AddCard(CardSleeve cardSleeve) {
        // Adds cards to TOP of deck, highest index on top.
        CardSleeves.Add(cardSleeve);
    }

    public void AddCards(List<CardSleeve> cardSleeves) {
        // Adds cards to TOP of deck, highest index on top.
        this.CardSleeves.AddRange(cardSleeves);
    }

    public bool IsEmpty() {
        return CardSleeves.Count == 0;
    }

    public void Shuffle() {
        CardSleeves = Shuffle(CardSleeves);
    }

    public List<CardSleeve> DrawCards(int amount) {
        List<CardSleeve> draw = new();
        if (amount > 0) {
            if (CardSleeves.Count == 0)
            {
                GD.Print("Deck is empty!");
                return OnDeckEmptied(amount);
            }

            if (CardSleeves.Count > 0) {
                draw.Add(CardSleeves[^1]);
                CardSleeves.RemoveAt(CardSleeves.Count - 1);
                draw.AddRange(DrawCards(amount - 1));
            }
        }

        return draw;
    }

    private List<CardSleeve> OnDeckEmptied(int amount) {
        if (Discard.IsEmpty()) { return new(); }

        AddCards(Discard.GetCards());
        Shuffle();
        return DrawCards(amount);
    }
    
    public override string ToString()
    {
        return
            $"{nameof(Id)}: {Id}, {nameof(Name)}: {Name}, {nameof(Owner)}: {Owner}, {nameof(CardSleeves)}: {CardSleeves}";
    }
    
    public enum UsedBy
    {
        Player,
        Enemy
    }

    public static List<CardSleeve> SleeveCards(List <Card> cards)
    {
        PackedScene cardScene = GD.Load<PackedScene>("res://scenes/battle/Card.tscn");
        
        List<CardSleeve> sleevedCards = new();

        foreach (Card card in cards)
        {
            CardSleeve cardSleeve = cardScene.Instantiate<CardSleeve>();
            cardSleeve.Card = card;
            sleevedCards.Add(cardSleeve);
        }
        
        return sleevedCards;
    }
}
