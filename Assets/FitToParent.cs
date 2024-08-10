using UnityEngine;

public class FitToParent : MonoBehaviour
{
    void Start()
    {
        RectTransform parentRectTransform = GetComponentInParent<RectTransform>();
        RectTransform childRectTransform = GetComponent<RectTransform>();

        if (parentRectTransform != null && childRectTransform != null)
        {
            childRectTransform.anchorMin = new Vector2(0, 0);
            childRectTransform.anchorMax = new Vector2(1, 1);
            childRectTransform.sizeDelta = Vector2.zero;
        }
    }
}
