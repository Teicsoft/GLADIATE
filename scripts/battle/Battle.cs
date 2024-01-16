using Godot;
using TeicsoftSpectacleCards.scripts.customresource;
using TeicsoftSpectacleCards.scripts.customresource.Cards;
using TeicsoftSpectacleCards.scripts.customresource.combos;
using TeicsoftSpectacleCards.scripts.customresource.deck;
using TeicsoftSpectacleCards.scripts.XmlParsing;

namespace Battle;

public partial class Battle : Node2D {

	public override void _Ready() {
		Enemy enemy1 = GetNode<Enemy>("Enemy1");
		Enemy enemy2 = GetNode<Enemy>("Enemy2");
		Hand hand = GetNode<Hand>("HUD/Hand");
		enemy1.EnemyAttacked += hand.PlaySelectedCards;
		enemy2.EnemyAttacked += hand.PlaySelectedCards;
		
		
		
		//This is a test to see if the card factory works, feel free to remove it
		CardModel card = CardXmlParser.ParseCardsFromXml("res://data/cards/card_template.xml");
		GD.Print(card.ToString());
		
		//This is a test to see if the combo parsing works, feel free to remove it
		ComboModel combo = ComboXmlParser.ParseComboFromXml("res://data/combos/combo_template.xml");
		GD.Print(combo.ToString());
		//
		// //This is a test to see if the Deck parsing works, feel free to remove it
		DeckModel deck = DeckXmlParser.ParseDeckFromXml("res://data/decks/deck_template.xml");
	}

	public override void _Process(double delta) { }
}
