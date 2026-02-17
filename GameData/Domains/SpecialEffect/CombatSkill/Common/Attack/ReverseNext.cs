using System;
using System.Collections.Generic;
using Config;
using GameData.Combat.Math;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;

namespace GameData.Domains.SpecialEffect.CombatSkill.Common.Attack
{
	// Token: 0x020005A4 RID: 1444
	public class ReverseNext : CombatSkillEffectBase
	{
		// Token: 0x060042E6 RID: 17126 RVA: 0x00268B64 File Offset: 0x00266D64
		protected ReverseNext()
		{
		}

		// Token: 0x060042E7 RID: 17127 RVA: 0x00268B79 File Offset: 0x00266D79
		protected ReverseNext(CombatSkillKey skillKey, int type) : base(skillKey, type, -1)
		{
		}

		// Token: 0x060042E8 RID: 17128 RVA: 0x00268B94 File Offset: 0x00266D94
		public override void OnEnable(DataContext context)
		{
			base.CreateAffectedData(209, EDataModifyType.Custom, -1);
			base.CreateAffectedData(199, EDataModifyType.AddPercent, -1);
			Events.RegisterHandler_PrepareSkillEffectNotYetCreated(new Events.OnPrepareSkillEffectNotYetCreated(this.OnPrepareSkillEffectNotYetCreated));
			Events.RegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
			Events.RegisterHandler_CastSkillAllEnd(new Events.OnCastSkillAllEnd(this.OnCastSkillAllEnd));
		}

		// Token: 0x060042E9 RID: 17129 RVA: 0x00268BF4 File Offset: 0x00266DF4
		public override void OnDisable(DataContext context)
		{
			Events.UnRegisterHandler_PrepareSkillEffectNotYetCreated(new Events.OnPrepareSkillEffectNotYetCreated(this.OnPrepareSkillEffectNotYetCreated));
			Events.UnRegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
			Events.UnRegisterHandler_CastSkillAllEnd(new Events.OnCastSkillAllEnd(this.OnCastSkillAllEnd));
		}

		// Token: 0x060042EA RID: 17130 RVA: 0x00268C30 File Offset: 0x00266E30
		private void OnPrepareSkillEffectNotYetCreated(DataContext context, CombatCharacter combatChar, short skillId)
		{
			bool flag = combatChar.GetId() != base.CharacterId || base.CombatChar.GetAutoCastingSkill() || base.EffectCount <= 0;
			if (!flag)
			{
				CombatSkillItem skillConfig = Config.CombatSkill.Instance[skillId];
				bool flag2 = skillConfig.SectId != this.AffectSectId || skillConfig.Type != this.AffectSkillType;
				if (!flag2)
				{
					CombatSkillKey skillKey = new CombatSkillKey(base.CharacterId, skillId);
					GameData.Domains.CombatSkill.CombatSkill skill = DomainManager.CombatSkill.GetElement_CombatSkills(skillKey);
					sbyte direction = skill.GetDirection();
					bool flag3 = direction != (base.IsDirect ? 1 : 0);
					if (!flag3)
					{
						bool flag4 = this._reversedSkillList.Contains(skillId);
						if (!flag4)
						{
							this._reversedSkillList.Add(skillId);
							base.ReduceEffectCount(1);
							base.ShowSpecialEffectTips(0);
							base.InvalidateCache(context, 209);
							short effectTemplateId = (short)((direction == 0) ? skillConfig.DirectEffectID : skillConfig.ReverseEffectID);
							SpecialEffectItem effectConfig = SpecialEffect.Instance[effectTemplateId];
							bool flag5 = effectConfig.EffectActiveType != 1;
							if (!flag5)
							{
								CombatSkillEffectBase skillEffect = (CombatSkillEffectBase)DomainManager.SpecialEffect.Get(skill.GetSpecialEffectId());
								skillEffect.SetIsDirect(context, base.IsDirect);
								DomainManager.Combat.ChangeSkillEffectDirection(context, base.CombatChar, new SkillEffectKey(skillId, !base.IsDirect), base.IsDirect);
							}
						}
					}
				}
			}
		}

		// Token: 0x060042EB RID: 17131 RVA: 0x00268DB0 File Offset: 0x00266FB0
		private void OnCastSkillEnd(DataContext context, int charId, bool isAlly, short skillId, sbyte power, bool interrupted)
		{
			bool flag = !this.SkillKey.IsMatch(charId, skillId) || !base.PowerMatchAffectRequire((int)power, 0);
			if (!flag)
			{
				base.AddMaxEffectCount(true);
			}
		}

		// Token: 0x060042EC RID: 17132 RVA: 0x00268DEC File Offset: 0x00266FEC
		private void OnCastSkillAllEnd(DataContext context, int charId, short skillId)
		{
			bool flag = charId != base.CharacterId || !this._reversedSkillList.Contains(skillId);
			if (!flag)
			{
				this._reversedSkillList.Remove(skillId);
				base.InvalidateCache(context, 209);
				CombatSkillKey skillKey = new CombatSkillKey(base.CharacterId, skillId);
				CombatSkillItem skillConfig = Config.CombatSkill.Instance[skillId];
				GameData.Domains.CombatSkill.CombatSkill skill = DomainManager.CombatSkill.GetElement_CombatSkills(skillKey);
				sbyte direction = skill.GetDirection();
				short effectTemplateId = (short)((direction == 0) ? skillConfig.DirectEffectID : skillConfig.ReverseEffectID);
				SpecialEffectItem effectConfig = SpecialEffect.Instance[effectTemplateId];
				bool flag2 = effectConfig.EffectActiveType != 1;
				if (!flag2)
				{
					CombatSkillEffectBase skillEffect = (CombatSkillEffectBase)DomainManager.SpecialEffect.Get(skill.GetSpecialEffectId());
					bool flag3 = skillEffect == null;
					if (!flag3)
					{
						skillEffect.SetIsDirect(context, !base.IsDirect);
						DomainManager.Combat.ChangeSkillEffectDirection(context, base.CombatChar, new SkillEffectKey(skillId, base.IsDirect), !base.IsDirect);
					}
				}
			}
		}

		// Token: 0x060042ED RID: 17133 RVA: 0x00268EFC File Offset: 0x002670FC
		public override int GetModifiedValue(AffectedDataKey dataKey, int dataValue)
		{
			bool flag = dataKey.CharId != base.CharacterId || !this._reversedSkillList.Contains(dataKey.CombatSkillId);
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
					result = (base.IsDirect ? 0 : 1);
				}
				else
				{
					result = dataValue;
				}
			}
			return result;
		}

		// Token: 0x060042EE RID: 17134 RVA: 0x00268F5C File Offset: 0x0026715C
		public override int GetModifyValue(AffectedDataKey dataKey, int currModifyValue)
		{
			bool flag = dataKey.CharId != base.CharacterId || !this._reversedSkillList.Contains(dataKey.CombatSkillId);
			int result;
			if (flag)
			{
				result = 0;
			}
			else
			{
				bool flag2 = dataKey.FieldId == 199;
				if (flag2)
				{
					result = 40;
				}
				else
				{
					result = 0;
				}
			}
			return result;
		}

		// Token: 0x040013CA RID: 5066
		private const sbyte AddPower = 40;

		// Token: 0x040013CB RID: 5067
		protected sbyte AffectSectId;

		// Token: 0x040013CC RID: 5068
		protected sbyte AffectSkillType;

		// Token: 0x040013CD RID: 5069
		private readonly List<short> _reversedSkillList = new List<short>();
	}
}
