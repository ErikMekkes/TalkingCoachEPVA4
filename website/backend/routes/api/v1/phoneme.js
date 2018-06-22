const express = require('express');
const router = express.Router();
const child_process = require('child_process');
const {spawn} = require('child_process');
const PhonemeDictionary = require('./phonemeDictionary');

const dict = new PhonemeDictionary();

let warnings = [];

/* GET /api/v1/phoneme */
router.get('/', function (req, res, next) {
	let params = req.query;
	warnings = [];

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
		console.log(code);
		let phonemeString = cleanPhonemeString(stdout, params.lang);
		let phonemeArray = getPhonemeArrayFromString(phonemeString);
		let phonemeArrayArpa = phonemeArrayToArpabet(phonemeArray);

		let jsonResponse = {
			_request: {route: req.baseUrl, query: req.query, api_ver: "v1"},
			phonemes: phonemeArrayArpa
		};

		if (warnings.length > 0) {
			jsonResponse.warnings = warnings;
		}

		res.status(200).json(jsonResponse);
	});
});

function cleanPhonemeString(messyString, language) {
	let result = messyString;
	console.log(messyString);
	result = result.trim();
	if (language === "nl" || language === "nl-NL") {
		result = result.replace(/[_!',|]/gi, "");
	} else if (language === "en-us" || language === "en-US" || language === "En-US") {
		result = result.replace(/[_:!',|]/gi, "");
	} else {
		result = result.replace(/[_:!',|]/gi, "");
		console.warn(`[WARN] Unknown language: ${language}. Don't know which cleanup to perform!`);
		warnings.push('language not supported by API')
	}
	result = result.trim();
	return result;
}

function getPhonemeArrayFromString(phonemeString) {
	phonemeString = phonemeString.replace(/[ ]/gi, './.');
	return phonemeString.split(/[. ]/gi);
}

function phonemeArrayToArpabet(phonemeArray) {
	let result = [];
	console.log(phonemeArray);
	for (let i = 0; i < phonemeArray.length; i++) {
		let phoneme = phonemeArray[i].trim();

		console.log(`eSpeak: ${phoneme}`);

		if (';' === phoneme || '_:' === phoneme || '' === phoneme || '\n' === phoneme) {
			console.log("This should be unreachable code. Firefly.");
			continue;
		}

		// TODO: Can be better, perhaps with a Map
		if (dict.dictionary.has(phoneme)) {
			dict.dictionary.get(phoneme).forEach((value) => {
				result.push(value);
				console.log(`Phoneme: ${value}`);
			});
		} else {
			console.log("!!! Invalid phoneme !!!")
		}
		console.log();
	}
	return result;
}

module.exports = router;