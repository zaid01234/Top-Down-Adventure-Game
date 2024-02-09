using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static UnityEngine.Rendering.DebugUI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public PlayerController _player;
    public Transform _camera;

    [SerializeField] private Text _collectKeyInform_Text;
    [SerializeField] private GameObject _collectKeyFillImage;
    [SerializeField] private GameObject _doorOpenImage;

    public GameObject _gameStartBtn;
    public GameObject _gameOverPanel;
    public GameObject _gameCompletePanel;
    public bool IsKeyCollected { get; private set; }

    private void Awake()
    {
        if (instance != null && instance != this)
            Destroy(gameObject);
        else
            instance = this;
    }
    private void Start()
    {
        Time.timeScale = 0;
    }
    public void StartGame()
    {
        Time.timeScale = 1;
        _gameStartBtn.SetActive(false);
        ShowCollectKeyMessageToPlayer();
    }
    public void ShowCollectKeyMessageToPlayer()
    {
        _collectKeyInform_Text.enabled = true;
        CancelInvoke("DisableCollectKeyMessageToPlayer");
        Invoke("DisableCollectKeyMessageToPlayer", 3f);
    }
    void DisableCollectKeyMessageToPlayer()
    {
        _collectKeyInform_Text.enabled = false;
    }
    public void KeyCollected()
    {
        IsKeyCollected = true;
        _collectKeyFillImage.SetActive(true);
        _collectKeyInform_Text.text = "Now go to Door to Open it.";
        ShowCollectKeyMessageToPlayer();
    }
    public void OpenDoor()
    {
        _doorOpenImage.SetActive(true);
        Invoke("GameWon", 0.5f);
    }
    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    void GameWon()
    {
        Time.timeScale = 0;
        _gameCompletePanel.SetActive(true);
    }
    public void GameOver()
    {
        Time.timeScale = 0;
        _gameOverPanel.SetActive(true);
    }
}
