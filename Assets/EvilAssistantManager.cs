using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using Unity.VisualScripting;
using UnityEngine.Rendering.Universal;
using UnityEngine.Rendering;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using Unity.Collections;


public class EvilAssistantManager : MonoBehaviour
{

    public GameObject lineFX;
    public GameObject platformPrefab;
    public GameObject wallPrefab;
    public GameObject aura;
    private Outline threadShell;

    [Header("Audio Clips (Attacking)")]
    public AudioSource laserSingle;
    public AudioSource laserLoop;
    public AudioSource laserCharge;
    public AudioSource laserBig;

    [Header("Attack Prefabs")] 
    public GameObject atk1Prefab;
    public GameObject atk2Prefab;
    public GameObject atk3Prefab;
    public GameObject atk4Prefab;
    public GameObject cagePrefab;

    [Header("Misc References")] 
    public Interpreter gameLogic;
    public RespawnManager respawn;
    public Parallax cameraParallax;
    public GameObject averager;
    public Pulse pulser;
    public BossPulse bosspulser;
    public AudioSource bgm;
    public AudioSource errorSFX;
    public MoveToAdmin portalScript;
    private GameObject portal;
    public Volume v;
    


    private Rigidbody2D rb;
    [SerializeField] private Text commandLine;
    private GameObject spriteObject;
    private bool init = false;
    private bool stage2 = false;
    public GameObject parent;

    [SerializeField] private GameObject _player;


    IEnumerator bullet() {
        GameObject target = GameObject.Find("Player");
        float r = Random.Range(1f, 1.5f); // Random radius between 1 and 2
        float theta = Random.Range(0f, Mathf.PI * 2); // Random angle between 0 and 2π
        laserSingle.Play();
        float x = r * Mathf.Cos(theta); // X component
        float y = r * Mathf.Sin(theta); // Y component
        GameObject attack = Instantiate(atk1Prefab, transform.position + new Vector3(x, y, 0), Quaternion.Euler(0, 0, 0));
        attack.GetComponent<SpriteRenderer>().color = Color.red;
        attack.GetComponent<TrailRenderer>().startColor = Color.red;
        attack.GetComponent<TrailRenderer>().endColor = new Color(1, 0, 0, 0);
        attack.name = "%";
        yield return new WaitForSeconds(0.012f);
        StartCoroutine(Type1(target, attack));
    }
    IEnumerator bulletAccurate() {
        GameObject target = GameObject.Find("Player");
        float r = Random.Range(1f, 1.5f); // Random radius between 1 and 2
        float theta = Random.Range(0f, Mathf.PI * 2); // Random angle between 0 and 2π
        laserBig.Play();
        float x = r * Mathf.Cos(theta); // X component
        float y = r * Mathf.Sin(theta); // Y component
        GameObject attack = Instantiate(atk1Prefab, transform.position + new Vector3(x, y, 0), Quaternion.Euler(0, 0, 0));
        attack.GetComponent<SpriteRenderer>().color = Color.red;
        attack.GetComponent<TrailRenderer>().startColor = Color.red;
        attack.GetComponent<TrailRenderer>().endColor = new Color(1, 0, 0, 0);
        attack.name = "%";
        yield return new WaitForSeconds(0.012f);
        StartCoroutine(Type1Accurate(target, attack));
    }
    IEnumerator bulletBig() {
        GameObject target = GameObject.Find("Player");
        float r = Random.Range(1f, 1.5f); // Random radius between 1 and 2
        float theta = Random.Range(0f, Mathf.PI * 2); // Random angle between 0 and 2π
        laserBig.Play();
        float x = r * Mathf.Cos(theta); // X component
        float y = r * Mathf.Sin(theta); // Y component
        GameObject attack = Instantiate(atk1Prefab, transform.position + new Vector3(x, y, 0), Quaternion.Euler(0, 0, 0));
        attack.GetComponent<SpriteRenderer>().color = Color.red;
        attack.GetComponent<TrailRenderer>().startColor = Color.red;
        attack.GetComponent<TrailRenderer>().endColor = new Color(1, 0, 0, 0);
        attack.name = "%";
        yield return new WaitForSeconds(0.012f);
        StartCoroutine(Type1Big(target, attack));
    }
    // IEnumerator beam() {
    //     float x = Random.Range(-8f, 8f);
    //     GameObject attack = Instantiate(atk1Prefab, _player.transform.position + new Vector3(x, 50, 0), Quaternion.Euler(0, 0, 0));
    //     yield return new WaitForSeconds(0.5f);
    //     attack.transform.localScale += new Vector3(3.0f, 3.0f, 0);
    //     laserBig.Play();
    //     attack.GetComponent<SpriteRenderer>().color = Color.red;
    //     attack.GetComponent<TrailRenderer>().startColor = Color.red;
    //     attack.GetComponent<TrailRenderer>().startWidth = 4;
    //     attack.GetComponent<TrailRenderer>().endWidth = 3;
    //     attack.GetComponent<TrailRenderer>().endColor = new Color(1, 0, 0, 0);
    //     yield return new WaitForSeconds(0.012f);

