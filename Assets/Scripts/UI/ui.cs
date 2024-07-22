using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ui : MonoBehaviour
{
    public static ui instance { get; private set; }

    public bool
        uiFadeToBlack;

    [SerializeField] TextMeshProUGUI
        uiLevelNum;

    [SerializeField] Image
        uiFade;

    [SerializeField] float
        uiFadeInSpeed,
        uiFadeOutSpeed;

    public float uiFadeAlpha;

    private const string
        uiLevelNumText = "Level ";

    void Awake()
    {
        instance = this;
        LevelLoader.instance.levelLoaded.AddListener(Refresh);
    }

    void Start()
    {

    }

    void Update()
    {
        uiFadeUpdate();
    }

    /// <summary>
    /// Updates all ui elements currently needed
    /// </summary>
    void Refresh()
    {

    }
    /// <summary>
    /// Updates the color of the ui fade element on screen used to hide the screen, uiFadeToBlack controls the direction of the fade
    /// </summary>
    void uiFadeUpdate()
    {
        uiFade.color = new Color(0.0f, 0.0f, 0.0f,
            Mathf.Clamp(
                uiFade.color.a + (uiFadeToBlack ? Time.deltaTime * uiFadeInSpeed : -Time.deltaTime * uiFadeOutSpeed),
                0f, 1f));
        uiFadeAlpha = uiFade.color.a;
    }
}