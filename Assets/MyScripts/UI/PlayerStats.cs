using TMPro;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public static PlayerStats Instance { get; private set;}

    private PlayerController player;
    private HealthSystem healthSystem;
    private SlowMotionBar slowMotionBar;
    private DashBar dashBar;

    public TextMeshProUGUI[] stats;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        player = FindObjectOfType<PlayerController>();
        healthSystem = FindObjectOfType<HealthSystem>();
        slowMotionBar = FindObjectOfType<SlowMotionBar>();
        dashBar = FindObjectOfType<DashBar>();
    }

    private void Update()
    {
        for (int i = 0; i < stats.Length; i++)
        {
            if (stats[i].name == "Healthtext") {
                stats[i].SetText("Health: " + healthSystem.currentHp.ToString());
            }

            if (stats[i].name == "SpeedText") {
                stats[i].SetText("Speed: " + player.rb.velocity.magnitude.ToString("f2"));
            }

            if (stats[i].name == "Slow-mo") {
                stats[i].SetText("Slow-mo: " + slowMotionBar.bar.fillAmount.ToString("f2"));
            }

            if (stats[i].name == "Dash") {
                stats[i].SetText("Dash: " + dashBar.bar.fillAmount.ToString("f2"));
            }
        }
    }
}
