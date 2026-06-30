using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;
    private AudioSource musicSource;
    private AudioSource ambientSource;
    [SerializeField]
    private GameObject SFXPrefab;

    private float musicVolume = 1f;
    private float sfxVolume = 1f;

    private AudioSource uiSource;
    void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        musicSource = gameObject.AddComponent<AudioSource>();
        ambientSource = gameObject.AddComponent<AudioSource>();

        uiSource = gameObject.AddComponent<AudioSource>();
        uiSource.spatialBlend = 0f;
    }

    public void PlayMusic(AudioClip _music, bool _loop = true)
    {
        musicSource.clip = _music;
        musicSource.volume = musicVolume;
        musicSource.loop = _loop;
        musicSource.Play();
    }

    public void StopMusic()
    {
        musicSource.Stop();
    }

    public void PlayAmbientSound(AudioClip _ambient)
    {
        ambientSource.clip = _ambient;
        ambientSource.volume = sfxVolume;
        ambientSource.Play();
    }

    public void StopAmbientSound()
    {
        ambientSource.Stop();
    }

    public void PlaySFX(AudioClip _sfx, Vector3 _position, float volume = 1f)
    {
        GameObject SFXClone = Instantiate(SFXPrefab, _position, Quaternion.identity);
        SFXClone.GetComponent<AudioSource>().clip = _sfx;
        SFXClone.GetComponent <AudioSource>().volume = sfxVolume;
        SFXClone.GetComponent<AudioSource>().volume = volume;
        SFXClone.GetComponent<AudioSource>().Play();
        Destroy(SFXClone, _sfx.length);
    }
    public void PlayUISFX(AudioClip clip)
    {
        uiSource.PlayOneShot(clip, sfxVolume);
    }

    public void SetMusicVolume(float _volume)
    {
        musicVolume = _volume;
        musicSource.volume = _volume;
    }
    public void SetSFXVolume(float _volume)
    {
        sfxVolume = _volume;
        ambientSource.volume = _volume;
    }
}