using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BlockX : MonoBehaviour
{
    private InputField _inputField;
    // Start is called before the first frame update
    void Start()
    {
        _inputField = GetComponent<InputField>();
        _inputField.onValueChanged.AddListener(OnValueChanged);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnValueChanged(string newtext)
    {
        string cleanedText = _inputField.text.Replace("x", "").Replace("X", "");
        _inputField.text = cleanedText;
    }
}
