using System;
using Config;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Combat;
using GameData.Domains.SpecialEffect.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Implement;
using GameData.Utilities;

namespace GameData.Domains.SpecialEffect.Animal.Loong.Neigong
{
	// Token: 0x020005EA RID: 1514
	public class LoongEarthImplementSilence : ISpecialEffectImplement, ISpecialEffectModifier
	{
		// Token: 0x170002C5 RID: 709
		// (get) Token: 0x0600449A RID: 17562 RVA: 0x00270006 File Offset: 0x0026E206
		// (set) Token: 0x0600449B RID: 17563 RVA: 0x0027000E File Offset: 0x0026E20E
		public CombatSkillEffectBase EffectBase { get; set; }

		// Token: 0x0600449C RID: 17564 RVA: 0x00270017 File Offset: 0x0026E217
		public virtual void OnEnable(DataContext context)
		{
			Events.RegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
		}

		// Token: 0x0600449D RID: 17565 RVA: 0x0027002C File Offset: 0x0026E22C
		public virtual void OnDisable(DataContext context)
		{
			Events.UnRegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
		}

		// Token: 0x0600449E RID: 17566 RVA: 0x00270044 File Offset: 0x0026E244
		private void OnCastSkillEnd(DataContext context, int charId, bool isAlly, short skillId, sbyte power, bool interrupted)
		{
			bool flag = isAlly == this.EffectBase.CombatChar.IsAlly || !this.EffectBase.PowerMatchAffectRequire((int)power, 0);
			if (!flag)
			{
				bool flag2 = CombatSkill.Instance[skillId].EquipType != 1;
				if (!flag2)
				{
					bool flag3 = !DomainManager.Combat.IsCurrentCombatCharacter(this.EffectBase.CombatChar);
					if (!flag3)
					{
						CombatCharacter enemyChar = DomainManager.Combat.GetElement_CombatCharacterDict(charId);
						short banableSkillId = enemyChar.GetRandomBanableSkillId(context.Random, null, -1);
						bool flag4 = banableSkillId >= 0;
						if (flag4)
						{
							this.DoSilence(context, enemyChar, banableSkillId);
						}
					}
				}
			}
		}

		// Token: 0x0600449F RID: 17567 RVA: 0x002700F0 File Offset: 0x0026E2F0
		private void DoSilence(DataContext context, CombatCharacter combatChar, short skillId)
		{
			Tester.Assert(this.SilenceFrame > 0, "SilenceFrame > 0");
			bool success = DomainManager.Combat.SilenceSkill(context, combatChar, skillId, this.SilenceFrame, 100);
			bool flag = !success;
			if (!flag)
			{
				this.EffectBase.ShowSpecialEffectTips(0);
				ILoongEarthExtra extra = this as ILoongEarthExtra;
				bool flag2 = extra != null;
				if (flag2)
				{
					extra.OnSilenced(context, combatChar, skillId);
				}
			}
		}

		// Token: 0x060044A0 RID: 17568 RVA: 0x00270158 File Offset: 0x0026E358
		public virtual int GetModifiedValue(AffectedDataKey dataKey, int dataValue)
		{
			return dataValue;
		}

		// Token: 0x0400144D RID: 5197
		public int SilenceFrame;
	}
}
