using System.Collections.Generic;
using System.Linq;
using GameData.Combat.Math;
using GameData.Common;

namespace GameData.Domains.SpecialEffect.SectStory.Shaolin;

public abstract class ChangeSubAttributeBase : DemonSlayerTrialEffectBase
{
	private readonly CValuePercentBonus _reduceBonus;

	protected abstract IReadOnlyList<ushort> SubAttributes { get; }

	protected ChangeSubAttributeBase(int charId, IReadOnlyList<int> parameters)
		: base(charId)
	{
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		_reduceBonus = CValuePercentBonus.op_Implicit(-parameters[0]);
	}

	public override void OnEnable(DataContext context)
	{
		base.OnEnable(context);
		foreach (ushort subAttribute in SubAttributes)
		{
			CreateAffectedData(subAttribute, (EDataModifyType)3, -1);
		}
	}

	public override int GetModifiedValue(AffectedDataKey dataKey, int dataValue)
	{
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		if (SubAttributes.Contains(dataKey.FieldId))
		{
			return dataValue * _reduceBonus;
		}
		return base.GetModifiedValue(dataKey, dataValue);
	}
}
