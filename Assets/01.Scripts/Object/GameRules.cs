using GameMathods;
using UnityEngine;

public class GameRules : MonoBehaviour,
IStarting, ILoad, IEnd
{
    private IDay day;
    private int eventOverTime;

    private InSideNpc[] inSideNpc;
    private OutSideNpc[] outSideNpc;

    public GameObject menu { get; private set; }
    private AudioClip sound;

    public void Starting()
    {
        GameService.eventManager.CallEvent(EventCode.ResetSleep);
        GameService.eventManager.AddEvent(EventCode.TimeEvent, TimeEventFuntion);
        GameService.eventManager.AddEvent(EventCode.ResetTime, ResetTime);
        GameService.eventManager.AddEvent(EventCode.ClearEvent, ClearEvent);
        GameService.eventManager.AddEvent(EventCode.Sleep, Sleep);

        if (GameService.progress.day == 1) day = this.gameObject.AddComponent<Day1>();
        else if (GameService.progress.day == 2) day = this.gameObject.AddComponent<Day2>();

        var indoor = Find.Child(this.transform, "InDoor").GetComponent<Door>();
        indoor.SetRules(this);

        menu = Find.Child(GameService.uiController.transform, "Menu").gameObject;
        sound = Find.SoundSource("TiredBreath");

        inSideNpc = FindClass(inSideNpc, "InSideNpc");
        outSideNpc = FindClass(outSideNpc, "OutSideNpc");

        GameService.SetComponent(this);
    }

    private T[] FindClass<T>(T[] _array, string _posName) where T : class
    {
        var findPos = Find.Child(this.transform, _posName);
        _array = new T[findPos.childCount];

        for (int i = 0; i < _array.Length; i++)
        {
            var child = findPos.GetChild(i).GetComponent<T>();
            if (child != null) _array[i] = child;
        }

        return _array;
    }

    public void Load() => OnRandomNpc();

    private void ResetTime() => eventOverTime = 0;

    public bool CheckEvent()
    {
        if (eventOverTime != 0) return true;
        else return false;
    }

    public void End()
    {
        GameService.eventManager.RemoveEvent(EventCode.TimeEvent);
        GameService.eventManager.RemoveEvent(EventCode.ResetTime);
        GameService.eventManager.RemoveEvent(EventCode.ClearEvent);
        GameService.eventManager.RemoveEvent(EventCode.Sleep);
    }

    private void TimeEventFuntion()
    {
        if (day.EventTime())
        {
            //이벤트 시작
            eventOverTime++;

            OnRandomEvent();
        }

        if (eventOverTime != 0)
        {
            //15초 타이머 시작
            eventOverTime++;

            //20초 후 패널티
            if (eventOverTime > 30)
            {
                //대화 중일 경우
                if (GameService.talkManager.talker != null)
                {
                    GameService.talkManager.EndTalk(false);
                    GameService.camController.LookNpc(Vector3.zero);
                }

                //UI가 꺼져있을 경우에만
                else if (!menu.activeSelf) GameService.progress.StopController(false);

                //헐떡이는 사운드
                GameService.eventManager.CallEvent(EventCode.ResetSleep);
                GameService.fade.PlayFade(true, null, 5f);
                Penalty();
                ClearEvent();
            }
        }
    }

    private void ClearEvent()
    {
        eventOverTime = 0;

        for (int i = 0; i < inSideNpc.Length; i++)
        {
            inSideNpc[i].npcNumber = -1;
            inSideNpc[i].isEvent = false;
            inSideNpc[i].isSoundEvent = false;
            inSideNpc[i].pos.Play("Idle", -1, 0);
        }

        //NPC 재배치
        OnRandomNpc();
    }

    public void Sleep()
    {
        //대화 중일 경우
        if (GameService.talkManager.talker != null) GameService.talkManager.EndTalk(false);
        else if (!menu.activeSelf) GameService.progress.StopController(false);

        //이벤트 시간일 경우
        if (eventOverTime != 0)
        {
            Penalty();
        }

        else
        {
            var ranLostNpc = 0;

            //랜덤 NPC실종 이벤트
            while (true)
            {
                ranLostNpc = Random.Range(0, inSideNpc.Length);

                //현재 활성화 되있는지 검사
                if (inSideNpc[ranLostNpc].npcNumber > -1)
                {
                    GameService.progress.LostNpc(inSideNpc[ranLostNpc].npcNumber);
                    break;
                }
            }

            GameService.camController.LookNpc(Vector3.zero);
            GameService.soundManager.OnEffect(sound);
        }

        GameService.eventManager.CallEvent(EventCode.ResetSleep);
        GameService.fade.PlayFade(true, null, 5f);
        ClearEvent();
    }

    public void Penalty()
    {
        for (int i = 0; i < inSideNpc.Length; i++)
        {
            if (inSideNpc[i].isEvent)
            {
                //사운드 이벤트일 경우
                if (inSideNpc[i].isSoundEvent)
                {
                    GameService.eventManager.CallEvent(EventCode.OffSoundEvent);
                    GameService.progress.LostNpc(inSideNpc[i].npcNumber);
                }

                //아웃 이벤트일 경우
                else if (inSideNpc[i].npcNumber == -1)
                {
                    for (int I = 0; I < outSideNpc.Length; I++)
                    {
                        //배치된 NPC 비활성화 (포스 아래로)
                        if (outSideNpc[I].npcNumber > -1)
                        {
                            GameService.progress.LostNpc(outSideNpc[I].npcNumber);

                            //초기화
                            outSideNpc[I].npcNumber = -1;
                            outSideNpc[I].pos.Play("Idle", -1, 0);
                            break;
                        }
                    }
                }

                //고스트 이벤트
                else
                {
                    GameService.progress.LostNpc(inSideNpc[i].npcNumber);
                }

                break;
            }
        }

        GameService.eventManager.CallUi(UiCode.StopVolume);
        GameService.camController.LookNpc(Vector3.zero);
        GameService.soundManager.OnEffect(sound);
    }

    private void OutEvent(int _npcIndex)
    {
        //대화 중일 경우
        if (GameService.talkManager.talker != null)
        {
            GameService.talkManager.EndTalk(false);
            GameService.camController.LookNpc(Vector3.zero);
        }

        //해당 NPC 움직이는 애니 재생
        var ranInSide = Random.Range(0, 2);
        var tempNpcNumber = inSideNpc[_npcIndex].npcNumber;

        inSideNpc[_npcIndex].npcNumber = -1;
        inSideNpc[_npcIndex].isEvent = true;
        inSideNpc[_npcIndex].pos.Play("MoveNpc" + tempNpcNumber.ToString(), -1, 0);
        inSideNpc[_npcIndex].npc[tempNpcNumber].Play("Run", -1, 0);

        var ranOutSide = Random.Range(0, 3);

        //랜덤 위치에 NPC배치
        outSideNpc[ranOutSide].npcNumber = tempNpcNumber;
        outSideNpc[ranOutSide].pos.Play("OnNpc" + tempNpcNumber.ToString(), -1, 0);
    }

    private void SoundEvent(int _npcIndex)
    {
        inSideNpc[_npcIndex].isEvent = true;
        inSideNpc[_npcIndex].isSoundEvent = true;

        GameService.eventManager.CallEvent(EventCode.OnSoundEvent);
    }

    private void GhostEvent()
    {
        var ranNpc = 0;

        if (GameService.talkManager.talker != null)
        {
            GameService.talkManager.EndTalk(false);
            GameService.camController.LookNpc(Vector3.zero);
        }

        while (true)
        {
            ranNpc = Random.Range(0, inSideNpc.Length);

            //해당 자리에 NPC가 없는 경우에만
            if (inSideNpc[ranNpc].npcNumber < 0)
            {
                var ranSpawn = Random.Range(0, 2);

                inSideNpc[ranNpc].isEvent = true;
                inSideNpc[ranNpc].npcNumber = ranSpawn;

                inSideNpc[ranNpc].npc[ranSpawn].Play("Sit", -1, 0);
                inSideNpc[ranNpc].pos.Play("SitNpc" + ranSpawn.ToString(), -1, 0);
                inSideNpc[ranNpc].outLine.SetFloat("_Scale", 0f);
                break;
            }
        }
    }

    public void OnRandomEvent()
    {
        //모든 NPC가 실종 상태일 경우 이벤트 발생 안됨
        if (GameService.progress.lostNpc[0] && GameService.progress.lostNpc[1])
        {
            eventOverTime = 0;
            return;
        }

        var ranNpc = 0;

        while (true)
        {
            ranNpc = Random.Range(0, inSideNpc.Length);

            //패널티가 아닌 NPC가 존재할 경우에만 랜덤 이벤트 호출
            if (inSideNpc[ranNpc].npcNumber > -1)
            {
                var ranEvent = Random.Range(0, 3);

                if (ranEvent == 0) OutEvent(ranNpc);
                else if (ranEvent == 1) SoundEvent(ranNpc);
                else if (ranEvent == 2) GhostEvent();

                break;
            }
        }
    }

    public void OnRandomNpc()
    {
        //모든 NPC가 실종 상태일 경우 이벤트 발생 안됨
        if (GameService.progress.lostNpc[0] && GameService.progress.lostNpc[1]) return;

        var spawnNpcCount = 0;
        var randomValue = 0;

        while (spawnNpcCount < 2)
        {
            randomValue = Random.Range(0, inSideNpc.Length);

            //패널티가 없는 NPC의 경우 = 셋팅
            if (!GameService.progress.lostNpc[spawnNpcCount] && inSideNpc[randomValue].npcNumber < 0)
            {
                inSideNpc[randomValue].npcNumber = spawnNpcCount;
                inSideNpc[randomValue].npc[spawnNpcCount].Play("Sit", -1, 0);
                inSideNpc[randomValue].pos.Play("SitNpc" + spawnNpcCount.ToString(), -1, 0);
                spawnNpcCount++;
            }

            //패널티가 있는 NPC의 경우 = 다음 카운트
            else if (GameService.progress.lostNpc[spawnNpcCount])
            {
                spawnNpcCount++;
            }
        }

        for (int i = 0; i < inSideNpc.Length; i++)
        {
            inSideNpc[i].outLine.SetFloat("_Scale", 0f);
        }
    }
}
