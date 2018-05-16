using System.Diagnostics;
using UnityEditor;

public class ScriptBatch
{
    [MenuItem("MyTools/Windows Build With Postprocess")]
    public static void BuildGame()
    {
        UnityEngine.Debug.Log("Building haha");
        System.Diagnostics.Debug.WriteLine("Building");
        // Get filename.
        string path = EditorUtility.SaveFolderPanel("Choose Location of Built Site", "", "");
        UnityEngine.Debug.Log(path);
//        string[] scenes = new string[] {"Assets/_Scenes/InteractiveAvatar.unity"};

        // Build player.
//        BuildPipeline.BuildPlayer(scenes, path, BuildTarget.WebGL, BuildOptions.None);
    }
}