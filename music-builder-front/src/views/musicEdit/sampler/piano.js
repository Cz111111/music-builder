import * as Tone from "tone";
const piano = new Tone.Sampler({
    urls: {
		"C4": "https://raw.githubusercontent.com/gleitz/midi-js-soundfonts/gh-pages/FatBoy/bright_acoustic_piano-mp3/C4.mp3",
	},
	release: 1,
});

export default piano;
//0-7 104-111 120-127 大钢琴
