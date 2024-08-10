using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using UnityEngine.SceneManagement;
using System.Collections;

public class TitleTransition : MonoBehaviour
{
    public VideoPlayer videoPlayer;
    //public CanvasGroup canvasGroup;
    //public float fadeDuration = 1.0f;
    public string nextSceneName;
    public SceneFader sceneFader;


    //private bool isFading = false;

    void Start()
    {
        videoPlayer.loopPointReached += OnVideoEnd;
        //canvasGroup.alpha = 0;  // Make sure the canvas is transparent at the start
    }

    void OnVideoEnd(VideoPlayer vp)
    {
        StartCoroutine(FadeOutAndLoadScene());
    }

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
        sceneFader.FadeToScene(nextSceneName);
        yield return null;

    }

    void Update()
    {
        // Optional: This is to ensure the canvas is not interactable during the fade
        //if (isFading)
        //{
        //    canvasGroup.blocksRaycasts = true;
        //}
        //else
        //{
        //    canvasGroup.blocksRaycasts = false;
        //}
    }
}
