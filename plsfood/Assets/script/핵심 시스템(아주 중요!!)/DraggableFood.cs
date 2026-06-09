using UnityEngine;
using UnityEngine.EventSystems;

public class DraggableFood : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private Transform parentReturnTo = null;
    private CanvasGroup canvasGroup;

    void Awake()
    {
        // CanvasGroup이 없다면 자동으로 넣어주는 친절한 코드
        canvasGroup = GetComponent<CanvasGroup>();
        if (canvasGroup == null)
        {
            canvasGroup = gameObject.AddComponent<CanvasGroup>();
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        parentReturnTo = transform.parent;
        
        // 드래그하는 동안 다른 UI에 가려지지 않도록 맨 앞으로 빼줍니다.
        transform.SetParent(transform.root); 
        canvasGroup.blocksRaycasts = false; // 마우스가 음식을 통과해 NPC에게 닿도록 설정

        if (TimerManager.Instance != null && TimerManager.Instance.IsGameOver) return;
    }

    public void OnDrag(PointerEventData eventData)
    {
        // 마우스/손가락 위치로 음식 이동
        transform.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.blocksRaycasts = true;

        // 만약 NPC에게 정확히 배달되지 못했다면 원래 자리(ResultArea)로 복귀합니다.
        if (transform.parent == transform.root)
        {
            transform.SetParent(parentReturnTo);
            transform.localPosition = Vector3.zero;
        }
    }


}