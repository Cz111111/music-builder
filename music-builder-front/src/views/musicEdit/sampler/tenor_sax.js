import * as Tone from "tone";
const tenor_sax = new Tone.Sampler({
    urls: {
		"C4": "https://raw.githubusercontent.com/gleitz/midi-js-soundfonts/gh-pages/FatBoy/tenor_sax-mp3/C4.mp3",
	},
	release: 1,
});

export default tenor_sax;
//64-71  中音萨克斯风