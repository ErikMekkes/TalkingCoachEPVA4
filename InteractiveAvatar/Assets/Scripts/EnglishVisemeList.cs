﻿using UnityEngine;

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
[System.Serializable]
public class EnglishVisemeList {
    [SerializeField] private AnimationClip Viseme0;
    [SerializeField] private AnimationClip Viseme1;
    [SerializeField] private AnimationClip Viseme2;
    [SerializeField] private AnimationClip Viseme3;
    [SerializeField] private AnimationClip Viseme4;
    [SerializeField] private AnimationClip Viseme5;
    [SerializeField] private AnimationClip Viseme6;
    [SerializeField] private AnimationClip Viseme7;
    [SerializeField] private AnimationClip Viseme8;
    [SerializeField] private AnimationClip Viseme9;
    
    [SerializeField] private AnimationClip Viseme10;
    [SerializeField] private AnimationClip Viseme11;
    [SerializeField] private AnimationClip Viseme12;
    [SerializeField] private AnimationClip Viseme13;
    [SerializeField] private AnimationClip Viseme14;
    [SerializeField] private AnimationClip Viseme15;
    [SerializeField] private AnimationClip Viseme16;
    [SerializeField] private AnimationClip Viseme17;
    [SerializeField] private AnimationClip Viseme18;
    [SerializeField] private AnimationClip Viseme19;
    
    [SerializeField] private AnimationClip Viseme20;
    [SerializeField] private AnimationClip Viseme21;
    [SerializeField] private AnimationClip Viseme22;
    [SerializeField] private AnimationClip Viseme23;
    [SerializeField] private AnimationClip Viseme24;
    [SerializeField] private AnimationClip Viseme25;
    [SerializeField] private AnimationClip Viseme26;
    [SerializeField] private AnimationClip Viseme27;
    [SerializeField] private AnimationClip Viseme28;
    [SerializeField] private AnimationClip Viseme29;
    
    [SerializeField] private AnimationClip Viseme30;
    [SerializeField] private AnimationClip Viseme31;
    [SerializeField] private AnimationClip Viseme32;
    [SerializeField] private AnimationClip Viseme33;
    [SerializeField] private AnimationClip Viseme34;
    [SerializeField] private AnimationClip Viseme35;
    [SerializeField] private AnimationClip Viseme36;
    [SerializeField] private AnimationClip Viseme37;
    [SerializeField] private AnimationClip Viseme38;
    [SerializeField] private AnimationClip Viseme39;
    
    [SerializeField] private AnimationClip Viseme40;
    [SerializeField] private AnimationClip Viseme41;
    [SerializeField] private AnimationClip Viseme42;
    [SerializeField] private AnimationClip Viseme43;
    [SerializeField] private AnimationClip Viseme44;
    [SerializeField] private AnimationClip Viseme45;
    [SerializeField] private AnimationClip Viseme46;
    [SerializeField] private AnimationClip Viseme47;
    [SerializeField] private AnimationClip Viseme48;
    [SerializeField] private AnimationClip Viseme49;
    
    [SerializeField] private AnimationClip Viseme50;
    [SerializeField] private AnimationClip Viseme51;
    [SerializeField] private AnimationClip Viseme52;
    [SerializeField] private AnimationClip Viseme53;
    [SerializeField] private AnimationClip Viseme54;
    [SerializeField] AnimationClip Viseme55;
    
    public AnimationClip[] getVisemes() {
        AnimationClip[] clips = new AnimationClip[56];
        
        clips[0] = Viseme0;
        clips[1] = Viseme1;
        clips[2] = Viseme2;
        clips[3] = Viseme3;
        clips[4] = Viseme4;
        clips[5] = Viseme5;
        clips[6] = Viseme6;
        clips[7] = Viseme7;
        clips[8] = Viseme8;
        clips[9] = Viseme9;
        
        clips[10] = Viseme10;
        clips[11] = Viseme11;
        clips[12] = Viseme12;
        clips[13] = Viseme13;
        clips[14] = Viseme14;
        clips[15] = Viseme15;
        clips[16] = Viseme16;
        clips[17] = Viseme17;
        clips[18] = Viseme18;
        clips[19] = Viseme19;
        
        clips[20] = Viseme20;
        clips[21] = Viseme21;
        clips[22] = Viseme22;
        clips[23] = Viseme23;
        clips[24] = Viseme24;
        clips[25] = Viseme25;
        clips[26] = Viseme26;
        clips[27] = Viseme27;
        clips[28] = Viseme28;
        clips[29] = Viseme29;
        
        clips[30] = Viseme30;
        clips[31] = Viseme31;
        clips[32] = Viseme32;
        clips[33] = Viseme33;
        clips[34] = Viseme34;
        clips[35] = Viseme35;
        clips[36] = Viseme36;
        clips[37] = Viseme37;
        clips[38] = Viseme38;
        clips[39] = Viseme39;
        
        clips[40] = Viseme40;
        clips[41] = Viseme41;
        clips[42] = Viseme42;
        clips[43] = Viseme43;
        clips[44] = Viseme44;
        clips[45] = Viseme45;
        clips[46] = Viseme46;
        clips[47] = Viseme47;
        clips[48] = Viseme48;
        clips[49] = Viseme49;
        
        clips[50] = Viseme50;
        clips[51] = Viseme51;
        clips[52] = Viseme52;
        clips[53] = Viseme53;
        clips[54] = Viseme54;
        clips[55] = Viseme55;
        
        return clips;
    }
}