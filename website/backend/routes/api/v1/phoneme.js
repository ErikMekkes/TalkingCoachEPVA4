const express = require('express');
const router = express.Router();
const child_process = require('child_process');
const {spawn} = require('child_process');

/* GET /api/v1/phoneme */
router.get('/', function (req, res, next) {
	let params = req.query;
	
	
	/* Enable CORS */
	res.header("Access-Control-Allow-Origin", "*");
	res.header("Access-Control-Allow-Headers", "Origin, X-Requested-With, Content-Type, Accept");

	/* Test if text query is defined */
	if (params.text === undefined) {
		res.status(404).json({
			_request: {
				route: req.baseUrl,
				query: req.query,
				api_ver: "v1",
				error: "malformed query - text not defined"
			}
		});
		return;
	}
	if (params.lang === undefined) {
		res.status(404).json({
			_request: {
				route: req.baseUrl,
				query: req.query,
				api_ver: "v1",
				error: "malformed query - lang not defined"
			}
		});
		return;
	}

	// const espeak = spawn('espeak-ng', ['-qx', '-v', `${params.lang}`, '--sep=.', '"' + params.text.trim() + '"']);

	let espeak = child_process.exec(`espeak-ng -qx -v ${params.lang} --sep=. "${params.text}"`);
	let stdout = "";

	espeak.stdout.on('data', (data) => {
		stdout += data;
	});
	espeak.on('close', (code) => {
		let phonemeString = cleanPhonemeString(stdout);
		let phonemeArray = getPhonemeArrayFromString(phonemeString);
		let phonemeArrayArpa = phonemeArrayToArpabet(phonemeArray);
		console.log(phonemeArrayArpa)
		res.status(200).json({
			_request: {route: req.baseUrl, query: req.query, api_ver: "v1"},
			phonemes: phonemeArrayArpa
		});
		// console.log(res);
	});
});

function cleanPhonemeString(messyString) {
	let result = messyString;
	result = result.trim();
	result = result.replace(/[_:!',|]/gi, "");
	result = result.trim();
	return result;
}

function getPhonemeArrayFromString(phonemeString) {
	phonemeString = phonemeString.replace(/[ ]/gi, './.');
	return phonemeString.split(/[. ]/gi);
}

function phonemeArrayToArpabet(phonemeArray) {
	let result = [];
	for (let i = 0; i < phonemeArray.length; i++) {
		let phoneme = phonemeArray[i].trim();
		let phonemeArpa = "";
		if (';' === phoneme || '_:' === phoneme || '' === phoneme || '\n' === phoneme) {
			continue;
		}

		// TODO: Can be better, perhaps with a Map
		switch (phoneme) {
				/* Vowels */
			case 'A':
			case 'O2':
			case '0':
				phonemeArpa = "AA";
				break;
			case 'aa':
			case 'a':
				phonemeArpa = "AE";
				break;
			case 'I2':
			case '@2':
			case '@':
			case 'V':
			case 'a#':
				phonemeArpa = "AH";
				break;
			case 'O@':
				result.push("AO");
				result.push("R");
				continue;
			case 'e@':
				result.push("EH");
				result.push("R");
				continue;
			case 'O':
			case 'o@':
			case 'U@':
				phonemeArpa = "AO";
				break;
			case 'aU':
				phonemeArpa = "AW";
				break;
			case 'aI':
				phonemeArpa = "AY";
				break;
			case 'E':
			case '@-':
				phonemeArpa = "EH";
				break;
			case '3':
			case '3r':
				phonemeArpa = "ER";
				break;
			case 'eI':
				phonemeArpa = "EY";
				break;
			case 'I':
			case 'I#':
				phonemeArpa = "IH";
				break;
			case 'i':
			case 'i@':
				phonemeArpa = "IY";
				break;
			case 'i@3':
				result.push("IY");
				result.push("ER");
				continue;
			case 'aI@':
				result.push("AY");
				result.push("AH");
				continue;
			case 'oU':
				phonemeArpa = "OW";
				break;
			case 'OI':
				phonemeArpa = "OY";
				break;
			case 'U':
				phonemeArpa = "UH";
				break;
			case 'u:':
			case 'u':
				phonemeArpa = "UW";
				break;
			case 'aI3':
				result.push("AY");
				result.push("ER");
				continue;
				/* Consonants */
			case 'A@':
				result.push("AA");
				result.push("R");
				continue;
			case 'b':
				phonemeArpa = "B";
				break;
			case 'tS':
				phonemeArpa = "CH";
				break;
			case 'd':
				phonemeArpa = "D";
				break;
			case 'D':
				phonemeArpa = "DH";
				break;
			case '@L':
				result.push("AH");
				result.push("L");
				continue;
				/* eSpeak doesn't recognise ARPA 'EM', is 'M' */
			case 'n-':
				phonemeArpa = "EN";
				break;
			case 'f':
				phonemeArpa = "F";
				break;
			case 'g':
				phonemeArpa = "G";
				break;
			case 'h':
				phonemeArpa = "HH";
				break;
			case 'dZ':
				phonemeArpa = "JH";
				break;
			case 'k':
				phonemeArpa = "K";
				break;
			case 'l':
				phonemeArpa = "L";
				break;
			case 'm':
				phonemeArpa = "M";
				break;
			case 'n':
				phonemeArpa = "N";
				break;
			case 'N':
				phonemeArpa = "NG";
				break;
			case 'p':
				phonemeArpa = "P";
				break;
				/* eSpeak doesn't recognise ARPA 'Q' */
			case 'r-':
			case 'r':
				phonemeArpa = "R";
				break;
			case 's':
				phonemeArpa = "S";
				break;
			case 'S':
				phonemeArpa = "SH";
				break;
			case 't2':
			case 't':
			case 't#':
				phonemeArpa = "T";
				break;
			case 'T':
				phonemeArpa = "TH";
				break;
			case 'v':
				phonemeArpa = "V";
				break;
			case 'w':
				phonemeArpa = "W";
				break;
				/* eSpeak doesn't recognise ARPA 'WH', is 'W' */
			case 'j':
				phonemeArpa = "Y";
				break;
			case 'z':
				phonemeArpa = "Z";
				break;
			case 'Z':
				phonemeArpa = "ZH";
				break;
				/* '/' denotes word boundaries */
			case '/':
				phonemeArpa = "/";
				break;
			default:
				phonemeArpa = "invalid phoneme";
				break;
		}
		result.push(phonemeArpa);
	}
	return result;
}

module.exports = router;