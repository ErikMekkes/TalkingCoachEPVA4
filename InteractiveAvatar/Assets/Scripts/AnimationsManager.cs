using UnityEngine;

// This class specifies certain components that are specific for the Unity
// Interface. As such, we only want these components to be executed when run
// with the Unity Interface. This is achieved with this preprocessor directive.
#if UNITY_EDITOR
using UnityEditor;

/// <summary>
/// This class represents an info interface element for the Unity Editor.
///
/// When added to the interface as component it draws a box with a small info
/// icon and the specified text. 
/// </summary>
public class InterfaceInfo : PropertyAttribute {
	// info text
	public readonly string info;
    
	// constructor
	public InterfaceInfo(string infoText) {
		info = infoText;
	}
}
/// <summary>
/// This class specifies custom draw instructions for the Unity Interface, for
/// objects of type InterfaceInfo
///
/// It instructs the Unity Interface to draw a HelpBox object with type 'info'
/// and the text specified within the InterfaceInfo Object.
/// The positioning is relative to the previous elements added by the script
/// that adds the InterfaceInfo Object.
/// </summary>
[CustomPropertyDrawer(typeof(InterfaceInfo))]
public class InterfaceInfoDrawer : PropertyDrawer {
	// get info text from object
	private InterfaceInfo interfaceInfo { get { return (InterfaceInfo)attribute; } }
    
	// specify custom draw instructions
	public override void OnGUI(Rect position, SerializedProperty prop, GUIContent label) {
		// draw help box of type info using info text
		EditorGUI.HelpBox(position, interfaceInfo.info, MessageType.Info);
	}
	
	// specify custom height for this element
	public override float GetPropertyHeight(SerializedProperty property,
		GUIContent label) {
		// TODO : less magic number
		return 60;
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
	
	// interface only component	
//	#if UNITY_EDITOR
//		[InterfaceInfo("With the file below, you can specify which lengths of visemes" +
//					   "should be used. The default is set to the English visemes lengths.")]
//	#endif
	
	[SerializeField] private VisemeTimingCalculator _visemeDurations;

	// interface only component	
	#if UNITY_EDITOR
	[InterfaceInfo("With the Viseme List below you can specify alternative" +
	               "animations to be used for visemes in the English" +
	               " language.\n" +
	               "The included Default Viseme viseme animation is used if no " +
	               "custom viseme animation is specified here\n\n" +
	               "Check the documentation at ... for more information on " +
	               "which motion each viseme number represents.")]
	public string help;
	#endif
	
	[SerializeField] private EnglishVisemeList visemesEnglish;

	// The Singleton instance of the class.
	private static AnimationsManager instance;

/// <summary>
	/// The initiation of the singleton: either returns the instance if it
	/// already exists or instantiates and returns an instance otherwise.
	/// </summary>
	public static AnimationsManager amInstance
	{
		get
		{
			if (instance == null)
			{
				instance = GameObject.FindObjectOfType<AnimationsManager>();
				DontDestroyOnLoad(instance.gameObject);
			}
			return instance;
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
	/// Get viseme durations.
	/// </summary>
	/// <returns>The VisemeTimingCalculator object to calculate viseme list lengths.</returns>
	public VisemeTimingCalculator getVisemeTimingCalculator()
	{
		return _visemeDurations;
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
		return visemesEnglish.getVisemes();
	}
}