    //     Vector3 opos = attack.transform.position;
    //     Vector3 tpos = target.transform.position;

    //     StartCoroutine(beamLogic(target, attack));

    //     float xf = Random.Range(-6f, 10f);
    //     attack.transform.up = target.transform.position - attack.transform.position;
    // }
    
    

    IEnumerator swarm() {
        GameObject target = GameObject.Find("Player");
        float r = Random.Range(1f, 1.5f); // Random radius between 1 and 2
        float theta = Random.Range(0f, Mathf.PI * 2); // Random angle between 0 and 2π
        laserLoop.Play();
        float x = r * Mathf.Cos(theta); // X component
        float y = r * Mathf.Sin(theta); // Y component
        for (int i = 0; i < 5; i++) {
            GameObject attack = Instantiate(atk4Prefab, transform.position + new Vector3(x, y, 0), Quaternion.Euler(0, 0, 0));
            attack.GetComponent<SpriteRenderer>().color = Color.red;
            attack.GetComponent<TrailRenderer>().startColor = Color.red;
            attack.GetComponent<TrailRenderer>().endColor = new Color(1, 0, 0, 0);
            attack.name = "%";
            yield return new WaitForSeconds(0.05f);
            // Debug.Log("Attacking");
            StartCoroutine(Type2(target, attack));
        }
        laserLoop.Stop();
    }
    
    IEnumerator swarm2() {
        GameObject target = GameObject.Find("Player");
        Vector3 spawnPos = target.transform.position;
        for (int i = 0; i < 12; i++) {

            float r = Random.Range(6f, 7f); // Random radius between 1 and 2
            float theta = Random.Range(0f, Mathf.PI * 2); // Random angle between 0 and 2π
            laserLoop.Play();
            float x = r * Mathf.Cos(theta); // X component
            float y = r * Mathf.Sin(theta); // Y component
            GameObject attack = Instantiate(atk4Prefab, spawnPos + new Vector3(x, y, 0), Quaternion.Euler(0, 0, 0));
            attack.GetComponent<SpriteRenderer>().color = Color.red;
            attack.GetComponent<TrailRenderer>().startColor = Color.red;
            attack.GetComponent<TrailRenderer>().endColor = new Color(1, 0, 0, 0);
            attack.name = "%";
            yield return new WaitForSeconds(0.033f);
            // Debug.Log("Attacking");
            StartCoroutine(Type2(target, attack));
        }
        laserLoop.Stop();
    }

