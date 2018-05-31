using UnityEngine;

namespace exampleViewerFiles
{
	public class CameraWireframe : MonoBehaviour
	{
		public bool doEffect = false;
    
		void OnPreRender ()
		{
			if (doEffect) 
				GL.wireframe = true;
		}
        
		void OnPostRender ()
		{
			GL.wireframe = false;
		}
	}
}
