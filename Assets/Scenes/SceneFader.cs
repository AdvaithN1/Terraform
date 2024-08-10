using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class SceneFader : MonoBehaviour
{
    public Image fadeImage;
    public float fadeDuration = 1f;
    public Canvas flickerCanvas;
    public float notFlickerFreq = 0.95f;
    public float holdTime = 0.4f;

    Color[] colors = new Color[]
        {
            //new Color(255, 0, 0),     // Red
            //new Color(0, 255, 0),     // Green
            //new Color(0, 0, 255),     // Blue
            //new Color(0, 255, 255),   // Cyan
            //new Color(255, 255, 0),   // Yellow
            //new Color(255, 0, 255),   // Magenta
            new Color(255, 255, 255), // White
            new Color(0, 0, 0)        // Black
        };

    private Transform[] childPanels;
    public AudioSource[] glitchRandom;

    private void Start()
    {
        // Cache child panels
        childPanels = new Transform[flickerCanvas.transform.childCount];
        for (int i = 0; i < flickerCanvas.transform.childCount; i++)
        {
            childPanels[i] = flickerCanvas.transform.GetChild(i);
            childPanels[i].GetComponent<Image>().enabled = false;
        }


        // Optionally, start with a fade in
        StartCoroutine(FadeIn());
    }

    public void FadeToScene(string sceneName)
    {
        StartCoroutine(FadeOut(sceneName));
    }

    private IEnumerator FadeIn()
    {
        yield return new WaitForSeconds(holdTime);
        float elapsedTime = 0f;
        Color color = fadeImage.color;
        Color originalColor = fadeImage.color;
        originalColor.a = 0;
        fadeImage.color = originalColor;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;

            // Random flicker effect
            if (Random.value > notFlickerFreq) // Adjust the flicker frequency as needed
            {
                FlickerRandomPanel(); // Flicker a random panel
            }
            else
            {
                //fadeImage.color = originalColor;
                //color.a = 1 - Mathf.Clamp01(elapsedTime / fadeDuration);
                //fadeImage.color = color;
            }

            yield return null;
        }

        // Ensure the image is fully transparent at the end
        color.a = 0f;
        fadeImage.color = color;
    }

    private IEnumerator FadeOut(string sceneName)
    {
        float elapsedTime = 0f;
        Color color = fadeImage.color;
        Color originalColor = fadeImage.color;
        originalColor.a = 0;
        fadeImage.color = originalColor;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;

            // Random flicker effect
            if (Random.value > notFlickerFreq) // Adjust the flicker frequency as needed
            {
                FlickerRandomPanel(); // Flicker a random panel
            }
            else
            {
                //fadeImage.color = originalColor;
                //color.a = Mathf.Clamp01(elapsedTime / fadeDuration);
                //fadeImage.color = color;
            }

            yield return null;
        }

        // Ensure the image is fully opaque at the end
        color.a = 1f;
        fadeImage.color = color;

        SceneManager.LoadScene(sceneName);
    }

    private void FlickerRandomPanel()
    {
        int randomIndex = Random.Range(0, childPanels.Length);
        Image panelImage = childPanels[randomIndex].GetComponent<Image>();

        if (panelImage != null)
        {
            

            StartCoroutine(FlickerPanel(panelImage));
        }
    }

    private IEnumerator FlickerPanel(Image panelImage)
    {
        Debug.Log("ENABLING OBJECT: " + panelImage);
        panelImage.enabled = true; // Show the panel

        int index = Random.Range(0, glitchRandom.Length);
        glitchRandom[index].Play();

        if (!panelImage.gameObject.tag.Equals("NoChange")){
            int randnum = Random.Range(0, colors.Length);
            panelImage.color = colors[randnum];
            Debug.Log("Updated the color of: " + panelImage.gameObject);
        }
        yield return new WaitForSeconds(0.03f); // Adjust the display duration as needed
        panelImage.enabled = false; // Hide the panel
        yield return new WaitForSeconds(0.3f);
        glitchRandom[index].Stop();
    }

}

