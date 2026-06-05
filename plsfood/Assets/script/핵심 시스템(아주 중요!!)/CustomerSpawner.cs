using UnityEngine;
using System.Collections.Generic;
using TMPro;

public class CustomerSpawner : MonoBehaviour
{
    public static CustomerSpawner Instance;

    [Header("손님 프리팹 리스트")]
    public GameObject[] customerPrefabs; // NPC_1, NPC_2, NPC_3 프리팹 등록하는 곳
    public Transform canvasTransform;    // 손님이 생성될 UI Canvas

    [Header("공용 UI 및 데이터 연결")]
    public List<Recipe> recipeBook;
    public TextMeshProUGUI dialogueText;

    [Header("손님 스탠딩 위치 정보")]
    public RectTransform targetPositionObject; // 현재 호랑이 손님이 서 있는 UI 위치 컴포넌트

    private Vector2 savedTargetAnchoredPos;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        // 기준이 될 손님 위치 좌표를 기억해 둡니다.
        if (targetPositionObject != null)
        {
            savedTargetAnchoredPos = targetPositionObject.anchoredPosition;
            // 하이Hierarchy에 기본 배치되어 있던 고정 배치용 손님 오브젝트는 비활성화하거나 지웁니다.
            targetPositionObject.gameObject.SetActive(false);
        }

        // 첫 번째 손님 등장!
        SpawnRandomCustomer();
    }

    public void SpawnRandomCustomer()
    {
        if (customerPrefabs == null || customerPrefabs.Length == 0) return;

        // 1. 랜덤 손님 프리팹 선택
        int randomIndex = Random.Range(0, customerPrefabs.Length);
        GameObject chosenPrefab = customerPrefabs[randomIndex];

        // 2. 생성 및 캔버스 자식으로 배치
        GameObject newCustomer = Instantiate(chosenPrefab, canvasTransform);

        // 3. UI 스케일 꼬임 방지
        newCustomer.transform.localScale = Vector3.one;

        // 4. 손님 컴포넌트 가져와서 초기 움직임 및 주문 세팅 시작
        CustomerNPC customerScript = newCustomer.GetComponent<CustomerNPC>();
        if (customerScript != null)
        {
            customerScript.SetupCustomer(savedTargetAnchoredPos, recipeBook, dialogueText);
        }
    }

    // 손님이 완전히 화면 밖으로 내려가 사라졌을 때 호출되는 함수
    public void OnCustomerLeft()
    {
        // 다음 새로운 손님을 소환합니다!
        SpawnRandomCustomer();
    }
}