//using UnityEngine;
//using System.Collections;
//using System.Collections.Generic;

//public sealed class ParticleManager : Singleton<ParticleManager>, ISubscriberEvents
//{

//	#region Members

//	[SerializeField]
//	private ParticlesMenuEntry[] prefabParticles;
    
//	private Dictionary<int, ParticleSystem[]> particleSystems = null;

//	#endregion

//	override protected void Awake ()
//	{
//		base.Awake ();

//		particleSystems = new Dictionary<int, ParticleSystem[]> ();

//		for (int i = 0; i < prefabParticles.Length; ++i) {
//			// instantiate the gameobject from prefab
//			GameObject particleGameObj = Instantiate<GameObject> (prefabParticles [i].Particles);

//			// get the particleSystems embedded
//			ParticleSystem[] particles = particleGameObj.GetComponentsInChildren<ParticleSystem> (false);

//			// make the particleSystems children of ParticleManager
//			foreach (var particleSystem in particles) {
//				// making particle systems children of Instance
//				particleSystem.transform.parent = Instance.transform;
//			}

//			// insert particleSystems into dictionary
//			if (!particleSystems.ContainsKey (prefabParticles [i].KEY)) {
//				particleSystems.Add (prefabParticles [i].KEY, particles);
//			} else {
//				Debug.LogError ("Tried to insert different particle systems for the same key: " + prefabParticles [i].evnt);
//			}
            
//			// destroy prefab refs
//			prefabParticles [i].Particles = null;
//		}
//	}

//	void Start ()
//	{
////        Debug.Log ("starting particle system and registering event handlers");
//		RegisterEventHandlers ();    
//	}

//	new void OnDestroy ()
//	{
//		particleSystems.Clear ();
//		particleSystems = null;

//		this.UnRegisterEventHandlers ();

//		base.OnDestroy ();
//	}

//	private void PlayParticles (int index, Vector3 position)
//	{
////        Debug.Log ("trying to play particles for " + eventType);

//		ParticleSystem[] pSystems = null;

//        if (!particleSystems.TryGetValue (index, out pSystems)) {
//			Debug.LogError ("No associated particle system found for index: " + index);
//        }

//		foreach (var particleSystem in pSystems) {
//			if (!particleSystem.isPlaying) {
//				particleSystem.transform.localPosition = (Vector3.right * position.x) + (Vector3.up * position.y);
//				particleSystem.Play ();
//			}
//		}
//	}

//	#region Event Handlers

//	public void RegisterEventHandlers ()
//	{
//		GameManager.Instance.EventAggregator.Register<ScoreEvent> (Instance.HandleScoreEvent);
//		GameManager.Instance.EventAggregator.Register<InvalidWord> (Instance.HandleInValidWord);
//	}

//	public void UnRegisterEventHandlers ()
//	{
//		GameManager.Instance.EventAggregator.UnRegister<ScoreEvent> (Instance.HandleScoreEvent);
//		GameManager.Instance.EventAggregator.UnRegister<InvalidWord> (Instance.HandleInValidWord);
//	}

//	private void HandleScoreEvent (IEventType evnt)
//	{
//		var scoreEvent = evnt as ScoreEvent;

//		int index = 1;

//		// imp: these indices correspond to unity game objects in editor
//		if (scoreEvent.WordScore < 8)
//			index = 0;
//		else if (scoreEvent.WordScore >= 30)
//			index = 2;
//		else
//			index = 1;

//		PlayParticles (index, scoreEvent.WorldPosition);
//	}

//	private void HandleInValidWord (IEventType evnt)
//	{
//		PlayParticles (-1, (evnt as InvalidWord).WordPosition);
//	}

//	#endregion
//}
	
//[System.Serializable]
//public class ParticlesMenuEntry : MenuEntry<int, GameObject>{
//	public GameObject Particles { get { return this.VALUE; } set { this.VALUE = value; }}
//}