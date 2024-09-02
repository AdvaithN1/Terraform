using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scare : MonoBehaviour
{
    private bool done = false; 
    public AudioSource place; 
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.y > 37f && !done) {
            done = true;
            place.Play();
        }
    }
}
