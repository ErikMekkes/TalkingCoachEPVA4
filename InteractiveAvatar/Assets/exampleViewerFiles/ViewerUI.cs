using UnityEngine;

namespace exampleViewerFiles
{
	public class ViewerUI : MonoBehaviour
	{
		public AvatarController edAvatarController;
		public CameraWireframe edCamWireframe;

		private bool showWireframe = false;
		private bool playingAnimation = true;

		void OnGUI ()
		{
			GUILayout.BeginVertical ();
			{
				if (GUILayout.Button ("Toggle Wireframe"))
					edCamWireframe.doEffect = showWireframe = !showWireframe;
    
				if (GUILayout.Button ("Toggle Pause Play Animation")) {
					playingAnimation = !playingAnimation;
					edAvatarController.pauseAnimation (playingAnimation);
				}
    
				GUILayout.BeginHorizontal ();
				{
					if (GUILayout.Button ("next animation")) {
						playingAnimation = true;
						edAvatarController.playNextAnimation ();
					}
					GUILayout.Label (edAvatarController.getAnimName ());
				}
				GUILayout.EndHorizontal ();
				GUILayout.BeginHorizontal ();
				{
					if (GUILayout.Button ("next hair"))
						edAvatarController.changeNextHair ();
					
					GUILayout.Label (edAvatarController.getHairName ());
				}
				GUILayout.EndHorizontal ();
				GUILayout.BeginHorizontal ();
				{
					if (GUILayout.Button ("next glasses item"))
						edAvatarController.changeNextGlasses ();
					GUILayout.Label (edAvatarController.getGlassesName ());
				}
				GUILayout.EndHorizontal ();
				GUILayout.BeginHorizontal ();
				{
					if (GUILayout.Button ("next outfit"))
						edAvatarController.changeNextOutfit ();
					GUILayout.Label (edAvatarController.getOutfitName ());
				}
				GUILayout.EndHorizontal ();
			}
			GUILayout.EndVertical ();
		}
	}
}