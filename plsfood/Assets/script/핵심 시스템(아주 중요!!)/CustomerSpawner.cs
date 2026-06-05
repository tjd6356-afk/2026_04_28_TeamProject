using UnityEngine;
using System.Collections.Generic;
using TMPro;

public class CustomerSpawner : MonoBehaviour
{
    public static CustomerSpawner Instance;

    [Header("손님 프리팹 리스트")]
    public GameObject[] customerPrefabs;

    // [수정] 손님이 생성되어 배치될 레이어 전용 부모 오브젝트입니다.
    [Header("레이어 설정용 부모 오브젝트")]
    public Transform customerParent;

    [Header("공용 UI 및 데이터 연결")]
    public List<Recipe> recipeBook;
    public TextMeshProUGUI dialogueText;

    [Header("손님 스탠딩 위치 정보")]
    public RectTransform targetPositionObject;

    private Vector2 savedTargetAnchoredPos;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        if (targetPositionObject != null)
        {
            savedTargetAnchoredPos = targetPositionObject.anchoredPosition;
            targetPositionObject.gameObject.SetActive(false);
        }

        // 첫 손님 생성
        SpawnRandomCustomer();
    }

    public void SpawnRandomCustomer()
    {
        if (customerPrefabs == null || customerPrefabs.Length == 0) return;

        int randomIndex = Random.Range(0, customerPrefabs.Length);
        GameObject chosenPrefab = customerPrefabs[randomIndex];

        // [수정] 이제 전체 캔버스가 아니라 우리가 설정한 customerParent의 자식으로 쏙 들어갑니다.
        GameObject newCustomer = Instantiate(chosenPrefab, customerParent);

        newCustomer.transform.localScale = Vector3.one;

        CustomerNPC customerScript = newCustomer.GetComponent<CustomerNPC>();
        if (customerScript != null)
        {
            customerScript.SetupCustomer(savedTargetAnchoredPos, recipeBook, dialogueText);
        }
    }

    public void OnCustomerLeft()
    {
        SpawnRandomCustomer();
    }
}