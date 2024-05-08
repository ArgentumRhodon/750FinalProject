using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public Vector2 cameraDimensions;
    public Vector2 cameraWorldBounds;
    public bool hasWon = false;
    public bool hasLost = false;

    public GameObject winUI;
    public GameObject loseUI;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }

        cameraDimensions = new Vector2(Screen.width, Screen.height);
        cameraWorldBounds = Camera.main.ScreenToWorldPoint(cameraDimensions);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        winUI.SetActive(hasWon);
        loseUI.SetActive(hasLost);

        if(hasLost || hasWon)
        {
            Time.timeScale = 0.0f;
        }
    }
}
