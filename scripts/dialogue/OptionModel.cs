using System;

namespace TeicsoftSpectacleCards.scripts.dialogue;

public class OptionModel
{
    public string text { get; set;}
    public string next_shot { get; set;}
    public bool end_dialogue { get; set;}
    public String end_var { get; set;}
    
    public OptionModel(string text, string nextShot)
    {
        this.text = text;
        this.next_shot = nextShot; 
    }
    
    public OptionModel(string text, bool endDialogue, string endVar)
    {
        this.text = text;
        this.end_dialogue = endDialogue;
        this.end_var = endVar;
    }
}