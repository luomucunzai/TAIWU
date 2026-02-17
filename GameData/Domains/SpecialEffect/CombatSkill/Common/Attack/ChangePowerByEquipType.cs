using System;
using System.Collections.Generic;
using Config;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;
using GameData.Utilities;

namespace GameData.Domains.SpecialEffect.CombatSkill.Common.Attack
{
	// Token: 0x02000598 RID: 1432
	public class ChangePowerByEquipType : CombatSkillEffectBase
	{
		// Token: 0x17000293 RID: 659
		// (get) Token: 0x0600427E RID: 17022 RVA: 0x002672CF File Offset: 0x002654CF
		protected virtual sbyte ChangePowerUnitDirect
		{
			get
			{
				return 2;
			}
		}

		// Token: 0x17000294 RID: 660
		// (get) Token: 0x0600427F RID: 17023 RVA: 0x002672D2 File Offset: 0x002654D2
		protected virtual sbyte ChangePowerUnitReverse
		{
			get
			{
				return 2;
			}
		}

		// Token: 0x06004280 RID: 17024 RVA: 0x002672D5 File Offset: 0x002654D5
		protected ChangePowerByEquipType()
		{
		}

		// Token: 0x06004281 RID: 17025 RVA: 0x002672DF File Offset: 0x002654DF
		protected ChangePowerByEquipType(CombatSkillKey skillKey, int type) : base(skillKey, type, -1)
		{
		}

		// Token: 0x06004282 RID: 17026 RVA: 0x002672EC File Offset: 0x002654EC
		public override void OnEnable(DataContext context)
		{
			Events.RegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
		}

		// Token: 0x06004283 RID: 17027 RVA: 0x00267301 File Offset: 0x00265501
		public override void OnDisable(DataContext context)
		{
			Events.UnRegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
		}

		// Token: 0x06004284 RID: 17028 RVA: 0x00267318 File Offset: 0x00265518
		private void OnCastSkillEnd(DataContext context, int charId, bool isAlly, short skillId, sbyte power, bool interrupted)
		{
			bool flag = charId != base.CharacterId || skillId != base.SkillTemplateId;
			if (!flag)
			{
				sbyte unit = base.IsDirect ? this.ChangePowerUnitDirect : this.ChangePowerUnitReverse;
				int powerChangeValue = (int)(power / 10 * (base.IsDirect ? unit : (-unit)));
				bool flag2 = powerChangeValue != 0;
				if (flag2)
				{
					SkillEffectKey effectKey = new SkillEffectKey(base.SkillTemplateId, base.IsDirect);
					CombatCharacter combatChar = base.IsDirect ? base.CombatChar : DomainManager.Combat.GetCombatCharacter(!base.CombatChar.IsAlly, true);
					bool anyChanged = false;
					List<short> affectCombatSkills = ObjectPool<List<short>>.Instance.Get();
					affectCombatSkills.Clear();
					bool flag3 = combatChar.BossConfig == null;
					if (flag3)
					{
						affectCombatSkills.AddRange(combatChar.GetCombatSkillList(this.AffectEquipType));
					}
					else
					{
						affectCombatSkills.AddRange(combatChar.GetCharacter().GetLearnedCombatSkills().FindAll((short id) => Config.CombatSkill.Instance[id].EquipType == this.AffectEquipType));
					}
					foreach (short combatSkillId in affectCombatSkills)
					{
						bool flag4 = combatSkillId >= 0;
						if (flag4)
						{
							CombatSkillKey skillKey = new CombatSkillKey(combatChar.GetId(), combatSkillId);
							bool isDirect = base.IsDirect;
							SkillPowerChangeCollection powerChangeCollection;
							if (isDirect)
							{
								DomainManager.Combat.TryGetElement_SkillPowerAddInCombat(skillKey, out powerChangeCollection);
							}
							else
							{
								DomainManager.Combat.TryGetElement_SkillPowerReduceInCombat(skillKey, out powerChangeCollection);
							}
							int currChangeValue = (powerChangeCollection != null && powerChangeCollection.EffectDict.ContainsKey(effectKey)) ? powerChangeCollection.EffectDict[effectKey] : 0;
							int diff = powerChangeValue - currChangeValue;
							bool flag5 = base.IsDirect ? (diff > 0) : (diff < 0);
							if (flag5)
							{
								bool isDirect2 = base.IsDirect;
								if (isDirect2)
								{
									DomainManager.Combat.AddSkillPowerInCombat(context, skillKey, effectKey, diff);
								}
								else
								{
									DomainManager.Combat.ReduceSkillPowerInCombat(context, skillKey, effectKey, diff);
								}
								anyChanged = true;
							}
						}
					}
					bool flag6 = anyChanged;
					if (flag6)
					{
						base.ShowSpecialEffectTips(0);
					}
					ObjectPool<List<short>>.Instance.Return(affectCombatSkills);
				}
				base.RemoveSelf(context);
			}
		}

		// Token: 0x040013A8 RID: 5032
		protected sbyte AffectEquipType;
	}
}
