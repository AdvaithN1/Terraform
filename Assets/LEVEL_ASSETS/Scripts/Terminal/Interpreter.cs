using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Globalization;
using UnityEngine.UI;
using System.Threading;
using Unity.VisualScripting;

public class Interpreter : MonoBehaviour {


    public GameObject platformPrefab;
    public GameObject wallPrefab;
    public GameObject lineFX;
    public GameObject currPlatform;
    public PlayerAnimator AnimHandler;
    public AudioSource cmdSound;
    public bool godmodeUnlocked;

    public AudioSource[] glitchRandom;
    public AudioSource[] glitchRepeat;
    public AudioSource[] glitchBlip;
    public GameObject _player;
    private float lastShiftTime = 0f;

    List<string> response = new();

    private void Start() {
    }

    GameObject FindClosestGameObjectWithName(string nameFragment)
    {
        GameObject[] allObjects = GameObject.FindObjectsOfType<GameObject>();
        GameObject closestObject = null;
        float closestDistance = 15f;

        foreach (GameObject obj in allObjects)
        {
            if (obj.name.Contains(nameFragment) && obj.activeInHierarchy)
            {
                float distance = Mathf.Pow(Mathf.Pow(_player.transform.position.x - obj.transform.position.x, 2) + Mathf.Pow(_player.transform.position.y - obj.transform.position.y, 2), 0.5f);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestObject = obj;
                }
            }
        }

