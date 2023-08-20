using UnityEngine;
using UnityEngine.UI;

public class CrosshairCustomization : MonoBehaviour
{
    public Image reticle;
    public Image shadow;

    public Color reticleColor;
    public Color shadowColor;

    private void Start()
    {
        reticle.color = reticleColor;
        shadow.color = shadowColor;
    }

    private void Update()
    {
        reticle.color = reticleColor;
        shadow.color = shadowColor;
    }
}
