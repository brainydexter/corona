//using UnityEngine;

//using System;
//using System.Collections.Generic;

///*
// * 
// * - http://joseoncode.com/2010/04/29/event-aggregator-with-reactive-extensions/
// * - https://github.com/ephe-meral/EventAggregator-CSharp/tree/master/EpheMeral.EventDrivenDesign
// * 
// * Todo: Make it a singleton and track memory
// */
//[RequireComponent(typeof(AudioSource))]
//public sealed class AudioManager : Singleton<AudioManager>, ISubscriberEvents
//{
//    #region Mono Methods

//    public new void OnDestroy()
//    {
//        this.fxSpeaker = null;
//        this.musicSpeaker = null;

//        UnRegisterEventHandlers();

//        base.OnDestroy();
//    }

//    protected override void Awake()
//    {
//        if (ReadPlayerPrefsVolume())
//            WritePlayerPrefsVolume();

//        base.Awake();
//    }

//    void Start()
//    {
//        //		Debug.Log ("[AudioManager]: registering audio event handlers");

//        RegisterEventHandlers();
//    }
//    #endregion

//    private void PlayFX<T>(T key, MenuEntry<T, AudioClip>[] clipsCollection)
//    {
//        //Debug.Log("[AudioManager]: Trying to play audio" + key);

//        for (int i = 0; i < clipsCollection.Length; i++)
//        {
//            if (clipsCollection[i].evnt.Equals(key))
//            {
//                //Debug.Log("[AudioManager]: Playing sound effect: " + clipsCollection[i].resource.name);
//                fxSpeaker.PlayOneShot(clipsCollection[i].resource, 1F);
//                return;
//            }
//        }

//#if UNITY_EDITOR
//        Debug.LogWarning("[AudioManager]: No associated audio found for powerup: " + key);
//#endif
//    }

//    private void PlaySceneMusic(string sceneName)
//    {
//        while (musicSpeaker.isPlaying)
//            musicSpeaker.Stop();

//        if (sceneName == Constants.Scenes.BoardScene)
//        {
//            string boardAudioFile = BoardCollection.Instance.GetBoardMusic(GameManager.Instance.LevelManager.CurrentBoardId);
//            string audioFile = "audio/" + boardAudioFile;

//            // TODO: cache the clip if need be
//            musicSpeaker.clip = Utility.LoadResource<AudioClip>(audioFile);
//            musicSpeaker.Play();
//            return;
//        }

//        for (int i = 0; i < sceneMusic.Length; i++)
//        {
//            if (sceneMusic[i].evnt == sceneName)
//            {
//                //				Debug.Log ("[AudioManager]: Playing background music " + sceneMusic[i].resource.name);

//                musicSpeaker.clip = sceneMusic[i].resource;
//                musicSpeaker.Play();
//                break;
//            }
//        }
//    }

//    /// <summary>
//    /// Reads the player prefs volume for fx and music. 
//    /// </summary>
//    /// <returns><c>true</c>, if either fx or music volume was different on file vs in memory, <c>false</c> otherwise.</returns>
//    private bool ReadPlayerPrefsVolume()
//    {
//        bool update = false;

//        float vol = PlayerPrefs.GetFloat(Constants.Persist.FX_VOLUME, -1f);
//        update = ((vol == -1f) || (FxVolume != vol));

//        FxVolume = (vol != -1f) ? vol : 0.5f;

//        vol = PlayerPrefs.GetFloat(Constants.Persist.MUSIC_VOLUME, -1f);
//        update |= ((vol == -1f) || (MusicVolume != vol));

//        MusicVolume = (vol != -1f) ? vol : 0.5f;

//        return update;
//    }

//    /// <summary>
//    /// Persists the fx and music volume to player prefs
//    /// </summary>
//    public void WritePlayerPrefsVolume()
//    {
//        PlayerPrefs.SetFloat(Constants.Persist.FX_VOLUME, FxVolume);
//        PlayerPrefs.SetFloat(Constants.Persist.MUSIC_VOLUME, MusicVolume);
//    }

//    #region event handlers

//    private void HandleValidWord(IEventType evnt)
//    {
//        ValidWord validWordEvent = evnt as ValidWord;

//        if (!validWordEvent.Has0Multiplier)
//            PlayFX<GEventType>(GEventType.VALIDWORD, audioClips);
//        else
//        {

//            //			if(validWordEvent.Mul0LetterIndex != -1)
//            //				fxSpeaker.PlayOneShot (zeroLetterMultiplierClip);
//            if (validWordEvent.Mul0WordIndex != -1)
//                fxSpeaker.PlayOneShot(zeroWordMultiplierClip);
//        }
//    }

