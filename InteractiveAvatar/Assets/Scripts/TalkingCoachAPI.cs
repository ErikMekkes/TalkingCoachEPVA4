using UnityEngine;

/// <summary>
/// API for controlling the TalkingCoach Object
/// 
/// These functions are attached to the TalkingCoach object in the Unity
/// scene. They are exposed and can be called by using SendMessage from within
/// Unity scripts or javascript with WebGL.
/// 
/// Functions called by WebGLTemplates/InteractiveAvatar/UnityInteraction.js
/// </summary>
public class TalkingCoachAPI : MonoBehaviour {

	/// <summary>
	/// Get the voices.
	/// </summary>
	public void getVoices(){
		TextManager.tmInstance.getVoices();
	}

	/// <summary>
	/// Set the voice of the talking coach.
	/// </summary>
	/// <param name="voice">Web speech API voice name.</param>
	public void setVoice(string voice){
		TextManager.tmInstance.setVoice(voice);
	}

	/// <summary>
	/// Set the language of the talking coach.
	/// </summary>
	/// <param name="language">Web speech API language name.</param>
	public void setLanguage(string language) {
		TextManager.tmInstance.setLanguage(language);
	}

	/// <summary>
	/// Set the hostname for espeak phoneme server API calls.
	/// </summary>
	/// <param name="hName">Hostname with protocal prefix and port suffix.</param>
	public void setPhonemeServerHost(string hName) {
		TextManager.tmInstance.setPhonemeServerHost(hName);
	}

	/// <summary>
	/// Convert text to speech.
	/// </summary>
	/// <param name="text">The convertable text.</param>
	public void convertToSpeech(string text){
		TextManager.tmInstance.startSpeech(text);
	}

	/// <summary>
	/// Stop the speech. And indirectly the animation through stop callback.
	/// </summary>
	public void stopSpeech() {
		TextManager.tmInstance.stopSpeech();
	}

	/// <summary>
	/// Pauses the speech, relies on onboundary events for accuracy.
	/// Use window.speechSynthesis.pause() if onboundary not available.
	/// </summary>
	public void pauseSpeech() {
		SpeechAnimationManager.instance.pauseSpeech();
	}

	/// <summary>
	/// Resumes the speech, relies on pauseSpeech, no effect if still speaking.
	/// Use window.speechSynthesis.resume() if onboundary not available.
	/// </summary>
	public void resumeSpeech() {
		SpeechAnimationManager.instance.resumeSpeech();
	}

	/// <summary>
	/// Change the background.
	/// </summary>
	public void changeBackround(){
		ApplicationManager.amInstance.changeBackground();
	}

	/// <summary>
	/// Change the coach.
	/// </summary>
	public void changeCoach(){
		ApplicationManager.amInstance.changeCoach();
	}

	/// <summary>
	/// Zoom the avatar's camera.
	/// </summary>
	/// <param name="zoom">The value to zoom by.</param>
	public void zoom(int zoom){
		ApplicationManager.amInstance.zoomAvatarCamera(zoom);
	}

	/// <summary>
	/// Move the avatar horizontally.
	/// </summary>
	/// <param name="horizontal">The value to move the avatar by.</param>
	public void moveAvatarHorizontal(int horizontal){
		ApplicationManager.amInstance.moveCoach(horizontal,0);
	}

	/// <summary>
	/// Move the avatar vertically.
	/// </summary>
	/// <param name="vertical">The value to move the avatar by.</param>
	public void moveAvatarVertical(int vertical){
		ApplicationManager.amInstance.moveCoach(0,vertical);
	}
}
