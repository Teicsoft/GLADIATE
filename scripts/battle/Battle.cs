using Godot;
using TeicsoftSpectacleCards.scripts.customresource;
using TeicsoftSpectacleCards.scripts.customresource.Cards;

namespace Battle;

public partial class Battle : Node2D {

	public override void _Ready() {
		Enemy enemy1 = GetNode<Enemy>("Enemy1");
		Enemy enemy2 = GetNode<Enemy>("Enemy2");
		Hand hand = GetNode<Hand>("HUD/Hand");
		enemy1.EnemyAttacked += hand.PlaySelectedCards;
		enemy2.EnemyAttacked += hand.PlaySelectedCards;
		
		CardModel card = CardXmlParser.ParseCardsFromXml("res://data/cards/card_template.xml");
		GD.Print(card.ToString());
	}

	public override void _Process(double delta) { }
}
