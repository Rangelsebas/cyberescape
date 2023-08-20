using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    private PlayerController player;
    private AudioManager audioM;
    private UIController UI;
    private Inventory inventory;
    private HealthSystem healthSystem;

    public string mainMenuScene;
    public string currentSceneName;

    [HideInInspector]
    public bool levelEnding;

    //public bool gameIsPaused;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        audioM = FindObjectOfType<AudioManager>();
        player = FindObjectOfType<PlayerController>();
        UI = FindObjectOfType<UIController>();
        inventory = FindObjectOfType<Inventory>();
        healthSystem = FindObjectOfType<HealthSystem>();

        Rigidbody[] rigidbodies = FindObjectsOfType<Rigidbody>();
        for (int i = 0; i < rigidbodies.Length; i++)
        {
            if (rigidbodies != null)
            {
                rigidbodies[i].detectCollisions = true;
            }
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            player.transform.position = player.spawPoint.position;
        }

        if (healthSystem.PlayerIsDead())
            StartCoroutine(RestartDelayed());

        if (Input.GetKeyDown(KeyCode.Escape) && !inventory.inventoryActive)
        {
            QuitToMainMenu();

            UI.SetActivePauseMenu(true);

            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            WeaponScript[] weapons = FindObjectsOfType<WeaponScript>();
            for (int i = 0; i < weapons.Length; i++)
            {
                weapons[i].enabled = false;
            }

            EnemyController[] enemies = FindObjectsOfType<EnemyController>();
            for (int i = 0; i < enemies.Length; i++)
            {
                enemies[i].enabled = false;
            }

            NavMeshAgent[] navMeshes = FindObjectsOfType<NavMeshAgent>();
            for (int i = 0; i < navMeshes.Length; i++)
            {
                navMeshes[i].enabled = false;
            }

            Rigidbody[] rigidbodies = FindObjectsOfType<Rigidbody>();
            for (int i = 0; i < rigidbodies.Length; i++)
            {
                if (rigidbodies != null)
                {
                    rigidbodies[i].isKinematic = true;
                }
            }

            ParticleSystem[] particleSystems = FindObjectsOfType<ParticleSystem>();
            for (int i = 0; i < particleSystems.Length; i++)
            {
                particleSystems[i].Pause();
            }

            audioM.PauseSounds();

            player.enabled = false;
            inventory.enabled = false;
            FPSArmsSettings.Instance.enabled = false;
            AudioManager.Instance.enabled = false;
            PPSettings.Instance.enabled = false;
            HealthBar.Instance.enabled = false;
        }

        /*if (Input.GetKeyDown(KeyCode.Escape) && !inventory.inventoryActive)
        {
            PauseUnPause();
        }*/
    }

    /*public void PauseUnPause()
    {
        if (!gameIsPaused)
        {
            PauseGame();
        }
        else
        {
            UnPauseGame();
        }
    }*/

    public IEnumerator LoadMain()
    {
        UI.loadingScreen.SetActive(true);

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(mainMenuScene);

        asyncLoad.allowSceneActivation = false;

        while (!asyncLoad.isDone)
        {
            if (asyncLoad.progress >= 0.9f)
            {
                UI.loadingText.gameObject.SetActive(false);
                UI.loadingIcon.SetActive(false);

                UI.loadingMessage.gameObject.SetActive(true);
                UI.loadingMessage.text = "PRESS ANY KEY TO CONTINUE";

                if (Input.anyKeyDown)
                {
                    asyncLoad.allowSceneActivation = true;

                    Time.timeScale = 1f;
                }
            }

            yield return null;
        }
    }

    IEnumerator RestartDelayed()
    {
        yield return new WaitForSeconds(3f);
        RestartLevel(currentSceneName);
    }

    private void RestartLevel(string levelName)
    {
        SceneManager.LoadScene(levelName);
    }

    /*public void PauseGame()
    {
        UI.SetActivePauseMenu(true);
        //gameIsPaused = true;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        WeaponScript[] weapons = FindObjectsOfType<WeaponScript>();
        for (int i = 0; i < weapons.Length; i++)
        {
            weapons[i].enabled = false;
        }

        EnemyController[] enemies = FindObjectsOfType<EnemyController>();
        for (int i = 0; i < enemies.Length; i++)
        {
            enemies[i].enabled = false;
        }

        NavMeshAgent[] navMeshes = FindObjectsOfType<NavMeshAgent>();
        for (int i = 0; i < navMeshes.Length; i++)
        {
            navMeshes[i].enabled = false;
        }

        Rigidbody[] rigidbodies = FindObjectsOfType<Rigidbody>();
        for (int i = 0; i < rigidbodies.Length; i++)
        {
            if (rigidbodies != null)
            {
                rigidbodies[i].isKinematic = true;
            }
        }

        ParticleSystem[] particleSystems = FindObjectsOfType<ParticleSystem>();
        for (int i = 0; i < particleSystems.Length; i++)
        {
            particleSystems[i].Pause();
        }

        audioM.PauseSounds();

        player.enabled = false;
        FPSArmsSettings.Instance.enabled = false;
        AudioManager.Instance.enabled = false;
        PPSettings.Instance.enabled = false;
        HealthBar.Instance.enabled = false;
    }
    public void UnPauseGame()
    {
        UI.SetActivePauseMenu(false);
        //gameIsPaused = false;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        WeaponScript[] weapons = FindObjectsOfType<WeaponScript>();
        for (int i = 0; i < weapons.Length; i++)
        {
            weapons[i].enabled = true;
        }

        EnemyController[] enemies = FindObjectsOfType<EnemyController>();
        for (int i = 0; i < enemies.Length; i++)
        {
            enemies[i].enabled = true;
        }

        NavMeshAgent[] navMeshes = FindObjectsOfType<NavMeshAgent>();
        for (int i = 0; i < navMeshes.Length; i++)
        {
            navMeshes[i].enabled = true;
        }

        Rigidbody[] rigidbodies = FindObjectsOfType<Rigidbody>();
        for (int i = 0; i < rigidbodies.Length; i++)
        {
            if (rigidbodies != null)
            {
                rigidbodies[i].isKinematic = false;
            }
        }

        ParticleSystem[] particleSystems = FindObjectsOfType<ParticleSystem>();
        for (int i = 0; i < particleSystems.Length; i++)
        {
            particleSystems[i].Play();
        }

        audioM.UnPauseSounds();

        player.enabled = true;
        FPSArmsSettings.Instance.enabled = true;
        AudioManager.Instance.enabled = true;
        PPSettings.Instance.enabled = true;
        HealthBar.Instance.enabled = true;
    }*/

    /*public void OpenOptionsMenu()
    {
        UI.optionsMenu.SetActive(true);
        UI.pauseMenu.SetActive(false);
    }

    public void CloseOptionsMenu()
    {
        UI.optionsMenu.SetActive(false);
        UI.pauseMenu.SetActive(true);
    }*/

    public void QuitToMainMenu()
    {
        StartCoroutine(LoadMain());
    }

    public void SetPlayer(PlayerController player) {
        this.player = player;
    }
}
