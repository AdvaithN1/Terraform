using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class FakeLoadingScreen : MonoBehaviour
{
    public GameObject loadingPanel;
    public Slider loadingBar;
    public Text loadingText;
    public string scene = "temp";
    public SceneFader sceneFader;
    public bool corrupted = false;

    private string[] loadingMessages = new string[]
    {
        "Preparing Scanner",
        "Decrypting file",
        "Establishing Socket",
        "Reading File",
        "Initializing Terminal"
    };

    private void Start()
    {
        if (corrupted) {
            loadingMessages = new string[]
            {
                "Preparing <color=red>WATCHING</color>",
                "<color=red>ALWAYS</color> file",
                "<color=red>IM</color> <color=red>WATCHING</color>",
                "Readi<color=red>IM</color> <color=red>WATCHING</color>",
                "<color=red>ALWAYS</color> <color=red>WATCHING</color>"
            };
        }
        // Hide the loading panel at the start
        loadingPanel.SetActive(false);
    }

    public void StartLoading(string scene)
    {
        // Debug.Log("StARTING LOADING");
        // Show the loading panel
        loadingPanel.SetActive(true);

        this.scene = scene;

        // Start the loading process
        StartCoroutine(Load());
    }

    private IEnumerator Load()
    {
        loadingBar.value = 0;

        //OLD TIMING:
        //
        //for (int i = 0; i < loadingMessages.Length; i++)
        //{
        //    loadingText.text = loadingMessages[i];

        //    while (loadingBar.value < (i + 1) / (float)loadingMessages.Length)
        //    {
        //        loadingBar.value += Time.deltaTime / 1;
        //        yield return null;
        //    }

        //    //yield return new WaitForSeconds(0.5f);
        //}


        ////NEW TIMING:
        //for (int i = 0; i < loadingMessages.Length; i++)
        //{
        //    loadingText.text = loadingMessages[i];

        //    float targetValue = (i + 1) / (float)loadingMessages.Length;
        //    float randomDuration = Random.Range(0f, 0.5f);
        //    float elapsedTime = 0f;

        //    while (loadingBar.value < targetValue)
        //    {
        //        elapsedTime += Time.deltaTime;
        //        loadingBar.value = Mathf.Lerp(loadingBar.value, targetValue, elapsedTime / randomDuration);
        //        yield return null;
        //    }

        //    yield return new WaitForSeconds(Random.Range(0.1f, 0.5f));
        //}

        // NEWER TIMING:
        for (int i = 0; i < loadingMessages.Length; i++)
        {
            loadingText.text = loadingMessages[i];

            float targetValue = (i + 1) / (float)loadingMessages.Length;
            float randomDuration = Random.Range(0.08f, 0.4f); // Random duration between 0.5 and 2 seconds
            float randomSpeed = Random.Range(0.1f, 0.5f); // Random speed multiplier
            float elapsedTime = 0f;

            
            float stopPoint = Random.Range(loadingBar.value, targetValue);

            while (loadingBar.value < targetValue)
            {
                elapsedTime += Time.deltaTime * randomSpeed;

                if (loadingBar.value < stopPoint)
                {
                    loadingBar.value = Mathf.Lerp(loadingBar.value, stopPoint, elapsedTime / randomDuration);
                }
                else
                {
                    elapsedTime = 0f;
                    stopPoint = targetValue;
                }

                yield return null;
            }

            yield return new WaitForSeconds(Random.Range(0.2f, 0.5f)); 
        }


        loadingText.text = "Complete!";
        if (corrupted) {
            loadingText.text = "<b><color=red>:) :) :) :) :)</color></b>";
        }
        yield return new WaitForSeconds(0.8f);
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
        sceneFader.FadeToScene(scene);
        return null;

    }
}
