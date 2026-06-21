using UnityEngine;

public class SettingsMenuController : MonoBehaviour
{
    [Header("설정창 UI 게임 오브젝트")]
    [SerializeField] private GameObject settingsPanel;

    void Update()
    {
        // ESC 키가 눌렸는지 매 프레임 확인
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ToggleSettingsMenu();
        }
    }

    private void ToggleSettingsMenu()
    {
        if (settingsPanel != null)
        {
            // 현재 활성화 상태를 반전 (true -> false, false -> true)
            bool isActive = settingsPanel.activeSelf;
            settingsPanel.SetActive(!isActive);
        }
    }
}
