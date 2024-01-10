using System;
using System.Xml;
using Godot;

namespace TeicsoftSpectacleCards.scripts.dialogue;

public static class DialogueXMLParser
{
    public static CharacterModel[] ParseCharactersFromXML(String filePath)
    	{
    		using var file = FileAccess.Open(filePath, FileAccess.ModeFlags.Read);
    		string content = file.GetAsText();
    		
    		XmlDocument doc = new XmlDocument();
    		doc.LoadXml(content);
    		
    		XmlNodeList characterNodes = doc.SelectNodes("/characters/character");
    		CharacterModel[] characterList = new CharacterModel[characterNodes.Count];
    		
    		int i = 0;
    		
    		foreach (XmlNode characterNode in characterNodes)
    		{
    			CharacterModel character = new CharacterModel(
    				characterNode.Attributes["id"].Value,
    				characterNode.SelectSingleNode("name").InnerText,
    				characterNode.SelectSingleNode("font").InnerText,
    				characterNode.SelectSingleNode("color").InnerText,
    				characterNode.SelectSingleNode("sprite").InnerText,
    				characterNode.SelectSingleNode("default_animation").InnerText);
    
    			characterList[i] = character;
    			GD.Print(characterList[i].ToString());
    		}
    
    		return characterList;
    	}
    	
	    public static LocationModel[] ParseLocationsFromXML(String filePath)
    	{
    		GD.Print("test");
    		using var file = FileAccess.Open(filePath, FileAccess.ModeFlags.Read);
    		string content = file.GetAsText();
    		
    		XmlDocument doc = new XmlDocument();
    		doc.LoadXml(content);
    		
    		XmlNodeList locationNodes = doc.SelectNodes("/scene_locations/location");
    		LocationModel[] locationList = new LocationModel[locationNodes.Count];
    		
    		int i = 0;
    		
    		foreach (XmlNode locationNode in locationNodes)
    		{
    			LocationModel location = new LocationModel(
    				locationNode.Attributes["id"].Value,
    				locationNode.SelectSingleNode("name").InnerText,
    				locationNode.SelectSingleNode("image").InnerText);
    
    			locationList[i] = location;
    			GD.Print(locationList[i].ToString());
    		}
    
    		return locationList;
    	}

}