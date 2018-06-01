using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.UI;
using AOT;

/// <summary>
/// This class handles all text to speech processes.
/// </summary>
public class TextManager : MonoBehaviour {

	private string voice = "Dutch Female";

	// whether the agent is currently paused, initialised to false.
	private bool isSpeaking = false;
	private bool isPaused = false;
	// current text string to be spoken.
	private string textInput = null;
	// most recent boundary character encountered while speaking text.
	private static int lastWordIndex = 0;

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
	/// <param name="boundaryCallback">The function to call when a word is finished.</param>
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

	private static TextManager instance;

	/// <summary>
	/// The initiation of the singleton: either returns the instance of it already exists and creates an instantiates
	/// an instance otherwise.
	/// </summary>
	public static TextManager tmInstance
	{
		get
		{
			if (instance == null)
			{
				instance = GameObject.FindObjectOfType<TextManager>();
				DontDestroyOnLoad(instance.gameObject);
			}
			return instance;
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
		voice = newVoice;
	}


	public void startSpeech(string text) {
		textInput = text;
		isSpeaking = true;
		// start speech, animation started with callback functions
		Speak(text, this.voice, callbackStart, callbackEnd, callbackBoundary);
	}

	public void startDemo() {
		Debug.Log("startDemo()");
		textInput = "The quick brown fox jumps over the lazy dog.";
		isSpeaking = true;
		// start speech, animation started with callback functions
		Speak(textInput, voice, callbackDemoStart, callbackEnd, callbackBoundary);
	}

	public void stopSpeech() {
		textInput = null;
		isSpeaking = false;
		//stop speech
		Stop();
		//stop animation
		ApplicationManager.amInstance.stopAnimation();
	}

	/// <summary>
	/// Pauses speech synthesis and animation from a speaking state.
	/// </summary>
	public void pauseSpeech() {
		// if not speaking do nothing
		if (!isSpeaking) return;
		
		isPaused = true;
		isSpeaking = false;
		
		// store remainder of text for pause.
		textInput = textInput.Substring(lastWordIndex);
		lastWordIndex = 0;
		
		// stop speaking
		Stop();
		// stop animation
		ApplicationManager.amInstance.stopAnimation();
		
		Debug.Log("Paused Speech!");
	}

	/// <summary>
	/// Resumes speech synthesis and animation from a paused state.
	/// </summary>
	public void resumeSpeech() {
		// if not paused do nothing
		if (!isPaused) return;
		
		isPaused = false;
		isSpeaking = true;
		
		// resume speaking with remainder of text after pause.
		Speak(textInput, voice, callbackStart, callbackEnd, callbackBoundary);
		Debug.Log("Resumed Speech!");
	}

	[MonoPInvokeCallback(typeof(StartDelegate))]
	public static void callbackStart(float elapsedTime){
		Debug.Log("callback start at : " + elapsedTime);
		
		ApplicationManager.amInstance.playAnimation();
	}
	
	[MonoPInvokeCallback(typeof(StartDelegate))]
	public static void callbackDemoStart(float elapsedTime){
		Debug.Log("callback Demo start at : " + elapsedTime);
		
		ApplicationManager.amInstance.animateFox();
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
	/// <param name="lastWord">Index of the most recently encountered word while speaking.</param>
	/// <param name="elapsedTime">Time that has elapsed</param>
	[MonoPInvokeCallback(typeof(BoundaryDelegate))]
	public static void callbackBoundary(int lastWord, float elapsedTime) {
		lastWordIndex = lastWord;
		Debug.Log("Last Boundary Char = " + lastWord + " at time : " + elapsedTime);
	}

    /// <summary>
	/// Returns true if speaking and false if not. This function is used for testing.
	/// </summary>
    public bool getIsSpeaking()
    {
        return isSpeaking;
    }

    /// <summary>
	/// Returns true if speaking and false if not. This function is used for testing.
	/// </summary>
    public bool getIsPaused()
    {
        return isPaused;
    }
}