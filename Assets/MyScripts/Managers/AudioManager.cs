using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    public float sPitches;

    public AudioSource[] SFX;
    public AudioSource bgMusic;
    private PlayerController player;
    private HealthSystem healthSystem;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        player = FindObjectOfType<PlayerController>();
        healthSystem = FindObjectOfType<HealthSystem>();

        //Set up pitches
        foreach (var activeAudios in GameObject.FindGameObjectsWithTag("Audio"))
        {
            sPitches = activeAudios.GetComponent<AudioSource>().pitch;
        }

        //Set all pitches at 1
        foreach (var activeAudios in GameObject.FindGameObjectsWithTag("Audio"))
        {
            activeAudios.GetComponent<AudioSource>().pitch = 1f;
        }
        GameObject.Find("GunEquipSound").GetComponent<AudioSource>().pitch = 1.2f;
    }

    private void Update()
    {
        transform.position = player.transform.position;

        if (healthSystem.PlayerIsDead())
        {
            bgMusic.Stop();
        }
    }

    public void PlayOneShot(string name)
    {
        for (int i = 0; i < SFX.Length; i++)
        {
            if (name == SFX[i].name)
            {
                SFX[i].Play();
            }
        }
    }

    public void StopOneShot(string name)
    {
        for (int i = 0; i < SFX.Length; i++)
        {
            if (name == SFX[i].name)
            {
                SFX[i].Stop();
            }
        }
    }

    public void PauseSounds()
    {
        foreach (var sfx in GameObject.FindGameObjectsWithTag("Audio"))
        {
            sfx.GetComponent<AudioSource>().Pause();
        }
    }

    public void UnPauseSounds()
    {
        foreach (var sfx in GameObject.FindGameObjectsWithTag("Audio"))
        {
            sfx.GetComponent<AudioSource>().UnPause();
        }
    }
}
