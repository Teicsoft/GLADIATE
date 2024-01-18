namespace TeicsoftSpectacleCards.scripts.dialogue;

public class DialogueModel
{
    public CharacterModel[] CharactersPresent { get; set; }
    public LocationModel[] LocationsPresent { get; set; }
    public ShotModel[] ShotList { get; set; }

    public DialogueModel(CharacterModel[] charactersPresent, LocationModel[] locationsPresent, ShotModel[] shotList)
    {
        this.CharactersPresent = charactersPresent;
        this.LocationsPresent = locationsPresent;
        this.ShotList = shotList;
    }

    public override string ToString()
    {
        return
            $"{nameof(CharactersPresent)}: {CharactersPresent}, {nameof(LocationsPresent)}: {LocationsPresent}, {nameof(ShotList)}: {ShotList}";
    }
}