using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class Item {
	public string name;
	public AudioClip audioClip;

	public Item(string name, AudioClip aClip){
		this.name = name;
		this.audioClip = aClip;
	}
}

public class ApplicationManager : MonoBehaviour {

	public List<GameObject> coach_prefabs;
	public GameObject coach_holder;
	public GameObject backround_holder;

	public Camera avatarCamera;

	private GameObject new_coach;

	private Animation animation;

	Sprite[] backgroundTexture;
	SpriteRenderer backgroundSprit;

	//[SerializeField]
	private string idle;

	//[SerializeField]
	private string talk;

	private string talkmix;

	public List<Item> itemList;
	public Transform contentPanel;
	public AudioSource audio_source;
	public GameObject audioButton;

	private GameObject changeModelButton;

	private AudioButton start;
	private AudioButton stop;

	private static ApplicationManager _instance;

	//Singleton Initiation
	public static ApplicationManager instance
	{
		get
		{
			if (_instance == null)
			{
				_instance = GameObject.FindObjectOfType<ApplicationManager>();
				DontDestroyOnLoad(_instance.gameObject);
			}
			return _instance;
		}
	}

	private Camera[] cams;
	public float timeOut = 30.0f; // Time Out Setting in Seconds
	private float timeOutTimer = 0.0f;

	int coach_number = 0;
	int backround_number;
	private CoachType coach_type;

	void Start () {

		cams = Camera.allCameras;
		foreach( Camera cam in cams){
			if(cam.gameObject.name == "InactiveCamera"){
				cam.enabled = false;
			}
		}
	}

	//Awake function
	void Awake()
	{
		#if !UNITY_EDITOR && UNITY_WEBGL
		WebGLInput.captureAllKeyboardInput = false;
		#endif
		this.on_load();
	}

	private void on_load(){
		backgroundSprit =  backround_holder.GetComponent<SpriteRenderer>();
		this.load_background();
		this.load_coach();
		this.populateList();
	}

	private void load_background(){
		backgroundTexture = Resources.LoadAll<Sprite>("Textures");
	}

	public void zoomAvatarCamera(int zoomValue){
		Vector3 changeZoom = new Vector3(0,0,zoomValue);
		avatarCamera.transform.transform.position += changeZoom;	
	}

	public void moveCoah(int moveHorizontal, int moveVertical){
		Vector3 changePosition = new Vector3(moveHorizontal, moveVertical, 0);
		new_coach.transform.position += changePosition;
	}
	//load all the coach/avatar
	private void load_coach()
	{
		//int coach_number;
//
//		switch (this.coach_type)
//		{
//		case CoachType.F1:
//			coach_number = 0;
//			break;
//		case CoachType.F2:
//			coach_number = 1;
//			break;
//		case CoachType.F3:
//			coach_number = 2;
//			break;
//		case CoachType.M1:
//			coach_number = 3;
//			break;
//		case CoachType.M2:
//			coach_number = 4;
//			break;
//		case CoachType.M3:
//			coach_number = 5;
//			break;
//		default:
//			coach_number = 0;
//			break;
//		}
		//coach_number = 0;
		new_coach = GameObject.Instantiate(coach_prefabs[coach_number]);
		new_coach.transform.parent = coach_holder.transform;

		new_coach.transform.localPosition = new Vector3(0, 0, 0);
		new_coach.transform.localRotation = Quaternion.identity;
		new_coach.transform.localScale = new Vector3(1, 1, 1);

		this.loadAnimations(new_coach);
	}

	public void changeBackground(){
		this.backround_number = (this.backround_number + 1) % backgroundTexture.Length;
		backgroundSprit.sprite = backgroundTexture[this.backround_number];
	}

	public void changeCoach(){
		Vector3 oldCoachPosition = new_coach.transform.position;
		this.coach_number = (coach_number + 1) % coach_prefabs.Count;
		Destroy(this.new_coach);
		this.stopClip(this.stop);
		this.load_coach();
		new_coach.transform.position = oldCoachPosition;
	}


