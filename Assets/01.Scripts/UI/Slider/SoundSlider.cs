using GameMathods;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SoundSlider : MonoBehaviour,
IStarting, ISave, ILoad, ILenguage
{
    private Slider slider;
    private TMP_Text sliderText;
    private TMP_Text titleText;

    public void Starting()
    {
        slider = GetComponent<Slider>();
        slider.value = 0.6f;
        var textValue = slider.value * 100;

        titleText = Find.Child(this.transform, "TitleText").GetComponent<TMP_Text>();
        titleText.text = "Sound Volume";

        sliderText = Find.Child(this.transform, "SliderText").GetComponent<TMP_Text>();
        sliderText.text = textValue.ToString("F1") + "%";

        GameService.SetComponent(this);
    }

    public void Save()
    {
        GameService.dataManager.AddData(new Data(this.name, 0, false, slider.value));
    }

    public void Load()
    {
        var loadvalue = GameService.dataManager.FindData(this.name).floating;
        slider.value = loadvalue == 0 ? 0.6f : loadvalue;

        ChangeVolume();
    }

    public void ChangeLenguage() => titleText.text = GameService.progress.isKorean ? "¼Ò¸® º¼·ý" : "Sound Volume";

    public void ChangeVolume()
    {
        if (sliderText == null) return;

        var volume = slider.value;
        GameService.soundManager.SoundVolume(volume);

        volume *= 100;
        sliderText.text = volume.ToString("F1") + "%";
    }
}
