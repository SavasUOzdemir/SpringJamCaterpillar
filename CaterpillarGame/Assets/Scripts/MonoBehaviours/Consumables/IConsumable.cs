
public interface IConsumable
{
    void Initialise(ConsumableSpawnpoint consumableSpawnpoint);
    float GetMassRegen();
    ConsumableType GetConsumableType();
    ConsumableSpawnpoint GetSpawnpoint();
    void Destroy();
    void SetIsTriggered(bool isTriggered);
    float GetRequiredMassForConsumableType();
}