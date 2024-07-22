using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class uiDebug : MonoBehaviour
{
    public static uiDebug instance { get; private set; }
    [SerializeField] TextMeshProUGUI
        uiFPSText,
        uiRes,
        //uiStats,
        uiStatsPlayer,
        uiStatsMiscellaneous,
        uiStatsLevel,
        uiVersion;
    [SerializeField] GameObject 
        uiDebugGroup,
        uiFPS;
    public bool debugMode { get; private set; }
    public bool debugLines { get; private set; }
    public bool uiFPSEnabled;
    public float noclipSpeed = 10f;
    private int FPS;
    [Tooltip("Lower is faster")] public float statsRepeatRate = 0.02f;
    bool noclipEnabled, godEnabled;

    public const string
        // player
        str_playerTitle = "<u>Player/Game;</u>",
        str_targetFPS = "\ntargetFPS = ",
        str_vSync = ", vSync = ",
        str_mouseRotation = "\nmouseRotation = ",
        str_lookSensitivity = "\nlookSensitivity = ",
        str_multiply = " * ",
        str_notApplicable = "n/a",
        // level
        str_levelTitle = "<u>Level;</u>\ninLevel = ",
        str_assetKey = "\nassetKey = ",
        str_inGameName = "\ninGameName = ",
        str_playerStartPos = "\nplayerStartPos = ",
        //other
        str_x = "x",
        str_v = "v",
        str_dash = " - ",
        str_equals = " = ",
        str_divide = " / ",
        str_newLine = "\n",
        str_newLineDash = "\n - ",
        str_hz = "Hz",
        str_resFormat = "{0}:{1}",
        str_noclip = "noclip ",
        str_god = "god mode ",
        str_enabled = "enabled",
        str_disabled = "disabled",
        // class names
        str_uiDebug = "uiDebug",
        str_uiDebugConsole = "uiDebugConsole",
        str_level = "Level",
        str_levelLoader = "LevelLoader";

    void Awake()
    { 
        instance = this;
        uiVersion.text = str_v + Application.version;
        uiFPSText = uiFPS.GetComponent<TextMeshProUGUI>();
        #if UNITY_EDITOR
        debugMode = true;
        #endif
    }
    void Start()
    {
        StartRepeating();
        InvokeRepeating(nameof(GetFPS), 0f, statsRepeatRate);
    }
    void StartRepeating()
    {
        InvokeRepeating(nameof(GetRes), 0f, 1f);
        InvokeRepeating(nameof(GetStats), 0f, statsRepeatRate);
    }
    void StopRepeating()
    {
        CancelInvoke(nameof(GetRes));
        CancelInvoke(nameof(GetStats));
    }
    public void RefreshRepeating()
    {
        CancelInvoke();
        StartRepeating();
        InvokeRepeating(nameof(GetFPS), 0f, statsRepeatRate);
    }
    void Update()
    {
        FPS = Mathf.FloorToInt(1.0f / Time.unscaledDeltaTime);
        if (Input.GetKeyDown(KeyCode.F3)) 
        {
            debugMode = !debugMode; 
            if (debugMode) { StartRepeating(); }
            else { StopRepeating();  }
            if (!uiFPSEnabled) uiFPS.SetActive(debugMode);
        }
        if (Input.GetKeyDown(KeyCode.F4)) { debugLines = debugMode && !debugLines; }
        uiDebugGroup.SetActive(debugMode);
        if (debugMode) { Controls(); }
        if (noclipEnabled) { Noclip(); }
    }
    void GetRes() // gets the current resolution, refresh rate and aspect ratio
    {
        float gcd = Extensions.CalcGCD(Screen.width, Screen.height);
        uiRes.text = Screen.width.ToString() + str_x + Screen.height.ToString() + str_newLine
            + Screen.currentResolution.refreshRateRatio + str_hz + str_newLine +
            (string.Format(str_resFormat, Screen.width / gcd, Screen.height / gcd));
    }
    void GetFPS() // fps counter
    {
        uiFPSText.text = FPS.ToString();
    }
    void GetStats() // contructs all stats for the debug overlay, uses stringbuilder & append to slightly improve performance
    {
        if (!debugMode)
        {
            uiStatsPlayer.text = string.Empty;
            uiStatsLevel.text = string.Empty;
            uiStatsMiscellaneous.text = string.Empty;
            return;
        }
        uiStatsPlayer.text = Player.instance.debugGetStats().ToString();
        uiStatsLevel.text = new StringBuilder(str_levelTitle)
            .Append(LevelLoader.instance.inLevel)
            .Append(LevelLoader.instance.levelCurrent is not null 
                 ? LevelLoader.instance.levelCurrent.debugGetStats().ToString() : string.Empty).ToString();
        uiStatsMiscellaneous.text = new StringBuilder("<u>Miscellaneous;</u>")
            .Append("\nuiFadeAlpha = ").Append(ui.instance.uiFadeAlpha).ToString();
    }
    void Controls() // allows for WASD movement control and scroll to change the grapple distance
    {
        // scroll to change hook distance
        //float scrollY = Input.mouseScrollDelta.y;
        //if (scrollY != 0) { Grapple.instance.debugMaxDistanceEdit(scrollY); }

        // toggles debug console
        if (Input.GetKeyDown(KeyCode.BackQuote) || Input.GetKeyDown(KeyCode.Tilde)) 
        {
            uiDebugConsole.instance.gameObject.SetActive(!uiDebugConsole.instance.gameObject.activeSelf);
        }
    }
    void Noclip()
    {
        // wasd movement
        //if (uiDebugConsole.instance.gameObject.activeSelf) { return; }
        //Player.instance.transform.position += noclipSpeed * Time.deltaTime * Player.instance.transform.TransformDirection(new(Extensions.FloatFromAxis(Input.GetKey(KeyCode.D), Input.GetKey(KeyCode.A)), 0, Extensions.FloatFromAxis(Input.GetKey(KeyCode.W), Input.GetKey(KeyCode.S))));
    }
    public void ToggleNoclip()
    {
        noclipEnabled = !noclipEnabled;
        uiMessage.instance.New(str_noclip + (noclipEnabled ? str_enabled : str_disabled), str_uiDebug); 
    }
    public void ToggleGod()
    {
        godEnabled = !godEnabled;
        uiMessage.instance.New(str_god + (godEnabled ? str_enabled : str_disabled), str_uiDebug);
    }
    public void ToggleFPS()
    {
        uiFPSEnabled = !uiFPSEnabled;
        if (!uiFPSEnabled && debugMode) { return; }
        uiFPS.SetActive(uiFPSEnabled);
    }
}