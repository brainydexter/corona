//using UnityEngine;

//using System.Collections;
//using System.Collections.Generic;
//using System;

//public class AnimationManager : Singleton<AnimationManager>, ISubscriberEvents
//{

//    override protected void Awake()
//    {
//        base.Awake();

//        powerupsPool = new Dictionary<PowerupType, GameObjectPool>(prefabPowerupGameObjects.Length);

//        for (int i = 0; i < prefabPowerupGameObjects.Length; i++)
//        {

//            //			 insert animator into dictionary
//            if (!powerupsPool.ContainsKey(prefabPowerupGameObjects[i].PowerupType))
//            {
//                GameObjectPool pool = new GameObjectPool(prefabPowerupGameObjects[i].Prefab, Instance.transform);
//                powerupsPool.Add(prefabPowerupGameObjects[i].PowerupType, pool);
//            }
//            else
//            {
//                Debug.LogError("Tried to insert different animation gameobject for the same key: " + prefabPowerupGameObjects[i].evnt);
//            }
//        }

//        // marking refs to null for prefabs so they can be reclaimed
//        for (int i = 0; i < prefabPowerupGameObjects.Length; i++)
//        {
//            prefabPowerupGameObjects[i].Prefab = null;
//            prefabPowerupGameObjects[i] = null;
//        }
//        prefabPowerupGameObjects = null;

//        letterDissapearPool = new GameObjectPool(letterDissapearFxPrefab, Instance.transform);

//        // releasing ref to prefab
//        letterDissapearFxPrefab = null;

//        zeroLetterFxPool = new GameObjectPool(zeroLetterMultiplierFxPrefab, Instance.transform);
//        zeroWordFxPool = new GameObjectPool(zeroWordMultiplierFxPrefab, Instance.transform);
//        multiplierFxPool = new GameObjectPool(multiplierFxPrefab, Instance.transform);

//        starEarnedFx = Instantiate<GameObject>(starEarnedFx);
//        starEarnedFx.transform.SetParent(Instance.transform);
//        starEarnedFx.SetActive(false);

//        zeroLetterMultiplierFxPrefab = null;
//        zeroWordMultiplierFxPrefab = null;
//        multiplierFxPrefab = null;
//    }

//    // Use this for initialization
//    void Start()
//    {
//        //        Debug.Log ("starting animation system and registering event handlers");

//        this.RegisterEventHandlers();
//    }

//    protected new void OnDestroy()
//    {
//        //		Debug.Log ("Stopping animation system and unregistering event handlers");

//        powerupsPool.Clear(); powerupsPool = null;
//        letterDissapearPool.Clear(); letterDissapearPool = null;
//        zeroLetterFxPool.Clear(); zeroLetterFxPool = null;
//        zeroWordFxPool.Clear(); zeroWordFxPool = null;
//        multiplierFxPool.Clear(); multiplierFxPool = null;

//        this.UnRegisterEventHandlers();

//        base.OnDestroy();
//    }

//    protected void PlayAnimation(PowerupGenericEvent powerupEvnt)
//    {
//        //		Debug.Log ("trying to play animations for " + powerupEvnt.PowerupType);

//        GameObjectPool pool = null;

//        if (!powerupsPool.TryGetValue(powerupEvnt.PowerupType, out pool))
//        {
//            Debug.LogWarning("No associated animation found for event: " + powerupEvnt.PowerupType);
//        }

//        if (pool != null)
//        {
//            // using localPosition since the associated gameobject has the Z value and this affects the local position
//            var powerup = pool.Spawn();
//            powerup.transform.localPosition = (Vector3.right * powerupEvnt.Position.x) + (Vector3.up * powerupEvnt.Position.y);
//        }

//    }

//    #region implementation

//    public void RegisterEventHandlers()
//    {
//        GameManager.Instance.EventAggregator.Register<ValidWord>(Instance.HandleValidWord);
//        GameManager.Instance.EventAggregator.Register<PowerupGenericEvent>(Instance.HandleGenericPowerup);
//        GameManager.Instance.EventAggregator.Register<StarEarnedEvent>(HandleStarsEarnedEvent);
//        GameManager.Instance.EventAggregator.Register<LetterPoppedEvent>(HandleLetterPoppedEvent);
//    }

//    public void UnRegisterEventHandlers()
//    {
//        GameManager.Instance.EventAggregator.UnRegister<ValidWord>(Instance.HandleValidWord);
//        GameManager.Instance.EventAggregator.UnRegister<PowerupGenericEvent>(this.HandleGenericPowerup);
//        GameManager.Instance.EventAggregator.UnRegister<StarEarnedEvent>(HandleStarsEarnedEvent);
//        GameManager.Instance.EventAggregator.UnRegister<LetterPoppedEvent>(HandleLetterPoppedEvent);
//    }

//    #endregion

//    void HandleGenericPowerup(IEventType evnt)
//    {
//        PlayAnimation(evnt as PowerupGenericEvent);
//    }

//    private void HandleLetterPoppedEvent(IEventType evnt)
//    {
//        var letterPoppedEvent = evnt as LetterPoppedEvent;
//        var pool = powerupsPool[PowerupType.POPCORN];
//        var powerup = pool.Spawn();
//        powerup.transform.localPosition = (Vector3.right * letterPoppedEvent.X) + (Vector3.up * letterPoppedEvent.Y);
//    }

//    #region valid word animation
//    void HandleValidWord(IEventType evnt)
//    {
//        ValidWord validWordEvent = evnt as ValidWord;

//        Debug.Assert(validWordEvent != null, "ValidWordevent cannot be null");

//        // play the 0 multiplier FX
//        if (validWordEvent.Has0Multiplier)
//        {

