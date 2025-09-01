using System.Collections.Generic;
using UnityEngine;
using GameMathods;

public class GameProgress
{
    private List<IEnd> endComponent = new List<IEnd>();

    private ILenguage[] lenguageScripts = new ILenguage[0];
    public bool isStop { get; private set; }
    public bool isAllStop { get; private set; }
    public bool isKorean { get; private set; }
    public bool isLoad { get; private set; } = true;

    //해당 게임 전용 변수
    public int gameHour { get; private set; } = 0;
    public int gameMin { get; private set; } = 0;
    public int day { get; private set; } = 1;
    public bool[] lostNpc { get; private set; } = { false, false };

    public void PlusTime()
    {
        gameMin += 1;

        if(gameMin == 60)
        {
            gameMin = 0;
            gameHour += 1;

            if (gameHour == 6)
            {
                gameHour = 0;
                day += 1;
            }
        }
    }

    public void LostNpc(int _npcNumber) => lostNpc[_npcNumber] = true;

    public void StopController(bool _isStop)
    {
        isStop = _isStop;

        Cursor.visible = _isStop;
        Cursor.lockState = _isStop ? CursorLockMode.None : CursorLockMode.Locked;
    }

    public void StopAllController(bool _isStop)
    {
        isAllStop = _isStop;
        StopController(_isStop);
    }

    public void ChangeLenguage(bool _isKorean)
    {
        isKorean = _isKorean;

        for (int i = 0; i < lenguageScripts.Length; i++)
        {
            lenguageScripts[i].ChangeLenguage();
        }
    }

    public void Add(MonoBehaviour _script)
    {
        if (_script is IEnd end && !endComponent.Contains(end))
        {
            endComponent.Add(end);
        }

        if (_script is ILenguage isLenguage && !Service.ContainArray(lenguageScripts, isLenguage))
        {
            lenguageScripts = Service.AddArray(lenguageScripts, isLenguage);
        }
    }

    public void EndScene()
    {
        for (int i = 0; i < endComponent.Count; i++)
        {
            endComponent[i].End();
        }
    }

    public void ResetScripts(bool _allReset)
    {
        endComponent.Clear();

        if (_allReset)
        {
            //해당 게임 변수
            day = 1;
            gameMin = 0;
            gameHour = 0;

            lostNpc[0] = false;
            lostNpc[1] = false;

            lenguageScripts = new ILenguage[0];
        }
    }

    public void CanLoad(bool _isLoad) => isLoad = _isLoad; //이어하기 경우에만 데이터 로드를 위한 메서드
    public void Remove(ILenguage _lenguage) => lenguageScripts = Service.RemoveArray(lenguageScripts, _lenguage);
}
