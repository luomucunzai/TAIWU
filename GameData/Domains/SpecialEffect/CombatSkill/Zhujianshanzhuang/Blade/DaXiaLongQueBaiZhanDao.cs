using System;
using System.Collections.Generic;
using GameData.Combat.Math;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Character;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Zhujianshanzhuang.AttackCommon;

namespace GameData.Domains.SpecialEffect.CombatSkill.Zhujianshanzhuang.Blade
{
	// Token: 0x020001DB RID: 475
	public class DaXiaLongQueBaiZhanDao : BladeUnlockEffectBase
	{
		// Token: 0x17000174 RID: 372
		// (get) Token: 0x06002D82 RID: 11650 RVA: 0x0020BFD1 File Offset: 0x0020A1D1
		private int AddBouncePower
		{
			get
			{
				return base.IsDirectOrReverseEffectDoubling ? 80 : 40;
			}
		}

		// Token: 0x17000175 RID: 373
		// (get) Token: 0x06002D83 RID: 11651 RVA: 0x0020BFE1 File Offset: 0x0020A1E1
		private int AddFightBackPower
		{
			get
			{
				return base.IsDirectOrReverseEffectDoubling ? 200 : 100;
			}
		}

		// Token: 0x17000176 RID: 374
		// (get) Token: 0x06002D84 RID: 11652 RVA: 0x0020BFF4 File Offset: 0x0020A1F4
		private bool CastingSelf
		{
			get
			{
				return base.CombatChar.GetPreparingSkillId() == base.SkillTemplateId || base.CombatChar.GetPerformingSkillId() == base.SkillTemplateId;
			}
		}

		// Token: 0x17000177 RID: 375
		// (get) Token: 0x06002D85 RID: 11653 RVA: 0x0020C020 File Offset: 0x0020A220
		protected override IEnumerable<short> RequireWeaponTypes
		{
			get
			{
				yield return 4;
				yield break;
			}
		}

		// Token: 0x06002D86 RID: 11654 RVA: 0x0020C03F File Offset: 0x0020A23F
		public DaXiaLongQueBaiZhanDao()
		{
		}

		// Token: 0x06002D87 RID: 11655 RVA: 0x0020C049 File Offset: 0x0020A249
		public DaXiaLongQueBaiZhanDao(CombatSkillKey skillKey) : base(skillKey, 9202)
		{
		}

		// Token: 0x06002D88 RID: 11656 RVA: 0x0020C05C File Offset: 0x0020A25C
		public override void OnEnable(DataContext context)
		{
			base.OnEnable(context);
			base.CreateAffectedData(111, EDataModifyType.Add, -1);
			base.CreateAffectedData(177, EDataModifyType.Custom, -1);
			base.CreateAffectedData(112, EDataModifyType.Add, -1);
			base.CreateAffectedData(193, EDataModifyType.Custom, -1);
			Events.RegisterHandler_CastAttackSkillBegin(new Events.OnCastAttackSkillBegin(this.OnCastAttackSkillBegin));
		}

		// Token: 0x06002D89 RID: 11657 RVA: 0x0020C0B6 File Offset: 0x0020A2B6
		public override void OnDisable(DataContext context)
		{
			Events.UnRegisterHandler_CastAttackSkillBegin(new Events.OnCastAttackSkillBegin(this.OnCastAttackSkillBegin));
			base.OnDisable(context);
		}

		// Token: 0x06002D8A RID: 11658 RVA: 0x0020C0D4 File Offset: 0x0020A2D4
		private void OnCastAttackSkillBegin(DataContext context, CombatCharacter attacker, CombatCharacter defender, short skillId)
		{
			bool flag = this.SkillKey.IsMatch(attacker.GetId(), skillId) && base.IsReverseOrUsingDirectWeapon;
			if (flag)
			{
				base.ShowSpecialEffectTips(base.IsDirect, 1, 0);
			}
		}

