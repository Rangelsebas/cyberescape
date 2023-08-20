using UnityEngine;

public class FPSArmsSettings : MonoBehaviour
{
    public static FPSArmsSettings Instance;

    [Header("Tilt Position")]
    public float posAmount = 0.02f;
    public float maxPosAmount = 0.06f;
    public float smoothPosAmount = 4f;

    [Header("Tilt Rotation")]
    public float rotAmount = 2f;
    public float maxRotAmount = 3f;
    public float smoothRotAmount = 10f;

    [Header("Bobbing")]
	public float bobSpeed = 0.13f;
    public float bobDistance = 0.15f;
    public float spread;

    public float jerkAmount;
    public float speedAmount;

    private float horizontal, vertical, timer, waveSlice;
    private Vector3 midPoint;

    [Space]
    public bool rotX;
    public bool rotY;
    public bool rotZ;

    public Vector3 shortGunPos;
    public Vector3 mediumGunPos;
    public Vector3 longGunPos;

    private Vector3 initialPos;
    private Quaternion initialRot;

    private Inventory inventory;
    private PlayerController player;
    public WeaponScript pistol, assaultRifle, SMG, sniper, shotgun, grenadeLauncher, semiAuto, banana;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        initialPos = transform.localPosition;
        initialRot = transform.localRotation;

        midPoint = transform.localPosition;

        player = FindObjectOfType<PlayerController>();
        inventory = FindObjectOfType<Inventory>();
    }

    private void LateUpdate()
    {
        if (inventory.inventoryActive) return;

        float inputX, inputY;

        CalculateSway(out inputX, out inputY);
        MoveSway(inputX, inputY);
        TiltSway(inputX, inputY);
        Bobbing();
        SetGunsReloadPosition();
    }

    private void SetGunsReloadPosition()
    {
        if (pistol.isReloading || SMG.isReloading || banana.isReloading)
        {
            transform.localPosition = Vector3.Lerp(transform.localPosition, shortGunPos, Time.deltaTime * 3f);
        }
        if (assaultRifle.isReloading || shotgun.isReloading || grenadeLauncher.isReloading || semiAuto.isReloading)
        {
            transform.localPosition = Vector3.Lerp(transform.localPosition, mediumGunPos, Time.deltaTime * 3f);
        }
        if (sniper.isReloading)
        {
            transform.localPosition = Vector3.Lerp(transform.localPosition, longGunPos, Time.deltaTime * 3f);
        }
    }

    private void Bobbing()
    {
        if (pistol.isScoped || assaultRifle.isScoped || SMG.isScoped || sniper.isScoped || shotgun.isScoped || grenadeLauncher.isScoped || semiAuto.isScoped || banana.isScoped) return;

        float xMove = player.rb.velocity.x, zMove = player.rb.velocity.z;

        horizontal = xMove;
        vertical = zMove;

        Vector3 localPosition = transform.localPosition;

        if (Mathf.Abs(horizontal) == 0 && Mathf.Abs(vertical) == 0)
        {
            timer = 0.0f;
        }
        else
        {
            waveSlice = Mathf.Sin(timer);
            timer = timer + bobSpeed;
            if (timer > Mathf.PI * 2)
            {
                timer = timer - (Mathf.PI * 2);
            }
        }

        if (waveSlice != 0)
        {
            float translateChange = waveSlice * bobDistance;
            float totalAxes = Mathf.Abs(horizontal) + Mathf.Abs(vertical);
            totalAxes = Mathf.Clamp(totalAxes, 0.0f, 1.0f);
            translateChange = totalAxes * translateChange;

            localPosition.y = midPoint.y + translateChange;
            localPosition.x = midPoint.x + translateChange * 2f;
        }
        else
        {
            localPosition.y = midPoint.y;
            localPosition.x = midPoint.x;
        }

        localPosition = ArmsJerk(localPosition);

        transform.localPosition = Vector3.Lerp(transform.localPosition, localPosition, Time.deltaTime * speedAmount);
    }

    private Vector3 ArmsJerk(Vector3 pos)
    {
        if (!player.readyToJump && !player.grounded)
        {
            Vector3 jerkPos = new Vector3(0, -0.4f, 0);

            float translateChanges = jerkPos.y * jerkAmount * speedAmount;
            float totalAxes = Mathf.Abs(horizontal) + Mathf.Abs(vertical);
            totalAxes = Mathf.Clamp(totalAxes, 0.0f, 1.0f);
            pos.y = translateChanges + jerkPos.y;
        }
        else
        {
            pos.y = midPoint.y;
        }

        return pos;
    }

    private void MoveSway(float inputX, float inputY)
    {
        float xPos = Mathf.Clamp(inputX * posAmount, -maxPosAmount, maxPosAmount);
        float yPos = Mathf.Clamp(inputY * posAmount, -maxPosAmount, maxPosAmount);

        Vector3 finalPos = new Vector3(xPos, yPos, 0);

        transform.localPosition = Vector3.Lerp(transform.localPosition, finalPos + initialPos, Time.deltaTime * smoothPosAmount);
    }

    private void TiltSway(float inputX, float inputY)
    {
        float tiltY = Mathf.Clamp(inputX * rotAmount, -maxRotAmount, maxRotAmount);
        float tiltX = Mathf.Clamp(inputY * rotAmount, -maxRotAmount, maxRotAmount);

        Quaternion finalRot = Quaternion.Euler(new Vector3(
            rotX ? -tiltX: 0f,
            rotY ? tiltY: 0f,
            rotZ ? tiltY: 0f
            ));

        transform.localRotation = Quaternion.Slerp(transform.localRotation, finalRot * initialRot, Time.deltaTime * smoothRotAmount);
    }

    private static void CalculateSway(out float inputX, out float inputY)
    {
        inputX = -Input.GetAxis("Mouse X");
        inputY = -Input.GetAxis("Mouse Y");
    }
}
