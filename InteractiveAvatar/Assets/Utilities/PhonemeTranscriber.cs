using System;
using System.Diagnostics;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace Utilities
{
    public class PhonemeTranscriber
    {
        public const string LANG_NL = "nl";
        public const string LANG_EN_US = "en-us";
        
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        public static void TranscribeTest()
        {
            Transcribe("Pa’s wijze lynx bezag vroom het fikse aquaduct.", SpeechLanguage.nl);
        }
        
        // TODO This implementation is still quite dirty, using the command line to use eSpeak
        // There must be a better way to do this.

        public static String Transcribe(string text, SpeechLanguage lang)
        {
            string cmdText = "espeak-ng -x --sep=. -v "+ lang +" \"" + text + "\"";
//            string cmdText = "echo %PATH%";
            
            Process process = new Process();
            process.StartInfo.FileName = "cmd.exe";
            process.StartInfo.Arguments = "/C " + cmdText;
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.RedirectStandardError = true;
            process.StartInfo.UseShellExecute = false;
            process.Start();
            
            Debug.Assert(process != null, "process != null");
            
            while(process.HasExited == false) {}

            if (process.ExitCode != 0)
            {
                Debug.Log("Something went wrong, Transcribe process exited with code " + process.ExitCode);
                Debug.Log("Error stream \n" + process.StandardError.ReadToEnd());
                Debug.Log("Output stream \n" + process.StandardOutput.ReadToEnd());
                return null;
            }
            
            Debug.Log("Output");
            Debug.Log(process.StandardOutput.ReadToEnd());
            
            return null;
        }

    }

    public sealed class SpeechLanguage
    {
        private readonly String name;
        private readonly int value;

        public static readonly SpeechLanguage nl = new SpeechLanguage (1, "nl");
        public static readonly SpeechLanguage en_US = new SpeechLanguage (2, "en-us");  

        private SpeechLanguage(int value, String name){
            this.name = name;
            this.value = value;
        }

        public override String ToString(){
            return name;
        }
    }
}