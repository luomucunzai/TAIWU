using System;
using System.Collections.Generic;
using GameData.Combat.Math;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;

namespace GameData.Domains.SpecialEffect.CombatSkill.Common.Attack
{
	// Token: 0x02000599 RID: 1433
	public abstract class CurseSilenceCombatSkill : CombatSkillEffectBase
	{
		// Token: 0x17000295 RID: 661
		// (get) Token: 0x06004286 RID: 17030
		protected abstract sbyte TargetEquipType { get; }

		// Token: 0x17000296 RID: 662
		// (get) Token: 0x06004287 RID: 17031 RVA: 0x00267576 File Offset: 0x00265776
		protected IReadOnlySet<CombatSkillKey> SilencingSkills
		{
			get
			{
				return this._silencingSkills;
			}
		}

		// Token: 0x06004288 RID: 17032 RVA: 0x0026757E File Offset: 0x0026577E
		protected CurseSilenceCombatSkill()
		{
		}

		// Token: 0x06004289 RID: 17033 RVA: 0x00267593 File Offset: 0x00265793
		protected CurseSilenceCombatSkill(CombatSkillKey skillKey, int type) : base(skillKey, type, -1)
		{
		}

		// Token: 0x0600428A RID: 17034 RVA: 0x002675AC File Offset: 0x002657AC
		public override void OnEnable(DataContext context)
		{
			bool isDirect = base.IsDirect;
			if (isDirect)
			{
				base.CreateAffectedData(199, EDataModifyType.AddPercent, base.SkillTemplateId);
			}
			Events.RegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
			Events.RegisterHandler_SkillSilenceEnd(new Events.OnSkillSilenceEnd(this.OnSkillSilenceEnd));
		}

		// Token: 0x0600428B RID: 17035 RVA: 0x002675FB File Offset: 0x002657FB
		public override void OnDisable(DataContext context)
		{
			Events.UnRegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
			Events.UnRegisterHandler_SkillSilenceEnd(new Events.OnSkillSilenceEnd(this.OnSkillSilenceEnd));
		}

		// Token: 0x0600428C RID: 17036 RVA: 0x00267624 File Offset: 0x00265824
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
				result = ((dataKey.FieldId == 199) ? (this._silencingSkills.Count * 40) : 0);
			}
			return result;
		}

		// Token: 0x0600428D RID: 17037 RVA: 0x00267670 File Offset: 0x00265870
		private void OnCastSkillEnd(DataContext context, int charId, bool isAlly, short skillId, sbyte power, bool interrupted)
		{
			bool flag = charId != base.CharacterId || skillId != base.SkillTemplateId;
			if (!flag)
			{
				bool flag2 = base.PowerMatchAffectRequire((int)power, 0);
				if (flag2)
				{
					bool anySilenced = false;
					CombatCharacter enemyChar = base.CurrEnemyChar;
					int maxCount = base.IsDirect ? 1 : 2;
					foreach (short banableSkillId in enemyChar.GetRandomUnrepeatedBanableSkillIds(context.Random, maxCount, null, this.TargetEquipType, -1))
					{
						DomainManager.Combat.SilenceSkill(context, enemyChar, banableSkillId, base.IsDirect ? 900 : 1800, 100);
						CombatSkillKey skillKey = new CombatSkillKey(enemyChar.GetId(), banableSkillId);
						this._silencingSkills.Add(skillKey);
						this.OnSilenceBegin(context, skillKey);
						anySilenced = true;
					}
					bool flag3 = anySilenced;
					if (flag3)
					{
						base.ShowSpecialEffectTips(0);
					}
					bool flag4 = anySilenced && base.IsDirect;
					if (flag4)
					{
						base.InvalidateCache(context, 199);
						base.ShowSpecialEffectTips(1);
					}
				}
				bool flag5 = !base.IsDirect && !interrupted;
				if (flag5)
				{
					DomainManager.Combat.SilenceSkill(context, base.CombatChar, base.SkillTemplateId, 1800, -1);
					base.ShowSpecialEffectTips(1);
				}
			}
		}

		// Token: 0x0600428E RID: 17038 RVA: 0x002677E0 File Offset: 0x002659E0
		private void OnSkillSilenceEnd(DataContext context, CombatSkillKey skillKey)
		{
			bool flag = !this._silencingSkills.Contains(skillKey);
			if (!flag)
			{
				this._silencingSkills.Remove(skillKey);
				base.InvalidateCache(context, 199);
				this.OnSilenceEnd(context, skillKey);
			}
		}

		// Token: 0x0600428F RID: 17039
		protected abstract void OnSilenceBegin(DataContext context, CombatSkillKey skillKey);

		// Token: 0x06004290 RID: 17040
		protected abstract void OnSilenceEnd(DataContext context, CombatSkillKey skillKey);

		// Token: 0x040013A9 RID: 5033
		private const int DirectSilenceCount = 1;

		// Token: 0x040013AA RID: 5034
		private const int DirectSilenceFrame = 900;

		// Token: 0x040013AB RID: 5035
		private const int ReverseSilenceCount = 2;

		// Token: 0x040013AC RID: 5036
		private const int ReverseSilenceFrame = 1800;

		// Token: 0x040013AD RID: 5037
		private const int ReverseSilenceSelfFrame = 1800;

		// Token: 0x040013AE RID: 5038
		private const int AddPowerPercent = 40;

		// Token: 0x040013AF RID: 5039
		private readonly HashSet<CombatSkillKey> _silencingSkills = new HashSet<CombatSkillKey>();
	}
}
