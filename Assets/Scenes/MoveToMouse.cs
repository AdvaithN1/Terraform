using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveToMouse : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Cursor.visible = false;
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        transform.position = new Vector3(mousePosition.x, mousePosition.y, 0);

        if (Input.GetMouseButton(0)){
            transform.localScale = new Vector3(1.4f, 1.4f, 1);
        }
        else {
            transform.localScale = new Vector3(2, 2, 1);
        }

    }
}
