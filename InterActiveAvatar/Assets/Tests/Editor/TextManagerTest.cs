﻿using System;
using UnityEngine;
using UnityEditor;
using NUnit.Framework;
using NMock3;
using NMockTests;

public class TextManagerTest
{

    //TextManager tm = GameObject.FindObjectOfType<TextManager>();
    private TextManager tm;

    [Test]
    public void StartSpeechTest()
    {
        MockFactory mockFactory = new MockFactory();

        tm = new TextManager();
        Assert.IsFalse(tm.getIsSpeaking());
        tm.startSpeach("This is a string that is used for testing the start speech function");
        Assert.IsTrue(tm.getIsSpeaking());
    }

}
