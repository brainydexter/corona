using UnityEngine;
using System.Collections;

public class DestroyOnBoardOverCleanup : MonoBehaviour, ISubscriberEvents
{

    #region Mono Methods

    void Awake()
    {
        RegisterEventHandlers();
    }

    void OnDestroy()
    {
        UnRegisterEventHandlers();
    }
    #endregion

    #region ISubscriberEvents implementation

    public void RegisterEventHandlers()
    {
        //GameManager.Instance.EventAggregator.Register<BoardOverCleanup>(HandleBoardOverCleanup);
    }

    public void UnRegisterEventHandlers()
    {
        //GameManager.Instance.EventAggregator.UnRegister<BoardOverCleanup>(HandleBoardOverCleanup);
    }

    void HandleBoardOverCleanup(IEventType obj)
    {
        Destroy(gameObject);
    }

    #endregion
}
