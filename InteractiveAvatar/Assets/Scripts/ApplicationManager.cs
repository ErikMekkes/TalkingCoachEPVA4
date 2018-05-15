using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Map container for audioclips.
/// </summary>
[System.Serializable]
public class Item {
	/// <summary>
	/// The name of the audioclip.
	/// </summary>
	public string name;
	
	/// <summary>
	/// The AudioClip itself.
	/// </summary>
	public AudioClip audioClip;

	/// <summary>
	/// Constructor for an Item.
	/// </summary>
	/// <param name="name">The name of the audioclip.</param>
	/// <param name="aClip">The audioclip itself.</param>
	public Item(string name, AudioClip aClip){
		this.name = name;
		this.audioClip = aClip;
	}
}

/// <summary>
/// Manager for the applications, determines the flow when running.
/// </summary>
public class ApplicationManager : MonoBehaviour {

	/// <summary>
	/// Prefabs of coaches.
	/// </summary>
	public List<GameObject> coach_prefabs;
	
	/// <summary>
	/// A holder for the coach.
	/// </summary>
	public GameObject coach_holder;
	
	/// <summary>
	/// A holder for the background.
	/// </summary>
	public GameObject backround_holder;

	/// <summary>
	/// The camera for the avatar.
	/// </summary>
	public Camera avatarCamera;

	/// <summary>
	/// The current coach.
	/// </summary>
	private GameObject new_coach;

	/// <summary>
	/// The current animation.
	/// </summary>
	private Animation animation;

	/// <summary>
	/// The background texture.
	/// </summary>
	Sprite[] backgroundTexture;
	
	/// <summary>
	/// The background sprite renderer.
	/// </summary>
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

	/// <summary>
	/// The start button.
	/// </summary>
	private AudioButton start;
	
	/// <summary>
	/// The stop button.
	/// </summary>
	private AudioButton stop;

	/// <summary>
	/// The Singleton instance of the class.
	/// </summary>
	private static ApplicationManager _instance;

	/// <summary>
	/// Constructs an instance of ApplicationManager if it doesn't exist and returns the
	/// instance if it already exists.
	/// </summary>
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

	/// <summary>
	/// Starts the application by instantiating all cameras.
	/// </summary>
	void Start () {

		cams = Camera.allCameras;
		foreach( Camera cam in cams){
			if(cam.gameObject.name == "InactiveCamera"){
				cam.enabled = false;
			}
		}
	}

	/// <summary>
	/// Handler for the application when it is awake. Won't capture all keyboard input and will load the application.
	/// </summary>
	void Awake()
	{
		#if !UNITY_EDITOR && UNITY_WEBGL
		WebGLInput.captureAllKeyboardInput = false;
		#endif
		this.on_load();
	}

	/// <summary>
	/// Load the application by setting the background sprite, loading background textures, loading the coach
	/// and loading audioclips.
	/// </summary>
	private void on_load(){
		backgroundSprit =  backround_holder.GetComponent<SpriteRenderer>();
		this.load_background();
		this.load_coach();
		this.populateList();
	}

	/// <summary>
	/// Load the background texture.
	/// </summary>
	private void load_background(){
		backgroundTexture = Resources.LoadAll<Sprite>("Textures");
	}

	/// <summary>
	/// Zoom the camera for the avatar by a given value.
	/// </summary>
	/// <param name="zoomValue">The value to zoom by.</param>
	public void zoomAvatarCamera(int zoomValue){
		Vector3 changeZoom = new Vector3(0,0,zoomValue);
		avatarCamera.transform.transform.position += changeZoom;	
	}

	/// <summary>
	/// Move the coach horizontally and vertically.
	/// </summary>
	/// <param name="moveHorizontal">The horizontal movement.</param>
	/// <param name="moveVertical">The vertical movement.</param>
	public void moveCoah(int moveHorizontal, int moveVertical){
		Vector3 changePosition = new Vector3(moveHorizontal, moveVertical, 0);
		new_coach.transform.position += changePosition;
	}
	
	/// <summary>
	/// Load the coach based on the current coach number and the coach prefabs. Will also set the
	/// position, rotation and scale of the coach.
	/// </summary>
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

	/// <summary>
	/// Will increase the current background number by 1 and load a new background sprite based on the new value.
	/// </summary>
	public void changeBackground(){
		this.backround_number = (this.backround_number + 1) % backgroundTexture.Length;
		backgroundSprit.sprite = backgroundTexture[this.backround_number];
	}

	/// <summary>
	/// Will increase the current coach number by 1 and load the new coach based on the new value.
	/// </summary>
	public void changeCoach(){
		Vector3 oldCoachPosition = new_coach.transform.position;
		this.coach_number = (coach_number + 1) % coach_prefabs.Count;
		Destroy(this.new_coach);
		this.stopClip(this.stop);
		this.load_coach();
		new_coach.transform.position = oldCoachPosition;
	}

	/// <summary>
	/// Loads the ide, talk and the mixed talk animatons for a given coach.
	/// </summary>
	/// <param name="coach">The coach to load the animations for.</param>
	public void loadAnimations(GameObject coach){
		idle = coach.GetComponent<AnimationsManager>().getIdle();
		talk = coach.GetComponent<AnimationsManager>().getTalk();
		talkmix = coach.GetComponent<AnimationsManager>().getTalkmix();
		this.animation = this.new_coach.GetComponent<Animation> () as Animation;
		this.animation [idle].layer = 1;
		this.animation [talk].layer = 2;
		this.animation [talkmix].layer = 3;
	}

	/// <summary>
	/// Add audioclips to a temporarylist and instantiate start, stop, coach change and background change buttons.
	/// </summary>
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
		
	/// <summary>
	/// Play a clip assigned to an AudioButton.
	/// </summary>
	/// <param name="button">The button the audioclip is contained within.</param>
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

	/// <summary>
	/// Stop playing a clip assigned to an AudioButton.
	/// </summary>
	/// <param name="button">The button the audioclip is contained within.</param>
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
	/// Play the loaded animation on the coach.
	/// </summary>
	public void PlayAnimation(){
		this.new_coach.GetComponent<Animation>().wrapMode = WrapMode.Loop;
		this.new_coach.GetComponent<Animation>().CrossFade (talk, 0.0f, PlayMode.StopAll);
		this.new_coach.GetComponent<Animation>().Blend(idle);
	}

	/// <summary>
	/// Stop the loaded animation on the coach.
	/// </summary>
	public void StopAnimation(){
		this.new_coach.GetComponent<Animation>().wrapMode = WrapMode.Loop;
		this.new_coach.GetComponent<Animation>().CrossFade (idle, 0.0f, PlayMode.StopAll);
	}


	/// <summary>
	/// Wait for finish before making the coach idle.
	/// </summary>
	/// <param name="waitTime">The amount of seconds to wait.</param>
	/// <returns>The IEnumerator for waiting.</returns>
	IEnumerator waitForAudioToFinish(float waitTime){
		yield return new WaitForSeconds(waitTime);
		//this.animation.PlayQueued(idle);
		this.new_coach.GetComponent<Animation>().CrossFade (idle, 0.5f, PlayMode.StopAll);
	}

	/// <summary>
	/// This function is called every Time.deltaTime milliseconds and can process all necessary calls.
	/// </summary>
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
