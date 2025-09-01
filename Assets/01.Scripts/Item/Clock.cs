using System.Text;
using GameMathods;
using TMPro;
using UnityEngine;

public class Clock : MonoBehaviour,
IStarting, IInteraction, IEnd
{
    private bool isClear = false;
    private int saveDay = 0;
    private int activeTime = 0;

    private TMP_Text timeText;
    private StringBuilder builder = new StringBuilder(30);
    private AudioClip onSound;
    private AudioClip endSound;

    public void Starting()
    {
        saveDay = GameService.progress.day;

        timeText = Find.Child(this.transform, "Time").GetComponent<TMP_Text>();
        this.gameObject.AddComponent<TouchingComponent>().Starting();

        onSound = Find.SoundSource("OnClock");
        endSound = Find.SoundSource("EndClock");
        GameService.SetComponent(this);
    }

    private void NextStageFunction()
    {
        if (saveDay == 1) GameService.ChangeScene("Clear", false);
        else if (saveDay == 2) GameService.ChangeScene("End", false);

        if(GameService.talkManager.talker != null)
        {
            GameService.talkManager.EndTalk(false);
        }

        GameService.eventManager.CallUi(UiCode.StopVolume);
        GameService.itemController.ResetItem();
        GameService.gameManager.SetAllOffItem();

        GameService.eventManager.CallUi(UiCode.OffMenu);
        GameService.playerController.transform.position = Vector3.zero;

        GameService.fade.PlayFade(true, null, 1f);
        GameService.progress.StopController(false);
    }

    private void AnimClockTime()
    {
        if (isClear) return;

        GameService.progress.PlusTime();

        if (saveDay < GameService.progress.day)
        {
            isClear = true;
            GameService.playerController.MoveAction(0);
            GameService.eventManager.CallEvent(EventCode.OffSlider);
            GameService.eventManager.CallUi(UiCode.PointExit);

            GameService.soundManager.OnBackGround(null);
            GameService.progress.StopController(true);

            GameService.soundManager.OnEffect(endSound);
            GameService.fade.PlayFade(false, NextStageFunction, 1f);
            return;
        }

        GameService.eventManager.CallEvent(EventCode.TimeEvent);

        //활성화 상태일 경우 비활성화
        if (timeText.gameObject.activeSelf)
        {
            TimeUpdate();
            activeTime += 1;

            if (activeTime == 3)
            {
                activeTime = 0;
                timeText.gameObject.SetActive(false);
            }
        }
    }

    private void TimeUpdate()
    {
        var gameTime = GameService.progress;

        builder.Clear();
        timeText.text = "";

        //시
        builder.Append("0");
        builder.Append(gameTime.gameHour);
        builder.Append(" : ");

        //분
        if (gameTime.gameMin < 10) builder.Append("0");
        builder.Append(gameTime.gameMin);

        //일
        builder.Append("\n7/");
        builder.Append(gameTime.day);

        //출력
        timeText.text = builder.ToString();
    }

    public void Interaction()
    {
        //활성화 상태라면 상호작용 안됨
        if (!timeText.gameObject.activeSelf)
        {
            TimeUpdate();
            timeText.gameObject.SetActive(true);
        }

        GameService.soundManager.OnEffect(onSound);
        GameService.eventManager.CallUi(UiCode.PointEnter);
    }

    public virtual void End() => GameService.gameManager.Remove(this);
}
