using UnityEngine;

public class TimeManager : MonoBehaviour
{
    public static TimeManager Instance { get; private set; }

    public float slowmoTime = 0f;

    public bool isInSlowmo = false;
    private bool slowmo;

    private GameManager GM;
    private SlowMotionBar slowmoBar;
    private AudioManager audioM;
    private Inventory inventory;

    private void Awake()
    {
        Instance = this;

        audioM = FindObjectOfType<AudioManager>();
        GM = FindObjectOfType<GameManager>();
        slowmoBar = FindObjectOfType<SlowMotionBar>();
        inventory = FindObjectOfType<Inventory>();
    }

    bool end = false;
    private void Update()
    {
        /*if (GM.gameIsPaused)
            Time.timeScale = 0f;
        else
            Time.timeScale = 1f;*/

        if (inventory.inventoryActive /*&& !GM.gameIsPaused*/)
        { 
            Time.timeScale = Mathf.Lerp(Time.timeScale, 0.2f, Time.time * 6f);
            foreach (var activeAudios in GameObject.FindGameObjectsWithTag("Audio"))
            {
                activeAudios.GetComponent<AudioSource>().pitch = 0.8f;
            }
        }
        else
        { 
            Time.timeScale = Mathf.Lerp(Time.timeScale, 1f, Time.time * 6f);
            foreach (var activeAudios in GameObject.FindGameObjectsWithTag("Audio"))
            {
                activeAudios.GetComponent<AudioSource>().pitch = audioM.sPitches;
            }
        }

        if (slowmo)
        {
            foreach (var activeAudios in GameObject.FindGameObjectsWithTag("Audio"))
            {
                activeAudios.GetComponent<AudioSource>().pitch = 0.8f;
            }
        }
        else
        {
            foreach (var activeAudios in GameObject.FindGameObjectsWithTag("Audio"))
            {
                activeAudios.GetComponent<AudioSource>().pitch = audioM.sPitches;
            }
        }


        //Slow mo
        if (Input.GetKeyDown(KeyCode.Q))
        {
            isInSlowmo = !isInSlowmo;

            if (isInSlowmo)
                DoSlowmotion();
            else
                StopSlowmotion();
        }

        /*if (!GM.gameIsPaused)
        {
            
        }*/

        if (slowmo)
            {
                slowmoBar.bar.fillAmount -= Time.deltaTime * 0.42f;
                if (slowmoBar.bar.fillAmount <= 0f)
                {
                    StopSlowmotion();
                    end = true;
                }
            }

            if (end)
            {
                slowmoBar.bar.fillAmount += Time.deltaTime * .15f;
                if (slowmoBar.bar.fillAmount >= 1f)
                {
                    end = false;
                }
            }
    }

    private void DoSlowmotion()
    {
        slowmo = true;

        Time.timeScale = 0.4f;
        Time.fixedDeltaTime = 0.02f * Time.timeScale;

        PPSettings.Instance.PulseSlowmotion();

        SetPlayer();

        void SetPlayer()
        {
            PlayerController player = PlayerController.Instance;
            PlayerSettings settings = PlayerSettings.Instance;

            player.jumpForce = 1350f;
            settings.sensitivity = settings.slowmoSensitivity;
        }
    }

    private void StopSlowmotion()
    {
        slowmo = false;

        Time.timeScale = 1f;
        Time.fixedDeltaTime = 0.02f * Time.timeScale;

        PPSettings.Instance.StopSlowmotion();

        SetPlayer();

        void SetPlayer()
        {
            PlayerController player = PlayerController.Instance;
            PlayerSettings settings = PlayerSettings.Instance;

            player.jumpForce = 550f;
            settings.sensitivity = settings.normalSensitivity;
        }
    }
}
