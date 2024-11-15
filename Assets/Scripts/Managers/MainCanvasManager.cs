using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainCanvasManager : MonoBehaviour
{
    public static MainCanvasManager Instance;

    [SerializeField]
    private TextMeshProUGUI scoreText;

    [SerializeField]
    private TextMeshProUGUI healthText;

    [SerializeField]
    private GameObject gameOverPanel;

    [SerializeField]
    private GameObject gamePanel;

    [SerializeField]
    private GameObject menuPanel;

    [SerializeField]
    private Button restartButton;

    [SerializeField]
    private Button startButton;

    [SerializeField]
    private TMP_InputField ballCountInput;

    [SerializeField]
    private TMP_InputField durationInput;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        restartButton.onClick.AddListener(() => SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex));
        startButton.onClick.AddListener(() => OnStart());
    }

    private void OnStart()
    {
        if (ballCountInput.text != string.Empty)
            GolfBallManager.Instance.BallCount = int.Parse(ballCountInput.text);

        if (durationInput.text != string.Empty)
            FindObjectOfType<NPCHealth>().Duration = int.Parse(durationInput.text);

        GolfBallManager.Instance.SpawnBalls();

        StartCoroutine(OpenGamePanel());
    }

    private IEnumerator OpenGamePanel()
    {
        menuPanel.SetActive(false);
        CameraManager.Instance.SwitchCamera();
        yield return new WaitForSeconds(2.1f);
        gamePanel.SetActive(true);
        FindObjectOfType<NPCController>().IsGameStarted = true;
    }

    public void UpdateScoreText(int score)
    {
        scoreText.SetText(score.ToString());
    }

    public void UpdateHealthText(int health)
    {
        healthText.SetText(health.ToString());
    }

    public void OpenGameOverPanel()
    {
        gameOverPanel.SetActive(true);
    }
}
