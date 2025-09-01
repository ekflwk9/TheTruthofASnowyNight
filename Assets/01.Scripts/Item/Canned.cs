using UnityEngine;

public class Canned : Item
{
    public override Vector3 showItemPos { get; protected set; } = new Vector3(0.017f, -0.207f, 0.056f);
    public override EventCode eventCode { get; protected set; } = EventCode.Canned;

    public override bool Use()
    {
        return false;
    }
}
