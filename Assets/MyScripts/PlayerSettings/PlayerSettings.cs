using UnityEngine;

public class PlayerSettings : MonoBehaviour
{
    public static PlayerSettings Instance { get; private set;}

    [Range(0f, 1f)]
    public float crouchHeight;
    [Range(0, 100)]
    public float sensitivity;
    [Range(100f, 500f)]
    public float slowmoSensitivity;
    [Range(0, 10)]
    public float sensMultiplier;
    [Range(1f, 2f)]
    public float playerScale;

    [Header("Bools")]
    public bool invertXAxis;
    public bool invertYAxis;
    public bool toggleInventory;
    [Space]
    public bool useReticle;
    public bool damagedPanel;
    public bool healthBar;
    public bool minimap;
    public bool dashBar;
    public bool slowmoBar;
    [Space]
    public bool gameMessages;
    public bool platformNames;

    [HideInInspector] public float normalSensitivity;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        normalSensitivity = sensitivity;
    }
}
