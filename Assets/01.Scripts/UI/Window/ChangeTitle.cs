using GameMathods;
using TMPro;
using UnityEngine;

public class ChangeTitle : MonoBehaviour,
IStarting, ILenguage
{
    [SerializeField] private string koreanTitle;
    private TMP_Text titleText;

    public void Starting()
    {
        GameService.SetComponent(this);

        titleText = Find.Child(this.transform, "TitleText").GetComponent<TMP_Text>();
        titleText.text = this.name;
    }

    public void ChangeLenguage() => titleText.text = GameService.progress.isKorean ? koreanTitle : this.name;
}
