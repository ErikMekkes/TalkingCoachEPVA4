using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.UI;
using AOT;

public class TextManager : MonoBehaviour {

	private string voice = "Dutch Female";

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

	public void startSpeach(string text){
		if( Application.platform == RuntimePlatform.WebGLPlayer){
			Speak(text, this.voice, callbackStart, callbackEnd);
		}	
	}

	public void stopSpeach(){
		if( Application.platform == RuntimePlatform.WebGLPlayer){
			Stop();
		}
		ApplicationManager.instance.StopAnimation();
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