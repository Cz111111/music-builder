import * as Tone from "tone";
const string_ensemble = new Tone.Sampler({
    urls: {
		"C4": "https://raw.githubusercontent.com/gleitz/midi-js-soundfonts/gh-pages/FatBoy/string_ensemble_1-mp3/C4.mp3",
	},
	release: 1,
});

export default string_ensemble;
//48-55 弦乐合奏音色1