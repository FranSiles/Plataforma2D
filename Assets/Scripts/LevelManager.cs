using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    //Singleton for ask the info of this script without getting the component
    //accesing directly to the Instance
    public static LevelManager Instance;

    [Header("Panels")]
    [SerializeField] private GameObject gameOverPanel; //reference to the GameOver Panel
    [SerializeField] private GameObject winLevelPanel; //reference to the winLevel Panel

    [Space]
    [Tooltip("Level time in seconds")]
    [Range(1f, 300f)]
    [SerializeField] private float levelTime;

    private float internalLevelTime;
    public float InternalLevelTime { get => internalLevelTime; set => internalLevelTime = value; }

    //Variables to manage Powerups in the level : total, current and remaining
    private int totalLevelPowerUps;

    private int currentPlayerPowerUps;
    public int CurrentPlayerPowerUps { get => currentPlayerPowerUps; set => currentPlayerPowerUps = value; }

    private int remainingPowerUps;
    public int RemainingPowerUps { get => remainingPowerUps; set => remainingPowerUps = value; }

    PlayerHealth playerHealth;

    bool endLevel = false;


    private void Awake()
    {
        //initialize 
        endLevel = false;

        //create the singleton
        Instance = this;

        //Initiatly Disable the Panels
        gameOverPanel.SetActive(false);
        winLevelPanel.SetActive(false);

        //restart timeScale
        Time.timeScale = 1;

        //initially total level Time
        internalLevelTime = levelTime;

    }

    private void Start()
    {
        totalLevelPowerUps = GameObject.FindGameObjectsWithTag("Enemy").Length;
        playerHealth = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerHealth>();
        remainingPowerUps = totalLevelPowerUps;

    }

    private void Update()
    {
        if (!endLevel && (currentPlayerPowerUps >= totalLevelPowerUps))
        {
            endLevel = true;
            WinLevel();
        }

        //Manage Time
        internalLevelTime -= Time.deltaTime;

        //if the time is out --> GameOVER
        if (internalLevelTime <= 0.9f) GameOver();

        //Update of powerups
        remainingPowerUps = totalLevelPowerUps - currentPlayerPowerUps;

    }

    /// <summary>
    /// Activates the Game Over Panel and pauses the game
    /// </summary>
    public void GameOver()
    {

        //Activate the Game Over Panel
        gameOverPanel.SetActive(true);

        //Pause Game
        Time.timeScale = 0;
    }

    /// <summary>
    /// Activates the Win Level Panel and pauses the game
    /// </summary>
    public void WinLevel()
    {
        //add remaining time and player health to player points
        if (GameManager.Instance != null)
        {
            //time
            GameManager.Instance.PlayerPoints += (int)internalLevelTime * 100;
            //health
            GameManager.Instance.PlayerPoints += playerHealth.Health * 250;
        }

        //Activate the Panel
        winLevelPanel.SetActive(true);

        //Pause Game
        Time.timeScale = 0;
    }



    /// <summary>
    /// Changes the scene to Main Menu Scene
    /// </summary>
    /// <param name="nameScene"></param>
    public void MainMenuScene()
    {
        CheckHighScore();

        CheckMaxLevel();

        //Initialize Player Points to 0 for new Game
        GameManager.Instance.PlayerPoints = 0;


        SceneManager.LoadScene(GameConstants.MAINMENU_LEVEL);
    }

    /// <summary>
    /// Reload current Scene
    /// </summary>
    public void ReloadScene()
    {
        CheckHighScore();

        CheckMaxLevel();

        //Initialize Player Points to 0 for new Game
        GameManager.Instance.PlayerPoints = 0;


        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    /// <summary>
    /// Go to Next Level
    /// </summary>
    public void NextLevel()
    {
        CheckMaxLevel();



        //total number of levels
        int levelCount = SceneManager.sceneCountInBuildSettings - 1;
        //If there are more levels after this level
        if (SceneManager.GetActiveScene().buildIndex < levelCount)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
        //This is the last level
        else
        {
            CheckHighScore();

            //Initialize Player Points to 0 for new Game
            GameManager.Instance.PlayerPoints = 0;
        }
    }

    /// <summary>
    /// Check Max Level
    /// </summary>
    void CheckMaxLevel()
    {
        //ask for the current level
        int currentLevel = int.Parse(SceneManager.GetActiveScene().name.Substring(5));
        //Save the next Level for the first time
        if (!PlayerPrefs.HasKey(GameConstants.MAXLEVEL_KEY))
        {
            PlayerPrefs.SetInt(GameConstants.MAXLEVEL_KEY, currentLevel + 1);
        }
        else
        {
            if (PlayerPrefs.GetInt(GameConstants.MAXLEVEL_KEY) < currentLevel + 1)
            {
                PlayerPrefs.SetInt(GameConstants.MAXLEVEL_KEY, currentLevel + 1);
            }

        }
    }

    private void CheckHighScore()
    {
        //Check if previous high score saved
        if (PlayerPrefs.HasKey(GameConstants.HIGHSCORE_KEY))
        {
            //Get the High Score Value saved
            int highScore = PlayerPrefs.GetInt(GameConstants.HIGHSCORE_KEY);

            //check if the playerPoints is highet than the High Scored saved
            if (GameManager.Instance.PlayerPoints > highScore)
            {
                PlayerPrefs.SetInt(GameConstants.HIGHSCORE_KEY, GameManager.Instance.PlayerPoints);
            }

        }
        //First time played
        else
        {
            //Save first time Player Points
            PlayerPrefs.SetInt(GameConstants.HIGHSCORE_KEY, GameManager.Instance.PlayerPoints);
        }
    }
}
