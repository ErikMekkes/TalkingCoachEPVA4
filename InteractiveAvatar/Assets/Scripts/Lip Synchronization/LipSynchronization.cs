﻿using System.Collections;
using System.Collections.Generic;
using Models;
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

    /// <summary>
    /// The URL of the API to connect to.
    /// </summary>
    private const string api = "http://localhost:3001/api/v1/";
    
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
    /// <returns>An IEnumerator being the send action of the UnityWebRequest.</returns>
    public IEnumerator synchronize(string text, string lang) {
        Debug.Log("Trying to make request...");
        using ( var www = UnityWebRequest.Get(api + "phoneme?text=" + text + "&lang=" + lang))
        {
            Debug.Log("request made");
            yield return www.Send();
            Debug.Log("request sent to " + www.url);
            
            if (www.isError) {
                Debug.Log(www.responseCode + ": " + www.error);
            } else {
                Debug.Log("response successfully received");
                var response = JSON.Parse(www.downloadHandler.text);
                var phonemes = response["phonemes"].AsArray;
                var phonemeList = JSONUtil.arrayToList(phonemes);
                var actualPhonemeList = Phoneme.getPhonemeFromCode(phonemeList);
                var actualVisemeList = Phoneme.toVisemes(actualPhonemeList);
                Debug.Log("playing list...");
                TextManager.tmInstance.startActualSpeech(text);
                SpeechAnimationManager.instance.playVisemeList(actualVisemeList);
            }
        }
    }
}