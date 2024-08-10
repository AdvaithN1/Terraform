using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelRespawnManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.y < -50) {
            transform.position = new Vector3(-8, 0, transform.position.z);
        }
    }
}
