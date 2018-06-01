using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using SimpleJson;
using SimpleJSON;

public class LipSynchronization
{
    private static LipSynchronization instance;

    private const string api = "localhost:3001/api/v1/";
    
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

    public void synchronize(string text, string lang)
    {
        // Text to phonemes and possible phonemes to visemes call
        
    }
    
    private IEnumerator retrievePhonemes(string text, string lang) {
        var www = UnityWebRequest.Get(api + "phoneme?text=" + text + "&lang=" + lang);
        yield return www.Send();
 
        if (www.isError) {
            Debug.Log(www.error);
        } else {
            var response = JSON.Parse(www.downloadHandler.text);
            var phonemes = response["phonemes"];
            
        }
    }
}