using System;
using System.Collections.Generic;
using Config;
using GameData.Combat.Math;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Character;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;

namespace GameData.Domains.SpecialEffect.CombatSkill.Fulongtan.Blade
{
	// Token: 0x02000530 RID: 1328
	public class LiuXinSiYiDao : CombatSkillEffectBase
	{
		// Token: 0x06003F84 RID: 16260 RVA: 0x0025A41E File Offset: 0x0025861E
		public LiuXinSiYiDao()
		{
		}

		// Token: 0x06003F85 RID: 16261 RVA: 0x0025A428 File Offset: 0x00258628
		public LiuXinSiYiDao(CombatSkillKey skillKey) : base(skillKey, 14203, -1)
		{
		}

		// Token: 0x06003F86 RID: 16262 RVA: 0x0025A43C File Offset: 0x0025863C
		public override void OnEnable(DataContext context)
		{
			bool isDirect = base.IsDirect;
			if (isDirect)
			{
				base.CreateAffectedData(199, EDataModifyType.Add, -1);
			}
			else
			{
				base.CreateAffectedAllEnemyData(199, EDataModifyType.Add, -1);
			}
			Events.RegisterHandler_PrepareSkillBegin(new Events.OnPrepareSkillBegin(this.OnPrepareSkillBegin));
			Events.RegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
		}

		// Token: 0x06003F87 RID: 16263 RVA: 0x0025A496 File Offset: 0x00258696
		public override void OnDisable(DataContext context)
		{
			Events.UnRegisterHandler_PrepareSkillBegin(new Events.OnPrepareSkillBegin(this.OnPrepareSkillBegin));
			Events.UnRegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
		}

		// Token: 0x06003F88 RID: 16264 RVA: 0x0025A4C0 File Offset: 0x002586C0
		private unsafe void OnPrepareSkillBegin(DataContext context, int charId, bool isAlly, short skillId)
		{
			bool flag = Config.CombatSkill.Instance[skillId].EquipType != 1 || base.EffectCount <= 0;
			if (!flag)
			{
				bool flag2 = base.IsDirect ? (charId != base.CharacterId) : (isAlly == base.CombatChar.IsAlly);
				if (!flag2)
				{
					Dictionary<CombatSkillKey, SkillPowerChangeCollection> powerAddDict = base.IsDirect ? DomainManager.Combat.GetAllSkillPowerAddInCombat() : DomainManager.Combat.GetAllSkillPowerReduceInCombat();
					List<short> skillList = DomainManager.Combat.GetElement_CombatCharacterDict(charId).GetAttackSkillList();
					bool isDrunk = (*this.CharObj.GetEatingItems()).ContainsWine();
					this._changePower = 0;
					for (int i = 0; i < skillList.Count; i++)
					{
						short attackSkillId = skillList[i];
						bool flag3 = attackSkillId < 0 || attackSkillId == skillId;
						if (!flag3)
						{
							CombatSkillKey skillKey = new CombatSkillKey(charId, attackSkillId);
							bool flag4 = !powerAddDict.ContainsKey(skillKey);
							if (!flag4)
							{
								int power = powerAddDict[skillKey].GetTotalChangeValue() / (isDrunk ? 1 : 2);
								bool flag5 = base.IsDirect ? (power > this._changePower) : (power < this._changePower);
								if (flag5)
								{
									this._changePower = power;
								}
							}
						}
					}
					bool flag6 = this._changePower == 0;
					if (!flag6)
					{
						bool isValid = this._affectingSkillKey.IsValid;
						if (isValid)
						{
							this.ResetAffectingSkillKey(context);
						}
						this._affectingSkillKey = new CombatSkillKey(charId, skillId);
						base.InvalidateCache(context, charId, 199);
						base.ShowSpecialEffectTips(0);
					}
				}
			}
		}

		// Token: 0x06003F89 RID: 16265 RVA: 0x0025A668 File Offset: 0x00258868
		private void OnCastSkillEnd(DataContext context, int charId, bool isAlly, short skillId, sbyte power, bool interrupted)
		{
			bool flag = this._affectingSkillKey.IsMatch(charId, skillId);
			if (flag)
			{
				this.ResetAffectingSkillKey(context);
				base.ReduceEffectCount(1);
			}
			bool flag2 = charId == base.CharacterId && skillId == base.SkillTemplateId && base.PowerMatchAffectRequire((int)power, 0);
			if (flag2)
			{
				base.AddMaxEffectCount(true);
			}
		}

		// Token: 0x06003F8A RID: 16266 RVA: 0x0025A6C8 File Offset: 0x002588C8
		private void ResetAffectingSkillKey(DataContext context)
		{
			bool flag = !this._affectingSkillKey.IsValid;
			if (!flag)
			{
				int charId = this._affectingSkillKey.CharId;
				this._affectingSkillKey = CombatSkillKey.Invalid;
				base.InvalidateCache(context, charId, 199);
			}
		}

		// Token: 0x06003F8B RID: 16267 RVA: 0x0025A710 File Offset: 0x00258910
		public override int GetModifyValue(AffectedDataKey dataKey, int currModifyValue)
		{
			bool flag = dataKey.SkillKey != this._affectingSkillKey;
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
					result = this._changePower;
				}
				else
				{
					result = 0;
				}
			}
			return result;
		}

		// Token: 0x040012B8 RID: 4792
		private CombatSkillKey _affectingSkillKey;

		// Token: 0x040012B9 RID: 4793
		private int _changePower;
	}
}
