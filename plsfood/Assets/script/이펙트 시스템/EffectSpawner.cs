using UnityEngine;

public class EffectSpawner : MonoBehaviour
{
    [SerializeField] private GameObject effectPrefab;

    [SerializeField] private int effectCount = 30;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            SpawnFirework();
        }
    }

    void SpawnFirework()
    {
        Vector3 mousePosition =
            Camera.main.ScreenToWorldPoint(Input.mousePosition);

        mousePosition.z = 0;

        for (int i = 0; i < effectCount; i++)
        {
            GameObject effect =
                Instantiate(effectPrefab,
                           mousePosition,
                           Quaternion.identity);

            Vector2 dir =
                Random.insideUnitCircle.normalized;

            effect.GetComponent<Effect>()
                  .Initialize(dir);
        }
    }
}