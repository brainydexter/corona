using UnityEngine;
using System;


public class GameManager : Singleton<GameManager>, ISubscriberEvents
{
    override protected void Awake()
    {
        base.Awake();

        if (deleteSavedData)
        {
            PlayerPrefs.DeleteAll();
        }

        Instance.eventAggregator = new EventAggregator();

        //levelManager = new LevelManager();
        pauseComponent = new PauseGameComponent();

        //m_analyticsManager = new AnalyticsManager(Instance.eventAggregator);
        //m_analyticsManager.RegisterEventHandlers();
    }

    protected void Start()
    {
        RegisterEventHandlers();

        //if (deleteSavedData)
        //{
        //    UpsellAdMgr.Instance.SetPaidStatusNothing();
        //}

        //LevelManager.Start(); // calling this here, since BoardCollection would have been initialized by now

        //Instance.levelManager.Initialize();
    }

    protected new void OnDestroy()
    {
        //m_analyticsManager.UnRegisterEventHandlers();
        //m_analyticsManager = null;

        pauseComponent = null;
        //levelManager = null;

        UnRegisterEventHandlers();

        eventAggregator = null;

        base.OnDestroy();
    }

    protected GameManager()
    {
    }

    protected void Update()
    {
        EventAggregator.Update();
    }

    #region Event Handlers

    public void RegisterEventHandlers()
    {

    }

    public void UnRegisterEventHandlers()
    {

    }

    #endregion

    #region public properties
    public IEventAggregator EventAggregator { get { return Instance.eventAggregator; } }

    //public LevelManager LevelManager { get { return Instance.levelManager; } }

    public PauseGameComponent PauseComponent { get { return Instance.pauseComponent; } }

    //public AnalyticsManager AnalyticsManager { get { return Instance.m_analyticsManager; } }
    #endregion

    #region private members
    private IEventAggregator eventAggregator;

    //private LevelManager levelManager;

    private PauseGameComponent pauseComponent = null;

    [SerializeField] private bool deleteSavedData = false;

    //private AnalyticsManager m_analyticsManager = null;
    #endregion
}
