using UnityEngine;
using System.Collections.Generic;

public class ControladorSonidos : MonoBehaviour
{
    public static ControladorSonidos Instance;
    public enum ModePlay { play, playOneShoot }

    [Header("🎚 Configuración de pool")]
    public int initialPoolSize = 1;
    public int maxPoolSize = 10;

    [Header("🔇 Control desde el editor")]
    [SerializeField] bool muteFromEditor = false;

    private Queue<AudioSource> audioSources;
    private Dictionary<AudioSource, float> originalVolumes = new Dictionary<AudioSource, float>();
    private bool isMuted = false;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        audioSources = new Queue<AudioSource>();
        for (int i = 0; i < initialPoolSize; i++)
        {
            CreateNewAudioSource();
        }
    }

    void Update()
    {
        // 👀 Detecta si cambió el booleano en el editor
        if (muteFromEditor && !isMuted)
        {
            MuteAll();
        }
        else if (!muteFromEditor && isMuted)
        {
            RestoreVolume();
        }
    }

    public void PlaySoundAtPosition(AudioClip clip, Vector3 position, ModePlay mode, bool stopActualClip, float volume)
    {
        if (clip == null) return;
        AudioSource source = null;

        if (stopActualClip)
        {
            if (mode == ModePlay.play)
            {
                source = StopAudioSourceWithClip(clip);
            }

            if (source == null)
            {
                source = GetAvailableAudioSource();
            }
        }
        else
        {
            source = GetAvailableAudioSource();
        }

        source.clip = clip;
        source.transform.position = position;
        source.volume = isMuted ? 0 : volume;
        if (mode == ModePlay.playOneShoot) source.PlayOneShot(clip);
        else source.Play();
    }

    public void PlaySoundGlobal(AudioClip clip, ModePlay mode, bool stopActualClip, float volume)
    {
        if (stopActualClip)
        {
            AudioSource source = StopAudioSourceWithClip(clip);
            if (source != null)
            {
                PlaySoundAtPosition(source, clip, transform.position, mode, volume);
            }
            else
            {
                PlaySoundAtPosition(clip, transform.position, mode, false, volume);
            }
        }
        else
        {
            PlaySoundAtPosition(clip, transform.position, mode, false, volume);
        }
    }

    public void PlaySoundGlobal(AudioClip clipOriginal, AudioClip newClip, ModePlay mode, bool stopActualClip, float volume)
    {
        if (stopActualClip)
        {
            StopAudioSourceWithClip(clipOriginal);
            StopAudioSourceWithClip(newClip);
            PlaySoundAtPosition(newClip, transform.position, mode, true, volume);
        }
        else
        {
            PlaySoundAtPosition(newClip, transform.position, mode, stopActualClip, volume);
        }
    }

    private void PlaySoundAtPosition(AudioSource source, AudioClip clip, Vector3 position, ModePlay mode, float volume)
    {
        if (clip == null) return;

        source.Stop();
        source.clip = clip;
        source.transform.position = position;
        source.volume = isMuted ? 0 : volume;

        if (mode == ModePlay.playOneShoot) source.PlayOneShot(clip);
        else source.Play();
    }

    public void SetVolumen(AudioClip clip, float pasoVolumen)
    {
        Queue<AudioSource> updatedQueue = new Queue<AudioSource>();

        while (audioSources.Count > 0)
        {
            AudioSource source = audioSources.Dequeue();
            if (source.isPlaying && source.clip.name == clip.name)
            {
                source.volume = Mathf.Clamp01(source.volume + pasoVolumen);
            }
            updatedQueue.Enqueue(source);
        }

        audioSources = updatedQueue;
    }

    public AudioSource StopAudioSourceWithClip(AudioClip clip)
    {
        Queue<AudioSource> updatedQueue = new Queue<AudioSource>();
        AudioSource sourceToreturn = null;

        while (audioSources.Count > 0)
        {
            AudioSource source = audioSources.Dequeue();

            if (source.isPlaying && source.clip.name == clip.name)
            {
                source.Stop();
                sourceToreturn = source;
            }

            updatedQueue.Enqueue(source);
        }

        audioSources = updatedQueue;
        return sourceToreturn;
    }

    private AudioSource GetAvailableAudioSource()
    {
        foreach (AudioSource source in audioSources)
        {
            if (!source.isPlaying)
            {
                return source;
            }
        }

        if (audioSources.Count < maxPoolSize)
        {
            return CreateNewAudioSource();
        }

        AudioSource oldestSource = audioSources.Dequeue();
        oldestSource.Stop();
        audioSources.Enqueue(oldestSource);
        return oldestSource;
    }

    private AudioSource CreateNewAudioSource()
    {
        AudioSource newSource = gameObject.AddComponent<AudioSource>();
        newSource.volume = 0.7f;
        newSource.playOnAwake = false;
        audioSources.Enqueue(newSource);
        return newSource;
    }

    public void MuteAll()
    {
        if (isMuted) return;
        originalVolumes.Clear();

        foreach (AudioSource source in audioSources)
        {
            originalVolumes[source] = source.volume;
            source.volume = 0f;
        }

        isMuted = true;
    }

    public void RestoreVolume()
    {
        if (!isMuted) return;

        foreach (AudioSource source in audioSources)
        {
            if (originalVolumes.ContainsKey(source))
            {
                source.volume = originalVolumes[source];
            }
        }

        isMuted = false;
    }
}
