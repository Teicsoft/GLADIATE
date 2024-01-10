using System;
using System.Linq;
using System.Xml;
using Godot;

namespace TeicsoftSpectacleCards.scripts.dialogue;

public static class DialogueXMLParser
{
	public static CharacterModel[] ParseCharactersFromXML(String filePath)
	{
		GD.Print("\nParseCharactersFromXML: " + filePath);
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
		GD.Print("\nParseLocationsFromXML: " + filePath);
		
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
	
	
	public static ShotModel[] ParseShotsFromXML(String filePath)
	{
		GD.Print("\nParseShotsFromXML: " + filePath);
		
		using var file = FileAccess.Open(filePath, FileAccess.ModeFlags.Read);
		string content = file.GetAsText();
		
		XmlDocument doc = new XmlDocument();
		doc.LoadXml(content);

		XmlNodeList shotNodes = doc.SelectNodes("dialogue/shots/shot");
		ShotModel[] shotList = new ShotModel[shotNodes.Count];
		
		int i = 0;
		
		foreach (XmlNode shotNode in shotNodes)
		{
			// GD.Print(shotNode.Attributes["id"].Value);
			// GD.Print(shotNode.Attributes["location_id"].Value);
			
			XmlNodeList lineNodes = shotNode.SelectNodes("lines/line");
			XmlNodeList optionNodes = shotNode.SelectNodes("options/option");
			
			LineModel[] lineList = new LineModel[lineNodes.Count];
			OptionModel[] optionList = new OptionModel[optionNodes.Count];
			
			int j = 0;
			foreach (XmlNode lineNode in lineNodes)
			{
				try {
					string overridePosition = lineNode.SelectSingleNode("override_pos").InnerText;
					LineModel line = new LineModel(
						lineNode.SelectSingleNode("text").InnerText,
						lineNode.Attributes["character_id"].Value,
						lineNode.Attributes["animation_id"].Value,
						overridePosition);
					lineList.Append(line);
					j+=1;
					// GD.Print(line.ToString());
				} catch (NullReferenceException e) {
					LineModel line = new LineModel(
						lineNode.SelectSingleNode("text").InnerText,
						lineNode.Attributes["character_id"].Value,
						lineNode.Attributes["animation_id"].Value
						);
					lineList[j] = line;
					j+=1;
					// GD.Print(line.text);
					// GD.Print(e);
				}
			}


			int k = 0;
			foreach (XmlNode optionNode in optionNodes)
			{
				try
				{
					XmlNode end_dialogue = (optionNode.SelectSingleNode("end_dialogue"));
					string end_var = optionNode.SelectSingleNode("end_var").InnerText;
					try
					{
					}
					catch (NullReferenceException e)
					{
						GD.Print(e);
					}
				}
				catch (NullReferenceException e)
				{
					GD.Print("test2");
					OptionModel option = new OptionModel(
						optionNode.SelectSingleNode("text").InnerText,
						optionNode.Attributes["next_shot"].Value
					);

					optionList[k] = option;
					k += 1;
					GD.Print(option.text);
					GD.Print(e);
				}
			}
		}
		return shotList;
	}
}
