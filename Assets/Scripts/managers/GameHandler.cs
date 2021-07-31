using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameHandler : MonoBehaviour
{
    public void ReloadScene()
    {
        StartCoroutine(WaitAndReload());
    }
    IEnumerator WaitAndReload()
    {
        yield return new WaitForSeconds(0.5f);
        Scene actcene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(actcene.name);
    }
    public void ExitGame()
    {
        Application.Quit();
    }
}
