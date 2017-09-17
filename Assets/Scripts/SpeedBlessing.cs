public class SpeedBlessing : Blessing
{
	public float SpeedFactor = 1.5f;
	public float AccelerationFactor = 1.2f;
	
	protected override void OnApply(Player player)
	{
		player.Entity.MaxSpeed *= SpeedFactor;
		player.Entity.MaxAcceleration *= AccelerationFactor;
	}

	protected override void OnExpire(Player player)
	{
		player.Entity.MaxSpeed /= SpeedFactor;
		player.Entity.MaxAcceleration /= AccelerationFactor;
	}
}
