using System;
using System.Collections.Generic;
using Config;
using GameData.Combat.Math;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;
using GameData.Utilities;

namespace GameData.Domains.SpecialEffect.CombatSkill.Ranshanpai.Sword
{
	// Token: 0x02000447 RID: 1095
	public class WanHuaShiSiJian : CombatSkillEffectBase
	{
		// Token: 0x06003A2C RID: 14892 RVA: 0x002423B6 File Offset: 0x002405B6
		public WanHuaShiSiJian()
		{
		}

		// Token: 0x06003A2D RID: 14893 RVA: 0x002423C0 File Offset: 0x002405C0
		public WanHuaShiSiJian(CombatSkillKey skillKey) : base(skillKey, 7208, -1)
		{
		}

		// Token: 0x06003A2E RID: 14894 RVA: 0x002423D4 File Offset: 0x002405D4
		public override void OnEnable(DataContext context)
		{
			this._copyNotLearnSkillKey.SkillTemplateId = -1;
			this._castingCopySkill = -1;
			base.CreateAffectedData(209, EDataModifyType.Custom, -1);
			base.CreateAffectedData(base.IsDirect ? 280 : 284, EDataModifyType.Custom, -1);
			Events.RegisterHandler_PrepareSkillBegin(new Events.OnPrepareSkillBegin(this.OnPrepareSkillBegin));
			Events.RegisterHandler_AttackSkillAttackEnd(new Events.OnAttackSkillAttackEnd(this.OnAttackSkillAttackEnd));
			Events.RegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
		}

		// Token: 0x06003A2F RID: 14895 RVA: 0x00242458 File Offset: 0x00240658
		public override void OnDisable(DataContext context)
		{
			this.ClearCopySkillData(context);
			Events.UnRegisterHandler_PrepareSkillBegin(new Events.OnPrepareSkillBegin(this.OnPrepareSkillBegin));
			Events.UnRegisterHandler_AttackSkillAttackEnd(new Events.OnAttackSkillAttackEnd(this.OnAttackSkillAttackEnd));
			Events.UnRegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
		}

		// Token: 0x06003A30 RID: 14896 RVA: 0x002424A4 File Offset: 0x002406A4
		private void OnPrepareSkillBegin(DataContext context, int charId, bool isAlly, short skillId)
		{
			bool flag;
			if (base.CombatChar.IsAlly != isAlly && base.EffectCount > 0)
			{
				if (base.CombatChar.GetWeaponTricks().Exist((sbyte trick) => WanHuaShiSiJian.RequireTricks.Exist(trick)) && base.CombatChar.GetPreparingSkillId() < 0)
				{
					flag = (base.CombatChar.NeedUseSkillId >= 0);
					goto IL_6B;
				}
			}
			flag = true;
			IL_6B:
			bool flag2 = flag;
			if (!flag2)
			{
				CombatSkillItem skillConfig = Config.CombatSkill.Instance[skillId];
				bool flag3;
				if (skillConfig.EquipType == 1)
				{
					flag3 = !skillConfig.TrickCost.Exists((NeedTrick needTrick) => WanHuaShiSiJian.RequireTricks.Exist(needTrick.TrickType));
				}
				else
				{
					flag3 = true;
				}
				bool flag4 = flag3;
				if (!flag4)
				{
					GameData.Domains.CombatSkill.CombatSkill combatSkill;
					bool flag5 = !DomainManager.CombatSkill.TryGetElement_CombatSkills(new CombatSkillKey(base.CharacterId, skillId), out combatSkill);
					if (flag5)
					{
						List<short> learnedSkills = base.CombatChar.GetCharacter().GetLearnedCombatSkills();
						this._copyNotLearnSkillKey = DomainManager.CombatSkill.CreateCombatSkill(base.CharacterId, skillId, 0).GetId();
						learnedSkills.Add(skillId);
						base.CombatChar.GetCharacter().SetLearnedCombatSkills(learnedSkills, context);
					}
					bool flag6 = !DomainManager.Combat.CanCastSkill(base.CombatChar, skillId, true, false);
					if (flag6)
					{
						this.ClearCopySkillData(context);
					}
					else
					{
						this._castingCopySkill = skillId;
						DomainManager.SpecialEffect.InvalidateCache(context, base.CharacterId, 209);
						bool flag7 = skillId != base.SkillTemplateId;
						if (flag7)
						{
							DomainManager.Combat.SetSkillPowerReplaceInCombat(context, new CombatSkillKey(base.CharacterId, skillId), this.SkillKey);
						}
						DomainManager.Combat.CastSkillFree(context, base.CombatChar, skillId, ECombatCastFreePriority.Normal);
						base.ShowSpecialEffectTips(1);
						base.ReduceEffectCount(1);
					}
				}
			}
		}

