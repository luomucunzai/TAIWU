using System;
using System.Collections.Generic;
using GameData.Combat.Math;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;

namespace GameData.Domains.SpecialEffect.CombatSkill.Xuannvpai.Music
{
	// Token: 0x0200026B RID: 619
	public class HuangZhuGe : CombatSkillEffectBase
	{
		// Token: 0x06003078 RID: 12408 RVA: 0x0021738B File Offset: 0x0021558B
		public HuangZhuGe()
		{
		}

		// Token: 0x06003079 RID: 12409 RVA: 0x002173A4 File Offset: 0x002155A4
		public HuangZhuGe(CombatSkillKey skillKey) : base(skillKey, 8303, -1)
		{
		}

		// Token: 0x0600307A RID: 12410 RVA: 0x002173C4 File Offset: 0x002155C4
		public override void OnEnable(DataContext context)
		{
			this._autoCastSkillId = -1;
			this.AffectDatas = new Dictionary<AffectedDataKey, EDataModifyType>();
			this.AffectDatas.Add(new AffectedDataKey(base.CharacterId, 199, base.SkillTemplateId, -1, -1, -1), EDataModifyType.AddPercent);
			Events.RegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
		}

		// Token: 0x0600307B RID: 12411 RVA: 0x0021741C File Offset: 0x0021561C
		public override void OnDisable(DataContext context)
		{
			Events.UnRegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
		}

		// Token: 0x0600307C RID: 12412 RVA: 0x00217434 File Offset: 0x00215634
		private void OnCastSkillEnd(DataContext context, int charId, bool isAlly, short skillId, sbyte power, bool interrupted)
		{
			bool flag = charId != base.CharacterId || skillId != base.SkillTemplateId;
			if (!flag)
			{
				bool flag2 = this._addPower == 0;
				if (flag2)
				{
					bool flag3 = base.PowerMatchAffectRequire((int)power, 0);
					if (flag3)
					{
						int maxSkillGrade = base.IsDirect ? ((int)base.CurrEnemyChar.GetTrickCount(20)) : (base.CurrEnemyChar.GetDefeatMarkCollection().MindMarkList.Count / 2);
						this._autoCastSkillId = DomainManager.Combat.GetRandomAttackSkill(base.CombatChar, 13, (sbyte)maxSkillGrade, context.Random, true, -1);
						bool flag4 = this._autoCastSkillId >= 0;
						if (flag4)
						{
							DomainManager.Combat.CastSkillFree(context, base.CombatChar, this._autoCastSkillId, ECombatCastFreePriority.Normal);
							base.ShowSpecialEffectTips(0);
							bool flag5 = this._autoCastSkillId == base.SkillTemplateId;
							if (flag5)
							{
								this._addPower = (int)this.AddPower;
								DomainManager.SpecialEffect.InvalidateCache(context, base.CharacterId, 199);
								base.ShowSpecialEffectTips(1);
							}
						}
					}
				}
				else
				{
					this._addPower = 0;
					DomainManager.SpecialEffect.InvalidateCache(context, base.CharacterId, 199);
				}
			}
		}

		// Token: 0x0600307D RID: 12413 RVA: 0x00217574 File Offset: 0x00215774
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

		// Token: 0x04000E61 RID: 3681
		private sbyte AddPower = 20;

		// Token: 0x04000E62 RID: 3682
		private int _addPower = 0;

		// Token: 0x04000E63 RID: 3683
		private short _autoCastSkillId;
	}
}
