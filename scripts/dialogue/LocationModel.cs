namespace TeicsoftSpectacleCards.scripts.dialogue;

public class LocationModel
{
    public string Id { get; set;}
    public string LocName { get; set;}
    public string BgImage { get; set;}
    public string[] Animations { get; set;}

    public LocationModel(string id, string locName, string bgImage, string[] animations)
    {
        this.Id = id;
        this.LocName = locName;
        this.BgImage = bgImage;
        this.Animations = animations;
    }
    
    public override string ToString()
    {
        return $"{nameof(Id)}: {Id}, {nameof(LocName)}: {LocName}, {nameof(BgImage)}: {BgImage}, {nameof(Animations)}: {Animations}"; 
    }
    
}