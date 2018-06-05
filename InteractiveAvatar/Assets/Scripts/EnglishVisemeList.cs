using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class represents a list for 56 English Visemes, to be included within a
/// Unity Interface Component.
///
/// The code is ugly, and it's not very flexible.
///
/// Some other options were attempted, such as arrays and lists, while they work
/// and look better in the code, they don't appear as nice as this class within
/// the Editor.
///
/// There should be an approach to modify the way Unity renders a specific array
/// or list, but there's very little documentation available. Probably requires
/// wrapping within another object, it doesn't seem possibe to modify array or
/// list rendering directly. 
/// </summary>
///
#pragma warning disable 0649, 
[System.Serializable]
public class EnglishVisemeList {
    [SerializeField] private AnimationClip viseme0;
    [SerializeField] private AnimationClip viseme1;
    [SerializeField] private AnimationClip viseme2;
    [SerializeField] private AnimationClip viseme3;
    [SerializeField] private AnimationClip viseme4;
    [SerializeField] private AnimationClip viseme5;
    [SerializeField] private AnimationClip viseme6;
    [SerializeField] private AnimationClip viseme7;
    [SerializeField] private AnimationClip viseme8;
    [SerializeField] private AnimationClip viseme9;
    
    [SerializeField] private AnimationClip viseme10;
    [SerializeField] private AnimationClip viseme11;
    [SerializeField] private AnimationClip viseme12;
    [SerializeField] private AnimationClip viseme13;
    [SerializeField] private AnimationClip viseme14;
    [SerializeField] private AnimationClip viseme15;
    [SerializeField] private AnimationClip viseme16;
    [SerializeField] private AnimationClip viseme17;
    [SerializeField] private AnimationClip viseme18;
    [SerializeField] private AnimationClip viseme19;
    
    [SerializeField] private AnimationClip viseme20;
    [SerializeField] private AnimationClip viseme21;
    [SerializeField] private AnimationClip viseme22;
    [SerializeField] private AnimationClip viseme23;
    [SerializeField] private AnimationClip viseme24;
    [SerializeField] private AnimationClip viseme25;
    [SerializeField] private AnimationClip viseme26;
    [SerializeField] private AnimationClip viseme27;
    [SerializeField] private AnimationClip viseme28;
    [SerializeField] private AnimationClip viseme29;
    
    [SerializeField] private AnimationClip viseme30;
    [SerializeField] private AnimationClip viseme31;
    [SerializeField] private AnimationClip viseme32;
    [SerializeField] private AnimationClip viseme33;
    [SerializeField] private AnimationClip viseme34;
    [SerializeField] private AnimationClip viseme35;
    [SerializeField] private AnimationClip viseme36;
    [SerializeField] private AnimationClip viseme37;
    [SerializeField] private AnimationClip viseme38;
    [SerializeField] private AnimationClip viseme39;

    [SerializeField] private AnimationClip viseme40;
    
    /**
    [SerializeField] private AnimationClip viseme40;
    [SerializeField] private AnimationClip viseme41;
    [SerializeField] private AnimationClip viseme42;
    [SerializeField] private AnimationClip viseme43;
    [SerializeField] private AnimationClip viseme44;
    [SerializeField] private AnimationClip viseme45;
    [SerializeField] private AnimationClip viseme46;
    [SerializeField] private AnimationClip viseme47;
    [SerializeField] private AnimationClip viseme48;
    [SerializeField] private AnimationClip viseme49;
    
    [SerializeField] private AnimationClip viseme50;
    [SerializeField] private AnimationClip viseme51;
    [SerializeField] private AnimationClip viseme52;
    [SerializeField] private AnimationClip viseme53;
    [SerializeField] private AnimationClip viseme54;
    [SerializeField] private AnimationClip viseme55;
    **/

    private AnimationClip[] clips;
    private Dictionary<string, int> indexMapping;
    
    #pragma warning restore 0649
    
    public AnimationClip[] getVisemes() {
        if (clips == null)
        {
            clips = new AnimationClip[56];
        }
        
        clips[0] = viseme0;
        clips[1] = viseme1;
        clips[2] = viseme2;
        clips[3] = viseme3;
        clips[4] = viseme4;
        clips[5] = viseme5;
        clips[6] = viseme6;
        clips[7] = viseme7;
        clips[8] = viseme8;
        clips[9] = viseme9;
        
        clips[10] = viseme10;
        clips[11] = viseme11;
        clips[12] = viseme12;
        clips[13] = viseme13;
        clips[14] = viseme14;
        clips[15] = viseme15;
        clips[16] = viseme16;
        clips[17] = viseme17;
        clips[18] = viseme18;
        clips[19] = viseme19;
        
        clips[20] = viseme20;
        clips[21] = viseme21;
        clips[22] = viseme22;
        clips[23] = viseme23;
        clips[24] = viseme24;
        clips[25] = viseme25;
        clips[26] = viseme26;
        clips[27] = viseme27;
        clips[28] = viseme28;
        clips[29] = viseme29;
        
        clips[30] = viseme30;
        clips[31] = viseme31;
        clips[32] = viseme32;
        clips[33] = viseme33;
        clips[34] = viseme34;
        clips[35] = viseme35;
        clips[36] = viseme36;
        clips[37] = viseme37;
        clips[38] = viseme38;
        clips[39] = viseme39;
        
        clips[40] = viseme40;
        
        /**
        clips[40] = viseme40;
        clips[41] = viseme41;
        clips[42] = viseme42;
        clips[43] = viseme43;
        clips[44] = viseme44;
        clips[45] = viseme45;
        clips[46] = viseme46;
        clips[47] = viseme47;
        clips[48] = viseme48;
        clips[49] = viseme49;
  
        clips[50] = viseme50;
        clips[51] = viseme51;
        clips[52] = viseme52;
        clips[53] = viseme53;
        clips[54] = viseme54;
        clips[55] = viseme55;
        **/
        
        return clips;
    }

    /// <summary>
    /// Construct the mapping from visemes to indices.
    /// </summary>
    /// <returns>A dictionary, which maps visemes (string) to their corresponding indices (int). </returns>
    public Dictionary<string, int> getVisemeIndexMapping()
    {
        if (clips == null)
        {
            getVisemes();
        }

        if (indexMapping == null)
        {
            indexMapping = new Dictionary<string, int>();
        }

        for (int i = 0; i < clips.Length; i++)
        {
            if (clips[i] == null)
            {
                continue;
            }

            indexMapping[clips[i].name] = i;
        }

        return indexMapping;
    }
}