namespace Project.Kart
{
    public interface IKartInput
    {
        float GetHorizontal();
        float GetVertical();

        bool GetItemButtomDown();
        bool GetJumpButtonDown();
        bool GetJumpButtonUp();
        bool GetBoostButtonPressed();
    }
}