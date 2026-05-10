using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class SynthesisManager : MonoBehaviour
{
    [Header("연결할 것들")]
    public Transform[] inputSlots;    // 인스펙터에서 슬롯 4개를 드래그해서 넣으세요
    public Transform resultArea;      // 오른쪽 하단 빨간 네모 칸
    public List<Recipe> recipeBook;   // 레시피 파일들 리스트

    public void CheckRecipe()
    {
        List<string> currentIngredients = new List<string>();

        foreach (Transform slot in inputSlots)
        {
            if (slot.childCount > 0)
            {
                // (Clone) 문구 제거 및 공백 제거로 정확한 이름 비교
                string itemName = slot.GetChild(0).name.Replace("(Clone)", "").Trim();
                currentIngredients.Add(itemName);
            }
        }

        // 재료가 4개 모두 찼을 때 합성을 시도합니다.
        if (currentIngredients.Count == 4)
        {
            TrySynthesize(currentIngredients);
        }
    }

    void TrySynthesize(List<string> inputs)
    {
        inputs.Sort(); // 입력된 재료 정렬

        Debug.Log("현재 슬롯 재료: " + string.Join(", ", inputs)); // 현재 슬롯에 뭐가 있는지 출력
        inputs.Sort();
        // ... 나머지 코드

        foreach (Recipe recipe in recipeBook)
        {
            // 레시피에 설정된 재료가 4개가 아니면 건너뜁니다.
            if (recipe.ingredients.Count != 4) continue;

            List<string> recipeIngredients = new List<string>(recipe.ingredients);
            recipeIngredients.Sort(); // 레시피 재료 정렬

            // 두 리스트가 일치하는지 비교
            if (inputs.SequenceEqual(recipeIngredients))
            {
                CreateFood(recipe.resultPrefab);
                ClearSlots();
                return;
            }
        }
    }

    void CreateFood(GameObject foodPrefab)
    {
        foreach (Transform child in resultArea) { Destroy(child.gameObject); }

        GameObject newFood = Instantiate(foodPrefab, resultArea);
        newFood.transform.localPosition = Vector3.zero;
        newFood.transform.localScale = Vector3.one;

        // 결과물의 이름에서 (Clone) 제거 (나중에 다시 재료로 쓸 경우 대비)
        newFood.name = foodPrefab.name;
        Debug.Log("🎉 4개 재료 합성 성공!");
    }

    void ClearSlots()
    {
        foreach (Transform slot in inputSlots)
        {
            if (slot.childCount > 0) Destroy(slot.GetChild(0).gameObject);
        }
    }
}