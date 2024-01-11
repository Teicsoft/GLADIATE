using System;
using System.Linq;
using System.Xml;
using Godot;

namespace TeicsoftSpectacleCards.scripts.dialogue;

public static class DialogueXMLParser
{
	public static CharacterModel[] ParseCharactersFromXML(String filePath)
	{
		GD.Print("\nParseCharactersFromXML: " + filePath + "\n");
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
			// GD.Print(characterList[i].ToString());
		}

		return characterList;
	}
		
	public static LocationModel[] ParseLocationsFromXML(String filePath)
	{
		GD.Print("\nParseLocationsFromXML: " + filePath + "\n");
		
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
			// GD.Print(locationList[i].ToString());
		}

		return locationList;
	}
	
	
	public static ShotModel[] ParseShotsFromXML(String filePath)
	{
		GD.Print("\nParseShotsFromXML: " + filePath + "\n");
		
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
			
			
			// get list of LineModel objects
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

			// get list of OptionModel objects
			int k = 0;
			foreach (XmlNode optionNode in optionNodes)
			{

				XmlNode end_dialogue;
				string end_var;
				string text;
				string next_shot;
				OptionModel option;
				
				try
				{
					end_dialogue = (optionNode.SelectSingleNode("dialogue_end"));
					end_var = end_dialogue.Attributes["end_var"].Value;
					// GD.Print("end_var: " + end_var);
				}
				catch (NullReferenceException e)
				{
					end_dialogue = null;
					end_var = null;
				}
				
				try
				{
					text = optionNode.SelectSingleNode("text").InnerText;
				}
				catch (Exception e)
				{
					text = null;
				}
				
				if (end_dialogue == null)
				{
					try
					{
						next_shot = optionNode.SelectSingleNode("next_shot_id").InnerText;
						// GD.Print("next_shot: " + next_shot);
					}
					catch (NullReferenceException e)
					{
						next_shot = null;
						GD.Print("\nOption has no next_shot attribute" + e);
					}
					
					if (text != null)
					{
						option = new OptionModel(text, next_shot);
					}
					else
					{
						option = new OptionModel(next_shot);
					}
				}
				else
				{
					if (text != null)
					{
						option = new OptionModel(text, true, end_var);
					}
					else
					{
						option = new OptionModel(true, end_var);
					}
				}
				optionList[k] = option;
				k += 1;
			}
			// GD.Print(optionList);

			
			//make shot object and add to shotList
			
			ShotModel shot = new ShotModel(
				shotNode.Attributes["id"].Value,
				shotNode.Attributes["location_id"].Value,
				lineList,
				optionList);
			
			// GD.Print(shot.ToString());
			
			shotList[i] = shot;
			i += 1;
		}
		GD.Print(shotList);
		return shotList;
	}
}
