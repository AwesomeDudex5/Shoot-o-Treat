using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Game_Controller : MonoBehaviour
{
    public int numberOfWaves;
    private int currentWave;
    //number of enemies per wave, add more each round
    private int previousCount;
    public int enemyCount;
    public int enemiesAlive;

    //enemy types
    public GameObject[] enemyPrefab;

    //spawners
    public GameObject[] spawners;

    //Text Holders
    public Text waveNumber;
    public Text numberOfEnemies;
    //public Text playerHealth;

    //player object info
    public GameObject player;
    private Vector3 previousPlayerPosition;
    private Quaternion previousPlayerRotation;

    //play state
    public bool isPlaying;
    public bool canPause; //to enable pause UI
    public bool restarting; //to signal to gun script and restock ammo
    public GameObject mainCamera;
    public GameObject gun;
    public GameObject pauseUI;
    public GameObject gameplayUI;
    public GameObject gameoverUI;

    //Options UI
    public Slider sensitivitySlider;
    public Text sensitivityText;

    // Start is called before the first frame update
    void Start()
    {
        previousPlayerPosition = player.transform.position;
        previousPlayerRotation = player.transform.rotation;
        previousCount = enemyCount;
        sensitivitySlider.value = mainCamera.GetComponent<Camera_Controller>().sensitivity;
        sensitivityText.text = "Mouse Sensitivity: " + Mathf.Round(sensitivitySlider.value);
        isPlaying = false;
        restarting = false;
        canPause = false;
        enemiesAlive = 0;
        currentWave = 0;
        PauseGame();
    }

    // Update is called once per frame
    void Update()
    {

        if (!isPlaying)
        {
            return;
        }

        restarting = false;

        //get escape key input
        //pauses/resumes game
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!canPause)
            {
                StartGame();
                pauseUI.SetActive(false);
                gameplayUI.SetActive(true);
            }
            else
            {
                PauseGame();
                Cursor.lockState = CursorLockMode.None;
                pauseUI.SetActive(true);
                gameplayUI.SetActive(false);
            }
        }

        if (player.GetComponent<PlayerInfo>().isDead == true)
        {
            isPlaying = false;
            canPause = false;
            PauseGame();
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            mainCamera.GetComponent<Camera_Controller>().enabled = false;
            player.GetComponent<Player_Movement>().enabled = false;
            gun.SetActive(false);
            gameoverUI.SetActive(true);
            gameplayUI.SetActive(false);
        }




       if (enemiesAlive == 0)
            {
                currentWave++;
            enemyCount += 2;
                enemiesAlive = enemyCount;

                waveNumber.text = "Wave: " + currentWave;
                numberOfEnemies.text = "Enemies Left: "+ enemiesAlive;

                for (int i = 0; i < enemyCount; i++)
                {
                    int randomIndexSpawner = Random.Range(0, spawners.Length);
                    int randomIndexEnemy = Random.Range(0, enemyPrefab.Length);

                    //get a random enemy prefab and random spawn location, then spawn enemy
                    spawners[randomIndexSpawner].GetComponent<Spawn_Script>().spawnEnemy(enemyPrefab[randomIndexEnemy]);
                }
            }
            
        
    }

    public void enablePlayState()
    {
        isPlaying = true;
    }

    public void StartGame()
    {
        player.transform.rotation = previousPlayerRotation;

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        mainCamera.GetComponent<Camera_Controller>().enabled = true;
        player.GetComponent<Player_Movement>().enabled = true;
        gun.SetActive(true);
        Debug.Log("Resumes the Game");
        Time.timeScale = 1.0f;
        canPause = true;
    }

    public void PauseGame()
    {
        Cursor.visible = true;
        mainCamera.GetComponent<Camera_Controller>().enabled = false;
        player.GetComponent<Player_Movement>().enabled = false;
        gun.SetActive(false);
        Debug.Log("Paused the Game");
        Time.timeScale = 0f;
        canPause = false;
    }

    public void RestartGame()
    {
        //restart player position and health
        player.transform.position = previousPlayerPosition;
        player.transform.rotation = previousPlayerRotation;
        player.GetComponent<PlayerInfo>().resetHealth();

        gun.GetComponent<Gun_Controller>().resetAmmo();

        //destroy enemies in scene
        GameObject[] enemiesInScene;
        enemiesInScene = GameObject.FindGameObjectsWithTag("Enemy");
        foreach(GameObject Enemy in enemiesInScene)
        {
            Destroy(Enemy);
        }

        restarting = true;

        //reset wave
        currentWave = 0;
        enemiesAlive = 0;
        enemyCount = previousCount;

        enablePlayState();
        StartGame();
    }

    public void CloseGame()
    {
        Application.Quit();
    }

    public void ApplySensitivityChange()
    {
        mainCamera.GetComponent<Camera_Controller>().changeMouseSpeed(Mathf.Round(sensitivitySlider.value));
        sensitivityText.text = "Mouse Sensitivity: " + Mathf.Round(sensitivitySlider.value);
    }

}
