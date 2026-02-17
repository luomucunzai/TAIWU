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
	// Token: 0x02000588 RID: 1416
	public class AddMaxPowerOrUseRequirement : CombatSkillEffectBase
	{
		// Token: 0x060041EE RID: 16878 RVA: 0x00264B90 File Offset: 0x00262D90
		protected AddMaxPowerOrUseRequirement()
		{
		}

		// Token: 0x060041EF RID: 16879 RVA: 0x00264B9A File Offset: 0x00262D9A
		protected AddMaxPowerOrUseRequirement(CombatSkillKey skillKey, int type) : base(skillKey, type, -1)
		{
		}

		// Token: 0x060041F0 RID: 16880 RVA: 0x00264BA8 File Offset: 0x00262DA8
		public override void OnEnable(DataContext context)
		{
			this._maxSkillPower = 0;
			bool isDirect = base.IsDirect;
			if (isDirect)
			{
				this.AffectDatas = new Dictionary<AffectedDataKey, EDataModifyType>();
				this.AffectDatas.Add(new AffectedDataKey(base.CharacterId, 200, -1, -1, -1, -1), EDataModifyType.Add);
			}
			Events.RegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
		}

		// Token: 0x060041F1 RID: 16881 RVA: 0x00264C07 File Offset: 0x00262E07
		public override void OnDisable(DataContext context)
		{
			Events.UnRegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
		}

		// Token: 0x060041F2 RID: 16882 RVA: 0x00264C1C File Offset: 0x00262E1C
		private void OnCastSkillEnd(DataContext context, int charId, bool isAlly, short skillId, sbyte power, bool interrupted)
		{
			bool flag = charId != base.CharacterId || skillId != base.SkillTemplateId;
			if (!flag)
			{
				bool isDirect = base.IsDirect;
				if (isDirect)
				{
					bool flag2 = power > this._maxSkillPower;
					if (flag2)
					{
						this._maxSkillPower = power / 10;
						DomainManager.SpecialEffect.InvalidateCache(context, base.CharacterId, 200);
						base.ShowSpecialEffectTips(0);
					}
				}
				else
				{
					CombatCharacter enemyChar = base.CurrEnemyChar;
					short silenceSkill = enemyChar.GetRandomBanableSkillId(context.Random, null, this.AffectEquipType);
					int cdFrame = (int)(60 * power / 10);
					bool flag3 = silenceSkill >= 0 && cdFrame > 0;
					if (flag3)
					{
						DomainManager.Combat.SilenceSkill(context, enemyChar, silenceSkill, (int)((short)cdFrame), 100);
						base.ShowSpecialEffectTips(0);
					}
				}
			}
		}

		// Token: 0x060041F3 RID: 16883 RVA: 0x00264CF0 File Offset: 0x00262EF0
		public override int GetModifyValue(AffectedDataKey dataKey, int currModifyValue)
		{
			bool flag = dataKey.CharId != base.CharacterId || Config.CombatSkill.Instance[dataKey.CombatSkillId].EquipType != this.AffectEquipType;
			int result;
			if (flag)
			{
				result = 0;
			}
			else
			{
				bool flag2 = dataKey.FieldId == 200;
				if (flag2)
				{
					result = (int)(3 * this._maxSkillPower);
				}
				else
				{
					result = 0;
				}
			}
			return result;
		}

		// Token: 0x04001374 RID: 4980
		private const sbyte AddMaxPowerUnit = 3;

		// Token: 0x04001375 RID: 4981
		private const sbyte CdFrameUnit = 60;

		// Token: 0x04001376 RID: 4982
		protected sbyte AffectEquipType;

		// Token: 0x04001377 RID: 4983
		private sbyte _maxSkillPower;
	}
}
