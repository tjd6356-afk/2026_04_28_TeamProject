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

    public float offScreenY = -1600f;

    private Vector2 targetPosition;       // 도착해서 주문받을 목적지 좌표
    private Vector2 startBottomPosition;  // 화면 아래쪽 대기 좌표

    private string desiredFoodName;
    private string currentDialogue;
    private bool isMoving = false;
    private bool isServed = false;
    private RectTransform rectTransform;

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    public void SetupCustomer(Vector2 targetPos, List<Recipe> recipes, TextMeshProUGUI textUI)
    {
        targetPosition = targetPos;
        recipeBook = recipes;
        dialogueText = textUI;

        // [수정] 목적지에서 빼는 방식이 아니라, 화면 아래쪽 외부(offScreenY)에서 시작하도록 설정
        startBottomPosition = new Vector2(targetPosition.x, offScreenY);
        rectTransform.anchoredPosition = startBottomPosition;

        // 위로 올라오는 연출 시작
        StartCoroutine(MoveToPosition(targetPosition, () => {
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

    public void OnDrop(PointerEventData eventData)
    {
        if (isServed || isMoving) return;

        GameObject droppedObject = eventData.pointerDrag;
        if (droppedObject == null) return;

        DraggableFood food = droppedObject.GetComponent<DraggableFood>();
        if (food != null)
        {
            string foodName = droppedObject.name.Replace("(Clone)", "").Trim();

            if (foodName == desiredFoodName)
            {
                isServed = true;
                if (dialogueText != null) dialogueText.text = " 와! 정말 맛있네요! 감사합니다!";

                ScoreManager.Instance.AddScore(100);

                Destroy(droppedObject);
                LeaveStation();
            }
            else
            {
                if (dialogueText != null) dialogueText.text = $" 앗, 이건 제가 주문한 {desiredFoodName}이 아니에요!";
                ScoreManager.Instance.AddScore(-50);
            }
        }
    }

    void LeaveStation()
    {
        isMoving = true;
        StartCoroutine(WaitAndLeave());
    }

    IEnumerator WaitAndLeave()
    {
        yield return new WaitForSeconds(1.5f);

        if (dialogueText != null) dialogueText.text = "";

        // 완전히 화면 밖 좌표(startBottomPosition)로 내려갑니다.
        StartCoroutine(MoveToPosition(startBottomPosition, () => {
            CustomerSpawner.Instance.OnCustomerLeft();
            Destroy(gameObject);
        }));
    }

    IEnumerator MoveToPosition(Vector2 target, System.Action onComplete)
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