using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;


// for drawing info window
public class InterfaceInfo : PropertyAttribute {
	public readonly string info;
     
	public InterfaceInfo(string infoText) {
		info = infoText;
	}
}
// for drawing info window
[CustomPropertyDrawer(typeof(InterfaceInfo))]
public class InterfaceInfoDrawer : PropertyDrawer {
	private InterfaceInfo interfaceInfo { get { return (InterfaceInfo)attribute; } }
    
	public override void OnGUI(Rect position, SerializedProperty prop, GUIContent label) {
		EditorGUI.HelpBox(position, interfaceInfo.info, MessageType.Info);
	}
	
	public override float GetPropertyHeight(SerializedProperty property,
		GUIContent label) {
		return 30;
	}
}
#endif


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

	// idle animation name
	[SerializeField] private string idle;

	// talk animation name
	[SerializeField] private string talk;

	// talkmix animation name
	[SerializeField] private string talkmix;

	
#if UNITY_EDITOR 
	[InterfaceInfo("With the Viseme List below you can specify which" +
	               "animation should be used for which viseme in the English" +
	               " language.\n" +
	               "Check the documentation at ... for more information on " +
	               "which motion each viseme number represents.")]
	public string Help;
#endif

	// list of viseme animations, it looks really nice in the interface,
	// but the behind the scenes in VisemeList.cs is horrid.
	[SerializeField] private VisemeList VisemesEnglish;


	// The Singleton instance of the class.
	private static AnimationsManager _instance;

/// <summary>
	/// The initiation of the singleton: either returns the instance if it
	/// already exists or instantiates and returns an instance otherwise.
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
	/// Return the name of the idle animation.
	/// </summary>
	/// <returns>The idle animation's name.</returns>
	public string getIdle(){
		return idle;
	}

	/// <summary>
	/// Return the name of the talk animation.
	/// </summary>
	/// <returns>The talk animation's name.</returns>
	public string getTalk(){
		return talk;
	}

	/// <summary>
	/// Return the name of the talkmix animation.
	/// </summary>
	/// <returns>The talkmix animation's name.</returns>
	public string getTalkmix(){
		return talkmix;
	}

	/// <summary>
	/// Returns an array of viseme animations. These are the animations as
	/// included with the selected model in Unity. Null indicates the specific
	/// viseme has no included animation.
	///
	/// See //TODO viseme documentation for details.
	/// </summary>
	/// <returns>
	/// Array of 56 viseme animation names for English speech. starting with
	/// The SI (silent) viseme as entry 0.
	/// </returns>
	public AnimationClip[] getEnglishVisemes() {
		return VisemesEnglish.GetVisemes();
	}
}
