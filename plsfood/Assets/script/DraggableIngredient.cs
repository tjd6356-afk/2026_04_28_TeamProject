using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DraggableIngredient : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private CanvasGroup canvasGroup;
    [HideInInspector] public Transform parentToReturnTo = null; // 돌아갈 부모를 저장하는 변수

    // [Lifting Effect]: 들어 올렸을 때 살짝 커지게 함
    private Vector3 dragScale = new Vector3(1.5f, 1.5f, 1.0f);
    private Vector3 originalScale;

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        originalScale = transform.localScale; // 원래 크기 저장
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        // 1. 드래그 시작 시 현재 부모(그리드)를 기억함
        parentToReturnTo = transform.parent;

        // 2. 드래그 중엔 화면 맨 위로 보이게 Canvas 바로 아래로 옮김
        transform.SetParent(transform.root);
        transform.SetAsLastSibling();

        // 3. 마우스 광선 통과 (이걸 해야 슬롯이 마우스를 인식함)
        canvasGroup.blocksRaycasts = false;

        canvasGroup.alpha = 0.6f; // 살짝 투명하게 만들어서 "들고 있다"는 느낌을 줌
        // [Visual]: 크기를 키워 "들어 올린 느낌" 연출
        transform.localScale = dragScale;

        if (TimerManager.Instance != null && TimerManager.Instance.IsGameOver) return;
    }

    public void OnDrag(PointerEventData eventData) => transform.position = Input.mousePosition;

    public void OnEndDrag(PointerEventData eventData)
    {
        // 4. 드래그 종료! 저장된 부모(슬롯 혹은 원래 그리드)로 복귀
        transform.SetParent(parentToReturnTo);

        // 5. 마우스 광선 다시 켜기
        canvasGroup.blocksRaycasts = true;

        // 6. 위치 초기화 (부모가 그리드라면 Layout Group이 알아서 정렬해줌)
        transform.localPosition = Vector3.zero;

        canvasGroup.alpha = 1.0f; // 불투명하게 돌리기
        // [Visual]: 크기를 원래대로 돌림
        transform.localScale = originalScale;
        Object.FindAnyObjectByType<SynthesisManager>()?.CheckRecipe();
    }
}