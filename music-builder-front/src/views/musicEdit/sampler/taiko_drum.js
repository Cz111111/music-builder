import * as Tone from "tone";
const taiko_drum = new Tone.Sampler({
    urls: {
		"C4": "https://raw.githubusercontent.com/gleitz/midi-js-soundfonts/gh-pages/FatBoy/taiko_drum-mp3/C4.mp3",
	},
	release: 1,
});

export default taiko_drum
//112-119 太鼓