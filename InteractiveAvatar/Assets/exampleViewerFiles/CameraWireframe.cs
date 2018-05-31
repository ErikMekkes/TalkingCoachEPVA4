using UnityEngine;

namespace exampleViewerFiles
{
		public class CameraWireframe : MonoBehaviour
		{
				public bool doEffect;
    
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
