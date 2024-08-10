using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeGame : MonoBehaviour
{
    public float speed = 0f;

    // Start is called before the first frame update
    void Start()
    {
        //Debug.Log("A");
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log("B");
        speed = speed + 9.8f * Time.deltaTime;
        transform.position += Vector3.down * speed * Time.deltaTime;
    }
}
