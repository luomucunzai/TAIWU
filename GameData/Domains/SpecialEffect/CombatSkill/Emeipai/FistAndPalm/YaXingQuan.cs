using System;
using System.Collections.Generic;
using GameData.Combat.Math;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.CombatSkill;

namespace GameData.Domains.SpecialEffect.CombatSkill.Emeipai.FistAndPalm
{
	// Token: 0x02000554 RID: 1364
	public class YaXingQuan : CombatSkillEffectBase
	{
		// Token: 0x0600405D RID: 16477 RVA: 0x0025DD21 File Offset: 0x0025BF21
		public YaXingQuan()
		{
		}

		// Token: 0x0600405E RID: 16478 RVA: 0x0025DD2B File Offset: 0x0025BF2B
		public YaXingQuan(CombatSkillKey skillKey) : base(skillKey, 2101, -1)
		{
		}

		// Token: 0x0600405F RID: 16479 RVA: 0x0025DD3C File Offset: 0x0025BF3C
		public override void OnEnable(DataContext context)
		{
			this.AffectDatas = new Dictionary<AffectedDataKey, EDataModifyType>();
			this.AffectDatas.Add(new AffectedDataKey(base.CharacterId, 199, base.SkillTemplateId, -1, -1, -1), EDataModifyType.AddPercent);
			Events.RegisterHandler_PrepareSkillEnd(new Events.OnPrepareSkillEnd(this.OnPrepareSkillEnd));
			Events.RegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
		}

		// Token: 0x06004060 RID: 16480 RVA: 0x0025DD9F File Offset: 0x0025BF9F
		public override void OnDisable(DataContext context)
		{
			Events.UnRegisterHandler_PrepareSkillEnd(new Events.OnPrepareSkillEnd(this.OnPrepareSkillEnd));
			Events.UnRegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
		}

		// Token: 0x06004061 RID: 16481 RVA: 0x0025DDC8 File Offset: 0x0025BFC8
		private void OnPrepareSkillEnd(DataContext context, int charId, bool isAlly, short skillId)
		{
			bool flag = charId != base.CharacterId || skillId != base.SkillTemplateId;
			if (!flag)
			{
				int hazardPercent = (int)base.CurrEnemyChar.AiController.GetHazardPercent();
				this._addPower = (base.IsDirect ? (100 - hazardPercent) : hazardPercent) * YaXingQuan.AddPowerPercent;
				DomainManager.SpecialEffect.InvalidateCache(context, base.CharacterId, 199);
			}
		}

		// Token: 0x06004062 RID: 16482 RVA: 0x0025DE44 File Offset: 0x0025C044
		private void OnCastSkillEnd(DataContext context, int charId, bool isAlly, short skillId, sbyte power, bool interrupted)
		{
			bool flag = charId != base.CharacterId || skillId != base.SkillTemplateId;
			if (!flag)
			{
				bool flag2 = !base.PowerMatchAffectRequire((int)power, 0) && !interrupted;
				if (flag2)
				{
					base.CurrEnemyChar.AiController.ChangeHazardValue(context, base.IsDirect ? -200 : 200);
					base.ShowSpecialEffectTips(0);
				}
				base.RemoveSelf(context);
			}
		}

		// Token: 0x06004063 RID: 16483 RVA: 0x0025DEC0 File Offset: 0x0025C0C0
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
					result = this._addPower;
				}
				else
				{
					result = 0;
				}
			}
			return result;
		}

		// Token: 0x040012E8 RID: 4840
		private const short ChangeHazard = 200;

		// Token: 0x040012E9 RID: 4841
		private static readonly CValuePercent AddPowerPercent = 40;

		// Token: 0x040012EA RID: 4842
		private int _addPower;
	}
}
