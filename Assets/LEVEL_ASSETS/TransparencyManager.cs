using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TransparencyManager : MonoBehaviour
{
    private float startTime;
    private bool update;

    void Awake() {
        startTime = Time.time;
        update = true;
    }

    void Update()
    {
        if (update)  {
        foreach (Transform child in transform)
        {
            if (Mathf.Min(1f + (3f + Mathf.Min(startTime - Time.time, -3f) / 5f), 1f) <= 0) {
                child.GetComponent<Text>().color = new Color(child.GetComponent<Text>().color.r, child.GetComponent<Text>().color.g, child.GetComponent<Text>().color.b, 0f);

                update = false;
            } else {
                child.GetComponent<Text>().color = new Color(child.GetComponent<Text>().color.r, child.GetComponent<Text>().color.g, child.GetComponent<Text>().color.b, Mathf.Min(1f + (3f + Mathf.Min(startTime - Time.time, -3f) / 5f), 1f));
            }
        }
        }
    }
}
