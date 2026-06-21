using UnityEngine;
using UnityEngine.EventSystems;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("오디오 소스 설정")]
    [SerializeField] private AudioSource bgmSource;
    [SerializeField] private AudioSource sfxSource;

    [Header("오디오 클립 등록")]
    [SerializeField] private AudioClip bgmClip;
    [SerializeField] private AudioClip clickSfxClip;

    void Awake()
    {
        // 싱글톤 패턴 적용
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        // BGM 루프 설정 및 재생
        if (bgmSource != null && bgmClip != null)
        {
            bgmSource.clip = bgmClip;
            bgmSource.loop = true; // 루프 재생 활성화
            bgmSource.Play();
        }
    }

    void Update()
    {
        // 마우스 좌클릭 시 효과음 재생
        if (Input.GetMouseButtonDown(0))
        {
            // [옵션] 만약 UI(볼륨 조절 바 등)를 클릭할 때는 효과음 재생을 막고 싶다면 아래 주석을 해제하세요.
            // if (EventSystem.current.IsPointerOverGameObject()) return;

            PlayClickSFX();
        }
    }

    public void PlayClickSFX()
    {
        if (sfxSource != null && clickSfxClip != null)
        {
            sfxSource.PlayOneShot(clickSfxClip);
        }
    }

    // 볼륨 조절 함수 (0.0 ~ 1.0)
    public void SetBGMVolume(float volume)
    {
        if (bgmSource != null)
        {
            bgmSource.volume = Mathf.Clamp01(volume);
        }
    }
    public void SetSFXVolume(float volume)
    {
        if (sfxSource != null) sfxSource.volume = Mathf.Clamp01(volume);
    }
}