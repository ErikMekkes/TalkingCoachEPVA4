using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

/// <summary>
/// This class calculates timings for viseme pronounciations.
/// </summary>
[System.Serializable]
public class VisemeTimingCalculator
{
    
    /// <summary>
    /// The specified language's viseme durations.
    /// </summary>
    [SerializeField] private TextAsset Language;

    /// <summary>
    /// The in-memory map of viseme to double (duration).
    /// </summary>
    private IDictionary<string, double> visemeDictionary;
    
    /// <summary>
    /// The Singleton instance of the class.
    /// </summary>
    private static VisemeTimingCalculator instance;
    
    /// <summary>
    /// The initiation of the singleton: either returns the instance of it already exists and creates an instantiates
    /// an instance otherwise.
    /// </summary>
    public static VisemeTimingCalculator getInstance
    {
        get
        {
            if (instance == null)
            {
                instance = new VisemeTimingCalculator();
            }
            return instance;
        }
    }

    /// <summary>
    /// Construct a dictionary based on the input viseme CSV file.
    /// </summary>
    public void constructDictionary(StreamReader reader)
    {
        if (visemeDictionary != null)
        {
            return;
        }

        visemeDictionary = new Dictionary<string, double>();
        
        string line;
        while ((line = reader.ReadLine()) != null)
        {
            var splitLine = line.Split(',');
            Debug.Log(splitLine[0] + " : " + splitLine[1]);
            visemeDictionary[splitLine[0]] = float.Parse(splitLine[1]);
        }
        
        reader.Close();
    }

    /// <summary>
    /// Getter for the dictionary.
    /// </summary>
    /// <returns>The dictionary of visemes mapped to their corresponding lengths.</returns>
    public IDictionary<string, double> getDictionary()
    {
        return visemeDictionary;
    } 

    /// <summary>
    /// Get the duration of a list of visemes.
    /// </summary>
    /// <param name="visemes">A list of strings, representing visemes.</param>
    /// <returns>A list of doubles, starting with 0, ascendingly ordered.</returns>
    public List<double> getVisemeDurations(IEnumerable<string> visemes)
    {
        if (visemeDictionary == null)
        {
            constructDictionary(new StreamReader(new MemoryStream(Encoding.UTF8.GetBytes(Language.text))));
            return getVisemeDurations(visemes);
        }
        var durationList = new List<double>();
        var currentDuration = 0.0d;
        foreach (var t in visemes)
        {
            durationList.Add(currentDuration);
            currentDuration += visemeDictionary[t];
        }

        return durationList;
    }

}