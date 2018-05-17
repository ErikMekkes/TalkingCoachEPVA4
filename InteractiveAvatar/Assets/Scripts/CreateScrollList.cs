using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Audio;
using System.Collections.Generic;
using System.IO;

[System.Serializable]
public class Item {
	public string name;
	public AudioClip audioClip;

	public Item(string name, AudioClip aClip){
		this.name = name;
		this.audioClip = aClip;
	}
}

public class CreateScrollList : MonoBehaviour {

	private Animation animation;

	[SerializeField]
	private string idle = "male_idle_01_biped_animclip";
	
	[SerializeField]
	public string talk = "avatar_talk";

	public GameObject maleAvatar;
	public GameObject femaleAvatar;

	// Use this for initialization
	public GameObject audioButton;
	public List<Item> itemList;
	public Transform contentPanel;
	public AudioSource audio;

	private AudioButton start;
	private AudioButton stop;

	private Camera[] cams;
	public float timeOut = 30.0f; // Time Out Setting in Seconds
	private float timeOutTimer = 0.0f;


	void Start () {

		cams = Camera.allCameras;
		foreach( Camera cam in cams){
			if(cam.gameObject.name == "InactiveCamera"){
				cam.enabled = false;
			}
		}

		//Screen.SetResolution(Screen.currentResolution.width, Screen.currentResolution.height, false);
		//itemList = new List<Item>();
		//this.animation = this.GetComponent<Animation> () as Animation;
		this.animation = femaleAvatar.GetComponent<Animation> () as Animation;
		this.animation [idle].layer = 1;
		this.animation [talk].layer = 2;
		this.animation.CrossFadeQueued(idle);

		populateList();

	}

	void Update(){
		timeOutTimer += Time.deltaTime;
		// If screen is tapped, reset timer
		if(Input.anyKeyDown || Input.GetAxis("Mouse X") != 0 || Input.GetAxis("Mouse Y") != 0){
			timeOutTimer = 0.0f;
			//Dont active screensaver
			foreach( Camera cam in cams){
				//Debug.Log("main camera : " + Camera.current);
				if(cam.gameObject.name == "InactiveCamera"){
					cam.enabled = false;
				}else{
					cam.enabled = true;
				}
			}

		}
		// If timer reaches zero, start screensaver
		if (timeOutTimer > timeOut){
			//Activate Screensaver
			foreach( Camera cam in cams){
				if(cam.gameObject.name == "InactiveCamera"){
					cam.enabled = true;
				}else{
					cam.enabled = false;
				}
			}

		}	
	}

	void populateList(){
		this.audio = this.GetComponent<AudioSource>();
		Object[] audioClips = Resources.LoadAll("Audioclips");
		foreach(var audioClip in audioClips){
			if(audioClip.GetType() == typeof(AudioClip)){
				AudioClip aClip = (AudioClip) audioClip;
				Item item = new Item(aClip.name, aClip);
				itemList.Add(item);
			}
		}

		/*foreach  (var item in itemList){
			GameObject newButton = Instantiate (audioButton) as GameObject;
			AudioButton button = newButton.GetComponent <AudioButton> ();
			button.nameLabel.text = item.name;
			button.audioClip = item.audioClip;
			button.button.onClick.RemoveAllListeners();
			button.button.onClick.AddListener(() => this.playSound(button));
			newButton.transform.SetParent (contentPanel);
		}*/

		GameObject startButton = Instantiate (audioButton) as GameObject;
		start = startButton.GetComponent <AudioButton> ();
		start.nameLabel.text = "Start";
		start.audioClip = itemList[0].audioClip;
		start.button.onClick.RemoveAllListeners();
		start.button.onClick.AddListener(() => this.playSound(start));
		startButton.transform.SetParent (contentPanel);


		GameObject stopButton = Instantiate (audioButton) as GameObject;
		stop = stopButton.GetComponent <AudioButton> ();
		stop.nameLabel.text = "Stop";
		stop.audioClip = itemList[0].audioClip;
		stop.button.onClick.RemoveAllListeners();
		stop.button.onClick.AddListener(() => this.stopSound(stop));
		stopButton.transform.SetParent (contentPanel);


		/*GameObject replayButton = Instantiate (audioButton) as GameObject;
		AudioButton button3 = replayButton.GetComponent <AudioButton> ();
		button3.nameLabel.text = "Replay";
		button3.audioClip = itemList[0].audioClip;
		button3.button.onClick.RemoveAllListeners();
		button3.button.onClick.AddListener(() => this.playSound(button3));
		replayButton.transform.SetParent (contentPanel);*/

	}

	public void playSound(AudioButton button){

		this.audio.clip = button.audioClip;
		float clipLength = button.audioClip.length;
		this.audio.Play();
		this.animation.wrapMode = WrapMode.Loop;
		this.animation.CrossFade (talk, 0.0f, PlayMode.StopAll);
		this.animation.Blend(idle);
		this.StartCoroutine(waitForAudioToFinish(clipLength));
		button.nameLabel.text = "Replay";
	}

	public void stopSound(AudioButton button){
		this.audio.clip = button.audioClip;
		float clipLength = button.audioClip.length;
		this.audio.Stop();
		this.animation.wrapMode = WrapMode.Loop;
		this.animation.CrossFade (idle, 0.0f, PlayMode.StopAll);
		this.StartCoroutine(waitForAudioToFinish(clipLength));
		this.start.nameLabel.text = "Start";
	}


	IEnumerator waitForAudioToFinish(float waitTime){
		yield return new WaitForSeconds(waitTime);
		//this.animation.PlayQueued(idle);
		this.animation.CrossFade (idle, 0.5f, PlayMode.StopAll);
	}
	
}
