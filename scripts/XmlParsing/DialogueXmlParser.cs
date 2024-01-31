using System;
using System.Xml;
using Godot;

namespace TeicsoftSpectacleCards.scripts.dialogue;

public static class DialogueXmlParser
{
    public static CharacterModel[] ParseCharactersFromXml(string filePath)
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
            string charId = characterNode.Attributes["id"].Value;
            string charName = characterNode.SelectSingleNode("name").InnerText;
            string fontRef = characterNode.SelectSingleNode("font").InnerText;
            string colorHex = characterNode.SelectSingleNode("color").InnerText;
            string spriteRef = characterNode.SelectSingleNode("sprite").InnerText;
            string defaultAnimation = characterNode.SelectSingleNode("default_animation").InnerText;


            XmlNodeList animationNodes = characterNode.SelectNodes("animations/animation");
            string[] animationList = new string[animationNodes.Count];

            int j = 0;
            foreach (XmlNode animationNode in animationNodes)
            {
                String animationId = animationNode.Attributes["id"].Value;
                animationList[j] = animationId;
                j += 1;
            }

            CharacterModel character = new CharacterModel(
                charId,
                charName,
                fontRef,
                colorHex,
                spriteRef,
                defaultAnimation,
                animationList
            );

            characterList[i] = character;
            i += 1;
        }

        return characterList;
    }

    public static LocationModel[] ParseLocationsFromXml(String filePath)
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
            string locationId = locationNode.Attributes["id"].Value;
            string locationName = locationNode.SelectSingleNode("name").InnerText;
            string locationImage = locationNode.SelectSingleNode("image").InnerText;

            XmlNodeList animationNodes = locationNode.SelectNodes("animations/animation");
            string[] animationList = new string[animationNodes.Count];

            int j = 0;
            foreach (XmlNode animationNode in animationNodes)
            {
                String animationId = animationNode.Attributes["id"].Value;
                animationList[j] = animationId;
                j += 1;
            }


            LocationModel location = new LocationModel(
                locationId,
                locationName,
                locationImage,
                animationList
            );

            locationList[i] = location;
            i += 1;
            // GD.Print(locationList[i].ToString());
        }

        return locationList;
    }


    public static ShotModel[] ParseShotsFromXml(String filePath)
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
                String overridePosition;
                LineModel line;

                try
                {
                    overridePosition = lineNode.SelectSingleNode("override_pos").InnerText;
                    // GD.Print("override_pos: " );
                }
                catch (Exception e)
                {
                    overridePosition = null;
                }


                if (overridePosition != null)
                {
                    line = new LineModel(
                        lineNode.SelectSingleNode("text").InnerText,
                        lineNode.Attributes["character_id"].Value,
                        lineNode.Attributes["animation_id"].Value,
                        overridePosition);
                    // GD.Print(line.ToString());
                }
                else
                {
                    line = new LineModel(
                        lineNode.SelectSingleNode("text").InnerText,
                        lineNode.Attributes["character_id"].Value,
                        lineNode.Attributes["animation_id"].Value
                    );
                    // GD.Print(line.text);
                    // GD.Print(e);
                }

                lineList[j] = line;

                j += 1;
            }

            // get list of OptionModel objects
            int k = 0;
            foreach (XmlNode optionNode in optionNodes)
            {
                XmlNode endDialogue;
                string endVar;
                string text;
                string nextShot;
                OptionModel option;

                try
                {
                    endDialogue = (optionNode.SelectSingleNode("dialogue_end"));
                    endVar = endDialogue.Attributes["end_var"].Value;
                    // GD.Print("end_var: " + end_var);
                }
                catch (NullReferenceException e)
                {
                    endDialogue = null;
                    endVar = null;
                }

                try
                {
                    text = optionNode.SelectSingleNode("text").InnerText;
                }
                catch (Exception e)
                {
                    text = null;
                }

                if (endDialogue == null)
                {
                    try
                    {
                        nextShot = optionNode.SelectSingleNode("next_shot_id").InnerText;
                        // GD.Print("next_shot: " + next_shot);
                    }
                    catch (NullReferenceException e)
                    {
                        nextShot = null;
                        GD.Print("\nOption has no next_shot attribute" + e);
                    }

                    if (text != null)
                    {
                        option = new OptionModel(text, nextShot);
                    }
                    else
                    {
                        option = new OptionModel(nextShot);
                    }
                }
                else
                {
                    if (text != null)
                    {
                        option = new OptionModel(text, true, endVar);
                    }
                    else
                    {
                        option = new OptionModel(true, endVar);
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

        // GD.Print(shotList);
        return shotList;
    }
}
