using System;
using System.Collections.Generic;
using Config;
using GameData.Combat.Math;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;
using GameData.Utilities;

namespace GameData.Domains.SpecialEffect.CombatSkill.Emeipai.FistAndPalm
{
	// Token: 0x02000551 RID: 1361
	public class SanShiLiuBiShou : CombatSkillEffectBase
	{
		// Token: 0x0600404A RID: 16458 RVA: 0x0025D6FD File Offset: 0x0025B8FD
		public SanShiLiuBiShou()
		{
		}

		// Token: 0x0600404B RID: 16459 RVA: 0x0025D707 File Offset: 0x0025B907
		public SanShiLiuBiShou(CombatSkillKey skillKey) : base(skillKey, 2100, -1)
		{
		}

		// Token: 0x0600404C RID: 16460 RVA: 0x0025D718 File Offset: 0x0025B918
		public override void OnEnable(DataContext context)
		{
			Events.RegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
			Events.RegisterHandler_SkillEffectChange(new Events.OnSkillEffectChange(this.OnSkillEffectChange));
		}

		// Token: 0x0600404D RID: 16461 RVA: 0x0025D73F File Offset: 0x0025B93F
		public override void OnDisable(DataContext context)
		{
			Events.UnRegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
			Events.UnRegisterHandler_SkillEffectChange(new Events.OnSkillEffectChange(this.OnSkillEffectChange));
		}

		// Token: 0x0600404E RID: 16462 RVA: 0x0025D768 File Offset: 0x0025B968
		private void OnCastSkillEnd(DataContext context, int charId, bool isAlly, short skillId, sbyte power, bool interrupted)
		{
			bool flag = charId != base.CharacterId || skillId != base.SkillTemplateId;
			if (!flag)
			{
				bool flag2 = !this.IsSrcSkillPerformed;
				if (flag2)
				{
					bool flag3 = base.PowerMatchAffectRequire((int)power, 0);
					if (flag3)
					{
						this.IsSrcSkillPerformed = true;
						base.AppendAffectedData(context, base.CharacterId, base.IsDirect ? 138 : 164, EDataModifyType.Custom, -1);
						base.AddMaxEffectCount(true);
					}
					else
					{
						base.RemoveSelf(context);
					}
				}
				else
				{
					bool flag4 = base.PowerMatchAffectRequire((int)power, 0);
					if (flag4)
					{
						base.RemoveSelf(context);
					}
				}
			}
		}

		// Token: 0x0600404F RID: 16463 RVA: 0x0025D80C File Offset: 0x0025BA0C
		private void OnSkillEffectChange(DataContext context, int charId, SkillEffectKey key, short oldCount, short newCount, bool removed)
		{
			bool flag = removed && this.IsSrcSkillPerformed && charId == base.CharacterId && key.SkillId == base.SkillTemplateId && key.IsDirect == base.IsDirect;
			if (flag)
			{
				base.RemoveSelf(context);
			}
		}

		// Token: 0x06004050 RID: 16464 RVA: 0x0025D85C File Offset: 0x0025BA5C
		public override bool GetModifiedValue(AffectedDataKey dataKey, bool dataValue)
		{
			bool flag = dataKey.FieldId == 138 && dataKey.CustomParam1 == 0 && dataKey.CustomParam2 == 1 && !base.CombatChar.GetWeaponTricks().Exist((sbyte)dataKey.CustomParam0);
			bool result;
			if (flag)
			{
				base.ShowSpecialEffectTips(0);
				base.ReduceEffectCount(1);
				result = false;
			}
			else
			{
				result = dataValue;
			}
			return result;
		}

		// Token: 0x06004051 RID: 16465 RVA: 0x0025D8C4 File Offset: 0x0025BAC4
		public override List<NeedTrick> GetModifiedValue(AffectedDataKey dataKey, List<NeedTrick> dataValue)
		{
			bool flag = dataKey.CharId != base.CharacterId;
			List<NeedTrick> result;
			if (flag)
			{
				result = dataValue;
			}
			else
			{
				DataContext context = DomainManager.Combat.Context;
				bool flag2 = dataKey.FieldId == 164 && dataKey.CustomParam1 == 0;
				if (flag2)
				{
					sbyte[] weaponTricks = base.CombatChar.GetWeaponTricks();
					int reducedCount = 0;
					for (int i = 0; i < dataValue.Count; i++)
					{
						NeedTrick costTrick = dataValue[i];
						bool flag3 = weaponTricks.Exist(costTrick.TrickType);
						if (flag3)
						{
							int reduceCount = Math.Min((int)costTrick.NeedCount, base.EffectCount - reducedCount);
							reducedCount += reduceCount;
							costTrick.NeedCount = (byte)((int)costTrick.NeedCount - reduceCount);
							dataValue[i] = costTrick;
							bool flag4 = reducedCount >= base.EffectCount;
							if (flag4)
							{
								break;
							}
						}
					}
					bool flag5 = reducedCount > 0;
					if (flag5)
					{
						base.ShowSpecialEffectTips(0);
						base.ReduceEffectCount(reducedCount);
					}
				}
				result = dataValue;
			}
			return result;
		}
	}
}
