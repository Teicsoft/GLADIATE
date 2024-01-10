namespace TeicsoftSpectacleCards.scripts.dialogue;

public class LineModel
{
    public string text { get; set;}
    public string character_id { get; set;}
    public string animation_id { get; set;}
    public string override_position { get; set;}
    
    public LineModel(string text)
    {
        this.text = text;
    }

    public LineModel(string text, string characterId)
    {
        this.text = text;
        this.character_id = characterId;
    }
    
    public LineModel(string text, string characterId, string animationId)
    {
        this.text = text;
        this.character_id = characterId;
        this.animation_id = animationId;
    }

    public LineModel(string text, string characterId, string animationId, string overridePosition)
    {
        this.text = text;
        this.character_id = characterId;
        this.animation_id = animationId;
        this.override_position = overridePosition;
    }
    
}