using UnityEngine;
using System.Collections.Generic;

public class ItemSpawner : MonoBehaviour
{
    [Header("설정")]
    [Tooltip("게임을 시작할 때 아이템을 가득 채울지 여부입니다.")]
    public bool spawnOnStart = false; // 기본값은 false로 변경

    [Header("설정")]
    public GameObject[] ingredientPrefabs; // 랜덤으로 나올 재료 프리팹 리스트
    public Transform inventoryGrid;        // 재료가 들어갈 부모(InventoryGrid)
    public int maxSlots = 36;              // 채울 칸 수 (6x6이면 36)

    void Start()
    {
        if (spawnOnStart)
        {
            SpawnRandomItems();
        }
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

    public void SpawnOneRandomItem()
    {
        //
        // 1. 재료 프리팹이 등록되어 있는지 확인
        if (TimerManager.Instance != null && TimerManager.Instance.IsGameOver) return;

        if (ingredientPrefabs == null || ingredientPrefabs.Length == 0)
        {
            Debug.LogError("재료 프리팹 배열이 비어있습니다. 프리팹을 등록해주세요.");
            return;
        }

        // 2. 현재 그리드에 있는 아이템 수를 확인합니다.
        int currentItemCount = inventoryGrid.childCount;

        // 3. 인벤토리가 가득 차지 않았을 때만 생성합니다.
        //
        if (currentItemCount < maxSlots)
        {
            // 랜덤 인덱스 선택
            //
            int randomIndex = Random.Range(0, ingredientPrefabs.Length);
            GameObject newItemPrefab = ingredientPrefabs[randomIndex];

            // 4. 아이템을 InventoryGrid의 자식으로 생성
            //
            GameObject newItem = Instantiate(newItemPrefab, inventoryGrid);

            //
            // 5. [중요] 아이템이 드래그 종료 후 돌아갈 '고향' 주소를 그리드로 설정
            DraggableIngredient draggableScript = newItem.GetComponent<DraggableIngredient>();
            if (draggableScript != null)
            {
                draggableScript.parentToReturnTo = inventoryGrid;
            }

            // 6. UI 내에서 크기가 어긋나지 않게 (1, 1, 1)로 설정
            //
            newItem.transform.localScale = Vector3.one;

            Debug.Log($"랜덤 아이템 생성됨: {newItemPrefab.name}");
        }
        else
        {
            Debug.LogWarning("인벤토리가 가득 찼습니다! 더 이상 아이템을 생성할 수 없습니다.");
        }
    }

}