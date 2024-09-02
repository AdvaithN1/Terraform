using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine.UI;
using UnityEngine;

public class DestroyOnContact : MonoBehaviour
{

    public GameObject amongus;
    public GameObject player;
    public Text display;
    public AudioSource[] glitchRandom;
    public GameObject lineFX;
    private bool destroyed1;
    public bool postScare = false;

    void Start() {
        destroyed1 = false;
        gameObject.transform.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, player.transform.position.z);
    }
    
    void Update()
    {
        if (postScare) {
            if (Vector3.Distance(gameObject.transform.position, player.transform.position) < 3f) {
                display.fontSize +=10;
                destroySelf();
                postScare = false;
            }
        }
        if (Physics2D.Distance(amongus.GetComponent<BoxCollider2D>(), player.GetComponent<BoxCollider2D>()).distance <= 0.7f && !destroyed1) {
            Destroy(amongus.GetComponent<BoxCollider2D>());
            StartCoroutine(quickDestroy(amongus));
            destroyed1 = true;
        }
    }

    public IEnumerator quickDestroy(GameObject rel) {
        int index_g1 = Random.Range(0, glitchRandom.Length);
        glitchRandom[index_g1].Play();
        List<GameObject> FXList = new List<GameObject>();
        Vector3[] rel_corners = new Vector3[4];
        rel.GetComponent<RectTransform>().GetWorldCorners(rel_corners);
        rel.GetComponent<SpriteRenderer>().color = Color.red;

        for (int j = 0; j < rel_corners.Length; j++) {
            GameObject line = Instantiate(lineFX, new Vector3(0, 0, 0), Quaternion.Euler(0, 0, 0));
            line.GetComponent<LineRenderer>().startColor = gameObject.GetComponent<SpriteRenderer>().color;
            line.GetComponent<LineRenderer>().endColor = gameObject.GetComponent<SpriteRenderer>().color;
            Vector3[] points = new Vector3[] {gameObject.transform.position, rel_corners[j]};
            line.GetComponent<LineRenderer>().SetPositions(points);
            FXList.Add(line);
        }
        yield return new WaitForSeconds(0.01f);
        for (int j = 1; j < 8; j++) {
            GameObject line = Instantiate(lineFX, new Vector3(0, 0, 0), Quaternion.Euler(0, 0, 0));
            line.GetComponent<LineRenderer>().startColor = gameObject.GetComponent<SpriteRenderer>().color;
            line.GetComponent<LineRenderer>().endColor = gameObject.GetComponent<SpriteRenderer>().color;
            Vector3[] points = new Vector3[] {gameObject.transform.position, rel.transform.position + new Vector3(Random.Range(-1.3f * j / 4, 1.3f * j / 4), Random.Range(-1.3f * Mathf.Log(j) / 5, 1.3f * Mathf.Log(j) / 5), 0)};
            line.GetComponent<LineRenderer>().SetPositions(points);

            rel.transform.localScale += new Vector3(Random.Range(-1.0f * j / 3, 1.0f * j / 5), Random.Range(-2.0f * j / 4, 2.0f * j / 4), 0);
            yield return new WaitForSeconds(1.7f / (j + 20));
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
        if (!postScare) {
            yield return new WaitForSeconds(0.8f);
            Destroy(gameObject);
        }
    }

    public IEnumerator destroySelf() {
        yield return new WaitForSeconds(0.8f);
        Destroy(gameObject);
    }


}
