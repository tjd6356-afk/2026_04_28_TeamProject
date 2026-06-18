using UnityEngine;

public class Effect : MonoBehaviour
{
    [Header("수명")]
    [SerializeField] private float lifetime = 1.0f;

    [Header("이동 거리")]
    [SerializeField] private float minDistance = 0.5f;
    [SerializeField] private float maxDistance = 3.0f;

    private Vector3 startPos;
    private Vector3 targetPos;

    private float timer;

    private SpriteRenderer spriteRenderer;

    private Vector3 startScale;

    public void Initialize(Vector2 direction)
    {
        startPos = transform.position;

        float distance = Random.Range(minDistance, maxDistance);

        targetPos = startPos + (Vector3)(direction.normalized * distance);
    }

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        startScale = transform.localScale;

        float randomSize = Random.Range(0.5f, 1.5f);
        transform.localScale *= randomSize;
        startScale = transform.localScale;
    }

    void Update()
    {
        timer += Time.deltaTime;

        float t = timer / lifetime;

        // 이동
        float curve = 1f - Mathf.Pow(1f - t, 2f);

        transform.position =
            Vector3.Lerp(startPos, targetPos, curve);

        // 크기 감소
        transform.localScale = Vector3.Lerp(
            startScale,
            Vector3.zero,
            t
        );

        // 투명도 감소
        if (spriteRenderer != null)
        {
            Color c = spriteRenderer.color;
            c.a = Mathf.Lerp(1f, 0f, t);
            spriteRenderer.color = c;
        }

        if (timer >= lifetime)
        {
            Destroy(gameObject);
        }
    }
}