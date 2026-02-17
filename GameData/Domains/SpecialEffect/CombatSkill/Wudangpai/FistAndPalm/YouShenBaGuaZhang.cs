using System;
using System.Collections.Generic;
using GameData.Combat.Math;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;

namespace GameData.Domains.SpecialEffect.CombatSkill.Wudangpai.FistAndPalm
{
	// Token: 0x020003D4 RID: 980
	public class YouShenBaGuaZhang : CombatSkillEffectBase
	{
		// Token: 0x060037A7 RID: 14247 RVA: 0x00236828 File Offset: 0x00234A28
		public YouShenBaGuaZhang()
		{
		}

		// Token: 0x060037A8 RID: 14248 RVA: 0x00236832 File Offset: 0x00234A32
		public YouShenBaGuaZhang(CombatSkillKey skillKey) : base(skillKey, 4103, -1)
		{
		}

		// Token: 0x060037A9 RID: 14249 RVA: 0x00236844 File Offset: 0x00234A44
		public override void OnEnable(DataContext context)
		{
			this._distanceAccumulator = 0;
			DomainManager.Combat.AddSkillEffect(context, base.CombatChar, new SkillEffectKey(base.SkillTemplateId, base.IsDirect), 0, base.MaxEffectCount, false);
			this.AffectDatas = new Dictionary<AffectedDataKey, EDataModifyType>();
			this.AffectDatas.Add(new AffectedDataKey(base.CharacterId, 204, base.SkillTemplateId, -1, -1, -1), EDataModifyType.TotalPercent);
			this.AffectDatas.Add(new AffectedDataKey(base.CharacterId, base.IsDirect ? 32 : 38, base.SkillTemplateId, -1, -1, -1), EDataModifyType.AddPercent);
			this.AffectDatas.Add(new AffectedDataKey(base.CharacterId, base.IsDirect ? 33 : 39, base.SkillTemplateId, -1, -1, -1), EDataModifyType.AddPercent);
			this.AffectDatas.Add(new AffectedDataKey(base.CharacterId, base.IsDirect ? 34 : 40, base.SkillTemplateId, -1, -1, -1), EDataModifyType.AddPercent);
			Events.RegisterHandler_DistanceChanged(new Events.OnDistanceChanged(this.OnDistanceChanged));
			Events.RegisterHandler_SkillEffectChange(new Events.OnSkillEffectChange(this.OnSkillEffectChange));
			Events.RegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
		}

		// Token: 0x060037AA RID: 14250 RVA: 0x00236978 File Offset: 0x00234B78
		public override void OnDisable(DataContext context)
		{
			Events.UnRegisterHandler_DistanceChanged(new Events.OnDistanceChanged(this.OnDistanceChanged));
			Events.UnRegisterHandler_SkillEffectChange(new Events.OnSkillEffectChange(this.OnSkillEffectChange));
			Events.UnRegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
		}

		// Token: 0x060037AB RID: 14251 RVA: 0x002369B4 File Offset: 0x00234BB4
		private void OnDistanceChanged(DataContext context, CombatCharacter mover, short distance, bool isMove, bool isForced)
		{
			bool flag = mover != base.CombatChar || !isMove || isForced || base.EffectCount >= (int)base.MaxEffectCount;
			if (!flag)
			{
				this._distanceAccumulator += (int)Math.Abs(distance);
				while (this._distanceAccumulator >= 10 && base.EffectCount < (int)base.MaxEffectCount)
				{
					this._distanceAccumulator -= 10;
					DomainManager.Combat.ChangeSkillEffectCount(context, base.CombatChar, new SkillEffectKey(base.SkillTemplateId, base.IsDirect), 1, true, false);
					base.ShowSpecialEffectTips(0);
				}
			}
		}

		// Token: 0x060037AC RID: 14252 RVA: 0x00236A68 File Offset: 0x00234C68
		private void OnSkillEffectChange(DataContext context, int charId, SkillEffectKey key, short oldCount, short newCount, bool removed)
		{
			bool flag = charId != base.CharacterId || key.SkillId != base.SkillTemplateId || key.IsDirect != base.IsDirect;
			if (!flag)
			{
				DomainManager.SpecialEffect.InvalidateCache(context, base.CharacterId, 204);
				DomainManager.SpecialEffect.InvalidateCache(context, base.CharacterId, base.IsDirect ? 32 : 38);
				DomainManager.SpecialEffect.InvalidateCache(context, base.CharacterId, base.IsDirect ? 33 : 39);
				DomainManager.SpecialEffect.InvalidateCache(context, base.CharacterId, base.IsDirect ? 34 : 40);
			}
		}

		// Token: 0x060037AD RID: 14253 RVA: 0x00236B20 File Offset: 0x00234D20
		private void OnCastSkillEnd(DataContext context, int charId, bool isAlly, short skillId, sbyte power, bool interrupted)
		{
			bool flag = charId != base.CharacterId || skillId != base.SkillTemplateId || interrupted || base.EffectCount <= 0;
			if (!flag)
			{
				base.ReduceEffectCount(1);
				DomainManager.SpecialEffect.InvalidateCache(context, base.CharacterId, 204);
				DomainManager.SpecialEffect.InvalidateCache(context, base.CharacterId, base.IsDirect ? 32 : 38);
				DomainManager.SpecialEffect.InvalidateCache(context, base.CharacterId, base.IsDirect ? 33 : 39);
				DomainManager.SpecialEffect.InvalidateCache(context, base.CharacterId, base.IsDirect ? 34 : 40);
			}
		}

		// Token: 0x060037AE RID: 14254 RVA: 0x00236BE0 File Offset: 0x00234DE0
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
				bool flag2 = dataKey.FieldId == 204 && dataKey.CombatSkillId == base.SkillTemplateId;
				if (flag2)
				{
					result = -20 * base.EffectCount;
				}
				else
				{
					bool flag3 = dataKey.FieldId == 32 || dataKey.FieldId == 33 || dataKey.FieldId == 34 || dataKey.FieldId == 38 || dataKey.FieldId == 39 || dataKey.FieldId == 40;
					if (flag3)
					{
						result = 4 * base.EffectCount;
					}
					else
					{
						result = 0;
					}
				}
			}
			return result;
		}

		// Token: 0x04001039 RID: 4153
		private const sbyte NeedDistance = 10;

		// Token: 0x0400103A RID: 4154
		private const sbyte ReduceCostUnit = -20;

		// Token: 0x0400103B RID: 4155
		private const sbyte AddPropertyUnit = 4;

		// Token: 0x0400103C RID: 4156
		private int _distanceAccumulator;
	}
}
