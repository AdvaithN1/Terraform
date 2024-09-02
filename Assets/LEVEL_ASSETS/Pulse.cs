using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

public class Pulse : MonoBehaviour
{

    private Volume v;
    private Bloom b;
    private int count = 0;
    public float MOD = 0.8f;
    private ChromaticAberration ca;
    private LensDistortion ld;
    public AnimationCurve analogIntensityCurve;
    public Image terminalBG;
    public AudioSource bgm;
    private float lastPlayed;
    private float offset = 0;

    void Awake() {
        bgm.clip.LoadAudioData();
    }

    void OnEnable() {
        v = GetComponent<Volume>();
        v.profile.TryGet(out b);
        v.profile.TryGet(out ca);
        v.profile.TryGet(out ld);
        lastPlayed = -10000;
        offset = Time.time;
    }

    void Update()
    {
        if (!bgm.isPlaying && Time.time - lastPlayed > 10000) {
            lastPlayed = Time.time;
            offset = Time.time;
        }
        if (Time.time - offset + 0.268f >= 1.999 / MOD && !bgm.isPlaying) {
            bgm.Play();
        } else if (bgm.isPlaying) {
            if ((int) ((Time.time - offset + 0.268f) / (0.499798f / MOD) + 0.5f) % 4 == 2) {
                ca.intensity.Override(-0.023f + analogIntensityCurve.Evaluate(Mathf.Repeat(Time.time - offset + 0.268f, 0.499798f / MOD)));
                b.intensity.Override(-2.85f + 160 * analogIntensityCurve.Evaluate(Mathf.Repeat(Time.time - offset + 0.268f, 0.499798f / MOD)));
                ld.intensity.Override(-0.122f + 1.2f * analogIntensityCurve.Evaluate(Mathf.Repeat(Time.time - offset + 0.268f, 0.499798f / MOD)));
                terminalBG.color = new Color(0.2f*(analogIntensityCurve.Evaluate(Mathf.Repeat(Time.time - offset + 0.268f, 0.499798f / MOD))-0.05f), 0.7f*(analogIntensityCurve.Evaluate(Mathf.Repeat(Time.time - offset + 0.268f, 0.499798f / MOD))-0.05f), 2.0f * (analogIntensityCurve.Evaluate(Mathf.Repeat(Time.time - offset + 0.268f, 0.499798f / MOD))-0.05f));
            } else if ((int) ((Time.time - offset + 0.268f) / (0.499798f / MOD) + 0.5f) % 4 == 3) {
                ca.intensity.Override(-0.015f + 0.7f * analogIntensityCurve.Evaluate(Mathf.Repeat(Time.time - offset + 0.268f, 0.499798f / MOD)));
                b.intensity.Override(80 * analogIntensityCurve.Evaluate(Mathf.Repeat(Time.time - offset + 0.268f, 0.499798f / MOD)));
                ld.intensity.Override(-0.105f + 0.8f * analogIntensityCurve.Evaluate(Mathf.Repeat(Time.time - offset + 0.268f, 0.499798f / MOD)));
                terminalBG.color = new Color(1.6f*(analogIntensityCurve.Evaluate(Mathf.Repeat(Time.time - offset + 0.268f, 0.499798f / MOD))-0.05f), 0.2f*(analogIntensityCurve.Evaluate(Mathf.Repeat(Time.time - offset + 0.268f, 0.499798f / MOD))-0.05f), 0.2f * (analogIntensityCurve.Evaluate(Mathf.Repeat(Time.time - offset + 0.268f, 0.499798f / MOD))-0.05f));
            } else {
                ca.intensity.Override(0.005f + 0.4f * analogIntensityCurve.Evaluate(Mathf.Repeat(Time.time - offset + 0.268f, 0.499798f / MOD)));
                b.intensity.Override(2.2f + 45 * analogIntensityCurve.Evaluate(Mathf.Repeat(Time.time - offset + 0.268f, 0.499798f / MOD)));
                ld.intensity.Override(-0.08f + 0.45f * analogIntensityCurve.Evaluate(Mathf.Repeat(Time.time - offset + 0.268f, 0.499798f / MOD)));
            }
        }
    }
}