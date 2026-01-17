using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class StartGame : MonoBehaviour
{
    public void LoadingNewScene()
    {
        SceneManager.LoadScene("Main");
    }
}
