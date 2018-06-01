const express = require('express');
const router = express.Router();
const child_process = require('child_process');
const {spawn} = require('child_process');

/* GET /api/v1/phoneme */
router.get('/', function (req, res, next) {
	let params = req.query;

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

	let espeak = null;

	if (params.ipa === undefined) {
		espeak = child_process.exec(`espeak-ng -qx -v ${params.lang} --sep=. "${params.text}"`);
	} else {
		espeak = child_process.exec(`espeak-ng -qx --ipa -v ${params.lang} --sep=. "${params.text}"`);
	}

	let stdout = "";

	espeak.stdout.on('data', (data) => {
		// console.log("stdout: " + data);
		stdout += data;
	});
	espeak.on('close', (code) => {
		let phonemeString = cleanPhonemeString(stdout);
		console.log(phonemeString);
		let phonemeArray = getPhonemeArrayFromString(phonemeString);
		let phonemeArrayArpa = phonemeArrayToArpabet(phonemeArray);
		res.status(200).json({
			_request: {route: req.baseUrl, query: req.query, api_ver: "v1"},
			phonemes: phonemeArrayArpa
		});
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
		let phoneme = phonemeArray[i];
		let phonemeArpa = "";

		// TODO: Can be better, perhaps with a Map
		switch (phoneme) {
				/* Vowels */
			case 'A':
			case '0':
				phonemeArpa = "AA";
				break;
			case 'a':
				phonemeArpa = "AE";
				break;
			case 'V':
				phonemeArpa = "AH";
				break;
			case 'O':
				phonemeArpa = "AO";
				break;
			case 'aU':
				phonemeArpa = "AW";
				break;
			case 'a#':
				phonemeArpa = "AX";
				break;
			case 'aI':
				phonemeArpa = "AY";
				break;
			case 'E':
				phonemeArpa = "EH";
				break;
			case '3':
				phonemeArpa = "ER";
				break;
			case 'eI':
				phonemeArpa = "EY";
				break;
			case 'I':
				phonemeArpa = "IH";
				break;
			case 'I#':
				phonemeArpa = "IX";
				break;
			case 'i':
				phonemeArpa = "IY";
				break;
			case 'oU':
				phonemeArpa = "OW";
				break;
			case 'OI':
				phonemeArpa = "OY";
				break;
			case 'U':
				phonemeArpa = "UH";
				break;
			case 'u':
				phonemeArpa = "UW";
				break;

				/* Consonants */
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
			case 't#':
				phonemeArpa = "DX";
				break;
			case '@L':
				phonemeArpa = "EL";
				break;
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
				phonemeArpa = "NX";
				break;
			case 'p':
				phonemeArpa = "P";
				break;
				/* eSpeak doesn't recognise ARPA 'Q' */
			case 'r':
				phonemeArpa = "R";
				break;
			case 's':
				phonemeArpa = "S";
				break;
			case 'S':
				phonemeArpa = "SH";
				break;
			case 't':
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
		result[i] = phonemeArpa;
	}
	return result;
}

module.exports = router;