using System.Collections.Generic;
using Models;
using UnityEngine;

public class SpeechAnimationManager : MonoBehaviour {

    // names of english visemes, used for starting viseme animations.
    // set to default values, these can be updated through API
    private string[] englishVisemeNames = {
        "Silence", "AA", "AE", "AH", "AO", "AW", "AY", "B", "CH", "D", "DH",
        "EH", "EL", "ER", "EY", "F", "G", "HX", "IH", "IY", "JH",
        "K", "LL", "M", "N", "NX", "OW", "OY", "P", "R", "S",
        "SH", "T", "TH", "UH", "UW", "V", "W", "Y", "Z", "ZH"
    };
    // length of english visemes, used for animation duration.
    // set to default values, these can be updated through API
    private float[] englishVisemeLengths = {
        0.05f, 0.05f, 0.05f, 0.05f, 0.05f, 0.05f, 0.05f, 0.05f, 0.05f, 0.05f,
        0.05f, 0.05f, 0.05f, 0.05f, 0.05f, 0.05f, 0.05f, 0.05f, 0.05f, 0.05f,
        0.05f, 0.05f, 0.05f, 0.05f, 0.05f, 0.05f, 0.05f, 0.05f, 0.05f, 0.05f,
        0.05f, 0.05f, 0.05f, 0.05f, 0.05f, 0.05f, 0.05f, 0.05f, 0.05f, 0.05f,
        0.05f
    };
    
    // Avatar model
    private GameObject newCoach;
    // Unity Animator component for the coach 
    private Animator animator;

    // time elapsed since start of last viseme animation
    private float elapsedTime = 0;
    // name of currently playing viseme animation
    private string currentVisemeName;
    // length of currently playing viseme animation    
    private float currentVisemeLength = 0;
    // index of currently playing viseme in the list to be played
    private int currentVisemeInList = 0;
    
    // layer for viseme (speech) animation
    private int visemeLayer = 1;
	
    // list of visemes that are currently playing
    private List<Viseme> visemeList;

    private string currentText;
    private bool isSpeaking = false;

    // TODO temporary while anticipating future changes
    private bool usingNumbers = true;
    private List<int> visemeListNumbers;
    // amount of visemes in current set to play
    private int visemeAmount = 0;
    
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
    /// Updates the elapsed time since the last rendered frame. Starts the next
    /// viseme animation if the elapsed time exceeds the duration of the
    /// currently playing viseme animation.
    /// </summary>
    /// <param name="lastFrameDuration">
    /// The elapsed time since the last rendered frame.
    /// </param>
    public void frameUpdate(float lastFrameDuration) {
        // update elapsed time since last frame
        elapsedTime += lastFrameDuration;
        
        // return if no visemes to animate
        if (!isSpeaking) {
            return;
        }

        // if last viseme animation finished
        if (elapsedTime >= currentVisemeLength) {
            // play next viseme animation
            playNextViseme();
            // reset elapsed time
            elapsedTime = 0;
        }
    }

    /// <summary>
    /// Sets the viseme animations to be played to be the specified list of
    /// viseme numbers. The first viseme from the list is played instantly,
    /// further visemes from the list are played after the currently playing
    /// viseme has finished untill all visemes from the list have been played.
    ///
    /// The frameUpdate function is used to determine when the next animation
    /// should be played.
    /// </summary>
    /// <param name="visList">
    /// Set of viseme numbers to be played.
    /// </param>
    private void playVisemeList(List<int> visList) {
        // save list of visemes to play
        visemeListNumbers = visList;
        visemeAmount = visemeListNumbers.Count;
        // set current viseme playing
        currentVisemeInList = 0;
        // play first viseme
        playNextViseme();
    }

    /// <summary>
    /// Sets the viseme animations to be played to be the specified list of
    /// viseme numbers. The first viseme from the list is played instantly,
    /// further visemes from the list are played after the currently playing
    /// viseme has finished untill all visemes from the list have been played.
    ///
    /// The frameUpdate function is used to determine when the next animation
    /// should be played.
    /// </summary>
    /// <param name="visList">
    /// Set of visemes to be played.
    /// </param>
    public void playVisemeList(List<Viseme> visList) {
        // save list of visemes to play
        visemeList = visList;
        visemeAmount = visemeList.Count;
        // set current viseme playing
        currentVisemeInList = 0;
        // play first viseme
        playNextViseme();
    }

