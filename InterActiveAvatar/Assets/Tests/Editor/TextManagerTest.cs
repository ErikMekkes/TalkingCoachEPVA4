using UnityEngine;
using UnityEditor;
using System;
using NUnit.Framework;
using NMock;

public class TextManagerTest
{

    TextManager text = GameObject.FindObjectOfType<TextManager>();
    private MockFactory _factory = new MockFactory();

    /// <summary>
    /// Test for the method StartSpeech.
    /// </summary>
    [Test]
    public void StartSpeechTest()
    {
        var mockTM = _factory.CreateMock<TextManager>();
        //mockTM.Expects.One.GetProperty(_=>_.startSpeach).WillReturn("Hello");
        //mockTM.Expects.One.SetPropertyTo(_ => _.startSpeach = "Test");

        //var controller = new Controller(mockTM.MockObject);
        //Assert.AreEqual("Hello Test", controller.PropActions("Test"));
    }

}
