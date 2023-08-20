using UnityEngine;
using TMPro;
using EZCameraShake;
using System.Collections;

public class WeaponScript : MonoBehaviour
{
    public static WeaponScript Instance;

    public GameObject bullet;
    public float shootForce;
    public float upwardForce;

    [Header("Names")]
    public string shootSound;
    public string weaponScopedAnimName;

    [Header("Gun stats")]
    public float range;
    public float effectiveRange;
    public float fireRate;

    [Space(10)]
    public float spread;

    [Space(10)]
    public float reloadTime;
    public float upRecoil;
    public float sideRecoil;

    public float recoilForce;

    [Space(10)]
    public int magazine;
    public int bulletsPerTap;
    public float timeBtwShots;

    [HideInInspector] public int bulletsLeft;
    private int bulletsShot;
    private float normalSpread;
    
    [Header("Customization")]
    public bool smg;
    public bool canScope;
    public bool canUseScopeOverlay;
    public bool allowButtonHold;

    [Header("Reference")]
    public Camera fpsCam;
    public Transform firepoint;
    public ParticleSystem muzzleFlash;
    public TextMeshProUGUI ammoText;
    public Recoil recoil;
    public GameObject weaponCamera;

    private Animator anim;
    private Ray ray;

    private WeaponSettings weaponSettings;
    private AudioManager audioM;
    private PlayerController player;
    private CameraShaker camShake;
    private UIController UI;
    private CameraSettings cameraSettings;
    private Inventory inventory;
    private HealthSystem healthSystem;

    [HideInInspector] public bool shooting, readyToShoot = true, isReloading, reloading, scoping, isScoped = false;
    private bool allowInvoke = true;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        weaponSettings = FindObjectOfType<WeaponSettings>();
        audioM = FindObjectOfType<AudioManager>();
        player = FindObjectOfType<PlayerController>();
        anim = GetComponent<Animator>();
        UI = FindObjectOfType<UIController>();
        camShake = CameraShaker.Instance;
        cameraSettings = CameraSettings.Instance;
        inventory = FindObjectOfType<Inventory>();
        healthSystem = FindObjectOfType<HealthSystem>();
        
        bulletsLeft = magazine;
        normalSpread = spread;
    }

    private void Update()
    {
        if (healthSystem.PlayerIsDead()) return;

        if (inventory.inventoryActive) return;

        if (allowButtonHold)
            shooting = Input.GetMouseButton(0);
        else
            shooting = Input.GetMouseButtonDown(0);

        reloading = Input.GetKeyDown(KeyCode.R);
        scoping = Input.GetMouseButtonDown(1);

        if (reloading && bulletsLeft < magazine && !isReloading) Reload();
        if (readyToShoot && shooting && !isReloading && bulletsLeft <= 0) Reload();

        UpdateUI();

        //Change the fireRate
        if (Input.GetKeyDown(KeyCode.F) && !isScoped)
        {
            allowButtonHold = !allowButtonHold;
            audioM.PlayOneShot("ChangeFireRateSound");
        }

        float normalFOV = cameraSettings.FOV;
        float maxScopedSpeed = 10f;
        float reachedFOV = 50f;
        if (isScoped && canUseScopeOverlay)
            cameraSettings.SettFOV(reachedFOV, maxScopedSpeed);
        else
            cameraSettings.SettFOV(normalFOV, maxScopedSpeed);

        //Aim down sights
        if (scoping && canScope && !isReloading)
        {
            isScoped = !isScoped;
            PlayScopedAnimation(isScoped);

            if (isScoped && canUseScopeOverlay && !isReloading)
                StartCoroutine(UnScoped());
            else
                OnUnScoped();
        }

    }

    private void UpdateUI()
    {
        if (bulletsLeft == 0)
            UI.UpdateReloadText(true);
        else
            UI.UpdateReloadText(false);

        UI.SetReloadBar(this);
        UI.UpdateReloadFillImage(isReloading);

        ammoText.SetText(bulletsLeft / bulletsPerTap + " / " + magazine / bulletsPerTap);
    }

    private void FixedUpdate()
    {
        if (shooting && readyToShoot && bulletsLeft > 0 && !isReloading)
        {
            bulletsShot = 0;
            Shoot();
        }
    }

    private void Shoot()
    {
        readyToShoot = false;

        ray = fpsCam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
        RaycastHit hit;

        Vector3 targetPoint;

        if (Physics.Raycast(ray, out hit, range))
            targetPoint = hit.point;
        else
            targetPoint = ray.GetPoint(75);

        Vector3 dir = targetPoint - firepoint.position;

        float x = Random.Range(-spread, spread);
        float y = Random.Range(-spread, spread);
        float z = Random.Range(-spread, spread);

        Vector3 newDir = dir + new Vector3(x, y, z);

        GameObject currentBullet = Instantiate(bullet, firepoint.position, Quaternion.identity);

        currentBullet.transform.forward = newDir.normalized;

        Rigidbody bulletRb = currentBullet.GetComponent<Rigidbody>();
        if (bulletRb != null)
        {
            bulletRb.AddForce(newDir.normalized * shootForce, ForceMode.Impulse);
            bulletRb.AddForce(Vector3.up * upwardForce, ForceMode.Impulse);
        }

        Collider[] enemies = Physics.OverlapSphere(transform.position, effectiveRange, LayerMask.GetMask("Enemies"));
        foreach (Collider activeEnemies in enemies)
        {
            if (activeEnemies != null)
            {
                firepoint.LookAt(activeEnemies.transform.position);

                BulletScript bullet = currentBullet.GetComponent<BulletScript>();
                if (bullet != null)
                {
                    float f = bullet.explosionRange;
                    f = bullet.explosionRange * 2f;
                }
            }
        }

        muzzleFlash.Play();
        recoil.Fire();
        player.AddRecoil(upRecoil / 4, sideRecoil / 4);
        audioM.PlayOneShot(shootSound);

        bulletsLeft--;
        bulletsShot++;

        if (allowInvoke)
        {
            Invoke(nameof(ResetShot), fireRate);
            allowInvoke = false;

            player.rb.AddForce(-newDir.normalized * recoilForce, ForceMode.Impulse);
        }

        if (bulletsShot < bulletsPerTap && bulletsLeft > 0)
            Invoke(nameof(Shoot), timeBtwShots);
    }

    private void ResetShot()
    {
        readyToShoot = true;
        allowInvoke = true;
    }

    private void Reload()
    {
        isReloading = true;
        Invoke(nameof(PlayReloadSound), 0.35f);

        Invoke(nameof(ReloadFinish), reloadTime);
    }

    private void ReloadFinish()
    {
        bulletsLeft = magazine;
        isReloading = false;
    }

    private void PlayScopedAnimation(bool isScoped_)
    {
        anim.SetBool(weaponScopedAnimName, isScoped_);
    }

    private IEnumerator UnScoped()
    {
        yield return new WaitForSeconds(0.2f);

        UI.UpdateScopeOverlay(isScoped, this);
        weaponCamera.SetActive(false);

        firepoint.forward = ray.direction;

        //Reduce spread to increase accuracy
        spread = 0.5f;
    }

    private void OnUnScoped()
    {
        UI.UpdateScopeOverlay(isScoped, this);
        weaponCamera.SetActive(true);

        //Increase spred to reduce accuracy
        spread = normalSpread;
    }

    //Other
    private void PlayReloadSound()
    {
        audioM.PlayOneShot("GunsReloadSound");
    }

    public void PlayGunEquipSound()
    {
        audioM.PlayOneShot("GunEquipSound");
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, effectiveRange);
    }
}

