using System;
using System.Linq;
using GameData.Combat.Math;
using GameData.Common;
using GameData.Domains.Character;
using GameData.Domains.CombatSkill;

namespace GameData.Domains.SpecialEffect.Animal.Loong.Neigong
{
	// Token: 0x02000601 RID: 1537
	public class Fuxi : AnimalEffectBase
	{
		// Token: 0x06004512 RID: 17682 RVA: 0x002714B5 File Offset: 0x0026F6B5
		public Fuxi()
		{
		}

		// Token: 0x06004513 RID: 17683 RVA: 0x002714BF File Offset: 0x0026F6BF
		public Fuxi(CombatSkillKey skillKey) : base(skillKey)
		{
		}

		// Token: 0x06004514 RID: 17684 RVA: 0x002714CC File Offset: 0x0026F6CC
		public override void OnEnable(DataContext context)
		{
			base.OnEnable(context);
			foreach (ushort fieldId in Fuxi.HitAvoidFieldIds)
			{
				base.CreateAffectedData(fieldId, EDataModifyType.Custom, -1);
			}
			foreach (ushort fieldId2 in Fuxi.PenetrateOrResistFieldIds)
			{
				base.CreateAffectedData(fieldId2, EDataModifyType.Custom, -1);
			}
		}

		// Token: 0x06004515 RID: 17685 RVA: 0x00271530 File Offset: 0x0026F730
		public override int GetModifiedValue(AffectedDataKey dataKey, int dataValue)
		{
			bool flag = dataKey.CharId != base.CharacterId || !base.IsCurrent;
			int result;
			if (flag)
			{
				result = dataValue;
			}
			else
			{
				int maxValue = this.GetMaxValue(dataKey.FieldId);
				result = Math.Min(dataValue, maxValue);
			}
			return result;
		}

		// Token: 0x06004516 RID: 17686 RVA: 0x00271578 File Offset: 0x0026F778
		public override long GetModifiedValue(AffectedDataKey dataKey, long dataValue)
		{
			bool flag = dataKey.CharId != base.CharacterId || !base.IsCurrent;
			long result;
			if (flag)
			{
				result = dataValue;
			}
			else
			{
				int maxValue = this.GetMaxValue(dataKey.FieldId);
				result = Math.Min(dataValue, (long)maxValue);
			}
			return result;
		}

		// Token: 0x06004517 RID: 17687 RVA: 0x002715C4 File Offset: 0x0026F7C4
		private int GetMaxValue(ushort fieldId)
		{
			Character character = base.CurrEnemyChar.GetCharacter();
			sbyte[] lifeSkillTypes = Array.Empty<sbyte>();
			bool flag = Fuxi.HitAvoidFieldIds.Contains(fieldId);
			if (flag)
			{
				lifeSkillTypes = Fuxi.HitOrAvoidLifeSkillTypes;
			}
			bool flag2 = Fuxi.PenetrateOrResistFieldIds.Contains(fieldId);
			if (flag2)
			{
				lifeSkillTypes = Fuxi.PenetrateOrResistLifeSkillTypes;
			}
			return lifeSkillTypes.Sum((sbyte x) => (int)character.GetLifeSkillAttainment(x));
		}

		// Token: 0x0400146B RID: 5227
		private static readonly sbyte[] HitOrAvoidLifeSkillTypes = new sbyte[]
		{
			0,
			1,
			2,
			3
		};

		// Token: 0x0400146C RID: 5228
		private static readonly sbyte[] PenetrateOrResistLifeSkillTypes = new sbyte[]
		{
			7,
			6,
			10,
			11
		};

		// Token: 0x0400146D RID: 5229
		private static readonly ushort[] HitAvoidFieldIds = new ushort[]
		{
			60,
			61,
			62,
			63,
			90,
			91,
			92,
			93
		};

		// Token: 0x0400146E RID: 5230
		private static readonly ushort[] PenetrateOrResistFieldIds = new ushort[]
		{
			66,
			67,
			98,
			99
		};
	}
}