//    private void HandleInValidWord(IEventType evnt)
//    {
//        PlayFX<GEventType>(GEventType.INVALIDWORD, audioClips);
//    }

//    private void HandleGenericPowerup(IEventType evnt)
//    {
//        PlayFX<PowerupType>((evnt as PowerupGenericEvent).PowerupType, powerupAudioClips);
//    }

//    /// <summary>
//    /// Play popcorn audio when a letter is popped
//    /// </summary>
//    /// <param name="obj"></param>
//    private void HandleLetterPopped(IEventType obj)
//    {
//        PlayFX<PowerupType>(PowerupType.POPCORN, powerupAudioClips);
//    }

//    private void HandleFxEvent(IEventType evnt)
//    {
//        PlayFX<FxEventType>((evnt as FxEvent).FxType, fxAudioClips);
//    }

//    private void HandleBoardComplete(IEventType evnt)
//    {
//        var boardCompleteEvent = evnt as BoardCompleteEvent;

//        Debug.Assert(boardCompleteEvent.Stars >= 0 && boardCompleteEvent.Stars <= 3, "[AudioManager]: Number of stars earned should be between 0-3. Earned: " + boardCompleteEvent.Stars);

//        PlayFX<FxEventType>(FxEventType.BOARD_COMPLETED_0_STARS + boardCompleteEvent.Stars, fxAudioClips);

//        musicSpeaker.clip = boardCompleteEvent.Completed ? boardWonClip : boardLostClip;
//        musicSpeaker.Play();
//    }

//    private void HandleSceneChange(IEventType evnt)
//    {
//        PlaySceneMusic((evnt as SceneChangeEvent).SceneName);
//    }

//    private void HandlePurchaseComplete(IEventType obj)
//    {
//        var purchaseComplete = obj as PurchaseCompleteEvent;

//        if (purchaseComplete.PurchaseSuccessful)
//        {
//            fxSpeaker.PlayOneShot(purchaseSuccessfulClip, 1F);
//        }
//        else
//        {
//            fxSpeaker.PlayOneShot(purchaseCancelClip, 1F);
//        }
//    }

//    private void HandleStarsEarnedEvent(IEventType obj)
//    {
//        var starsEvent = obj as StarEarnedEvent;

//        Debug.Assert(starsEvent.Stars >= 1 && starsEvent.Stars <= 3, "[AudioManager]: stars earned can only be: 1-3. Stars Earned: " + starsEvent.Stars);

//        PlayFX<FxEventType>(FxEventType.STARS_1 + (starsEvent.Stars - 1), fxAudioClips);

//        if (starsEvent.Stars == 3)
//        {
//            fxSpeaker.clip = Utility.LoadResource<AudioClip>("audio/win_bonus");
//            fxSpeaker.Play();
//        }
//    }

//    #endregion

//    #region Volume properties
//    private float fxVolume = 0f;
//    public float FxVolume
//    {
//        get { return fxVolume; }
//        set
//        {
//            fxVolume = value;
//            fxSpeaker.volume = value;
//        }
//    }

//    private float musicVolume = 0f;
//    public float MusicVolume
//    {
//        get
//        {
//            return musicVolume;
//        }
//        set
//        {
//            musicVolume = value;
//            musicSpeaker.volume = value;
//        }
//    }
//    #endregion

//    #region ISubscriberEvents implementation

//    public void RegisterEventHandlers()
//    {
//        GameManager.Instance.EventAggregator.Register<ValidWord>(Instance.HandleValidWord);
//        GameManager.Instance.EventAggregator.Register<InvalidWord>(Instance.HandleInValidWord);

//        GameManager.Instance.EventAggregator.Register<PowerupGenericEvent>(Instance.HandleGenericPowerup);
//        GameManager.Instance.EventAggregator.Register<LetterPoppedEvent>(Instance.HandleLetterPopped);
//        GameManager.Instance.EventAggregator.Register<SceneChangeEvent>(Instance.HandleSceneChange);
//        GameManager.Instance.EventAggregator.Register<FxEvent>(Instance.HandleFxEvent);
//        GameManager.Instance.EventAggregator.Register<BoardCompleteEvent>(Instance.HandleBoardComplete);

//        GameManager.Instance.EventAggregator.Register<PurchaseCompleteEvent>(Instance.HandlePurchaseComplete);
//        GameManager.Instance.EventAggregator.Register<StarEarnedEvent>(HandleStarsEarnedEvent);
//    }

