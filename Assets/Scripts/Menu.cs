using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
    [SerializeField] private Button btnPlay;
    [SerializeField] private Button btnHowToPlay;
    [SerializeField] private Button btnClose;
    [SerializeField] private GameObject panelHowToPlay;


    void Awake()
    {
        FindObjectOfType<PlayerMovement>().isMove = false;

        btnPlay.onClick.AddListener(PlayClick);
        btnHowToPlay.onClick.AddListener(() => HowToPlayClick(true));
        btnClose.onClick.AddListener(() => HowToPlayClick(false));
    }

    private void PlayClick()
    {
        SceneManager.LoadScene("Level1");
    }

    private void HowToPlayClick(bool status)
    {
        panelHowToPlay.SetActive(status);
    }
}
