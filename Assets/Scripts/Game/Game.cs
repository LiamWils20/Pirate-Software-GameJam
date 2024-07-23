using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class Game : MonoBehaviour
{
    public enum Directions
    {
        north,
        northEast,
        east,
        southEast,
        south,
        southWest,
        west,
        northWest,
    }
    public LayerMask defaultLayer;
    public static Game instance { get; private set; }
    public System.Random random = new();
    public double clock;
    public bool inGame, paused;
    public Volume globalVolume;
    public FilmGrain filmGrain;
    public Vignette vignette;
    public int targetFramerate = 120;
    void Awake()
    {
        instance = this;
        globalVolume = GetComponent<Volume>();
        globalVolume.profile.TryGet(out filmGrain);
        globalVolume.profile.TryGet(out vignette);
        Application.targetFrameRate = targetFramerate;
    }
    void Start()
    {
        //Pause(true);
    }
    void Update()
    {
        if (inGame) 
        { 
            clock += Time.deltaTime;
        }
    }
    public void Pause(bool state = false)
    {
        paused = state;
        Time.timeScale = state ? 0 : 1;
        Cursor.lockState = state ? CursorLockMode.None : CursorLockMode.Locked;
        Cursor.visible = state;
        InputHandler.instance.SetActive(!state);
    }
}