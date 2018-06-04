using System.Collections.Generic;
using UnityEngine;

public class SpeechAnimationManager : MonoBehaviour {

    private string[] visemeNames = {
        "Silence", "AA", "AE", "AH", "AO", "AW", "AY", "B", "CH", "D", "DH",
        "EH", "EL", "ER", "EY", "F", "G", "HX", "IH", "IY", "JH",
        "K", "LL", "M", "N", "NX", "OW", "OY", "P", "R", "S",
        "SH", "T", "TH", "UH", "UW", "V", "W", "Y", "Z", "ZH"
    };
    // Avatar model
    private GameObject newCoach;
    
    // Unity Animation component and manager script instance
    private new Animation animation;
    private AnimationsManager animationsManager;
    
    // Array of viseme Animations
    private AnimationClip[] visemeAnimations;
    
    // layer for viseme (speech) animation
    private const int visemeLayer = 2;
	
    // list of viseme numbers that are currently playing
    private List<int> visemeList;
    
    // Singleton Instance
    private static SpeechAnimationManager amInstance;

    /// <summary>
    /// Constructs an instance of ApplicationManager if it doesn't exist and
    /// returns the instance if it already exists.
    /// </summary>
    public static SpeechAnimationManager instance
    {
        get
        {
            if (amInstance == null)
            {
                amInstance = GameObject.FindObjectOfType<SpeechAnimationManager>();
                DontDestroyOnLoad(amInstance.gameObject);
            }
            return amInstance;
        }
    }

    /// <summary>
    /// Plays the specified list of viseme numbers sequentially. Animations are
    /// played once, when an animation ends the next one in the list is played
    /// until there are no remaining visemes in the list.
    /// </summary>
    /// <param name="visList">
    /// List of viseme numbers to play sequentially.
    /// </param>
    public void playVisemeList(List<int> visList) {
        // stop previously playing animations in viseme layer
        stopVisemeAnimations();
        // save list of visemes to play
        visemeList = visList;
        // loop through the set of viseme numbers
        foreach (int visNumber in visList) {
            // TODO api to set transition time, finding the right time to set
            float transitionTime = 0;
            // find the animation clip using the viseme number
            string clipName = visemeAnimations[visNumber].name;
			
            // Add the animation clip to the queue using the specified
            // transition time to smooth out animation. Animations added to the
            // queueu are set to let other animations complete before playing.
            animation.CrossFadeQueued(
                clipName,
                transitionTime,
                QueueMode.CompleteOthers);
        }
    }

    public void setCurentWord(int currentIndex) {
        
    }

    public void pauseSpeechAnimation(int currentIndex) {
        
    }

    public void resumeSpeechAnimation(int currentIndex) {
        
    }

    public void loadVisemeAnimations(GameObject coach) {
        // set the coach object
        newCoach = coach;
        
        // Get animation manager script attached to current avatar GameObject
        animationsManager = newCoach.GetComponent<AnimationsManager>();
        
        // get viseme animations specified in Unity
        visemeAnimations = animationsManager.getEnglishVisemes();
        
        // load default viseme animations for visemes not specified in Unity
        loadDefaultVisemes();
        
        // ensure viseme animations loaded from Unity have the right properties
        setAnimationProperties();
    }

    private void setAnimationProperties() {
        // Get Unity Animation component attached to current avatar GameObject
        animation = newCoach.GetComponent<Animation>();
        
        foreach (AnimationClip clip in visemeAnimations) {
            if (clip == null) {
                continue;
            }
            // enable legacy mode for manual animation management.
            clip.legacy = true;
            // set viseme animation to play once.
            clip.wrapMode = WrapMode.Once;
            // add clip to animation component
            animation.AddClip(clip, clip.name);
            // set visime animation layer
            animation[clip.name].layer = visemeLayer;
            // set visime animation speed
            animation[clip.name].speed = 1;
        }
    }

    private void loadDefaultVisemes() {
        int index = 0;
        // loop through animation names
        foreach (string name in visemeNames) {
            // if animation not specified by Unity user
            if (!visemeAnimations[index]) {
                // load default viseme animation from resources
                AnimationClip clip = Resources.Load("visemes/" + name) as AnimationClip;

                // print error if default animation resource not found
                if (!clip) {
                    Debug.LogError("Missing animation " + index + " : " + name + ".");
                }
                
                // update local Animations
                visemeAnimations[index] = clip;
            }
            index++;
        }
    }

    /// <summary>
    /// Stops all currently playing viseme animations
    /// </summary>
    private void stopVisemeAnimations() {
        // return if no animations playing
        if (visemeList == null) return;
        // loop through currently playing viseme numbers
        foreach (int visNumber in visemeList) {
            // find the animation
            AnimationClip clip = visemeAnimations[visNumber];
            // stop the animation by blending to weight 0 over 0 seconds.
            animation.Stop(clip.name);
        }
    }

    public void animateFox() {
        // make a list of visemes for the sentenc:
        // "The quick brown fox jumps over the lazy dog"
        List<int> fox = new List<int> {25, 9, 0, 37, 16, 2, 37, 0, 35, 18, 8, 22, 0, 26, 6, 37, 30, 
            0, 25, 17, 9, 21, 34, 30, 0, 11, 27, 20, 0, 25, 9, 0, 15, 3, 31, 1, 0, 25, 6, 38, 0};
        // play the list of animations sequentially
        playVisemeList(fox);
    }
}