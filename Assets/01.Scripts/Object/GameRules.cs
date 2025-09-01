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
            //�̺�Ʈ ����
            eventOverTime++;

            OnRandomEvent();
        }

        if (eventOverTime != 0)
        {
            //15�� Ÿ�̸� ����
            eventOverTime++;

            //20�� �� �г�Ƽ
            if (eventOverTime > 30)
            {
                //��ȭ ���� ���
                if (GameService.talkManager.talker != null)
                {
                    GameService.talkManager.EndTalk(false);
                    GameService.camController.LookNpc(Vector3.zero);
                }

                //UI�� �������� ��쿡��
                else if (!menu.activeSelf) GameService.progress.StopController(false);

                //�涱�̴� ����
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

        //NPC ���ġ
        OnRandomNpc();
    }

    public void Sleep()
    {
        //��ȭ ���� ���
        if (GameService.talkManager.talker != null) GameService.talkManager.EndTalk(false);
        else if (!menu.activeSelf) GameService.progress.StopController(false);

        //�̺�Ʈ �ð��� ���
        if (eventOverTime != 0)
        {
            Penalty();
        }

        else
        {
            var ranLostNpc = 0;

            //���� NPC���� �̺�Ʈ
            while (true)
            {
                ranLostNpc = Random.Range(0, inSideNpc.Length);

                //���� Ȱ��ȭ ���ִ��� �˻�
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
                //���� �̺�Ʈ�� ���
                if (inSideNpc[i].isSoundEvent)
                {
                    GameService.eventManager.CallEvent(EventCode.OffSoundEvent);
                    GameService.progress.LostNpc(inSideNpc[i].npcNumber);
                }

                //�ƿ� �̺�Ʈ�� ���
                else if (inSideNpc[i].npcNumber == -1)
                {
                    for (int I = 0; I < outSideNpc.Length; I++)
                    {
                        //��ġ�� NPC ��Ȱ��ȭ (���� �Ʒ���)
                        if (outSideNpc[I].npcNumber > -1)
                        {
                            GameService.progress.LostNpc(outSideNpc[I].npcNumber);

                            //�ʱ�ȭ
                            outSideNpc[I].npcNumber = -1;
                            outSideNpc[I].pos.Play("Idle", -1, 0);
                            break;
                        }
                    }
                }

                //��Ʈ �̺�Ʈ
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
        //��ȭ ���� ���
        if (GameService.talkManager.talker != null)
        {
            GameService.talkManager.EndTalk(false);
            GameService.camController.LookNpc(Vector3.zero);
        }

        //�ش� NPC �����̴� �ִ� ���
        var ranInSide = Random.Range(0, 2);
        var tempNpcNumber = inSideNpc[_npcIndex].npcNumber;

        inSideNpc[_npcIndex].npcNumber = -1;
        inSideNpc[_npcIndex].isEvent = true;
        inSideNpc[_npcIndex].pos.Play("MoveNpc" + tempNpcNumber.ToString(), -1, 0);
        inSideNpc[_npcIndex].npc[tempNpcNumber].Play("Run", -1, 0);

        var ranOutSide = Random.Range(0, 3);

        //���� ��ġ�� NPC��ġ
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

            //�ش� �ڸ��� NPC�� ���� ��쿡��
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
        //��� NPC�� ���� ������ ��� �̺�Ʈ �߻� �ȵ�
        if (GameService.progress.lostNpc[0] && GameService.progress.lostNpc[1])
        {
            eventOverTime = 0;
            return;
        }

        var ranNpc = 0;

        while (true)
        {
            ranNpc = Random.Range(0, inSideNpc.Length);

            //�г�Ƽ�� �ƴ� NPC�� ������ ��쿡�� ���� �̺�Ʈ ȣ��
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
        //��� NPC�� ���� ������ ��� �̺�Ʈ �߻� �ȵ�
        if (GameService.progress.lostNpc[0] && GameService.progress.lostNpc[1]) return;

        var spawnNpcCount = 0;
        var randomValue = 0;

        while (spawnNpcCount < 2)
        {
            randomValue = Random.Range(0, inSideNpc.Length);

            //�г�Ƽ�� ���� NPC�� ��� = ����
            if (!GameService.progress.lostNpc[spawnNpcCount] && inSideNpc[randomValue].npcNumber < 0)
            {
                inSideNpc[randomValue].npcNumber = spawnNpcCount;
                inSideNpc[randomValue].npc[spawnNpcCount].Play("Sit", -1, 0);
                inSideNpc[randomValue].pos.Play("SitNpc" + spawnNpcCount.ToString(), -1, 0);
                spawnNpcCount++;
            }

            //�г�Ƽ�� �ִ� NPC�� ��� = ���� ī��Ʈ
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
