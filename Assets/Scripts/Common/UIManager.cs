using UnityEngine;
using UnityEngine.SceneManagement;

using System.Collections;
using System.Collections.Generic;

public class UIManager : Singleton<UIManager>
{
    #region Public Methods
    /// <summary>
    /// Only the specified scene is loaded in memory by the end of this function
    /// </summary>
    /// <param name="sceneName"></param>
    public void LoadScene(string sceneName)
    {
        m_operations.Enqueue(new UIOperation(UIOperation.UIAction.LoadScene, sceneName));
    }

    /// <summary>
    /// Loads the given scene on top of the existing scene(s)
    /// </summary>
    /// <param name="popupSceneName"></param>
    public void LoadPopup(string popupSceneName)
    {
        var activeSceneName = SceneManager.GetActiveScene();

        //Debug.Assert (popupSceneName != activeSceneName, "Loading Popup scene is the same as the active scene: " + popupSceneName);

#if UNITY_EDITOR
        //Debug.Log("[UIManager]: Loading scene: " + popupSceneName + " on top of scene call stack " + activeSceneName.name);
#endif
        m_operations.Enqueue(new UIOperation(UIOperation.UIAction.ShowPopup, popupSceneName));
    }

    /// <summary>
    /// Akin to closing the popup and making underlying scene active
    /// </summary>
    /// <param name="popupSceneName"></param>
    public void UnLoadPopup(string popupSceneName)
    {
        var activeScenePath = SceneManager.GetActiveScene().path;

        Debug.Assert(activeScenePath.Contains(popupSceneName), " Current scene is not the popup you wish to unload. Corruption happened");
        Debug.Assert(m_sceneCallStack.Count != 0, "Trying to pop out from empty stack");

        m_operations.Enqueue(new UIOperation(UIOperation.UIAction.HidePopup, popupSceneName));
    }

    #endregion

    #region Coroutine handlers
    /// <summary>
    /// Coroutine which removes all the other scenes and loads the given scene name
    /// </summary>
    /// <param name="operation"></param>
    /// <returns></returns>
    private IEnumerator LoadSingleSceneAsync(UIOperation operation)
    {
        BeginSafetyOperation();

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(operation.SceneName, LoadSceneMode.Single);

        while (!asyncLoad.isDone)
        {
            yield return null;
        }

        m_sceneCallStack.Clear();
        operation.Complete();

        GameManager.Instance.EventAggregator.Publish(SceneChangeEvent.Build(operation.SceneName));

        EndSafetyOperation();
    }

    /// <summary>
    /// Coroutine which stacks the given scene name on top of existing scene.
    /// Tracks existing scene name in stack such that it can be set active later on when given scene is popped.
    /// Example: In BoardScene (BS), Pause Button is clicked which will trigger PauseScene (PS) to be stacked on top
    /// - In stack, BS will be pushed. => Stack (BS)
    /// - PS will be marked as the active scene => Active Scene (PS)
    /// In Pause Menu, if now Level select is clicked, LevelSelectScene (LS) needs to be stacked on top
    /// - In stack, PS will be pushed. => Stack (PS, BS) (PS is top) 
    /// - LS will be marked as the active scene => Active Scene (LS)
    /// </summary>
    /// <param name="uiOperation"></param>
    /// <returns></returns>
    private IEnumerator LoadAdditiveSceneAsync(UIOperation uiOperation)
    {
        BeginSafetyOperation();

        m_sceneCallStack.Push(SceneManager.GetActiveScene().path);

        var popupSceneName = uiOperation.SceneName;

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(popupSceneName, LoadSceneMode.Additive);

        while (!asyncLoad.isDone)
        {
            yield return null;
        }

        // explicitly need to set this active scene since it doesnt happen automatically in additive load

        var popupScene = SceneManager.GetSceneByName(popupSceneName);

        while (!popupScene.isLoaded)
        {
            yield return null;
        }

        yield return StartCoroutine(SetActiveScene(popupScene));

        uiOperation.Complete();

        GameManager.Instance.EventAggregator.Publish(SceneChangeEvent.Build(popupSceneName));

        EndSafetyOperation();
    }

    /// <summary>
    /// Coroutine which pops the top of the stack and 
    /// expects top of the stack to be same as uiOperation.SceneName
    /// It also pops from the stack and after popping, if there are two scenes in memory,
    /// it disambiguates by marking the correct base scene as the active scene.
    /// </summary>
    /// <param name="uiOperation"></param>
    /// <returns></returns>
    private IEnumerator UnloadAdditiveSceneAsync(UIOperation uiOperation)
    {
        BeginSafetyOperation();

        var popupSceneName = uiOperation.SceneName;
        AsyncOperation asyncLoad = SceneManager.UnloadSceneAsync(popupSceneName);

        while (!asyncLoad.isDone)
        {
            yield return null;
        }

#if UNITY_EDITOR
        //Debug.Log("Active scene before: " + SceneManager.GetActiveScene().name);
#endif
        string baseScenePath = m_sceneCallStack.Pop();

        // only need to set scene active, if there were >= 2 scenes
        // if after pop, stack is empty => now there is only 1 active scene
        // we dont need to set the only scene as active scene
        if (m_sceneCallStack.Count != 0)
        {
            var baseScene = SceneManager.GetSceneByPath(baseScenePath);
            yield return StartCoroutine(SetActiveScene(baseScene));
        }

        uiOperation.Complete();

        GameManager.Instance.EventAggregator.Publish(SceneChangeEvent.Build(SceneManager.GetSceneByPath(baseScenePath).name));

        EndSafetyOperation();
    }

