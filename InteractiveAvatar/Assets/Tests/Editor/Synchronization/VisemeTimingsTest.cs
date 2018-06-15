using System.IO;
using NUnit.Framework;
using Assert = UnityEngine.Assertions.Assert;

namespace Tests.Editor
{
    public class VisemeTimingsTest
    {
        [Test]
        public void readDictionaryTest()
        {
            var vtc = VisemeTimings.getInstance;
            vtc.constructDictionary(new StreamReader("Assets/Resources/Languages/eng-viseme.txt"));
            Assert.IsTrue(vtc.getDictionary()["AE"] > 0.260d && vtc.getDictionary()["AE"] < 0.280d);
        }
    }
}