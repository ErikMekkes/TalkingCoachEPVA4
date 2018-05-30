using UnityEngine;
using UnityEditor;
using NUnit.Framework;
using NMock;

public class TalkingCoachAPITest {

    // The TalkingCoachAPI makes a lot of calls to TextManager and ApplicationManager.
    
    private MockFactory _factory;
    Mock<ITextManager> textMock;
    Mock<ApplicationManager> appMock;

    [SetUp]
    public void SetUp()
    {
        _factory = new MockFactory();

        textMock = _factory.CreateMock<ITextManager>();
        appMock = _factory.CreateMock<ApplicationManager>();

        // Somehow inject the dependency
        //TalkingCoachAPI tcapi = new TalkingCoachAPI(textMock, appMock);
    }

    /// <summary>
    /// Default test by Unity.
    /// </summary>
    [Test]
	public void EditorTest() {
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
    /// Test using mocks.
    /// </summary>
    [Test]
    public void TestGetVoices()
    {
        Expect.On(textMock).One.GetProperty(t => t.getVoices.Will(Return.Value(""));
        Expect.On(textMock).One.Method( t => t.getVoices());

        //Expect.On(talkingCoach).One.GetProperty(TextManager.instance => TextManager.Instance.getVoices());
        // TODO: Test the function calls of the mocks inside TalkingCoachAPI.
    }

    [TearDown]
    public void TearDown()
    {
        _factory.VerifyAllExpectationsHaveBeenMet();
    }
}
