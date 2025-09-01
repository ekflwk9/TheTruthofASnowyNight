using System;
using TMPro;
using UnityEngine;
using GameMathods;

public class ChangeKeyButton : Button,
ISave, ILoad
{
    [Header("¹Ù²Ü Å°")]
    [SerializeField] private ConstKey keyCode;

    private bool isClick;
    private KeyCode[] allKey;
    private TMP_Text buttonText;

    public override void Starting()
    {
        base.Starting();
        allKey = (KeyCode[])Enum.GetValues(typeof(KeyCode));

        buttonText = Find.Child(transform, "ButtonText").gameObject.GetComponent<TMP_Text>();
        buttonText.text = GameKey.key[keyCode].ToString();
    }

    public void Save()
    {
        GameService.dataManager.AddData(new Data(this.name, 0, false, 0f, "", Vector3.zero, GameKey.key[keyCode]));
    }

    public void Load()
    {
        var loadKey = GameService.dataManager.FindData(this.name).keyCode;

        if (loadKey != KeyCode.None)
        {
            GameKey.CanChangeKey(keyCode, loadKey);
            buttonText.text = GameKey.key[keyCode].ToString();
        }
    }

    protected override void Click()
    {
        buttonText.text = GameService.progress.isKorean ? "ÀÔ·Â" : "Press Key";
        isClick = true;
    }

    private void Update()
    {
        if (isClick)
        {
            for (int i = 0; i < allKey.Length; i++)
            {
                if (Input.GetKeyDown(allKey[i]))
                {
                    isClick = false;
                    var isChange = GameKey.CanChangeKey(keyCode, allKey[i]);
                    buttonText.text = isChange ? allKey[i].ToString() : GameKey.key[keyCode].ToString();
                }
            }
        }
    }
}
