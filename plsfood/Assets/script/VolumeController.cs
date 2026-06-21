using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class VolumeController : MonoBehaviour, IPointerDownHandler, IDragHandler
{
    public enum VolumeType { BGM, SFX }

    [Header("볼륨 타입 설정")]
    [SerializeField] private VolumeType volumeType;

    private Image filledImage;
    private RectTransform rectTransform;

    void Awake()
    {
        filledImage = GetComponent<Image>();
        rectTransform = GetComponent<RectTransform>();

        // 이미지 타입을 강제로 Filled 및 Horizontal(가로)로 세팅합니다.
        if (filledImage != null)
        {
            filledImage.type = Image.Type.Filled;
            filledImage.fillMethod = Image.FillMethod.Horizontal;
            filledImage.fillOrigin = (int)Image.OriginHorizontal.Left;
        }
    }

    // 마우스로 클릭했을 때
    public void OnPointerDown(PointerEventData eventData)
    {
        UpdateVolume(eventData);
    }

    // 마우스로 드래그할 때
    public void OnDrag(PointerEventData eventData)
    {
        UpdateVolume(eventData);
    }

    private void UpdateVolume(PointerEventData eventData)
    {
        if (rectTransform == null || filledImage == null) return;

        // 마우스 클릭/드래그 위치를 UI 로컬 좌표로 변환
        Vector2 localPoint;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTransform, eventData.position, eventData.pressEventCamera, out localPoint);

        // 가로 길이를 기준으로 0.0 ~ 1.0 사이의 비율 계산 (Pivot이 중심(0.5)에 있을 때 기준)
        float width = rectTransform.rect.width;
        float pivotOffset = rectTransform.pivot.x * width;
        float normalizedValue = (localPoint.x + pivotOffset) / width;

        // 0과 1 사이로 값 제한
        normalizedValue = Mathf.Clamp01(normalizedValue);

        // UI 이미지의 채우기 양(Fill Amount) 변경
        filledImage.fillAmount = normalizedValue;

        // AudioManager에 볼륨 값 전달
        if (AudioManager.Instance != null)
        {
            if (volumeType == VolumeType.BGM)
            {
                AudioManager.Instance.SetBGMVolume(normalizedValue);
            }
            else if (volumeType == VolumeType.SFX)
            {
                AudioManager.Instance.SetSFXVolume(normalizedValue);
            }
        }
    }
}