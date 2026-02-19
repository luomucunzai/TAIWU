using GameData.Combat.Math;
using GameData.Common;
using GameData.Domains.Character;
using GameData.Domains.CombatSkill;

namespace GameData.Domains.SpecialEffect.CombatSkill.Common.Attack;

public class PowerUpByMainAttribute : PowerUpOnCast
{
	protected sbyte RequireMainAttributeType;

	private CValuePercent AddPowerFactor => CValuePercent.op_Implicit(base.IsDirect ? 40 : 60);

	protected override EDataModifyType ModifyType => (EDataModifyType)1;

	public PowerUpByMainAttribute()
	{
	}

	public PowerUpByMainAttribute(CombatSkillKey skillKey, int type)
		: base(skillKey, type)
	{
	}

	public unsafe override void OnEnable(DataContext context)
	{
		//IL_006d: Unknown result type (might be due to invalid IL or missing references)
		MainAttributes currMainAttributes = CharObj.GetCurrMainAttributes();
		short num = currMainAttributes.Items[RequireMainAttributeType];
		if (base.IsDirect)
		{
			PowerUpValue = num;
		}
		else
		{
			MainAttributes maxMainAttributes = CharObj.GetMaxMainAttributes();
			PowerUpValue = maxMainAttributes.Items[RequireMainAttributeType] - num;
		}
		PowerUpValue *= AddPowerFactor;
		base.OnEnable(context);
	}
}
