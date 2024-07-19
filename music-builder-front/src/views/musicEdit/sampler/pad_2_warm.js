import * as Tone from "tone";
const pad_2_warm = new Tone.Sampler({
    urls: {
		"C4": "https://raw.githubusercontent.com/gleitz/midi-js-soundfonts/gh-pages/FatBoy/pad_2_warm-mp3/C4.mp3",
	},
	release: 1,
});

export default pad_2_warm;
//88-95 合成音色2 （温暖）