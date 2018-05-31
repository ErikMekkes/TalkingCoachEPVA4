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
		TextManager.amInstance.getVoices();
	}

	/// <summary>
	/// Set the voice.
	/// </summary>
	/// <param name="voice">The new voice to set to.</param>
	public void setVoice(string voice){
		TextManager.amInstance.setVoice(voice);
	}

	/// <summary>
	/// Convert text to speech.
	/// </summary>
	/// <param name="text">The convertable text.</param>
	public void convertToSpeech(string text){
		TextManager.amInstance.startSpeech(text);
	}

	/// <summary>
	/// Convert text to speech.
	/// </summary>
	public void startDemo(){
		TextManager.amInstance.startDemo();
	}

	/// <summary>
	/// Stop the speech.
	/// </summary>
	public void stopSpeech(){
		TextManager.amInstance.stopSpeech();
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

	/// <summary>
	/// Sends the instruction to pause speech to the TextManager script in Unity.
	/// </summary>
	public void pauseSpeech() {
		TextManager.amInstance.pauseSpeech();
	}

	/// <summary>
	/// Sends the instruction to pause speech to the TextManager script in Unity.
	/// </summary>
	public void resumeSpeech() {
		TextManager.amInstance.resumeSpeech();
	}
}
