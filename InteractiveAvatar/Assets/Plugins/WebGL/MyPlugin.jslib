// Unity functions are in c#, web functions are javascript, need method to 
// call c# from javascript : Runtime.dynCall(signature, pointer, args)
// signature = return type and arguments of function to be called.
// pointer = reference of function to be called
// args = array of arguments for function

// example: want a javascript function that calls callbackBoundary, which is a c#
// function that returns void and takes an integer as argument. The javascript function
// should take `event` as input, and call callbackBoundary with event.charIndex as input.
// signature : return type void (v), integer as input (i) = "vi"
// pointer : callbackBoundary
// args : [event.charIndex]
// result : function(event){Runtime.dynCall("vi", callbackBoundary, [event.charIndex])}

var MyPlugin = {
    // Speak javascript function that calls the textToSpeach.speak function 
    // with as arguments (text, voicename, parameters)
    Speak: function(textMessage, voiceType, callbackStart, callbackEnd, callbackBoundary) {
        textToSpeach.speak(Pointer_stringify(textMessage), Pointer_stringify(voiceType), {
            onstart: function(event){Runtime.dynCall('vf', callbackStart, [event.elapsedTime])},
            onend: function(event){Runtime.dynCall('vf', callbackEnd, [event.elapsedTime])},
            onboundary: function(event){Runtime.dynCall("vif", callbackBoundary, [event.charIndex, event.elapsedTime])}
        });
    },
    
	Stop: function() {
        textToSpeach.cancel();
    },
    
    getSystemVoices: function() {
    	console.log(textToSpeach.getSystemVoices())
    	//return textToSpeach.getSystemVoices();
    },
};
mergeInto(LibraryManager.library, MyPlugin);