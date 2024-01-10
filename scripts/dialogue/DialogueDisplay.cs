using Godot;
using System;
using TeicsoftSpectacleCards.scripts.dialogue;

public partial class DialogueDisplay : PanelContainer
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		String charactersXMLPath = "res://assets/writing/dialogue/characters.xml";
		String locationsXMLPath = "res://assets/writing/dialogue/locations.xml";
		String dialogueXMLPath = "res://assets/writing/dialogue/dialogue_example.xml";
		
		CharacterModel[] characterList = DialogueXMLParser.ParseCharactersFromXML(charactersXMLPath);
		LocationModel[] locList = DialogueXMLParser.ParseLocationsFromXML(locationsXMLPath);
		ShotModel[] shotlist = DialogueXMLParser.ParseShotsFromXML(dialogueXMLPath);
	}


	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
	
	
	
}