//            if (validWordEvent.Mul0LetterIndex != -1)
//            {
//                var fx = zeroLetterFxPool.Spawn();
//                // position the prefab
//                fx.transform.position = (validWordEvent.letterPositions[validWordEvent.Mul0LetterIndex].Position + (Vector3.forward * -2f));
//                if (fx.transform.position.y == 0)
//                    fx.transform.position += Vector3.up;
//            }

//            if (validWordEvent.Mul0WordIndex != -1)
//            {
//                var fx = zeroWordFxPool.Spawn();
//                // position the prefab
//                fx.transform.position = (validWordEvent.letterPositions[validWordEvent.Mul0WordIndex].Position + (Vector3.forward * -2f));
//                if (fx.transform.position.y == 0)
//                    fx.transform.position += Vector3.up;
//            }
//        }
//        else
//        {
//            List<MultiplierPlayedFX> multiplierFxs = new List<MultiplierPlayedFX>();
//            for (int i = 0; i < validWordEvent.Word.Length; i++)
//            {

//                // if there is no multiplier at this position
//                if (validWordEvent.LetterMultipliers[i] == 1 && validWordEvent.WordMultipliers[i] == 1)
//                {
//                    GameObject validWordFX = letterDissapearPool.Spawn();

//                    if (validWordFX)
//                    {
//                        validWordFX.transform.position = (validWordEvent.letterPositions[i].Position + (Vector3.forward * -2f));
//                        validWordFX.SetActive(true);
//                    }
//                }
//                else
//                { // there is a multiplier at i

//                    var letterPosition = (validWordEvent.letterPositions[i].Position + (Vector3.forward * -2f));
//                    var fx = multiplierFxPool.Spawn(true).GetComponent<MultiplierPlayedFX>();

//                    bool isWordMultiplier = validWordEvent.WordMultipliers[i] != 1;
//                    int factor = isWordMultiplier ? validWordEvent.WordMultipliers[i] : validWordEvent.LetterMultipliers[i];

//                    fx.InitializeMultiplier(factor, isWordMultiplier, letterPosition);
//                    GameManager.Instance.AnalyticsManager.RaiseMultiplierEvent(validWordEvent.BoardId, letterPosition, factor, isWordMultiplier);

//                    multiplierFxs.Add(fx);
//                }
//            }

//            // handle word length bonus text
//            var wordLengthBonus = ComputeWordLengthBonus(validWordEvent.Word.Length);
//            if (wordLengthBonus > 0)
//            {
//                var gameObjectPosition = (validWordEvent.WordPosition + (Vector3.forward * -2f));

//                var fx = multiplierFxPool.Spawn(true).GetComponent<MultiplierPlayedFX>();
//                fx.InitializeWordLengthBonus(wordLengthBonus, gameObjectPosition);

//                GameManager.Instance.AnalyticsManager.RaiseWordLengthBonusEvent(validWordEvent.BoardId, validWordEvent.Word, wordLengthBonus);

//                multiplierFxs.Add(fx);
//            }

//            // animate the multiplier
//            StartCoroutine(AnimateMultipliers(multiplierFxs));
//        }
//    }

//    /// <summary>
//    /// Computes the word length bonus that gets awarded to player based on its length
//    /// </summary>
//    /// <param name="wordLength"></param>
//    /// <returns></returns>
//    private int ComputeWordLengthBonus(int wordLength)
//    {
//        if (wordLength >= 7)
//            return Constants.WordLengthBonus.LENGTH_7_BONUS;
//        else if (wordLength >= 6)
//            return Constants.WordLengthBonus.LENGTH_6_BONUS;
//        else if (wordLength >= 5)
//            return Constants.WordLengthBonus.LENGTH_5_BONUS;

//        return 0;
//    }

//    private IEnumerator AnimateMultipliers(List<MultiplierPlayedFX> multiplierFxs)
//    {
//        for (int i = 0; i < multiplierFxs.Count; i++)
//        {
//            var fx = multiplierFxs[i];

//            fx.StartAnimating();

//            yield return new WaitWhile(() => fx.Animating);
//        }
//    }

//    private void HandleStarsEarnedEvent(IEventType obj)
//    {
//        var starsEvent = obj as StarEarnedEvent;

//        Debug.Assert(starsEvent.Stars >= 1 && starsEvent.Stars <= 3, "[AudioManager]: stars earned can only be: 1-3. Stars Earned: " + starsEvent.Stars);

//        float factor = 1f;
//        switch (starsEvent.Stars)
//        {
//            case 1:
//                factor = 3.1f;
//                break;
//            case 2:
//                factor = 5.7f;
//                break;
//            case 3:
//                factor = 8.28f;
//                break;
//        }

//        starEarnedFx.transform.position = Vector3.up * factor;
//        starEarnedFx.SetActive(true);
//    }
//    #endregion

//    #region Members
//    [SerializeField]
//    private PowerupGameObjectMenuEntry[] prefabPowerupGameObjects;

//    [SerializeField]
//    private GameObject letterDissapearFxPrefab;

//    [SerializeField]
//    private GameObject zeroLetterMultiplierFxPrefab;

//    [SerializeField]
//    private GameObject zeroWordMultiplierFxPrefab;

//    [SerializeField]
//    private GameObject multiplierFxPrefab;

//    [SerializeField]
//    private GameObject starEarnedFx;

//    /// <summary>
//    /// The powerups pool.
//    /// </summary>
//    private Dictionary<PowerupType, GameObjectPool> powerupsPool;

//    /// <summary>
//    /// The letter dissapear pool.
//    /// </summary>
//    private GameObjectPool letterDissapearPool;

//    private GameObjectPool zeroLetterFxPool;
//    private GameObjectPool zeroWordFxPool;

//    private GameObjectPool multiplierFxPool;

//    #endregion
//}