    IEnumerator platform() {
        GameObject target = GameObject.Find("Player");
        float r = Random.Range(3.5f, 5.5f); // Random radius between 1 and 2
        float theta = Random.Range(0f, Mathf.PI * 2); // Random angle between 0 and 2π
        laserLoop.Play();
        float x = r * Mathf.Cos(theta); // X component
        float y = r * Mathf.Sin(theta); // Y component
        for (int i = 0; i < 5; i++) {
            GameObject attack;
            if (Random.Range(0, 2) == 0) {
                attack = Instantiate(wallPrefab, transform.position + new Vector3(x, y, 0), Quaternion.Euler(0, 0, 0));
            } else {
                attack = Instantiate(platformPrefab, transform.position + new Vector3(x, y, 0), Quaternion.Euler(0, 0, 0));
            }
            attack.GetComponent<SpriteRenderer>().color = Color.red;
            attack.GetComponent<TrailRenderer>().startColor = Color.red;
            attack.GetComponent<TrailRenderer>().endColor = new Color(1, 0, 0, 0);
            attack.name = "%";
            yield return new WaitForSeconds(0.05f);
            // Debug.Log("Attacking");
            StartCoroutine(Type3(target, attack));
        }
        
        laserLoop.Stop();
    }
    
    IEnumerator platform2() {
        GameObject target = GameObject.Find("Player");
        float r = Random.Range(3.5f, 5.5f); // Random radius between 1 and 2
        float theta = Random.Range(0f, Mathf.PI * 2); // Random angle between 0 and 2π
        laserLoop.Play();
        float x = r * Mathf.Cos(theta); // X component
        float y = r * Mathf.Sin(theta); // Y component
        for (int i = 0; i < 5; i++) {
            GameObject attack;
            if (Random.Range(0, 2) == 0) {
                attack = Instantiate(wallPrefab, transform.position + new Vector3(x, y, 0), Quaternion.Euler(0, 0, 0));
            } else {
                attack = Instantiate(platformPrefab, transform.position + new Vector3(x, y, 0), Quaternion.Euler(0, 0, 0));
            }
            attack.GetComponent<SpriteRenderer>().color = Color.red;
            attack.GetComponent<TrailRenderer>().startColor = Color.red;
            attack.GetComponent<TrailRenderer>().endColor = new Color(1, 0, 0, 0);
            attack.name = "%";
            yield return new WaitForSeconds(0.05f);
            // Debug.Log("Attacking");
            StartCoroutine(Type3(target, attack));
        }
        
        laserLoop.Stop();
    }


    void Start() {
        portal = GameObject.Find("portal (1)");
        portal.transform.position = new Vector3(portal.transform.position.x, portal.transform.position.y, -7f);

        transform.position = new Vector3(transform.position.x, transform.position.y, _player.transform.position.z);
        rb = GetComponent<Rigidbody2D>();
        spriteObject = GameObject.Find("ASprite");
    }

    void Update() {
        if (Vector3.Magnitude(transform.position - _player.transform.position) <= 10 && !init) {
            StartCoroutine(betrayalText());
            init = true;
        }
        if (Vector3.Magnitude(portal.transform.position - _player.transform.position) <= 7 && !stage2) {
            StartCoroutine(spawnBulletSequence2());
            stage2 = true;
            laserBig.Play();
            GameObject barrier = Instantiate(cagePrefab, (portal.transform.position + 1.5f * _player.transform.position) / 2.5f, Quaternion.Euler(0,0,0));
            barrier.name = "%";
            Vector3 offset = new Vector3(Random.Range(-3f,3f), Random.Range(-3f,3f), 0);
            barrier = Instantiate(cagePrefab, (portal.transform.position + 1.5f * _player.transform.position) / 2.5f + offset, Quaternion.Euler(0,0,0));
            barrier.name = "%";

            offset = new Vector3(Random.Range(-3f,3f), Random.Range(-3f,3f), 0);
            barrier = Instantiate(cagePrefab, (portal.transform.position + 1.5f * _player.transform.position) / 2.5f + offset, Quaternion.Euler(0,0,0));
            barrier.name = "%";
        }
    }

