using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "New Customer Data", menuName = "Cooking/Customer Data")]
public class CustomerData : ScriptableObject
{
    public string customerName; // 손님 이름
    
    [TextArea(3, 5)] // 인스펙터 창에서 대사를 넓게 입력할 수 있게 해줍니다.
    public List<string> greetings; // 손님의 기본 인사 대사 리스트
}