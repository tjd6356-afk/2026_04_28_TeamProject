using UnityEngine;
using UnityEngine.UI; // 버튼 활성화/비활성화를 제어할 때 필요합니다.

public class HelpMenu : MonoBehaviour
{
    [Header("페이지 설정")]
    public GameObject[] helpPages; // 도움말 페이지들을 순서대로 넣어주세요.
    private int currentIndex = 0;   // 현재 보고 있는 페이지 번호

    [Header("버튼 설정")]
    public Button prevButton; // 이전 버튼
    public Button nextButton; // 다음 버튼

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        UpdateUI();
    }

    // 다음 페이지로 이동
    public void NextPage()
    {
        if (currentIndex < helpPages.Length - 1)
        {
            currentIndex++;
            UpdateUI();
        }
    }

    // 이전 페이지로 이동
    public void PreviousPage()
    {
        if (currentIndex > 0)
        {
            currentIndex--;
            UpdateUI();
        }
    }

    // 페이지를 갱신하는 핵심 로직
    private void UpdateUI()
    {
        // 1. 모든 페이지를 일단 끕니다.
        for (int i = 0; i < helpPages.Length; i++)
        {
            helpPages[i].SetActive(false);
        }

        // 2. 현재 인덱스의 페이지 만 켭니다.
        helpPages[currentIndex].SetActive(true);

        // 3. (선택사항) 첫 페이지면 '이전' 버튼을, 마지막 페이지면 '다음' 버튼을 못 누르게 막습니다.
        if (prevButton != null) prevButton.interactable = (currentIndex > 0);
        if (nextButton != null) nextButton.interactable = (currentIndex < helpPages.Length - 1);
    }
}
