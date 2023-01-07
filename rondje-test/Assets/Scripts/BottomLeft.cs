using System;

public class BottomLeft : InteractableSprite
{
    public override float TimeToFade => 0.1f;
    public override string AttachedAnimation => "test";
    public override string AttachedSound => "Rechts_Onder";
    public override string AttachedTrigger => "BottomLeftTrigger";
    public override bool UntriggeredInteraction { get; set; }
}
