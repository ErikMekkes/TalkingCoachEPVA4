using UnityEngine;
using NUnit.Framework;
//using NMock;

namespace Tests.Editor
{
	public class ApplicationManagerTest
	{

		//ApplicationManager application = GameObject.FindObjectOfType<ApplicationManager>();
		//private MockFactory _factory = new MockFactory();
		/// <summary>
		/// Default test by Unity.
		/// </summary>
		[Test]
		public void editorTest()
		{
			//Arrange
			var gameObject = new GameObject();

			//Act
			//Try to rename the GameObject
			var newGameObjectName = "My game object";
			gameObject.name = newGameObjectName;

			//Assert
			//The object has a new name
			Assert.AreEqual(newGameObjectName, gameObject.name);
		}

	}
}