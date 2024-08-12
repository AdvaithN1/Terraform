using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalCollision : MonoBehaviour
{
    public GameObject player;
    public SceneFader sceneFader;
    public string scene = "Home";
    public float time = 0;
    public int levelNumber;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (CompletedLevels.maxUnlockedLevel == levelNumber)
        {
            CompletedLevels.maxUnlockedLevel++;
        }
        if (collision.gameObject == player)
        {
            sceneFader.FadeToScene(scene);
        }
    }
}