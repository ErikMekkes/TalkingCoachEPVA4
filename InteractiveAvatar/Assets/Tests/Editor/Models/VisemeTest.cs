using Models;
using NUnit.Framework;

namespace Tests.Editor
{
    public class VisemeTest
    {
        public void getVisemeCodeTest()
        {
            var phoneme = "AA";
            var viseme = Phoneme.getPhonemeFromCode(phoneme).toViseme();
            Assert.AreEqual("AA", viseme.getVisemeCode());
        }

        public void getDurationTest()
        {
            var phoneme = "AA";
            var viseme = Phoneme.getPhonemeFromCode(phoneme).toViseme();
            Assert.IsTrue(viseme.getDuration() > 0.300 && viseme.getDuration() < 0.320);
        }
    }
}