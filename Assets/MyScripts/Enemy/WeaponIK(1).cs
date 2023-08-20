using UnityEngine;

public class WeaponIK : MonoBehaviour
{
    public Transform targetTransform;
    public Transform aimTransform;
    public Transform bone;

    public int iterations = 10;
    [Range(0, 1)]
    public float weight = 1.0f;

    public float angleLimit = 90.0f;
    public float distanceLimit = 1.5f;

    private void LateUpdate()
    {
        if (aimTransform == null) return;
        if (targetTransform == null) return;

        Vector3 targetPosition = GetTargetPosition();
        for(int i = 0; i < iterations; i++) {
            AimTarget(bone, targetPosition);
        }
    }

    Vector3 GetTargetPosition()
    {
        Vector3 targetDirection = targetTransform.position - aimTransform.position;
        Vector3 aimDirection = aimTransform.forward;
        float blendOut = 0.0f;

        float targetAngle = Vector3.Angle(targetDirection, aimDirection);
        if (targetAngle > angleLimit) {
            blendOut += (targetAngle - angleLimit) / 50.0f;
        }

        Vector3 direction = Vector3.Slerp(targetDirection, aimDirection, blendOut);
        return aimTransform.position + direction;
    }

    private void AimTarget(Transform bone, Vector3 targetPosition) {
        Vector3 aimDirection = aimTransform.forward;
        Vector3 targetDirection = targetPosition - aimTransform.position;
        Quaternion aimTowards = Quaternion.FromToRotation(aimDirection, targetDirection);
        Quaternion blendedRotation = Quaternion.Slerp(Quaternion.identity, aimTowards, weight);
        bone.rotation = blendedRotation * bone.rotation;
    }

    public void SetTargetTransform(Transform target)
    {
        targetTransform = target;
    }

    public void SetAimTransform(Transform aim)
    {
        aimTransform = aim;
    }
}
