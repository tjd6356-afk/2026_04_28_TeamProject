using UnityEngine;

public class EffectSpawner : MonoBehaviour
{
    [SerializeField] private GameObject effectPrefab;
    [SerializeField] private RectTransform effectParent;
    [Header("파티클 수")]
    [SerializeField] private int effectCount = 12;

    private Camera cam;

    private void Start()
    {
        cam = Camera.main;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            SpawnClickEffect();
        }
    }

    private void SpawnClickEffect()
    {
        Vector3 mousePos =
            cam.ScreenToWorldPoint(Input.mousePosition);

        mousePos.z = 0f;

        for (int i = 0; i < effectCount; i++)
        {
            GameObject effect =
                Instantiate(effectPrefab,
                           mousePos,
                           Quaternion.identity);

            Vector2 direction =
                Random.insideUnitCircle.normalized;

            effect.GetComponent<Effect>()
                  .Initialize(direction);
        }
    }
}