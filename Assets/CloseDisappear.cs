using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CloseDisappear : MonoBehaviour
{
    public Camera view;
    public GameObject player;
    public GameObject sprite;
    public GameObject canvas;
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
        if (Mathf.Abs(view.transform.position.x - transform.position.x) <= 16 && !active) {
            canvas.SetActive(true);
            sprite.SetActive(true);
            active = true;
        } else if (active && Mathf.Abs(player.transform.position.x - transform.position.x) <= 1.6 && !destroying) {
            destroying = true;
            StartCoroutine(destroySelf());
        } else if (Mathf.Abs(player.transform.position.x - transform.position.x) <= 1.9) {
            text.text = "$ <size=2>:) :) :) :) :)</size>";
        } else if (Mathf.Abs(player.transform.position.x - transform.position.x) <= 6) {
            text.text = "$ <size=2>:)</size>";
        } else if (Mathf.Abs(player.transform.position.x - transform.position.x) <= 9) {
            text.text = "$ :)";
        } else if (Mathf.Abs(player.transform.position.x - transform.position.x) <= 12) {
            text.text = "$ :";
        } else if (Mathf.Abs(player.transform.position.x - transform.position.x) <= 15) {
            text.text = "$ ";
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
