import * as Tone from "tone";
const violin = new Tone.Sampler({
    urls: {
		"C4": "https://raw.githubusercontent.com/gleitz/midi-js-soundfonts/gh-pages/FatBoy/violin-mp3/C4.mp3",
	},
	release: 1,
});

export default violin;
//40-47 小提琴