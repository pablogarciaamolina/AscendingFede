using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : GenericSingleton<MenuManager>
{
    // make it generic singleton
    [SerializeField] private GameObject playButton;
    [SerializeField] private GameObject fede;
    [SerializeField] private GameObject gameoverImage;
    private float health = 1000;

    // Start is called before the first frame update
    public override void Awake()
    {
        base.Awake();
        fede.GetComponent<FedeHealth>().healthUpdate += updateCurrentHealth;
        gameoverImage.SetActive(false);
    }
    void Start()
    {
        Pause();
    }

    // Update is called once per frame
    void Update()
    {
        CheckHealth();
    }

    private void updateCurrentHealth(float h)
    {
        health = h;
    }

    private void Pause()
    {
        Time.timeScale = 0f;
        playButton.SetActive(true);
    }

    public void Play()
    { 
        Time.timeScale = 1.0f;
        playButton.SetActive(false);
    }

    private void CheckHealth()
    {
        if (health <= 0 || fede.transform.position.y <= -10)
        {
            GameOver();
        }
    }

    private void GameOver()
    {
        gameoverImage.SetActive(true);
        Pause();
    }
}
