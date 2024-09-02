using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TerminalManager : MonoBehaviour
{

    public GameObject directoryLine;
    public GameObject responseLine;
    public InputField terminalInput;
    public GameObject userInputLine;
    public GameObject msgList;
    public AudioSource commandSFX;

    private Interpreter interpreter;


    void Start()
    {
        interpreter = GetComponent<Interpreter>();
        terminalInput.ActivateInputField();
        terminalInput.Select();
    }


    void Update()
    {
        if (terminalInput.text.Length > 0) {
            if (terminalInput.text[^1] == 'x' || terminalInput.text[^1] == 'X') {
                terminalInput.text.Remove(terminalInput.text.Length - 1);
            }
        }
        terminalInput.caretPosition = terminalInput.text.Length + 1;
        if (!terminalInput.isFocused) {
            terminalInput.ActivateInputField();
            terminalInput.Select();
        }
    }

    private void OnGUI() {
        if (terminalInput.isFocused && terminalInput.text != "" && Input.GetKeyDown(KeyCode.Return)) {
            commandSFX.Play();
            string userInput = terminalInput.text;
            ClearInputField();
            AddDirectoryLine(userInput);
            // if (interpreter != null) {
            //     Debug.Log("Interpreter is set.");
            // }
            int lines = AddInterpreterLines(interpreter.Interpret(userInput));
            userInputLine.transform.SetAsFirstSibling();
            terminalInput.ActivateInputField();
            terminalInput.Select();
        }
    }

    void ClearInputField() {
        terminalInput.text = "";
    }

    void AddDirectoryLine(string userInput) {
        Vector2 msgListSize = msgList.GetComponent<RectTransform>().sizeDelta;
        msgList.GetComponent<RectTransform>().sizeDelta = new Vector2(msgListSize.x, msgListSize.y + 35.0f);
        GameObject msg = Instantiate(directoryLine, msgList.transform);

        // MODIFY FOR TOP DOWN TERMINAL
        // msg.transform.SetSiblingIndex(msgList.transform.childCount - 1);
        // msg.transform.SetAsLastSibling();
        msg.transform.SetAsFirstSibling();

        msg.GetComponentsInChildren<Text>()[1].text = userInput;
    }

    int AddInterpreterLines(List<string> interpretation) {
        for (int i = interpretation.Count - 1; i >= 0; i--) {
            GameObject msg = Instantiate(responseLine, msgList.transform);
            msg.transform.SetAsFirstSibling();

            Vector2 listSize = msgList.GetComponent<RectTransform>().sizeDelta;
            msgList.GetComponent<RectTransform>().sizeDelta = new Vector2(listSize.x, listSize.y + 35.0f);

            msg.GetComponentInChildren<Text>().text = interpretation[i];
        }

        return interpretation.Count;
    }

}
