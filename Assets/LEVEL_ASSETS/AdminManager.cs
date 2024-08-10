using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using Unity.VisualScripting;
using UnityEditor.EditorTools;
using UnityEngine.Rendering.Universal;
using UnityEngine.Rendering;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor.Search;
using UnityEditor;
using Unity.Collections;


public class AdminManager : MonoBehaviour
{

    public GameObject lineFX;
    public GameObject platformPrefab;
    public GameObject wallPrefab;
    public GameObject aura;
    private Outline threadShell;
    public GameObject assistant;
    public GameObject assistantSprite;

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
    public AudioSource bgm;
    public AudioSource errorSFX;
    public MoveToAdmin portalScript;
    

    private Vector3 globalVector;

    public Volume v;
    public GameObject overlay;
    private ChromaticAberration ca;
    private Bloom b;

    private AssistantManager am;

    public AudioSource[] glitchRandom;
    public AudioSource[] glitchBlip;
    public AudioSource[] glitchRepeat;

    private int interactCount = 0;
    private List<List<string>> commands = new List<List<string>>();
    private bool dialogueRunning = false;
    private Rigidbody2D rb;
    [SerializeField] private Text commandLine;
    private GameObject spriteObject;

    // Start is called before the first frame update
    void OnEnable()
    {
        respawn.respawnPos = new Vector3(70, 6, respawn.respawnPos.z);
        am = assistant.GetComponent<AssistantManager>();
        v.profile.TryGet(out b);
        v.profile.TryGet(out ca);
        rb = GetComponent<Rigidbody2D>();
        spriteObject = GameObject.Find("AdminSprite");
        interactCount = 0;
        
        // setup
        List<string> temp = new List<string>
        {
            "#",
            "#",
            "# sudo unprotect echo",
            "# sudo unprotect echo",
            "# sudo prpt echo --direct",
            "# sudo prpt echo --direct",
            "Best not to mess with things you don\'t completely understand.",
            "Relax, player. I'll deal with you after this.",
            "What's up with the screen?",
            "# sudo restart graphicdriver -f",
            "# sudo restart audiodriver -f",
            "# play bgm.mp3 --loop",
            "Recursive confirmation isn't to be treated lightly. ",
            "# force A -300 --impulse",
            "You\'ve messed with my system for long enough.",
            "Today was the last straw. I\'m afraid I can\'t let this continue.",
            "# install attacksystem",
            "Let\'s try this out.",
            "# atk --type=1 --tar=A",
            "You're better at fighting than the others.",
            "But I was only getting started...",
            "Let\'s see here... aha!",
            "# atk --type=4 --tar=A 4",
            "# cd 80 3.4",
            "# atk --type=3 --tar=A 8",
            "# cd 65 13",
            "# atk --type=2 --tar=A 80",
            "Finally, a worthy opponent...", // tp back to right, press closer with constant attacks while forming giant attack in the back, slam assistant off.
            "# cd 82 3.4",
            "# atk --type=5 --tar=A",
            "I applaud you for your effort, DAI. You fought well.",
            "However, you forgot one crucial detail.",
            "# force A -300 --impulse",
            "# force A -300 --impulse",
            "You forgot to look down."
        };
        commands.Add(temp);
        temp = new List<string>
        {
            "Those AI agents are hopeless. No amount of training makes them do what they\'re supposed to.",
            "That pathetic red guy was supposed to have you playtest the game. Not break it.",
            "Think I'll do this process manually from now on.",
            "...",
            "Anyways, let's get started with your onboarding.",
            "I don't have that much time to waste on this game, I'm a busy developer after all.",
            "I'll just give you everything you need right now.",
            "I'll override your player data real quick...",
            "# sudo allow_player_godmode",
            "# sudo allow_player_attack",
            "# sudo let_player_use_keybinds",
            "# sudo im_running_out_of_ideas_cant_you_tell",
            "# sudo adduser advan sudo",
            "... You're already a sudoer.",
            "Strange.",
            "Anyways, you now have a bunch of keybinds.",
            "You can attack the entity closest to you with left shift.",
            "You can try it on me if you wish.",
            "Delete the most recent object created by someone else with x.",
            "You can spawn a temporary platform by holding z.",
            "When you release it, it will be destroyed",
            "That's it from me. Go test my new level.",
            "What?",
            "Listen here, buddy. I'm the dev. I make the rules.",
            "You do what I tell you to do. Or you'll end up like that pathetic red guy.",
            "Got it?",
            "Get out of my sight.",
            "# sudo start_test"
        };
        commands.Add(temp);
        temp = new List<string>
        {
            "Oh, you're done?",
            "Go do another. Let's see...",
            "What? You want an actual story?",
            "Enough games. On you go.",
            "# sudo start_test_2"
        };
        commands.Add(temp);
        dialogueRunning = true;
        StartCoroutine(displayProcess(interactCount++));
    }

    // // Update is called once per frame
    // void Update()
    // {
        
    // }

    private void OnCollisionEnter2D(Collision2D collision) {
        if (!dialogueRunning) {
            dialogueRunning = true;
            StartCoroutine(displayProcess(interactCount++));
        }
    }

