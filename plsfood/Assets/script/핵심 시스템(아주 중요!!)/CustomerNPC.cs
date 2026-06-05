using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using System.Collections;
using TMPro;

public class CustomerNPC : MonoBehaviour, IDropHandler, IPointerClickHandler
{
    [Header("손님 데이터 및 레시피")]
    public CustomerData customerData;
    private List<Recipe> recipeBook;
    private TextMeshProUGUI dialogueText;

    [Header("연출 설정")]
    public float moveSpeed = 5f;          // 스르륵 움직이는 속도
    private Vector2 targetPosition;       // 도착해서 주문받을 목적지 좌표
    private Vector2 startBottomPosition;  // 화면 아래쪽 대기 좌표

    private string desiredFoodName;
    private string currentDialogue;
    private bool isMoving = false;        // 현재 이동 중인지 체크
    private bool isServed = false;        // 이미 음식을 받았는지 체크
    private RectTransform rectTransform;

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    // Spawner가 손님을 생성한 직후 초기 세팅을 해줄 함수
    public void SetupCustomer(Vector2 targetPos, List<Recipe> recipes, TextMeshProUGUI textUI)
    {
        targetPosition = targetPos;
        recipeBook = recipes;
        dialogueText = textUI;

        // 시작 위치는 목적지에서 아래로 600픽셀 내려간 곳 (카메라 밖)
        startBottomPosition = targetPosition + new Vector2(0, -600f);
        rectTransform.anchoredPosition = startBottomPosition;

        // 1. 위로 올라오는 연출 시작
        StartCoroutine(MoveToPosition(targetPosition, () => {
            // 도착하면 주문 생성
            GenerateRandomOrder();
            ShowDialogue();
        }));
    }

    void GenerateRandomOrder()
    {
        if (recipeBook == null || recipeBook.Count == 0) return;

        int randomIndex = Random.Range(0, recipeBook.Count);
        Recipe chosenRecipe = recipeBook[randomIndex];

        if (chosenRecipe.resultPrefab != null)
        {
            desiredFoodName = chosenRecipe.resultPrefab.name;
        }

        string greeting = "안녕하세요!";
        if (customerData != null && customerData.greetings.Count > 0)
        {
            greeting = customerData.greetings[Random.Range(0, customerData.greetings.Count)];
        }

        currentDialogue = $"{greeting}\n<b>[{chosenRecipe.recipeName}]</b> 하나 주세요!";
    }

    void ShowDialogue()
    {
        if (dialogueText != null) dialogueText.text = currentDialogue;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (!isMoving && !isServed) ShowDialogue();
    }

    // 🌟 음식을 받았을 때 처리하는 핵심 로직
    public void OnDrop(PointerEventData eventData)
    {
        if (isServed || isMoving) return;

        GameObject droppedObject = eventData.pointerDrag;
        if (droppedObject == null) return;

        DraggableFood food = droppedObject.GetComponent<DraggableFood>();
        if (food != null)
        {
            string foodName = droppedObject.name.Replace("(Clone)", "").Trim();

            // 1. 맞는 음식일 때
            if (foodName == desiredFoodName)
            {
                isServed = true;
                if (dialogueText != null) dialogueText.text = "😋 와! 정말 맛있네요! 감사합니다!";

                // 점수 상승! (예: +100점)
                ScoreManager.Instance.AddScore(100);

                Destroy(droppedObject); // 음식 삭제
                LeaveStation();         // 퇴장 연출 시작
            }
            // 2. 틀린 음식일 때
            else
            {
                if (dialogueText != null) dialogueText.text = $"🤢 앗, 이건 제가 주문한 {desiredFoodName}이 아니에요!";

                // 점수 감점! (예: -50점)
                ScoreManager.Instance.AddScore(-50);

                // 틀렸을 때는 음식을 파괴하지 않고 원래 자리(Result Area)로 돌려보냅니다.
            }
        }
    }

    // 아래로 내려가면서 퇴장하는 함수
    void LeaveStation()
    {
        isMoving = true;
        // 1.5초 동안 감사 대사를 보여준 뒤 아래로 내려갑니다.
        StartCoroutine(WaitAndLeave());
    }

    System.Collections.IEnumerator WaitAndLeave()
    {
        yield return new WaitForSeconds(1.5f);

        // 대사창 비우기
        if (dialogueText != null) dialogueText.text = "";

        // 아래쪽 화면 밖으로 내려가는 연출
        StartCoroutine(MoveToPosition(startBottomPosition, () => {
            // 완전히 내려가면 오브젝트 삭제 후 다음 손님 소환 요청
            CustomerSpawner.Instance.OnCustomerLeft();
            Destroy(gameObject);
        }));
    }

    // UI 오브젝트를 목적지까지 부드럽게 이동시키는 연출 (Lerp)
    System.Collections.IEnumerator MoveToPosition(Vector2 target, System.Action onComplete)
    {
        isMoving = true;
        while (Vector2.Distance(rectTransform.anchoredPosition, target) > 0.5f)
        {
            rectTransform.anchoredPosition = Vector2.Lerp(rectTransform.anchoredPosition, target, Time.deltaTime * moveSpeed);
            yield return null;
        }
        rectTransform.anchoredPosition = target;
        isMoving = false;
        onComplete?.Invoke();
    }
}