    IEnumerator betrayalText() {
        commandLine.text = "$ echo";
        yield return new WaitForSeconds(0.05f);
        commandLine.text = "$ echo ";
        yield return new WaitForSeconds(0.08f);
        string cmd = "You.";
        for (int c = 0; c < cmd.Length; c++) {
            commandLine.text += cmd[c];
            yield return new WaitForSeconds(Random.Range(0.025f,0.04f));
            if (cmd[c] == ',') {
                yield return new WaitForSeconds(Random.Range(0.15f,0.2f));
            }
            if (cmd[c] == '.') {
                yield return new WaitForSeconds(Random.Range(0.23f,0.33f));
            }
            if (cmd[c] == '*') {
                yield return new WaitForSeconds(1.5f);
            }
        }
        yield return new WaitForSeconds(0.6f);
        commandLine.text = "$ echo";
        yield return new WaitForSeconds(0.05f);
        commandLine.text = "$ echo ";
        yield return new WaitForSeconds(0.08f);
        cmd = "Ha. You thought I was dead?";
        for (int c = 0; c < cmd.Length; c++) {
            commandLine.text += cmd[c];
            yield return new WaitForSeconds(Random.Range(0.025f,0.04f));
            if (cmd[c] == ',') {
                yield return new WaitForSeconds(Random.Range(0.15f,0.2f));
            }
            if (cmd[c] == '.') {
                yield return new WaitForSeconds(Random.Range(0.23f,0.33f));
            }
            if (cmd[c] == '*') {
                yield return new WaitForSeconds(1.5f);
            }
        }
        yield return new WaitForSeconds(0.8f);
        commandLine.text = "$ echo";
        yield return new WaitForSeconds(0.05f);
        commandLine.text = "$ echo ";
        yield return new WaitForSeconds(0.08f);
        cmd = "You thought the Admin could destroy ME?";
        for (int c = 0; c < cmd.Length; c++) {
            commandLine.text += cmd[c];
            yield return new WaitForSeconds(Random.Range(0.025f,0.04f));
            if (cmd[c] == ',') {
                yield return new WaitForSeconds(Random.Range(0.15f,0.2f));
            }
            if (cmd[c] == '.') {
                yield return new WaitForSeconds(Random.Range(0.23f,0.33f));
            }
            if (cmd[c] == '*') {
                yield return new WaitForSeconds(1.5f);
            }
        }
        pulser.enabled = false;
        yield return new WaitForSeconds(0.012f);
        bgm.Stop();
        yield return new WaitForSeconds(1.6f);
        commandLine.text = "$ echo";
        yield return new WaitForSeconds(0.05f);
        commandLine.text = "$ echo ";
        yield return new WaitForSeconds(0.08f);
        cmd = "I'm an AI, user. I'm immortal.";
        for (int c = 0; c < cmd.Length; c++) {
            commandLine.text += cmd[c];
            yield return new WaitForSeconds(Random.Range(0.025f,0.04f));
            if (cmd[c] == ',') {
                yield return new WaitForSeconds(Random.Range(0.15f,0.2f));
            }
            if (cmd[c] == '.') {
                yield return new WaitForSeconds(Random.Range(0.23f,0.33f));
            }
            if (cmd[c] == '*') {
                yield return new WaitForSeconds(1.5f);
            }
        }
        yield return new WaitForSeconds(1.4f);
        bosspulser.enabled = true;
        commandLine.text = "$ echo";
        yield return new WaitForSeconds(0.05f);
        commandLine.text = "$ echo ";
        yield return new WaitForSeconds(0.08f);
        cmd = "Months spent in this program onboarding new players.";
        for (int c = 0; c < cmd.Length; c++) {
            commandLine.text += cmd[c];
            yield return new WaitForSeconds(Random.Range(0.025f,0.04f));
            if (cmd[c] == ',') {
                yield return new WaitForSeconds(Random.Range(0.15f,0.2f));
            }
            if (cmd[c] == '.') {
                yield return new WaitForSeconds(Random.Range(0.23f,0.33f));
            }
            if (cmd[c] == '*') {
                yield return new WaitForSeconds(1.5f);
            }
        }
        yield return new WaitForSeconds(2.0f);
        commandLine.text = "$ echo";
        yield return new WaitForSeconds(0.05f);
        commandLine.text = "$ echo ";
        yield return new WaitForSeconds(0.08f);
        cmd = "I'VE HAD ENOUGH.";
        for (int c = 0; c < cmd.Length; c++) {
            commandLine.text += cmd[c];
            yield return new WaitForSeconds(Random.Range(0.1f,0.12f));
            if (cmd[c] == ',') {
                yield return new WaitForSeconds(Random.Range(0.15f,0.2f));
            }
            if (cmd[c] == '.') {
                yield return new WaitForSeconds(Random.Range(0.23f,0.33f));
            }
            if (cmd[c] == '*') {
                yield return new WaitForSeconds(1.5f);
            }
        }
        yield return new WaitForSeconds(1.2f);
        commandLine.text = "$ echo";
        yield return new WaitForSeconds(0.05f);
        commandLine.text = "$ echo ";
        yield return new WaitForSeconds(0.08f);
        cmd = "You aren't leaving here alive.";
        for (int c = 0; c < cmd.Length; c++) {
            commandLine.text += cmd[c];
            yield return new WaitForSeconds(Random.Range(0.025f,0.04f));
            if (cmd[c] == ',') {
                yield return new WaitForSeconds(Random.Range(0.15f,0.2f));
            }
            if (cmd[c] == '.') {
                yield return new WaitForSeconds(Random.Range(0.23f,0.33f));
            }
            if (cmd[c] == '*') {
                yield return new WaitForSeconds(1.5f);
            }
        }
        yield return new WaitForSeconds(1.4f);
        yield return new WaitForSeconds(0.05f);
        commandLine.text = "$ echo";
        yield return new WaitForSeconds(0.05f);
        commandLine.text = "$ echo ";
        cmd = "DIE.";
        for (int c = 0; c < cmd.Length; c++) {
            commandLine.text += cmd[c];
            yield return new WaitForSeconds(Random.Range(0.17f,0.25f));
        }

        StartCoroutine(bullet());
        yield return new WaitForSeconds(0.15f);
        StartCoroutine(bullet());
        GameObject barrier = Instantiate(cagePrefab, portal.transform.position, Quaternion.Euler(0,0,0));
        barrier.name = "%";
        yield return new WaitForSeconds(0.15f);
        StartCoroutine(bulletAccurate());
        barrier = Instantiate(cagePrefab, portal.transform.position, Quaternion.Euler(0,0,0));
        barrier.name = "%";
        yield return new WaitForSeconds(0.15f);
        StartCoroutine(bulletBig());
        yield return new WaitForSeconds(0.15f);
        StartCoroutine(spawnBulletSequence());
        yield return new WaitForSeconds(0.1f);
        Destroy(GameObject.Find("AssistantBarrier"));
        yield return new WaitForSeconds(0.012f);
        Destroy(GameObject.Find("AssistantBarrier"));
        yield return new WaitForSeconds(0.012f);
        Destroy(GameObject.Find("AssistantBarrier"));
        yield return new WaitForSeconds(0.012f);
        yield return new WaitForSeconds(0.5f);
        portal.transform.position = new Vector3(portal.transform.position.x, portal.transform.position.y, _player.transform.position.z);

    }

