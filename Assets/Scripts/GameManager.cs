using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    [SerializeField] GameObject gameOverText;
    [SerializeField] GameObject gameClearText;

    private void Awake()
    {
        Application.targetFrameRate = 60;
        Instance = this;
    }

    // ゲームオーバー
    public void GameOver()
    {
        gameOverText.SetActive(true);
        Debug.Log("Game Over");
        StartCoroutine(RestartRoutine());
    }

    // ゲームクリア
    public void GameClear()
    {
        gameClearText.SetActive(true);
        Debug.Log("Game Clear");
        StartCoroutine(RestartRoutine());
    }

    // リスタート
    private IEnumerator RestartRoutine()
    {
        Time.timeScale = 0f;

        yield return new WaitForSecondsRealtime(1f);

        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}