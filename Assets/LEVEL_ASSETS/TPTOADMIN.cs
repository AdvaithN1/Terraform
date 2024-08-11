using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TPTOADMIN : MonoBehaviour
{
    private GameObject player;
    private GameObject assistant;

    void Start()
    {
        player = GameObject.Find("Player");
        assistant = GameObject.Find("Assistant");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnCollisionEnter2D() {
        player.transform.position = new Vector3(70, 6, player.transform.position.z);
        player.GetComponent<RespawnManager>().respawnPos = new Vector3(70, 6, player.transform.position.z);
        // assistant.transform.position = new Vector3(558, -2, assistant.transform.position.z);
        // AssistantManager am = assistant.GetComponent<AssistantManager>();
        // am.commands = new List<List<string>>();
        // am.interactCount = 0;
        
    }
}
