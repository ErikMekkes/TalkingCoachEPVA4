using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// This class is a container for a button along with an audioclip.
/// </summary>
public class AudioButton : MonoBehaviour {
	
	/// <summary>
	/// The contained button.
	/// </summary>
	public Button button;
	
	/// <summary>
	/// The contained label.
	/// </summary>
	public Text nameLabel;
	
	/// <summary>
	/// The contained AudioClip.
	/// </summary>
	public AudioClip audioClip;
}
