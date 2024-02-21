namespace GLADIATE.scripts.dialogue;

public class LineModel
{
    public string Text { get; set; }
    public string CharacterId { get; set; }
    public string AnimationId { get; set; }
    public string OverridePosition { get; set; }

    public LineModel(string text)
    {
        this.Text = text;
    }

    public LineModel(string text, string characterId)
    {
        this.Text = text;
        this.CharacterId = characterId;
    }

    public LineModel(string text, string characterId, string animationId)
    {
        this.Text = text;
        this.CharacterId = characterId;
        this.AnimationId = animationId;
    }

    public LineModel(string text, string characterId, string animationId, string overridePosition)
    {
        this.Text = text;
        this.CharacterId = characterId;
        this.AnimationId = animationId;
        this.OverridePosition = overridePosition;
    }

    public override string ToString()
    {
        return
            $"{nameof(Text)}: {Text}, {nameof(CharacterId)}: {CharacterId}, {nameof(AnimationId)}: {AnimationId}, {nameof(OverridePosition)}: {OverridePosition}";
    }
}
