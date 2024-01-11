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


		RichTextLabel label = GetNode<RichTextLabel>("TextVBoxContainer/TextureRect/TextLabel");
		if (label != null)
		{
			label.Text = shotList[0].Lines[0].Text;
			GD.Print(label.Text);
		}
		else
		{
			GD.Print("No label found!");
		}
	}


	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
	
	
	
}
