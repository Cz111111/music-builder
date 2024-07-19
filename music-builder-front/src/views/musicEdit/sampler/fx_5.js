import * as Tone from "tone";
const fx_5 = new Tone.Sampler({
    urls: {
		"C4": "https://raw.githubusercontent.com/gleitz/midi-js-soundfonts/gh-pages/FatBoy/fx_5_brightness-mp3/C4.mp3",
	},
	release: 1,
});

export default fx_5;
//96-103 合成效果 5 明亮