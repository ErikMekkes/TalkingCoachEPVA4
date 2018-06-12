using NUnit.Framework;
using SimpleJSON;

namespace Tests.Editor.JSON
{
    public class JSONUtilTest
    {
        [Test]
        public void arrayToListTest()
        {
            var jsonArray = new JSONArray();
            jsonArray.Add(0);
            jsonArray.Add(1);
            jsonArray.Add(2);

            var jsonList = JSONUtil.arrayToList(jsonArray);
            
            Assert.AreEqual(jsonArray[0].ToString(), jsonList[0]);
            Assert.AreEqual(jsonArray[1].ToString(), jsonList[1]);
            Assert.AreEqual(jsonArray[2].ToString(), jsonList[2]);
        }
    }
}