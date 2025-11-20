using TMPro;
using UnityEngine;

public class HUDController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI healthText;
    [SerializeField] private TextMeshProUGUI enemyText;
    [SerializeField] private TextMeshProUGUI timeText;
    [SerializeField] private TextMeshProUGUI winPointsText;
    [SerializeField] private TextMeshProUGUI gameOverPointsText;


    private PlayerHealth playerHealth;

    private void Awake()
    {
        playerHealth = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerHealth>();


    }

    private void Start()
    {
        healthText.text = playerHealth.Health.ToString("00");
        enemyText.text = LevelManager.Instance.RemainingenemiesKilled.ToString("00");
    }

    private void Update()
    {
        healthText.text = playerHealth.Health.ToString("00");
        enemyText.text = LevelManager.Instance.RemainingenemiesKilled.ToString("00");

        int minutes = (int)LevelManager.Instance.InternalLevelTime / 60;
        int seconds = (int)LevelManager.Instance.InternalLevelTime % 60;

        timeText.text = minutes.ToString("00") + ":" + seconds.ToString("00");

        //only visible when the level end
        winPointsText.text = GameManager.Instance?.PlayerPoints.ToString("00000");
        gameOverPointsText.text = GameManager.Instance?.PlayerPoints.ToString("00000");

    }
}
