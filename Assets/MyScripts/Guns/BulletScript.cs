using UnityEngine;
using EZCameraShake;

[RequireComponent(typeof(Rigidbody))]
public class BulletScript : MonoBehaviour
{
    private Rigidbody rb;

    [Header("Stats")]
    public GameObject explosionFX;
    public LayerMask Enemies;
    [Range(0f, 10f)]
    public float explosionRange;
    public float damage;
    public float criticalDamage;
    public float explosionDelay;
    public float explosionForce;

    [Header("Customization")]
    [Range(0f, 1f)]
    public float bounciness;
    public float lifeTime;
    public bool explodeOnTouch = true;
    public bool useGravity;
    public bool canUseExplosionSound;
    public bool allowCameraShakeWhileExploding;

    public AudioClip explosionSound;

    private PhysicMaterial myMat;
    private bool isCriticalHit;

    private UIController UI;
    private AudioManager audioM;
    private AudioSource myPlayer;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        UI = FindObjectOfType<UIController>();
        audioM = FindObjectOfType<AudioManager>();
        myPlayer = GetComponent<AudioSource>();

        SetUp();

        rb.useGravity = useGravity;
    }

    private void SetUp()
    {
        myMat = new PhysicMaterial();
        myMat.bounciness = bounciness;
        myMat.frictionCombine = PhysicMaterialCombine.Minimum;
        myMat.bounceCombine = PhysicMaterialCombine.Maximum;

        GetComponent<SphereCollider>().material = myMat;
    }

    private void Update()
    {
        lifeTime -= Time.deltaTime;
        if (lifeTime <= 0)
            Explode();
    }

    private void Explode()
    {
        GameObject explosionGO = Instantiate(explosionFX, transform.position, Quaternion.identity);
        Destroy(explosionGO, 1f);

        Collider[] enemies = Physics.OverlapSphere(transform.position, explosionRange, Enemies);
        for (int i = 0; i < enemies.Length; i++)
        {
            EnemyHealth enemyHealth = enemies[i].GetComponent<EnemyHealth>();
            if (enemyHealth != null) {
                enemyHealth.DamageEnemy(damage);
            }

            audioM.PlayOneShot("HitSound");

            if (isCriticalHit) {
                damage = criticalDamage;
                //UI.UpdateCriticalSprite();
            }
            if (!isCriticalHit) {
                UI.UpdateHitSprite();
            }

            //Damage popup
            DamagePopup.Create(transform.position, damage, isCriticalHit);
        }

        if (allowCameraShakeWhileExploding)
        {
            CameraShaker.Instance.ShakeOnce(3f, 2.5f, 0.1f, 1f);
        }

        Destroy(gameObject, explosionDelay);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (canUseExplosionSound)
            myPlayer.PlayOneShot(explosionSound);

        Collider[] rigidbodies = Physics.OverlapSphere(transform.position, explosionRange, ~LayerMask.GetMask("Player"));
        foreach (Collider otherRigidbodies in rigidbodies)
        {
            Rigidbody activeRigidbodies = otherRigidbodies.GetComponent<Rigidbody>();
            if (activeRigidbodies != null)
            {
                float range = 20f;
                activeRigidbodies.AddExplosionForce(explosionForce, transform.position, range);
            }
        }

        //Headshot
        if (collision.collider.tag == "Head" && explodeOnTouch)
        {
            isCriticalHit = true;

            EnemyHealth enemyHealth = collision.collider.GetComponentInParent<EnemyHealth>();
            if (enemyHealth != null) {
                enemyHealth.DamageEnemy(criticalDamage);
            }
        }

        Explode();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRange);
    }
}
