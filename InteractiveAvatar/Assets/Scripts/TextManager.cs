using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.UI;
using AOT;

public class TextManager : MonoBehaviour {

	private string voice = "Dutch Female";

	// whether the agent is currently paused, initialised to false.
	private bool _isSpeaking = false;
	private bool _isPaused = false;
	// current text string to be spoken.
	private string _textInput = null;
	// most recent boundary character encountered while speaking text.
	private static int _lastWordIndex = 0;

	//delegate declarations for callback functions
	public delegate void StartDelegate();
	public delegate void EndDelegate();

	Button btn;

	#if UNITY_WEBGL
	[DllImport("__Internal")]
	private static extern string Speak(string text, string voice, StartDelegate startCallback, EndDelegate endCallback );

	[DllImport("__Internal")]
	private static extern string Stop();

	[DllImport("__Internal")]
	private static extern string getSystemVoices();
	#endif

	private static TextManager _instance;

	//Singleton Initiation
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

	public void getVoices(){
		Debug.Log("Get Voices");
		Debug.Log(getSystemVoices());
	}

	public void setVoice(string voice){
		this.voice = voice;
	}

	public void startSpeach(string text) {
		_textInput = text;
		_isSpeaking = true;
		//TODO add onboundary callback function here instead of using TalkingCoachAPI
		if( Application.platform == RuntimePlatform.WebGLPlayer){
			Speak(text, this.voice, callbackStart, callbackEnd);
		}	
	}

	public void stopSpeach() {
		_textInput = null;
		_isSpeaking = false;
		if( Application.platform == RuntimePlatform.WebGLPlayer){
			Stop();
		}
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
		Speak(_textInput, voice, callbackStart, callbackEnd);
		Debug.Log("Resumed Speech!");
	}

	/// <summary>
	/// Updates the index of the most recently encountered word while speaking.
	/// Index is the place of the word's first character in the text.
	/// </summary>
	/// <param name="lastWord">Index of the most recently encountered word while speaking.</param>
	public void lastWordIndex(int lastWord) {
		//TODO check if there is a timing risk here
		// don't update index while paused
		//if (_isPaused) return;
		
		_lastWordIndex = lastWord;
		Debug.Log("Last Boundary Char: " + lastWord);
	}

	[MonoPInvokeCallback(typeof(StartDelegate))]
	public static void callbackStart(){
		Debug.Log("callback start");
		ApplicationManager.instance.PlayAnimation();
	}
		
	[MonoPInvokeCallback(typeof(EndDelegate))]
	public static void callbackEnd(){
		Debug.Log("callback ended");
		ApplicationManager.instance.StopAnimation();
	}
}