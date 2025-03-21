using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;

public class uiDebugConsole : MonoBehaviour
{
    public static uiDebugConsole instance { get; private set; }
    TMP_InputField inputField;
    [SerializeField] bool outputCommandInputs;
    List<string> previousInputs = new() { string.Empty };
    int previousInputsIndex = 0;
    VolumeProfile defaultProfile;
    string[] consoleInput;

    // commands
    Dictionary<string, Action> commands;
    public List<string> commandKeyList;
    void ping() { Debug.Log("pong"); }
    void noclip()
    {
        if (consoleInput.Length > 1)
        {
            if (float.TryParse(consoleInput[1], out float value))
            {
                uiDebug.instance.noclipSpeed = value;
            }
        }
        else { uiDebug.instance.ToggleNoclip(); }
    }
    void god() { uiDebug.instance.ToggleGod(); }
    void fps()
    {
        if (consoleInput.Length > 1)
        {
            if (int.TryParse(consoleInput[1], out int value))
            {
                Application.targetFrameRate = value;
            }
        }
        else { uiDebug.instance.ToggleFPS(); }
    }
    void vsync()
    {
        bool vSyncEnabled = QualitySettings.vSyncCount == 1;
        QualitySettings.vSyncCount = vSyncEnabled ? 0 : 1;
        uiMessage.instance.New("vSync " + (vSyncEnabled ? "disabled" : "enabled"));
    }
    void quit() { Application.Quit(); }
    void debugUpdateRate() {  }

    void loadLevel()
    {
        LevelLoader.instance.LoadLevel(new LevelLoader.levelLoadData(consoleInput[1], true));
    }
    // void interact()
    // {
    //     if (consoleInput.Length > 1)
    //     {
    //         if (Interactions.TryGetAction(consoleInput[1], out Action<GameObject> action))
    //         {
    //             action(new GameObject());
    //         }
    //         else
    //         {
    //             uiMessage.instance.New("interaction \"" + consoleInput[1] + "\" does not exist");
    //         }
    //     }
    //     else { InputMissing(); }
    // }
    void Awake()
    {
        instance = this;
        inputField = GetComponent<TMP_InputField>();
        inputField.onSubmit.AddListener((string playerInput) => Command(playerInput));
        gameObject.SetActive(false);
        //defaultProfile = Game.instance.globalVolume.profile;
    }
    public void Start()
    {
        // to add a new command, just duplicate a command below and replace the string with the 
        commands = new()
        {
            { "ping", ping },
            { "noclip", noclip },
            { "god", god }, { "godmode", god },
            { "fps", fps },
            { "vsync", vsync },
            //{ "fov", fov },
            { "quit", quit }, { "exit", quit },
            { "debugupdaterate", debugUpdateRate },
            { "level" , loadLevel }
            //{ "interact", interact },
            //{ "reset",  }, { "restart",  },
        };
        commandKeyList = commands.Keys.ToList();
        commandKeyList.Sort();
    }
    void Update()
    {
        inputField.ActivateInputField();
        PreviousInput();
        //if (Input.GetKeyDown(KeyCode.Tab)) { AutoFill(); }
    }
    void OnDisable()
    {
        previousInputs[0] = string.Empty;
        inputField.text = string.Empty;
        previousInputsIndex = 0;
    }
    void Command(string input)
    {
        previousInputs.Insert(1, input);
        string[] parsedInput = ParseInput(input);

        //typeof(uiDebugConsole).GetMethod(parsedInput[0]).Invoke(instance, null);
        
        consoleInput = parsedInput;
        if (commands.TryGetValue(parsedInput[0].ToLower(), out Action action))
        {
            action();
        }
        else { InvalidCommand(); }
        inputField.text = "";

        gameObject.SetActive(false);
        if (outputCommandInputs) { logConsoleInputs(); }
    }
    string[] ParseInput(string input)
    {
        List<string> returnInput = new() { input.Trim() };
        char[] chars = returnInput[0].ToCharArray();

        // getting indexes of the spaces in the input text
        List<int> spaceIndexes = new();
        for (int i = 0; i < chars.Length; i++) { if (chars[i] == ' ') { spaceIndexes.Add(i); } }

        // if there are no spaces then return the input in full
        if (spaceIndexes.Count == 0)
        {
            return returnInput.ToArray();
        }

        returnInput[0] = new string(chars[..spaceIndexes[0]]);

        for (int i = 0; i < spaceIndexes.Count; i++)
        {
            if (i + 1 == spaceIndexes.Count)
            {
                returnInput.Add(new string(chars[(spaceIndexes[i] + 1)..]));
            }
            else
            {
                returnInput.Add(new string(chars[(spaceIndexes[i] + 1)..spaceIndexes[i + 1]]));
            }
        }

        return returnInput.ToArray();
    }
    public void InvalidCommand(string commandOverride = "")
    {
        string commandOutput = commandOverride == "" ? consoleInput[0] : commandOverride;
        uiMessage.instance.New("Invalid Command: " + commandOutput, uiDebug.str_uiDebugConsole);
    }
    public void InvalidInput(string dataOverride = default, string commandOverride = default)
    {
        string dataOutput = dataOverride == default ? consoleInput[1] : dataOverride;
        string commandOutput = commandOverride == default ? consoleInput[0] : commandOverride;
        uiMessage.instance.New("Invalid input \"" + dataOutput + "\" for \"" + commandOutput + "\" command", uiDebug.str_uiDebugConsole);
    }
    public void InputMissing()
    {
        uiMessage.instance.New("An input must be provided for \"" + consoleInput[0] + "\" command!");
    }
    void PreviousInput()
    {
        if (previousInputsIndex == 0) { previousInputs[0] = inputField.text; }
        if (previousInputs.Count > 1)
        {
            if (Input.GetKeyDown(KeyCode.UpArrow) && previousInputsIndex < previousInputs.Count - 1)
            {
                previousInputsIndex++;
                inputField.text = previousInputs[previousInputsIndex];
                char[] inputFieldChars = inputField.text.ToCharArray();
                inputField.stringPosition = inputFieldChars.ToList().IndexOf(inputFieldChars[^1]) + 1;
            }
            if (Input.GetKeyDown(KeyCode.DownArrow) && previousInputsIndex > 0)
            {
                previousInputsIndex--;
                inputField.text = previousInputs[previousInputsIndex];
            }
        }
    }
    public void InternalCommandCall(string input)
    {
        Command(input);
    }
    void logConsoleInputs()
    {
        StringBuilder dataText = new();
        for (int i = 1; i < consoleInput.Length; i++)
        {
            dataText.Append(", \ndata").Append(i.ToString()).Append(" = \"").Append(consoleInput[i]).Append("\"");
        }
        Debug.Log("command = \"" + consoleInput[0] + "\"" + dataText);
    }
}