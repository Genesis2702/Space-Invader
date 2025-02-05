using UnityEngine;
using TMPro;
using Unity.VisualScripting;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI waveText;
    public TextMeshProUGUI liveText;
    public TextMeshProUGUI gameOverText;
    public Button restartButton;
    public Button menuButton;
    private PlayerController playerControllerScript;
    private int score;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerControllerScript = GameObject.Find("Player").GetComponent<PlayerController>();
        UpdateScore(0);
        UpdateWave(1);
    }

    // Update is called once per frame
    void Update()
    {
        GameOver();
    }

    public void UpdateScore(int scoreToAdd)
    {
        score += scoreToAdd;
        scoreText.text = "Score:" + score;
    }

    public void UpdateWave(int wave)
    {
        waveText.text = "Wave " + wave;
    }

    public void UpdateLive(int lives)
    {
        liveText.text = "Lives:" + lives;
    }

    private void GameOver()
    {
        if (!playerControllerScript.isAlive)
        {
            gameOverText.gameObject.SetActive(true);
            restartButton.gameObject.SetActive(true);
            menuButton.gameObject.SetActive(true);
        }
    }

    public void GameRestart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void GameMenu()
    {
        SceneManager.LoadScene(0);
    }
}
