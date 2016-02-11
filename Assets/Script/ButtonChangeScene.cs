using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class ButtonChangeScene : MonoBehaviour
{
    public void ChangeSceneButton(int index)
    {
        SceneManager.LoadScene(index);
    }

    public void ChangeSceneButton(string levelName)
    {
        SceneManager.LoadScene(levelName);
    }
}