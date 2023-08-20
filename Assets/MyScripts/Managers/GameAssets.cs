using UnityEngine;

public class GameAssets : MonoBehaviour
{
    private static GameAssets _i;

    public static GameAssets i;

    private void Awake()
    {
        i = this;
    }




    public Transform pfDamagePopup;
    public GameObject minimapHealthIcon;
}
