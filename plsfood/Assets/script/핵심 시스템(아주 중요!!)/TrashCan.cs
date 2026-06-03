using UnityEngine;
using UnityEngine.EventSystems;

public class TrashCan : MonoBehaviour, IDropHandler
{
    public void OnDrop(PointerEventData eventData)
    {
        // 1. 드래그해서 놓은 물체(아이템)를 가져옵니다.
        GameObject droppedObject = eventData.pointerDrag;

        if (droppedObject != null)
        {
            // 2. 그 물체가 '재료'이거나 '완성된 음식'인지 확인합니다.
            DraggableIngredient ingredient = droppedObject.GetComponent<DraggableIngredient>();
            DraggableFood food = droppedObject.GetComponent<DraggableFood>();

            // 둘 중 하나라도 맞다면 쓰레기통에 버릴 수 있는 물건입니다!
            if (ingredient != null || food != null)
            {
                Debug.Log($" 쓰레기통: {droppedObject.name}을(를) 버렸습니다.");

                // 3. 오브젝트를 완전히 파괴(삭제)합니다.
                Destroy(droppedObject);

                //  나중에 여기에 "샥-" 하는 쓰레기 버리는 효과음을 넣으면 아주 좋습니다!
            }
        }
    }
}