using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Object = System.Object;

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

	public void getVoices(){
		TextManager.instance.getVoices();
	}

	public void setVoice(string voice){
		TextManager.instance.setVoice(voice);
	}

	public void convertToSpeach(string text){
		TextManager.instance.startSpeach(text);
	}

	public void stopSpeach(){
		TextManager.instance.stopSpeach();
	}

	public void changeBackround(){
		ApplicationManager.instance.changeBackground();
	}

	public void changeCoach(){
		ApplicationManager.instance.changeCoach();
	}

	public void zoom(int zoom){
		ApplicationManager.instance.zoomAvatarCamera(zoom);
	}

	public void moveAvatarHorizontal(int horizontal){
		ApplicationManager.instance.moveCoah(horizontal,0);
	}

	public void moveAvatarVertical(int vertical){
		ApplicationManager.instance.moveCoah(0,vertical);
	}

	/// <summary>
	/// Sends the instruction to pause speech to the TextManager script in Unity.
	/// </summary>
	public void pauseSpeech() {
		TextManager.instance.pauseSpeech();
	}

	/// <summary>
	/// Sends the instruction to pause speech to the TextManager script in Unity.
	/// </summary>
	public void resumeSpeech() {
		TextManager.instance.resumeSpeech();
	}

	/// <summary>
	/// Updates the most recent boundary char encountered while speaking.
	/// </summary>
	public void boundaryChar(int lastChar) {
		TextManager.instance.lastWordIndex(lastChar);
	}
	
	
}
