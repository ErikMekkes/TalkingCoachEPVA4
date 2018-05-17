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

	private Animation _animation;
	private AnimationsManager _animationsManager;

	Sprite[] backgroundTexture;
	SpriteRenderer backgroundSprit;

	// Only thing required to find an animation in Untiy is a name
	// TODO find out what happens with same names
	private string idle;
	private string talk;
	// list of  of all included viseme animations
	private string[] _visemes;
	
	// was included, looks unnecessary, set te be removed
	//TODO remove unused parts such as buttons and clips
//	private string _talkmix;

	public List<Item> itemList;
	public Transform contentPanel;
	public AudioSource audio_source;
	public GameObject audioButton;

	private GameObject changeModelButton;

	private AudioButton start;
	private AudioButton stop;

	private Camera[] cams;
	public float timeOut = 30.0f; // Time Out Setting in Seconds
	private float timeOutTimer = 0.0f;

	// initial coach avatar selected from prefabs.
	private int _coachNumber = 0;
	// initial background selected.
	private int _backgroundNumber = 0;

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
	
	/// <summary>
	/// Awake is called when this script instance is being loaded.
	/// </summary>
	void Awake()
	{
		// disable capturing keyboard input in browser.
		#if !UNITY_EDITOR && UNITY_WEBGL
		WebGLInput.captureAllKeyboardInput = false;
		#endif
		// run the on_load function
		this.on_load();
	}
	
	/// <summary>
	/// Start is called on the frame when a script is enabled just before any
	/// of the Update methods are called the first time. It runs after Awake().
	/// </summary>
	void Start () {
		// disable inactive cameras?
		cams = Camera.allCameras;
		foreach( Camera cam in cams){
			if(cam.gameObject.name == "InactiveCamera"){
				cam.enabled = false;
			}
		}
		
		//TODO REMOVE THIS DEMO OF RUNNING ANIMATION
		runningDemo();
	}

	private void runningDemo() {
		// hihi puns, it runs...
		// demonstrate calling animation by name, second coach runs
		// change coach, viseme[0] is set as a running animation in Unity for coach 2
		Destroy(new_coach);
		_coachNumber = 1;
		load_coach();
		// play animations forever
		_animation.wrapMode = WrapMode.Loop;
		// make the viseme running a base layer animation
		_animation[_visemes[0]].layer = 0;
		// talk
		// Can choose to stop all animations beforehand, or stop all in same layer
		_animation.CrossFade (talk, 0.0f, PlayMode.StopAll);
		// talk and run at the same time
		// PlayViseme stops all in the same layer beforehand
		PlayViseme(0);
	}

	private void on_load(){
		backgroundSprit =  backround_holder.GetComponent<SpriteRenderer>();
		load_background();
		load_coach();
		populateList();
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
		
		
		new_coach = GameObject.Instantiate(coach_prefabs[_coachNumber]);
		new_coach.transform.parent = coach_holder.transform;

		new_coach.transform.localPosition = new Vector3(0, 0, 0);
		new_coach.transform.localRotation = Quaternion.identity;
		new_coach.transform.localScale = new Vector3(1, 1, 1);

		this.loadAnimations();
	}

	public void changeBackground(){
		this._backgroundNumber = (this._backgroundNumber + 1) % backgroundTexture.Length;
		backgroundSprit.sprite = backgroundTexture[this._backgroundNumber];
	}

	public void changeCoach(){
		Vector3 oldCoachPosition = new_coach.transform.position;
		_coachNumber = (_coachNumber + 1) % coach_prefabs.Count;
		Destroy(this.new_coach);
		this.stopClip(this.stop);
		this.load_coach();
		new_coach.transform.position = oldCoachPosition;
	}


	public void loadAnimations() {
		// Get animation manager script attached to current avatar GameObject
		_animationsManager = new_coach.GetComponent<AnimationsManager>();
		// get names of viseme animations
		_visemes = _animationsManager.getEnglishVisemes56();
		// get names of idle, talk and talkmix animations
		idle = _animationsManager.getIdle();
		talk = _animationsManager.getTalk();
		// doesn't seem useful at the moment
//		_talkmix = _animationsManager.getTalkmix();
		// Get Unity Animation component attached to current avatar GameObject
		_animation = new_coach.GetComponent<Animation>();
		// Set layers for animation, higher layers are overlayed on the lower.
		// e.g. idle (full body) first, talk (mouth) overlayed on idle.
		// TODO discuss layers with other team
		_animation[idle].layer = 1;
		_animation[talk].layer = 2;
		foreach (string viseme in _visemes) {
			if (!string.IsNullOrEmpty(viseme)) {
				_animation[viseme].layer = 2;
			}
		}
//		_animation [_talkmix].layer = 3;
	}

	void populateList(){
		
		AudioClip[] audioClips = Resources.LoadAll<AudioClip>("Audioclips");
		foreach(AudioClip audioClip in audioClips){
			//if(audioClip.GetType() == typeof(AudioClip)){
			Item item = new Item(audioClip.name, audioClip);
			itemList.Add(item);
			//}
		}
			
//		GameObject startButton = Instantiate (audioButton) as GameObject;
//		start = startButton.GetComponent <AudioButton> ();
//		start.nameLabel.text = "Start";
//		start.audioClip = itemList[0].audioClip;
//		start.button.onClick.RemoveAllListeners();
//		start.button.onClick.AddListener(() => this.playClip(start));
//		startButton.transform.SetParent (contentPanel);


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
		
//	public void playClip(AudioButton button){
//
//		this.audio_source.clip = button.audioClip;
//		float clipLength = button.audioClip.length;
//		this.audio_source.Play();
//		this.new_coach.GetComponent<Animation>().wrapMode = WrapMode.Loop;
//		this.new_coach.GetComponent<Animation>().CrossFade (talk, 0.0f, PlayMode.StopAll);
//		this.new_coach.GetComponent<Animation>().Blend(idle);
//		this.new_coach.GetComponent<Animation>().Blend(_talkmix);
//		this.StartCoroutine(waitForAudioToFinish(clipLength));
//		button.nameLabel.text = "Replay";
//	}

	public void stopClip(AudioButton button){
		this.audio_source.clip = button.audioClip;
		float clipLength = button.audioClip.length;
		this.audio_source.Stop();
		this.new_coach.GetComponent<Animation>().wrapMode = WrapMode.Loop;
		this.new_coach.GetComponent<Animation>().CrossFade (idle, 0.0f, PlayMode.StopAll);
		this.StartCoroutine(waitForAudioToFinish(clipLength));
		this.start.nameLabel.text = "Start";
	}

	/// <summary>
	/// Plays the numbered viseme animation. Viseme animations have their own
	/// animation layer, when playing a new viseme, previous animations in the
	/// same layer as the new animation are stopped.
	///
	/// Animations are set to loop continuously when called.
	/// </summary>
	/// <param name="visNumber">
	/// Number of viseme Animation to play.
	/// </param>
	public void PlayViseme(int visNumber) {
		// loop animations endlessly
		_animation.wrapMode = WrapMode.Loop;
		// play the given viseme animation without fading in, stopping previous
		// animations in the same layer beforehand (other visemes)
		_animation.CrossFade(_visemes[visNumber], 0.0f, PlayMode.StopSameLayer);
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
