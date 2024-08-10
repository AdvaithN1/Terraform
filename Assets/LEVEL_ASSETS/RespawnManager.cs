using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnManager : MonoBehaviour
{
    public Vector3 respawnPos {get; set;}
    // Start is called before the first frame update
    void Start()
    {
        respawnPos = new Vector3(-8, 0, transform.position.z);
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.y < -30) {
            transform.position = respawnPos;
        }
    }
}
