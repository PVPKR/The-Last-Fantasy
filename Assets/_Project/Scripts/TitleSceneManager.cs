using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleSceneManager : MonoBehaviour
{
    [Header("Main Menu")]
    [SerializeField] private GameObject mainMenuPanel;

    [Header("Panels")]
    [SerializeField] private GameObject howToPlayPanel;
    [SerializeField] private GameObject gameInfoPanel;

    private void Start()
    {
        // 게임 시작 시 메인 메뉴 표시
        if (mainMenuPanel != null)
        {
            mainMenuPanel.SetActive(true);
        }

        // 팝업 패널들은 숨김
        if (howToPlayPanel != null)
        {
            howToPlayPanel.SetActive(false);
        }

        if (gameInfoPanel != null)
        {
            gameInfoPanel.SetActive(false);
        }
    }

    public void PlayGame()
    {
        SceneManager.LoadScene("GameScene");
    }

    public void OpenHowToPlay()
    {
        if (mainMenuPanel != null)
        {
            mainMenuPanel.SetActive(false);
        }

        if (gameInfoPanel != null)
        {
            gameInfoPanel.SetActive(false);
        }

        if (howToPlayPanel != null)
        {
            howToPlayPanel.SetActive(true);
        }
    }

    public void CloseHowToPlay()
    {
        if (howToPlayPanel != null)
        {
            howToPlayPanel.SetActive(false);
        }

        if (mainMenuPanel != null)
        {
            mainMenuPanel.SetActive(true);
        }
    }

    public void OpenGameInfo()
    {
        if (mainMenuPanel != null)
        {
            mainMenuPanel.SetActive(false);
        }

        if (howToPlayPanel != null)
        {
            howToPlayPanel.SetActive(false);
        }

        if (gameInfoPanel != null)
        {
            gameInfoPanel.SetActive(true);
        }
    }

    public void CloseGameInfo()
    {
        if (gameInfoPanel != null)
        {
            gameInfoPanel.SetActive(false);
        }

        if (mainMenuPanel != null)
        {
            mainMenuPanel.SetActive(true);
        }
    }
}