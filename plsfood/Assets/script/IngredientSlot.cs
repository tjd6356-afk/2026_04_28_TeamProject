using UnityEngine;
using UnityEngine.EventSystems;

public class IngredientSlot : MonoBehaviour, IDropHandler
{
    public void OnDrop(PointerEventData eventData)
    {
        // 드래그 중인 오브젝트를 가져옴
        GameObject dropped = eventData.pointerDrag;
        DraggableIngredient draggable = dropped.GetComponent<DraggableIngredient>();
        

        if (draggable != null && transform.childCount == 0)
        {
            // 🌟 아이템이 돌아갈 부모를 이 슬롯으로 바꿔치기함!
            draggable.parentToReturnTo = transform;
        }
    }
}