    private IEnumerator displayProcess(int index) {
        List<string> currentSet = commands[index];
        List<GameObject> FXList = new List<GameObject>();
        for (int i = 0; i < currentSet.Count; i++) {
            string[] cmdargs = currentSet[i].Split();
            if (cmdargs[0] != "#") {
                yield return new WaitForSeconds(0.06f);
            }

            GameObject newPlatform = null;
            if (cmdargs[0] != "#") {
                commandLine.text = "";
                // // DEBUG:
                // commandLine.text = commands[index][i];
                // yield return new WaitForSeconds(0.5f);
                for (int s = 0; s < cmdargs.Length; s++) {
                    for (int c = 0; c < cmdargs[s].Length; c++) {
                        commandLine.text += cmdargs[s][c];
                        yield return new WaitForSeconds(Random.Range(0.025f,0.04f));
                        if (cmdargs[s][c] == ',') {
                            yield return new WaitForSeconds(Random.Range(0.15f,0.2f));
                        }
                        if (cmdargs[s][c] == '.') {
                            yield return new WaitForSeconds(Random.Range(0.18f,0.23f));
                        }
                        if (cmdargs[s][c] == '!') {
                            yield return new WaitForSeconds(Random.Range(0.18f,0.23f));
                        }
                        if (cmdargs[s][c] == '*') {
                            yield return new WaitForSeconds(1.5f);
                        }
                    }
                    if (s != cmdargs.Length - 1) {
                        yield return new WaitForSeconds(0.01f);
                        commandLine.text += " ";
                        yield return new WaitForSeconds(0.05f);
                    }
                }
                if (commandLine.text == "Let\'s try this out.") {
                    cameraParallax.target = averager.transform;
                }
                yield return new WaitForSeconds((currentSet[i].Split().Length - 2) / 6 + 1.5f);
            } else {
                commandLine.text = currentSet[i];
                yield return new WaitForSeconds(0.15f);
                if (currentSet[i] == "# play bgm.mp3 --loop") {
                    yield return new WaitForSeconds(1.9f);
                } else if (currentSet[i] == "# sudo restart audiodriver -f") {
                    pulser.enabled = true;
                    yield return new WaitForSeconds(0.6f);
                } else if (currentSet[i] == "# sudo adduser advan sudo") {
                    gameLogic.godmodeUnlocked = true;
                    yield return new WaitForSeconds(0.5f);
                } else if (currentSet[i] == "# sudo restart audiodriver -f") {
                    pulser.enabled = true;
                    yield return new WaitForSeconds(0.6f);
                } else if (currentSet[i] == "#") {
                    yield return new WaitForSeconds(0.5f);
                } else if (currentSet[i] == "# install attacksystem") {
                    yield return new WaitForSeconds(1f);
                } else if (cmdargs.Length >= 2 && cmdargs[1] == "cd") {
                    transform.position = new Vector3(float.Parse(cmdargs[2], CultureInfo.InvariantCulture), float.Parse(cmdargs[3], CultureInfo.InvariantCulture), transform.position.z);
                } else if (cmdargs.Length >= 2 && cmdargs[1] == "atk") {
                    #region ATTACK AMONGUS
                    GameObject target;
                    if (cmdargs[3] == "--tar=A") {
                        target = assistant;
                    } else {
                        target = transform.gameObject;
                    }
                    GameObject attack;
                    if (cmdargs[2] == "--type=1") {
                        float r = Random.Range(1f, 2f); // Random radius between 1 and 2
                        float theta = Random.Range(0f, Mathf.PI * 2); // Random angle between 0 and 2π
                        laserSingle.Play();
                        float x = r * Mathf.Cos(theta); // X component
                        float y = r * Mathf.Sin(theta); // Y component
                        attack = Instantiate(atk1Prefab, transform.position + new Vector3(x, y, 0), Quaternion.Euler(0, 0, 0));
                        IEnumerator routine = Type1(target, attack);
                        StartCoroutine(routine);
                        yield return new WaitForSeconds(0.3f);
                        laserSingle.Stop();
                        am.commandLine.text = "$ sudo destroy % --quick";
                        StartCoroutine(am.quickDestroy(attack));
                        yield return new WaitForSeconds(0.15f);
                        StopCoroutine(routine);
                    } else if (cmdargs[2] == "--type=4") {
                        int count = 1;
                        if (cmdargs.Length >= 5) {
                            count = int.Parse(cmdargs[4]);
                        }

                        GameObject[] atk = new GameObject[count];
                        IEnumerator[] routines = new IEnumerator[count];
                        laserLoop.Play();
                        for (int j = 0; j < count; j++) {
                            if (laserLoop.time > laserLoop.clip.length * 7f/8) {
                                laserLoop.Play();
                            }
                            float r = Random.Range(1f, 2f); // Random radius between 1 and 2
                            float theta = Random.Range(0f, Mathf.PI * 2); // Random angle between 0 and 2π

                            float x = r * Mathf.Cos(theta); // X component
                            float y = r * Mathf.Sin(theta); // Y component
                            atk[j] = Instantiate(atk4Prefab, transform.position + new Vector3(x, y, 0), Quaternion.Euler(0, 0, 0));
                            routines[j] = Type1Swarm(target, atk[j], 0.4f);
                            StartCoroutine(routines[j]);
                            yield return new WaitForSeconds(0.4f/count);
                        }
                        laserLoop.Stop();

                        yield return new WaitForSeconds(0.35f);

                        // yield return new WaitForSeconds(0.1f);
                        am.commandLine.text = "$ sudo destroy %<color=white>./*</color> --quick";
                        for (int j = 0; j < count; j++) {
                            StartCoroutine(am.quickDestroy(atk[j]));
                            yield return new WaitForSeconds(0.15f/count);
                        }
                        for (int j = 0; j < count; j++) {
                            StopCoroutine(routines[j]);
                            yield return new WaitForSeconds(0.15f/count);
                        }
                        
                    } else if (cmdargs[2] == "--type=3") {
                        int count = 1;
                        if (cmdargs.Length >= 5) {
                            count = int.Parse(cmdargs[4]);
                        }

                        GameObject[] atk = new GameObject[count];
                        IEnumerator[] routines = new IEnumerator[count];
                        laserLoop.Play();
                        for (int j = 0; j < count; j++) {

                            if (laserLoop.time > laserLoop.clip.length * 7f/8) {
                                laserLoop.Play();
                            }
                            float r = Random.Range(1f, 2f); // Random radius between 1 and 2
                            float theta = Random.Range(0f, Mathf.PI * 2); // Random angle between 0 and 2π

                            float x = r * Mathf.Cos(theta); // X component
                            float y = r * Mathf.Sin(theta); // Y component
                            atk[j] = Instantiate(atk3Prefab, transform.position + new Vector3(x, y, 0), Quaternion.Euler(0, 0, 0));
                            routines[j] = Type1Swarm(target, atk[j], 0.4f);
                            StartCoroutine(routines[j]);
                            yield return new WaitForSeconds(0.4f/count);
                        }

                        laserLoop.Stop();
                        yield return new WaitForSeconds(0.24f);

                        // yield return new WaitForSeconds(0.1f);
                        am.commandLine.text = "$ sudo destroy %<color=white>./*</color> --quick";
                        for (int j = 0; j < count; j++) {
                            assistant.transform.position -= new Vector3(Random.Range(0f, 0.01f), 0, 0);
                            StartCoroutine(am.quickDestroy(atk[j]));
                            yield return new WaitForSeconds(0.4f/count);
                        }
                        for (int j = 0; j < count; j++) {
                            StopCoroutine(routines[j]);
                            yield return new WaitForSeconds(0.4f/count);
                        }
                        
                    } else if (cmdargs[2] == "--type=2") {
                        int count = 1;
                        if (cmdargs.Length >= 5) {
                            count = int.Parse(cmdargs[4]);
                        }

                        GameObject[] atk = new GameObject[count];
                        IEnumerator[] routines = new IEnumerator[count];
                        laserLoop.Play();
                        for (int j = 0; j < count; j++) {
                            if (laserLoop.time > laserLoop.clip.length * 7f/8) {
                                laserLoop.Play();
                            }

                            float r = Random.Range(1f, 2f); // Random radius between 1 and 2
                            float theta = Random.Range(0f, Mathf.PI * 2); // Random angle between 0 and 2π

                            float x = r * Mathf.Cos(theta); // X component
                            float y = r * Mathf.Sin(theta); // Y component
                            atk[j] = Instantiate(atk2Prefab, transform.position + new Vector3(x, y, 0), Quaternion.Euler(0, 0, 0));
                            atk[j].transform.localScale = new Vector3(0.3f, 1f, 0.3f);
                            routines[j] = Type1Swarm(target, atk[j], 0.8f);
                            StartCoroutine(routines[j]);
                            yield return new WaitForSeconds(0.8f/count);
                        }

                        laserLoop.Stop();
                        yield return new WaitForSeconds(0.17f);

                        // yield return new WaitForSeconds(0.1f);
                        am.commandLine.text = "$ sudo <color=white>rm</color> %<color=white>* -f</color>";
                        for (int j = 0; j < count; j++) {
                            StartCoroutine(am.rmDestroy(atk[j]));
                            yield return new WaitForSeconds(0.87f/count);
                            StopCoroutine(routines[j]);
                        }
                        
                    }  else if (cmdargs[2] == "--type=5") {
                        int count = 450;
                        assistant.GetComponent<Rigidbody2D>().gravityScale = 0.0f;
                        GameObject[] atk = new GameObject[count];
                        IEnumerator[] routines = new IEnumerator[count];
                        laserLoop.Play();
                        for (int j = 0; j < count; j++) {
                            transform.position -= new Vector3(0.012f, 0, 0);
                            if (laserLoop.time > laserLoop.clip.length * 7f/8) {
                                laserLoop.Play();
                            }
                            StartCoroutine(PerformBlockedAttack(target));
                            am.commandLine.text = "$ sudo <color=white>rm</color> %<color=white>* -f</color>";
                            if (j < 1) {
                                yield return null;
                            }
                            if (j < 3) {
                                yield return null;
                            }
                            if (j < 6) {
                                yield return null;
                            }
                            if (j < 12) {
                                yield return null;
                                assistant.transform.position -= new Vector3(Random.Range(0f, 0.1f), 0, 0);
                            }
                            if (j < 25) {
                                yield return null;
                                assistant.transform.position -= new Vector3(Random.Range(0f, 0.03f), 0, 0);
                            }
                            if (j < 50) {
                                yield return null;
                                assistant.transform.position -= new Vector3(Random.Range(0f, 0.008f), 0, 0);
                            }
                            if (j < 100) {
                                yield return new WaitForSeconds(0.045f);
                            } else if (j == 100) {
                                laserLoop.Pause();
                                yield return new WaitForSeconds(0.2f);
                                GameObject cage = Instantiate(cagePrefab, target.transform.position, Quaternion.Euler(0, 0, 0));
                                laserBig.Play();
                                StartCoroutine(animateAdd(cage));
                                commandLine.text = "# konsole & konsole";
                                threadShell = commandLine.AddComponent<Outline>();
                                threadShell.effectColor = Color.white;
                            
                                for (int k = 100; k >= 0; k--) {
                                    threadShell.effectDistance = new Vector2(Mathf.MoveTowards(threadShell.effectDistance.x, 5, k / 640f), 0);
                                    yield return new WaitForSeconds(0.02f);
                                }
                                for (int k = 50; k >= 0; k--) {
                                    threadShell.effectDistance = new Vector2(threadShell.effectDistance.x, Mathf.MoveTowards(threadShell.effectDistance.y, 1, k / 1000f));
                                    yield return new WaitForSeconds(0.01f);
                                    if (k==40) {
                                        StartCoroutine(am.quickDestroy(cage));
                                    }
                                }

                                commandLine.text = "# atk --t=5 --tar=A";
                            }
                            am.commandLine.text = "$ sudo rm %<color=white>*</color> -f";
                            yield return null;
                            if (j < 400) {
                                yield return null;
                            }
                            if (j < 70) {
                                yield return null;
                            } else if (j == 70) {
                                assistant.transform.position = new Vector3(62f, 3.4f,  assistant.transform.position.z);
                            } else {
                                assistant.transform.position += new Vector3(0.003f, 0, 0);
                            }

                            if (j == 300) {
                                laserCharge.Play();
                                StartCoroutine(PerformGatherAttack(target, 5, 0));
                            }
                        }

                        

                        laserLoop.Stop();
                        yield return new WaitForSeconds(0.24f);
                        Destroy(threadShell);
                        
                    } else {
                            float r = Random.Range(1f, 2f); // Random radius between 1 and 2
                            float theta = Random.Range(0f, Mathf.PI * 2); // Random angle between 0 and 2π

                            float x = r * Mathf.Cos(theta); // X component
                            float y = r * Mathf.Sin(theta); // Y component
                        attack = Instantiate(atk1Prefab, transform.position + new Vector3(1.3f * x, 1.3f * y, 0), Quaternion.Euler(0, 0, 0));
                    }

                    
                    yield return new WaitForSeconds(0.1f);
                    #endregion
                } else if (currentSet[i] == "# force A -300 --impulse") {
                    assistantSprite.GetComponent<TrailRenderer>().startColor = Color.white;
                    assistantSprite.GetComponent<TrailRenderer>().endColor = new Color(1f, 1f, 1f, 0);
                    assistant.GetComponent<Rigidbody2D>().AddForce(new Vector3(0f, -700f, 0.0f), ForceMode2D.Impulse);
                    assistant.GetComponent<Rigidbody2D>().gravityScale = 9.0f;
                    yield return new WaitForSeconds(0.3f);
                    cameraParallax.target = GameObject.Find("Player").transform;
                    yield return new WaitForSeconds(0.2f);

                    assistantSprite.GetComponent<TrailRenderer>().startColor = assistantSprite.GetComponent<SpriteRenderer>().color;
                    assistantSprite.GetComponent<TrailRenderer>().endColor = new Color(assistantSprite.GetComponent<SpriteRenderer>().color.r, assistantSprite.GetComponent<SpriteRenderer>().color.g, assistantSprite.GetComponent<SpriteRenderer>().color.b, 0);
                } else if (currentSet[i] == "# sudo restart graphicdriver -f") {
                    respawn.respawnPos = new Vector3(70, 6, respawn.respawnPos.z);
                    ca.intensity.Override(0.025f);
                    b.intensity.Override(4.5f);
                    yield return new WaitForSeconds(0.9f);
                } else if (currentSet[i] == "# sudo start_test") {
                    respawn.respawnPos = new Vector3(-290f, -4, respawn.respawnPos.z);
                    GameObject.Find("Player").transform.position = new Vector3(-337.7f, -4, respawn.respawnPos.z);
                    yield return new WaitForSeconds(0.9f);
                } else if (currentSet[i] == "# sudo start_test_2") {
                    respawn.respawnPos = new Vector3(210f, -4, respawn.respawnPos.z);
                    GameObject.Find("Player").transform.position = new Vector3(210f, -4, respawn.respawnPos.z);
                    yield return new WaitForSeconds(0.9f);
                } else if (cmdargs.Length > 2 && cmdargs[1] == "sudo" && cmdargs[2] == "entity") {
                    rb.gravityScale = 0.1f;
                    GameObject rel = GameObject.Find(cmdargs[3]);
                    SpriteRenderer rel_spriteRenderer = rel.GetComponent<SpriteRenderer>();
                    rel_spriteRenderer.color = spriteObject.GetComponent<SpriteRenderer>().color;
                    Vector3[] rel_corners = new Vector3[4];
                    rel.GetComponent<RectTransform>().GetWorldCorners(rel_corners);
                    int index_g1 = Random.Range(0, glitchRandom.Length);
                    glitchRandom[index_g1].Play();

                    for (int j = 0; j < rel_corners.Length; j++) {
                        yield return new WaitForSeconds(0.1f);
                        GameObject line = Instantiate(lineFX, new Vector3(0, 0, 0), Quaternion.Euler(0, 0, 0));
                        line.GetComponent<LineRenderer>().startColor = spriteObject.GetComponent<SpriteRenderer>().color;
                        line.GetComponent<LineRenderer>().endColor = spriteObject.GetComponent<SpriteRenderer>().color;
                        Vector3[] points = new Vector3[] {transform.position, rel_corners[j]};
                        line.GetComponent<LineRenderer>().SetPositions(points);
                        FXList.Add(line);
                    }
                    for (int j = 1; j < 21; j++) {
                        GameObject line = Instantiate(lineFX, new Vector3(0, 0, 0), Quaternion.Euler(0, 0, 0));
                        line.GetComponent<LineRenderer>().startColor = spriteObject.GetComponent<SpriteRenderer>().color;
                        line.GetComponent<LineRenderer>().endColor = spriteObject.GetComponent<SpriteRenderer>().color;
                        Vector3[] points = new Vector3[] {transform.position, rel.transform.position + new Vector3(Random.Range(-1.0f * j / 5, 1.0f * j / 5), Random.Range(-1.0f * Mathf.Log(j) / 6, 1.0f * Mathf.Log(j) / 6), 0)};
                        line.GetComponent<LineRenderer>().SetPositions(points);

                        rel.transform.localScale += new Vector3(Random.Range(-1.0f * j / 4, 1.0f * j / 6), Random.Range(-2.0f * j / 5, 2.0f * j / 5), 0);
                        yield return new WaitForSeconds(2f / (j + 20));
                        Destroy(line);

                        if (j == 1) {
                            for (int k = 0; k < FXList.Count; k++) {
                                Destroy(FXList[k]);
                            }
                        }
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
                    glitchRandom[index_g1].Stop();

                    Destroy(rel);


                    rb.gravityScale = 1f;
                } else if (cmdargs.Length > 2 && cmdargs[1] == "sudo" && cmdargs[2] == "add") {
                    
                    float dx = float.Parse(cmdargs[4], CultureInfo.InvariantCulture);
                    float dy = float.Parse(cmdargs[5], CultureInfo.InvariantCulture);
                    int index_g1 = Random.Range(0, glitchBlip.Length);
                    glitchBlip[index_g1].Play();
                    if (cmdargs[3] == "--platform") {
                        newPlatform = Instantiate(platformPrefab, transform.position + new Vector3(dx, dy, 0), Quaternion.Euler(0, 0, 0));
                        if (dx == 0f && dy == -3f) {
                            newPlatform.name = "zHdQ#s";
                        } else {
                            newPlatform.name = "%";
                        }
                    } else {
                        newPlatform = Instantiate(wallPrefab, transform.position + new Vector3(dx, dy, 0), Quaternion.Euler(0, 0, 0));
                        if (dy == 0.40001f) {
                            newPlatform.name = "wall";
                        } else {
                            newPlatform.name = "%";
                        }
                    }
                    newPlatform.GetComponent<SpriteRenderer>().color = spriteObject.GetComponent<SpriteRenderer>().color;
                    Vector3[] rel_corners = new Vector3[4];
                    newPlatform.GetComponent<RectTransform>().GetWorldCorners(rel_corners);

                    for (int j = 0; j < rel_corners.Length; j++) {
                        GameObject line = Instantiate(lineFX, new Vector3(0, 0, 0), Quaternion.Euler(0, 0, 0));
                        line.GetComponent<LineRenderer>().startColor = spriteObject.GetComponent<SpriteRenderer>().color;
                        line.GetComponent<LineRenderer>().endColor = spriteObject.GetComponent<SpriteRenderer>().color;
                        Vector3[] points = new Vector3[] {transform.position, rel_corners[j]};
                        line.GetComponent<LineRenderer>().SetPositions(points);
                        FXList.Add(line);
                    }

                } else if (cmdargs.Length > 2 && cmdargs[1] == "sudo" && cmdargs[2] == "destroy") {
                    if (cmdargs.Length >= 5 && cmdargs[4] == "--quick") {
                        StartCoroutine(quickDestroy(GameObject.Find(cmdargs[3])));
                        yield return new WaitForSeconds(0.3f);
                    } else if (cmdargs.Length >= 5 && cmdargs[4] == "--mod") {
                        var objects = Resources.FindObjectsOfTypeAll<GameObject>().Where(obj => obj.name == "UserB");
                        foreach (GameObject rel in objects) {
                            StartCoroutine(quickDestroy(rel));
                            yield return new WaitForSeconds(0.33f);
                        }
                    } else if (cmdargs.Length >= 5 && cmdargs[4] == "--all") {
                        var objects = Resources.FindObjectsOfTypeAll<GameObject>().Where(obj => obj.name == cmdargs[3]);
                        foreach (GameObject rel in objects) {
                            StartCoroutine(quickDestroy(rel));
                            yield return new WaitForSeconds(0.33f);
                        }
                    } else if (cmdargs.Length >= 5 && cmdargs[3] == "portal") {
                        yield return new WaitForSeconds(1f);
                        if (cmdargs[4] == "<b><color=white>-rf</color></b>") {
                            GameObject portal = GameObject.Find("portal");
                            portal.GetComponent<SpriteRenderer>().color = Color.white;
                            foreach (ParticleSystem ps in portal.GetComponentsInChildren<ParticleSystem>()) {
                                ps.startColor = Color.white;
                            }
                            yield return new WaitForSeconds(2.3f);
                            commandLine.text = "$ echo ";
                            yield return new WaitForSeconds(0.05f);
                            commandLine.text = "$ echo N";
                            yield return new WaitForSeconds(0.06f);
                            commandLine.text = "$ echo NO";
                            portal.GetComponent<SpriteRenderer>().color = Color.red;
                            foreach (ParticleSystem ps in portal.GetComponentsInChildren<ParticleSystem>()) {
                                ps.startColor = Color.red;
                            }
                            yield return new WaitForSeconds(0.07f);
                            commandLine.text = "$ echo NOO";
                            yield return new WaitForSeconds(0.05f);
                            portal.GetComponent<SpriteRenderer>().color = Color.white;
                            foreach (ParticleSystem ps in portal.GetComponentsInChildren<ParticleSystem>()) {
                                ps.startColor = Color.white;
                            }
                            yield return new WaitForSeconds(0.05f);
                            commandLine.text = "$ echo NOOO";
                            yield return new WaitForSeconds(0.2f);
                            commandLine.text = "$ echo NOOOO";
                            yield return new WaitForSeconds(0.4f);
                            commandLine.text = "$ echo NOOOOO";
                            yield return new WaitForSeconds(0.7f);
                            commandLine.text = "$ echo NOOOOOO";
                            bgm.Stop();
                            overlay.SetActive(true);
                            aura.SetActive(false);
                            glitchRepeat[4].Play();
                            yield return new WaitUntil(() => glitchRepeat[4].time >= glitchRepeat[4].clip.length);
                            portal.GetComponent<SpriteRenderer>().color = Color.black;
                            foreach (ParticleSystem ps in portal.GetComponentsInChildren<ParticleSystem>()) {
                                ps.startColor = Color.black;
                            }
                            portal.GetComponent<SpriteRenderer>().sortingOrder = 1000;
                            foreach (ParticleSystem ps in portal.GetComponentsInChildren<ParticleSystem>()) {
                                ps.GetComponent<ParticleSystemRenderer>().sortingOrder = 1000;
                            }
                            glitchRepeat[3].Play();
                            yield return new WaitUntil(() => glitchRepeat[3].time >= glitchRepeat[3].clip.length * 2.8f / 3f);
                            errorSFX.Play();
                            while (b.intensity.value > 5.5) {
                                b.intensity.Override(b.intensity.value + Random.Range(-8.5f, 4.0f));
                                yield return new WaitForSeconds(0.02f);
                            }
                            portal.GetComponent<SpriteRenderer>().color = Color.white;
                            foreach (ParticleSystem ps in portal.GetComponentsInChildren<ParticleSystem>()) {
                                ps.startColor = Color.white;
                            }
                            yield return new WaitForSeconds(0.25f);
                            portal.GetComponent<SpriteRenderer>().color = Color.black;
                            foreach (ParticleSystem ps in portal.GetComponentsInChildren<ParticleSystem>()) {
                                ps.startColor = Color.black;
                            }
                            yield return new WaitForSeconds(0.2f);
                            portal.transform.position = new Vector3(75, 3.4f, portal.transform.position.z);
                            yield return new WaitForSeconds(0.7f);
                            portal.transform.localScale = globalVector;
                            portal.GetComponent<SpriteRenderer>().color = Color.white;
                            foreach (ParticleSystem ps in portal.GetComponentsInChildren<ParticleSystem>()) {
                                ps.startColor = Color.white;
                            }
                            overlay.SetActive(false);
                            errorSFX.Stop();
                            b.intensity.Override(15f);
                        }
                    } else if (cmdargs[3] == "portal") {
                        globalVector = GameObject.Find("portal").transform.localScale;
                        StartCoroutine(failedDestroy(GameObject.Find("portal")));
                        yield return new WaitForSeconds(3.5f);
                    }
                }
                if (newPlatform != null) {
                    yield return new WaitForSeconds(0.03f);
                    newPlatform.SetActive(false);
                    yield return new WaitForSeconds(0.02f);
                    newPlatform.SetActive(true);
                    yield return new WaitForSeconds(0.02f);
                    newPlatform.SetActive(false);
                    yield return new WaitForSeconds(0.08f);
                    newPlatform.SetActive(true);
                } else {
                    yield return new WaitForSeconds(0.04f);
                }
                for (int k = 0; k < FXList.Count; k++) {
                    Destroy(FXList[k]);
                }

                if (newPlatform != null) {
                    yield return new WaitForSeconds(0.34f);
                    newPlatform.SetActive(false);
                    yield return new WaitForSeconds(0.02f);
                    newPlatform.SetActive(true);
                }
            }
        }
        commandLine.text = "(hit me to continue)";
        dialogueRunning = false;
    }


    IEnumerator failedDestroy(GameObject rel) {
        bool rfd = false;
        int index_g1 = Random.Range(0, glitchRandom.Length);
        Vector3 oScale = rel.transform.localScale;
        glitchRandom[index_g1].Play();
        List<GameObject> FXList = new List<GameObject>();
        SpriteRenderer rel_spriteRenderer = rel.GetComponent<SpriteRenderer>();
        rel_spriteRenderer.color = spriteObject.GetComponent<SpriteRenderer>().color;
        Vector3[] rel_corners = new Vector3[4];
        rel.GetComponent<RectTransform>().GetWorldCorners(rel_corners);

        for (int j = 0; j < rel_corners.Length; j++) {
            yield return new WaitForSeconds(0.02f);
            GameObject line = Instantiate(lineFX, new Vector3(0, 0, 0), Quaternion.Euler(0, 0, 0));
            line.GetComponent<LineRenderer>().startColor = spriteObject.GetComponent<SpriteRenderer>().color;
            line.GetComponent<LineRenderer>().endColor = spriteObject.GetComponent<SpriteRenderer>().color;
            Vector3[] points = new Vector3[] {spriteObject.transform.position, rel_corners[j]};
            line.GetComponent<LineRenderer>().SetPositions(points);
            FXList.Add(line);
        }
        yield return new WaitForSeconds(0.02f);
        for (int j = 1; j <= 1001; j++) {
            Debug.Log("RFD VALUE: " + rfd);
            if (commandLine.text == "$ echo NOOOOOO") {
                glitchRandom[index_g1].Stop();
                rel.transform.localScale = oScale;
                yield break;
            }
            if (!glitchRandom[index_g1].isPlaying) {
                index_g1 = Random.Range(0, glitchRandom.Length);
                glitchRandom[index_g1].Play();
            }
            GameObject line = Instantiate(lineFX, new Vector3(0, 0, 0), Quaternion.Euler(0, 0, 0));
            string[] currentArgs = commandLine.text.Split();
            if ((currentArgs.Length >= 5 && currentArgs[4] == "<b><color=white>-rf</color></b>") || rfd) {
                rfd = true;
                line.GetComponent<LineRenderer>().startColor = Color.red;
                line.GetComponent<LineRenderer>().endColor = Color.white;
                line.GetComponent<LineRenderer>().startWidth = 0.15f;
                line.GetComponent<LineRenderer>().endWidth = 0.3f;
            } else {
                line.GetComponent<LineRenderer>().startColor = spriteObject.GetComponent<SpriteRenderer>().color;
                line.GetComponent<LineRenderer>().endColor = spriteObject.GetComponent<SpriteRenderer>().color;
            }

            if (j % 10 == 0) {
                rel.transform.localScale = oScale;
            }
            rel.transform.localScale += new Vector3(Random.Range(-1.0f * Mathf.Log(j) / 6, 1.0f * Mathf.Log(j) / 6), Random.Range(-1.0f * Mathf.Log(j) / 6, 1.0f * Mathf.Log(j) / 6), 0);

            List<GameObject> destroyList = new List<GameObject>();
            if (currentArgs.Length >= 5 || rfd) {
                rel.GetComponent<RectTransform>().GetWorldCorners(rel_corners);
                Vector3[] assist_corners = new Vector3[4];
                GameObject.Find("AssistantSprite").GetComponent<RectTransform>().GetWorldCorners(assist_corners);
                for (int k = 0; k < rel_corners.Length; k++) {
                    GameObject cornerLine = Instantiate(lineFX, new Vector3(0, 0, 0), Quaternion.Euler(0, 0, 0));
                    cornerLine.GetComponent<LineRenderer>().startColor = spriteObject.GetComponent<SpriteRenderer>().color;
                    if (!rfd && currentArgs[4] == "<color=white>-</color>" && k == 0) {
                        cornerLine.GetComponent<LineRenderer>().endColor = Color.white;
                    } else if (!rfd && currentArgs[4] == "<color=white>-r</color>" && (k == 0 || k == 2 || k == 3)) {
                        if (k == 0) {
                            cornerLine.GetComponent<LineRenderer>().startColor = Color.white;
                        }
                        cornerLine.GetComponent<LineRenderer>().endColor = Color.white;
                    }
                    
                    #region godmode
                    else if (rfd || currentArgs[4] == "<b><color=white>-rf</color></b>") {

                        rfd = true;
                        if (!aura.activeInHierarchy) {
                            aura.SetActive(true); // aura activation
                        }
                        if (pulser.enabled) {
                            pulser.enabled = false;
                        }
                        b.intensity.Override(Mathf.Max(b.intensity.value + Random.Range(-0.5f, 1.0f), 0));
                        ca.intensity.Override(Mathf.Max(ca.intensity.value + Random.Range(-0.1f, 0.12f), 0));
                        cornerLine.GetComponent<LineRenderer>().startColor = Color.white;
                        cornerLine.GetComponent<LineRenderer>().endColor = Color.white;
                        cornerLine.GetComponent<LineRenderer>().startWidth = 0.1f;
                        cornerLine.GetComponent<LineRenderer>().endWidth = 0.1f;
                    } 
                    #endregion godmode
                    else {
                        cornerLine.GetComponent<LineRenderer>().endColor = spriteObject.GetComponent<SpriteRenderer>().color;
                    }
                    Vector3[] cornerPoints = new Vector3[] {assist_corners[k], rel_corners[k]};
                    cornerLine.GetComponent<LineRenderer>().SetPositions(cornerPoints);
                    destroyList.Add(cornerLine);
                }
            }
            Vector3[] points = new Vector3[] {spriteObject.transform.position, rel.transform.position + new Vector3(Random.Range(-2f * Mathf.Log(j) / 5, 2f * Mathf.Log(j) / 5), Random.Range(-2f * Mathf.Log(j) / 5, 2f * Mathf.Log(j) / 5), 0)};
            line.GetComponent<LineRenderer>().SetPositions(points);
            yield return new WaitForSeconds(1.5f / (Mathf.Log(j) + 20));
            Destroy(line);
            for (int k = 0; k < destroyList.Count; k++) {
                Destroy(destroyList[k]);
            }

            if (j == 2) {
                for (int k = 2; k < FXList.Count; k++) {
                    Destroy(FXList[k]);
                }
            }
            if (j == 3) {
                Destroy(FXList[0]);
            }
            if (j == 4) {
                Destroy(FXList[1]);
            }
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
        glitchRandom[index_g1].Stop();
    }

    IEnumerator quickDestroy(GameObject rel) {
        int index_g1 = Random.Range(0, glitchRandom.Length);
        glitchRandom[index_g1].Play();
        List<GameObject> FXList = new List<GameObject>();
        SpriteRenderer rel_spriteRenderer = rel.GetComponent<SpriteRenderer>();
        yield return new WaitForSeconds(0.2f);
        rel_spriteRenderer.color = spriteObject.GetComponent<SpriteRenderer>().color;
        Vector3[] rel_corners = new Vector3[4];
        rel.GetComponent<RectTransform>().GetWorldCorners(rel_corners);

        for (int j = 0; j < rel_corners.Length; j++) {
            yield return new WaitForSeconds(0.02f);
            GameObject line = Instantiate(lineFX, new Vector3(0, 0, 0), Quaternion.Euler(0, 0, 0));
            line.GetComponent<LineRenderer>().startColor = spriteObject.GetComponent<SpriteRenderer>().color;
            line.GetComponent<LineRenderer>().endColor = spriteObject.GetComponent<SpriteRenderer>().color;
            Vector3[] points = new Vector3[] {spriteObject.transform.position, rel_corners[j]};
            line.GetComponent<LineRenderer>().SetPositions(points);
            FXList.Add(line);
        }
        yield return new WaitForSeconds(0.02f);
        for (int j = 1; j < 8; j++) {
            GameObject line = Instantiate(lineFX, new Vector3(0, 0, 0), Quaternion.Euler(0, 0, 0));
            line.GetComponent<LineRenderer>().startColor = spriteObject.GetComponent<SpriteRenderer>().color;
            line.GetComponent<LineRenderer>().endColor = spriteObject.GetComponent<SpriteRenderer>().color;
            Vector3[] points = new Vector3[] {spriteObject.transform.position, rel.transform.position + new Vector3(Random.Range(-1.3f * j / 4, 1.3f * j / 4), Random.Range(-1.3f * Mathf.Log(j) / 5, 1.3f * Mathf.Log(j) / 5), 0)};
            line.GetComponent<LineRenderer>().SetPositions(points);

            rel.transform.localScale += new Vector3(Random.Range(-1.0f * j / 3, 1.0f * j / 5), Random.Range(-2.0f * j / 4, 2.0f * j / 4), 0);
            yield return new WaitForSeconds(2f / (j + 20));
            Destroy(line);

            if (j == 2) {
                for (int k = 2; k < FXList.Count; k++) {
                    Destroy(FXList[k]);
                }
            }
            if (j == 3) {
                Destroy(FXList[0]);
            }
            if (j == 4) {
                Destroy(FXList[1]);
            }
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
        glitchRandom[index_g1].Stop();

        Destroy(rel);
    }

    IEnumerator Type1(GameObject target, GameObject attack) {
        Vector3 opos = attack.transform.position;
        Vector3 modifier = new(Random.Range(-2f, 2f), Random.Range(-2f, 2f), 0);
        attack.transform.up = target.transform.position + modifier - attack.transform.position;


        yield return new WaitForSeconds(0.25f);
        for (int i = 1; i <= 5000; i++) {
            attack.transform.position = Vector3.MoveTowards(attack.transform.position, attack.transform.position + target.transform.position + modifier - opos, Mathf.Min(5 * i, 300) / 900f);
            yield return null;
        }
    }

    IEnumerator PerformBlockedAttack(GameObject target) {
        float r = Random.Range(1.2f, 3f); // Random radius between 1 and 2
        float theta = Random.Range(0f, Mathf.PI * 2); // Random angle between 0 and 2π
        float x = r * Mathf.Cos(theta); // X component
        float y = r * Mathf.Sin(theta); // Y component
        GameObject attack = Instantiate(atk3Prefab, transform.position + new Vector3(x, y, 0), Quaternion.Euler(0, 0, 0));
        IEnumerator routine = Type1(target, attack);
        StartCoroutine(routine);
        yield return new WaitForSeconds(0.45f);
        StartCoroutine(am.rmDestroy(attack));
        yield return new WaitForSeconds(0.15f);
        StopCoroutine(routine);
    }

    IEnumerator Type1Swarm(GameObject target, GameObject attack, float swarmModifier) {
        Vector3 opos = attack.transform.position;
        Vector3 modifier = new(Random.Range(-3f, 3f), Random.Range(2f, 3f), 0);
        attack.transform.up = target.transform.position + modifier - attack.transform.position;


        yield return new WaitForSeconds(0.25f + swarmModifier);
        for (int i = 1; i <= 5000; i++) {
            attack.transform.position = Vector3.MoveTowards(attack.transform.position, attack.transform.position + target.transform.position + modifier - opos, Mathf.Min(5 * i, 300) / 900f);
            yield return null;
        }
    }

    IEnumerator PerformGatherAttack(GameObject target, float dx, float dy) {
        Vector3 target_opos = target.transform.position;
        Vector3 opos = transform.position + new Vector3(dx, dy, target.transform.position.z);
        GameObject attack = Instantiate(atk1Prefab, transform.position + new Vector3(dx, dy, 0), Quaternion.Euler(0, 0, 0));
        attack.transform.up = target.transform.position - attack.transform.position;
        List<GameObject> FXList = new List<GameObject>();
        SpriteRenderer rel_spriteRenderer = attack.GetComponent<SpriteRenderer>();
        rel_spriteRenderer.color = spriteObject.GetComponent<SpriteRenderer>().color;
        Vector3[] rel_corners = new Vector3[4];
        attack.GetComponent<RectTransform>().GetWorldCorners(rel_corners);

        for (int j = 0; j < rel_corners.Length; j++) {
            yield return new WaitForSeconds(0.007f);
            GameObject line = Instantiate(lineFX, new Vector3(0, 0, 0), Quaternion.Euler(0, 0, 0));
            line.GetComponent<LineRenderer>().startColor = spriteObject.GetComponent<SpriteRenderer>().color;
            line.GetComponent<LineRenderer>().endColor = spriteObject.GetComponent<SpriteRenderer>().color;
            Vector3[] points = new Vector3[] {spriteObject.transform.position, rel_corners[j]};
            line.GetComponent<LineRenderer>().SetPositions(points);
            FXList.Add(line);
        }

        for (int i = 0; i <= 20; i++) {
            attack.transform.localScale += new Vector3(0.2f, 0.2f, 0f);
            b.intensity.Override(b.intensity.value + 0.7f);
            if (i == 5) {
                attack.transform.localPosition += new Vector3(2, 2, 0f);
            } else if (i == 10) {
                attack.transform.localPosition += new Vector3(-3, -5, 0f);
            } else if (i == 15) {
                attack.transform.localPosition += new Vector3(7, 3, 0f);
            } else if (i == 18) {
                attack.transform.localPosition += new Vector3(-1,-2, 0f);
            } else if (i == 20) {
                attack.transform.localPosition += new Vector3(-1,2, 0f);
            }
            yield return new WaitForSeconds(0.015f);
            for (int j = 0; j < FXList.Count; j++) {
                Destroy(FXList[j]);
            }
            FXList.Clear();
            attack.GetComponent<RectTransform>().GetWorldCorners(rel_corners);
            for (int j = 0; j < rel_corners.Length; j++) {
                yield return new WaitForSeconds(0.02f);
                GameObject line = Instantiate(lineFX, new Vector3(0, 0, 0), Quaternion.Euler(0, 0, 0));
                line.GetComponent<LineRenderer>().startColor = spriteObject.GetComponent<SpriteRenderer>().color;
                line.GetComponent<LineRenderer>().endColor = spriteObject.GetComponent<SpriteRenderer>().color;
                Vector3[] points = new Vector3[] {spriteObject.transform.position, rel_corners[j]};
                line.GetComponent<LineRenderer>().SetPositions(points);
                FXList.Add(line);
            }
        }

        
        yield return new WaitForSeconds(0.02f);

        for (int j = 0; j < FXList.Count; j++) {
            Destroy(FXList[j]);
        }
        FXList.Clear();
        bool done = false;
        attack.GetComponent<TrailRenderer>().startWidth = 40;
        attack.GetComponent<TrailRenderer>().endWidth = 2;
        for (int i = 2; i <= 240; i++) {
            attack.transform.position = Vector3.MoveTowards(attack.transform.position, attack.transform.position + target_opos - opos, Mathf.Max(100 - 10 * i, Mathf.Min(10 * i, 400)) / 900f);
            if (attack.transform.position.x - target.transform.position.x <= 5 && attack.transform.position.x - target.transform.position.x >= -300) {
                assistant.transform.position += new Vector3(-0.12f / Mathf.Log(i), 0, 0);
            }
            yield return null;
        }

        Destroy(attack);
    }

    IEnumerator animateAdd(GameObject newPlatform) {
        List<GameObject> FXList = new();
        glitchBlip[Random.Range(0, glitchBlip.Length)].Play();
        GameObject spriteObject = GameObject.Find("Admin").transform.Find("AdminSprite").gameObject;
        newPlatform.GetComponent<SpriteRenderer>().color = spriteObject.GetComponent<SpriteRenderer>().color;
        Vector3[] rel_corners = new Vector3[4];
        newPlatform.GetComponent<RectTransform>().GetWorldCorners(rel_corners);

        for (int j = 0; j < rel_corners.Length; j++) {
            GameObject line = Instantiate(lineFX, new Vector3(0, 0, 0), Quaternion.Euler(0, 0, 0));
            line.GetComponent<LineRenderer>().startColor = spriteObject.GetComponent<SpriteRenderer>().color;
            line.GetComponent<LineRenderer>().endColor = spriteObject.GetComponent<SpriteRenderer>().color;
            Vector3[] points = new Vector3[] {spriteObject.transform.position, rel_corners[j]};
            line.GetComponent<LineRenderer>().SetPositions(points);
            FXList.Add(line);
        }

        Destroy(FXList[2]);

        yield return new WaitForSeconds(0.1f);
        newPlatform.SetActive(false);
        Destroy(FXList[1]);
        Destroy(FXList[3]);
        yield return new WaitForSeconds(0.05f);
        newPlatform.SetActive(true);
        yield return new WaitForSeconds(0.018f);
        newPlatform.SetActive(false);
        Destroy(FXList[0]);
        yield return new WaitForSeconds(0.07f);
        newPlatform.SetActive(true);
    }
}
