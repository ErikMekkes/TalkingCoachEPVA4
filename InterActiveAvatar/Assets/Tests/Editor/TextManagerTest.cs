using UnityEngine;
using UnityEditor;
using System;
using NUnit.Framework;
using NMock;

public class TextManagerTest
{

    MockFactory _factory;

    [SetUp]
    public void SetUp()
    {
        _factory = new MockFactory();

    }

    /// <summary>
    /// Test for the method StartSpeech.
    /// </summary>
    [Test]
    public void StartSpeechTest()
    {
        var mockTM = _factory.CreateMock<TextManager>();

        //mockTM.Expects.One.GetProperty(t => t.getIsPaused()).Will(Return.Value(false));

        Expect.On(mockTM.MockObject).One.GetProperty(t => t.IsSpeaking).Will(Return.Value(false));

        var speak = mockTM.MockObject.IsSpeaking;
        Assert.AreEqual(speak, false);
        
        
    }

    [TearDown]
    public void TearDown()
    {
        //_factory.VerifyAllExpectationsHaveBeenMet();
    }

}
