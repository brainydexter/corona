using UnityEngine;
using System.Collections;

public class RunToCompletion : MonoBehaviour
{

    public void DisableGameObject()
    {
        gameObject.SetActive(false);
    }

    public void DisableParentGameObject()
    {
        gameObject.transform.parent.gameObject.SetActive(false);
    }

    #region Mono Methods

    void FixedUpdate()
    {
        if (runTimer)
        {

            dt += Time.fixedDeltaTime;

            if (dt >= duration)
            {
                dt = 0f;
                DisableGameObject();
            }
        }
    }

    void OnEnable()
    {
        dt = 0f;
    }

    void OnDisable()
    {
        dt = 0f;
    }

    #endregion

    #region Members

    [SerializeField]
    private bool runTimer = false;

    [SerializeField]
    private float duration = 0f;

    private float dt = 0f;

    #endregion
}
