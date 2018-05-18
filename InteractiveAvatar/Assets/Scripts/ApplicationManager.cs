﻿using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ApplicationManager : MonoBehaviour {
	// public attributes: Unity editor interface fields for ApplicationManager
	public List<GameObject> coach_prefabs;
	public GameObject coach_holder;
	public GameObject background_holder;
	// Main Camera
	public Camera avatarCamera;
	// Time Out Setting in Seconds
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
	
	// background texture and renderer
	Sprite[] backgroundTexture;
	SpriteRenderer backgroundSprite;

	// Only thing required to find an animation in Untiy is a name
	// TODO find out what happens with same names
	private string idle;
	private string talk;
	// list of  of all included viseme animations
	private string[] _visemeNames;

	private List<int> _visemeList;
	private int _visemeListIndex = -1;
	private int _visemeNumber = -1;
	private int _visemeListCount = -1;

	// initial coach avatar selected from prefabs.
	private int _coachNumber = 0;
	// initial background selected.
	private int _backgroundNumber = 0;

	// Singleton Instance
	private static ApplicationManager _instance;

	// Singleton Initiation
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
		on_load();
	}
	
	/// <summary>
	/// Start is called on the frame when a script is enabled just before any
	/// of the Update methods are called the first time. It runs after Awake().
	/// </summary>
	void Start () {
		// ensure screensaves camera is disabled on start (see Update())
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
		_animation.Stop();
		// make a list of 5 viseme animations
		List<int> visList = new List<int> {0, 1, 2, 3, 4, 5};
		// play the list of animations sequentially
		playVisemeList(visList);
	}

	private void on_load(){
		backgroundSprite =  background_holder.GetComponent<SpriteRenderer>();
		load_background();
		load_coach();
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
	private void load_coach() {
		new_coach = GameObject.Instantiate(coach_prefabs[_coachNumber]);
		new_coach.transform.parent = coach_holder.transform;

		new_coach.transform.localPosition = new Vector3(0, 0, 0);
		new_coach.transform.localRotation = Quaternion.identity;
		new_coach.transform.localScale = new Vector3(1, 1, 1);

		loadAnimations();
	}

	public void changeBackground(){
		_backgroundNumber = (_backgroundNumber + 1) % backgroundTexture.Length;
		backgroundSprite.sprite = backgroundTexture[_backgroundNumber];
	}

	public void changeCoach(){
		Vector3 oldCoachPosition = new_coach.transform.position;
		_coachNumber = (_coachNumber + 1) % coach_prefabs.Count;
		Destroy(new_coach);
		load_coach();
		new_coach.transform.position = oldCoachPosition;
	}


	private void loadAnimations() {
		// Get animation manager script attached to current avatar GameObject
		_animationsManager = new_coach.GetComponent<AnimationsManager>();
		// get names of viseme animations
		_visemeNames = _animationsManager.getEnglishVisemes56();
		// get names of idle, talk and talkmix animations
		idle = _animationsManager.getIdle();
		talk = _animationsManager.getTalk();
		// Get Unity Animation component attached to current avatar GameObject
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
		foreach (string viseme in _visemeNames) {
			if (!string.IsNullOrEmpty(viseme)) {
				// set visime animation layer
				_animation[viseme].layer = 2;
				// set visime animation speed
				_animation[viseme].speed = 1;
				// set viseme animations to play once.
				_animation[viseme].wrapMode = WrapMode.Once;
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
		AnimationEvent[] events = 
			AnimationUtility.GetAnimationEvents(clip);
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
		AnimationUtility.SetAnimationEvents(clip, evts);
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
		_animation.CrossFade(_visemeNames[visNumber], 0.0f, PlayMode.StopSameLayer);
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
		// TODO stop previously playing animations in viseme layer
		
		// save list of visemes to play
		_visemeList = visList;
		// loop through the set of viseme numbers
		foreach (int visNumber in visList) {
			// TODO api to set transition time, finding the right time to set
			float transitionTime = 0.3f;
			
			// look up the animation for the specified number, add it to the
			// queue using the set transition time to smooth out animation
			_animation.CrossFadeQueued(
				_visemeNames[visNumber],
				transitionTime,
				QueueMode.CompleteOthers);
		}
	}

	public void PlayAnimation(){
		_animation.CrossFade (talk, 0.0f, PlayMode.StopAll);
		_animation.Blend(idle);
	}

	public void StopAnimation(){
		_animation.CrossFade (idle, 0.0f, PlayMode.StopAll);
	}

	void Update(){
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
