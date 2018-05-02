if (typeof textToSpeach != 'undefined') {
    console.log('textToSpeach already loaded');
    console.log(textToSpeach);
} else {

    var systemvoices;

            //Wait until system voices are ready and trigger the event OnVoiceReady
        if (typeof speechSynthesis != 'undefined') {
            speechSynthesis.onvoiceschanged = function () {

                systemvoices = window.speechSynthesis.getVoices();

                // if (self.OnVoiceReady != null) {
                //     self.OnVoiceReady.call();
                // }
            };
        }


    var TextToSpeach = function () {

    	var self = this;




        /*self.init = function() {

            if (typeof speechSynthesis === 'undefined') {

                console.log('TTS: Voice synthesis not supported');
            } else {
                //Waiting a few ms before calling getVoices() fixes some issues with safari on IOS as well as Chrome
                setTimeout(function () {
                    var gsvinterval = setInterval(function () {

                        var v = window.speechSynthesis.getVoices();

                        if (v.length == 0 && (systemvoices == null || systemvoices.length == 0)) {
                            //console.log('Voice support NOT ready');

                            voicesupport_attempts++;
                            if (voicesupport_attempts > VOICESUPPORT_ATTEMPTLIMIT) {
                                
                                clearInterval(gsvinterval);
                                
                                //On IOS, sometimes getVoices is just empty, but speech works. So we use a cached voice collection.
                                if (window.speechSynthesis != null) {
                                    
                                    if (self.iOS) {
                                        
                                        console.log('RV: Voice support ready (cached)');
                                        self.systemVoicesReady(cache_ios_voices);
                                        
                                    }else{
                                        
                                        console.log("RV: speechSynthesis present but no system voices found");
                                        self.enableFallbackMode();
                                    }
                                    
                                } else {
                                
                                    //We don't support voices. Using fallback
                                    self.enableFallbackMode();
                                }
                            }

                        } else {

                            console.log('RV: Voice support ready');
                            self.systemVoicesReady(v);
                            
                            clearInterval(gsvinterval);

                        }

                    }, 100);
                }, 100);
            }
            
            self.Dispatch("OnLoad");
        }*/


        self.getSystemVoices = function(){
            var voices = window.speechSynthesis.getVoices();
            // var voicesnames;
            // for (var i = 0; i < voices.length; i++) {
            //     voicesnames[i] = voices[i].name;
            // }
            console.log("voices" + voices);
            return voices;
        }

    	self.speak = function (text, voicename, parameters){

            self.msgtext = text;
            self.msgvoicename = voicename;
            self.msgparameters = parameters || {};

    		var msg = new SpeechSynthesisUtterance();
			msg.lang = 'nl-BE'
            msg.text = text;
            msg.volume = 1;
            msg.rate = 1;
            msg.pitch = 1;

            msg.onstart = self.speech_onstart;

            self.msgparameters.onendcalled = false;

            if (parameters != null) {

                            msg.onend = self.speech_onend;
                            msg.addEventListener('end',self.speech_onend);

                        msg.onerror = parameters.onerror || function (e) {
                            console.log('TTS: Error');
                            console.log(e);
                        };
                        
                        msg.onpause = parameters.onpause;
                        msg.onresume = parameters.onresume;
                        msg.onmark = parameters.onmark;
                        msg.onboundary = parameters.onboundary;
                    } else {
                        msg.onend = self.speech_onend;
                        msg.onerror = function (e) {
                            console.log('RV: Error');
                            console.log(e);
                        };
                    }

			window.speechSynthesis.speak(msg);
    	}

         self.cancel = function() {
            //self.checkAndCancelTimeout();
            //self.cancelled = true;
            console.log("Speach Cancelled");
            window.speechSynthesis.cancel();
        }


        
        self.checkAndCancelTimeout = function () {
            if (self.timeoutId != null) {
                //console.log("Timeout " + self.timeoutId + " cancelled");
                clearTimeout(self.timeoutId);
                self.timeoutId = null;
            }
        }

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
        }

        self.speech_onstart = function () {
            //if (!self.iOS)
            //console.log("Speech start");
            if (self.iOS)
                self.startTimeout(self.msgtext,self.speech_timedout);
            
            self.msgparameters.onendcalled=false;
            if (self.msgparameters != null && self.msgparameters.onstart != null) {
                self.msgparameters.onstart();
            }

        }


        self.Dispatch = function(name) {
            
            if (self.hasOwnProperty(name + "_callbacks") && 
                self[name + "_callbacks"].length > 0) {
                var callbacks = self[name + "_callbacks"];
                for(var i=0; i<callbacks.length; i++) {
                    callbacks[i]();
                }
                
            }
        }
        
        self.AddEventListener = function(name,callback) {
            if (self.hasOwnProperty(name + "_callbacks")) {
                self[name + "_callbacks"].push(callback);
            }else{
                console.log("RV: Event listener not found: " + name);
            }
        }

                //We should use jQuery if it's available
        // if (typeof $ === 'undefined') {
        //     document.addEventListener('DOMContentLoaded', function () {
        //         self.init();
        //     });
        // } else {

        //     $(document).ready(function () {
        //         self.init();
        //     });
        // } 

    }

    var textToSpeach = new TextToSpeach();
}