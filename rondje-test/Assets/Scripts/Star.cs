using System;

public class Star : InteractableSprite
{
    public override float TimeToFade => 1.0f;
    public override string AttachedAnimation => "Star";
    public override string AttachedSound => "Rechts_Onder";
    public override string AttachedTrigger => "StarTrigger";
    public override bool UntriggeredInteraction { get; set; }
}
