using System.IO;
using Models;
using NUnit.Framework;

namespace Tests.Editor
{
    public class VisemeTest
    {
        [Test]
        public void getVisemeCodeTest()
        {
            var phoneme = "AA";
            var viseme = Phoneme.getPhonemeFromCode(phoneme).toViseme();
            Assert.AreEqual("AA", viseme.getVisemeCode().getName());
        }

        [Test]
        public void getDurationTest()
        {
            var phoneme = "AA";
            var viseme = Phoneme.getPhonemeFromCode(phoneme).toViseme();

            // VisemeTimings instance has been created here locally since the actual used one in the code is defined through the Unity UI.
            var vtc = VisemeTimings.getInstance;
            vtc.constructDictionary(new StreamReader("Assets/Resources/Languages/eng-viseme.txt"));
            Assert.IsTrue(vtc.getDictionary()[viseme.getVisemeCode().getName()] > 0.300 &&
                          vtc.getDictionary()[viseme.getVisemeCode().getName()] < 0.320);
        }
    }
}