using GameMathods;
using Unity.Mathematics;
using UnityEngine;

public class OutSideNpc : MonoBehaviour,
IStarting, IInteraction, IEnd
{
    [HideInInspector] public int npcNumber;
    [HideInInspector] public Animator pos;
    private Quaternion saveRote;

    public void Starting()
    {
        npcNumber = -1;
        pos = this.gameObject.GetComponent<Animator>();
        saveRote = this.transform.rotation;

        GameService.SetComponent(this);
    }

    public virtual void End() => GameService.gameManager.Remove(this);

    private void FadeFuntion()
    {
        pos.Play("Idle", -1, 0);
        this.transform.rotation = saveRote;
        GameService.eventManager.CallEvent(EventCode.ClearEvent);

        GameService.fade.PlayFade(true, null, 1f);
        GameService.progress.StopController(false);
    }

    public void Interaction()
    {
        switch (GameService.progress.day)
        {
            case 1:
                Day1Talk();
                break;

            case 2:
                Day2Talk();
                break;
        }
    }

    private void LookPlayer()
    {
        //2D에서 Y축이 3D에선 X축으로 계산 = Y축 변환 (2D 에서는 Z축)
        var lookRote = GameService.playerController.transform.position - transform.position;
        var tan = math.atan2(lookRote.x, lookRote.z);
        var deg = math.degrees(tan);
        var newRote = Quaternion.Euler(0, deg, 0);

        this.transform.rotation = newRote;
    }

    private void Day1Talk()
    {
        var talk = GameService.talkManager;
        var isKorean = GameService.progress.isKorean;

        switch (talk.talkID)
        {
            case 0:
                LookPlayer();
                GameService.camController.LookNpc(this.transform.position + Vector3.up * 1.6f);

                talk.SetTalker(this);
                talk.ShowTalk(isKorean ? "내가 왜 여기 있는 거지?.." : "Why am I here?");

                talk.ShowYesButton(isKorean ? "밖에 나오면 안 돼 빨리 안으로 들어가자." : "We shouldn't be out here. Let's get inside.", 1);
                talk.ShowNoButton(null, 0);
                break;

            case 1:
                talk.SetTalker(this);
                talk.ShowTalk(isKorean ? "미안해 잠깐 정신이 나갔었나 봐.." : "I'm sorry. I must have gotten carried away.");

                talk.ShowYesButton("...", -1);
                talk.ShowNoButton(null, 0);
                break;

            case -1:
                talk.EndTalk(false);

                GameService.camController.LookNpc(Vector3.zero);
                GameService.progress.StopController(true);
                GameService.fade.PlayFade(false, FadeFuntion, 1f);
                break;
        }
    }

    private void Day2Talk()
    {
        var talk = GameService.talkManager;
        var isKorean = GameService.progress.isKorean;

        switch (talk.talkID)
        {
            case 0:
                LookPlayer();
                GameService.camController.LookNpc(this.transform.position + Vector3.up * 1.6f);

                talk.SetTalker(this);
                talk.ShowTalk("?..");

                talk.ShowYesButton("...", 1);
                talk.ShowNoButton(null, 0);
                break;

            case 1:
                talk.SetTalker(this);
                talk.ShowTalk(isKorean ? "왜 계속 내가 여기 있는지 모르겠어.." : "I don't know why I'm still here...");

                talk.ShowYesButton(isKorean ? "빨리 안으로 들어가자." : "Let's get inside.", 2);
                talk.ShowNoButton(null, 0);
                break;

            case 2:
                talk.SetTalker(this);
                talk.ShowTalk(isKorean ? "그래.. 정신 차리게 해줘서 고마워." : "Okay. Thanks for waking me up.");

                talk.ShowYesButton("...", -1);
                talk.ShowNoButton(null, 0);
                break;

            case -1:
                talk.EndTalk(false);

                GameService.camController.LookNpc(Vector3.zero);
                GameService.progress.StopController(true);
                GameService.fade.PlayFade(false, FadeFuntion, 1f);
                break;
        }
    }
}
