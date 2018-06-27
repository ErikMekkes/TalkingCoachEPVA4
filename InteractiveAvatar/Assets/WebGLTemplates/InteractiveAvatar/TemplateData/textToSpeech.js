if (typeof textToSpeach != 'undefined') {
    console.log('textToSpeach already loaded');
    console.log(textToSpeach);
} else {

    var systemvoices;

    //Wait until system voices are ready and trigger the event OnVoiceReady
    if (typeof speechSynthesis != 'undefined') {
        speechSynthesis.onvoiceschanged = function () {
            systemvoices = window.speechSynthesis.getVoices();
        };
    }

    var TextToSpeach = function () {

    	var self = this;

        self.getSystemVoices = function(){
            var voices = window.speechSynthesis.getVoices();
            console.log("voices" + voices);
            return voices;
        };
        
        // self.speak is also defined in Plugins/WebGL/MyPlugins.jslib which lets 
        // the Unity WebGL Build Process link it to this one. 
        // It is dynamically included in and called from Scripts/TextManager.cs
    	self.speak = function (text, voicename, parameters){

            self.msgtext = text;
            self.msgvoicename = voicename;
            self.msgparameters = parameters || {};

    		var msg = new SpeechSynthesisUtterance();
			msg.lang = self.msgparameters.language;
            msg.text = text;
            msg.volume = 1;
            msg.rate = 1;
            msg.pitch = 1;

            msg.onstart = self.speech_onstart;

            self.msgparameters.onendcalled = false;
            
            if (parameters != null) {
                msg.onerror = parameters.onerror || function (e) {
                    console.log('TTS: Error');
                    console.log(e);
                };
								
                msg.onpause = parameters.onpause;
                msg.onresume = parameters.onresume;
                msg.onmark = parameters.onmark;
                msg.onboundary = parameters.onboundary;
                msg.onstart = parameters.onstart;
                msg.onend = parameters.onend;
            } else {
                msg.onend = self.speech_onend;
                msg.onerror = function (e) {
                    console.log('RV: Error');
                    console.log(e);
                };
            }

			window.speechSynthesis.speak(msg);
    	};
        
        self.checkAndCancelTimeout = function () {
            if (self.timeoutId != null) {
                //console.log("Timeout " + self.timeoutId + " cancelled");
                clearTimeout(self.timeoutId);
                self.timeoutId = null;
            }
        };

        self.speech_onend = function () {
            self.checkAndCancelTimeout();
            
            //Avoid this being automatically called just after calling speechSynthesis.cancel
            if (self.cancelled === true) {
                self.cancelled = false;
                return;
            }
            
            //console.log("on end fired");
            if (self.msgparameters != null && self.msgparameters.onend != null && self.msgparameters.onendcalled!=true) {
                //console.log("Speech on end called  -" + self.msgtext);
                self.msgparameters.onendcalled=true;
                self.msgparameters.onend();
            } 
        };

        self.speech_onstart = function () {
            //if (!self.iOS)
            //console.log("Speech start");
            if (self.iOS)
                self.startTimeout(self.msgtext,self.speech_timedout);
            
            self.msgparameters.onendcalled=false;
            if (self.msgparameters != null && self.msgparameters.onstart != null) {
                self.msgparameters.onstart();
            }

        };


        self.Dispatch = function(name) {
            if (self.hasOwnProperty(name + "_callbacks") && 
                self[name + "_callbacks"].length > 0) {
                var callbacks = self[name + "_callbacks"];
                for(var i=0; i<callbacks.length; i++) {
                    callbacks[i]();
                }
            }
        };
        
        self.AddEventListener = function(name,callback) {
            if (self.hasOwnProperty(name + "_callbacks")) {
                self[name + "_callbacks"].push(callback);
            } else {
                console.log("RV: Event listener not found: " + name);
            }
        };

    };

    var textToSpeach = new TextToSpeach();
}