using UnityEngine;

public class IconFollower : MonoBehaviour
{
    public Transform targetToFollow;

    private void LateUpdate()
    {
        transform.position = targetToFollow.position;
    }
}
