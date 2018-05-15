using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class manages all animations.
/// </summary>
public class AnimationsManager : MonoBehaviour {

	[SerializeField]
	private string idle;

	[SerializeField]
	private string talk;

	[SerializeField]
	private string talkmix;

	/// <summary>
	/// The Singleton instance of the class.
	/// </summary>
	private static AnimationsManager _instance;

	/// <summary>
	/// The initiation of the singleton: either returns the instance of it already exists and creates an instantiates
	/// an instance otherwise.
	/// </summary>
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

	/// <summary>
	/// Return the idle status.
	/// </summary>
	/// <returns>The idle status.</returns>
	public string getIdle(){
		return idle;
	}
		
	/// <summary>
	/// Return the talking status.
	/// </summary>
	/// <returns>The talking status.</returns>
	public string getTalk(){
		return talk;
	}

	/// <summary>
	/// Return the talkmix status.
	/// </summary>
	/// <returns>The talkmix status.</returns>
	public string getTalkmix(){
		return talkmix;
	}
}
