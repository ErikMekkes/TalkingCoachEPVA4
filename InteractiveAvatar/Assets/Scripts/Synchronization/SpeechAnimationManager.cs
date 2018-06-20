using System.Collections.Generic;
using Models;
using UnityEngine;

public class SpeechAnimationManager : MonoBehaviour {
    // Avatar model
    private GameObject newCoach;
    // Unity Animator component for the coach 
    private Animator animator;
    
    // layer for viseme (speech) animation
    private int visemeLayer = 1;

    // whether the speech animation is active
    private bool isSpeaking = false;
    
    // time elapsed since start of last viseme animation
    private float elapsedTime = 0;
    // full text that is currently being spoken
    private string currentText;
    // list of visemes that are currently playing
    private List<Viseme> visemeList;
    // name of currently playing viseme animation
    private string currentVisemeName;
    // length of currently playing viseme animation    
    private float currentVisemeLength = 0;
    // index of currently playing viseme in the list to be played
    private int currentVisemeInList = 0;
    // amount of visemes in current set to play
    private int visemeAmount = 0;

    // Unity interface field for visemeTimings script.
    [SerializeField] private VisemeTimings visemeTimings;
    
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
    /// Sets the visemes to be played to be the specified list of visemes. The
    /// first viseme from the list is played instantly, further visemes from the
    /// list are played after the currently playing viseme has finished untill
    /// all visemes from the list have been played.
    ///
    /// The frameUpdate function is used to determine when the next animation
    /// should be played.
    /// </summary>
    /// <param name="visList">
    /// Set of visemes to be played.
    /// </param>
    public void playVisemeList(List<Viseme> visList) {
        if (null == visList) {
            return;
        }
        // save list of visemes to play
        visemeList = visList;
        visemeAmount = visemeList.Count;
        if (0 == visemeAmount) {
            return;
        }
        // set current viseme playing
        currentVisemeInList = 0;
        // play first viseme
        playNextViseme();
        isSpeaking = true;
    }

    /// <summary>
    /// Starts animating the next viseme in the current set of visemes.
    ///
    /// Empties the set of visemes and resets the currently playing viseme when
    /// all visemes in the set have been animated.
    /// </summary>
    private void playNextViseme() {
        // if all visemes in the set have been animated
        if (currentVisemeInList >= visemeAmount) {
            stopSpeechAnimation();
            return;
        }
        
        Viseme current = visemeList[currentVisemeInList];
        currentVisemeName = current.getVisemeCode().getName();
        currentVisemeLength = (float) current.getDuration();
        
        // play the found viseme animation
        animator.CrossFade(currentVisemeName, 0, visemeLayer);
        // increment currently playing viseme
        currentVisemeInList++;
    }
    
    public VisemeTimings getVisemeTimingCalculator()
    {
        return visemeTimings;
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
}