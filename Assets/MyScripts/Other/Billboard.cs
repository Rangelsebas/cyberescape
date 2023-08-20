using TMPro;
using UnityEngine;

public class Billboard : MonoBehaviour
{
    private PlayerController player;
    private PlayerSettings playerSettings;
    private TextMeshProUGUI text;
    public float range;

    [HideInInspector] public bool isInRange = false;

    private void Awake()
    {
        player = FindObjectOfType<PlayerController>();
        text = GetComponent<TextMeshProUGUI>();
        playerSettings = FindObjectOfType<PlayerSettings>();
    }

    private void Start()
    {
        if (text != null)
            text.color = new Color(text.color.r, text.color.g, text.color.b, 0f);
    }

    private void Update()
    {
        if (playerSettings.platformNames)
        {
            transform.LookAt(transform.position + player.playerCam.forward);

            if (text != null)
            { 
                if (Vector3.Distance(transform.position, player.GetPosition()) < range)
                {
                    text.color = new Color(text.color.r, text.color.g, text.color.b, Mathf.MoveTowards(text.color.a, 1f, Time.deltaTime * 6f));

                    isInRange = true;
                }
                else
                {
                    text.color = new Color(text.color.r, text.color.g, text.color.b, Mathf.MoveTowards(text.color.a, 0f, Time.deltaTime * 6f));

                    isInRange = false;
                }
            }
        }
        else
        {
            text.color = new Color(text.color.r, text.color.g, text.color.b, 0f);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, range);
    }
}
