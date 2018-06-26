using System.Runtime.Serialization;
using NUnit.Framework;
using UnityEngine;
using Assert = UnityEngine.Assertions.Assert;

namespace Tests.Editor
{
    public class SpeechAnimationManagerTest
    {
        [Test]
        public void startSpeechAnimationTest() {
            const int testValue = 2;
            var sam = new SpeechAnimationManager();
            Assert.IsFalse(sam.getIsSpeaking());
            Assert.AreEqual(0, sam.getCharIndex());
            sam.startSpeechAnimation(testValue);
            Assert.IsTrue(sam.getIsSpeaking());
            Assert.AreEqual(testValue, sam.getCharIndex());
        }

        [Test]
        public void stopSpeechAnimationTest() {
            const int testValue = 2;
            var sam = new SpeechAnimationManager();
            sam.startSpeechAnimation(0);
            sam.stopSpeechAnimation(testValue);
            Assert.IsFalse(sam.getIsSpeaking());
            Assert.AreEqual(testValue, sam.getCharIndex());
        }

        [Test]
        public void testOnboundary() {
            const int testValue = -2;
            var sam = new SpeechAnimationManager();
            Assert.AreEqual(0, sam.getCharIndex());
            sam.onBoundary(testValue);
            Assert.AreEqual(testValue, sam.getCharIndex());
        }

        [Test]
        public void pauseSpeechAnimationTest() {
            const int testValue = 5;
            var sam = new SpeechAnimationManager();
            sam.startSpeechAnimation(0);
            sam.pauseSpeechAnimation(testValue);
            Assert.IsFalse(sam.getIsSpeaking());
            Assert.AreEqual(testValue, sam.getCharIndex());
        }

        [Test]
        public void resumeSpeechAnimationTest() {
            const int testValue = 0;
            var sam = new SpeechAnimationManager();
            sam.pauseSpeechAnimation(4);
            sam.resumeSpeechAnimation(testValue);
            Assert.IsTrue(sam.getIsSpeaking());
            Assert.AreEqual(testValue, sam.getCharIndex());
        }

        [Test]
        public void frameUpdateNotSpeakingTest() {
            var sam = new SpeechAnimationManager();
            const float testValue = 1.5f;
            Assert.AreEqual(0, sam.getElapsedTime());
            sam.frameUpdate(testValue);
            Assert.AreEqual(testValue, sam.getElapsedTime());
        }

        [Test]
        public void frameUpdateSpeakingTest() {
            var sam = new SpeechAnimationManager();
            sam.startSpeechAnimation(0);
            sam.frameUpdate(1.5f);
            Assert.AreEqual(0, sam.getElapsedTime());
        }
    }
}