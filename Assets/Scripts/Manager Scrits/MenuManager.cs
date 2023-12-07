using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class MenuManager : GenericSingleton<MenuManager> {
    [SerializeField] private GameObject playButton;

    public override void Awake()
    {
        base.Awake();
    }
    // Start is called before the first frame update
    void Start()
    {
        Pause();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Pause()
    {
        Time.timeScale = 0f;
    }

    public void Play()
    {
        playButton.SetActive(false);
        Time.timeScale = 1f;
    }
    public void GameOver()
    {
        //añadir evento y pantalla de gameOver, reiniciar todo y ya
        playButton.SetActive(true);

        Pause();
    }
}
