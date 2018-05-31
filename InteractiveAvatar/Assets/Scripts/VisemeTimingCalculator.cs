using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

[System.Serializable]
public class VisemeTimingCalculator
{
    [SerializeField] private string _lang;

    private static IDictionary<string, double> _dictionary;

    [MenuItem("Tools/Read file")]
    static void constructDictionary()
    {
        string path = "Assets/Resources/Languages/eng-viseme.txt";

        //Read the text from directly from the test.txt file
        StreamReader reader = new StreamReader(path);

        string line;
        while ((line = reader.ReadLine()) != null)
        {
            string[] splitLine = line.Split(',');
            _dictionary[splitLine[0]] = float.Parse(splitLine[1]);
        }
        
        Debug.Log(reader.ReadToEnd());
        reader.Close();
    }

}