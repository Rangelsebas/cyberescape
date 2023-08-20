using UnityEngine;

public class Headshot : MonoBehaviour
{
    private float hp;

    private EnemyHealth enemyHealth;
    public AudioSource sfx;
    public AudioClip headshotSound;

    private void Start()
    {
        enemyHealth = GetComponentInParent<EnemyHealth>();

        hp = enemyHealth.hp;
    }

    public void HeadshotDamage(float headshotAmount)
    {
        hp -= headshotAmount;

        if (hp <= 0f)
        {
            sfx.PlayOneShot(headshotSound);
            Destroy(gameObject);
        }
    }
}
