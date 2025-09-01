using GameMathods;
using UnityEngine;

public class InSideNpc : MonoBehaviour,
IStarting, IInteraction, IEnd
{
    [HideInInspector] public int npcNumber;
    [HideInInspector] public bool isEvent;
    [HideInInspector] public bool isSoundEvent;

    [HideInInspector] public Animator pos;
    [HideInInspector] public Animator[] npc = new Animator[2];
    [HideInInspector] public Material outLine;

    private AudioClip sound;
    private bool isTalk = false;
    private int ranTalk = 0;

    public void Starting()
    {
        npcNumber = -1;
        pos = this.gameObject.GetComponent<Animator>();
        sound = Find.SoundSource("Sleep");

        npc[0] = Find.Child(this.transform, "Npc0").GetComponent<Animator>();
        npc[1] = Find.Child(this.transform, "Npc1").GetComponent<Animator>();

        outLine = Game.SpawnOutLine(this);
        GameService.SetComponent(this);
    }

    public void End() => GameService.gameManager.Remove(this);

    private void SleepFadeFunction()
    {
        //모든 NPC가 없는 상태에서 잠에 들을 경우 게임 종료
        if (GameService.progress.lostNpc[0] && GameService.progress.lostNpc[1])
        {
            GameService.eventManager.CallEvent(EventCode.OnOutSound);
            GameService.eventManager.CallEvent(EventCode.OnBreath);
            GameService.eventManager.CallEvent(EventCode.OffSlider);
            GameService.eventManager.CallUi(UiCode.OffMenu);

            GameService.gameManager.SetAllOffItem();
            GameService.itemController.ResetItem();
            GameService.ChangeScene("GameOver", false);
        }

        else
        {
            if(!isEvent) outLine.SetFloat("_Scale", 1.02f);
            GameService.soundManager.OnEffect(sound);

            GameService.eventManager.CallUi(UiCode.PointEnter);
            GameService.eventManager.CallEvent(EventCode.ResetSleep);
        }

        GameService.eventManager.CallUi(UiCode.StopVolume);
        GameService.progress.StopController(false);
        GameService.fade.PlayFade(true, null, 1.7f);
    }

    private void ClearFadeFunction()
    {
        //사운드 이벤트의 경우 사운드 종료
        if (isSoundEvent) GameService.eventManager.CallEvent(EventCode.OffSoundEvent);

        GameService.eventManager.CallEvent(EventCode.ClearEvent);
        GameService.progress.StopController(false);
        GameService.fade.PlayFade(true, null, 1f);
    }

    public void Interaction()
    {
        var talk = GameService.talkManager;
        var isKorean = GameService.progress.isKorean;

        //의자만 있을 경우
        if (npcNumber < 0)
        {
            switch (talk.talkID)
            {
                case 0:
                    outLine.SetFloat("_Scale", 0f);
                    talk.SetTalker(this);
                    talk.ShowTalk(isKorean ? "(잠깐 눈 좀 붙일까?..)" : "(Should I take a quick nap?..)");

                    talk.ShowYesButton(isKorean ? "네" : "Yes", 10);
                    talk.ShowNoButton(isKorean ? "아니오" : "No", -1);
                    break;

                case 10:
                    talk.EndTalk(false);
                    GameService.progress.StopController(true);
                    GameService.eventManager.CallUi(UiCode.OnVignette);
                    GameService.fade.PlayFade(false, SleepFadeFunction, 0.3f);
                    break;

                case -1:
                    outLine.SetFloat("_Scale", 1.02f);
                    talk.EndTalk(true);
                    break;
            }
        }

        else
        {
            //사운드 이벤트
            if (isSoundEvent)
            {
                switch (talk.talkID)
                {
                    case 0:
                        GameService.camController.LookNpc(this.transform.position + Vector3.up * 0.6f);

                        talk.SetTalker(this);
                        talk.ShowTalk(isKorean ? "밖에서 무슨 소리가 들리는 것 같아.." : "I think I hear something outside...");

                        for (int i = 1; i < 7; i++)
                        {
                            var itemName = "Canned" + i.ToString();

                            if (GameService.itemController.CheckPickUp(itemName))
                            {
                                GameService.itemController.RemoveItem(itemName);

                                talk.ShowYesButton(isKorean ? "이거 먹고 정신 좀 차려." : "Eat this and stay awake.", 2);
                                talk.ShowNoButton(null, 0);
                                return;
                            }
                        }

                        talk.ShowYesButton(isKorean ? "졸려서 그런 거일 거야.." : "It must be because you're sleepy.", 1);
                        talk.ShowNoButton(null, 0);
                        break;

                    case 1:
                        talk.ShowTalk(isKorean ? "그래.. 그런 거 같아.." : "Yeah, I think so.");

                        talk.ShowYesButton("...", -1);
                        talk.ShowNoButton(null, 0);
                        break;

                    case 2:
                        talk.ShowTalk(isKorean ? "그래..가져다줘서 고마워." : "Okay..Thanks for bringing this.");

                        talk.ShowYesButton("...", -2);
                        talk.ShowNoButton(null, 0);
                        break;

                    case -1:
                        talk.EndTalk(true);
                        GameService.camController.LookNpc(Vector3.zero);
                        break;

                    case -2:
                        talk.EndTalk(false);
                        GameService.camController.LookNpc(Vector3.zero);

                        GameService.progress.StopController(true);
                        GameService.fade.PlayFade(false, ClearFadeFunction, 1f);
                        break;
                }
            }

            //고스트 이벤트
            else if (isEvent && npcNumber > -1)
            {
                switch (talk.talkID)
                {
                    case 0:
                        GameService.camController.LookNpc(this.transform.position + Vector3.up * 0.6f);
                        talk.SetTalker(this);
                        talk.ShowTalk(isKorean ? "아까부터 밖에서 이상한 소리가 들려.." : "I've been hearing strange noises outside..");

                        talk.ShowYesButton("...", 1);
                        talk.ShowNoButton(null, 0);
                        break;

                    case 1:
                        talk.ShowTalk(isKorean ? "내가 직접 나가서 확인해 보고 올게.." : "I'll go out and check it out myself..");

                        talk.ShowYesButton("...", -1);
                        talk.ShowNoButton(null, 0);
                        break;

                    case -1:
                        talk.EndTalk(true);
                        outLine.SetFloat("_Scale", 1.02f);

                        GameService.eventManager.CallEvent(EventCode.ResetTime);
                        GameService.camController.LookNpc(Vector3.zero);
                        pos.Play("MoveNpc" + npcNumber.ToString(), -1, 0);
                        npc[npcNumber].Play("Run", -1, 0);

                        npcNumber = -1;
                        isEvent = false;
                        break;
                }
            }

            //아무런 이벤트가 없는 경우
            else
            {
                if (!isTalk)
                {
                    ranTalk = Random.Range(0, 3);
                    isTalk = true;
                }

                //1일차의 경우
                if(GameService.progress.day == 1)
                {
                    if (ranTalk == 0) Talk0();
                    else if (ranTalk == 1) Talk1();
                    else Talk2();
                }

                //2일차의 경우
                else
                {
                    if (ranTalk == 0) Talk3();
                    else if (ranTalk == 1) Talk4();
                    else Talk5();
                }
            }
        }
    }

    private void Talk0()
    {
        var talk = GameService.talkManager;
        var isKorean = GameService.progress.isKorean;

        switch (talk.talkID)
        {
            case 0:
                GameService.camController.LookNpc(this.transform.position + Vector3.up * 0.6f);
                talk.SetTalker(this);
                talk.ShowTalk(isKorean ? "시간이 꽤 흐른거 같은데.." : "It seems like a long time has passed..");

                talk.ShowYesButton("...", -1);
                talk.ShowNoButton(null, 0);
                break;

            case -1:
                talk.EndTalk(true);
                GameService.camController.LookNpc(Vector3.zero);
                isTalk = false;
                break;
        }
    }

    private void Talk1()
    {
        var talk = GameService.talkManager;
        var isKorean = GameService.progress.isKorean;

        switch (talk.talkID)
        {
            case 0:
                GameService.camController.LookNpc(this.transform.position + Vector3.up * 0.6f);
                talk.SetTalker(this);
                talk.ShowTalk(isKorean ? "밖에서 무슨 소리가 들리는 것 같아.." : "I think I hear something outside..");

                talk.ShowYesButton("...", 1);
                talk.ShowNoButton(null, 0);
                break;

            case 1:
                talk.ShowTalk(isKorean ? "정말 우리 말고 다른 무언가가 있는 걸까..?" : "Is there really something out there besides us..?");

                talk.ShowYesButton("...", -1);
                talk.ShowNoButton(null, 0);
                break;

            case -1:
                talk.EndTalk(true);
                GameService.camController.LookNpc(Vector3.zero);
                isTalk = false;
                break;
        }
    }

    private void Talk2()
    {
        var talk = GameService.talkManager;
        var isKorean = GameService.progress.isKorean;

        switch (talk.talkID)
        {
            case 0:
                GameService.camController.LookNpc(this.transform.position + Vector3.up * 0.6f);
                talk.SetTalker(this);
                talk.ShowTalk(isKorean ? "이번엔 꼭 정상을 가야 할텐데.." : "We should really go to the top this time..");

                talk.ShowYesButton("...", -1);
                talk.ShowNoButton(null, 0);
                break;

            case -1:
                talk.EndTalk(true);
                GameService.camController.LookNpc(Vector3.zero);
                isTalk = false;
                break;
        }
    }

    private void Talk3()
    {
        var talk = GameService.talkManager;
        var isKorean = GameService.progress.isKorean;

        switch (talk.talkID)
        {
            case 0:
                GameService.camController.LookNpc(this.transform.position + Vector3.up * 0.6f);
                talk.SetTalker(this);
                talk.ShowTalk(isKorean ? "너무 졸려.. 조금만 자도 될까..?" : "I'm so sleepy... Can I get some sleep..?");

                talk.ShowYesButton("...", -1);
                talk.ShowNoButton(null, 0);
                break;

            case -1:
                talk.EndTalk(true);
                GameService.camController.LookNpc(Vector3.zero);
                isTalk = false;
                break;
        }
    }

    private void Talk4()
    {
        var talk = GameService.talkManager;
        var isKorean = GameService.progress.isKorean;

        switch (talk.talkID)
        {
            case 0:
                GameService.camController.LookNpc(this.transform.position + Vector3.up * 0.6f);
                talk.SetTalker(this);
                talk.ShowTalk(isKorean ? "밖에 누군가 나가는 것을 본 거 같은데.." : "I think I saw someone go outside.");

                talk.ShowYesButton("...", -1);
                talk.ShowNoButton(null, 0);
                break;

            case -1:
                talk.EndTalk(true);
                GameService.camController.LookNpc(Vector3.zero);
                isTalk = false;
                break;
        }
    }

    private void Talk5()
    {
        var talk = GameService.talkManager;
        var isKorean = GameService.progress.isKorean;

        switch (talk.talkID)
        {
            case 0:
                GameService.camController.LookNpc(this.transform.position + Vector3.up * 0.6f);
                talk.SetTalker(this);
                talk.ShowTalk(isKorean ? "꿈에서 나랑 같은 사람이 이곳에 앉아있는 것을 봤어.." : "In my dream, I saw someone like me sitting here..");

                talk.ShowYesButton("...", 1);
                talk.ShowNoButton(null, 0);
                break;

            case 1:
                talk.ShowTalk(isKorean ? "아.. 꿈이 아니었나..?" : "Oh... Wasn't that a dream...?");

                talk.ShowYesButton("...", -1);
                talk.ShowNoButton(null, 0);
                break;

            case -1:
                talk.EndTalk(true);
                GameService.camController.LookNpc(Vector3.zero);
                isTalk = false;
                break;
        }
    }

    protected void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && npcNumber < 0)
        {
            outLine.SetFloat("_Scale", 1.02f);
        }
    }

    protected void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && npcNumber < 0)
        {
            outLine.SetFloat("_Scale", 0f);
        }
    }
}
