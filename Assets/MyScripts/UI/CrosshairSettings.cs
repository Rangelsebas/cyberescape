using UnityEngine;
using UnityEngine.UI;

public class CrosshairSettings : MonoBehaviour
{
    public static CrosshairSettings Instance { get; private set;}

    private void Awake()
    {
        Instance = this;
    }

    public GameObject crosshair;
    public GameObject reticle;
    public GameObject aimCrosshair;
    [Space(10)]
    public GameObject crosshairAutoChage;
    public GameObject reticleAutoChange;
    [Space(10)]
    public WeaponScript sniper;
    public WeaponScript pistol;
    public WeaponScript shotgun;
    public WeaponScript SMG;
    public WeaponScript GL;
    public WeaponScript famas;
    public WeaponScript banana;
    public WeaponScript rifle;
    [Space(10)]
    public Color aimCrosshairColor;
    [Space(10)]
    public Image reticle_1;
    public Image reticle_2;

    private PlayerSettings playerSettings;

    private void Start()
    {
        crosshair.SetActive(true);
        reticle.SetActive(false);
        aimCrosshair.SetActive(false);

        playerSettings = FindObjectOfType<PlayerSettings>();
    }

    private void Update()
    {
        if (playerSettings.useReticle)
        {
            reticle.SetActive(true);
            crosshair.SetActive(false);
        }
        else
        {
            reticle.SetActive(false);
            crosshair.SetActive(true);
        }

        WeaponScript[] weapons = FindObjectsOfType<WeaponScript>();
        for (int i = 0; i < weapons.Length; i++)
        {
            if (playerSettings.useReticle)
            {
                reticleAutoChange.SetActive(weapons[i].allowButtonHold);
                crosshairAutoChage.SetActive(false);
            }
            else
            {
                crosshairAutoChage.SetActive(weapons[i].allowButtonHold);
                reticleAutoChange.SetActive(false);
            }
        }

        //if scoping, disbale the current crosshair
        if (sniper.isScoped || pistol.isScoped || shotgun.isScoped || SMG.isScoped || GL.isScoped || famas.isScoped || banana.isScoped || rifle.isScoped)
        {
            aimCrosshair.SetActive(true);

            crosshair.SetActive(false);
            reticle.SetActive(false);
        }
        else
        {
            aimCrosshair.SetActive(false);

            if (playerSettings.useReticle)
            {
                reticle.SetActive(true);
                crosshair.SetActive(false);
            }
            else
            {
                crosshair.SetActive(true);
                reticle.SetActive(false);
            }
        }

        reticle_1.color = aimCrosshairColor;
        reticle_2.color = aimCrosshairColor;
    }
}
