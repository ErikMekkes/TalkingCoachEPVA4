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