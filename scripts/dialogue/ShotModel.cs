namespace TeicsoftSpectacleCards.scripts.dialogue;

public class ShotModel
{
    public string Id { get; set;}
    public string LocationId { get; set;}
    public LineModel[] Lines { get; set;}
    public OptionModel[] Options { get; set;}
    
    
    public ShotModel(string id, string locationId, LineModel[] lines, OptionModel[] options)
    {
        this.Id = id;
        this.LocationId = locationId;
        this.Lines = lines;
        this.Options = options;
    }
    
    public override string ToString()
    {
        return $"{nameof(Id)}: {Id}, {nameof(LocationId)}: {LocationId}, {nameof(Lines)}: {Lines}, {nameof(Options)}: {Options}";
    }
}