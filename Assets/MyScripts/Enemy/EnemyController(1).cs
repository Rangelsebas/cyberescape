using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class EnemyController : MonoBehaviour
{
    [Header("Assignables")]
    public GameObject bullet;
    public Transform firepoint;
    public Transform gun;

    private float timeBtwShots;

    [Header("Stats")]
    public float timeBtwShotsValue = 2f;
    public float shootForce = 100f;
    public float upwardForce = 0f;
    public float minDistance = 20f;
    public float maxDistance = 30f;
    public float timeInSeconds = 2f;

    [Header("Agent Customization")]
    [Range(0f, 10f)]
    public float speed;
    [Range(0f, 20f)]
    public float acceleration;
    [Range(0f, 360f)]
    public float angularSpeed;
    [Range(0f, 8f)]
    public float stoppingDistance;

    private PlayerController player;
    private NavMeshAgent agent;
    private EnemyHealth health;

    private Vector3 startingPosition;
    private float distanceToPlayer;

    public bool isProvoke = false;

    private AudioSource sfx;
    public AudioClip shootSound;

    private void Start()
    {
        timeBtwShots = timeBtwShotsValue;
        player = FindObjectOfType<PlayerController>();
        agent = GetComponent<NavMeshAgent>();
        health = GetComponent<EnemyHealth>();
        sfx = GetComponentInChildren<AudioSource>();

        sfx.minDistance = minDistance;
        sfx.maxDistance = maxDistance;

        AgentVauesSetUp();

        startingPosition = transform.position;
    }

    private void AgentVauesSetUp()
    {
        agent.acceleration = acceleration;
        agent.speed = speed;
        agent.angularSpeed = angularSpeed;
        agent.stoppingDistance = stoppingDistance;
    }

    private void Update()
    {
        distanceToPlayer = Vector3.Distance(transform.position, player.GetPosition());

        if (distanceToPlayer <= minDistance) {
            isProvoke = true;
        }
        else if (distanceToPlayer >= maxDistance) {
            isProvoke = false;
        }

        if (isProvoke)
            ChasePlayer();

        if (!isProvoke)
            LosePlayer();

        if (health.IsDead())
        {
            enabled = false;
            agent.enabled = false;
            Destroy(gameObject);
        }
    }

    private void ChasePlayer()
    {
        if (distanceToPlayer <= stoppingDistance) {
            agent.SetDestination(transform.position);
        }
        else if (distanceToPlayer >= stoppingDistance) {
            agent.SetDestination(player.GetPosition());
        }

        timeBtwShots -= Time.deltaTime;
        if (timeBtwShots <= 0)
        {
            Shoot();
            timeBtwShots = timeBtwShotsValue;
        }

        FaceTarget();
        gun.LookAt(player.GetPosition());
    }

    private void LosePlayer()
    {
        agent.SetDestination(startingPosition);
    }

    private void Shoot() {

        GameObject currentBullet = Instantiate(bullet, firepoint.position, Quaternion.identity);
        currentBullet.transform.forward = player.FuturePos(timeInSeconds) * 100f;

        Rigidbody bulletRb = currentBullet.GetComponent<Rigidbody>();
        if (bulletRb != null)
        {
            bulletRb.AddForce(firepoint.forward * shootForce, ForceMode.Impulse);
            bulletRb.AddForce(firepoint.up * upwardForce, ForceMode.Impulse);
        }

        sfx.PlayOneShot(shootSound);
    }

    private void FaceTarget() {
        Vector3 dir = (player.transform.position - transform.position).normalized;
        Quaternion lookRot = Quaternion.LookRotation(new Vector3(dir.x, 0f, dir.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRot, Time.deltaTime * 5f);
    }

    public void OnDamageTaken()
    {
        isProvoke = true;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, minDistance);
    }
}
