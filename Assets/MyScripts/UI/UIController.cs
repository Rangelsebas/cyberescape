using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public static UIController Instance;

    [Space(10)]
    public Image hitSprite;
    public Image criticalSprite;
    public Image reloadFillImage;
    public Image deathPanel;
    public Image hitPanel;
    public Image blackScreen;
    [Space(10)]
    public GameObject scopeOverlay;
    public GameObject reloadTextPanel;
    public GameObject pauseMenu;
    public GameObject minimap;
    public GameObject dashBar;
    public GameObject slowmoBar;
    public GameObject healthBar;
    public GameObject[] healthBarComponents;

    [Space(10)]
    public float normalSpeed;
    public float criticalSpeed;
    [Space(10)]
    public GameObject loadingScreen;
    public GameObject loadingIcon;
    public TextMeshProUGUI loadingText;
    public TextMeshProUGUI loadingMessage;

    private bool isScoped;
    private bool isReloading_;

    private float reloadSpeed;

    private float reloadTime = 0f;
    private float fadeSpeed = 1.5f;

    private bool normal;
    private Vector3 normalDefaultScale, criticalDefaultScale;

    private WeaponScript weaponScript;
    private WeaponWheel weaponWheel;
    private HealthSystem healthSystem;
    private PlayerSettings playerSettings;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        SetUp();

        weaponScript = FindObjectOfType<WeaponScript>();
        weaponWheel = FindObjectOfType<WeaponWheel>();
        healthSystem = FindObjectOfType<HealthSystem>();
        playerSettings = FindObjectOfType<PlayerSettings>();
    }

    private void SetUp()
    {
        hitSprite.color = new Color(hitSprite.color.r, hitSprite.color.g, hitSprite.color.b, 0f);
        criticalSprite.color = new Color(criticalSprite.color.r, criticalSprite.color.g, criticalSprite.color.b, 0f);
        deathPanel.color = new Color(deathPanel.color.r, deathPanel.color.g, deathPanel.color.b, 0f);
        hitPanel.color = new Color(hitPanel.color.r, hitPanel.color.g, hitPanel.color.b, 0f);

        scopeOverlay.SetActive(false);
        reloadTextPanel.SetActive(false);

        reloadFillImage.gameObject.SetActive(false);
        reloadFillImage.fillAmount = 1.0f;

        normalDefaultScale = hitSprite.transform.localScale;
        criticalDefaultScale = criticalSprite.transform.localScale;

        pauseMenu.SetActive(false);

    }
    private void Update()
    {
        if (!GameManager.Instance.levelEnding)
        {
            blackScreen.color = new Color(blackScreen.color.r, blackScreen.color.g, blackScreen.color.b, Mathf.MoveTowards(blackScreen.color.a, 0f, fadeSpeed * Time.deltaTime));
        }
        else
        {
            blackScreen.color = new Color(blackScreen.color.r, blackScreen.color.g, blackScreen.color.b, Mathf.MoveTowards(blackScreen.color.a, 1f, fadeSpeed * Time.deltaTime));
        }

        if (normal)
        {
            hitSprite.color = new Color(hitSprite.color.r, hitSprite.color.g, hitSprite.color.b, Mathf.MoveTowards(hitSprite.color.a, 1f, normalSpeed * Time.deltaTime));

            //Scale the normal hitmarker
            hitSprite.transform.localScale = Vector3.Lerp(hitSprite.transform.localScale, hitSprite.transform.localScale * 1.5f, Time.deltaTime * 5f);

            //Clamping the magnitude
            hitSprite.transform.localScale = Vector3.ClampMagnitude(hitSprite.transform.localScale, 3f);
        }
        else
        {
            hitSprite.color = new Color(hitSprite.color.r, hitSprite.color.g, hitSprite.color.b, Mathf.MoveTowards(hitSprite.color.a, 0f, normalSpeed * Time.deltaTime));

            //Return back to normal hitmarker scale
            hitSprite.transform.localScale = Vector3.Lerp(hitSprite.transform.localScale, normalDefaultScale, Time.deltaTime * 5f);
        }

        scopeOverlay.SetActive(isScoped);

        //if Player is dead show death panel
        if (healthSystem.PlayerIsDead())
        {
            deathPanel.color = new Color(deathPanel.color.r, deathPanel.color.g, deathPanel.color.b, Mathf.MoveTowards(deathPanel.color.a, 1f, Time.deltaTime * 3f));
        }

        //HUD Options
        if (playerSettings.healthBar)
        {
            for (int i = 0; i < healthBarComponents.Length; i++)
            {
                healthBarComponents[i].SetActive(true);
            }
        }
        else
        {
            for (int i = 0; i < healthBarComponents.Length; i++)
            {
                healthBarComponents[i].SetActive(false);
            }
        }

        if (playerSettings.minimap) minimap.SetActive(true);
        else minimap.SetActive(false);

        if (playerSettings.dashBar) dashBar.SetActive(true);
        else dashBar.SetActive(false);

        if (playerSettings.slowmoBar) slowmoBar.SetActive(true);
        else slowmoBar.SetActive(false);
    }

    public void SetActivePauseMenu(bool state)
    {
        pauseMenu.SetActive(state);
    }

    public void SetReloadBar(WeaponScript weapon)
    {
        weaponScript = weapon;
        float f = weaponScript.reloadTime;

        if (weaponScript.reloading && weaponScript.bulletsLeft < weaponScript.magazine && reloadTime <= 0f)
        {
            reloadTime = f;

            reloadFillImage.gameObject.SetActive(true);
        }

        if (reloadTime > 0f)
        {
            reloadFillImage.gameObject.SetActive(true);

            reloadTime -= Time.deltaTime;
            SetValue(100 - reloadTime / f * 100f);
        }
        else
        {
            reloadFillImage.gameObject.SetActive(false);
        }

        void SetValue(float amount)
        {
            reloadFillImage.fillAmount = amount / 100f;
        }
    }

    //Normal
    public void UpdateHitSprite()
    {
        normal = true;
        Invoke(nameof(Disable), 0.3f);
    }

    void Disable()
    {
        normal = false;
    }

    public void UpdateReloadFillImage(bool state)
    {
        isReloading_ = state;
    }

    public void UpdateScopeOverlay(bool state, WeaponScript weaponScript_)
    {
        weaponScript = weaponScript_;

        if (weaponScript_.canUseScopeOverlay)
            isScoped = state;
    }

    public void UpdateReloadText(bool state)
    {
        reloadTextPanel.SetActive(state);
    }

    public void DamageTaken()
    {
        if (playerSettings.damagedPanel) { 
            Animator anim = hitPanel.GetComponent<Animator>();
            if (anim != null) {
                anim.SetTrigger("FadeIn");
            }
        }
    }

    public void PingPongHealth()
    {
        if (playerSettings.damagedPanel) { 
            Animator anim = hitPanel.GetComponent<Animator>();
            if (anim != null) anim.enabled = false;

            hitPanel.color = new Color(hitPanel.color.r, hitPanel.color.g, hitPanel.color.b, Mathf.PingPong(Time.time, 0.45f) * 0.5f);
        }
    }

    public void StopPingPongHealth()
    {
        if (playerSettings.damagedPanel) { 
            Animator anim = hitPanel.GetComponent<Animator>();
            if (anim != null) anim.enabled = true;

            hitPanel.color = new Color(hitPanel.color.r, hitPanel.color.g, hitPanel.color.b, 0f);
        }
    }
}
