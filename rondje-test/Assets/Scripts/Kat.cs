
using System;

public class Kat : InteractableSprite
{
    public override float TimeToFade => 1f;
    public override string AttachedAnimation => "Kat_Incomming";
    public override string AttachedSound => "Kat";
    public override string AttachedTrigger => "Kat_Trigger";
    public override bool UntriggeredInteraction { get; set; }
}
