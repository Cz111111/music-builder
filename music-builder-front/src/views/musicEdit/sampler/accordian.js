import * as Tone from "tone";
const accordian = new Tone.Sampler({
    urls: {
		"C4": "https://raw.githubusercontent.com/gleitz/midi-js-soundfonts/gh-pages/FatBoy/accordion-mp3/C4.mp3",
	},
	release: 1,
});

export default accordian;
//16-23 手风琴