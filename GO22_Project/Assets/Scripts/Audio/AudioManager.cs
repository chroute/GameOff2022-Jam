using UnityEngine.Audio;
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class AudioManager : MonoBehaviour
{

	public static AudioManager Instance;

	public Sound[] sounds;
	private Dictionary<string, Sound> soundDict = new Dictionary<string, Sound>();

	void Awake()
	{
		if (Instance != null)
		{
			Destroy(gameObject);
		}
		else
		{
			Instance = this;
			DontDestroyOnLoad(gameObject);
		}

		foreach (Sound s in sounds)
		{
			s.InitializeAudioSoure(gameObject.AddComponent<AudioSource>());
			soundDict.Add(s.name, s);
		}
	}

	public void Play(string sound)
	{
		Sound s = soundDict[sound];
		if (s == null)
		{
			Debug.LogWarning("Sound: " + name + " not found!");
			return;
		}

		if (s.delayInSeconds > 0)
		{
			StartCoroutine(PlaySound(s, s.delayInSeconds));
		}
		else
		{
			s.source.Play();
		}
		
	}

	private IEnumerator PlaySound(Sound s, float delayInSeconds)
	{
		yield return new WaitForSeconds(delayInSeconds);
		s.source.Play();
	}

}
