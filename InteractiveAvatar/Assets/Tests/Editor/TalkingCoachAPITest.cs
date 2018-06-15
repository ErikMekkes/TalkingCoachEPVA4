using NUnit.Framework;
using UnityEngine;

namespace Tests.Editor
{
    public class TalkingCoachAPITest
    {
        // The TalkingCoachAPI makes a lot of calls to TextManager and ApplicationManager.

        //TalkingCoachAPI talkingCoach = GameObject.FindObjectOfType<TalkingCoachAPI>();
        //private MockFactory _factory = new MockFactory();

	    /// <summary>
	    ///     Default test by Unity.
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

	    /// <summary>
	    ///     Test using mocks.
	    /// </summary>
	    [Test]
        public void testWithMocking()
        {
            //var textMock = _factory.CreateMock<TextManager>();
            //var appMock = _factory.CreateMock<ApplicationManager>();

            // TODO: Test the function calls of the mocks inside TalkingCoachAPI.
        }
    }
}