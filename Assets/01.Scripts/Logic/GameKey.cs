using UnityEngine;
using System.Collections.Generic;

public enum ConstKey
{
    Up = 0,
    Down = 1,
    Left = 2,
    Right = 3,
    Run = 4,
    Mouse0 = 5,
    Jump = 6,
    Slot1 = 7,
    Slot2 = 8,
    Drop = 9,
}

public static class GameKey
{
    public static Dictionary<ConstKey, KeyCode> key = new Dictionary<ConstKey, KeyCode>()
    {
        { ConstKey.Up, KeyCode.W },
        { ConstKey.Down, KeyCode.S },
        { ConstKey.Left, KeyCode.A },
        { ConstKey.Right, KeyCode.D },
        
        { ConstKey.Run, KeyCode.LeftShift },
        //{ ConstKey.Jump, KeyCode.Space },
        { ConstKey.Mouse0, KeyCode.Mouse0 },

        { ConstKey.Slot1, KeyCode.Alpha1 },
        { ConstKey.Slot2, KeyCode.Alpha2 },
        { ConstKey.Drop, KeyCode.G },
    };

    public static bool CanChangeKey(ConstKey _constKey, KeyCode _changeKey)
    {
        if(key.ContainsValue(_changeKey)) return false;

        key[_constKey] = _changeKey;
        return true;
    }
}
