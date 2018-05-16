using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
}