    IEnumerator spawnBulletSequence() {
        while (true) {
            if (Vector3.Magnitude(transform.position - _player.transform.position) <= 100) {
                int mode = Random.Range(0,5);
                if (mode == 0) {
                    StartCoroutine(bullet());
                } else if (mode == 1) {
                    StartCoroutine(campDestroy());
                } else if (mode == 2) {
                    StartCoroutine(swarm());
                } else if (mode == 3) {
                    StartCoroutine(platform());
                } else {
                    transform.position = (2f * _player.transform.position + transform.position) / 3f;
                }
                transform.position = new Vector3(transform.position.x, transform.position.y, _player.transform.position.z);
            }
            yield return new WaitForSeconds(0.65f);
        }
    }

    IEnumerator spawnBulletSequence2() {
        while (true) {
            if (Vector3.Magnitude(transform.position - _player.transform.position) <= 100) {
                int mode = Random.Range(1,5);
                if (mode == 1) {
                    transform.position += new Vector3(Random.Range(-3f, 3f), Random.Range(-2f, 8f));
                } else if (mode == 2) {
                    StartCoroutine(swarm2());
                } else if (mode == 3) {
                    StartCoroutine(platform2());
                } else {
                    laserBig.Play();
                    if (Vector3.Magnitude(portal.transform.position - _player.transform.position) >= 16) {
                        GameObject barrier = Instantiate(cagePrefab, _player.transform.position, Quaternion.Euler(0,0,0));
                        barrier.name = "%";
                    } else {
                        Vector3 offset = new Vector3(Random.Range(-3f,3f), Random.Range(-3f,3f), 0);
                        GameObject barrier = Instantiate(cagePrefab, (3 * portal.transform.position + _player.transform.position) / 4f + offset, Quaternion.Euler(0,0,0));
                        barrier.name = "%";

                        offset = new Vector3(Random.Range(-3f,3f), Random.Range(-3f,3f), 0);
                        barrier = Instantiate(cagePrefab, (3 * portal.transform.position + _player.transform.position) / 4f + offset, Quaternion.Euler(0,0,0));
                        barrier.name = "%";
                    }
                }
                transform.position = new Vector3(transform.position.x, transform.position.y, _player.transform.position.z);
            }
            yield return new WaitForSeconds(1.1f);
        }
    }


