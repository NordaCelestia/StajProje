using UnityEngine;

public class SFX : MonoBehaviour
{
    public static SFX Instance { get; private set; }

    [SerializeField] private AudioClip[] audioClips; 
    private AudioSource[] audioSources; 

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        
        audioSources = new AudioSource[audioClips.Length];
        for (int i = 0; i < audioClips.Length; i++)
        {
            audioSources[i] = gameObject.AddComponent<AudioSource>();
            audioSources[i].clip = audioClips[i];
        }
    }

    public void PlaySound(int i)
    {
        if (i >= 0 && i < audioSources.Length)
        {
            audioSources[i].Play();
        }
        else
        {
            Debug.LogWarning("Invalid audio source index");
        }
    }
}
