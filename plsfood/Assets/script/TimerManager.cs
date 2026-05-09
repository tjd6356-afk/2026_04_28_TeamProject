using UnityEngine;
using UnityEngine.UI; // 게이지(Image)를 조절하기 위해 꼭 필요해요!
using TMPro;

public class TimerManager : MonoBehaviour
{
    [Header("UI 연결")]
    public Image timerGauge;       // 아까 만든 TimerFill 이미지를 여기에 넣을 거예요.
    public TextMeshProUGUI timerText; // (선택사항) 숫자 시간도 같이 보고 싶을 때 사용

    [Header("설정")]
    public float totalTime = 60f;
    private float currentTime;
    private bool isTimerRunning = false;

    void Start()
    {
        currentTime = totalTime;
        isTimerRunning = true;
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
                Debug.Log("⌛ 제한 시간 종료!");
            }
        }
    }

    void UpdateTimerUI()
    {
        // 1. 게이지 양 조절 (0~1 사이의 값)
        // 현재 시간을 전체 시간으로 나누면 비율이 나옵니다.
        timerGauge.fillAmount = currentTime / totalTime;

        // 2. 숫자 텍스트 업데이트 (있을 때만)
        if (timerText != null)
        {
            int minutes = Mathf.FloorToInt(currentTime / 60);
            int seconds = Mathf.FloorToInt(currentTime % 60);
            timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        }

        // 3. 시간이 줄어들수록 색깔을 바꾸는 센스! (초록 -> 빨강)
        if (timerGauge.fillAmount > 0.5f)
            timerGauge.color = Color.green;
        else if (timerGauge.fillAmount > 0.2f)
            timerGauge.color = Color.yellow;
        else
            timerGauge.color = Color.red;
    }
}