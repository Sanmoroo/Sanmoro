using System;
using UnityEngine;

public class TopLeft : InteractableSprite
{
    public override float TimeToFade => 1.0f;
    public override string AttachedAnimation => "test";
    public override string AttachedSound => "Rechts_Onder";
    public override string AttachedTrigger => "TopLeftTrigger";
    public override DateTime PrevTriggered { get; set; }
}
