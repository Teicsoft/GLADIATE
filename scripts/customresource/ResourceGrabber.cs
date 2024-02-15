using Godot;

namespace GLADIATE.scripts.customresource;

public static class ResourceGrabber
{
    // This Class will attempt to grab the resource path from the _closed_source_assets folder first. 
    // If this folder is not present, it will fall back to open source assets. 

    public const string ClosedSourceAssetsFolder = "res://assets/_closed_source_assets/GLADIATE-ClosedAssets/";
    public const string OpenSourceAssetsFolder = "res://assets/";


    // Check if folder exists
    public static bool ClosedSourceFolderExist()
    {
        bool dirExists = DirAccess.DirExistsAbsolute(ClosedSourceAssetsFolder);
        return dirExists;
    }


    public static bool ClosedSourceAssetExist(string assetName, string folderName)
    {
        string path = ClosedSourceAssetsFolder + folderName + "/" + assetName;
        bool fileExists = FileAccess.FileExists(path);
        return fileExists;
    }

    public static bool ClosedSourceAssetExist(string path)
    {
        string assetPath = ClosedSourceAssetsFolder + path;
        bool fileExists = FileAccess.FileExists(assetPath);
        return fileExists;
    }


    // Get the path to the resource
    public static string GetAssetPath(string assetName, string folderName)
    {
        string closedPath;
        string openPath;

        if (folderName == "")
        {
            closedPath = ClosedSourceAssetsFolder + assetName;
            openPath = OpenSourceAssetsFolder + assetName;
        }
        else
        {
            closedPath = ClosedSourceAssetsFolder + folderName + "/" + assetName;
            openPath = OpenSourceAssetsFolder + folderName + "/" + assetName;
        }

        return ClosedSourceAssetExist(assetName, folderName) ? closedPath : openPath;
    }


    public static string GetAssetPath(string path)
    {
        var closedPath = ClosedSourceAssetsFolder + path;
        var openPath = OpenSourceAssetsFolder + path;


        if (ClosedSourceFolderExist())
        {
            if (ClosedSourceAssetExist(path))
            {
                return closedPath;
            }
            else
            {
                return openPath;
            }
        }
        else
        {
            return openPath;
        }
    }
}