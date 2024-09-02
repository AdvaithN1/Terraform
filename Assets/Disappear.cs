using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Disappear : MonoBehaviour
{
    public Camera view;
    public bool destroying;
    public AudioSource jumpscare;
    // Start is called before the first frame update
    void Start()
    {
        destroying = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Mathf.Abs(view.transform.position.x - transform.position.x) <= 20 && !destroying) {
            StartCoroutine(destroySelf());
            destroying = true;
        }
    }

    IEnumerator destroySelf() {
        jumpscare.Play();
        yield return new WaitForSeconds(0.5f);
        jumpscare.Stop();
        Destroy(gameObject);
    }
}
