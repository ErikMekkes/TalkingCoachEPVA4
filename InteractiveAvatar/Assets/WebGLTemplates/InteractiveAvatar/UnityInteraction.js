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

function startTalk(){
	var text = document.getElementById("textForSpeech").value;
	SendMessage('TalkingCoach', 'convertToSpeach', text);
}

function stopTalk(){
	SendMessage('TalkingCoach', 'stopSpeach');
}

function changeBackground(){
	SendMessage('TalkingCoach', 'changeBackround');
}

function changeCoach(){
	SendMessage('TalkingCoach', 'changeCoach');
}

function zoomIn(){
	SendMessage('TalkingCoach', 'zoom', -5);
}
function zoomOut(){
	SendMessage('TalkingCoach', 'zoom', 5);
}

function moveAvatarHorizontal(){
	SendMessage('TalkingCoach', 'moveAvatarHorizontal', 5);
}
function moveAvatarVertical(){
	SendMessage('TalkingCoach', 'moveAvatarVertical', 5);
}

//TODO use callback functions instead of messaging for pause / resume
//low priority, tested and difference is negligible, no efficiency gain

/**
 * Call the pauseSpeech function attached to the TalkingCoach Object. This
 * Should pause the currently ongoing speech synthesis. Has no effect if there
 * is no ongoing speech synthesis.
 */
function pauseSpeech() {
    SendMessage('TalkingCoach', 'pauseSpeech');
}

/**
 * Call the resumeSpeech function attached to the TalkingCoach Object. This
 * Should resume the currently paused speech synthesis. Has no effect if the
 * speech synthesis is not in a paused state.
 */
function resumeSpeech() {
    SendMessage('TalkingCoach', 'resumeSpeech');
    //TODO : could make this work like startTalk if not in paused state.
}

/**
 * Call the startDemo function attached to the TalkingCoach Object. This
 * Should start the prepared demo sentence with speech animation.
 */
function startDemo() {
    SendMessage('TalkingCoach', 'startDemo');
}