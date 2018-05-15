using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.UI;
using AOT;

/// <summary>
/// This class handles all text to speech processes.
/// </summary>
public class TextManager : MonoBehaviour {

	/// <summary>
	/// The default voice.
	/// </summary>
	private string voice = "Dutch Female";

	/// <summary>
	/// The callback when starting the speech.
	/// </summary>
	public delegate void StartDelegate();
	
	/// <summary>
	/// The callback when the speech ends.
	/// </summary>
	public delegate void EndDelegate();

	Button btn;

	#if UNITY_WEBGL
	/// <summary>
	/// Start speaking a given text in a given voice, executing callbacks at the start and at the end of the speech.
	/// </summary>
	/// <param name="text">The text to pronounce.</param>
	/// <param name="voice">The boice to pronounce in.</param>
	/// <param name="startCallback">The function to call when speech starts.</param>
	/// <param name="endCallback">The function to call when speech ends.</param>
	/// <returns>The state of the TTS.</returns>
	[DllImport("__Internal")]
	private static extern string Speak(string text, string voice, StartDelegate startCallback, EndDelegate endCallback );

	/// <summary>
	/// Stops the speech.
	/// </summary>
	/// <returns>The state of the TTS.</returns>
	[DllImport("__Internal")]
	private static extern string Stop();

	/// <summary>
	/// Returns system voices.
	/// </summary>
	/// <returns>Gets all the system voices.</returns>
	[DllImport("__Internal")]
	private static extern string getSystemVoices();
	#endif

	/// <summary>
	/// The Singleton instance of the class.
	/// </summary>
	private static TextManager _instance;

	/// <summary>
	/// The initiation of the singleton: either returns the instance of it already exists and creates an instantiates
	/// an instance otherwise.
	/// </summary>
	public static TextManager instance
	{
		get
		{
			if (_instance == null)
			{
				_instance = GameObject.FindObjectOfType<TextManager>();
				DontDestroyOnLoad(_instance.gameObject);
			}
			return _instance;
		}
	}
		
//	public void SpeakTTS_Click(){
//
//		this.getVoices();
//
//		this.btn = gameObjectSpeak.GetComponent<Button>();
//		String speakButtonText = btn.GetComponentInChildren<Text>().text;
//
//		if(speakButtonText == "Stop"){
//			this.btn.GetComponentInChildren<Text>().text = "Speak";
//			Stop();
//			ApplicationManager.instance.StopAnimation();
//		}else{
//			string textMessage = speakText.text;
//			if(textMessage != ""){
//
//				if( Application.platform == RuntimePlatform.WebGLPlayer){
//					Speak(textMessage, "Dutch Female", callbackStart, callbackEnd);
//					this.btn.GetComponentInChildren<Text>().text = "Stop";
//				}
//			}	
//		}
//	}

	/// <summary>
	/// Get the available system voices.
	/// </summary>
	public void getVoices(){
		Debug.Log("Get Voices");
		Debug.Log(getSystemVoices());
	}

	/// <summary>
	/// Set the current voice.
	/// </summary>
	/// <param name="voice">The new Voice to use.</param>
	public void setVoice(string voice){
		this.voice = voice;
	}

	/// <summary>
	/// Start the speech.
	/// </summary>
	/// <param name="text">The text to pronounce.</param>
	public void startSpeach(string text){
		if( Application.platform == RuntimePlatform.WebGLPlayer){
			Speak(text, this.voice, callbackStart, callbackEnd);
		}	
	}

	/// <summary>
	/// Stop the speech.
	/// </summary>
	public void stopSpeach(){
		if( Application.platform == RuntimePlatform.WebGLPlayer){
			Stop();
		}
		ApplicationManager.instance.StopAnimation();
	}

	/// <summary>
	/// The start of the callback when talking starts.
	/// </summary>
	[MonoPInvokeCallback(typeof(StartDelegate))]
	public static void callbackStart(){
		Debug.Log("callback start");
		ApplicationManager.instance.PlayAnimation();
	}
		
	/// <summary>
	/// The end of the callback when talking ends.
	/// </summary>
	[MonoPInvokeCallback(typeof(EndDelegate))]
	public static void callbackEnd(){
		Debug.Log("callback ended");
		ApplicationManager.instance.StopAnimation();
	}
}