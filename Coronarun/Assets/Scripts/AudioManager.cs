using UnityEngine;
using System;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
	public Sound[] sounds;

	public static AudioManager instance;
    
    void Awake()
    {
    	if (instance == null)
    	{
    		instance = this;
    	}
		else
		{
			Destroy(gameObject);
			return;
		}
        foreach(Sound s in sounds)
        {
        	DontDestroyOnLoad(gameObject);
        	s.source = gameObject.AddComponent<AudioSource>();
        	s.source.clip = s.clip;
        	s.source.volume = s.volume;
        	s.source.pitch = s.pitch;
        	s.source.loop = s.loop;
        	s.source.playOnAwake = false;
        }
    }

    public void updatePitch()
    {
    	var sources = GameObject.FindObjectsOfType<AudioSource>();
    	foreach(AudioSource s in sources)
        {
        	s.pitch = Mathf.Sqrt(Time.timeScale);
        }
    }

    public void Play(string name)
    {
    	Sound s = Array.Find(sounds, sound => sound.name == name);
    	if(s == null)
    		return;
    	s.source.Play();
    }
}