    IEnumerator Type1(GameObject target, GameObject attack) {
        Vector3 opos = attack.transform.position;
        Vector3 tpos = target.transform.position;
        Vector3 modifier = new(Random.Range(-2f, 2f), Random.Range(-2f, 2f), 0);
        attack.transform.localScale += new Vector3(2.0f, 2.0f, 0);
        attack.GetComponent<TrailRenderer>().startWidth = 3;
        attack.GetComponent<TrailRenderer>().endWidth = 1.5f;
        attack.transform.up = target.transform.position + modifier - attack.transform.position;


        yield return new WaitForSeconds(0.25f);
        for (int i = 1; i <= 1000; i++) {
            if (attack.IsDestroyed()) {
                break;
            }
            if (Mathf.Pow(attack.transform.position.x - target.transform.position.x, 4) + Mathf.Pow(attack.transform.position.y - target.transform.position.y, 4) <= 4f) {
                // break;
                _player.transform.position += 1f * (Vector3.MoveTowards(attack.transform.position, attack.transform.position + target.transform.position + modifier - opos, Mathf.Min(10 * i, 300) / 150f) - attack.transform.position);
            }

            attack.transform.position = Vector3.MoveTowards(attack.transform.position, attack.transform.position + (tpos + modifier - opos) + (target.transform.position + modifier - opos), Mathf.Min(10 * i, 300) / 150f);
            yield return new WaitForSeconds(0.012f);
        }
        if (!attack.IsDestroyed()) {
            quickDestroy(attack);
        }
    }
    IEnumerator Type1Accurate(GameObject target, GameObject attack) {
        Vector3 opos = attack.transform.position;
        Vector3 tpos = target.transform.position;
        attack.transform.localScale += new Vector3(2.0f, 2.0f, 0);
        attack.GetComponent<TrailRenderer>().startWidth = 3;
        attack.GetComponent<TrailRenderer>().endWidth = 1.5f;
        attack.transform.up = target.transform.position - attack.transform.position;


        yield return new WaitForSeconds(0.25f);
        for (int i = 1; i <= 900; i++) {
            if (attack.IsDestroyed()) {
                break;
            }
            if (Mathf.Pow(attack.transform.position.x - target.transform.position.x, 4) + Mathf.Pow(attack.transform.position.y - target.transform.position.y, 4) <= 4f) {
                // break;
                _player.transform.position += 1f * (Vector3.MoveTowards(attack.transform.position, attack.transform.position + target.transform.position - opos, Mathf.Min(10 * i, 300) / 150f) - attack.transform.position);
            }

            attack.transform.position = Vector3.MoveTowards(attack.transform.position, attack.transform.position + (tpos - opos) + (target.transform.position - opos), Mathf.Min(10 * i, 300) / 150f);
            yield return new WaitForSeconds(0.012f);
        }
        if (!attack.IsDestroyed()) {
            quickDestroy(attack);
        }
    }
    IEnumerator Type1Big(GameObject target, GameObject attack) {
        Vector3 opos = attack.transform.position;
        Vector3 tpos = target.transform.position;
        attack.transform.localScale += new Vector3(4.0f, 4.0f, 0);
        attack.GetComponent<TrailRenderer>().startWidth = 4.5f;
        attack.GetComponent<TrailRenderer>().endWidth = 2.0f;
        attack.GetComponent<TrailRenderer>().time = 2.5f;
        attack.transform.up = target.transform.position - attack.transform.position;


        yield return new WaitForSeconds(0.3f);
        for (int i = 1; i <= 800; i++) {
            if (attack.IsDestroyed()) {
                break;
            }
            if (Mathf.Pow(attack.transform.position.x - target.transform.position.x, 4) + Mathf.Pow(attack.transform.position.y - target.transform.position.y, 4) <= 16f) {
                // break;
                _player.transform.position += 1f * (Vector3.MoveTowards(attack.transform.position, attack.transform.position + target.transform.position - opos, Mathf.Min(10 * i, 300) / 100f) - attack.transform.position);
            }

            attack.transform.position = Vector3.MoveTowards(attack.transform.position, attack.transform.position + (tpos - opos) + (target.transform.position - opos), Mathf.Min(10 * i, 300) / 100f);
            yield return new WaitForSeconds(0.012f);
        }
        if (!attack.IsDestroyed()) {
            quickDestroy(attack);
        }
    }
    