	public void loadAnimations(GameObject coach){
		idle = coach.GetComponent<AnimationsManager>().getIdle();
		talk = coach.GetComponent<AnimationsManager>().getTalk();
		talkmix = coach.GetComponent<AnimationsManager>().getTalkmix();
		this.animation = this.new_coach.GetComponent<Animation> () as Animation;
		this.animation [idle].layer = 1;
		this.animation [talk].layer = 2;
		this.animation [talkmix].layer = 3;
	}

	void populateList(){
		
		AudioClip[] audioClips = Resources.LoadAll<AudioClip>("Audioclips");
		foreach(AudioClip audioClip in audioClips){
			//if(audioClip.GetType() == typeof(AudioClip)){
			Item item = new Item(audioClip.name, audioClip);
			itemList.Add(item);
			//}
		}
			
		GameObject startButton = Instantiate (audioButton) as GameObject;
		start = startButton.GetComponent <AudioButton> ();
		start.nameLabel.text = "Start";
		start.audioClip = itemList[0].audioClip;
		start.button.onClick.RemoveAllListeners();
		start.button.onClick.AddListener(() => this.playClip(start));
		startButton.transform.SetParent (contentPanel);


		GameObject stopButton = Instantiate (audioButton) as GameObject;
		stop = stopButton.GetComponent <AudioButton> ();
		stop.nameLabel.text = "Stop";
		stop.audioClip = itemList[0].audioClip;
		stop.button.onClick.RemoveAllListeners();
		stop.button.onClick.AddListener(() => this.stopClip(stop));
		stopButton.transform.SetParent (contentPanel);

		GameObject changeCoach = Instantiate (audioButton) as GameObject;
		AudioButton changeButton = changeCoach.GetComponent <AudioButton> ();
		changeButton.nameLabel.text = "Change Coach";
		changeButton.button.onClick.RemoveAllListeners();
		changeButton.button.onClick.AddListener(() => this.changeCoach());
		changeCoach.transform.SetParent (contentPanel);

		GameObject background = Instantiate (audioButton) as GameObject;
		AudioButton changeBackground = background.GetComponent <AudioButton> ();
		changeBackground.nameLabel.text = "Change Background";
		changeBackground.button.onClick.RemoveAllListeners();
		changeBackground.button.onClick.AddListener(() => this.changeBackground());
		changeBackground.transform.SetParent (contentPanel);
	}
		
	public void playClip(AudioButton button){

		this.audio_source.clip = button.audioClip;
		float clipLength = button.audioClip.length;
		this.audio_source.Play();
		this.new_coach.GetComponent<Animation>().wrapMode = WrapMode.Loop;
		this.new_coach.GetComponent<Animation>().CrossFade (talk, 0.0f, PlayMode.StopAll);
		this.new_coach.GetComponent<Animation>().Blend(idle);
		this.new_coach.GetComponent<Animation>().Blend(talkmix);
		this.StartCoroutine(waitForAudioToFinish(clipLength));
		button.nameLabel.text = "Replay";
	}

	public void stopClip(AudioButton button){
		this.audio_source.clip = button.audioClip;
		float clipLength = button.audioClip.length;
		this.audio_source.Stop();
		this.new_coach.GetComponent<Animation>().wrapMode = WrapMode.Loop;
		this.new_coach.GetComponent<Animation>().CrossFade (idle, 0.0f, PlayMode.StopAll);
		this.StartCoroutine(waitForAudioToFinish(clipLength));
		this.start.nameLabel.text = "Start";
	}

	public void PlayAnimation(){
		this.new_coach.GetComponent<Animation>().wrapMode = WrapMode.Loop;
		this.new_coach.GetComponent<Animation>().CrossFade (talk, 0.0f, PlayMode.StopAll);
		this.new_coach.GetComponent<Animation>().Blend(idle);
	}

	public void StopAnimation(){
		this.new_coach.GetComponent<Animation>().wrapMode = WrapMode.Loop;
		this.new_coach.GetComponent<Animation>().CrossFade (idle, 0.0f, PlayMode.StopAll);
	}


	IEnumerator waitForAudioToFinish(float waitTime){
		yield return new WaitForSeconds(waitTime);
		//this.animation.PlayQueued(idle);
		this.new_coach.GetComponent<Animation>().CrossFade (idle, 0.5f, PlayMode.StopAll);
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
}
