using TMPro;
using UnityEngine;

public class DamagePopup : MonoBehaviour
{
    public static DamagePopup Create(Vector3 position, float damageAmount, bool isCriticalHit)
    {
        Transform damagePopupTransform = Instantiate(GameAssets.i.pfDamagePopup, position, Quaternion.identity);

        DamagePopup damagePopup = damagePopupTransform.GetComponent<DamagePopup>();
        damagePopup.SetUp(damageAmount, isCriticalHit);

        return damagePopup;
    }

    private int sortingOrder;

    private const float DISAPPEAR_TIMER_MAX = 1f;

    private TextMeshPro textMesh;
    private float disappearTimer;
    private Color textColor;
    private Vector3 moveVector;

    private PlayerController player;

    private void Awake()
    {
        textMesh = transform.GetComponent<TextMeshPro>();
        player = FindObjectOfType<PlayerController>();
    }

    private void Update()
    {
        transform.position += moveVector * Time.deltaTime;
        moveVector -= moveVector * 8f * Time.deltaTime;

        if (disappearTimer > DISAPPEAR_TIMER_MAX * 0.5f) {
            float increaseScaleAmount = 1f;
            transform.localScale += transform.localScale * increaseScaleAmount * Time.deltaTime;
        }
        else {
            float decreaseScaleAmount = 1f;
            transform.localScale -= transform.localScale * decreaseScaleAmount * Time.deltaTime;
        }

        disappearTimer -= Time.deltaTime;
        if (disappearTimer < 0) {
            float disappearSpeed = 3f;
            textColor.a -= disappearSpeed * Time.deltaTime;
            textMesh.color = textColor;
            if (textColor.a < 0) {
                Destroy(gameObject);
            }
        }

        //Always look at player
        transform.LookAt(transform.position + player.playerCam.forward);
    }

    public void SetUp(float damageAmount, bool isCriticalHit ) {
        textMesh.SetText(damageAmount.ToString());

        if (!isCriticalHit) {
            textMesh.fontSize = 40;
            textColor = Color.yellow;
        }
        else {
            textMesh.fontSize = 50;
            textColor = Color.red;
        }

        textMesh.color = textColor;
        disappearTimer = DISAPPEAR_TIMER_MAX;

        sortingOrder++;
        textMesh.sortingOrder = sortingOrder;

        moveVector = new Vector3(.3f, 1) * 30f;
    }
}
