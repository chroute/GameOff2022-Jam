using UnityEngine.Audio;
using UnityEngine;

[System.Serializable]
public class Sound {

	public string name;
	public AudioClip clip;
	public AudioMixerGroup mixer;

	[Range(0f, 1f)]
	public float volume = .75f;

	[Range(.1f, 3f)]
	public float pitch = 1f;

	public bool loop = false;

	public float delayInSeconds = 0f;


	[HideInInspector]
	public AudioSource source;

	public void InitializeAudioSoure(AudioSource audioSource) {
		source = audioSource;
		source.clip = clip;
		source.volume = volume;
		source.loop = loop;
		source.pitch = pitch;
		source.outputAudioMixerGroup = mixer;
	}
}
