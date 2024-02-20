using System;
using Godot;

namespace GLADIATE.scripts.dialogue;

public partial class DialogueDisplay : Control
{
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        RunDialogue("dialogue_example.xml");
    }


    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
    }

    public void RunDialogue(String dialogueName)
    {
        String charactersXmlPath = "res://assets/writing/characters.xml";
        String locationsXmlPath = "res://assets/writing/locations.xml";
        String dialogueXmlPath = "res://assets/writing/dialogue/dialogue_example.xml";

        CharacterModel[] characterList = DialogueXmlParser.ParseCharactersFromXml(charactersXmlPath);
        LocationModel[] locList = DialogueXmlParser.ParseLocationsFromXml(locationsXmlPath);
        ShotModel[] shotList = DialogueXmlParser.ParseShotsFromXml(dialogueXmlPath);

        GD.Print(shotList[0].Lines);

        VBoxContainer dialogueTarget =
            GetNode<VBoxContainer>("TextureRect/MarginContainer/ScrollContainer/DialogueTarget_VBoxContainer");
        var scene = GD.Load<PackedScene>("res://scenes/sub/dialogue_item.tscn");


        int i = 0;
        foreach (ShotModel shot in shotList)
        {
            int j = 0;
            foreach (LineModel line in shot.Lines)
            {
                var instance = scene.Instantiate();

                Label label = instance.GetNode<Label>("Label");
                label.Text = shot.Lines[j].CharacterId;

                RichTextLabel richTextLabel = instance.GetNode<RichTextLabel>("RichTextLabel");
                richTextLabel.Text = shot.Lines[j].Text;

                dialogueTarget.AddChild(instance);

                j++;
            }

            i++;
        }
    }
}