        return closestObject;
    }

    private void Update() {
        if (godmodeUnlocked) {
            if (Input.GetKeyDown(KeyCode.LeftShift) && Time.time - lastShiftTime >= 0.1) { // add temp platform
                lastShiftTime = Time.time;
                AnimHandler.doCommand = true;
                AnimHandler.playerText.text = "$ sudo add --platform -1 0";
                GameObject spriteObject = GameObject.Find("Player").transform.Find("Square").gameObject;
                currPlatform = Instantiate(platformPrefab, spriteObject.transform.position + new Vector3(0, -1, 0), Quaternion.Euler(0, 0, 0));
                currPlatform.name = "temp";
                StartCoroutine(animateAdd(currPlatform));
            }
            if (Input.GetKeyUp(KeyCode.LeftShift)) {
                if (!currPlatform.IsDestroyed()) {
                    StartCoroutine(zDestroy(currPlatform));
                }
            }
            if (Input.GetKeyDown(KeyCode.X)) { // destroy other platform
                AnimHandler.doCommand = true;
                GameObject other = FindClosestGameObjectWithName("%");
                AnimHandler.playerText.text = "$ sudo destroy % 1";
                if (other != null) {
                    StartCoroutine(xDestroy(other));
                }
            }
        }
    }
    

    public List<string> Interpret(string userInput) {
        // Debug.Log("Interpreted.");
        response.Clear();
        string[] args = userInput.Split();
        if (args[0] == "sudo") {
            if (args[1] == "help") {
                response.Add("sudo help - displays this information");
                response.Add("sudo add --[type] [dx] [dy] - creates a new entity at dx dy from player");
                response.Add("sudo move [blockID] [dist-x] [dist-y] - Moves a block a certain amount dx and dy");
                response.Add("sudo destroy [blockID] - Destroys a block");
            } else if (args[1] == "move") {
                string block = args[2];
                float dx = float.Parse(args[3], CultureInfo.InvariantCulture);
                float dy = float.Parse(args[4], CultureInfo.InvariantCulture);

                GameObject obj = GameObject.Find(block);

                if (obj != null) {
                    StartCoroutine(animateMove(obj, dx, dy));
                    response.Add("Moved entity " + block + " to new position.");
                } else {
                    response.Add("zsh: entity not found: " + block);
                }
            } else if (args[1] == "destroy") {
                string block = args[2];
                GameObject rel = GameObject.Find(block);

                if (rel != null) {
                    StartCoroutine(animateDestroy(rel));
                    response.Add("Destroyed entity " + block);
                } else {
                    response.Add("zsh: entity not found: " + block);
                }
            } else if (args[1] == "add") {
                string block = args[2];
                float dx = float.Parse(args[3], CultureInfo.InvariantCulture);
                float dy = float.Parse(args[4], CultureInfo.InvariantCulture);
                GameObject newPlatform = null;
                GameObject spriteObject = GameObject.Find("Player").transform.Find("Square").gameObject;
                if (args[2] == "--platform") {
                    newPlatform = Instantiate(platformPrefab, spriteObject.transform.position + new Vector3(dx, dy, 0), Quaternion.Euler(0, 0, 0));
                    response.Add("Added a new platform at " + (dx + spriteObject.transform.position.x) + " " + (dy + spriteObject.transform.position.y));
                    newPlatform.name = "UserB";
                } else if (args[2] == "--wall") {
                    newPlatform = Instantiate(wallPrefab, spriteObject.transform.position + new Vector3(dx, dy, 0), Quaternion.Euler(0, 0, 0));
                    response.Add("Added a new wall at " + (dx + spriteObject.transform.position.x) + " " + (dy + spriteObject.transform.position.y));
                    newPlatform.name = "UserB";
                } else {
                    response.Add("zsh: invalid type: " + block);
                }

                if (newPlatform != null) {
                    StartCoroutine(animateAdd(newPlatform));
                }
            } else {
                response.Add("zsh: command not found: " + args[1]);
            }
        } 
        else if (args[0] == "help") {
            response.Add("To modify game content, run a sudo command.");
            response.Add("You can run \"sudo help\" for more details.");
                // ...
        } 
        else {
            response.Add("zsh: command not found: " + args[0]);
            response.Add("For help on how to use the terminal, run \"help\".");
        }
        return response;
    }


    IEnumerator animateMove(GameObject rel, float dx, float dy) {
        
        Vector3 final_pos = rel.transform.position;
        Color o_color = rel.GetComponent<SpriteRenderer>().color;
        final_pos.x += dx;
        final_pos.y += dy;
        Vector3 o_pos = rel.transform.position;
        List<GameObject> FXList = new();
        SpriteRenderer rel_spriteRenderer = rel.GetComponent<SpriteRenderer>();
        int index = Random.Range(0, glitchRandom.Length);
        glitchRandom[index].Play();
        GameObject spriteObject = GameObject.Find("Player").transform.Find("Square").gameObject;
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
        for (int j = 1; j < 6; j++) {
            GameObject line = Instantiate(lineFX, new Vector3(0, 0, 0), Quaternion.Euler(0, 0, 0));
            line.GetComponent<LineRenderer>().startColor = spriteObject.GetComponent<SpriteRenderer>().color;
            line.GetComponent<LineRenderer>().endColor = spriteObject.GetComponent<SpriteRenderer>().color;
            Vector3 disp;
            Vector3[] points;
            if (j == 4) {
                glitchRandom[index].Pause();
                int index_1 = Random.Range(0, glitchRepeat.Length);
                glitchRepeat[index_1].Play();
                Vector3 currentPos = rel.transform.position;
                disp = new(Random.Range(-1.0f * j / 5, 1.0f * j / 5), Random.Range(-1.0f * j / 5, 1.0f * j / 5), 0);
                points = new Vector3[] {spriteObject.transform.position, final_pos + disp};
                for (int k = 0; k < 3; k++) {
                    rel.transform.position = final_pos + disp;
                    yield return new WaitForSeconds(0.02f);
                    rel.transform.position = currentPos;
                    yield return new WaitForSeconds(0.02f);
                }
                glitchRepeat[index_1].Stop();
                rel.transform.position += disp;
                glitchRandom[index].UnPause();
            } else {
                disp = new(Random.Range(-1.0f * j / 5, 1.0f * j / 5), Random.Range(-1.0f * j / 5, 1.0f * j / 5), 0);
                points = new Vector3[] {spriteObject.transform.position, rel.transform.position + disp};
                rel.transform.position += disp;
            }
            if (j == 5) {
                rel.transform.position = o_pos;
            }
            line.GetComponent<LineRenderer>().SetPositions(points);
            yield return new WaitForSeconds(1.4f / (j + 20));
            Destroy(line);
            

            if (j == 1) {
                for (int k = 2; k < FXList.Count; k++) {
                    Destroy(FXList[k]);
                }
            }
            if (j == 2) {
                Destroy(FXList[0]);
                Destroy(FXList[1]);
            }
            if (j >= 4) {
                if (j >= 5) {
                    Destroy(FXList[0]);
                }
                FXList[0] = Instantiate(lineFX, new Vector3(0, 0, 0), Quaternion.Euler(0, 0, 0));
                line.GetComponent<LineRenderer>().startColor = spriteObject.GetComponent<SpriteRenderer>().color;
                line.GetComponent<LineRenderer>().endColor = spriteObject.GetComponent<SpriteRenderer>().color;
                disp = new(Random.Range(-1.0f * j / 5, 1.0f * j / 5), Random.Range(-1.0f * j / 5, 1.0f * j / 5), 0);
                points = new Vector3[] {spriteObject.transform.position, final_pos + disp};
                line.GetComponent<LineRenderer>().SetPositions(points);
            }
        }
        FXList.Clear();


        glitchRandom[index].Pause();
        GameObject rel_line = Instantiate(lineFX, new Vector3(0, 0, 0), Quaternion.Euler(0, 0, 0));
        rel_line.GetComponent<LineRenderer>().startColor = spriteObject.GetComponent<SpriteRenderer>().color;
        rel_line.GetComponent<LineRenderer>().endColor = spriteObject.GetComponent<SpriteRenderer>().color;
        Vector3[] pts;
        pts = new Vector3[] {spriteObject.transform.position, final_pos};
        int index_2 = Random.Range(0, glitchRepeat.Length);
        glitchRepeat[index_2].Play();
        for (int k = 0; k < 6; k++) {
            rel.transform.position = final_pos;
            yield return new WaitForSeconds(0.02f);
            rel.transform.position = o_pos;
            yield return new WaitForSeconds(0.02f);
        }
        glitchRepeat[index_2].Stop();
        rel_line.GetComponent<LineRenderer>().SetPositions(pts);
        Destroy(rel_line);


        glitchRandom[index].UnPause();
        rel.transform.position = final_pos;
        for (int j = 4; j > 0; j--) {
            GameObject line = Instantiate(lineFX, new Vector3(0, 0, 0), Quaternion.Euler(0, 0, 0));
            line.GetComponent<LineRenderer>().startColor = spriteObject.GetComponent<SpriteRenderer>().color;
            line.GetComponent<LineRenderer>().endColor = spriteObject.GetComponent<SpriteRenderer>().color;
            Vector3 disp;
            Vector3[] points;
            if (j == 2) {
                glitchRandom[index].Pause();
                Vector3 currentPos = rel.transform.position;
                disp = new(Random.Range(-4.0f * j / 5, 4.0f * j / 5), Random.Range(-4.0f * j / 5, 4.0f * j / 5), 0);
                points = new Vector3[] {spriteObject.transform.position, final_pos + disp};
                int index_3 = Random.Range(0, glitchRepeat.Length);
                glitchRepeat[index_3].Play();
                for (int k = 0; k < 2; k++) {
                    rel.transform.position = final_pos + disp;
                    rel.GetComponent<SpriteRenderer>().color = spriteObject.GetComponent<SpriteRenderer>().color;
                    yield return new WaitForSeconds(0.02f);
                    rel.transform.position = currentPos;
                    rel.GetComponent<SpriteRenderer>().color = o_color;
                    yield return new WaitForSeconds(0.02f);
                }
                glitchRepeat[index_3].Stop();
                rel.transform.position += disp;
                glitchRandom[index].UnPause();
            } else {
                disp = new(Random.Range(-1.0f * j / 5, 1.0f * j / 5), Random.Range(-1.0f * j / 5, 1.0f * j / 5), 0);
                points = new Vector3[] {spriteObject.transform.position, rel.transform.position + disp};
                rel.transform.position += disp;
            }
            line.GetComponent<LineRenderer>().SetPositions(points);
            yield return new WaitForSeconds(1.2f / (23 - j));
            Destroy(line);
        }
        rel.transform.position = final_pos;

        rel.GetComponent<RectTransform>().GetWorldCorners(rel_corners);
        for (int j = 0; j < rel_corners.Length; j++) {
            yield return new WaitForSeconds(0.01f);
            GameObject line = Instantiate(lineFX, new Vector3(0, 0, 0), Quaternion.Euler(0, 0, 0));
            line.GetComponent<LineRenderer>().startColor = spriteObject.GetComponent<SpriteRenderer>().color;
            line.GetComponent<LineRenderer>().endColor = spriteObject.GetComponent<SpriteRenderer>().color;
            Vector3[] points = new Vector3[] {spriteObject.transform.position, rel_corners[j]};
            line.GetComponent<LineRenderer>().SetPositions(points);
            FXList.Add(line);
        }

        glitchRandom[index].Stop();
        yield return new WaitForSeconds(0.2f);
        Destroy(FXList[0]);
        yield return new WaitForSeconds(0.01f);
        Destroy(FXList[1]);
        yield return new WaitForSeconds(0.02f);
        Destroy(FXList[3]);
        yield return new WaitForSeconds(0.01f);
        Destroy(FXList[2]);
        yield return new WaitForSeconds(0.6f);
        rel.GetComponent<SpriteRenderer>().color = spriteObject.GetComponent<SpriteRenderer>().color;
        yield return new WaitForSeconds(0.06f);
        rel.GetComponent<SpriteRenderer>().color = o_color;
    }


    IEnumerator animateAdd(GameObject newPlatform) {
        List<GameObject> FXList = new();
        glitchBlip[Random.Range(0, glitchBlip.Length)].Play();
        GameObject spriteObject = GameObject.Find("Player").transform.Find("Square").gameObject;
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

        yield return new WaitForSeconds(0.04f);
        newPlatform.SetActive(false);
        Destroy(FXList[1]);
        Destroy(FXList[3]);
        yield return new WaitForSeconds(0.02f);
        newPlatform.SetActive(true);
        yield return new WaitForSeconds(0.018f);
        newPlatform.SetActive(false);
        Destroy(FXList[0]);
        yield return new WaitForSeconds(0.07f);
        newPlatform.SetActive(true);
    }

    IEnumerator animateDestroy(GameObject rel) {
        List<GameObject> FXList = new();
        SpriteRenderer rel_spriteRenderer = rel.GetComponent<SpriteRenderer>();
        int index = Random.Range(0, glitchRandom.Length);
        glitchRandom[index].Play();
        GameObject spriteObject = GameObject.Find("Player").transform.Find("Square").gameObject;
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
        for (int j = 1; j < 11; j++) {
            GameObject line = Instantiate(lineFX, new Vector3(0, 0, 0), Quaternion.Euler(0, 0, 0));
            line.GetComponent<LineRenderer>().startColor = spriteObject.GetComponent<SpriteRenderer>().color;
            line.GetComponent<LineRenderer>().endColor = spriteObject.GetComponent<SpriteRenderer>().color;
            Vector3[] points = new Vector3[] {spriteObject.transform.position, rel.transform.position + new Vector3(Random.Range(-1.2f * j / 4, 1.2f * j / 4), Random.Range(-1.2f * Mathf.Log(j) / 5, 1.2f * Mathf.Log(j) / 5), 0)};
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

        Destroy(rel);

        glitchRandom[index].Stop();
    }

    IEnumerator zDestroy(GameObject rel) {
        List<GameObject> FXList = new();
        SpriteRenderer rel_spriteRenderer = rel.GetComponent<SpriteRenderer>();
        int index = Random.Range(0, glitchRandom.Length);
        glitchRandom[index].Play();
        GameObject spriteObject = GameObject.Find("Player").transform.Find("Square").gameObject;
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
            GameObject line = Instantiate(lineFX, new Vector3(0, 0, 0), Quaternion.Euler(0, 0, 0));
            line.GetComponent<LineRenderer>().startColor = spriteObject.GetComponent<SpriteRenderer>().color;
            line.GetComponent<LineRenderer>().endColor = spriteObject.GetComponent<SpriteRenderer>().color;
            Vector3[] points = new Vector3[] {spriteObject.transform.position, rel.transform.position + new Vector3(Random.Range(-1.2f * j / 4, 1.2f * j / 4), Random.Range(-1.2f * Mathf.Log(j) / 5, 1.2f * Mathf.Log(j) / 5), 0)};
            line.GetComponent<LineRenderer>().SetPositions(points);

            rel.transform.localScale += new Vector3(Random.Range(-1.0f * j / 5, 1.0f * j / 5), Random.Range(-1.0f * j / 5, 1.0f * j / 5), 0);
            yield return new WaitForSeconds(1f / (j + 20));
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

        rel.SetActive(false);

        glitchRandom[index].Stop();
        yield return new WaitForSeconds(2f);
        Destroy(rel);
    }

    IEnumerator xDestroy(GameObject rel) {
        Vector3 opos = rel.transform.position;
        List<GameObject> FXList = new();
        SpriteRenderer rel_spriteRenderer = rel.GetComponent<SpriteRenderer>();
        int index = Random.Range(0, glitchRandom.Length);
        glitchRandom[index].Play();
        GameObject spriteObject = GameObject.Find("Player").transform.Find("Square").gameObject;
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
        for (int j = 1; j < 11; j++) {
            GameObject line = Instantiate(lineFX, new Vector3(0, 0, 0), Quaternion.Euler(0, 0, 0));
            line.GetComponent<LineRenderer>().startColor = spriteObject.GetComponent<SpriteRenderer>().color;
            line.GetComponent<LineRenderer>().endColor = spriteObject.GetComponent<SpriteRenderer>().color;
            Vector3[] points = new Vector3[] {spriteObject.transform.position, rel.transform.position + new Vector3(Random.Range(-1.2f * j / 4, 1.2f * j / 4), Random.Range(-1.2f * Mathf.Log(j) / 5, 1.2f * Mathf.Log(j) / 5), 0)};
            line.GetComponent<LineRenderer>().SetPositions(points);

            rel.transform.localScale += new Vector3(Random.Range(-1.0f * j / 4, 1.0f * j / 4), Random.Range(-1.0f * j / 4, 1.0f * j / 4), 0);
            rel.transform.position = opos;
            yield return new WaitForSeconds(0.9f / (j + 20));
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

        rel.SetActive(false);

        glitchRandom[index].Stop();
    }
}
