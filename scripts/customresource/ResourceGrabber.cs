using Godot;

namespace TeicsoftSpectacleCards.scripts.customresource;

public static class ResourceGrabber
{
    // This Class will attempt to grab the resource path from the _closed_source_assets folder first. 
    // If this folder is not present, it will fall back to open source assets. 


    public const string ClosedSourceAssetsPath = "res://assets/_closed_source_assets/";
    public const string OpenSourceAssetsPath = "res://assets/";
    
    
    // Check if folder exists
    public static bool ClosedSourceFolderExist()
    {
        bool dirExists = DirAccess.DirExistsAbsolute(ClosedSourceAssetsPath);
        return dirExists;
    }

    
    public static bool ClosedSourceAssetExist(string assetName, string folderName)
    {
        string path = ClosedSourceAssetsPath + folderName + "/" + assetName;
        bool fileExists = FileAccess.FileExists(path);
        return fileExists;
    }
    
    
    // Get the path to the resource
    public static string GetAssetPath(string assetName, string folderName)
    {
        string closedPath;
        string openPath;
        
        if (folderName == "")
        {
            closedPath = ClosedSourceAssetsPath + assetName;
            openPath = OpenSourceAssetsPath + assetName;
        }
        else
        {
            closedPath = ClosedSourceAssetsPath + folderName + "/" + assetName;
            openPath = OpenSourceAssetsPath + folderName + "/" + assetName;
        }
        
        
        if (ClosedSourceFolderExist())
        {
            if (ClosedSourceAssetExist(assetName, folderName))
            {
                return closedPath; 
            }
            else
            {
                return openPath;
            }
        } else { 
            return openPath;
        }
    }
}