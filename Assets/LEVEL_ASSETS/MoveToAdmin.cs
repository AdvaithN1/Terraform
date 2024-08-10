using UnityEngine;
using System.Collections;
using UnityEngine.Rendering.Universal;
using UnityEngine.Rendering;

public class MoveToAdmin : MonoBehaviour
{
    public GameObject target; 
    public Volume v;
    private ChromaticAberration ca;
    private Bloom b;
    public AudioSource glitch;
    private float duration = 5f;

    public void Start() {
        v.profile.TryGet(out b);
        v.profile.TryGet(out ca);
    }

    public void StartRoutine()
    {
        glitch.Play();
        gameObject.GetComponent<AudioSource>().Play();
        StartCoroutine(MoveAndScaleOverTime());
    }

    IEnumerator MoveAndScaleOverTime()
    {
        Vector3 startPosition = transform.position;
        Vector3 startScale = transform.localScale;
        Vector3 targetPosition = target.transform.position;
        Vector3 targetScale = target.transform.localScale;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            if (elapsedTime >= 0.2 && glitch.isPlaying) {
                glitch.Stop();
            }

            elapsedTime += Time.deltaTime;
            float t = elapsedTime / duration;
            transform.position = Vector3.Lerp(startPosition, targetPosition, t);
            transform.localScale = Vector3.Lerp(startScale, targetScale, t);
            b.intensity.Override(b.intensity.value - 0.01f);
            ca.intensity.Override(ca.intensity.value * 0.998f);

            if (elapsedTime <= duration / 4) {
                yield return null;
            }
            if (elapsedTime <= duration / 8) {
                yield return null;
            }
            if (elapsedTime <= duration / 16) {
                yield return null;
            }
            if (elapsedTime <= duration / 32) {
                yield return null;
            }
            if (elapsedTime <= duration / 64) {
                yield return null;
            }
            if (elapsedTime <= duration / 128) {
                yield return null;
            }
            yield return null;
        }

        transform.position = targetPosition;
        transform.localScale = targetScale;
        transform.position += new Vector3(500, 500, 100);
        gameObject.GetComponent<SpriteRenderer>().sortingOrder = -100;
        gameObject.layer = 0;
        Destroy(gameObject.GetComponent<BoxCollider2D>());
        target.SetActive(true);
        for (int i = 0; i <= 30; i++) {
            b.intensity.Override(b.intensity.value + 2f + i / 20f);
            ca.intensity.Override(ca.intensity.value + 0.08f + i / 300f);
            yield return null;
        }

        foreach (ParticleSystem ps in gameObject.GetComponentsInChildren<ParticleSystem>()) {
            Destroy(ps.gameObject);
        }

        for (int i = 30; i >= 20; i--) {
            b.intensity.Override(b.intensity.value - 2f - i / 20f);
            ca.intensity.Override(ca.intensity.value - 0.08f - i / 300f);
            yield return null;
        }

        for (int i = 0; i >= 200; i--) {
            b.intensity.Override(b.intensity.value - 0.043f);
            ca.intensity.Override(ca.intensity.value - 0.008f);
            yield return null;
        }

        for (int i = 800; i >= 0; i--) {
            b.intensity.Override(b.intensity.value - 0.043f);
            ca.intensity.Override(ca.intensity.value - i/200000f);
            yield return null;
        }
        yield return new WaitForSeconds(5f);
        gameObject.SetActive(false);
    }
}
