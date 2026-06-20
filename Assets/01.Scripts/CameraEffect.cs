using System.Collections;
using UnityEngine;
using Unity.Cinemachine;
using DG.Tweening;

public class CameraEffect : MonoBehaviour
{
    public static CameraEffect Inst;

    [Header("시네머신 설정")]
    [SerializeField] private CinemachineCamera virtualCamera;

    private float defaultFOV;
    private Coroutine currentEffectCoroutine;
    private Tweener zoomTweener;

    private void Awake()
    {
        Inst = this;

        if (virtualCamera == null)
        {
            virtualCamera = GameObject.FindAnyObjectByType<CinemachineCamera>();
        }

        if (virtualCamera != null)
        {
            defaultFOV = virtualCamera.Lens.FieldOfView;
        }
    }

    public void PlayHitEffect(float slowDuration, float slowTimeScale, float zoomFOV)
    {
        if (currentEffectCoroutine != null)
        {
            StopCoroutine(currentEffectCoroutine);
        }

        if (zoomTweener != null && zoomTweener.IsActive())
        {
            zoomTweener.Kill();
        }

        currentEffectCoroutine = StartCoroutine(HitEffectRoutine(slowDuration, slowTimeScale, zoomFOV));
    }

    private IEnumerator HitEffectRoutine(float duration, float timeScale, float zoomFOV)
    {
        Time.timeScale = timeScale;
        Time.fixedDeltaTime = 0.02f * Time.timeScale;

        if (virtualCamera != null)
        {
            zoomTweener = DOTween.To(
                () => virtualCamera.Lens.FieldOfView,
                x => virtualCamera.Lens.FieldOfView = x,
                zoomFOV,
                duration * 0.35f
            )
            .SetUpdate(true)
            .SetEase(Ease.OutCubic);
        }

        yield return new WaitForSecondsRealtime(duration);

        Time.timeScale = 1.0f;
        Time.fixedDeltaTime = 0.02f;

        if (virtualCamera != null)
        {
            virtualCamera.Lens.FieldOfView = defaultFOV;
        }
    }

    private void OnDestroy()
    {
        if (zoomTweener != null && zoomTweener.IsActive())
        {
            zoomTweener.Kill();
        }
    }
}