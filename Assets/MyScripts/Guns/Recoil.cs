using System.Collections;
using UnityEngine;

public class Recoil : MonoBehaviour
{
    [Header("Recoil_Transform")]
    public Transform RecoilPositionTranform;
    public Transform RecoilRotationTranform;
    [Space(10)]
    [Header("Recoil_Settings")]
    public float PositionDampTime;
    public float RotationDampTime;
    [Space(10)]
    public float Recoil1;
    public float Recoil2;
    public float Recoil3;
    public float Recoil4;
    [Space(10)]
    public Vector3 RecoilRotation;
    public Vector3 RecoilKickBack;
    [Space(10)]
    public Vector3 crouchPosition;
    public Vector3 crouchRotation;

    private Vector3 defaultPos;
    private Vector3 defaultRot;

    [Space(10)]
    public Vector3 RecoilRotation_Aim;
    public Vector3 RecoilKickBack_Aim;
    [Space(10)]
    public Vector3 CurrentRecoil1;
    public Vector3 CurrentRecoil2;
    public Vector3 CurrentRecoil3;
    public Vector3 CurrentRecoil4;

    private Vector3 RotationOutput;

    private PlayerController player;
    private WeaponScript weaponScript;

    public bool aim;

    private void Start()
    {
        player = FindObjectOfType<PlayerController>();
        weaponScript = GetComponentInParent<WeaponScript>();

        defaultPos = RecoilPositionTranform.localPosition;
        defaultRot = RecoilRotationTranform.localPosition;
    }

    void FixedUpdate()
    {
        CurrentRecoil1 = Vector3.Lerp(CurrentRecoil1, Vector3.zero, Recoil1 * Time.fixedDeltaTime);
        CurrentRecoil2 = Vector3.Lerp(CurrentRecoil2, CurrentRecoil1, Recoil2 * Time.fixedDeltaTime);
        CurrentRecoil3 = Vector3.Lerp(CurrentRecoil3, Vector3.zero, Recoil3 * Time.fixedDeltaTime);
        CurrentRecoil4 = Vector3.Lerp(CurrentRecoil4, CurrentRecoil3, Recoil4 * Time.fixedDeltaTime);

        RecoilPositionTranform.localPosition = Vector3.Slerp(RecoilPositionTranform.localPosition, CurrentRecoil3, PositionDampTime * Time.fixedDeltaTime);
        RotationOutput = Vector3.Slerp(RotationOutput, CurrentRecoil1, RotationDampTime * Time.fixedDeltaTime);
        RecoilRotationTranform.localRotation = Quaternion.Euler(RotationOutput);

        if (!weaponScript.isScoped)
            Crouch();
    }

    private void Crouch()
    {
        if (player.crouching)
        {
            RecoilPositionTranform.localPosition = Vector3.Lerp(RecoilPositionTranform.localPosition, crouchPosition, PositionDampTime * Time.fixedDeltaTime);

            Vector3 rot = Vector3.Slerp(RecoilRotationTranform.localPosition, crouchRotation, Time.fixedDeltaTime * RotationDampTime);
            RecoilRotationTranform.localRotation = Quaternion.Euler(rot);
        }
        else
        {
            RecoilPositionTranform.localPosition = Vector3.Lerp(RecoilPositionTranform.localPosition, defaultPos, PositionDampTime * Time.fixedDeltaTime);

            Vector3 rot = Vector3.Slerp(RecoilRotationTranform.localPosition, defaultRot, Time.fixedDeltaTime * RotationDampTime);
            RecoilRotationTranform.localRotation = Quaternion.Euler(rot);
        }
    }

    public void Fire()
    {
        if (aim)
        {
            CurrentRecoil1 += new Vector3(RecoilRotation_Aim.x, Random.Range(-RecoilRotation_Aim.y, RecoilRotation_Aim.y), Random.Range(-RecoilRotation_Aim.z, RecoilRotation_Aim.z));
            CurrentRecoil3 += new Vector3(Random.Range(-RecoilKickBack_Aim.x, RecoilKickBack_Aim.x), Random.Range(-RecoilKickBack_Aim.y, RecoilKickBack_Aim.y), RecoilKickBack_Aim.z);
        }
        if (!aim)
        {
            CurrentRecoil1 += new Vector3(RecoilRotation.x, Random.Range(-RecoilRotation.y, RecoilRotation.y), Random.Range(-RecoilRotation.z, RecoilRotation.z));
            CurrentRecoil3 += new Vector3(Random.Range(-RecoilKickBack.x, RecoilKickBack.x), Random.Range(-RecoilKickBack.y, RecoilKickBack.y), RecoilKickBack.z);
        }
    }
}
