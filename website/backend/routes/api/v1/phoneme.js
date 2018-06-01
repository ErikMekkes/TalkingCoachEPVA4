const express = require('express');
const router = express.Router();
const child_process = require('child_process');
const {spawn} = require('child_process');

/* GET /api/v1/phoneme */
router.get('/', function(req, res, next) {
	let params = req.query;

	/* Test if text query is defined */
	if(params.text === undefined) {
		res.status(404).json({ _request: {route: req.baseUrl, query: req.query, api_ver: "v1", error: "malformed query - text not defined"} });
		return;
	}
	if(params.lang === undefined) {
		res.status(404).json({ _request: {route: req.baseUrl, query: req.query, api_ver: "v1", error: "malformed query - lang not defined"} });
		return;
	}

	// const espeak = spawn('espeak-ng', ['-qx', '-v', `${params.lang}`, '--sep=.', '"' + params.text.trim() + '"']);
	const espeak = child_process.exec(`espeak-ng -qx -v ${params.lang} --sep=. "${params.text}"`);
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
		res.status(200).json({ _request: {route: req.baseUrl, query: req.query, api_ver: "v1"}, phonemes: phonemeArrayArpa });
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
	phonemeString = phonemeString.replace(/[ ]/gi, '.<wordspace>.');
	return phonemeString.split(/[. ]/gi);
}

function phonemeArrayToArpabet(phonemeArray) {
	let result = [];
	for(let i = 0; i < phonemeArray.length; i++) {
		let phoneme = phonemeArray[i];
		let phonemeArpa = "";
		switch(phoneme) {
			default:
				phonemeArpa = phoneme;
				break;
		}
		result[i] = phonemeArpa;
	}
	return result;
}

module.exports = router;