using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadingOpener : MonoBehaviour
{
    public string scene;
    public FakeLoadingScreen f;

    void OnMouseDown()
    {
        Debug.Log("clicky");
        f.StartLoading(scene);
    }

    public void Open()
    {
        Debug.Log("clicky");
        f.StartLoading(scene);
    }
}