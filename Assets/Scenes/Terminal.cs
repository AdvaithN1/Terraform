using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Terminal : MonoBehaviour
{
    public InputField inputField;
    public Text outputText;

    private List<string> commandHistory = new List<string>();
    private int historyIndex = 0;

    void Start()
    {
        inputField.onEndEdit.AddListener(HandleInput);
    }

    void HandleInput(string input)
    {
        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            outputText.text += "\n> " + input;
            commandHistory.Add(input);
            historyIndex = commandHistory.Count;

            ProcessCommand(input);
            inputField.text = "";
            inputField.ActivateInputField();
        }
    }

    void ProcessCommand(string input)
    {
        // Simple command processing example
        if (input.ToLower() == "help")
        {
            outputText.text += "\nAvailable commands: help, clear";
        }
        else if (input.ToLower() == "clear")
        {
            outputText.text = "";
        }
        else
        {
            outputText.text += "\nUnknown command: " + input;
        }
    }

    void Update()
    {
        // Handle command history navigation
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            if (historyIndex > 0)
            {
                historyIndex--;
                inputField.text = commandHistory[historyIndex];
                inputField.MoveTextEnd(false);
            }
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            if (historyIndex < commandHistory.Count - 1)
            {
                historyIndex++;
                inputField.text = commandHistory[historyIndex];
                inputField.MoveTextEnd(false);
            }
            else
            {
                historyIndex = commandHistory.Count;
                inputField.text = "";
            }
        }
    }
}

