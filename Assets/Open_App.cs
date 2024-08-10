using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Open_App : MonoBehaviour
{
    public GameObject targetObject;
    public float newZPosition = -1;

    // Start is called before the first frame update
    void OnMouseDown()
    {
        if (targetObject != null)
        {
            Vector3 targetPosition = targetObject.transform.position;
            targetPosition.z = newZPosition;
            targetObject.transform.position = targetPosition;
            Debug.Log("Moved Terminal to top");
        }
        Debug.Log("Object is null");
    }
}
