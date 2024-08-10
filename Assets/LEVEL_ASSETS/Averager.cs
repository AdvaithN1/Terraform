using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Averager : MonoBehaviour
{
    public GameObject assistant;
    public GameObject admin;
    public GameObject player;

    // Update is called once per frame
    void Update()
    {
        transform.position = (2 * assistant.transform.position + admin.transform.position + player.transform.position) / 4f;
    }
}
