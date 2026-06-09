using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement; //  [추정] 씬 전환(다시시작, 이동)을 위해 반드시 필요합니다!

public class TimerManager : MonoBehaviour
{
    //  다른 스크립트에서 TimerManager.Instance.IsGameOver 로 접근할 수 있게 만듭니다 (싱글톤)
    public static TimerManager Instance;

    [Header("UI 연결")]
    public Image timerGauge;
    public TextMeshProUGUI timerText;
    public GameObject gameOverPanel;  //  유니티에서 제작할 '게임 종료 패널'을 여기에 드래그 앤 드롭합니다.
    public TextMeshProUGUI finalScoreText;

    [Header("설정")]
    public float totalTime = 60f;
    private float currentTime;
    private bool isTimerRunning = false;

    //  현재 게임이 끝났는지 외부에서 확인할 수 있는 변수
    public bool IsGameOver { get; private set; } = false;

    void Awake()
    {
        // 싱글톤 초기화
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        currentTime = totalTime;
        isTimerRunning = true;
        IsGameOver = false;

        // 시작할 때는 종료 패널을 숨깁니다.
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(false);
        }
    }

    void Update()
    {
        if (isTimerRunning)
        {
            if (currentTime > 0)
            {
                currentTime -= Time.deltaTime;
                UpdateTimerUI();
            }
            else
            {
                currentTime = 0;
                isTimerRunning = false;
                IsGameOver = true; //  게임 종료 상태로 변경

                Debug.Log(" 제한 시간 종료!");
                ShowGameOverPanel(); //  종료 패널 띄우기
            }
        }
    }

    void UpdateTimerUI()
    {
        timerGauge.fillAmount = currentTime / totalTime;

        if (timerText != null)
        {
            int minutes = Mathf.FloorToInt(currentTime / 60);
            int seconds = Mathf.FloorToInt(currentTime % 60);
            timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        }

        if (timerGauge.fillAmount > 0.5f)
            timerGauge.color = Color.green;
        else if (timerGauge.fillAmount > 0.2f)
            timerGauge.color = Color.yellow;
        timerGauge.color = Color.red;
    }

    //  게임 종료 패널을 화면에 표시하는 함수
    void ShowGameOverPanel()
    {
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);
        }

        if (finalScoreText != null && ScoreManager.Instance != null)
        {
            int totalScore = ScoreManager.Instance.GetCurrentScore();
            finalScoreText.text = $"최종 점수: {totalScore}점";
            // 팁: 글자 형태는 원하시는 대로 얼마든지 수정 가능합니다! (예: $"SCORE\n{totalScore}")
        }
    }

    //  [버튼 기능 1] 게임 다시 시작
    public void RestartGame()
    {
        // 현재 열려있는 게임 씬을 처음부터 다시 로드합니다.
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    //  [버튼 기능 2] frist_page 씬으로 가기 (요청하신 철자 그대로 반영)
    public void GoToFirstPage()
    {
        SceneManager.LoadScene("frist_page");
    }

    //  [버튼 기능 3] 게임 완전히 종료
    public void ExitGame()
    {
#if UNITY_EDITOR
        // 유니티 에디터에서 테스트 중일 때 꺼지게 만듦
        UnityEditor.EditorApplication.isPlaying = false;
#else
        // 빌드된 실제 게임 파일에서 꺼지게 만듦
        Application.Quit();
#endif
    }
}