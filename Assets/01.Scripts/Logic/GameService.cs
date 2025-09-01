using UnityEngine;
using GameMathods;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public static class GameService
{
    public static PlayerController playerController { get; private set; }
    public static CamController camController { get; private set; }
    public static UiController uiController { get; private set; }
    public static ItemController itemController { get; private set; }
    public static FadeComponent fade { get; private set; }
    public static GameProgress progress { get; private set; } = new GameProgress();
    public static GameManager gameManager { get; private set; } = new GameManager();
    public static SoundManager soundManager { get; private set; } = new SoundManager();
    public static EventManager eventManager { get; private set; } = new EventManager();
    public static DataManager dataManager { get; private set; } = new DataManager();
    public static TalkManager talkManager { get; private set; } = new TalkManager();

    public static void SetComponent(MonoBehaviour _component)
    {
        //class
        if (_component is DropComponent isDrop) gameManager.AddScripts(isDrop);
        if (_component is SoundComponent isSound) soundManager.AddScripts(isSound);
        if (_component is PlayerController isPlayer) playerController = isPlayer;
        else if (_component is UiController isUiCon) uiController = isUiCon;
        else if (_component is CamController isCam) camController = isCam;
        else if (_component is ItemController isTrigger) itemController = isTrigger;
        else if (_component is FadeComponent isFade) fade = isFade;

        //interface
        //if (_component is ILoad) dataManager.AddScripts(_component);
        if (_component is ISave) dataManager.AddScripts(_component);
        if (_component is ILenguage) progress.Add(_component);
        if (_component is IEnd) progress.Add(_component);
        if (_component is IInteraction) gameManager.AddScripts(_component);
        if (_component is IUse) gameManager.AddScripts(_component);
    }

    public static void ChangeScene(string _sceneName, bool _resetAll)
    {
        progress.EndScene();

        if (_resetAll)
        {
            soundManager.ResetScripts(true);
            progress.ResetScripts(true);
            eventManager.ResetScripts();
            dataManager.ResetScripts();
            gameManager.ResetScript();

            Object.Destroy(playerController.gameObject);
            Object.Destroy(uiController.gameObject);
        }

        else
        {
            soundManager.ResetScripts(false);
            progress.ResetScripts(false);
        }

        SceneManager.LoadScene(_sceneName);
    }
}



