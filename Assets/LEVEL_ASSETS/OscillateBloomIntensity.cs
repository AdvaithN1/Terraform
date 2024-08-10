using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class OscillateBloomIntensity : MonoBehaviour
{
    public Volume globalVolume;
    public float minIntensity = 0.5f;
    public float maxIntensity = 2.0f;
    public float speed = 1.0f;

    private Bloom bloom;
    private float time;

    private void Start()
    {
        if (globalVolume != null && globalVolume.profile.TryGet(out Bloom bloomComponent))
        {
            bloom = bloomComponent;
        }
        else
        {
            Debug.LogError("Bloom component not found on the global volume.");
        }
    }

    private void Update()
    {
        if (bloom != null)
        {
            time += Time.deltaTime * speed;
            float intensity = minIntensity + (maxIntensity - minIntensity) * Mathf.Abs(Mathf.Sin(time * Mathf.PI));
            bloom.intensity.value = intensity;
        }
    }
}
