using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using TMPro; // UI 텍스트 출력을 위해 필요합니다!

public class CustomerNPC : MonoBehaviour, IDropHandler, IPointerClickHandler
{
    [Header("손님 데이터 및 레시피")]
    public CustomerData customerData;    // 위에서 만든 손님 에셋을 넣는 곳
    public List<Recipe> recipeBook;      // 전체 레시피 리스트를 여기에 넣어줍니다
    
    [Header("UI 연결")]
    public TextMeshProUGUI dialogueText; // 대사를 띄워줄 UI 텍스트 상자

    private string desiredFoodName;      // 손님이 원하는 음식 프리팹 이름
    private string currentDialogue;      // 현재 손님이 할 대사

    void Start()
    {
        GenerateRandomOrder();
    }

    // 🌟 레시피 중 하나를 랜덤하게 골라 주문 대사를 만듭니다.
    void GenerateRandomOrder()
    {
        if (recipeBook == null || recipeBook.Count == 0)
        {
            Debug.LogError("레시피 북이 비어있습니다! 레시피를 등록해주세요.");
            return;
        }

        // 1. 레시피 북에서 무작위로 하나 선택
        int randomIndex = Random.Range(0, recipeBook.Count);
        Recipe chosenRecipe = recipeBook[randomIndex];

        // 2. 손님이 원하는 음식 이름 기억하기 (프리팹 이름 기준)
        if (chosenRecipe.resultPrefab != null)
        {
            desiredFoodName = chosenRecipe.resultPrefab.name;
        }

        // 3. ScriptableObject에서 랜덤 인사말 가져오기
        string greeting = "안녕하세요!";
        if (customerData != null && customerData.greetings.Count > 0)
        {
            int dialogIndex = Random.Range(0, customerData.greetings.Count);
            greeting = customerData.greetings[dialogIndex];
        }

        // 4. 인사말과 주문 대사 합치기
        currentDialogue = $"{greeting}\n<b>[{chosenRecipe.recipeName}]</b> 하나 주세요!";
        
        // 처음에 바로 대사를 보여주고 싶다면 아래 주석을 해제하세요.
        // ShowDialogue();
    }

    // 대사창에 글자 띄우기
    void ShowDialogue()
    {
        if (dialogueText != null)
        {
            dialogueText.text = currentDialogue;
        }
    }

    // 🌟 손님(이미지)을 클릭했을 때 실행되는 함수
    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log($"{customerData?.customerName} 클릭됨!");
        ShowDialogue();
    }

    // 음식을 받아먹는 로직 (기존 기능 유지)
    public void OnDrop(PointerEventData eventData)
    {
        GameObject droppedObject = eventData.pointerDrag;

        if (droppedObject != null)
        {
            DraggableFood food = droppedObject.GetComponent<DraggableFood>();

            if (food != null)
            {
                string foodName = droppedObject.name.Replace("(Clone)", "").Trim();

                // 주문한 음식과 일치할 때
                if (foodName == desiredFoodName)
                {
                    if (dialogueText != null) 
                        dialogueText.text = " 와! 정말 맛있네요! 감사합니다!";
                    
                    Destroy(droppedObject); // 음식 삭제
                    
                    // 성공 후 다음 손님 주문을 받고 싶다면 아래 주석 해제 (1초 뒤 새로운 주문)
                    // Invoke("GenerateRandomOrder", 1.5f);
                }
                else
                {
                    if (dialogueText != null)
                        dialogueText.text = $" 음? 이건 제가 주문한 {desiredFoodName}이 아니에요.";
                }
            }
        }
    }
}