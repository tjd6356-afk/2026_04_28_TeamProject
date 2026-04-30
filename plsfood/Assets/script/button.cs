using UnityEngine;
using UnityEngine.SceneManagement; // 씬 전환을 위해 필수!

public class button : MonoBehaviour
{
    
    public GameObject helpPanel; 

    public void GameStart()
    {
        
        SceneManager.LoadScene("Game_Play");
    }

    public void Gametitle()
    {
        
        SceneManager.LoadScene("frist_page");
    }

    public void OpenHelp()
    {
        
        helpPanel.SetActive(true);
    }

    public void CloseHelp()
    {
        helpPanel.SetActive(false);
    }
}