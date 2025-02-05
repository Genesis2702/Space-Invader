using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class DifficultyButton : MonoBehaviour
{
    private Button button;
    public int difficulty;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(SetDifficulty);
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void SetDifficulty()
    {
        if (difficulty == 1)
        {
            SceneManager.LoadScene(1);
        }
        else if (difficulty == 2)
        {
            SceneManager.LoadScene(1);
        }
        else if (difficulty == 3)
        {

            SceneManager.LoadScene(1);
        }
    }
}
