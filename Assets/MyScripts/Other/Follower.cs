using UnityEngine;

public class Follower : MonoBehaviour
{
    public AudioClip explosionSound;
    public EnemyHealth enemyHealth;

    private AudioSource sfx;

    private void Start()
    {
        sfx = GetComponent<AudioSource>();
    }

    private void Update()
    {
        transform.position = enemyHealth.transform.position;

        if (enemyHealth.IsDead())
        {
            sfx.PlayOneShot(explosionSound);
        }
    }
}
