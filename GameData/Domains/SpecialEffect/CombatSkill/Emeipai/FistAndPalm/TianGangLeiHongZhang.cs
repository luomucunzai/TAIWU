using System;
using System.Collections.Generic;
using GameData.Combat.Math;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;

namespace GameData.Domains.SpecialEffect.CombatSkill.Emeipai.FistAndPalm
{
	// Token: 0x02000553 RID: 1363
	public class TianGangLeiHongZhang : CombatSkillEffectBase
	{
		// Token: 0x06004054 RID: 16468 RVA: 0x0025D9FF File Offset: 0x0025BBFF
		public TianGangLeiHongZhang()
		{
		}

		// Token: 0x06004055 RID: 16469 RVA: 0x0025DA09 File Offset: 0x0025BC09
		public TianGangLeiHongZhang(CombatSkillKey skillKey) : base(skillKey, 2105, -1)
		{
		}

		// Token: 0x06004056 RID: 16470 RVA: 0x0025DA1C File Offset: 0x0025BC1C
		public override void OnEnable(DataContext context)
		{
			base.AddMaxEffectCount(false);
			this.AffectDatas = new Dictionary<AffectedDataKey, EDataModifyType>();
			this.AffectDatas.Add(new AffectedDataKey(base.CharacterId, base.IsDirect ? 64 : 65, base.SkillTemplateId, -1, -1, -1), EDataModifyType.AddPercent);
			this.AffectDatas.Add(new AffectedDataKey(base.CharacterId, base.IsDirect ? 7 : 8, -1, -1, -1, -1), EDataModifyType.TotalPercent);
			Events.RegisterHandler_PrepareSkillBegin(new Events.OnPrepareSkillBegin(this.OnPrepareSkillBegin));
			Events.RegisterHandler_PrepareSkillEnd(new Events.OnPrepareSkillEnd(this.OnPrepareSkillEnd));
			Events.RegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
			Events.RegisterHandler_SkillEffectChange(new Events.OnSkillEffectChange(this.OnSkillEffectChange));
		}

		// Token: 0x06004057 RID: 16471 RVA: 0x0025DADC File Offset: 0x0025BCDC
		public override void OnDisable(DataContext context)
		{
			Events.UnRegisterHandler_PrepareSkillBegin(new Events.OnPrepareSkillBegin(this.OnPrepareSkillBegin));
			Events.UnRegisterHandler_PrepareSkillEnd(new Events.OnPrepareSkillEnd(this.OnPrepareSkillEnd));
			Events.UnRegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
			Events.UnRegisterHandler_SkillEffectChange(new Events.OnSkillEffectChange(this.OnSkillEffectChange));
		}

		// Token: 0x06004058 RID: 16472 RVA: 0x0025DB34 File Offset: 0x0025BD34
		private void OnPrepareSkillBegin(DataContext context, int charId, bool isAlly, short skillId)
		{
			bool flag = charId != base.CharacterId || skillId != base.SkillTemplateId || base.EffectCount <= 0;
			if (!flag)
			{
				DomainManager.Combat.ChangeSkillPrepareProgress(base.CombatChar, base.CombatChar.SkillPrepareTotalProgress * 50 / 100);
			}
		}

		// Token: 0x06004059 RID: 16473 RVA: 0x0025DB8C File Offset: 0x0025BD8C
		private void OnPrepareSkillEnd(DataContext context, int charId, bool isAlly, short skillId)
		{
			bool flag = charId != base.CharacterId || skillId != base.SkillTemplateId || base.EffectCount <= 0 || !DomainManager.Combat.InAttackRange(base.CombatChar);
			if (!flag)
			{
				base.ShowSpecialEffectTips(0);
			}
		}

		// Token: 0x0600405A RID: 16474 RVA: 0x0025DBDC File Offset: 0x0025BDDC
		private void OnCastSkillEnd(DataContext context, int charId, bool isAlly, short skillId, sbyte power, bool interrupted)
		{
			bool flag = charId != base.CharacterId || skillId != base.SkillTemplateId || base.EffectCount <= 0;
			if (!flag)
			{
				base.ReduceEffectCount(1);
			}
		}

		// Token: 0x0600405B RID: 16475 RVA: 0x0025DC1C File Offset: 0x0025BE1C
		private void OnSkillEffectChange(DataContext context, int charId, SkillEffectKey key, short oldCount, short newCount, bool removed)
		{
			bool flag = charId == base.CharacterId && key.SkillId == base.SkillTemplateId && key.IsDirect == base.IsDirect;
			if (flag)
			{
				DomainManager.SpecialEffect.InvalidateCache(context, base.CharacterId, base.IsDirect ? 7 : 8);
			}
		}

		// Token: 0x0600405C RID: 16476 RVA: 0x0025DC74 File Offset: 0x0025BE74
		public override int GetModifyValue(AffectedDataKey dataKey, int currModifyValue)
		{
			bool flag = dataKey.CharId != base.CharacterId;
			int result;
			if (flag)
			{
				result = 0;
			}
			else
			{
				bool flag2 = dataKey.FieldId == 7 || dataKey.FieldId == 8;
				if (flag2)
				{
					result = -20 * base.EffectCount;
				}
				else
				{
					bool flag3 = dataKey.CombatSkillId != base.SkillTemplateId;
					if (flag3)
					{
						result = 0;
					}
					else
					{
						bool flag4 = dataKey.FieldId == 64 || dataKey.FieldId == 65;
						if (flag4)
						{
							result = ((base.EffectCount > 0 && DomainManager.Combat.InAttackRange(base.CombatChar)) ? 40 : 0);
						}
						else
						{
							result = 0;
						}
					}
				}
			}
			return result;
		}

		// Token: 0x040012E5 RID: 4837
		private const sbyte PrepareProgressPercent = 50;

		// Token: 0x040012E6 RID: 4838
		private const sbyte AddPenetrate = 40;

		// Token: 0x040012E7 RID: 4839
		private const sbyte ReduceRecoveryUnit = -20;
	}
}
