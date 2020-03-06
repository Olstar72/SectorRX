﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine.Video;
using DG.Tweening;

/*
Programmer: 
Additional Programmers: Kavian Kermani, Scott Watman
Description: Managing UI, loading, starting level, etc.
*/

public class UIManager : MonoBehaviour
{
    public static bool menuProperties;
    public RectTransform mainMenu, playerInputMenu, loadingScreen, logo;
    public Animator logoAnim;
    private Animator buttonAnim;
    public Button startButton;
    public VideoPlayer videoPlayer;
    [SerializeField] private MenuCamera _Camera;

    [SerializeField] private int _PlayersReady = 0;

    public AudioClip[] combatant = new AudioClip[4];
    public AudioSource announceSource;

    public void Start()
    {
        Time.timeScale = 1;
        logoAnim = logoAnim.GetComponent<Animator>();

        if (menuProperties == true)
        {
            // Set menu Animation to be done

            playerInputMenu.DOAnchorPos(new Vector2(69, -2), 0.4f);
            EventSystem.current.SetSelectedGameObject(null);
            _Camera.ToggleCamera(1);
        }
        else
        {
            StartCoroutine(StartMenu());
        }

        buttonAnim = startButton.GetComponent<Animator>();
    }

    private void Update()
    {
    }

    public void BackToMainMenu(GameObject pTarget)
    {
        logo.DOAnchorPos(new Vector2(401, 6), 0.4f);
        mainMenu.DOAnchorPos(new Vector2(44, 21), 0.4f);
        playerInputMenu.DOAnchorPos(new Vector2(69, 4120), 0.4f);
        menuProperties = false;
        EventSystem.current.SetSelectedGameObject(pTarget);
    }

    public void GoToPlayer()
    {
        logo.DOAnchorPos(new Vector2(401, -2057), 0.4f);
        playerInputMenu.DOAnchorPos(new Vector2(69, -2), 0.4f);
        mainMenu.DOAnchorPos(new Vector2(44, -2060), 0.4f);
        menuProperties = true;
        EventSystem.current.SetSelectedGameObject(null);
    }

    public void LoadLevel(int sceneIndex)
    {
        videoPlayer.Prepare();
        playerInputMenu.DOAnchorPos(new Vector2(71, -4120), 0.4f);
        loadingScreen.DOAnchorPos(new Vector2(0, 0), 0.4f);
        StartCoroutine(LoadAsyncLevel(sceneIndex));
    }

    IEnumerator LoadAsyncLevel(int sceneIndex)
    {
        yield return new WaitForSeconds(0.5f);
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneIndex);
        videoPlayer.Play();
        while (!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / 0.9f);

            if (videoPlayer.isPlaying)
            {
                videoPlayer.loopPointReached += EndReached;
                yield return new WaitForSeconds(2.0f);
            }

            yield return null;
        }
    }

    public void PlayerReadyToggle(bool pReady)
    {
        _PlayersReady += (pReady ? 1 : -1);
        
        //Announcer sound for player joining
        if(pReady)
        {
            announceSource.clip = combatant[_PlayersReady - 1];
            announceSource.Play();
        }
        
        PlayerReadyUpdateUI();
    }

    public void PlayerReadyUpdateUI()
    {
        bool canStart = _PlayersReady > 0;
        buttonAnim.SetBool("Interactable", canStart);
        startButton.interactable = canStart;
    }

    void EndReached(UnityEngine.Video.VideoPlayer videoPlayer)
    {
        StartCoroutine(CompleteLoadVisual());  
    }

    IEnumerator CompleteLoadVisual()
    {
        yield return new WaitForSeconds(0.75f);
        videoPlayer.isLooping = false;
    }

    IEnumerator StartMenu()
    {
        yield return new WaitForSeconds(7.5f);
        logoAnim.enabled = false;
        mainMenu.DOAnchorPos(new Vector2(44, 21), 0.4f);
    }
 }
