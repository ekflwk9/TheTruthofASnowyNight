using GameMathods;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public abstract class Button : MonoBehaviour,
IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler, IStarting, ILenguage
{
    [Tooltip("한글 제목을 기입해야함, 영어 제목은 오브젝트 이름으로 처리함")]
    [Header("버튼 한글 제목")]
    [SerializeField] private string koreanName;

    protected GameObject touchImage;
    protected TMP_Text titleText;
    protected AudioClip sound;

    public virtual void Starting()
    {
        GameService.SetComponent(this);
        sound = Find.SoundSource("ClickSound");

        touchImage = Find.Child(transform, "TouchImage").gameObject;
        titleText = Find.Child(transform, "TitleText").gameObject.GetComponent<TMP_Text>();
        titleText.text = this.name;
    }

    public virtual void ChangeLenguage() => titleText.text = GameService.progress.isKorean ? koreanName : this.name;
    protected abstract void Click();

    public virtual void OnPointerClick(PointerEventData eventData)
    {
        GameService.soundManager.OnEffect(sound);
        Click();
    }

    public void OnPointerEnter(PointerEventData eventData) => touchImage.SetActive(true);
    public void OnPointerExit(PointerEventData eventData) => touchImage.SetActive(false);
}
