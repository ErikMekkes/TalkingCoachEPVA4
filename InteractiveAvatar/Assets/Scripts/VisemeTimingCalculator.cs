using System.Collections.Generic;
using System.IO;
using UnityEngine;

/// <summary>
/// This class calculates timings for viseme pronounciations.
/// </summary>
[System.Serializable]
public class VisemeTimingCalculator
{
    [SerializeField] private TextAsset Language;

    private static IDictionary<string, double> _visemeDictionary;
    
    /// <summary>
    /// The Singleton instance of the class.
    /// </summary>
    private static VisemeTimingCalculator _instance;
    
    /// <summary>
    /// The initiation of the singleton: either returns the instance of it already exists and creates an instantiates
    /// an instance otherwise.
    /// </summary>
    public static VisemeTimingCalculator instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new VisemeTimingCalculator();
                _instance.constructDictionary();
            }
            return _instance;
        }
    }

    /// <summary>
    /// Construct a dictionary based on the input viseme CSV file.
    /// </summary>
    private void constructDictionary()
    {
        if (_visemeDictionary != null)
        {
            return;
        }
        
        // currently hard coded
        const string path = "Assets/Resources/Languages/eng-viseme.txt";

        var reader = new StreamReader(path);
        _visemeDictionary = new Dictionary<string, double>();
        
        string line;
        while ((line = reader.ReadLine()) != null)
        {
            var splitLine = line.Split(',');
            Debug.Log(splitLine[0] + " : " + splitLine[1]);
            _visemeDictionary[splitLine[0]] = float.Parse(splitLine[1]);
        }
        
        reader.Close();
    }

    /// <summary>
    /// Get the duration of a list of visemes.
    /// </summary>
    /// <param name="visemes">A list of strings, representing visemes.</param>
    /// <returns>A list of doubles, starting with 0, ascendingly ordered.</returns>
    public List<double> getVisemeDurations(IEnumerable<string> visemes)
    {
        var durationList = new List<double>();
        var currentDuration = 0.0d;
        foreach (var t in visemes)
        {
            durationList.Add(currentDuration);
            currentDuration += _visemeDictionary[t];
        }

        return durationList;
    }

}