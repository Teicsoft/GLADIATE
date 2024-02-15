namespace GLADIATE.scripts.dialogue;

public class OptionModel
{
    public string Text { get; set; }
    public string NextShot { get; set; }
    public bool EndDialogue { get; set; }
    public string EndVar { get; set; }


    public OptionModel(string nextShot)
    {
        this.NextShot = nextShot;
    }

    public OptionModel(string text, string nextShot)
    {
        this.Text = text;
        this.NextShot = nextShot;
    }

    public OptionModel(bool endDialogue, string endVar)
    {
        this.EndDialogue = endDialogue;
        this.EndVar = endVar;
    }

    public OptionModel(string text, bool endDialogue, string endVar)
    {
        this.Text = text;
        this.EndDialogue = endDialogue;
        this.EndVar = endVar;
    }

    public override string ToString()
    {
        if (EndDialogue)
        {
            if (this.Text != null)
            {
                return $"{nameof(Text)}: {Text}, {nameof(EndDialogue)}: {EndDialogue}, {nameof(EndVar)}: {EndVar}";
            }
            else
            {
                return $"{nameof(EndDialogue)}: {EndDialogue}, {nameof(EndVar)}: {EndVar}";
            }
        }
        else
        {
            if (this.Text != null)
            {
                return $"{nameof(Text)}: {Text}, {nameof(NextShot)}: {NextShot}";
            }
            else
            {
                return $"{nameof(NextShot)}: {NextShot}";
            }
        }
    }
}