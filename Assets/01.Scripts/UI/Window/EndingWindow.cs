using GameMathods;
using UnityEngine;
using TMPro;

public class EndingWindow : MonoBehaviour,
IStarting
{
    private TMP_Text endingTitle;

    public void Starting()
    {
        GameService.eventManager.AddEvent(EventCode.OnEnding, OnEndingWindown);
        endingTitle = Find.Child(this.transform, "EndingTitle").GetComponent<TMP_Text>();

        var isKorean = GameService.progress.isKorean;

        //GoodEnding
        if (!GameService.progress.lostNpc[0] && !GameService.progress.lostNpc[1]) endingTitle.text = isKorean ? "01. ����� �ḻ Ŭ����" : "01. Clear the Good Ending";

        //SecretEnding
        else if (!GameService.progress.lostNpc[0] && GameService.progress.lostNpc[1]) endingTitle.text = isKorean ? "03. ������ �ḻ Ŭ����" : "03. Clear the Secret Ending";

        //BadEnding
        else endingTitle.text = isKorean ? "02. ������ �ḻ Ŭ����" : "02. Clear the Bad Ending";

        if(this.gameObject.activeSelf) this.gameObject.SetActive(false);
    }

    private void ChangeScene() => GameService.ChangeScene("Start", true);

    private void OffBackGround() => GameService.eventManager.CallEvent(EventCode.OffEndingBackGround);

    private void OnEndingWindown() => this.gameObject.SetActive(true);
}
