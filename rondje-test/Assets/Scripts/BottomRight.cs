
public class BottomRight : InteractableSprite
{
    public override float TimeToFade => 0.1f;
    public override string AttachedAnimation => "test_links_onder";

    //This should be added to something that is happening , so you can call upon the Audiomanager to play a certain sound
    //FindObjectOfType<AudioManager>().Play("Name of Audioclip from Audiomanager (Rechts_Onder), not from filename")


    void Update()
    {

    }
}