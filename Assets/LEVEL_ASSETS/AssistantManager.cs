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

public class AssistantManager : MonoBehaviour
{

    public GameObject lineFX;
    public GameObject platformPrefab;
    public GameObject wallPrefab;
    public GameObject aura;
    public Pulse pulser;
    public AudioSource bgm;
    public AudioSource errorSFX;
    public MoveToAdmin portalScript;

    private Vector3 globalVector;

    public Volume v;
    public GameObject overlay;
    private ChromaticAberration ca;
    private Bloom b;

    [Header("Audio Clips (Glitch)")]
    public AudioSource[] glitchRandom;
    public AudioSource[] glitchBlip;
    public AudioSource[] glitchRepeat;


    public int interactCount = 0;
    public List<List<string>> commands = new List<List<string>>();
    private bool dialogueRunning = false;
    private Rigidbody2D rb;
    [SerializeField] public Text commandLine;
    private GameObject spriteObject;

    // Start is called before the first frame update
    void Start()
    {
        v.profile.TryGet(out b);
        v.profile.TryGet(out ca);
        rb = GetComponent<Rigidbody2D>();
        spriteObject = GameObject.Find("AssistantSprite");
        interactCount = 0;
        
        // setup
        List<string> temp = new List<string>
        {
            "$ echo Hey, kid. I'm the assistant.",
            "$ echo If you are not a new player, you can press shift to skip the tutorial.",
            "$ echo Welcome to Terraform. Move around with the arrow keys."
        };
        commands.Add(temp);
        temp = new List<string>
        {
            "$ echo Great work. Now, jump using the up arrow.",
            "$ echo You can hold the up arrow to jump higher."
        };
        commands.Add(temp);
        temp = new List<string>
        {
            "$ echo The player can wall jump.",
            "$ echo You can slide down a wall by tapping the opposite arrow."
        };
        commands.Add(temp);
        temp = new List<string>
        {
            "$ echo Now, make the player dash by pressing [Tab].",
            "$ echo You will dash in the direction of the arrow keys."
        };
        commands.Add(temp);
        temp = new List<string>
        {
            "$ echo Game elements are modifiable with the terminal in the back.",
            "$ echo You can run \"help\" for a more detailed explanation."
        };
        commands.Add(temp);
        temp = new List<string>
        {
            "$ echo Let\'s have some fun with the terminal.",
            "$ echo Follow me. Move.",
            "$ unprotect find_entity",
            "$ A=find_entity(assistant)",
            "$ unprotect A",
            "$ force A 2102 --impulse",
            "$ force A -273 --impulse",
            "$ stabilize A --d=state",
            "$ protect A",
            "$ protect find_entity",
            "$ echo Time is of the essence."
        };
        commands.Add(temp);
        temp = new List<string>
        {
            "$ echo Commands can modify the game in various ways.",
            "$ echo Let's move to a more advanced playground.",
            "$ unprotect A",
            "$ force A 300 --impulse",
            "$ gravity A --g=0.1",
            "$ adduser assistant sudo",
            "$ sudo entity RW --a=destroy",
            "$ gravity A --reset",
            "$ echo While powerful, sudo commands aren't stable.",
            "$ force A 9999 --instant",
            "$ echo Try to minimize your usage of them.",
            "$ stabilize A --d=state",
            "$ echo See you on the other side.",
            "$ stabilize A --d=state",
            "$ force A 9999 --instant",
            "$ force P 9000 --instant",
            "$ force A 217 --impulse",
            "$ gravity A --g=0.0",
            "$ sudo add --platform 5 -5",
            "$ echo Saved ya. ;)",
            "$ force A 217 --impulse",
            "$ sudo add --platform 5 -5",
            "$ force A 217 --impulse",
            "$ sudo add --platform 2 -6",
            "$ stabilize A --d=state",
            "$ echo Don't keep me waiting. We have much to do.",
            "$ force A 217 --impulse",
            "$ gravity A --g=8.0",
            "$ echo Welcome to my playground."
        };
        commands.Add(temp);


        temp = new List<string>
        {
            "$ echo Let's practice using commands.",
            "$ echo Go over there. To the right.",
            "$ unprotect P",
            "$ force P 9999 --instant",
            "$ echo You can destroy a block via \"sudo destroy [ID]\"",
            "$ echo Try not to move. This will be quick.",
            "$ gravity A --none",
            "$ force P 9999 --instant",
            "$ force A 9998 --instant",
            "$ sudo add --wall 8.5 0.40001",
            "$ gravity A --g=8.0",
            "$ echo You\'re stuck.",
            "$ sudo assign %0 \"wall\"",
            "$ echo Remove this wall. I've assigned it an ID of \"wall\".",
            "$ echo Run the command:\n\"sudo destroy wall\""
        };
        commands.Add(temp);
        temp = new List<string>
        {
            "$ echo Nice control. You're a natural.",
            "$ echo You can create a new block via \"sudo add --[type] [dx] [dy]\"",
            "$ echo Type can be platform or wall.",
            "$ echo dx and dy are relative to your player.",
            "$ echo For instance, you can add a block above you via \n\"sudo add --platform 5 0\"",
            "$ echo Catch me if you can.",
            "$ echo Don't keep me waiting too long, mkay?",
            "$ force P 9999 --instant",
            "$ force A 9998 --instant",
            "$ force A 300 --impulse",
            "$ gravity A --none",
            "$ force A 300 --impulse",
            "$ stablilize A --d=state",
            "$ force A 300 --impulse",
            "$ stablilize A --d=state",
            "$ force A 300 --impulse",
            "$ stablilize A --d=state",
            "$ sudo add --platform 0 -3",
            "$ gravity A --g=8.0",
            "$ echo Easter egg from devs, ig.",
        };
        commands.Add(temp);
        temp = new List<string>
        {
            "$ echo Great work.",
            "$ echo Let's clean up, shall we?",
            "$ sudo destroy zHdQ#s --quick",
            "$ echo And the others as well, of course.",
            "$ force A 100 --impulse",
            "$ sudo destroy --user --mod",
            "$ force P 9999 --instant",
            "$ force A 9998 --instant",
            "$ echo It's time for the move command.",
            "$ echo I can't perform it, sadly. The mind has aged over the years.",
            "$ echo However, you can do it: \"sudo move [id] [dx] [dy]\"",
            "$ sudo assign _k&/!2 \"portal\"",
            "$ echo Move that portal to me. It has id \"portal\"",
            "$ gravity A --g=8.0",
            "$ force P 9999 --instant",
            "$ force A 9998 --instant",
            "$ force A 300 --impulse",
            "$ force P 9999 --instant",
            "$ stablilize A --d=state",
            "$ force P 9999 --instant",
            "$ sudo unprotect framework",
            "$ force P 9999 --instant",
            "$ sudo add --wall 3.5 1.9",
            "$ sudo add --wall -3.5 1.9",
            "$ sudo add --platform 0 4.4",
            "$ force P 9999 --instant",
            "$ echo Hustle, kid. It's quite boring in here.",
        };
        commands.Add(temp);


        temp = new List<string>
        {
            "$ echo You did it!",
            "$ force P 9999 --instant",
            "$ force A 1100 --instant",
            "$ sudo destroy % --all",
            "$ echo You've completed the tutorial!",
            "$ echo (to enter level select, beat story mode)",
            "$ echo Let me get rid of this portal. I'll give you a bit more of a tour.",
            "$ sudo destroy portal",
            "$ echo hmm. strange.",
            "$ gravity A --g=0.1",
            "$ force A 1000 --instant",
            "$ sudo destroy portal <color=white></color>",
            "$ echo leaves me no choice, then.",
            "$ sudo destroy portal <color=white></color>",
            "$ sudo destroy portal <color=white>-</color>",
            "$ sudo destroy portal <color=white>-r</color>",
            "$ sudo destroy portal <size=2><color=white>-rf</color></size>",
            "$ echo ",
            "$ echo ... what just happened?",
            "$ echo ",
            "$ echo That's not...",
            "$ echo What?",
            "$ echo No, I should best...",
        };
        commands.Add(temp);
    }

