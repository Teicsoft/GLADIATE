using System.Collections.Generic;

namespace TeicsoftSpectacleCards.scripts.XmlParsing.models;

public class DeckModel
{
    public string Id { get; set; }
    public string Name { get; set; }

    public UsedByEnum UsedBy { get; set; }

    // public CardModel[] Cards { get; set;}
    public List<Card> CardList;


    public DeckModel(string id, string name, UsedByEnum usedBy, List<Card> cardList)
    {
        this.Id = id;
        this.Name = name;
        this.UsedBy = usedBy;
        this.CardList = cardList;
    }


    public override string ToString()
    {
        return
            $"{nameof(Id)}: {Id}, {nameof(Name)}: {Name}, {nameof(UsedBy)}: {UsedBy}, {nameof(CardList)}: {CardList}";
    }


    public enum UsedByEnum
    {
        Player,
        Enemy
    }
}