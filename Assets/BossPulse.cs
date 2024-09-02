

using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

public class BossPulse : MonoBehaviour
{

    private Volume v;
    private Bloom b;
    private int count = 0;
    private float MOD = 0.8f;
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
        float T = 60f/143.6f;
        if (!bgm.isPlaying && Time.time - lastPlayed > 10000) {
            lastPlayed = Time.time;
            offset = Time.time;
        }
        if (Time.time - offset + 0.268f >= 4 * T / MOD && !bgm.isPlaying) {
            bgm.Play();
        } else if (bgm.isPlaying) {
            ca.intensity.Override(-0.015f + 0.7f * analogIntensityCurve.Evaluate(Mathf.Repeat(Time.time - offset + 0.268f, T / MOD)));
            b.intensity.Override(80 * analogIntensityCurve.Evaluate(Mathf.Repeat(Time.time - offset + 0.268f, T / MOD)));
            ld.intensity.Override(-0.105f + 0.8f * analogIntensityCurve.Evaluate(Mathf.Repeat(Time.time - offset + 0.268f, T / MOD)));
            terminalBG.color = new Color(2.0f*(analogIntensityCurve.Evaluate(Mathf.Repeat(Time.time - offset + 0.268f, T / MOD))-0.05f), 0.3f*(analogIntensityCurve.Evaluate(Mathf.Repeat(Time.time - offset + 0.268f, T / MOD))-0.05f), 0.3f * (analogIntensityCurve.Evaluate(Mathf.Repeat(Time.time - offset + 0.268f, T / MOD))-0.05f));
        }
    }
}