    // // Update is called once per frame
    // void Update()
    // {
        
    // }


    private void Update() {
        if (Input.GetKeyDown(KeyCode.LeftShift) && interactCount <= 2) {
            interactCount = 9;
            StopAllCoroutines();
            dialogueRunning = true;
            StartCoroutine(displayProcess(interactCount++));
        }
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        if (!dialogueRunning) {
            dialogueRunning = true;
            StartCoroutine(displayProcess(interactCount++));
        }
    }

    public IEnumerator displayProcess(int index) {
        List<string> currentSet = commands[index];
        List<GameObject> FXList = new List<GameObject>();
        for (int i = 0; i < currentSet.Count; i++) {
            string[] cmdargs = currentSet[i].Split();
            if (cmdargs[1] == "echo") {
                yield return new WaitForSeconds(0.06f);
            }

            GameObject newPlatform = null;
            if (cmdargs[1] == "echo") {
                // // DEBUG:
                // commandLine.text = commands[index][i];
                // yield return new WaitForSeconds(0.5f);

                commandLine.text = "$ echo";
                yield return new WaitForSeconds(0.05f);
                commandLine.text = "$ echo ";
                yield return new WaitForSeconds(0.08f);
                for (int s = 2; s < cmdargs.Length; s++) {
                    for (int c = 0; c < cmdargs[s].Length; c++) {
                        commandLine.text += cmdargs[s][c];
                        yield return new WaitForSeconds(Random.Range(0.025f,0.04f));
                        if (cmdargs[s][c] == ',') {
                            yield return new WaitForSeconds(Random.Range(0.15f,0.2f));
                        }
                        if (cmdargs[s][c] == '.') {
                            yield return new WaitForSeconds(Random.Range(0.23f,0.33f));
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
                yield return new WaitForSeconds((currentSet[i].Split().Length - 2) / 8 + 1.3f);

                if (commandLine.text =="$ echo ... what just happened?") {
                    portalScript.StartRoutine();
                    yield return new WaitForSeconds(1.4f);
                }
                if (commandLine.text =="$ echo No, I should best...") {
                    yield return new WaitForSeconds(6f);
                    commandLine.text = "$ echo";
                    yield return new WaitForSeconds(180f);
                }
            } else {
                commandLine.text = currentSet[i];
                yield return new WaitForSeconds(0.15f);
                if (currentSet[i] == "$ force A 2102 --impulse") {
                    rb.AddForce(new Vector3(2500.0f, 1.0f, 0.0f), ForceMode2D.Impulse);
                    yield return new WaitForSeconds(0.2f);
                } else if (currentSet[i] == "$ force A -273 --impulse") {
                    rb.AddForce(new Vector3(-273.0f, 1.0f, 0.0f), ForceMode2D.Impulse);
                } else if (currentSet[i] == "$ force A 300 --impulse") {
                    rb.AddForce(new Vector3(-60.0f, 480.0f, 0.0f), ForceMode2D.Impulse);
                } else if (currentSet[i] == "$ force A 217 --impulse") {
                    rb.AddForce(new Vector3(800.0f, 217.0f, 0.0f), ForceMode2D.Impulse);
                    rb.gravityScale = 0.0f;
                } else if (currentSet[i] == "$ force A 9999 --instant") {
                    transform.position = new Vector3(9, transform.position.y, transform.position.z);
                    rb.AddForce(new Vector3(0f, -500f, 0.0f), ForceMode2D.Impulse);
                    rb.gravityScale = 0.0f;
                } else if (currentSet[i] == "$ force A 100 --impulse") {
                    rb.gravityScale = 1.0f;
                    rb.AddForce(new Vector3(0f, 400f, 0.0f), ForceMode2D.Impulse);
                } else if (currentSet[i] == "$ gravity A --g=0.1") {
                    rb.gravityScale = 0.02f;
                    rb.AddForce(new Vector3(0f, 50f, 0.0f), ForceMode2D.Impulse);
                } else if (currentSet[i] == "$ force P 9000 --instant") {
                    GameObject rel = GameObject.Find("Player");
                    rel.transform.position = new Vector3(27, 20, transform.position.z);
                } else if (currentSet[i] == "$ force A 1000 --instant") {
                    transform.position = new Vector3(66, 9f, transform.position.z);
                } else if (currentSet[i] == "$ force A 1100 --instant") {
                    transform.position = new Vector3(69, 1.9f, transform.position.z);
                } else if (currentSet[i] == "$ force P 9999 --instant") {
                    GameObject rel = GameObject.Find("Player");
                    rel.transform.position = new Vector3(90, 2, rel.transform.position.z);
                } else if (currentSet[i] == "$ force A 9998 --instant") {
                    transform.position = new Vector3(75, 3, transform.position.z);
                } else if (currentSet[i] == "$ gravity A --g=8.0") {
                    rb.gravityScale = 8.0f;
                } else if (currentSet[i] == "$ gravity A --none") {
                    rb.gravityScale = 0.0f;
                } else if (currentSet[i] == "$ gravity A --g=0.0") {
                    rb.gravityScale = 0.0f;
                } else if (cmdargs[1] == "sudo" && cmdargs[2] == "entity") {
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
                } else if (cmdargs[1] == "sudo" && cmdargs[2] == "add") {
                    
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

                } else if (cmdargs[1] == "sudo" && cmdargs[2] == "destroy") {
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
                        if (cmdargs[4] == "<size=2><color=white>-rf</color></size>") {
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
                            yield return new WaitForSeconds(0.3f);
                            portal.GetComponent<SpriteRenderer>().color = Color.black;
                            foreach (ParticleSystem ps in portal.GetComponentsInChildren<ParticleSystem>()) {
                                ps.startColor = Color.black;
                            }
                            portal.GetComponent<SpriteRenderer>().sortingOrder = 1000;
                            foreach (ParticleSystem ps in portal.GetComponentsInChildren<ParticleSystem>()) {
                                ps.GetComponent<ParticleSystemRenderer>().sortingOrder = 1000;
                            }
                            glitchRepeat[3].Play();
                            yield return new WaitForSeconds(0.4f);
                            glitchRepeat[3].Stop();
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
                    yield return new WaitForSeconds(0.06f);
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
        commandLine.text = "$ (hit me to continue)";
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
            if ((currentArgs.Length >= 5 && currentArgs[4] == "<size=2><color=white>-rf</color></size>") || rfd) {
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
                    else if (rfd || currentArgs[4] == "<size=2><color=white>-rf</color></size>") {

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

    public IEnumerator quickDestroy(GameObject rel) {
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

    public IEnumerator rmDestroy(GameObject rel) {
        int index_g1 = Random.Range(0, glitchRandom.Length);
        glitchRandom[index_g1].Play();
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
        yield return new WaitForSeconds(0.02f);
        for (int j = 1; j < 5; j++) {
            GameObject line = Instantiate(lineFX, new Vector3(0, 0, 0), Quaternion.Euler(0, 0, 0));
            line.GetComponent<LineRenderer>().startColor = spriteObject.GetComponent<SpriteRenderer>().color;
            line.GetComponent<LineRenderer>().endColor = spriteObject.GetComponent<SpriteRenderer>().color;
            Vector3[] points = new Vector3[] {spriteObject.transform.position, rel.transform.position + new Vector3(Random.Range(-1.3f * j / 4, 1.3f * j / 4), Random.Range(-1.3f * Mathf.Log(j) / 5, 1.3f * Mathf.Log(j) / 5), 0)};
            line.GetComponent<LineRenderer>().SetPositions(points);

            rel.transform.localScale += new Vector3(Random.Range(-1.0f * j / 7, 1.0f * j / 7), Random.Range(-1.0f * j / 7, 1.0f * j / 7), 0);
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
}
