using GameMathods;
using UnityEngine;
using UnityEngine.EventSystems;

public class YesTalkButton : Button,
IStarting
{
    private int nextID;

    public override void Starting()
    {
        base.Starting();
        GameService.eventManager.AddUi(UiCode.OnYesButton, On);
        GameService.eventManager.AddUi(UiCode.OffYesButton, Off);
    }

    private void On()
    {
        if (nextID != 0) this.gameObject.SetActive(true);
    }

    private void Off()
    {
        nextID = GameService.talkManager.talkID;
        titleText.text = GameService.talkManager.talkButton ?? "";
        if (this.gameObject.activeSelf) this.gameObject.SetActive(false);
    }

    protected override void Click()
    {
        GameService.talkManager.NextTalk(nextID);
        touchImage.SetActive(false);
    }

    public override void OnPointerClick(PointerEventData eventData) => Click();
}
