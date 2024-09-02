using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flicker : MonoBehaviour
{
    public Camera view;
    public bool destroying;
    public AudioSource jumpscare;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(destroySelf());
    }

    IEnumerator destroySelf() {
        yield return new WaitForSeconds(0.5f);
        Destroy(gameObject);
    }
}
