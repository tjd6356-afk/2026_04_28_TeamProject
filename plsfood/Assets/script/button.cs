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

    // 도움말 버튼을 눌렀을 때 호출할 함수
    public void OpenHelp()
    {
        if (helpPanel != null)
        {
            helpPanel.SetActive(true); // 패널을 화면에 보이게 합니다.
        }
    }

    // 도움말 창 닫기 버튼을 눌렀을 때 호출할 함수
    public void CloseHelp()
    {
        if (helpPanel != null)
        {
            helpPanel.SetActive(false); // 패널을 화면에서 숨깁니다.
        }
    }
}