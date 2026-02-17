using System;
using System.Linq;
using GameData.Combat.Math;
using GameData.Common;
using GameData.Domains.Character;

namespace GameData.Domains.SpecialEffect.Animal.Loong.Carrier
{
	// Token: 0x02000615 RID: 1557
	public class Fuxi : CarrierEffectBase
	{
		// Token: 0x0600457B RID: 17787 RVA: 0x0027278C File Offset: 0x0027098C
		public Fuxi(int charId) : base(charId)
		{
		}

		// Token: 0x170002E4 RID: 740
		// (get) Token: 0x0600457C RID: 17788 RVA: 0x00272797 File Offset: 0x00270997
		protected override short CombatStateId
		{
			get
			{
				return 206;
			}
		}

		// Token: 0x0600457D RID: 17789 RVA: 0x002727A0 File Offset: 0x002709A0
		protected override void OnEnableSubClass(DataContext context)
		{
			for (int i = 0; i < 4; i++)
			{
				base.CreateAffectedData((ushort)(32 + i), EDataModifyType.AddPercent, -1);
				base.CreateAffectedData((ushort)(38 + i), EDataModifyType.AddPercent, -1);
			}
			base.CreateAffectedData(44, EDataModifyType.AddPercent, -1);
			base.CreateAffectedData(45, EDataModifyType.AddPercent, -1);
			base.CreateAffectedData(46, EDataModifyType.AddPercent, -1);
			base.CreateAffectedData(47, EDataModifyType.AddPercent, -1);
		}

		// Token: 0x0600457E RID: 17790 RVA: 0x00272808 File Offset: 0x00270A08
		public override int GetModifyValue(AffectedDataKey dataKey, int currModifyValue)
		{
			bool flag = dataKey.CharId != base.CharacterId;
			int result;
			if (flag)
			{
				result = 0;
			}
			else
			{
				Character character = this.CharObj;
				ushort fieldId = dataKey.FieldId;
				if (!true)
				{
				}
				sbyte[] array;
				switch (fieldId)
				{
				case 32:
				case 33:
				case 34:
				case 35:
				case 38:
				case 39:
				case 40:
				case 41:
					array = Fuxi.HitOrAvoidLifeSkillTypes;
					goto IL_A2;
				case 44:
				case 45:
				case 46:
				case 47:
					array = Fuxi.PenetrateOrResistLifeSkillTypes;
					goto IL_A2;
				}
				array = Array.Empty<sbyte>();
				IL_A2:
				if (!true)
				{
				}
				sbyte[] lifeSkillTypes = array;
				result = lifeSkillTypes.Sum((sbyte x) => (int)character.GetLifeSkillAttainment(x)) / 100;
			}
			return result;
		}

		// Token: 0x0400148A RID: 5258
		private const int AttainmentDivisor = 100;

		// Token: 0x0400148B RID: 5259
		private static readonly sbyte[] HitOrAvoidLifeSkillTypes = new sbyte[]
		{
			0,
			1,
			2,
			3
		};

		// Token: 0x0400148C RID: 5260
		private static readonly sbyte[] PenetrateOrResistLifeSkillTypes = new sbyte[]
		{
			7,
			6,
			10,
			11
		};
	}
}
