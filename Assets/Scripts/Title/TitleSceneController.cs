using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleSceneController : MonoBehaviour
{
    public void PlayCurrentGame()
    {
        UIManager.Instance.LoadScene(Constants.Scenes.GameplayScene);
    }
}
