using System;
using System.Xml;

namespace TeicsoftSpectacleCards.scripts.XmlParsing;

public static class Utils
{
    public static int ParseIntNode(XmlNode parent, string s)
    {
        return parent.SelectSingleNode(s) != null ? int.Parse(parent.SelectSingleNode(s).InnerText) : 0;
    }

    public static string ParseTextNode(XmlNode parent, string s)
    {
        return parent.SelectSingleNode(s) != null ? parent.SelectSingleNode(s).InnerText : "";
    }

    public static bool ParseBoolNode(XmlNode parent, string s)
    {
        return parent.SelectSingleNode(s) != null ? parent.SelectSingleNode(s).InnerText=="true" : false;
    }
}