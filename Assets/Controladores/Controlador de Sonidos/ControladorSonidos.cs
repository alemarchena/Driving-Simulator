using UnityEngine;
using System.Collections.Generic;

public class ControladorSonidos : MonoBehaviour
{
    public static ControladorSonidos Instance;
    public enum ModePlay { play,playOneShoot}
     
    public int initialPoolSize = 1; 
    public int maxPoolSize = 10;  
    private Queue<AudioSource> audioSources; 

    void Awake()
    {
        // Implementar singleton
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        // Crear el pool inicial de AudioSources
        audioSources = new Queue<AudioSource>();
        for (int i = 0; i < initialPoolSize; i++)
        {
            CreateNewAudioSource();
        }
    }

    /// <summary>
    /// Reproduce un sonido en una posición específica x,y,z, buscando un "Nuevo" AudioSource disponible.
    /// </summary>
    /// 
    public void PlaySoundAtPosition(AudioClip clip, Vector3 position, ModePlay mode, bool stopActualClip)
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

        if(mode == ModePlay.playOneShoot) source.PlayOneShot(clip);
        else source.Play();
    }

    /// <summary>
    /// StopActualClip = true : utiliza un mismo AudioSource para el clip de sonido
    /// StopActualClip = false : utiliza un nuevo AudioSource para el clip de sonido
    /// </summary>
    /// <param name="clip"></param>
    /// <param name="mode"></param>
    /// <param name="stopActualClip"></param>
    public void PlaySoundGlobal(AudioClip clip,ModePlay mode,bool stopActualClip)
    {
        if (stopActualClip)
        {
            AudioSource source = StopAudioSourceWithClip(clip);
            if (source != null)
            {
                PlaySoundAtPosition(source, clip, transform.position, mode);
            }
            else
            {
                PlaySoundAtPosition(clip, transform.position, mode, false);
            }
        }
        else
        {
            PlaySoundAtPosition(clip, transform.position, mode,false);
        }
    }

    /// <summary>
    /// Detiene un AudioClip inicial y ejecuta un nuevo audio
    /// </summary>
    /// <param name="clipOriginal"></param>
    /// <param name="newClip"></param>
    /// <param name="mode"></param>
    /// <param name="stopActualClip"></param>
    public void PlaySoundGlobal(AudioClip clipOriginal, 
                                AudioClip newClip, 
                                ModePlay mode, 
                                bool stopActualClip)
    {
        if (stopActualClip)
        {
            StopAudioSourceWithClip(clipOriginal);
            StopAudioSourceWithClip(newClip);
            PlaySoundAtPosition(newClip, transform.position, mode,true);
            
        }
        else
        {
            PlaySoundAtPosition(newClip, transform.position, mode, stopActualClip);
        }
    }

    /// <summary>
    /// Reproduce un sonido en el AudioSource especificado
    /// </summary>
    /// <param name="source"></param>
    /// <param name="clip"></param>
    /// <param name="position"></param>
    /// <param name="mode"></param>
    private void PlaySoundAtPosition(AudioSource source, AudioClip clip, Vector3 position, ModePlay mode)
    {
        if (clip == null) return;

        source.Stop();
        source.clip = clip;
        source.transform.position = position;

        if (mode == ModePlay.playOneShoot) source.PlayOneShot(clip);
        else source.Play();
    }



    /// <summary>
    /// Busca un AudioSource que esté reproduciendo el mismo clip y lo detiene.
    /// </summary>
    public AudioSource StopAudioSourceWithClip(AudioClip clip)
    {
        // Usamos una cola temporal para mantener el orden
        Queue<AudioSource> updatedQueue = new Queue<AudioSource>();
        AudioSource sourceToreturn = null;

        while (audioSources.Count > 0)
        {
            AudioSource source = audioSources.Dequeue();

            // Si está reproduciendo y el clip es el mismo
            if (source.isPlaying && source.clip.name == clip.name)
            {
                source.Stop();
                sourceToreturn = source;
            }

            updatedQueue.Enqueue(source);
        }

        // Restaurar el orden original
        audioSources = updatedQueue;
        return sourceToreturn;
    }

    /// <summary>
    /// Busca un AudioSource disponible, o reutiliza el más antiguo si el pool ha alcanzado el límite.
    /// </summary>
    /// <returns>Un AudioSource listo para reproducir</returns>
    private AudioSource GetAvailableAudioSource()
    {
        foreach (AudioSource source in audioSources)
        {
            if (!source.isPlaying)
            {
                return source; // Retorna el primero disponible
            }
        }

        // Si todos están ocupados y el tamaño máximo no se ha alcanzado, crea uno nuevo
        if (audioSources.Count < maxPoolSize)
        {
            return CreateNewAudioSource();
        }

        // Si el pool está lleno, reutiliza el más antiguo (FIFO)
        AudioSource oldestSource = audioSources.Dequeue();
        oldestSource.Stop();
        audioSources.Enqueue(oldestSource); // Reinsertar al final de la cola
        return oldestSource;
    }

    /// <summary>
    /// Crea un nuevo AudioSource, lo agrega al pool y retorna.
    /// </summary>
    private AudioSource CreateNewAudioSource()
    {
        AudioSource newSource = gameObject.AddComponent<AudioSource>();
        newSource.volume = 0.7f;
        newSource.playOnAwake = false;
        audioSources.Enqueue(newSource);
        return newSource;
    }
}
