using System;

namespace TeicsoftSpectacleCards.scripts.dialogue;

public class OptionModel
{
    public string text { get; set;}
    public string next_shot { get; set;}
    public bool end_dialogue { get; set;}
    public string end_var { get; set;}
    
    
    public OptionModel(string nextShot)
    {
        this.next_shot = nextShot;
    }
   
    public OptionModel(string text, string nextShot)
    {
        this.text = text;
        this.next_shot = nextShot; 
    }
    
    public OptionModel(bool endDialogue, string endVar)
    {
        this.end_dialogue = endDialogue;
        this.end_var = endVar;
    }
    
    public OptionModel(string text, bool endDialogue, string endVar)
    {
        this.text = text;
        this.end_dialogue = endDialogue;
        this.end_var = endVar;
    }

    public override string ToString()
    {
        if (end_dialogue)
        {
            if (this.text != null)
            {
                return $"{nameof(text)}: {text}, {nameof(end_dialogue)}: {end_dialogue}, {nameof(end_var)}: {end_var}";
            }
            else
            {
                return $"{nameof(end_dialogue)}: {end_dialogue}, {nameof(end_var)}: {end_var}";
            }
        }
        else
        {
            if (this.text != null)
            {
                return $"{nameof(text)}: {text}, {nameof(next_shot)}: {next_shot}";
            }
            else
            {
                return $"{nameof(next_shot)}: {next_shot}";
            }
        }
    }
}