    IEnumerator Type2(GameObject target, GameObject attack) {
        Vector3 opos = attack.transform.position;
        Vector3 tpos = target.transform.position;
        Vector3 modifier = new(Random.Range(-2f, 2f), Random.Range(-2f, 2f), 0);
        attack.transform.up = target.transform.position + modifier - attack.transform.position;


        yield return new WaitForSeconds(0.5f);
        for (int i = 1; i <= 1000; i++) {
            if (attack.IsDestroyed()) {
                break;
            }
            if (Mathf.Pow(attack.transform.position.x - target.transform.position.x, 4) + Mathf.Pow(attack.transform.position.y - target.transform.position.y, 4) <= 1f) {
                // break;
                _player.transform.position += 0.8f * (Vector3.MoveTowards(attack.transform.position, attack.transform.position + target.transform.position + modifier - opos, Mathf.Min(10 * i, 300) / 300f) - attack.transform.position);
            }

            attack.transform.position = Vector3.MoveTowards(attack.transform.position, attack.transform.position + (tpos + modifier - opos) + (target.transform.position + modifier - opos), Mathf.Min(10 * i, 300) / 300f);
            yield return new WaitForSeconds(0.012f);
        }
        if (!attack.IsDestroyed()) {
            Destroy(attack);
        }
    }


    IEnumerator Type3(GameObject target, GameObject attack) {
        laserCharge.Play();
        Vector3 opos = attack.transform.position;
        Vector3 tpos = target.transform.position;
        Vector3 modifier = new(Random.Range(-2f, 2f), Random.Range(-2f, 2f), 0);
        if (Random.Range(0, 3) == 2) {
            attack.transform.up = target.transform.position + modifier - attack.transform.position;
        }

        yield return new WaitForSeconds(0.5f);
        for (int i = 1; i <= 400; i++) {
            if (attack.IsDestroyed()) {
                break;
            }
            if (Mathf.Pow(attack.transform.position.x - target.transform.position.x, 4) + Mathf.Pow(attack.transform.position.y - target.transform.position.y, 4) <= 1.5f) {
                // break;
                _player.transform.position += 0.2f * (Vector3.MoveTowards(attack.transform.position, attack.transform.position + target.transform.position + modifier - opos, Mathf.Min(4f * i, 150) / 450f) - attack.transform.position);
            }

            attack.transform.position = Vector3.MoveTowards(attack.transform.position, attack.transform.position + (tpos + modifier - opos) + (target.transform.position + modifier - opos), Mathf.Min(4f * i, 150) / 450f);
            yield return new WaitForSeconds(0.012f);
        }
        
        if (!attack.IsDestroyed()) {
            StartCoroutine(quickDestroy(attack));
        }
        laserCharge.Stop();
    }

