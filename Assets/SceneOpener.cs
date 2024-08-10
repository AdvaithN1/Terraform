using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using UnityEngine.SceneManagement;
using System.Collections;

public class SceneOpener : MonoBehaviour
{
    //public GameObject folderContent;
    public string scene;
    public SceneFader sceneFader;


    IEnumerator FadeOutAndLoadScene()
    {
        //isFading = true;
        //float timer = 0;

        //while (timer < fadeDuration)
        //{
        //    timer += Time.deltaTime;
        //    //canvasGroup.alpha = Mathf.Lerp(0, 1, timer / fadeDuration);
        //    yield return null;
        //}
        Debug.Log("Fading out scene");
        sceneFader.FadeToScene(scene);
        yield return null;

    }

    void OnMouseDown()
    {
        Debug.Log("LOADING SCENE");
        StartCoroutine(FadeOutAndLoadScene());
    }
}
