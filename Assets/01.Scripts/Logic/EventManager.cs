using System.Collections.Generic;
using GameMathods;
using UnityEngine;

public enum UiCode
{
    OnMenu = 0,
    OffMenu = 1,

    OnSetting = 2,
    OffSetting = 3,

    OnTouch = 4,
    OffTouch = 5,

    PointEnter = 6,
    PointExit = 7,
    PointClick = 8,

    OnYesButton = 9,
    OffYesButton = 10,
    OnNoButton = 11,
    OffNoButton = 12,

    OnOption = 13,
    OffOption = 14,

    OnTalk = 15,
    OffTalk = 16,

    OnCamera = 17,
    OffCamera = 18,

    OnBlock = 19,
    OffBlock = 20,

    OnStartWindow = 21,
    OffStartWindow = 22,

    //무조건 필요하지 않음
    OnDrop = 27,
    OffDrop = 28,
    DropWindow = 29,

    OnVignette = 30,
    OnFilm = 31,
    StopVolume= 32,
}

public enum EventCode
{
    None = 0,

    //게임 이벤트 등록
    ResetSleep = 1,
    Canned = 2,

    TimeEvent = 3,
    ResetTime = 4,
    ClearEvent = 5,
    Sleep = 6,

    OnSoundEvent = 7,
    OffSoundEvent = 8,

    OffSlider = 9,

    OnSnow = 10,
    OffSnow = 11,

    OnBreath = 12,
    OffBreath = 13,

    OnInSound = 14,
    OnOutSound = 15,

    OnGameOver = 16,
    OnEnding = 17,
    OffEndingBackGround = 18,
}

public class EventManager
{
    private Dictionary<UiCode, Function> uiEvent = new Dictionary<UiCode, Function>();
    private Dictionary<EventCode, Function> itemEvent = new Dictionary<EventCode, Function>();

    public void AddUi(UiCode _eventCode, Function _function)
    {
        if (!uiEvent.ContainsKey(_eventCode)) uiEvent.Add(_eventCode, _function);
        else Debug.Log(_eventCode.ToString() + "은 이미 추가된 UI이벤트임");
    }

    public void AddEvent(EventCode _eventCode, Function _function)
    {
        if (!itemEvent.ContainsKey(_eventCode)) itemEvent.Add(_eventCode, _function);
        else Debug.Log(_eventCode.ToString() + "은 이미 추가된 이벤트임");
    }

    public void RemoveUi(UiCode _isUi)
    {
        if (uiEvent.ContainsKey(_isUi)) uiEvent.Remove(_isUi);
        else Debug.Log(_isUi.ToString() + "의 추가된 정보는 없는 상태");
    }

    public void RemoveEvent(EventCode _eventCode)
    {
        if (itemEvent.ContainsKey(_eventCode)) itemEvent.Remove(_eventCode);
        else Debug.Log(itemEvent.ToString() + "의 추가된 정보는 없는 상태");
    }

    public void ResetScripts()
    {
        uiEvent.Clear();
        itemEvent.Clear();
    }

    public void CallUi(UiCode _eventCode)
    {
        if (uiEvent.ContainsKey(_eventCode)) uiEvent[_eventCode]();
        else Debug.Log(_eventCode.ToString() + "는 등록되지 않은 메서드");
    }

    public void CallEvent(EventCode _eventCode)
    {
        if (itemEvent.ContainsKey(_eventCode)) itemEvent[_eventCode]();
        else Debug.Log(_eventCode.ToString() + "는 등록되지 않은 메서드");
    }
}
