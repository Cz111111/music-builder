import * as Tone from "tone";
const acoustic_guitar = new Tone.Sampler({
    urls: {
		"C4": "https://raw.githubusercontent.com/gleitz/midi-js-soundfonts/gh-pages/FatBoy/acoustic_guitar_steel-mp3/C4.mp3",
	},
	release: 1,
});

export default acoustic_guitar;
//24-31  钢弦吉他