import * as Tone from "tone";
const dulcimer = new Tone.Sampler({
    urls: {
		"C4": "https://raw.githubusercontent.com/gleitz/midi-js-soundfonts/gh-pages/FatBoy/dulcimer-mp3/C4.mp3",
	},
	release: 1,
});

export default dulcimer;
//8-15 大扬琴