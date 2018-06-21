const dictionaryJson = require("./phonemeDictionary.json");

class PhonemeDictionary {

	constructor() {
		this.dictionary = PhonemeDictionary.objectToMap(dictionaryJson);
	}

	static objectToMap(obj) {
		let map = new Map();
		for (let k of Object.keys(obj)) {
			map.set(k, obj[k]);
		}
		return map;
	}

}

module.exports = PhonemeDictionary;