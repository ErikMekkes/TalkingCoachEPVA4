using System.Collections.Generic;
using System.IO;
using NUnit.Framework;

public class VisemeTestingCalculatorTest
{
    [Test]
    public void readDictionaryTest()
    {
        VisemeTimingCalculator vtc = VisemeTimingCalculator.getInstance;
        vtc.constructDictionary(new StreamReader("Assets/Resources/Languages/eng-viseme.txt"));
        Assert.IsTrue(vtc.getDictionary()["AE"] > 0.260d && vtc.getDictionary()["AE"] < 0.280d );
    }

    [Test]
    public void getVisemeDurationsTest()
    {
        VisemeTimingCalculator vtc = VisemeTimingCalculator.getInstance;
        vtc.constructDictionary(new StreamReader("Assets/Resources/Languages/eng-viseme.txt"));
        var visemeList = vtc.getVisemeDurations(new List<string>() {"AE", "AO", "AY"});
        Assert.IsTrue(visemeList[0] == 0 && visemeList[1] > 0 && visemeList[2] > visemeList[1]);
    }
}