using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.AI;

public class EnemyHealth : MonoBehaviour
{
    public static EnemyHealth Instance { get; private set;}

    public event EventHandler OnDamaged;

    public float hp;
    [HideInInspector] public float currentHp;

    public Image bar, bg;
    public GameObject healthBarHandle;

    private bool isDead = false;

    private NavMeshAgent agent;
    private EnemyAI enemyAI;

    public GameObject explosioFX;
    public AudioClip explosionSound;
    private AudioSource sfx;
    private MeshRenderer mesh;

    private void Start()
    {
        currentHp = hp;
        sfx = GetComponentInChildren<AudioSource>();
        agent = GetComponent<NavMeshAgent>();
        enemyAI = GetComponent<EnemyAI>();
        mesh = GetComponent<MeshRenderer>();

        healthBarHandle.SetActive(false);
        bar.fillAmount = 1.0f;

        OnDamaged += EnemyHealthSystem_OnDamaged;
    }

    private void Update()
    {
        SetHealth(GetNormalizedHealth());

        if (isDead) enabled = false;
    }

    public void DamageEnemy(float damageAmount)
    {
        currentHp -= damageAmount;

        if (currentHp <= 0) {
            Die();
        }

        if (OnDamaged != null) OnDamaged(this, EventArgs.Empty);
    }

    private void Die()
    {
        healthBarHandle.SetActive(false);
        bar.gameObject.SetActive(false);
        bg.gameObject.SetActive(false);
        agent.enabled = false;
        enabled = false;
        enemyAI.enabled = false;
        mesh.enabled = false;

        if (isDead) return;
        isDead = true;

        if (explosioFX != null) {
            GameObject explosionGO = Instantiate(explosioFX, transform.position, Quaternion.identity);
            Destroy(explosionGO, 1f);
        }

        sfx.PlayOneShot(explosionSound);
        Destroy(gameObject, 1f);
    }

    public bool IsDead()
    {
        return isDead;
    }

    private void EnemyHealthSystem_OnDamaged(object sender, EventArgs e)
    {
        healthBarHandle.SetActive(true);

        SetHealth(GetNormalizedHealth());
    }

    private void SetHealth(float normalizedValue)
    {
        bar.fillAmount = normalizedValue;
    }

    private float GetNormalizedHealth()
    {
        return currentHp / hp;
    }
}