//    public void UnRegisterEventHandlers()
//    {
//        GameManager.Instance.EventAggregator.UnRegister<ValidWord>(Instance.HandleValidWord);
//        GameManager.Instance.EventAggregator.UnRegister<InvalidWord>(Instance.HandleInValidWord);

//        GameManager.Instance.EventAggregator.UnRegister<PowerupGenericEvent>(Instance.HandleGenericPowerup);
//        GameManager.Instance.EventAggregator.UnRegister<LetterPoppedEvent>(Instance.HandleLetterPopped);

//        GameManager.Instance.EventAggregator.UnRegister<SceneChangeEvent>(Instance.HandleSceneChange);
//        GameManager.Instance.EventAggregator.UnRegister<FxEvent>(Instance.HandleFxEvent);
//        GameManager.Instance.EventAggregator.UnRegister<BoardCompleteEvent>(Instance.HandleBoardComplete);

//        GameManager.Instance.EventAggregator.UnRegister<PurchaseCompleteEvent>(Instance.HandlePurchaseComplete);
//        GameManager.Instance.EventAggregator.UnRegister<StarEarnedEvent>(HandleStarsEarnedEvent);
//    }

//    #endregion

//    #region members

//    [SerializeField]
//    private AudioMenuEntry[] audioClips;

//    [SerializeField]
//    private PowerupAudioMenuEntry[] powerupAudioClips;

//    [SerializeField]
//    private SceneMusicMenuEntry[] sceneMusic;

//    [SerializeField]
//    private AudioFxMenuEntry[] fxAudioClips;

//    [SerializeField]
//    private AudioClip zeroWordMultiplierClip;

//    [SerializeField]
//    private AudioClip purchaseSuccessfulClip;

//    [SerializeField]
//    private AudioClip purchaseCancelClip;

//    [SerializeField]
//    private AudioClip boardWonClip;

//    [SerializeField]
//    private AudioClip boardLostClip;

//    [SerializeField]
//    private AudioSource fxSpeaker = null;

//    [SerializeField]
//    private AudioSource musicSpeaker = null;

//    #endregion

//    [Serializable]
//    public class AudioFxMenuEntry : MenuEntry<FxEventType, AudioClip>
//    {
//    }

//    [Serializable]
//    public class SceneMusicMenuEntry : MenuEntry<string, AudioClip>
//    {
//    }

//    [Serializable]
//    public class AudioMenuEntry : MenuEntry<GEventType, AudioClip>
//    {
//    }

//    [Serializable]
//    public class PowerupAudioMenuEntry : MenuEntry<PowerupType, AudioClip>
//    {
//    }
//}

//public enum FxEventType
//{
//    LETTER_SPAWN = 0,               // triggered when the active letter is spawned
//    LETTER_SETTLED,             // triggered when the letter settles down
//    HORIZONTAL_SWIPE,               // triggered when the player swipes horizontally to move the active letter
//    VERTICAL_SWIPE,             // triggered when the player swipes vertically to plummet the letter
//    WORD_BEGIN,             // triggered when the player begins forming the word
//    POWERUP_PICK,               // triggered when the player picks up a powerup from conveyor belt
//    BUTTON_PLAY_BOARD,              // admit one button is clicked
//    BUTTON_PRESSED,             // other button clicked
//    BOARD_COMPLETED_0_STARS,               // player completed the board with 0 stars
//    BOARD_COMPLETED_1_STARS,               // player completed the board with 1 stars
//    BOARD_COMPLETED_2_STARS,               // player completed the board with 2 stars
//    BOARD_COMPLETED_3_STARS,               // player completed the board with 3 stars
//    VOLUME_CHANGED,             // player changes fx volume
//    STARS_1,                // when player scores 1 star
//    STARS_2,                // when player scores 2 star
//    STARS_3,				// when player scores 3 star
//    PICKACARD_SHUFFLE,               // when pick a card shuffling animation is happening
//    PICKACARD_PICK,               // when user selects a card
//    PICKACARD_REVEAL,               // when picked card is revealed
//    PICKACARD_REVEAL_NOTSELECTED,               // when not picked cards are revealed
//}

//public sealed class FxEvent : IEventType
//{
//    public static FxEvent Build(FxEventType fxType, bool fireImmediately = true)
//    {
//        var fxEvent = EventBuilder.Instance.Spawn<FxEvent>();
//        fxEvent.FxType = fxType;
//        fxEvent.FireImmediately = fireImmediately;

//        return fxEvent;
//    }

//    public override void Reset() { }

//    public FxEventType FxType { get; private set; }
//}