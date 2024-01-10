namespace TeicsoftSpectacleCards.scripts.dialogue;

public class LocationModel
{
    public string id { get; set;}
    public string loc_name { get; set;}
    public string image { get; set;}
    // public string[] animations { get; set;}

    public LocationModel(string id, string locName, string image) //string[] animations)
    {
        this.id = id;
        this.loc_name = locName;
        this.image = image;
        // this.animations = animations;
    }
    
    public override string ToString()
    {
        return $"{nameof(id)}: {id}, {nameof(loc_name)}: {loc_name}, {nameof(image)}: {image}"; //, {nameof(animations)}: {animations}"; 
    }
    
}