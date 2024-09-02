using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AppearThenDisappear : MonoBehaviour
{
    public Camera view;
    public GameObject sprite;
    public GameObject canvas;
    public GameObject player;
    public Text text;
    public bool active;
    public bool destroying;
    public AudioSource jumpscare;
    // Start is called before the first frame update
    void Start()
    {
        active = false;
        destroying = false;
        canvas.SetActive(false);
        sprite.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (player.transform.position.x >= 141 && player.transform.position.y >= 34 && !active) {
            canvas.SetActive(true);
            sprite.SetActive(true);
            active = true;
        }
        if (active && !destroying && Mathf.Abs(view.transform.position.x - transform.position.x) <= 12) {
            destroying = true;
            StartCoroutine(destroySelf());
        }

    }

    IEnumerator destroySelf() {
        canvas.SetActive(false);
        sprite.SetActive(false);
        jumpscare.Play();
        yield return new WaitForSeconds(0.2f);
        jumpscare.Stop();
        Destroy(gameObject);
    }
}
