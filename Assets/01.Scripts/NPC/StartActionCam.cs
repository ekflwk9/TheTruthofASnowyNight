using GameMathods;
using UnityEngine;

public class StartActionCam : MonoBehaviour,
IStarting, ILoad
{
    private bool isSkip = false;
    private Animator walkAnim;

    public void Starting() => walkAnim = Find.Child(this.transform.parent, "Walk").GetComponent<Animator>();

    public void Load() => GameService.eventManager.CallEvent(EventCode.OnBreath);

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !isSkip)
        {
            isSkip = true;
            GameService.fade.PlayFade(false, NextScene, 1f);
        }
    }

    private void EndAction()
    {
        if (!isSkip)
        {
            isSkip = true;
            GameService.fade.PlayFade(false, NextScene, 1f);
        }
    }

    private void NextScene()
    {
        GameService.ChangeScene("Ready", false);
        GameService.eventManager.CallUi(UiCode.OnCamera);
        GameService.eventManager.CallEvent(EventCode.OffBreath);

        GameService.fade.PlayFade(true);
        GameService.progress.StopController(false);
    }

    private void StopWalk() => walkAnim.SetFloat("Speed", 0f);
}
