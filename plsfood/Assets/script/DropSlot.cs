using UnityEngine;
using UnityEngine.EventSystems;

public class DropSlot : MonoBehaviour, IDropHandler
{

    public void OnDrop(PointerEventData eventData)
    {
        // 드롭된 오브젝트를 가져옴
        GameObject droppedItem = eventData.pointerDrag;
        DraggableItem draggableItem = droppedItem.GetComponent<DraggableItem>();

        // 드롭된 것이 드래그 가능한 아이템이고, 현재 슬롯이 비어있다면
        if (draggableItem != null && transform.childCount == 0)
        {
            // 아이템의 '드래그 후 부모'를 이 합성 칸으로 변경
            draggableItem.parentAfterDrag = transform;

            // 아이템이 슬롯에 들어왔으니 매니저에게 합성 체크를 요청함
            SynthesisManager.Instance.CheckRecipe();
        }
    }




}
