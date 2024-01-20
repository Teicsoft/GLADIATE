using System.Collections.Generic;
using TeicsoftSpectacleCards.scripts.XmlParsing;

namespace TeicsoftSpectacleCards.scripts.battle.card;

public static class CardPrototypes
{
    public static Dictionary<string, Card> cardPrototypeDict = CardXmlParser.ParseAllCards();
    
    
    public static Card CloneCard(string cardId)
    {
        return cardPrototypeDict[cardId].Clone();
    }
    
    public static void ReloadCardPrototypes()
    {
        cardPrototypeDict = CardXmlParser.ParseAllCards();
    }
}