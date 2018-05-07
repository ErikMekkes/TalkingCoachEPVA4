using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationsManager : MonoBehaviour {

	[SerializeField]
	private string idle;

	[SerializeField]
	private string talk;

	[SerializeField]
	private string talkmix;

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
}
