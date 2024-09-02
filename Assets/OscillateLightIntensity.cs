using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class OscillateLightIntensity : MonoBehaviour
{
    public Light2D light2D;
    public float minIntensity = 1.3f;
    public float maxIntensity = 2.0f;
    public float speed = 0.5f;

    private float time;

    private void Update()
    {
        if (light2D != null)
        {
            time += Time.deltaTime * speed;
            float intensity = (minIntensity + maxIntensity)/2 + 0.9f * (maxIntensity - minIntensity) * Mathf.Sin(time * Mathf.PI);
            light2D.intensity = intensity;
        }
    }
}
