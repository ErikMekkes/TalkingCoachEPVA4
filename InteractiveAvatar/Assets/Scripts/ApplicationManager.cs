﻿using System.Collections.Generic;
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
	private GameObject newCoach;

	// Unity Animation component and manager script instance
	private new Animation animation;
	
	// background texture and sprite renderer
	Sprite[] backgroundTexture;
	SpriteRenderer backgroundSprite;
	
	
	// initial coach avatar selected from prefabs.
	private int coachNumber = 0;
	// initial background selected.
	private int backgroundNumber = 0;

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
				instance = GameObject.FindObjectOfType<ApplicationManager>();
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
		// load hostname for text to phoneme API calls, only available from web.
		#if !UNITY_EDITOR && UNITY_WEBGL
        TextManager.tmInstance.loadPhonemeServerHost();
		#endif
	}
	
	/// <summary>
	/// Start is called on the frame when a script is enabled just before any
	/// of the Update methods are called the first time. It runs after Awake().
	/// </summary>
	void Start() {
		// load the available camera's from Unity
		cams = Camera.allCameras;
		// ensure screensaver camera is disabled on start (see also : Update())
		disableScreenSaver();
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
	private void load_background() {
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
		newCoach = GameObject.Instantiate(coach_prefabs[coachNumber]);
		// add the new coach object to the Unity parent container (CoachHolder)
		newCoach.transform.parent = coach_holder.transform;
		// set default coach position
		newCoach.transform.localPosition = new Vector3(0, 0, 0);
		newCoach.transform.localRotation = Quaternion.identity;
		newCoach.transform.localScale = new Vector3(1, 1, 1);
		// load viseme animations for new coach object
		SpeechAnimationManager.instance.loadCoach(newCoach);
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
	/// This function is called for every frame rendered in Unity. It is
	/// currently used to check for activity and start a screensaver if a
	/// set amount of time passes without activity.
	/// </summary>
	void Update(){
		// call the frameUpdate function of SpeechAnimationManager
		SpeechAnimationManager.instance.frameUpdate(Time.deltaTime);
		// updates the screensaver with elapsed time
		updateScreenSaver(Time.deltaTime);
	}

	/// <summary>
	/// Updates the screensaver with elapsed time. If the elapsed time without
	/// user interaction is greater than the screensaver timeout the screensaver
	/// is enabled. If user interaction occurs and the screensaver was active,
	/// the timour for the screensaver is reset and the screensaver is disabled. 
	/// </summary>
	/// <param name="deltaTime"></param>
	void updateScreenSaver(float deltaTime) {
		// add elapsed time to timer
		timeOutTimer += deltaTime;
		
		// If screen is tapped, reset timer and stop screensaver
		if (inputActive()) {
			timeOutTimer = 0.0f;
			disableScreenSaver();
		}
		// If timer reaches zero, start screensaver
		if (timeOutTimer > timeOut){
			enableScreenSaver();
		}	
	}

	/// <summary>
	/// Returns whether or not the user is interacting with the application.
	/// </summary>
	/// <returns>
	/// True if user is interacting with application. False otherwise.
	/// </returns>
	bool inputActive() {
		// active if key pressed, or mouse hovering over application window.
		return Input.anyKeyDown
		              || Input.GetAxis("Mouse X") != 0
		              || Input.GetAxis("Mouse Y") != 0;
	}

	/// <summary>
	/// Deactivates the Screensaver by switching to main camera.
	/// </summary>
	void disableScreenSaver() {
		foreach(Camera cam in cams) {
			if (cam.gameObject.name == "InactiveCamera") {
				cam.enabled = false;
			} else {
				cam.enabled = true;
			}
		}
	}

	/// <summary>
	/// Activates the Screensaver by switching to inactive camera.
	/// </summary>
	void enableScreenSaver() {
		foreach(Camera cam in cams) {
			if (cam.gameObject.name == "InactiveCamera") {
				cam.enabled = true;
			} else {
				cam.enabled = false;
			}
		}
	}
}
