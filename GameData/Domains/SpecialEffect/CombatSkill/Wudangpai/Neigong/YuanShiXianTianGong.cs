using System;
using GameData.Combat.Math;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;

namespace GameData.Domains.SpecialEffect.CombatSkill.Wudangpai.Neigong
{
	// Token: 0x020003CD RID: 973
	public class YuanShiXianTianGong : CombatSkillEffectBase
	{
		// Token: 0x06003782 RID: 14210 RVA: 0x00235C34 File Offset: 0x00233E34
		public YuanShiXianTianGong()
		{
		}

		// Token: 0x06003783 RID: 14211 RVA: 0x00235C3E File Offset: 0x00233E3E
		public YuanShiXianTianGong(CombatSkillKey skillKey) : base(skillKey, 4008, -1)
		{
		}

		// Token: 0x06003784 RID: 14212 RVA: 0x00235C50 File Offset: 0x00233E50
		public override void OnEnable(DataContext context)
		{
			base.OnEnable(context);
			Events.RegisterHandler_CreateGangqiAfterChangeNeiliAllocation(new Events.OnCreateGangqiAfterChangeNeiliAllocation(this.OnCreateGangqiAfterChangeNeiliAllocation));
			Events.RegisterHandler_AddDirectDamageValue(new Events.OnAddDirectDamageValue(this.OnAddDirectDamageValue));
			base.CreateAffectedData(323, EDataModifyType.Custom, -1);
			base.CreateAffectedData(320, EDataModifyType.Custom, -1);
		}

		// Token: 0x06003785 RID: 14213 RVA: 0x00235CA6 File Offset: 0x00233EA6
		public override void OnDisable(DataContext context)
		{
			Events.UnRegisterHandler_CreateGangqiAfterChangeNeiliAllocation(new Events.OnCreateGangqiAfterChangeNeiliAllocation(this.OnCreateGangqiAfterChangeNeiliAllocation));
			Events.UnRegisterHandler_AddDirectDamageValue(new Events.OnAddDirectDamageValue(this.OnAddDirectDamageValue));
			base.OnDisable(context);
		}

		// Token: 0x06003786 RID: 14214 RVA: 0x00235CD8 File Offset: 0x00233ED8
		private void OnCreateGangqiAfterChangeNeiliAllocation(DataContext context, CombatCharacter character)
		{
			bool flag = character.GetId() != base.CharacterId;
			if (!flag)
			{
				int value = character.GetNeiliAllocation().Sum();
				bool flag2 = value <= 0;
				if (!flag2)
				{
					character.CreateGangqi(context, value);
					base.ShowSpecialEffectTips(0);
				}
			}
		}

		// Token: 0x06003787 RID: 14215 RVA: 0x00235D2C File Offset: 0x00233F2C
		private void OnAddDirectDamageValue(DataContext context, int attackerId, int defenderId, sbyte bodyPart, bool isInner, int damageValue, short combatSkillId)
		{
			bool flag = base.CharacterId != (base.IsDirect ? attackerId : defenderId) || (long)damageValue < 30L;
			if (!flag)
			{
				int addGangqi = (int)Math.Min((long)damageValue / 30L, 2147483647L);
				bool flag2 = addGangqi <= 0;
				if (!flag2)
				{
					base.CombatChar.ChangeGangqi(context, addGangqi);
					base.ShowSpecialEffectTipsOnceInFrame(2);
				}
			}
		}

		// Token: 0x06003788 RID: 14216 RVA: 0x00235D98 File Offset: 0x00233F98
		public override long GetModifiedValue(AffectedDataKey dataKey, long dataValue)
		{
			bool flag = dataKey.CharId != base.CharacterId || base.CombatChar.GetGangqi() <= 0 || dataKey.CustomParam0 == base.CharacterId || dataKey.FieldId != (base.IsDirect ? 320 : 323);
			long result;
			if (flag)
			{
				result = dataValue;
			}
			else
			{
				int attackerId = base.IsDirect ? dataKey.CustomParam0 : base.CharacterId;
				CombatCharacter attacker = DomainManager.Combat.GetElement_CombatCharacterDict(attackerId);
				CombatSkillKey attackerSkillKey = new CombatSkillKey(attacker.GetId(), dataKey.CombatSkillId);
				sbyte innerRatio = dataKey.IsNormalAttack ? DomainManager.Combat.GetUsingWeaponData(attacker).GetInnerRatio() : DomainManager.CombatSkill.GetElement_CombatSkills(attackerSkillKey).GetCurrInnerRatio();
				int outerRatio = (int)(100 - innerRatio);
				int ratioHalfDiff = Math.Abs((int)innerRatio - outerRatio) / 2;
				CValuePercent ratioFactor = base.IsDirect ? (50 + ratioHalfDiff) : (100 - ratioHalfDiff);
				long delta = Math.Min(dataValue * ratioFactor, (long)base.CombatChar.GetGangqi() * 20L);
				bool flag2 = delta == 0L;
				if (flag2)
				{
					result = dataValue;
				}
				else
				{
					base.ShowSpecialEffectTipsOnceInFrame(1);
					DataContext context = DomainManager.Combat.Context;
					int costGangqi = (int)(delta / 20L);
					bool flag3 = costGangqi > 0;
					if (flag3)
					{
						this.DoCostGangqi(context, costGangqi, innerRatio, outerRatio);
					}
					result = (base.IsDirect ? (dataValue - delta) : (dataValue + delta));
				}
			}
			return result;
		}

		// Token: 0x06003789 RID: 14217 RVA: 0x00235F10 File Offset: 0x00234110
		private void DoCostGangqi(DataContext context, int costGangqi, sbyte innerRatio, int outerRatio)
		{
			base.CombatChar.ChangeGangqi(context, -costGangqi);
			CombatCharacter target = base.IsDirect ? base.CombatChar : base.EnemyChar;
			int sign = base.IsDirect ? 1 : -1;
			CValuePercent changeBreathPercent = (int)innerRatio * costGangqi / 1000;
			bool flag = changeBreathPercent > 0;
			if (flag)
			{
				base.ChangeBreathValue(context, target, target.GetMaxBreathValue() * changeBreathPercent * sign);
			}
			CValuePercent changeStancePercent = outerRatio * costGangqi / 1000;
			bool flag2 = changeStancePercent > 0;
			if (flag2)
			{
				base.ChangeStanceValue(context, target, target.GetMaxStanceValue() * changeStancePercent * sign);
			}
		}

		// Token: 0x04001033 RID: 4147
		private const long DamageToGangqiFactor = 30L;

		// Token: 0x04001034 RID: 4148
		private const long GangqiToDamageFactor = 20L;

		// Token: 0x04001035 RID: 4149
		private const int GangqiToBreathOrStanceFactor = 1000;
	}
}
