using System.Collections.Generic;
using SimpleJSON;

public class JSONUtil
{
    private JSONUtil() {}

    public static List<string> arrayToList(JSONArray array)
    {
        List<string> list = new List<string>();

        for (int i = 0; i < array.Count; i++)
        {
            list.Add(array[i]);
        }

        return list;
    }
}