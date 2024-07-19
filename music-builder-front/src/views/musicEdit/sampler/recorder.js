import * as Tone from "tone";
const recorder = new Tone.Sampler({
    urls: {
		"C4": "https://raw.githubusercontent.com/gleitz/midi-js-soundfonts/gh-pages/FatBoy/recorder-mp3/C4.mp3",
	},
	release: 1,
});

export default recorder;
//72-79 竖笛