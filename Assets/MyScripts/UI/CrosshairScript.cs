using UnityEngine;

public class CrosshairScript : MonoBehaviour
{
    private PlayerController player;

    public float speed;
    public float shootSpeed;
    public float maxLenght;

    private Vector3 startScale;

    private void Start()
    {
        player = FindObjectOfType<PlayerController>();
        startScale = transform.localScale;
    }
    

    private void Update()
    {
        if (player.keyPressed.magnitude != 0f) {
            transform.localScale = Vector3.Lerp(transform.localScale, transform.localScale * 1.2f, Time.deltaTime * speed);
        }
        else {
            transform.localScale = Vector3.Lerp(transform.localScale, startScale, Time.deltaTime * speed);
        }

        transform.localScale = Vector3.ClampMagnitude(transform.localScale, maxLenght);
    }
}
