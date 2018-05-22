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
	private float timeOutTimer = 0.0f;
	// Avatar model
	private GameObject new_coach;

	// Animation components and manager
	private Animation _animation;
	private AnimationsManager _animationsManager;
	// Array of viseme Animations
	private AnimationClip[] _visemeAnimations;
	
	// background texture and sprite renderer
	Sprite[] backgroundTexture;
	SpriteRenderer backgroundSprite;

	// Only thing required to find an animation in Untiy is a name
	// TODO find out what happens with same names
	private string idle;
	private string talk;
	
	// list of viseme numbers that are playing
	private List<int> _visemeList;
	
	// layer for viseme (speech) animation
	private const int Viseme_Layer = 2;
	
	// initial coach avatar selected from prefabs.
	private int _coachNumber = 0;
	// initial background selected.
	private int _backgroundNumber = 0;

	// Singleton Instance
	private static ApplicationManager _instance;

	/// <summary>
	/// Constructs an instance of ApplicationManager if it doesn't exist and
	/// returns the instance if it already exists.
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
	
	/// <summary>
	/// Awake is called when this script instance is being loaded.
	/// </summary>
	void Awake()
	{
		// disable capturing keyboard input in browser.
		#if !UNITY_EDITOR && UNITY_WEBGL
		WebGLInput.captureAllKeyboardInput = false;
		tabindex="1";
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
		
		//TODO REMOVE THIS DEMO OF RUNNING ANIMATION
		//runningDemo();
	}

	private void runningDemo() {
		// demonstrate calling a sequence of viseme animations with the prefab
		// demo coach that was added with coach number 1
		Destroy(new_coach);
		_coachNumber = 1;
		load_coach();
		// make a list of 5 viseme animations
		List<int> visList = new List<int> {0, 1, 2, 3, 4, 5};
		// play the list of animations sequentially
		playVisemeList(visList);
		// can be used to show stopping current speech animations works
		//visList = new List<int> {5};
		// can be used to show a random much longer order works
		
		
		//visList = new List<int> {5,3,1,4,5,2,3,4,1,0,2,3,1,0,4,5,2,2,3,1,4,5,2};
		// play the list of animations sequentially
		//playVisemeList(visList);
	}
	
	/// <summary>
	/// Load the application by setting the background sprite, loading 
	/// background textures and loading the coach.
	/// </summary>
	private void on_load(){
		backgroundSprite =  background_holder.GetComponent<SpriteRenderer>();
		load_background();
		load_coach();
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
	/// Load the coach based on the current coach number and the coach prefabs.
	/// Will also set the position, rotation and scale of the coach.
	/// 
	/// Also loads the animations for the coach.
	/// </summary>
	private void load_coach() {
		new_coach = GameObject.Instantiate(coach_prefabs[_coachNumber]);
		new_coach.transform.parent = coach_holder.transform;

		new_coach.transform.localPosition = new Vector3(0, 0, 0);
		new_coach.transform.localRotation = Quaternion.identity;
		new_coach.transform.localScale = new Vector3(1, 1, 1);

		loadAnimations();
	}

	/// <summary>
	/// Will increase the current background number by 1 and load a new
	/// background sprite based on the new value.
	/// </summary>
	public void changeBackground(){
		_backgroundNumber = (_backgroundNumber + 1) % backgroundTexture.Length;
		backgroundSprite.sprite = backgroundTexture[_backgroundNumber];
	}

	/// <summary>
	/// Will increase the current coach number by 1 and load the new coach based
	/// on the new value.
	/// </summary>
	public void changeCoach(){
		Vector3 oldCoachPosition = new_coach.transform.position;
		_coachNumber = (_coachNumber + 1) % coach_prefabs.Count;
		Destroy(new_coach);
		load_coach();
		new_coach.transform.position = oldCoachPosition;
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
		_animationsManager = new_coach.GetComponent<AnimationsManager>();
		// get viseme animations
		_visemeAnimations = _animationsManager.getEnglishVisemes();
		// get names of idle, talk and talkmix animations
		idle = _animationsManager.getIdle();
		talk = _animationsManager.getTalk();
		// Get Unity Animation component attached to current avatar
		//
		//
		//
		//
		//
		//
		//
		//
		//
		//
		//
		//
		//
		//
		//
		// +GameObject
		_animation = new_coach.GetComponent<Animation>();
		// default for animations is play once
		_animation.wrapMode = WrapMode.Once;
		// Set layers for animation, higher layers are overlayed on the lower.
		// e.g. idle (full body) first, talk (mouth) overlayed on idle.
		// TODO discuss layers with other team
		_animation[idle].layer = 1;
		_animation[idle].wrapMode = WrapMode.Loop;
		_animation[talk].layer = 2;
		
		// ensure viseme animations have the right properties
		foreach (AnimationClip clip in _visemeAnimations) {
			if (clip != null) {
				// enable legacy mode for manual animation management.
				clip.legacy = true;
				// add clip to animation component
				_animation.AddClip(clip, clip.name);
				// set visime animation layer
				_animation[clip.name].layer = Viseme_Layer;
				// set visime animation speed
				_animation[clip.name].speed = 1;
				// set viseme animations to play once.
				_animation[clip.name].wrapMode = WrapMode.Once;
			}
		}
	}

	/// <summary>
	/// This function adds an event to the loaded viseme animation specified by
	/// name. It adds an event at the end of the animation, which calls the
	/// visemeFinished function.
	///
	/// If there already is such an event at the end end of the animation, no
	/// changes are made. 
	/// Existing events in the animation are left unmodified.
	/// Returns without changes if specified animation was not found.
	///
	/// This allows a function to be called once an animation finishes.
	///
	/// Warning: Using variants of crossFade for smoothing animation transitions
	/// modifies the end / start frames for the transition, events during these
	/// frames might not be called.
	/// </summary>
	/// <param name="viseme"></param>
	public void addAnimationEvent(string viseme) {
		// find the loaded animation identified by the name
		AnimationClip clip = _animation[viseme].clip;
		// return if no animation was found
		if (clip == null) {
			return;
		}
		// check if animation already has a finished event, return if it does
		int length = clip.events.Length;
		if (length > 0 && (clip.events[length-1].time == _animation[viseme].length
		    || clip.events[length-1].functionName.Equals("visemeFinished"))) {
			return;
		}
		// retrieve events already in animations and copy to larger array
		AnimationEvent[] events = clip.events;
		AnimationEvent[] evts = new AnimationEvent[length+1];
		for (int i=0; i< length; i++) {
			evts[i] = events[i];
		}
		// add new visemeFinished event to array of events copy
		evts[length] = new AnimationEvent {
			time = _animation[viseme].length,
			functionName = "visemeFinished"
		};
		// set extended array to be the new set of AnimationEvents
		clip.events = evts;
	}

	/// <summary>
	/// Plays the numbered viseme animation. Viseme animations have their own
	/// animation layer, when playing a new viseme, previous animations in the
	/// same layer as the new animation are stopped.
	/// </summary>
	/// <param name="visNumber">
	/// Number of viseme Animation to play.
	/// </param>
	public void playViseme(int visNumber) {
		// play the given viseme animation without fading in, stopping previous
		// animations in the same layer beforehand (other visemes)
		_animation.CrossFade(_visemeAnimations[visNumber].name, 0.0f, PlayMode.StopSameLayer);
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
		_visemeList = visList;
		// loop through the set of viseme numbers
		foreach (int visNumber in visList) {
			// TODO api to set transition time, finding the right time to set
			float transitionTime = 0.3f;

			string clipName = _visemeAnimations[visNumber].name;
			//_animation[clipName].enabled = false;
			
			// look up the animation for the specified number, add it to the
			// queue using the set transition time to smooth out animation
			_animation.CrossFadeQueued(
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
		if (_visemeList == null) return;
		// loop through currently playing viseme numbers
		foreach (int visNumber in _visemeList) {
			// find the animation
			AnimationClip clip = _visemeAnimations[visNumber];
			// stop the animation by blending to weight 0 over 0 seconds.
			_animation.Stop(clip.name);
		}
	}
	
	/// <summary>
	/// Stops all animations in the specified animation layer
	/// </summary>
	/// <param name="layer">
	/// Animation layer to stop animations in.
	/// </param>
	private void stopAnimationLayer( int layer ) {
		//_animation.Stop();
		
		foreach ( int visNumber in _visemeList) {
			AnimationClip clip = _visemeAnimations[visNumber];
			if (clip != null && _animation[clip.name].layer == layer) {
				_animation.Blend(clip.name, 0.0f, 0.0f);
			}
		}
		/*
		// TODO find more efficient working version of this...
		foreach ( AnimationState animState in _animation ) {
			Debug.Log("Stopped animation " + animState.name);
			if (animState.layer == layer) {
				_animation.Blend(animState.name, 0.0f, 0.1f);
			}
		}*/
	}

	/// <summary>
	/// Play the loaded animation on the coach.
	/// </summary>
	public void PlayAnimation(){
		_animation.CrossFade (talk, 0.0f, PlayMode.StopAll);
		_animation.Blend(idle);
	}

	/// <summary>
	/// Stop the loaded animation on the coach.
	/// </summary>
	public void StopAnimation(){
		_animation.CrossFade (idle, 0.0f, PlayMode.StopAll);
	}

	public void foxRun()
	{
		Destroy(new_coach);
		_coachNumber = 1;
		load_coach();
		// make a list of 5 viseme animations
		List<int> fox = new List<int> {40, 9, 0, 49, 24, 2, 49, 0, 46, 26, 8, 32, 0, 37, 6, 49, 41, 
			0, 35, 25, 9, 31, 45, 41, 0, 11, 38, 21, 0, 40, 9, 0, 27, 3, 42, 1, 0, 35, 6, 50,0};
		// play the list of animations sequentially
		//TextManager.instance.startSpeach("The quick brown fox jumps over the lazy dog");
			
		playVisemeList(fox);
	}

	/// <summary>
	/// This function is called for every frame rendered in Unity. It is
	/// currently used to check for activity and start a screensaver if a
	/// set amount of time passes without activity.
	/// </summary>
	void Update(){

		if (Input.GetKeyDown("space"))
		{
			//Destroy(new_coach);
			//_coachNumber = 1;
			//load_coach();
			// make a list of 5 viseme animations
			//List<int> fox = new List<int> {40, 9, 0, 49, 24, 2, 49, 0, 46, 26, 8, 32, 0, 37, 6, 49, 41, 
			//	0, 35, 25, 9, 31, 45, 41, 0, 11, 38, 21, 0, 40, 9, 0, 27, 3, 42, 1, 0, 35, 6, 50,0};
			// play the list of animations sequentially
			TextManager.instance.startSpeach("The quick brown fox jumps over the lazy dog");
			
			//playVisemeList(fox);
		}
		
		timeOutTimer += Time.deltaTime;
		// If screen is tapped, reset timer
		if (Input.anyKeyDown
			|| Input.GetAxis("Mouse X") != 0
		    || Input.GetAxis("Mouse Y") != 0) {
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
