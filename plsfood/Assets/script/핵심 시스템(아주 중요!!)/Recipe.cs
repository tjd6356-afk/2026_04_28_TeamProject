using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "New Recipe", menuName = "Cooking/Recipe")]
public class Recipe : ScriptableObject
{
    public string recipeName;
    // 이제 인스펙터에서 Size를 4로 설정하고 재료 이름을 넣으면 됩니다.
    public List<string> ingredients;
    public GameObject resultPrefab;
}