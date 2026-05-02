using UnityEngine;
using System.Collections.Generic;

public class SynthesisManager : MonoBehaviour
{

    public static SynthesisManager Instance; // 싱글톤 패턴 (어디서든 접근 가능하게)

    public Transform[] craftSlots; // 인스펙터에서 위쪽 4개의 합성 칸(DropSlot)을 연결해주세요.

    private void Awake()
    {
        if (Instance == null) Instance = this;
    }

    // 아이템이 합성 칸에 들어올 때마다 호출되는 함수
    public void CheckRecipe()
    {
        List<string> currentIngredients = new List<string>();

        // 4개의 슬롯을 검사하여 어떤 재료가 들어있는지 확인
        foreach (Transform slot in craftSlots)
        {
            if (slot.childCount > 0)
            {
                // 자식 오브젝트(재료)의 이름을 리스트에 추가 (또는 아이템 ID를 사용해도 됩니다)
                string ingredientName = slot.GetChild(0).name;
                currentIngredients.Add(ingredientName.Replace("(Clone)", "").Trim()); // 프리팹 생성 시 붙는 Clone 글자 제거
            }
        }

        Debug.Log("현재 올라간 재료 개수: " + currentIngredients.Count);

        // 예시: 재료가 2개 이상 모였을 때 조합을 확인하는 로직
        if (currentIngredients.Count >= 2)
        {
            // 여기에 레시피 검사 로직을 추가하세요!
            // 예: currentIngredients에 "딸기"와 "밀가루"가 있다면 -> 딸기 케이크 생성!
            if (currentIngredients.Contains("Strawberry") && currentIngredients.Contains("Flour"))
            {
                Debug.Log("🎉 딸기 케이크 완성!");
                // 재료 소모 및 완성품 생성 로직 추가...
            }
        }


    }
}
