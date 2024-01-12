using System;
using Godot;

namespace TeicsoftSpectacleCards.scripts.dialogue;

public partial class DialogueDisplay : Control
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		String charactersXmlPath = "res://assets/writing/dialogue/characters.xml";
		String locationsXmlPath = "res://assets/writing/dialogue/locations.xml";
		String dialogueXmlPath = "res://assets/writing/dialogue/dialogue_example.xml";
		
		CharacterModel[] characterList = DialogueXmlParser.ParseCharactersFromXml(charactersXmlPath);
		LocationModel[] locList = DialogueXmlParser.ParseLocationsFromXml(locationsXmlPath);
		ShotModel[] shotList = DialogueXmlParser.ParseShotsFromXml(dialogueXmlPath);
		
		GD.Print(shotList[0].Lines);

		RunDialogue(shotList);
		
		// RichTextLabel RTlabel = GetNode<RichTextLabel>("TextureRect/MarginContainer/ScrollContainer/VBoxContainer/GridContainer/RichTextLabel");
		// if (RTlabel != null)
		// {
		// 	RTlabel.Text = shotList[0].Lines[0].Text;
		// 	GD.Print(RTlabel.Text);
		// }
		// else
		// {
		// 	GD.Print("No label found!");
		// }
		//
		// Label label = GetNode<Label>("TextureRect/MarginContainer/ScrollContainer/VBoxContainer/GridContainer/Label");
		// if (label != null)
		// {
		// 	label.Text = shotList[0].Lines[0].CharacterId;
		// 	GD.Print(label.Text);
		// }
		// else
		// {
		// 	GD.Print("No label found!");
		// }

	}


	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void RunDialogue(ShotModel[] shotList)
	{
		VBoxContainer dialogueTarget = GetNode<VBoxContainer>("TextureRect/MarginContainer/ScrollContainer/DialogueTarget_VBoxContainer");
		var scene = GD.Load<PackedScene>("res://scenes/sub/dialogue_item.tscn");


		int i = 0;
		foreach (ShotModel shot in shotList)
		{
			var instance = scene.Instantiate();

			int j = 0;
			foreach (LineModel line in shot.Lines)
			{
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