    /// <summary>
    /// Coroutine which sets the scene to be active scene
    /// </summary>
    /// <param name="scene"></param>
    /// <returns></returns>
    private IEnumerator SetActiveScene(Scene scene)
    {
        while (!SceneManager.SetActiveScene(scene))
        {
            yield return null;
        }

#if UNITY_EDITOR
        //Debug.Log("Active scene is now: " + SceneManager.GetActiveScene().name);
#endif
    }

    /// <summary>
    /// Encapsulates the ui operation and triggers the appropriate coroutine which 
    /// </summary>
    /// <param name="operation"></param>
    /// <returns></returns>
    private IEnumerator HandleUIOperation(UIOperation operation)
    {
        IEnumerator uiOperation = null;
        switch (operation.Action)
        {
            case UIOperation.UIAction.LoadScene:
                uiOperation = LoadSingleSceneAsync(operation);
                break;

            case UIOperation.UIAction.ShowPopup:
                uiOperation = LoadAdditiveSceneAsync(operation);
                break;

            case UIOperation.UIAction.HidePopup:
                uiOperation = UnloadAdditiveSceneAsync(operation);
                break;

            default:
                Debug.Assert(false, "Not handled given action: " + operation.Action);
                break;
        }

        operation.Start();
        yield return StartCoroutine(uiOperation);
    }

    #endregion

    #region Mono Methods

    void Start()
    {
        LoadScene(Constants.Scenes.TitleScene);
    }

    private void Update()
    {
        if (m_operations.Count != 0)
        {
            var operation = m_operations.Peek();
            switch (operation.OperationState)
            {
                case UIOperation.State.Completed:
                    {
                        m_operations.Dequeue();
                        break;
                    }
                case UIOperation.State.NotStarted:
                    {
                        StartCoroutine(HandleUIOperation(operation));
                        break;
                    }
                case UIOperation.State.InProgress:
                    {
                        break;
                    }
                default:
                    Debug.Assert(false, "Not handled given state: " + operation.OperationState);
                    break;
            }
        }
    }

    protected override void Awake()
    {
        base.Awake();

        m_sceneCallStack = new Stack<string>();
        m_operations = new Queue<UIOperation>();

#if UNITY_EDITOR
        m_opInProgres = false;
#endif
    }
    #endregion

    #region Members
    Stack<string> m_sceneCallStack;
    Queue<UIOperation> m_operations; // time ordered UI requested operations
    #endregion

    #region Safety Helpers (EDITOR only)

#if UNITY_EDITOR
    bool m_opInProgres = false;
#endif

    /// <summary>
    /// Marks an operation is in progress
    /// valid only in editor
    /// </summary>
    void BeginSafetyOperation()
    {
#if UNITY_EDITOR
        Debug.Assert(!m_opInProgres, "another operation in progress");
        m_opInProgres = true;
#endif
    }

    /// <summary>
    /// Marks an operation completed
    /// valid only in editor
    /// </summary>
    void EndSafetyOperation()
    {
#if UNITY_EDITOR
        m_opInProgres = false;
#endif
    }

    #endregion

    sealed class UIOperation
    {
        internal enum State
        {
            NotStarted,
            InProgress,
            Completed
        }

        internal enum UIAction
        {
            LoadScene,
            ShowPopup,
            HidePopup
        }

        internal UIAction Action { get; private set; }
        internal State OperationState { get; private set; }
        internal string SceneName { get; private set; }

        internal UIOperation(UIAction action, string sceneName)
        {
            Action = action;
            SceneName = sceneName;

            OperationState = State.NotStarted;
        }

        internal void Start()
        {
            Debug.Assert(OperationState == State.NotStarted, "Trying to start an already started operation");
            OperationState = State.InProgress;
        }

        internal void Complete()
        {
            Debug.Assert(OperationState == State.InProgress, "Trying to complete a operation which is not in progress: " + OperationState);
            OperationState = State.Completed;
        }
    }
}

public sealed class SceneChangeEvent : IEventType
{
    public SceneChangeEvent() { }

    public string SceneName { get; private set; }

    public static SceneChangeEvent Build(string sceneName)
    {
        var sceneChangeEvent = EventBuilder.Instance.Spawn<SceneChangeEvent>();
        sceneChangeEvent.SceneName = sceneName;

        return sceneChangeEvent;
    }

    public override void Reset()
    {
        this.SceneName = null;
    }
}