    /// <summary>
    /// Starts animating the next viseme in the current set of visemes.
    ///
    /// Empties the set of visemes and resets the currently playing viseme when
    /// all visemes in the set have been animated.
    /// </summary>
    private void playNextViseme() {
        // TODO temporary distinction while anticipating future changes
        if (usingNumbers) {
            // look up number of current viseme in the set to be animated
            int visNumber = visemeListNumbers[currentVisemeInList];
            // store name of currently playing viseme
            currentVisemeName = englishVisemeNames[visNumber];
            // store duration of currently playing viseme
            currentVisemeLength = englishVisemeLengths[visNumber];
        } else {
            Viseme current = visemeList[currentVisemeInList];
            currentVisemeName = current.getVisemeCode().getName();
            //TODO viseme length from viseme Object
            currentVisemeLength = 0.05f;
        }
        
        // play the found viseme animation
        animator.CrossFade(currentVisemeName, 0, visemeLayer);
        // increment currently playing viseme
        currentVisemeInList++;

        // if all visemes in the set have been animated
        if (currentVisemeInList >= visemeAmount) {
            stopSpeechAnimation();
        }
    }

    /// <summary>
    /// Update the names used for viseme animations in the Animator.
    /// </summary>
    /// <param name="visemeNames"></param>
    public void setEnglishVisemeNames(string[] visemeNames) {
        englishVisemeNames = visemeNames;
    }

    /// <summary>
    /// Update the durations for viseme animations used in the Animator.
    /// </summary>
    /// <param name="visemeLengths"></param>
    public void setEnglishVisemeLengths(float[] visemeLengths) {
        englishVisemeLengths = visemeLengths;
    }

    public void setText(string text) {
        // for local access to spoken text.
        currentText = text;
    }

    /// <summary>
    /// Indicates that the next word is being spoken. Specified index is the
    /// index of the first character of that word in the full text string.
    /// 
    /// To be called from the speech animation onboundary event.
    /// </summary>
    /// <param name="charIndex"></param>
    public void onBoundary(int charIndex) {
        // TODO start animating the word
        
        // currentIndex specifies the start of the next word in the whole text
        
        // find set of viseme numbers for that word
        
        // call playVisemeList() with the set of visemes for a word
    }

    /// <summary>
    /// Starts animation for speech synthesis.
    ///
    /// To be called form the speech synthesis start event.
    /// </summary>
    public void startSpeechAnimation() {
        isSpeaking = true;
        
        // TODO start animating the full sentence in case of no boundary events
        // start speech animation (fox dummy sentence for now)
        animateFox();
    }

    /// <summary>
    /// Stops all speech animation and clear the current sentence.
    ///
    /// To be called from the speech synthesis stop event.
    /// </summary>
    public void stopSpeechAnimation() {
        // reset the currently playing viseme set
        isSpeaking = false;
        visemeList = null;
        currentVisemeInList = 0;
        // play the silence viseme animation
        animator.CrossFade("Silence", 0, visemeLayer);
    }

    /// <summary>
    /// Pauses animation for the currently active speech synthesis sentence.
    ///
    /// To be called form the speech synthesis pause event.
    /// </summary>
    /// <param name="currentIndex"></param>
    public void pauseSpeechAnimation(int currentIndex) {
        isSpeaking = false;
        Debug.Log("Paused");
    }

    /// <summary>
    /// Resumes animation for the currently active speech synthesis sentence.
    ///
    /// To be called form the speech synthesis resume event.
    /// </summary>
    /// <param name="currentIndex"></param>
    public void resumeSpeechAnimation(int currentIndex) {
        isSpeaking = true;
        Debug.Log("Resumed");
    }

    /// <summary>
    /// Sets the index of the layer in the Animator component that contains the
    /// viseme animations.
    ///
    /// Default speech animation layer index is 1.
    /// </summary>
    /// <param name="layer">index of layer containing viseme animations</param>
    public void setVisemeLayer(int layer) {
        visemeLayer = layer;
    }

    /// <summary>
    /// Loads the coach GameObject for the SpeechAnimation Class.
    /// 
    /// The coach object is assumed to have an Animator component.
    /// This Animator should contain a Speech Animation Layer, which should be
    /// set properly using setVisemeLayer, and should contain the required
    /// viseme Animations.
    /// </summary>
    /// <param name="coach">Current coach GameObject used</param>
    public void loadCoach(GameObject coach) {
        // set the local coach object reference
        newCoach = coach;
        
        // set the local reference for the Animator component attached to coach.
        animator = newCoach.GetComponent<Animator>();
    }

    /// <summary>
    /// Animates a preset demo sentence :
    /// "The quick brown fox jumps over the lazy dog"
    /// </summary>
    public void animateFox() {
        // make a list of visemes for the sentenc:
        // "The quick brown fox jumps over the lazy dog"
        List<int> fox = new List<int> {25, 9, 0, 37, 16, 2, 37, 0, 35, 18, 8, 22, 0, 26, 6, 37, 30, 
            0, 25, 17, 9, 21, 34, 30, 0, 11, 27, 20, 0, 25, 9, 0, 15, 3, 31, 1, 0, 25, 6, 38, 0};
        // play the list of animations sequentially
        playVisemeList(fox);
    }
}