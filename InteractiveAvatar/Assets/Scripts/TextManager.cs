using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.UI;
using AOT;

/// <summary>
/// This class handles all text to speech processes.
/// </summary>
public class TextManager : MonoBehaviour {
	// default language voice name, overwritten with setVoice().
	private string voice = "Dutch Female";
	// default language name, overwritten with setLanguage().
	private string language = "en-US";
	// default ESpeak text to phoneme API host, overwritten with loadHostName().
	private string hostName = "http://test.emekkes.nl";

	// delegate declarations for javascript text to speech callback functions
	// TODO unsure if needed for dynamic linking in MyPlugin.jslib
	public delegate void StartDelegate(int lastword, float elapsedTime);
	public delegate void EndDelegate(int lastword, float elapsedTime);
	public delegate void BoundaryDelegate(int lastword, float elapsedTime);
	public delegate void PauseDelegate(int lastword, float elapsedTime);
	public delegate void ResumeDelegate(int lastword, float elapsedTime);

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
	/// <param name="pauseCallback">The function to call when speech is paused</param>
	/// <param name="resumeCallback">The function to call when speech is resumed</param>
	/// <param name="lang"></param>
	/// <returns>The state of the TTS.</returns>
	[DllImport("__Internal")]
	private static extern string Speak(
		string text,
		string voice,
		StartDelegate startCallback,
		EndDelegate endCallback,
		BoundaryDelegate boundaryCallback,
		PauseDelegate pauseCallback,
		ResumeDelegate resumeCallback,
		string lang
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
	
	/// <summary>
	/// Returns the hostname of the webpage unity is currently loaded on.
	/// </summary>
	/// <returns>Hostname string with protocol prefix</returns>
	[DllImport("__Internal")]
	private static extern string getHostNameString();
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

	/// <summary>
	/// Prints the available system voices.
	/// </summary>
	public void getVoices(){
		Debug.Log("Get Voices");
		Debug.Log(getSystemVoices());
	}

	/// <summary>
	/// Updates local hostname variable with the hostname used of the current
	/// webpage Unity is loaded in. Only useable from within the web page.
	/// </summary>
	public void loadHostName() {
		hostName = getHostNameString();
	}

	/// <summary>
	/// Sets the hostname variable to the specified hostname.
	/// </summary>
	public void setHostName(string hName) {
		hostName = hName;
	}

	/// <summary>
	/// Returns the current hostname string. Make sure it is updated with
	/// loadHostName, defaults to http://test.emekkes.nl otherwise
	/// </summary>
	/// <returns>Hostname string with protocol prefix</returns>
	public string getHostName() {
		return hostName;
	}
	
	/// <summary>
	/// Sets the currect voice of the coach.
	/// </summary>
	/// <param name="voice">Web API voice name</param>
	public void setVoice(string newVoice){
		voice = newVoice;
	}

	public void startActualSpeech(string text)
	{
		// start speech, animation started with callback functions
		Speak(text, voice, 
			callbackStart, callbackEnd, callbackBoundary, 
			callbackPause, callbackResume, language);
	}

	/// <summary>
	/// Returns the current voice.
	/// </summary>
	/// <returns></returns>
	public string getVoice() {
		return voice;
	}


	public void startSpeech(string text) {
		// send original spoken text to SpeechAnimationManager
		SpeechAnimationManager.instance.setText(text);
		
		StartCoroutine(LipSynchronization.getInstance.synchronize(text, language));
	}

	public void stopSpeech() {
		// stop web speech api
		Stop();
		// Instruct SpeechAnimationManager to stop speech animation
		SpeechAnimationManager.instance.stopSpeechAnimation();
	}


	public void setLanguage(string lang) {
		language = lang;
	}
	
	/// <summary>
	/// Callback function for speech synthesis start event. Attached as event
	/// handler for the javascript Web Speech API.
	/// 
	/// Notifies SpeechManager that speech has started.
	/// </summary>
	/// <param name="charIndex">Index of the character in the text at which the event was triggered.</param>
	/// <param name="elapsedTime">Total time that has elapsed while speaking</param>
	[MonoPInvokeCallback(typeof(StartDelegate))]
	public static void callbackStart(int charIndex, float elapsedTime) {
		Debug.Log("callback start at : " + elapsedTime);
		SpeechAnimationManager.instance.startSpeechAnimation();
	}
	
	/// <summary>
	/// Callback function for speech synthesis end event. Attached as event
	/// handler for the javascript Web Speech API.
	///
	/// Notifies SpeechManager that speech has stopped.
	/// </summary>
	/// <param name="charIndex">Index of the character in the text at which the event was triggered.</param>
	/// <param name="elapsedTime">Total time that has elapsed while speaking</param>
	[MonoPInvokeCallback(typeof(EndDelegate))]
	public static void callbackEnd(int charIndex, float elapsedTime) {
		Debug.Log("callback end at : " + elapsedTime);
		// Instruct SpeechAnimationManager to stop speech animation
		SpeechAnimationManager.instance.stopSpeechAnimation();
	}

	/// <summary>
	/// Callback function for speech synthesis onboundary event. Attached as
	/// event handler for the javascript Web Speech API.
	///
	/// Notifies SpeechManager that the next word is being spoken.
	/// </summary>
	/// <param name="charIndex">Index of the character in the text at which the event was triggered.</param>
	/// <param name="elapsedTime">Total time that has elapsed while speaking</param>
	[MonoPInvokeCallback(typeof(BoundaryDelegate))]
	public static void callbackBoundary(int charIndex, float elapsedTime) {
		// set most recently spoken word for SpeechAnimationManager
		SpeechAnimationManager.instance.onBoundary(charIndex);
	}
    
	/// <summary>
	/// Callback function for speech synthesis pause event. Attached as
	/// event handler for the javascript Web Speech API.
	/// 
	/// Notifies SpeechManager that speech has been paused.
	/// </summary>
	/// <param name="charIndex">Index of the character in the text at which the event was triggered.</param>
	/// <param name="elapsedTime">Total time that has elapsed while speaking</param>
	[MonoPInvokeCallback(typeof(PauseDelegate))]
	public static void callbackPause(int charIndex, float elapsedTime) {
		// Instruct SpeechAnimationManager to pause speech animation
		SpeechAnimationManager.instance.pauseSpeechAnimation(charIndex);
	}
    
	/// <summary>
	/// Callback function for speech synthesis resume event. Attached as
	/// event handler for the javascript Web Speech API.
	/// 
	/// Notifies SpeechManager that speech has been resumed.
	/// </summary>
	/// <param name="charIndex">Index of the character in the text at which the event was triggered.</param>
	/// <param name="elapsedTime">Total time that has elapsed while speaking</param>
	[MonoPInvokeCallback(typeof(ResumeDelegate))]
	public static void callbackResume(int charIndex, float elapsedTime) {
		// Instruct SpeechAnimationManager to resume speech animation
		SpeechAnimationManager.instance.resumeSpeechAnimation(charIndex);
	}
}