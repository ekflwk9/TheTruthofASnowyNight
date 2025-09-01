using GameMathods;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SensitiveSlider : MonoBehaviour,
IStarting, ISave, ILoad, ILenguage
{
    private Slider slider;
    private TMP_Text sliderText;
    private TMP_Text titleText;

    public void Starting()
    {
        slider = GetComponent<Slider>();
        slider.maxValue = 10f;
        slider.value = 2f;

        titleText = Find.Child(this.transform, "TitleText").GetComponent<TMP_Text>();
        titleText.text = "Mouse Sensitive";

        sliderText = Find.Child(this.transform, "SliderText").GetComponent<TMP_Text>();
        sliderText.text = slider.value.ToString("F1") + "%";

        GameService.SetComponent(this);
    }

    public void Save()
    {
        GameService.dataManager.AddData(new Data(this.name, 0, false, slider.value));
    }

    public void Load()
    {
        var loadvalue = GameService.dataManager.FindData(this.name).floating;
        slider.value = loadvalue == 0 ? 2f : loadvalue;
    }

    public void ChangeLenguage() => titleText.text = GameService.progress.isKorean ? "마우스 감도" : "Mouse Sensitive";

    public void ChangeSensitive()
    {
        if (sliderText == null) return;

        var value = slider.value;
        sliderText.text = value.ToString("F1") + "%";
        GameService.camController.ChangeSensitive(value);
    }
}
