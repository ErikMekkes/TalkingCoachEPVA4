using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This script acts as an interface for the Unity editor. Entries are generated
/// in the Unity editor for the animations defined in this script. The developer
/// working in Unity is free to select, modify or leave out animations for these
/// entries at will. 
///
/// An animation can be set to generate events at certain points within Unity.
/// These events can call public functions from scripts included with Objects in
/// Unity. This script provides such functions for the avatar's animations.
/// 
/// This script should be attached as component to each coach prefab in Unity.
/// </summary>
public class AnimationsManager : MonoBehaviour {
	// For each SerializeField, an entry is created in the Unity interface. The
	// values given for these entries in the Unity interface are accessible by
	// this script.

	private static ApplicationManager _appManager;
	
	// idle animation name
	[SerializeField]
	private string idle;
	// talk animation name
	[SerializeField]
	private string talk;

	// talkmix animation name
	[SerializeField]
	private string talkmix;

	// list of viseme animation names.
	[SerializeField] private string[] _Visemes_English_56 = new string[56];

	private static AnimationsManager _instance;

	//Singleton Initiation
	public static AnimationsManager instance
	{
		get
		{
			if (_instance == null)
			{
				_instance = GameObject.FindObjectOfType<AnimationsManager>();
				DontDestroyOnLoad(_instance.gameObject);
			}
			return _instance;
		}
	}

	public string getIdle(){
		return idle;
	}

	public string getTalk(){
		return talk;
	}

	public string getTalkmix(){
		return talkmix;
	}

	/// <summary>
	/// Returns an array of viseme animation names. These are the names of the
	/// animations as included with the selected model in Unity. Empty string
	/// or null indicates the specific viseme has no included animation.
	///
	/// See //TODO viseme documentation for details.
	/// </summary>
	/// <returns>
	/// Array of 56 viseme animation names for English speech. starting with
	/// The SI (silent) viseme as entry 0.
	/// </returns>
	public string[] getEnglishVisemes56() {
		return _Visemes_English_56;
	}

	/// <summary>
	/// This function is designed to handle the end of a viseme animation.
	/// It calls the nextViseme method, playing the next animation if there are
	/// animations remaining.
	///
	/// TODO investigate animation queueing for Unity
	/// </summary>
	public void visemeFinished() {
		// check if the ApplicationManager instance is loaded.
		if (!_appManager) {
			// load the instance if it isn't loaded.
			loadAppmanager();
		}
		// call the method to play next visime
		_appManager.nextViseme();
	}

	/// <summary>
	/// Loads the instance of the ApplicationManager script while Unity is
	/// running. Returns an error if no instance was found.
	/// </summary>
	private static void loadAppmanager() {
		// find the manager game object
		GameObject manager = GameObject.Find("ApplicationManager");
		if (manager) {
			// find the script attached to the manager game object
			_appManager = manager.GetComponent<ApplicationManager>();
			return;
		}
		// log error if unable to find script
		Debug.LogError("Unable to find instance for ApplicationManager script");
	}
}
