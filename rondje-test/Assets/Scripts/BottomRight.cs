
using System;

public class BottomRight : InteractableSprite
{
    public override float TimeToFade => 0.1f;
    public override string AttachedAnimation => "test_links_onder";
    public override string AttachedSound => "Rechts_Onder";
    public override string AttachedTrigger => "BottomRightTrigger";
    public override DateTime PrevTriggered { get; set; }
}