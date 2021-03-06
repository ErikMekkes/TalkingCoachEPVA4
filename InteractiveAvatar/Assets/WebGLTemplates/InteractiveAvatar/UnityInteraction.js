/**
 * This javascript file is static, it's included with the template web page.
 * 
 * SendMessage(object, function, value) calls function with value on object
 * within the Unity scene. Assuming object and function are present/attached.
 * 
 * Only one value is permitted (can be an array or other object), if the
 * function implementation takes no arguments the passed values is ignored.
 * 
 * Functions called on the TalkingCoach object are implemented in:
 * Scripts/TalkingCoachAPI.cs
 * 
 * This messaging functionality is enabled by the Unity WebGL Build process. 
 */

var gameInstance = UnityLoader.instantiate("gameContainer", "Build/mybuild.json", {onProgress: UnityProgress});


function startTalk(){
	var text = document.getElementById("textForSpeech").value;
    gameInstance.SendMessage('TalkingCoach', 'convertToSpeech', text);
}

function stopTalk(){
    gameInstance.SendMessage('TalkingCoach', 'stopSpeech');
}

function setLanguage() {
    var language = document.getElementById("langChoice").value;
    console.log(language);
    gameInstance.SendMessage('TalkingCoach', 'setLanguage', language);
}

function changeBackground(){
    gameInstance.SendMessage('TalkingCoach', 'changeBackround');
}

function changeCoach(){
    gameInstance.SendMessage('TalkingCoach', 'changeCoach');
}

function zoomIn(){
    gameInstance.SendMessage('TalkingCoach', 'zoom', -5);
}
function zoomOut(){
    gameInstance.SendMessage('TalkingCoach', 'zoom', 5);
}

function moveAvatarHorizontal(){
    gameInstance.SendMessage('TalkingCoach', 'moveAvatarHorizontal', 5);
}
function moveAvatarVertical(){
    gameInstance.SendMessage('TalkingCoach', 'moveAvatarVertical', 5);
}

/**
 * Call the pauseSpeech function attached to the TalkingCoach Object. This
 * Should pause the currently ongoing speech synthesis. Has no effect if there
 * is no ongoing speech synthesis.
 */
function pauseSpeech() {
    gameInstance.SendMessage('TalkingCoach', 'pauseSpeech');
}

/**
 * Call the resumeSpeech function attached to the TalkingCoach Object. This
 * Should resume the currently paused speech synthesis. Has no effect if the
 * speech synthesis is not in a paused state.
 */
function resumeSpeech() {
    gameInstance.SendMessage('TalkingCoach', 'resumeSpeech');
}

/**
 * Sets the hostname to use as Phoneme Server.
 * @param hostName the specified hostname for the phoneme server.
 */
function setHostName(hostName) {
    gameInstance.SendMessage('TalkingCoach', 'setPhonemeServerHost', hostName);
    console.log(hostName);
}

