using System.Collections.Generic;
using Models;
using NUnit.Framework;

namespace Tests.Editor
{
    public class PhonemeTest
    {
        [Test]
        public void phonemeSingleCodeTest()
        {
            var phoneme = "AA";
            var converted = Phoneme.getPhonemeFromCode(phoneme);
            Assert.AreEqual("AA", converted.getPhonemeCode().getName());
        }

        [Test]
        public void phoneListCodeTest()
        {
            var phonemeList = new List<string> {"AA", "AE", "AO"};
            var converted = Phoneme.getPhonemeFromCode(phonemeList);
            Assert.AreEqual("AE", converted[1].getPhonemeCode().getName());
        }

        [Test]
        public void toVisemeTest()
        {
            var phoneme = "AA";
            var viseme = Phoneme.getPhonemeFromCode(phoneme).toViseme();
            Assert.AreEqual("AA", viseme.getVisemeCode().getName());
        }

        [Test]
        public void toVisemesTest()
        {
            var phonemes = new List<string> { "AA", "AE", "AO" };
            var viseme = Phoneme.toVisemes(Phoneme.getPhonemeFromCode(phonemes));
            Assert.AreEqual("AE", viseme[1].getVisemeCode().getName());
        }
    }
}