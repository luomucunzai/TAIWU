using System;
using System.Collections.Generic;
using Config;
using GameData.Combat.Math;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;
using GameData.Utilities;

namespace GameData.Domains.SpecialEffect.Animal.Loong.Neigong
{
	// Token: 0x020005ED RID: 1517
	public class LoongEarthImplementSilenceColorful : LoongEarthImplementSilence, ILoongEarthExtra
	{
		// Token: 0x060044A5 RID: 17573 RVA: 0x002701A8 File Offset: 0x0026E3A8
		private LoongEarthImplementSilenceColorful.OnSilenceAffect GetAffectByCombatSkillId(short skillId)
		{
			sbyte equipType = Config.CombatSkill.Instance[skillId].EquipType;
			if (!true)
			{
			}
			LoongEarthImplementSilenceColorful.OnSilenceAffect result;
			switch (equipType)
			{
			case 1:
				result = new LoongEarthImplementSilenceColorful.OnSilenceAffect(this.OnSilenceAttack);
				break;
			case 2:
				result = new LoongEarthImplementSilenceColorful.OnSilenceAffect(this.OnSilenceAgile);
				break;
			case 3:
				result = new LoongEarthImplementSilenceColorful.OnSilenceAffect(this.OnSilenceDefense);
				break;
			case 4:
				result = new LoongEarthImplementSilenceColorful.OnSilenceAffect(this.OnSilenceAssist);
				break;
			default:
				result = null;
				break;
			}
			if (!true)
			{
			}
			return result;
		}

		// Token: 0x060044A6 RID: 17574 RVA: 0x00270230 File Offset: 0x0026E430
		public override void OnEnable(DataContext context)
		{
			base.OnEnable(context);
			base.EffectBase.CreateAffectedAllEnemyData(171, EDataModifyType.Custom, -1);
			base.EffectBase.CreateAffectedAllEnemyData(172, EDataModifyType.Custom, -1);
			base.EffectBase.CreateAffectedAllEnemyData(274, EDataModifyType.Custom, -1);
			Events.RegisterHandler_SkillSilenceEnd(new Events.OnSkillSilenceEnd(this.OnSkillSilenceEnd));
		}

		// Token: 0x060044A7 RID: 17575 RVA: 0x00270291 File Offset: 0x0026E491
		public override void OnDisable(DataContext context)
		{
			Events.UnRegisterHandler_SkillSilenceEnd(new Events.OnSkillSilenceEnd(this.OnSkillSilenceEnd));
			base.OnDisable(context);
		}

		// Token: 0x060044A8 RID: 17576 RVA: 0x002702B0 File Offset: 0x0026E4B0
		private void OnSkillSilenceEnd(DataContext context, CombatSkillKey skillKey)
		{
			HashSet<short> attackSkills;
			bool flag = this._silencingAttackSkills.TryGetValue(skillKey.CharId, out attackSkills) && attackSkills.Remove(skillKey.SkillTemplateId) && attackSkills.Count == 0;
			if (flag)
			{
				this.InvalidCacheAttack(context, skillKey.CharId);
			}
			HashSet<short> agileSkills;
			bool flag2 = this._silencingAgileSkills.TryGetValue(skillKey.CharId, out agileSkills) && agileSkills.Remove(skillKey.SkillTemplateId) && agileSkills.Count == 0;
			if (flag2)
			{
				this.InvalidCacheAgile(context, skillKey.CharId);
			}
		}

		// Token: 0x060044A9 RID: 17577 RVA: 0x00270340 File Offset: 0x0026E540
		public void OnSilenced(DataContext context, CombatCharacter combatChar, short skillId)
		{
			LoongEarthImplementSilenceColorful.OnSilenceAffect extraAffect = this.GetAffectByCombatSkillId(skillId);
			if (extraAffect != null)
			{
				extraAffect(context, combatChar, skillId);
			}
		}

		// Token: 0x060044AA RID: 17578 RVA: 0x00270368 File Offset: 0x0026E568
		private void InvalidCacheAttack(DataContext context, int charId)
		{
			DomainManager.SpecialEffect.InvalidateCache(context, charId, 171);
			DomainManager.SpecialEffect.InvalidateCache(context, charId, 172);
			bool flag = !DomainManager.Combat.IsCharInCombat(charId, true);
			if (!flag)
			{
				CombatCharacter combatChar = DomainManager.Combat.GetElement_CombatCharacterDict(charId);
				bool flag2 = combatChar.GetBreathValue() > combatChar.GetMaxBreathValue();
				if (flag2)
				{
					DomainManager.Combat.ChangeBreathValue(context, combatChar, 0, false, null);
				}
				bool flag3 = combatChar.GetStanceValue() > combatChar.GetMaxStanceValue();
				if (flag3)
				{
					DomainManager.Combat.ChangeStanceValue(context, combatChar, 0, false, null);
				}
			}
		}

