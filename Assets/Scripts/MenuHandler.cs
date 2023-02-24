using UnityEngine; //Connect to Unity Engine
using UnityEngine.SceneManagement; //Use scenemanagement to change scenes

public class MenuHandler : MonoBehaviour
{
    #region Variables
    [Header("References")]
    [Tooltip("Add the gamemanager in here")]
    public GameManager gameManager;
    #endregion
    #region Setup & Functions
    private void Start()
    {
        //Get references if they aren't connected
        if (gameManager == null && SceneManager.GetActiveScene().buildIndex == 1) gameManager = GameObject.Find("GameManager").GetComponent<GameManager>(); 
    }
    //Change scene based on scene index
    public void ChangeScene(int scene)
    {
        SceneManager.LoadScene(scene);
    }
    //Quit game or exit play mode based on if we are in Editor or build
    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
        Application.Quit();
    }
    public void Unpause()
    {
        //Change state to ingame and run ChangeState
        gameManager.state = GameManager.GameStates.InGame;
        gameManager.ChangeState(gameManager.state);
    }
    #endregion
}
