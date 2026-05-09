using UnityEngine;
using System.Collections.Generic;

public class ItemSpawner : MonoBehaviour
{
    [Header("설정")]
    public GameObject[] ingredientPrefabs; // 랜덤으로 나올 재료 프리팹 리스트
    public Transform inventoryGrid;        // 재료가 들어갈 부모(InventoryGrid)
    public int maxSlots = 36;              // 채울 칸 수 (6x6이면 36)

    void Start()
    {
        // 게임이 시작되자마자 실행!
        SpawnRandomItems();
    }

    public void SpawnRandomItems()
    {
        // 1. 혹시 이미 그리드에 아이템이 있다면 싹 비워주기 (청소)
        foreach (Transform child in inventoryGrid)
        {
            Destroy(child.gameObject);
        }

        // 2. 설정한 개수만큼 반복해서 생성하기
        for (int i = 0; i < maxSlots; i++)
        {
            // 3. 프리팹 리스트 중에서 아무거나 하나 고르기 (0번부터 끝번호 사이)
            int randomIndex = Random.Range(0, ingredientPrefabs.Length);
            GameObject selectedPrefab = ingredientPrefabs[randomIndex];

            // 4. 실제로 화면에 만들기 (붕어빵 찍기)
            GameObject newItem = Instantiate(selectedPrefab, inventoryGrid);

            // 5. UI 아이템은 크기가 변할 수 있으니 (1, 1, 1)로 고정해주기
            newItem.transform.localScale = Vector3.one;

            // 6. 이름 뒤에 (Clone) 붙는 게 보기 싫으면 이름 깔끔하게 정리
            newItem.name = selectedPrefab.name;
        }

        Debug.Log("🎉 모든 재료가 랜덤하게 배치되었습니다!");
    }
}