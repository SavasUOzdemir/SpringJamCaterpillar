
public interface IConsumable
{
    float GetMassRegen();
    ConsumableType GetConsumableType();
    void Destroy();
    void SetIsTriggered(bool isTriggered);
    float GetRequiredMassForConsumableType();
}