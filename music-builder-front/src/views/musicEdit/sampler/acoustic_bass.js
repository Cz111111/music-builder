import * as Tone from "tone";
const acoustic_bass = new Tone.Sampler({
    urls: {
		"C4": "https://raw.githubusercontent.com/gleitz/midi-js-soundfonts/gh-pages/FatBoy/acoustic_bass-mp3/C4.mp3",
	},
	release: 1,
});

export default acoustic_bass;
//32-39 大贝司