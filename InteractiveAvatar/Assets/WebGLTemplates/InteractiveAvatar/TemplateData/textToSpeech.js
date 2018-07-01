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
			
			/*
			 * Sets the host to use as Phoneme Server.
			 *
			 * The specified hostname must use the following format: protocol://hostname:port/route
			 * Ensure that the specified hostname is active, a phoneme server is required for speech.
			 * 
			 * When Unity is ready this function is automatically executed once by textToSpeech.js to
			 * make sure the hostName is set. The default hostName can be changed within this function.
			 */
			self.setPhonemeServerHost = function(hostName) {
				// if the function was not called with a specified hostName, use default value.
				if (!hostName) {
					// The default hostName can be specified here, by changing the value of hostName.
					
					// current default = http://current_hostname:3001/api/v1/
					var newUrl = "http://" + location.hostName + ":3001/api/v1/";
					
					hostName = newUrl;
					
					// Other location attributes are available to create a default hostName string,
					// see https://www.w3schools.com/jsref/obj_location.asp for available options.
				}
				
				gameInstance.SendMessage('TalkingCoach', 'setPhonemeServerHost', hostName);
				console.log("hostName set to : " + hostName);
			};
			
			/*
			 * Specifies the host to use as Phoneme Server.
			 * 
			 * When Unity is ready this function is automatically called once to load the specified hostName
			 * 
			 * The specified hostname must use the standard location format: protocol://hostname:port/route
			 * Ensure that the specified server is active, the phoneme server is required for speech.
			 */
			self.getPhonemeServerHost = function() {
                // current default = http://current_hostname:3001/api/v1/
			    var hostName = "http://" + location.hostName + ":3001/api/v1/";

                console.log("Phoneme Server host set to : " + hostName);
			    return hostName;
            };

			self.unityReady = function() {
				console.log("Unity Ready!");
				self.setPhonemeServerHost(false);
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