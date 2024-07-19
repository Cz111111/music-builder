import * as Tone from "tone";
const trombone = new Tone.Sampler({
    urls: {
		"C4": "https://raw.githubusercontent.com/gleitz/midi-js-soundfonts/gh-pages/FatBoy/trombone-mp3/C4.mp3",
	},
	release: 1,
});

export default trombone;
//56-63  长号