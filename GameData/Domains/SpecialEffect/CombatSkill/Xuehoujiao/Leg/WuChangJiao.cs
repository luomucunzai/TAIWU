using System;
using System.Collections.Generic;
using GameData.Combat.Math;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;

namespace GameData.Domains.SpecialEffect.CombatSkill.Xuehoujiao.Leg
{
	// Token: 0x0200022C RID: 556
	public class WuChangJiao : CombatSkillEffectBase
	{
		// Token: 0x06002F66 RID: 12134 RVA: 0x00212E38 File Offset: 0x00211038
		public WuChangJiao()
		{
		}

		// Token: 0x06002F67 RID: 12135 RVA: 0x00212E42 File Offset: 0x00211042
		public WuChangJiao(CombatSkillKey skillKey) : base(skillKey, 15302, -1)
		{
		}

		// Token: 0x06002F68 RID: 12136 RVA: 0x00212E54 File Offset: 0x00211054
		public override void OnEnable(DataContext context)
		{
			this.AffectDatas = new Dictionary<AffectedDataKey, EDataModifyType>();
			this.AffectDatas.Add(new AffectedDataKey(base.CharacterId, 199, base.SkillTemplateId, -1, -1, -1), EDataModifyType.AddPercent);
			this.AffectDatas.Add(new AffectedDataKey(base.CharacterId, 209, base.SkillTemplateId, -1, -1, -1), EDataModifyType.Custom);
			Events.RegisterHandler_DistanceChanged(new Events.OnDistanceChanged(this.OnDistanceChanged));
			Events.RegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
		}

		// Token: 0x06002F69 RID: 12137 RVA: 0x00212EDD File Offset: 0x002110DD
		public override void OnDisable(DataContext context)
		{
			Events.UnRegisterHandler_DistanceChanged(new Events.OnDistanceChanged(this.OnDistanceChanged));
			Events.UnRegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
		}

		// Token: 0x06002F6A RID: 12138 RVA: 0x00212F04 File Offset: 0x00211104
		private void OnDistanceChanged(DataContext context, CombatCharacter mover, short distance, bool isMove, bool isForced)
		{
			bool flag = mover.GetId() != base.CharacterId || !isMove || isForced;
			if (!flag)
			{
				bool flag2 = base.IsDirect ? (distance < 0) : (distance > 0);
				if (flag2)
				{
					this._distanceAccumulator += (int)Math.Abs(distance);
					while (this._distanceAccumulator >= 10)
					{
						this._distanceAccumulator -= 10;
						this._changePower = context.Random.Next(-40, 61);
						DomainManager.SpecialEffect.InvalidateCache(context, base.CharacterId, 199);
						base.ShowSpecialEffectTips(0);
					}
				}
			}
		}

		// Token: 0x06002F6B RID: 12139 RVA: 0x00212FB8 File Offset: 0x002111B8
		private void OnCastSkillEnd(DataContext context, int charId, bool isAlly, short skillId, sbyte power, bool interrupted)
		{
			bool flag = charId != base.CharacterId || skillId != base.SkillTemplateId;
			if (!flag)
			{
				base.ShowSpecialEffectTips(1);
				base.IsDirect = !base.IsDirect;
				this._changePower = 0;
				DomainManager.SpecialEffect.InvalidateCache(context, base.CharacterId, 209);
				DomainManager.SpecialEffect.InvalidateCache(context, base.CharacterId, 199);
			}
		}

		// Token: 0x06002F6C RID: 12140 RVA: 0x00213034 File Offset: 0x00211234
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
					result = this._changePower;
				}
				else
				{
					result = 0;
				}
			}
			return result;
		}

		// Token: 0x06002F6D RID: 12141 RVA: 0x0021308C File Offset: 0x0021128C
		public override int GetModifiedValue(AffectedDataKey dataKey, int dataValue)
		{
			bool flag = dataKey.CharId != base.CharacterId || dataKey.CombatSkillId != base.SkillTemplateId;
			int result;
			if (flag)
			{
				result = dataValue;
			}
			else
			{
				bool flag2 = dataKey.FieldId == 209;
				if (flag2)
				{
					result = (base.IsDirect ? 0 : 1);
				}
				else
				{
					result = dataValue;
				}
			}
			return result;
		}

		// Token: 0x04000E0F RID: 3599
		private const sbyte PowerRandomMin = -40;

		// Token: 0x04000E10 RID: 3600
		private const sbyte PowerRandomMax = 60;

		// Token: 0x04000E11 RID: 3601
		private int _distanceAccumulator;

		// Token: 0x04000E12 RID: 3602
		private int _changePower;
	}
}
