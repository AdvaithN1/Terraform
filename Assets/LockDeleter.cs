using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockDeleter : MonoBehaviour
{
    public int lockLevelNumber;

    public void Start()
    {
        Debug.Log("Current Value: " + CompletedLevels.maxUnlockedLevel);
        if (CompletedLevels.maxUnlockedLevel >= lockLevelNumber)
        {
            gameObject.SetActive(false);
        }
    }
}
