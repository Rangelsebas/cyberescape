using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class EnemyBullet : MonoBehaviour
{
    private Rigidbody rb;
    private EnemyHealth enemyHealth;

    public GameObject impactFX;

    public float lifeTime;
    public float damage;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        enemyHealth = FindObjectOfType<EnemyHealth>();
    }

    private void Update()
    {
        lifeTime -= Time.deltaTime;
        if (lifeTime <= 0)
        {
            Explode();
        }

        //maybe fix this later
        if (enemyHealth.currentHp <= 25f)
        {
            damage *= 1.25f;
        }
    }

    private void Explode()
    {
        GameObject impactGO = Instantiate(impactFX, transform.position, Quaternion.identity);
        Destroy(impactGO, 1f);

        Destroy(gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag == "Player")
        {
            HealthSystem healthSystem = collision.collider.GetComponent<HealthSystem>();
            if (healthSystem != null) { 
                healthSystem.DamagePlayer(damage);
            }
        }

        Explode();
    }
}
