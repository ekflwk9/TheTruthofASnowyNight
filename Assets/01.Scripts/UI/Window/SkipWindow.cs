using GameMathods;
using TMPro;
using UnityEngine;

public class SkipWindow : MonoBehaviour,
IStarting
{
    private TMP_Text skipText;

    public void Starting()
    {
        skipText = GetComponent<TMP_Text>();
        skipText.text = GameService.progress.isKorean ? "�����̽��ٷ� �ǳʶٱ�" : "Skip To Space";
    }
}
