import * as Tone from "tone";
const lead_6_voice = new Tone.Sampler({
    urls: {
		"C4": "https://raw.githubusercontent.com/gleitz/midi-js-soundfonts/gh-pages/FatBoy/lead_6_voice-mp3/C4.mp3",
	},
	release: 1,
});

export default lead_6_voice;
//80-87  合成主音6（人声）