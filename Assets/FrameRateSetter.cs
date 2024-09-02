using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrameRateSetter : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Application.targetFrameRate = 60;
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(1.0f / Time.deltaTime);
    }
}
