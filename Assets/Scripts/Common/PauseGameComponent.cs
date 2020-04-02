using UnityEngine;
using System.Collections;

public class PauseGameComponent
{
    /// <summary>
    /// Pause the game only if it is not currently paused
    /// </summary>
    public void Pause()
    {

        // pause only if game is not paused
        if (Time.timeScale != 0f)
        {
            gameTimeScale = Time.timeScale;

            Time.timeScale = 0f;

            //			Debug.Log ("[PauseGameComponent]: Pausing Game");
        }
    }

    /// <summary>
    /// Unpause the game only if it is currently paused
    /// </summary>
    public void Unpause()
    {

        // unpause only if game is paused
        if (Time.timeScale == 0f)
        {
            Time.timeScale = this.gameTimeScale;

            gameTimeScale = 0f;

            //			Debug.Log ("[PauseGameComponent]: Unpausing Game");
        }
    }

    /// <summary>
    /// Indicates whether the game is paused or not.
    /// </summary>
    /// <value><c>true</c> if this instance is game paused; otherwise, <c>false</c>.</value>
    public bool IsGamePaused { get { return Time.timeScale == 0f; } }

    private float gameTimeScale = 0f;
}