public class HealingTerrain : BaseTerrain
{
    public override void ModifyHealth(FedeHealth fedeHealth)
    {
        fedeHealth.StartHealing();
    }
}