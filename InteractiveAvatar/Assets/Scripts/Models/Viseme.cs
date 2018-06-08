using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

namespace Models
{
	public class Viseme
	{
		private static Dictionary<VisemeCode, AnimationClip> _visemeAnimationMapping;
		private VisemeCode _visemeCode;

		public Viseme(VisemeCode phonemeCode)
		{
			_visemeCode = phonemeCode;
			if (_visemeAnimationMapping == null)
			{
				_visemeAnimationMapping = new Dictionary<VisemeCode, AnimationClip>();
				mapVisemesToAnimations(_visemeAnimationMapping);
			}
		}

		public VisemeCode getVisemeCode()
		{
			return _visemeCode;
		}

		public AnimationClip toAnimation()
		{
			return _visemeAnimationMapping[_visemeCode];
		}

		public double getDuration()
		{
			Debug.Log("Searching for duration of viseme: " + _visemeCode.getName());
			return SpeechAnimationManager.instance.getVisemeTimingCalculator().getDictionary()[_visemeCode.getName()];
		}

		private static void mapVisemesToAnimations(Dictionary<VisemeCode, AnimationClip> visemeMapping)
		{
			string[] visemeCodesInOrder =
			{
				"Silence", "AA", "AE", "AH", "AO", "AW", "AY", "B", "CH", "D", "DH",
				"EH", "EL", "ER", "EY", "F", "G", "HX", "IH", "IY", "JH",
				"K", "LL", "M", "N", "NX", "OW", "OY", "P", "R", "S",
				"SH", "T", "TH", "UH", "UW", "V", "W", "Y", "Z", "ZH"
			};

			for (int i = 0; i < visemeCodesInOrder.Length; i++)
			{
				string name = visemeCodesInOrder[i];
				// load default viseme animation from resources
				AnimationClip clip = Resources.Load("visemes/" + name) as AnimationClip;

				// print error if default animation resource not found
				if (!clip)
				{
					Debug.LogError("Missing animation " + i + " : " + name + ".");
				}

				// update local Animations
				visemeMapping.Add(VisemeCode.getVisemeCode(name), clip);
			}
		}

		/// <summary>
		/// Type-safe enum pattern for viseme codes<br/>
		/// See <a href="http://www.javacamp.org/designPattern/enum.html">Typesafe Enum</a>
		/// </summary>
		public sealed class VisemeCode
		{
			private readonly int _value;
			private readonly String _name;

			public static readonly VisemeCode silence = new VisemeCode(0, "Silence");
			public static readonly VisemeCode AY = new VisemeCode(1, "AY");
			public static readonly VisemeCode AW = new VisemeCode(2, "AW");
			public static readonly VisemeCode AH = new VisemeCode(3, "AH");
			public static readonly VisemeCode AE = new VisemeCode(4, "AE");
			public static readonly VisemeCode IH = new VisemeCode(5, "IH");
			public static readonly VisemeCode AA = new VisemeCode(6, "AA");
			public static readonly VisemeCode B = new VisemeCode(7, "B");
			public static readonly VisemeCode D = new VisemeCode(8, "D");
			public static readonly VisemeCode JH = new VisemeCode(9, "JH");
			public static readonly VisemeCode DH = new VisemeCode(10, "DH");
			public static readonly VisemeCode EY = new VisemeCode(11, "EY");
			public static readonly VisemeCode EH = new VisemeCode(12, "EH");
			public static readonly VisemeCode ER = new VisemeCode(13, "ER");
			public static readonly VisemeCode OW = new VisemeCode(14, "OW");
			public static readonly VisemeCode F = new VisemeCode(15, "F");
			public static readonly VisemeCode G = new VisemeCode(16, "G");
			public static readonly VisemeCode HX = new VisemeCode(17, "HX");
			public static readonly VisemeCode IY = new VisemeCode(18, "IY");
			public static readonly VisemeCode Y = new VisemeCode(19, "Y");
			public static readonly VisemeCode K = new VisemeCode(20, "K");
			public static readonly VisemeCode LL = new VisemeCode(21, "LL");
			public static readonly VisemeCode M = new VisemeCode(22, "M");
			public static readonly VisemeCode N = new VisemeCode(23, "N");
			public static readonly VisemeCode NX = new VisemeCode(24, "NX");
			public static readonly VisemeCode AO = new VisemeCode(25, "AO");
			public static readonly VisemeCode OY = new VisemeCode(26, "OY");
			public static readonly VisemeCode P = new VisemeCode(27, "P");
			public static readonly VisemeCode R = new VisemeCode(28, "R");
			public static readonly VisemeCode S = new VisemeCode(29, "S");
			public static readonly VisemeCode SH = new VisemeCode(30, "SH");
			public static readonly VisemeCode T = new VisemeCode(31, "T");
			public static readonly VisemeCode CH = new VisemeCode(32, "CH");
			public static readonly VisemeCode UW = new VisemeCode(33, "UW");
			public static readonly VisemeCode UH = new VisemeCode(34, "UH");
			public static readonly VisemeCode V = new VisemeCode(35, "V");
			public static readonly VisemeCode W = new VisemeCode(36, "W");
			public static readonly VisemeCode Z = new VisemeCode(37, "Z");
			public static readonly VisemeCode ZH = new VisemeCode(38, "ZH");
			public static readonly VisemeCode TH = new VisemeCode(39, "TH");
			public static readonly VisemeCode EL = new VisemeCode(40, "EL");
			public static readonly VisemeCode wordspace = new VisemeCode(41, "Silence");
			public static readonly VisemeCode invalid = new VisemeCode(42, "invalid");

			private static readonly List<VisemeCode> _visemeCodeList = new List<VisemeCode>
			{
				silence,
				AY,
				AW,
				AH,
				AE,
				IH,
				AA,
				B,
				D,
				JH,
				DH,
				EY,
				EH,
				ER,
				OW,
				F,
				G,
				HX,
				IY,
				Y,
				K,
				LL,
				M,
				N,
				NX,
				AO,
				OY,
				P,
				R,
				S,
				SH,
				T,
				CH,
				UW,
				UH,
				V,
				W,
				Z,
				ZH,
				TH,
				EL,
				wordspace,
				invalid
			};

			private static Dictionary<String, VisemeCode> _visemeCodes = null;

			private VisemeCode(int value, String name)
			{
				_value = value;
				_name = name;
			}

			public string getName()
			{
				return _name;
			}

			public static VisemeCode getVisemeCode(string code)
			{
				if (_visemeCodes == null)
				{
					_visemeCodes = new Dictionary<String, VisemeCode>();
					for (int i = 0; i < _visemeCodeList.Count; i++)
					{
						VisemeCode visemeCode = _visemeCodeList[i];
						_visemeCodes.Add(visemeCode._name, visemeCode);
					}
				}

				return _visemeCodes[code];
			}
		}
	}
}