using System;
using System.CodeDom;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Timers;
using UnityEngine;
using UnityEngine.UI;
using AOT;

/// <summary>
/// This class handles all text to speech processes.
/// </summary>
public class TextManager : MonoBehaviour {

	private string _voice = "Dutch Female";

	// whether the agent is currently paused, initialised to false.
	private bool _isSpeaking = false;
	private bool _isPaused = false;
	// current text string to be spoken.
	private string _textInput = null;
	// most recent boundary character encountered while speaking text.
	private static int _lastWordIndex = 0;

	// delegate declarations for javascript text to speech callback functions
	// TODO not sure if needed, probably for dynamic linking
	public delegate void StartDelegate(float elapsedTime);
	
	/// <summary>
	/// The callback when the speech ends.
	/// </summary>
	public delegate void EndDelegate(float elapsedTime);
	public delegate void BoundaryDelegate(int lastword, float elapsedTime);

	Button btn;

	// These are javascript functions in WebGLTemplates/.../TemplateData/textToSpeech.js
	// They can be dynamically linked to this c# code through Plugins/WebGL/MyPlugin.jslib
	#if UNITY_WEBGL
	/// <summary>
	/// Start speaking a given text in a given voice, executing callbacks at the start and at the end of the speech.
	/// </summary>
	/// <param name="text">The text to pronounce.</param>
	/// <param name="voice">The voice to pronounce in.</param>
	/// <param name="startCallback">The function to call when speech starts.</param>
	/// <param name="endCallback">The function to call when speech ends.</param>
	/// <returns>The state of the TTS.</returns>
	[DllImport("__Internal")]
	private static extern string Speak(
		string text, string voice,
		StartDelegate startCallback, EndDelegate endCallback, BoundaryDelegate boundaryCallback
		);

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

	public void setVoice(string newVoice){
		_voice = newVoice;
	}


	public void startSpeech(string text) {
		_textInput = text;
		_isSpeaking = true;
		// start speech, animation started with callback functions
		Speak(text, this._voice, callbackStart, callbackEnd, callbackBoundary);
	}

	public void startDemo() {
		Debug.Log("startDemo()");
		_textInput = "The quick brown fox jumps over the lazy dog.";
		_isSpeaking = true;
		// start speech, animation started with callback functions
		Speak(_textInput, _voice, callbackDemoStart, callbackEnd, callbackBoundary);
	}

	public void stopSpeech() {
		_textInput = null;
		_isSpeaking = false;
		//stop speech
		Stop();
		//stop animation
		ApplicationManager.instance.StopAnimation();
	}

	/// <summary>
	/// Pauses speech synthesis and animation from a speaking state.
	/// </summary>
	public void pauseSpeech() {
		// if not speaking do nothing
		if (!_isSpeaking) return;
		
		_isPaused = true;
		_isSpeaking = false;
		
		// store remainder of text for pause.
		_textInput = _textInput.Substring(_lastWordIndex);
		_lastWordIndex = 0;
		
		// stop speaking
		Stop();
		// stop animation
		ApplicationManager.instance.StopAnimation();
		
		Debug.Log("Paused Speech!");
	}

	/// <summary>
	/// Resumes speech synthesis and animation from a paused state.
	/// </summary>
	public void resumeSpeech() {
		// if not paused do nothing
		if (!_isPaused) return;
		
		_isPaused = false;
		_isSpeaking = true;
		
		// resume speaking with remainder of text after pause.
		Speak(_textInput, _voice, callbackStart, callbackEnd, callbackBoundary);
		Debug.Log("Resumed Speech!");
	}

	[MonoPInvokeCallback(typeof(StartDelegate))]
	public static void callbackStart(float elapsedTime){
		Debug.Log("callback start at : " + elapsedTime);
		
		ApplicationManager.instance.PlayAnimation();
	}
	
	[MonoPInvokeCallback(typeof(StartDelegate))]
	public static void callbackDemoStart(float elapsedTime){
		Debug.Log("callback Demo start at : " + elapsedTime);
		
		ApplicationManager.instance.animateFox();
	}
	
	/// <summary>
	/// The end of the callback when talking ends.
	/// </summary>
	[MonoPInvokeCallback(typeof(EndDelegate))]
	public static void callbackEnd(float elapsedTime){
		Debug.Log("callback end at : " + elapsedTime);
		//ApplicationManager.instance.StopAnimation();
	}

	/// <summary>
	/// Update the index of the most recently encountered word while speaking.
	/// Index is the place of the word's first character in the text.
	/// 
	/// This is a callback function for the javascript Web Speech API. It is
	/// attached to the onboundary event, fired at the start of each word.
	/// </summary>
	/// <param name="lastWord">
	/// Index of the most recently encountered word while speaking.
	/// </param>
	[MonoPInvokeCallback(typeof(BoundaryDelegate))]
	public static void callbackBoundary(int lastWord, float elapsedTime) {
		_lastWordIndex = lastWord;
		Debug.Log("Last Boundary Char = " + lastWord + " at time : " + elapsedTime);
	}

    /// <summary>
	/// Returns true if speaking and false if not. This function is used for testing.
	/// </summary>
    public bool getIsSpeaking()
    {
        return _isSpeaking;
    }

    /// <summary>
	/// Returns true if speaking and false if not. This function is used for testing.
	/// </summary>
    public bool getIsPaused()
    {
        return _isPaused;
    }
}