namespace Systems.PropertyDataSystem.Properties
{
    public interface IHealthProperty
    {
        int CurrentHealth { get; set; }
        int MaxHealth { get; set; }
    }
}