using UnityEngine.SceneManagement;
using UnityEngine;

public class HomeOpen : MonoBehaviour
{
    void OnMouseDown()
    {
        Debug.Log("LOADING SCENE");
        SceneManager.LoadScene("Home");
    }


}
