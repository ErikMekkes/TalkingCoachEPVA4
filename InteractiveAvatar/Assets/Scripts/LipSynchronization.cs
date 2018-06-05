using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using SimpleJSON;

/// <summary>
/// This class combines all of the needed functionality to provide lip synchronization for the Virtual Agent.
/// </summary>
public class LipSynchronization
{
    
    /// <summary>
    /// The singleton instance of the class.
    /// </summary>
    private static LipSynchronization instance;

    private const string api = "localhost:3001/api/v1/";
    
    /// <summary>
    /// Private constructor to prevent initialization.
    /// </summary>
    private LipSynchronization() {}
    
    /// <summary>
    /// The initiation of the singleton: either returns the instance of it already exists and creates an instantiates
    /// an instance otherwise.
    /// </summary>
    public static LipSynchronization getInstance
    {
        get
        {
            if (instance == null)
            {
                instance = new LipSynchronization();
            }
            return instance;
        }
    }

    /// <summary>
    /// Synchronizes the lip with the speech.
    /// </summary>
    /// <param name="text">The text to synchronize.</param>
    /// <param name="lang">The language to synchronize in.</param>
    public void synchronize(string text, string lang)
    {
        retrievePhonemes(text, lang);
    }
    
    /// <summary>
    /// Retrieve phonemes from the server.
    /// </summary>
    /// <param name="text">The text to parse to phonemes.</param>
    /// <param name="lang">The language to take the phonemes from.</param>
    /// <returns>An IEnumerator being the send action of the UnityWebRequest.</returns>
    private IEnumerator retrievePhonemes(string text, string lang) {
        var www = UnityWebRequest.Get(api + "phoneme?text=" + text + "&lang=" + lang);
        yield return www.Send();
 
        if (www.isError) {
            Debug.Log(www.error);
        } else {
            var response = JSON.Parse(www.downloadHandler.text);
            var phonemes = response["phonemes"].AsArray;
            var phonemeList = JSONUtil.arrayToList(phonemes);
            var visemeDurationList =
                AnimationsManager.amInstance.getVisemeTimingCalculator().getVisemeDurations(phonemeList);
            SpeechAnimationManager.instance.playVisemeList(getVisemeIndices(phonemeList));
        }
    }

    private List<int> getVisemeIndices(List<string> visemes)
    {
        var indices = new List<int>();
        var visemeMap = AnimationsManager.amInstance.getVisemeDictictionary();

        for (var i = 0; i < visemes.Count; i++)
        {
            var currentViseme = visemes[i];
            indices.Add(visemeMap[currentViseme]);
        }

        return indices;
    }
}