		// Token: 0x060044AB RID: 17579 RVA: 0x00270400 File Offset: 0x0026E600
		private void InvalidCacheAgile(DataContext context, int charId)
		{
			DomainManager.SpecialEffect.InvalidateCache(context, charId, 274);
			bool flag = !DomainManager.Combat.IsCharInCombat(charId, true);
			if (!flag)
			{
				CombatCharacter combatChar = DomainManager.Combat.GetElement_CombatCharacterDict(charId);
				bool flag2 = combatChar.GetMobilityValue() > combatChar.GetMaxMobility();
				if (flag2)
				{
					DomainManager.Combat.ChangeMobilityValue(context, combatChar, 0, false, null, false);
				}
			}
		}

		// Token: 0x060044AC RID: 17580 RVA: 0x00270464 File Offset: 0x0026E664
		private void OnSilenceAttack(DataContext context, CombatCharacter combatChar, short skillId)
		{
			HashSet<short> skills = this._silencingAttackSkills.GetOrNew(combatChar.GetId());
			bool needUpdateCache = skills.Count == 0;
			skills.Add(skillId);
			bool flag = !needUpdateCache;
			if (!flag)
			{
				this.InvalidCacheAttack(context, combatChar.GetId());
				base.EffectBase.ShowSpecialEffectTips(1);
			}
		}

		// Token: 0x060044AD RID: 17581 RVA: 0x002704BC File Offset: 0x0026E6BC
		private void OnSilenceAgile(DataContext context, CombatCharacter combatChar, short skillId)
		{
			HashSet<short> skills = this._silencingAgileSkills.GetOrNew(combatChar.GetId());
			bool needUpdateCache = skills.Count == 0;
			skills.Add(skillId);
			bool flag = !needUpdateCache;
			if (!flag)
			{
				this.InvalidCacheAgile(context, combatChar.GetId());
				base.EffectBase.ShowSpecialEffectTips(2);
			}
		}

		// Token: 0x060044AE RID: 17582 RVA: 0x00270514 File Offset: 0x0026E714
		private void OnSilenceDefense(DataContext context, CombatCharacter combatChar, short skillId)
		{
			for (int i = 0; i < 4; i++)
			{
				DomainManager.Combat.AddFlaw(context, combatChar, 3, base.EffectBase.SkillKey, -1, 1, true);
			}
			base.EffectBase.ShowSpecialEffectTips(3);
		}

		// Token: 0x060044AF RID: 17583 RVA: 0x0027055C File Offset: 0x0026E75C
		private void OnSilenceAssist(DataContext context, CombatCharacter combatChar, short skillId)
		{
			for (int i = 0; i < 4; i++)
			{
				DomainManager.Combat.AddAcupoint(context, combatChar, 3, base.EffectBase.SkillKey, -1, 1, true);
			}
			base.EffectBase.ShowSpecialEffectTips(4);
		}

		// Token: 0x060044B0 RID: 17584 RVA: 0x002705A4 File Offset: 0x0026E7A4
		public override int GetModifiedValue(AffectedDataKey dataKey, int dataValue)
		{
			bool flag = dataKey.CharId == base.EffectBase.CharacterId;
			int result;
			if (flag)
			{
				result = dataValue;
			}
			else
			{
				ushort fieldId = dataKey.FieldId;
				bool flag2 = fieldId - 171 <= 1;
				HashSet<short> attackSkills;
				bool flag3 = flag2 && this._silencingAttackSkills.TryGetValue(dataKey.CharId, out attackSkills) && attackSkills.Count > 0;
				if (flag3)
				{
					result = Math.Min(50, dataValue);
				}
				else
				{
					HashSet<short> agileSkills;
					bool flag4 = dataKey.FieldId == 274 && this._silencingAgileSkills.TryGetValue(dataKey.CharId, out agileSkills) && agileSkills.Count > 0;
					if (flag4)
					{
						result = Math.Min(50, dataValue);
					}
					else
					{
						result = dataValue;
					}
				}
			}
			return result;
		}

		// Token: 0x04001450 RID: 5200
		private const int MaxValuePercent = 50;

		// Token: 0x04001451 RID: 5201
		private const int AddFlawOrAcupointLevel = 3;

		// Token: 0x04001452 RID: 5202
		private const int AddFlawOrAcupointCount = 4;

		// Token: 0x04001453 RID: 5203
		private readonly Dictionary<int, HashSet<short>> _silencingAttackSkills = new Dictionary<int, HashSet<short>>();

		// Token: 0x04001454 RID: 5204
		private readonly Dictionary<int, HashSet<short>> _silencingAgileSkills = new Dictionary<int, HashSet<short>>();

		// Token: 0x04001455 RID: 5205
		private readonly Dictionary<int, HashSet<short>> _silencingAssistSkills = new Dictionary<int, HashSet<short>>();

		// Token: 0x02000A64 RID: 2660
		// (Invoke) Token: 0x060087BC RID: 34748
		public delegate void OnSilenceAffect(DataContext context, CombatCharacter combatChar, short skillId);
	}
}
