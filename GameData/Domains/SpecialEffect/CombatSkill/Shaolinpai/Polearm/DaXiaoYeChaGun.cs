using System;
using GameData.Combat.Math;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Character;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;

namespace GameData.Domains.SpecialEffect.CombatSkill.Shaolinpai.Polearm
{
	// Token: 0x02000412 RID: 1042
	public class DaXiaoYeChaGun : CombatSkillEffectBase
	{
		// Token: 0x0600391E RID: 14622 RVA: 0x0023D4EF File Offset: 0x0023B6EF
		public DaXiaoYeChaGun()
		{
		}

		// Token: 0x0600391F RID: 14623 RVA: 0x0023D4F9 File Offset: 0x0023B6F9
		public DaXiaoYeChaGun(CombatSkillKey skillKey) : base(skillKey, 1303, -1)
		{
		}

		// Token: 0x06003920 RID: 14624 RVA: 0x0023D50C File Offset: 0x0023B70C
		public override void OnEnable(DataContext context)
		{
			this._addPower = 0;
			base.CreateAffectedData(199, EDataModifyType.AddPercent, base.SkillTemplateId);
			base.CreateAffectedData(base.IsDirect ? 146 : 145, EDataModifyType.Add, base.SkillTemplateId);
			Events.RegisterHandler_CastAttackSkillBegin(new Events.OnCastAttackSkillBegin(this.OnCastAttackSkillBegin));
			Events.RegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
		}

		// Token: 0x06003921 RID: 14625 RVA: 0x0023D57A File Offset: 0x0023B77A
		public override void OnDisable(DataContext context)
		{
			Events.UnRegisterHandler_CastAttackSkillBegin(new Events.OnCastAttackSkillBegin(this.OnCastAttackSkillBegin));
			Events.UnRegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
		}

		// Token: 0x06003922 RID: 14626 RVA: 0x0023D5A4 File Offset: 0x0023B7A4
		private int CalcAddPower()
		{
			short currDist = DomainManager.Combat.GetCurrentDistance();
			OuterAndInnerShorts attackRange = base.CombatChar.GetAttackRange();
			attackRange.Outer = Math.Max(attackRange.Outer, 20);
			attackRange.Inner = Math.Min(attackRange.Inner, 120);
			bool flag = attackRange.Outer > currDist || attackRange.Inner < currDist;
			int result;
			if (flag)
			{
				result = 0;
			}
			else
			{
				int noPowerDist = (int)((attackRange.Outer + attackRange.Inner) / 2);
				int power = 40 * (100 - (base.IsDirect ? ((int)((attackRange.Inner - currDist) * 100) / ((int)attackRange.Inner - noPowerDist)) : ((int)((currDist - attackRange.Outer) * 100) / (noPowerDist - (int)attackRange.Outer)))) / 100;
				result = Math.Max(power, 0);
			}
			return result;
		}

		// Token: 0x06003923 RID: 14627 RVA: 0x0023D66C File Offset: 0x0023B86C
		private void OnCastAttackSkillBegin(DataContext context, CombatCharacter attacker, CombatCharacter defender, short skillId)
		{
			bool flag = !this.SkillKey.IsMatch(attacker.GetId(), skillId);
			if (!flag)
			{
				this._addPower = this.CalcAddPower();
				bool flag2 = this._addPower <= 0;
				if (!flag2)
				{
					DomainManager.SpecialEffect.InvalidateCache(context, base.CharacterId, 199);
					base.ShowSpecialEffectTips(0);
				}
			}
		}

		// Token: 0x06003924 RID: 14628 RVA: 0x0023D6D4 File Offset: 0x0023B8D4
		private void OnCastSkillEnd(DataContext context, int charId, bool isAlly, short skillId, sbyte power, bool interrupted)
		{
			bool flag = charId != base.CharacterId || skillId != base.SkillTemplateId;
			if (!flag)
			{
				this._addPower = 0;
				DomainManager.SpecialEffect.InvalidateCache(context, base.CharacterId, 199);
			}
		}

		// Token: 0x06003925 RID: 14629 RVA: 0x0023D720 File Offset: 0x0023B920
		public override int GetModifyValue(AffectedDataKey dataKey, int currModifyValue)
		{
			bool flag = dataKey.SkillKey != this.SkillKey;
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
				if (fieldId - 145 > 1)
				{
					if (fieldId != 199)
					{
						num = 0;
					}
					else
					{
						num = this._addPower;
					}
				}
				else
				{
					num = 20;
				}
				if (!true)
				{
				}
				result = num;
			}
			return result;
		}

		// Token: 0x040010B6 RID: 4278
		private const sbyte AddRange = 20;

		// Token: 0x040010B7 RID: 4279
		private const sbyte MaxAddPower = 40;

		// Token: 0x040010B8 RID: 4280
		private int _addPower;
	}
}
