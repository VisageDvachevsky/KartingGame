namespace Project.Kart
{
    public interface IKartInput
    {
        bool IsPlayer { get; }
        float GetHorizontal();
        float GetVertical();

        bool GetItemButtomDown();
        bool GetJumpButtonDown();
        bool GetJumpButtonUp();
        bool GetBoostButtonPressed();
    }
}