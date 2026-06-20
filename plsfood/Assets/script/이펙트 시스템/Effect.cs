using UnityEngine;

public class Effect : MonoBehaviour
{
    [Header("수명")]
    [SerializeField] private float lifeTime = 0.25f;

    [Header("범위")]
    [SerializeField] private float maxDistance = 0.75f;

    private Vector3 startPos;
    private Vector3 targetPos;

    private Vector3 startScale;

    private SpriteRenderer spriteRenderer;

    private float timer;

    public void Initialize(Vector2 direction)
    {
        startPos = transform.position;

        // 대부분 가까운 거리에서 끝남
        float distance = Mathf.Pow(Random.value, 2f) * maxDistance;

        targetPos = startPos + (Vector3)(direction * distance);
    }

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        float randomSize = Random.Range(0.8f, 1.2f);

        transform.localScale *= randomSize;

        startScale = transform.localScale;
    }

    private void Update()
    {
        timer += Time.deltaTime;

        float t = timer / lifeTime;

        // 빠르게 퍼지고 감속
        float moveCurve = 1f - Mathf.Pow(1f - t, 3f);

        transform.position =
            Vector3.Lerp(startPos, targetPos, moveCurve);

        // 크기 감소
        transform.localScale =
            Vector3.Lerp(startScale, Vector3.zero, t);

        // 투명도 감소
        if (spriteRenderer != null)
        {
            Color c = spriteRenderer.color;
            c.a = 1f - t;
            spriteRenderer.color = c;
        }

        if (timer >= lifeTime)
        {
            Destroy(gameObject);
        }
    }
}