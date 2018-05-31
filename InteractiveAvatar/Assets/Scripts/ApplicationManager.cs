using System.Collections.Generic;
using UnityEngine;

public class ApplicationManager : MonoBehaviour {
	// public attributes: Unity editor interface fields for ApplicationManager
	// field for prefab avatar objects
	public List<GameObject> coach_prefabs;
	// field for scene component that should hold the avatar object
	public GameObject coach_holder;
	// field for scene component that should hold the background object
	public GameObject background_holder;
	// field for Main Camera
	public Camera avatarCamera;
	// field for screensaver timeOut setting in seconds
	public float timeOut = 30.0f;
	
	
	// cameras
	private Camera[] cams;
	// elapsed time for time out
	private float timeOutTimer;
	// Avatar model
	private GameObject newCoach;

	// Unity Animation component and manager script instance
	private new Animation animation;
	private AnimationsManager animationsManager;
	// Array of viseme Animations
	private AnimationClip[] visemeAnimations;
	
	// background texture and sprite renderer
	Sprite[] backgroundTexture;
	SpriteRenderer backgroundSprite;

	// idle and talk animations are referenced by name from Unity
	private string idle;
	private string talk;
	
	// list of viseme numbers that are currently playing
	private List<int> visemeList;
	
	// layer for viseme (speech) animation
	private const int visemeLayer = 2;
	
	// initial coach avatar selected from prefabs.
	private int coachNumber;
	// initial background selected.
	private int backgroundNumber;

	// Singleton Instance
	private static ApplicationManager instance;

