using System;
using System.Collections.Generic;
using GameData.Combat.Math;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;

namespace GameData.Domains.SpecialEffect.CombatSkill.Common.Attack
{
	// Token: 0x02000584 RID: 1412
	public class AccumulateNeiliAllocationToStrengthen : CombatSkillEffectBase
	{
		// Token: 0x060041CD RID: 16845 RVA: 0x0026419D File Offset: 0x0026239D
		protected AccumulateNeiliAllocationToStrengthen()
		{
		}

		// Token: 0x060041CE RID: 16846 RVA: 0x002641A7 File Offset: 0x002623A7
		protected AccumulateNeiliAllocationToStrengthen(CombatSkillKey skillKey, int type) : base(skillKey, type, -1)
		{
		}

		// Token: 0x060041CF RID: 16847 RVA: 0x002641B4 File Offset: 0x002623B4
		public override void OnEnable(DataContext context)
		{
			DomainManager.Combat.AddSkillEffect(context, base.CombatChar, new SkillEffectKey(base.SkillTemplateId, base.IsDirect), 0, base.MaxEffectCount, false);
			this.AffectDatas = new Dictionary<AffectedDataKey, EDataModifyType>();
			this.AffectDatas.Add(new AffectedDataKey(base.CharacterId, 199, base.SkillTemplateId, -1, -1, -1), EDataModifyType.AddPercent);
			this.AffectDatas.Add(new AffectedDataKey(base.CharacterId, 154, base.SkillTemplateId, -1, -1, -1), EDataModifyType.Custom);
			Events.RegisterHandler_NeiliAllocationChanged(new Events.OnNeiliAllocationChanged(this.OnNeiliAllocationChanged));
			Events.RegisterHandler_SkillEffectChange(new Events.OnSkillEffectChange(this.OnSkillEffectChange));
			Events.RegisterHandler_PrepareSkillBegin(new Events.OnPrepareSkillBegin(this.OnPrepareSkillBegin));
			Events.RegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
		}

		// Token: 0x060041D0 RID: 16848 RVA: 0x0026428C File Offset: 0x0026248C
		public override void OnDisable(DataContext context)
		{
			Events.UnRegisterHandler_NeiliAllocationChanged(new Events.OnNeiliAllocationChanged(this.OnNeiliAllocationChanged));
			Events.UnRegisterHandler_SkillEffectChange(new Events.OnSkillEffectChange(this.OnSkillEffectChange));
			Events.UnRegisterHandler_PrepareSkillBegin(new Events.OnPrepareSkillBegin(this.OnPrepareSkillBegin));
			Events.UnRegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
		}

		// Token: 0x060041D1 RID: 16849 RVA: 0x002642E4 File Offset: 0x002624E4
		private void OnNeiliAllocationChanged(DataContext context, int charId, byte type, int changeValue)
		{
			bool flag = charId != base.CharacterId || type != this.RequireNeiliAllocationType || base.EffectCount >= (int)base.MaxEffectCount || !(base.IsDirect ? (changeValue > 0) : (changeValue < 0));
			if (!flag)
			{
				short addValue = (short)Math.Min(Math.Abs(changeValue), (int)base.MaxEffectCount - base.EffectCount);
				DomainManager.Combat.ChangeSkillEffectCount(context, base.CombatChar, new SkillEffectKey(base.SkillTemplateId, base.IsDirect), addValue, true, false);
				bool flag2 = base.EffectCount >= (int)base.MaxEffectCount;
				if (flag2)
				{
					base.ShowSpecialEffectTips(0);
				}
			}
		}

		// Token: 0x060041D2 RID: 16850 RVA: 0x00264390 File Offset: 0x00262590
		private void OnSkillEffectChange(DataContext context, int charId, SkillEffectKey key, short oldCount, short newCount, bool removed)
		{
			bool flag = charId == base.CharacterId && key.SkillId == base.SkillTemplateId && key.IsDirect == base.IsDirect;
			if (flag)
			{
				DomainManager.SpecialEffect.InvalidateCache(context, base.CharacterId, 199);
				DomainManager.SpecialEffect.InvalidateCache(context, base.CharacterId, 154);
				DomainManager.Combat.UpdateSkillCanUse(context, base.CombatChar, base.SkillTemplateId);
			}
		}

		// Token: 0x060041D3 RID: 16851 RVA: 0x00264414 File Offset: 0x00262614
		private void OnPrepareSkillBegin(DataContext context, int charId, bool isAlly, short skillId)
		{
			bool flag = charId != base.CharacterId || skillId != base.SkillTemplateId || base.EffectCount < (int)base.MaxEffectCount;
			if (!flag)
			{
				DomainManager.Combat.ChangeSkillPrepareProgress(base.CombatChar, base.CombatChar.SkillPrepareTotalProgress * 50 / 100);
			}
		}

		// Token: 0x060041D4 RID: 16852 RVA: 0x00264470 File Offset: 0x00262670
		private void OnCastSkillEnd(DataContext context, int charId, bool isAlly, short skillId, sbyte power, bool interrupted)
		{
			bool flag = charId != base.CharacterId || skillId != base.SkillTemplateId || base.EffectCount < (int)base.MaxEffectCount;
			if (!flag)
			{
				base.ReduceEffectCount((int)base.MaxEffectCount);
			}
		}

		// Token: 0x060041D5 RID: 16853 RVA: 0x002644B8 File Offset: 0x002626B8
		public override int GetModifyValue(AffectedDataKey dataKey, int currModifyValue)
		{
			bool flag = dataKey.CharId != base.CharacterId || dataKey.CombatSkillId != base.SkillTemplateId;
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
					result = ((base.EffectCount == (int)base.MaxEffectCount) ? 20 : 0);
				}
				else
				{
					result = 0;
				}
			}
			return result;
		}

		// Token: 0x060041D6 RID: 16854 RVA: 0x0026451C File Offset: 0x0026271C
		public override bool GetModifiedValue(AffectedDataKey dataKey, bool dataValue)
		{
			bool flag = dataKey.CharId != base.CharacterId || dataKey.CombatSkillId != base.SkillTemplateId;
			bool result;
			if (flag)
			{
				result = dataValue;
			}
			else
			{
				bool flag2 = dataKey.FieldId == 154;
				if (flag2)
				{
					result = (base.EffectCount != (int)base.MaxEffectCount);
				}
				else
				{
					result = dataValue;
				}
			}
			return result;
		}

		// Token: 0x04001367 RID: 4967
		private const sbyte AddPower = 20;

		// Token: 0x04001368 RID: 4968
		private const sbyte PrepareProgressPercent = 50;

		// Token: 0x04001369 RID: 4969
		protected byte RequireNeiliAllocationType;
	}
}
