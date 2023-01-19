using UnityEngine.Audio;
using System;
using UnityEngine;

[System.Serializable]
public class Sound
{
    public string name;

    public AudioClip clip;

    public AudioMixerGroup mixerGroup;

    [Range(0, 1)]
    public float volume;
    [Range(.1f, 3)]
    public float pitch;
    [Range(0f, 10f)]
    public float speed;

    [HideInInspector]
    public AudioSource source;
}

public class SoundManager : MonoBehaviour
{
    public Sound[] sounds;
    private AudioSource[] audioSources;
    public AudioMixerGroup SFXMixer;
    public AudioMixerGroup MasterMixer;
    public AudioMixerGroup MusicMixer;
    // Start is called before the first frame update

    void Awake()
    {
        int x = 0;
        foreach(Sound s in sounds)
        {
            x++;
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.outputAudioMixerGroup = s.mixerGroup;
            Array.Resize(ref audioSources, x);
            audioSources[audioSources.Length - 1] = s.source;
        }
    }

    public AudioSource Play(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            return null;
        }
        s.source.Play();
        return s.source;
    }

    public AudioSource Play(string name, bool loop)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            return null;
        }
        s.source.Play();
        s.source.loop = loop;
        return s.source;
    }
    public void StopPlaying(AudioSource source)
    {
        source.Pause();
        source.time = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
