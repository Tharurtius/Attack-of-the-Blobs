using System.Collections;
using System.Collections.Generic;
using UnityEngine; //Connect to unity engine
using UnityEngine.XR;

public class GameManager : MonoBehaviour
{
    #region Variables
    //Public enum storing our game states
    public enum GameStates
    {
        Menu,
        InGame,
        Paused,
        PostGame
    }
    [Header("Game Management")]
    [Tooltip("Allows us to change state to test different settings are working between states")]
    public GameStates state;
    [Header("References")]
    [Tooltip("Add the Menu child of the pause object here")]
    public GameObject pauseMenu;
    [Tooltip("Add the menu child of the end object here")]
    public GameObject endMenu;
    [Tooltip("Helps keep track of player lives")]
    public GameObject livesCounter;
    [Tooltip("Actual sprite for lives counter")]
    public GameObject livesSprite;
    #endregion
    #region Setup
    private void Start()
    {
        //If our references aren't connected find them in scene
        if (pauseMenu == null) pauseMenu = GameObject.Find("Pause").transform.GetChild(0).gameObject;
        if (endMenu == null) endMenu = GameObject.Find("End").transform.GetChild(0).gameObject;
        //Set state to ingame at start and run ChangeState function
        state = GameStates.InGame;
        ChangeState(state);
        //set framerate
        Application.targetFrameRate = 60;
    }
    public void ChangeState(GameStates gameState)
    {
        //Adjust cursor and menus based on selected state
        switch (gameState)
        {
            case GameStates.Menu:
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                break;
            case GameStates.InGame:
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
                pauseMenu.SetActive(false);
                endMenu.SetActive(false);
                break;
            case GameStates.Paused:
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true; 
                pauseMenu.SetActive(true);
                endMenu.SetActive(false);
                break;
            case GameStates.PostGame:
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                pauseMenu.SetActive(false);
                endMenu.SetActive(true);
                break;
            default:
                break;
        }
    }
    #endregion
    #region Functions
    /// <summary>
    /// Call this when you lose a life to remove a life from the life counter
    /// </summary>
    public void LoseLife()
    {
        if (livesCounter.transform.childCount > 0)//if there are lives left
        {
            Destroy(livesCounter.transform.GetChild(0).gameObject);
        }
        else
        {
            Debug.Log("Error no more lives to lose!");
        }
    }

    /// <summary>
    /// Does various stuff to end the game
    /// </summary>
    public void EndGame()
    {
        ChangeState(GameStates.PostGame);
    }
    #endregion
}
