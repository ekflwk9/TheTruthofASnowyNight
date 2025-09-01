using GameMathods;
using UnityEngine;

public class SleepSlider : MonoBehaviour,
IStarting
{
    private Animator anim;

    public void Starting()
    {
        GameService.eventManager.AddEvent(EventCode.ResetSleep, ResetSleep);
        GameService.eventManager.AddEvent(EventCode.OffSlider, SetOffSlider);
        anim = this.gameObject.GetComponent<Animator>();
    }

    private void SetOffSlider() => this.gameObject.SetActive(false);

    private void StartSleepGauge()
    {
        switch (GameService.progress.day)
        {
            case 1:
                anim.SetFloat("SleepSpeed", 1f);
                break;

            case 2:
                anim.SetFloat("SleepSpeed", 1.2f);
                break;

            case 3:
                anim.SetFloat("SleepSpeed", 1.5f);
                break;
        }

        anim.Play("SleepSlider", -1, 0);
    }

    private void ResetSleep()
    {
        if (!this.gameObject.activeSelf) this.gameObject.SetActive(true);
        anim.Play("ResetSleep", -1, 0);
    }

    private void CheckSleep()
    {
        if (!GameService.progress.lostNpc[0] || !GameService.progress.lostNpc[1])
        {
            GameService.eventManager.CallEvent(EventCode.Sleep);
        }

        else
        {
            GameService.gameManager.SetAllOffItem();
            GameService.itemController.ResetItem();
            GameService.playerController.MoveAction(0);

            GameService.progress.StopController(true);
            GameService.fade.PlayFade(false, FadeFuntion, 1f);

            this.gameObject.SetActive(false);
        }
    }

    private void FadeFuntion()
    {
        GameService.eventManager.CallEvent(EventCode.OnOutSound);
        GameService.eventManager.CallEvent(EventCode.OnBreath);
        GameService.eventManager.CallEvent(EventCode.OffSlider);
        GameService.eventManager.CallUi(UiCode.PointExit);
        GameService.eventManager.CallUi(UiCode.OffMenu);

        //대화 중일 경우
        if (GameService.talkManager.talker != null)
        {
            GameService.talkManager.EndTalk(false);
        }

        GameService.ChangeScene("GameOver", false);
        GameService.progress.StopController(false);
        GameService.fade.PlayFade(true, null, 1.7f);
    }
}
