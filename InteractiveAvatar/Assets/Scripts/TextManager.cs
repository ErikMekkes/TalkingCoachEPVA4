using System;
using System.CodeDom;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Timers;
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
	
	private float timeOutTimer = 0.0f;
	private Boolean talking = false;
	private float[] animationTiming;
	private float animationDuration = 0.0575757575757576f;

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

	void Update()
	{
		timeOutTimer += Time.deltaTime;

		if (talking)
		{
			for (int i = 0; i < animationTiming.Length - 1; i++)
			{
				if (timeOutTimer >= animationTiming[i] && timeOutTimer < animationTiming[i + 1])
				{
					animationTiming[i] = float.MaxValue;
					ApplicationManager.instance.changeCoach();
					break;
				}
			}
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
		if( Application.platform == RuntimePlatform.WebGLPlayer)
		{
			timeOutTimer = 0;
			talking = true;
			animationTiming = new float[text.Length];
			float j = 0.0f;
			for (int i = 0; i < animationTiming.Length; i++)
			{
				animationTiming[i] = j + i * animationDuration;
			}
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