using System;
using GameData.Combat.Math;
using GameData.Common;
using GameData.Domains.Combat;

namespace GameData.Domains.SpecialEffect.SectStory.Yuanshan
{
	// Token: 0x020000E9 RID: 233
	public class VitalDemonB : VitalDemonEffectBase
	{
		// Token: 0x06002963 RID: 10595 RVA: 0x0020071D File Offset: 0x001FE91D
		public VitalDemonB(int charId) : base(charId, 1749)
		{
		}

		// Token: 0x06002964 RID: 10596 RVA: 0x00200730 File Offset: 0x001FE930
		public override void OnEnable(DataContext context)
		{
			base.OnEnable(context);
			foreach (CombatCharacter character in DomainManager.Combat.GetCharacters(base.CombatChar.IsAlly))
			{
				base.CreateAffectedData(character.GetId(), 145, EDataModifyType.Add, -1);
				base.CreateAffectedData(character.GetId(), 146, EDataModifyType.Add, -1);
				base.CreateAffectedData(character.GetId(), 328, EDataModifyType.AddPercent, -1);
			}
		}

		// Token: 0x06002965 RID: 10597 RVA: 0x002007D0 File Offset: 0x001FE9D0
		public override int GetModifyValue(AffectedDataKey dataKey, int currModifyValue)
		{
			bool flag = dataKey.FieldId == 328;
			if (flag)
			{
				base.ShowSpecialEffect(0);
			}
			ushort fieldId = dataKey.FieldId;
			if (!true)
			{
			}
			int result;
			if (fieldId - 145 > 1)
			{
				if (fieldId != 328)
				{
					result = base.GetModifyValue(dataKey, currModifyValue);
				}
				else
				{
					result = 200;
				}
			}
			else
			{
				result = 10000;
			}
			if (!true)
			{
			}
			return result;
		}

		// Token: 0x04000CBE RID: 3262
		private const int AddAttackRange = 10000;

		// Token: 0x04000CBF RID: 3263
		private const int GetTrickCountAddPercent = 200;
	}
}
