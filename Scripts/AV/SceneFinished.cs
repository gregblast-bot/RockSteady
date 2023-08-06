//using UnityEngine;
//using UnityEngine.Playables;
//using UnityEngine.SceneManagement;
//using UnityEngine.Timeline;

//public class CutsceneSceneSwitcher : MonoBehaviour
//{
//    public PlayableDirector timelineDirector;
//    public string nextSceneName;

//    void Start()
//    {
//        // Get the SignalAsset from the Timeline's Signal Emitter
//        SignalAsset signal = timelineDirector.playableAsset.outputs
//            .Find(output => output.streamName == "CutsceneFinished")
//            .sourceObject as SignalAsset;

//        // Register a callback for the captured signal
//        if (signal != null)
//        {
//            signal.RegisterSignalCallback(OnCutsceneFinished);
//        }
//    }

//    void OnCutsceneFinished(PlayableDirector director, object signal)
//    {
//        // This method will be called when the cutscene finishes
//        // Switch scenes here
//        SceneManager.LoadScene(nextSceneName);
//    }
//}