		// Token: 0x06002D8B RID: 11659 RVA: 0x0020C114 File Offset: 0x0020A314
		public override void DoAffectAfterCost(DataContext context, int weaponIndex)
		{
			short center = base.CombatChar.GetAttackRange().Average;
			short current = DomainManager.Combat.GetCurrentDistance();
			int addDistance = Math.Min(Math.Abs((int)(center - current)), 60);
			bool flag = addDistance > 0;
			if (flag)
			{
				DomainManager.Combat.ChangeDistance(context, base.EnemyChar, addDistance * ((current < center) ? 1 : -1), true);
				base.ShowSpecialEffectTips(base.IsDirect, 2, 1);
			}
			int flawCount = 1 + addDistance / 5;
			if (!true)
			{
			}
			sbyte b;
			if (addDistance > 10)
			{
				if (addDistance >= 20)
				{
					b = 2;
				}
				else
				{
					b = 1;
				}
			}
			else
			{
				b = 0;
			}
			if (!true)
			{
			}
			sbyte flawLevel = b;
			DomainManager.Combat.AddFlaw(context, base.EnemyChar, flawLevel, this.SkillKey, -1, flawCount, true);
			base.ShowSpecialEffectTips(base.IsDirect, 3, 2);
		}

		// Token: 0x06002D8C RID: 11660 RVA: 0x0020C1E4 File Offset: 0x0020A3E4
		public override int GetModifyValue(AffectedDataKey dataKey, int currModifyValue)
		{
			bool flag = dataKey.CharId != base.CharacterId || !this.CastingSelf || !base.IsReverseOrUsingDirectWeapon;
			int result;
			if (flag)
			{
				result = 0;
			}
			else
			{
				ushort fieldId = dataKey.FieldId;
				if (!true)
				{
				}
				int num;
				if (fieldId != 111)
				{
					if (fieldId != 112)
					{
						num = 0;
					}
					else
					{
						num = this.AddFightBackPower;
					}
				}
				else
				{
					num = this.AddBouncePower;
				}
				if (!true)
				{
				}
				result = num;
			}
			return result;
		}

		// Token: 0x06002D8D RID: 11661 RVA: 0x0020C254 File Offset: 0x0020A454
		public override OuterAndInnerInts GetModifiedValue(AffectedDataKey dataKey, OuterAndInnerInts dataValue)
		{
			bool flag = dataKey.CharId != base.CharacterId || !this.CastingSelf || !base.IsReverseOrUsingDirectWeapon;
			OuterAndInnerInts result;
			if (flag)
			{
				result = dataValue;
			}
			else
			{
				bool flag2 = dataKey.FieldId != 177;
				if (flag2)
				{
					result = dataValue;
				}
				else
				{
					OuterAndInnerInts skillRange = DomainManager.Combat.GetSkillAttackRange(base.CombatChar, base.SkillTemplateId);
					dataValue.Outer = Math.Min(dataValue.Outer, skillRange.Outer);
					dataValue.Inner = Math.Max(dataValue.Inner, skillRange.Inner);
					result = dataValue;
				}
			}
			return result;
		}

		// Token: 0x06002D8E RID: 11662 RVA: 0x0020C2F0 File Offset: 0x0020A4F0
		public override bool GetModifiedValue(AffectedDataKey dataKey, bool dataValue)
		{
			bool flag = dataKey.CharId != base.CharacterId || !this.CastingSelf || !base.IsReverseOrUsingDirectWeapon;
			bool result;
			if (flag)
			{
				result = dataValue;
			}
			else
			{
				bool flag2 = dataKey.FieldId != 193;
				result = (!flag2 || dataValue);
			}
			return result;
		}

		// Token: 0x04000DAC RID: 3500
		private const int MaxChangeDistance = 60;

		// Token: 0x04000DAD RID: 3501
		private const int ExtraFlawDistance = 5;
	}
}
