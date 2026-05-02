using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class DraggableItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{

    [HideInInspector] public Transform parentAfterDrag; // 드래그가 끝난 후 돌아갈 부모 위치
    private CanvasGroup canvasGroup;

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }

    // 1. 드래그를 시작할 때
    public void OnBeginDrag(PointerEventData eventData)
    {
        parentAfterDrag = transform.parent; // 원래 있던 위치(그리드 또는 슬롯) 기억
        transform.SetParent(transform.root); // 캔버스 최상단으로 옮겨서 다른 UI에 가려지지 않게 함
        transform.SetAsLastSibling();

        // 드래그 중에는 아이템이 마우스 포인터를 가리기 때문에 광선(Raycast)을 끕니다.
        // 그래야 아이템 아래에 있는 '합성 칸'이 마우스를 인식할 수 있습니다.
        canvasGroup.blocksRaycasts = false;
    }

    // 2. 드래그 중일 때
    public void OnDrag(PointerEventData eventData)
    {
        transform.position = Input.mousePosition; // 아이템이 마우스를 따라다니게 함
    }

    // 3. 드래그를 끝냈을 때 (마우스 버튼을 뗄 때)
    public void OnEndDrag(PointerEventData eventData)
    {
        transform.SetParent(parentAfterDrag); // 새로운 부모(합성칸) 또는 원래 위치로 쏙 들어감
        canvasGroup.blocksRaycasts = true; // 다시 광선 인식 켜기
    }



}