	/// <summary>
	/// Constructs an instance of ApplicationManager if it doesn't exist and
	/// returns the instance if it already exists.
	/// </summary>
	public static ApplicationManager amInstance
	{
		get
		{
			if (instance == null)
			{
				instance = FindObjectOfType<ApplicationManager>();
				DontDestroyOnLoad(instance.gameObject);
			}
			return instance;
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
		on_load();
	}
	
	/// <summary>
	/// Start is called on the frame when a script is enabled just before any
	/// of the Update methods are called the first time. It runs after Awake().
	/// </summary>
	void Start() {
		// ensure screensaver camera is disabled on start (see Update())
		cams = Camera.allCameras;
		foreach( Camera cam in cams){
			if(cam.gameObject.name == "InactiveCamera"){
				cam.enabled = false;
			}
		}
	}
	
	/// <summary>
	/// Load the application by setting the background image renderer, loading 
	/// background textures and loading the coach.
	/// </summary>
	private void on_load(){
		backgroundSprite =  background_holder.GetComponent<SpriteRenderer>();
		load_background();
		load_coach();
	}

	/// <summary>
	/// Load the background texture sprites from Unity.
	/// </summary>
	private void load_background(){
		// load all background texture sprites from Unity
		backgroundTexture = Resources.LoadAll<Sprite>("Textures");
	}

	/// <summary>
	/// Zoom the camera for the avatar by a given value.
	/// </summary>
	/// <param name="zoomValue">The value to zoom by.</param>
	public void zoomAvatarCamera(int zoomValue){
		// represent camera position change as vector for Z-axis
		Vector3 changeZoom = new Vector3(0,0,zoomValue);
		// update camera position by adding the position vector.
		avatarCamera.transform.transform.position += changeZoom;	
	}

	/// <summary>
	/// Move the coach horizontally and vertically.
	/// </summary>
	/// <param name="moveHorizontal">The horizontal movement.</param>
	/// <param name="moveVertical">The vertical movement.</param>
	public void moveCoach(int moveHorizontal, int moveVertical){
		// represent coach position change as vector
		Vector3 changePosition = new Vector3(moveHorizontal, moveVertical, 0);
		// update object position by adding the position vector.
		newCoach.transform.position += changePosition;
	}
	
	/// <summary>
	/// Load the coach based on the current coach number and the coach prefabs.
	/// Will also set the position, rotation and scale of the coach.
	/// 
	/// Also loads the animations for the coach.
	/// </summary>
	private void load_coach() {
		// create new coach Unity Gameobject
		newCoach = Instantiate(coach_prefabs[coachNumber]);
		// add the new coach object to the Unity parent container (CoachHolder)
		newCoach.transform.parent = coach_holder.transform;
		// set default coach position
		newCoach.transform.localPosition = new Vector3(0, 0, 0);
		newCoach.transform.localRotation = Quaternion.identity;
		newCoach.transform.localScale = new Vector3(1, 1, 1);
		// load animations for new coach object
		loadAnimations();
	}

	/// <summary>
	/// Will increase the current background number by 1 and load a new
	/// background sprite based on the new value.
	/// </summary>
	public void changeBackground(){
		// increment background number
		backgroundNumber = (backgroundNumber + 1) % backgroundTexture.Length;
		// set new background image for background image renderer
		backgroundSprite.sprite = backgroundTexture[backgroundNumber];
	}

	/// <summary>
	/// Will increase the current coach number by 1 and load the new coach based
	/// on the new value.
	/// </summary>
	public void changeCoach(){
		// store old coach object position
		Vector3 oldCoachPosition = newCoach.transform.position;
		// increment coach number
		coachNumber = (coachNumber + 1) % coach_prefabs.Count;
		// destroy current coach object
		Destroy(newCoach);
		// load new coach object (using coach number)
		load_coach();
		// update position of new coach object using the old position
		newCoach.transform.position = oldCoachPosition;
	}


	/// <summary>
	/// Loads the animations included with the current coach by accessing them
	/// through the AnimationsManager interface.
	///
	/// Also ensures attributes such as animation layer, wrapmode and speed are
	/// set properly.
	/// </summary>
	private void loadAnimations() {
		// Get animation manager script attached to current avatar GameObject
		animationsManager = newCoach.GetComponent<AnimationsManager>();
		// get viseme animations
		visemeAnimations = animationsManager.getEnglishVisemes();
		// get names of idle, talk and talkmix animations
		idle = animationsManager.getIdle();
		talk = animationsManager.getTalk();
		// Get Unity Animation component attached to current avatar GameObject
		animation = newCoach.GetComponent<Animation>();
		// default for animations is play once
		animation.wrapMode = WrapMode.Once;
		// Set layers for animation, higher layers are overlayed on the lower.
		// e.g. idle (full body) first, talk (mouth) overlayed on idle.
		animation[idle].layer = 1;
		animation[idle].wrapMode = WrapMode.Loop;
		animation[talk].layer = 2;
		
		// ensure viseme animations have the right properties
		foreach (AnimationClip clip in visemeAnimations) {
			if (clip != null) {
				// enable legacy mode for manual animation management.
				clip.legacy = true;
				// add clip to animation component
				animation.AddClip(clip, clip.name);
				// set visime animation layer
				animation[clip.name].layer = visemeLayer;
				// set visime animation speed
				animation[clip.name].speed = 1;
				// set viseme animations to play once.
				animation[clip.name].wrapMode = WrapMode.Once;
			}
		}
	}

	/// <summary>
	/// Plays the specified list of viseme numbers sequentially. Animations are
	/// played once, when an animation ends the next one in the list is played
	/// until there are no remaining visemes in the list.
	/// </summary>
	/// <param name="visList">
	/// List of viseme numbers to play sequentially.
	/// </param>
	public void playVisemeList(List<int> visList) {
		// stop previously playing animations in viseme layer
		stopVisemeAnimations();
		// save list of visemes to play
		visemeList = visList;
		// loop through the set of viseme numbers
		foreach (int visNumber in visList) {
			// TODO api to set transition time, finding the right time to set
			float transitionTime = 0;
			// find the animation clip using the viseme number
			string clipName = visemeAnimations[visNumber].name;
			
			// Add the animation clip to the queue using the specified
			// transition time to smooth out animation. Animations added to the
			// queueu are set to let other animations complete before playing.
			animation.CrossFadeQueued(
				clipName,
				transitionTime,
				QueueMode.CompleteOthers);
		}
	}
	
	/// <summary>
	/// Stops all currently playing viseme animations
	/// </summary>
	private void stopVisemeAnimations() {
		// return if no animations playing
		if (visemeList == null) return;
		// loop through currently playing viseme numbers
		foreach (int visNumber in visemeList) {
			// find the animation
			AnimationClip clip = visemeAnimations[visNumber];
			// stop the animation by blending to weight 0 over 0 seconds.
			animation.Stop(clip.name);
		}
	}

	/// <summary>
	/// Plays the idle and talk animations on the coach.
	///
	/// Will be deprecated with next iteration.
	/// </summary>
	public void playAnimation(){
		// fade in the talk animation over 0 seconds, stopping others
		animation.CrossFade (talk, 0.0f, PlayMode.StopAll);
		// blend the idle animation with the currently playing talk animation.
		animation.Blend(idle);
	}

	/// <summary>
	/// Stop all currently playing animations and play the idle animation.
	///
	/// Will be deprecated with next iteration.
	/// </summary>
	public void stopAnimation(){
		// fade in the idle animation over 0 seconds and stop other animations
		animation.CrossFade (idle, 0.0f, PlayMode.StopAll);
	}

	public void animateFox() {
		// make a list of visemes for the sentenc:
		// "The quick brown fox jumps over the lazy dog"
		List<int> fox = new List<int> {40, 9, 0, 49, 24, 2, 49, 0, 46, 26, 8, 32, 0, 37, 6, 49, 41, 
		0, 35, 25, 9, 31, 45, 41, 0, 11, 38, 21, 0, 40, 9, 0, 27, 3, 42, 1, 0, 35, 6, 50, 0};
		// play the list of animations sequentially
		playVisemeList(fox);
	}

	/// <summary>
	/// This function is called for every frame rendered in Unity. It is
	/// currently used to check for activity and start a screensaver if a
	/// set amount of time passes without activity.
	/// </summary>
	void Update(){
		timeOutTimer += Time.deltaTime;
		// If screen is tapped, reset timer
		if (Input.anyKeyDown
			|| Input.GetAxis("Mouse X") != 0
		    || Input.GetAxis("Mouse Y") != 0) {
			timeOutTimer = 0.0f;
			//Dont active screensaver
			foreach( Camera cam in cams){
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
