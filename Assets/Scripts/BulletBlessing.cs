public class BulletBlessing : Blessing
{
	public float FireRateFactor = 2f;
	public float BulletSpeedFactor = 1.5f;
	public float BulletDistanceFactor = 1.5f;
	public float ScreenShakeFactor = 3f;

	private Bullet _bullet;

	protected override void OnApply(Player player)
	{
		player.FireRateAuto *= FireRateFactor;
		player.FireScreenShake *= ScreenShakeFactor;

		_bullet = player.Bullet.GetComponent<Bullet>();
		_bullet.Speed *= BulletSpeedFactor;
		_bullet.MaxDistance *= BulletDistanceFactor;
	}

	protected override void OnExpire(Player player)
	{
		player.FireRateAuto /= FireRateFactor;
		player.FireScreenShake /= ScreenShakeFactor;
		_bullet.Speed /= BulletSpeedFactor;
		_bullet.MaxDistance /= BulletDistanceFactor;
	}
}
