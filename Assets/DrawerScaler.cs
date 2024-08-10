using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawerScaler : MonoBehaviour
{
    public Camera mainCamera;

    void Start()
    {
        if (mainCamera == null)
        {
            mainCamera = Camera.main;
        }

        RectTransform canvasRect = GetComponent<RectTransform>();

        if (canvasRect != null)
        {
            // Get the width of the camera in world units
            float cameraHeight = 2f * mainCamera.orthographicSize;
            float cameraWidth = cameraHeight * mainCamera.aspect;

            // Set the width of the canvas to match the camera width
            canvasRect.sizeDelta = new Vector2(cameraWidth, canvasRect.sizeDelta.y);
        }
        else
        {
            Debug.LogError("RectTransform component missing from Canvas.");
        }
    }
}