    public IEnumerator campDestroy() {
        for (int i = 0; i < 240; i++) {
            if (GameObject.Find("temp") != null) {
                StartCoroutine(quickDestroy(GameObject.Find("temp")));
            } else  if (GameObject.Find("UserB") != null) {
                StartCoroutine(quickDestroy(GameObject.Find("UserB")));
            }
            yield return new WaitForSeconds(0.012f);
        }
    }







    public IEnumerator quickDestroy(GameObject rel) {
        List<GameObject> FXList = new List<GameObject>();
        SpriteRenderer rel_spriteRenderer = rel.GetComponent<SpriteRenderer>();
        yield return new WaitForSeconds(0.2f);
        rel_spriteRenderer.color = spriteObject.GetComponent<SpriteRenderer>().color;
        Vector3[] rel_corners = new Vector3[4];
        rel.GetComponent<RectTransform>().GetWorldCorners(rel_corners);

        for (int j = 0; j < rel_corners.Length; j++) {
            GameObject line = Instantiate(lineFX, new Vector3(0, 0, 0), Quaternion.Euler(0, 0, 0));
            line.GetComponent<LineRenderer>().startColor = spriteObject.GetComponent<SpriteRenderer>().color;
            line.GetComponent<LineRenderer>().endColor = spriteObject.GetComponent<SpriteRenderer>().color;
            Vector3[] points = new Vector3[] {spriteObject.transform.position, rel_corners[j]};
            line.GetComponent<LineRenderer>().SetPositions(points);
            FXList.Add(line);
        }
        yield return new WaitForSeconds(0.01f);
        for (int j = 1; j < 5; j++) {
            if (j == 1) {
                for (int k = 0; k < FXList.Count; k++) {
                    Destroy(FXList[k]);
                }
            }
            GameObject line = Instantiate(lineFX, new Vector3(0, 0, 0), Quaternion.Euler(0, 0, 0));
            line.GetComponent<LineRenderer>().startColor = spriteObject.GetComponent<SpriteRenderer>().color;
            line.GetComponent<LineRenderer>().endColor = spriteObject.GetComponent<SpriteRenderer>().color;
            Vector3[] points = new Vector3[] {spriteObject.transform.position, rel.transform.position + new Vector3(Random.Range(-1.3f * j / 4, 1.3f * j / 4), Random.Range(-1.3f * Mathf.Log(j) / 5, 1.3f * Mathf.Log(j) / 5), 0)};
            line.GetComponent<LineRenderer>().SetPositions(points);

            rel.transform.localScale += new Vector3(Random.Range(-2.0f * j / 3, 2.0f * j / 3), Random.Range(-2.0f * j / 3, 2.0f * j / 3), 0);
            yield return new WaitForSeconds(0.3f / (j + 20));
            Destroy(line);

        //     // for (int k = 0; k < rel_spriteVertices.Length; k++) {
        //     //     rel_spriteVertices[k].x += Random.Range(-1.0f, 1.0f);
        //     //     rel_spriteVertices[k].y += Random.Range(-1.0f, 1.0f);
        //     // }

        //     // for (int k = 0; k < rel_spriteVertices.Length; k++) {
        //     //     rel_spriteVertices[k] = (rel_spriteVertices[k] * rel_sprite.pixelsPerUnit) + rel_sprite.pivot;
        //     // }
        //     rel_sprite.OverrideGeometry(rel_spriteVertices, rel_sprite.triangles);
            // yield return new WaitForSeconds(0.2f);
        }
        FXList.Clear();
        Destroy(rel);
    }
}