		// Token: 0x06003A31 RID: 14897 RVA: 0x00242684 File Offset: 0x00240884
		private void OnAttackSkillAttackEnd(CombatContext context, sbyte hitType, bool hit, int index)
		{
			bool flag = context.Attacker != base.CombatChar || this._castingCopySkill < 0 || index < 3;
			if (!flag)
			{
				bool flag2 = context.Attacker.GetAttackSkillPower() <= 0 || base.CurrEnemyChar.GetPreparingSkillId() != this._castingCopySkill;
				if (!flag2)
				{
					bool flag3 = !DomainManager.Combat.InterruptSkill(context, base.CurrEnemyChar, 100);
					if (!flag3)
					{
						base.CurrEnemyChar.SetAnimationToPlayOnce(DomainManager.Combat.GetHittedAni(base.CurrEnemyChar, 2), context);
						DomainManager.Combat.SetProperLoopAniAndParticle(context, base.CurrEnemyChar, false);
						DomainManager.Combat.AddSkillPowerInCombat(context, this.SkillKey, base.EffectKey, 20);
						base.ShowSpecialEffectTips(2);
					}
				}
			}
		}

		// Token: 0x06003A32 RID: 14898 RVA: 0x00242770 File Offset: 0x00240970
		private void OnCastSkillEnd(DataContext context, int charId, bool isAlly, short skillId, sbyte power, bool interrupted)
		{
			bool flag = charId != base.CharacterId;
			if (!flag)
			{
				bool repeatCheck = this._castingCopySkill == base.SkillTemplateId;
				this.ClearCopySkillData(context);
				bool flag2 = repeatCheck || skillId != base.SkillTemplateId || base.CombatChar.GetAutoCastingSkill() || !base.PowerMatchAffectRequire((int)power, 0);
				if (!flag2)
				{
					base.AddMaxEffectCount(true);
				}
			}
		}

		// Token: 0x06003A33 RID: 14899 RVA: 0x002427E0 File Offset: 0x002409E0
		private void ClearCopySkillData(DataContext context)
		{
			bool flag = this._copyNotLearnSkillKey.SkillTemplateId >= 0;
			if (flag)
			{
				List<short> learnedSkills = base.CombatChar.GetCharacter().GetLearnedCombatSkills();
				learnedSkills.Remove(this._copyNotLearnSkillKey.SkillTemplateId);
				base.CombatChar.GetCharacter().SetLearnedCombatSkills(learnedSkills, context);
				DomainManager.CombatSkill.RemoveCombatSkill(this._copyNotLearnSkillKey.CharId, this._copyNotLearnSkillKey.SkillTemplateId);
				this._copyNotLearnSkillKey.SkillTemplateId = -1;
			}
			bool flag2 = this._castingCopySkill > 0;
			if (flag2)
			{
				bool flag3 = this._castingCopySkill != base.SkillTemplateId;
				if (flag3)
				{
					DomainManager.Combat.RemoveSkillPowerReplaceInCombat(context, new CombatSkillKey(base.CharacterId, this._castingCopySkill));
				}
				this._castingCopySkill = -1;
				DomainManager.SpecialEffect.InvalidateCache(context, base.CharacterId, 209);
			}
		}

		// Token: 0x06003A34 RID: 14900 RVA: 0x002428C8 File Offset: 0x00240AC8
		public override int GetModifiedValue(AffectedDataKey dataKey, int dataValue)
		{
			bool flag = dataKey.CharId != base.CharacterId || this._castingCopySkill != dataKey.CombatSkillId;
			int result;
			if (flag)
			{
				result = dataValue;
			}
			else
			{
				bool flag2 = dataKey.FieldId == 209;
				if (flag2)
				{
					result = -1;
				}
				else
				{
					result = dataValue;
				}
			}
			return result;
		}

		// Token: 0x06003A35 RID: 14901 RVA: 0x0024291C File Offset: 0x00240B1C
		public override bool GetModifiedValue(AffectedDataKey dataKey, bool dataValue)
		{
			bool flag = dataKey.SkillKey != this.SkillKey;
			bool flag2 = flag;
			if (!flag2)
			{
				ushort fieldId = dataKey.FieldId;
				bool flag3 = fieldId == 280 || fieldId == 284;
				flag2 = !flag3;
			}
			bool flag4 = flag2;
			bool result;
			if (flag4)
			{
				result = dataValue;
			}
			else
			{
				bool trulyCost = dataKey.CustomParam0 == 1;
				bool flag5 = trulyCost;
				if (flag5)
				{
					base.ShowSpecialEffectTips(0);
				}
				result = true;
			}
			return result;
		}

		// Token: 0x04001105 RID: 4357
		private const int AddPower = 20;

		// Token: 0x04001106 RID: 4358
		private static readonly sbyte[] RequireTricks = new sbyte[]
		{
			3,
			4,
			5
		};

		// Token: 0x04001107 RID: 4359
		private CombatSkillKey _copyNotLearnSkillKey;

		// Token: 0x04001108 RID: 4360
		private short _castingCopySkill;
	}
}
