using UnityEngine;
using UnityEngine.SceneManagement;


public class button : MonoBehaviour
{
    public void GameStart()
    {
        SceneManagement.LoadScene("Game_Play")
    }
    public void Gametitle()
    {
        SceneManager.LoadScene("frist_page");
    }



}
