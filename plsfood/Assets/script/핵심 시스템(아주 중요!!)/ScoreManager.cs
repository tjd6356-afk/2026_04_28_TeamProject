using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    // 어디서나 점수에 접근할 수 있도록 싱글톤(Singleton) 설정
    public static ScoreManager Instance;

    [Header("UI 연결")]
    public TextMeshProUGUI scoreText; // 점수를 표시할 TMPro 텍스트

    private int currentScore = 0;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        UpdateScoreUI();
    }

    // 점수를 더하거나 빼는 함수
    public void AddScore(int amount)
    {
        currentScore += amount;

        // 점수가 0점 밑으로 내려가지 않게 방지 (원치 않으시면 이 줄을 지우세요)
        if (currentScore < 0) currentScore = 0;

        UpdateScoreUI();
    }

    void UpdateScoreUI()
    {
        if (scoreText != null)
        {
            scoreText.text = $"Score: {currentScore}";
        }
    }

    public int GetCurrentScore()
    {
        return currentScore;
    }

}