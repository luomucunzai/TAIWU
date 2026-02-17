using System;
using System.Collections.Generic;
using Config;
using GameData.Combat.Math;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;

namespace GameData.Domains.SpecialEffect.CombatSkill.Wudangpai.Sword
{
	// Token: 0x020003C4 RID: 964
	public class ZhenWuYouLongJian : CombatSkillEffectBase
	{
		// Token: 0x06003765 RID: 14181 RVA: 0x002355F5 File Offset: 0x002337F5
		public ZhenWuYouLongJian()
		{
		}

		// Token: 0x06003766 RID: 14182 RVA: 0x002355FF File Offset: 0x002337FF
		public ZhenWuYouLongJian(CombatSkillKey skillKey) : base(skillKey, 4205, -1)
		{
		}

		// Token: 0x06003767 RID: 14183 RVA: 0x00235610 File Offset: 0x00233810
		public override void OnEnable(DataContext context)
		{
			if (this.AffectDatas == null)
			{
				this.AffectDatas = new Dictionary<AffectedDataKey, EDataModifyType>
				{
					{
						new AffectedDataKey(base.CharacterId, 175, -1, -1, -1, -1),
						EDataModifyType.AddPercent
					}
				};
			}
			this._addMovedDist = 0;
			DomainManager.Combat.AddSkillEffect(context, base.CombatChar, new SkillEffectKey(base.SkillTemplateId, base.IsDirect), 0, base.MaxEffectCount, false);
			Events.RegisterHandler_DistanceChanged(new Events.OnDistanceChanged(this.OnDistanceChanged));
			Events.RegisterHandler_PrepareSkillBegin(new Events.OnPrepareSkillBegin(this.OnPrepareSkillBegin));
		}

		// Token: 0x06003768 RID: 14184 RVA: 0x002356A3 File Offset: 0x002338A3
		public override void OnDisable(DataContext context)
		{
			Events.UnRegisterHandler_DistanceChanged(new Events.OnDistanceChanged(this.OnDistanceChanged));
			Events.UnRegisterHandler_PrepareSkillBegin(new Events.OnPrepareSkillBegin(this.OnPrepareSkillBegin));
		}

		// Token: 0x06003769 RID: 14185 RVA: 0x002356CC File Offset: 0x002338CC
		private void OnDistanceChanged(DataContext context, CombatCharacter mover, short distance, bool isMove, bool isForced)
		{
			bool flag = mover != base.CombatChar || !isMove || isForced;
			if (!flag)
			{
				bool flag2 = base.IsDirect ? (distance < 0) : (distance > 0 && base.EffectCount < (int)base.MaxEffectCount && mover.GetPreparingSkillId() < 0);
				if (flag2)
				{
					this._addMovedDist += Math.Abs(distance);
					while (this._addMovedDist >= 10 && base.EffectCount < (int)base.MaxEffectCount)
					{
						this._addMovedDist -= 10;
						DomainManager.Combat.ChangeSkillEffectCount(context, base.CombatChar, new SkillEffectKey(base.SkillTemplateId, base.IsDirect), 1, true, false);
					}
				}
				else
				{
					bool flag3 = base.EffectCount > 0 && (base.IsDirect ? (distance > 0) : (distance < 0)) && mover.GetPreparingSkillId() >= 0 && Config.CombatSkill.Instance[mover.GetPreparingSkillId()].EquipType == 1;
					if (flag3)
					{
						this._reduceMovedDist += Math.Abs(distance);
						while (this._reduceMovedDist >= 20 && base.EffectCount > 0)
						{
							this._reduceMovedDist -= 20;
							base.ReduceEffectCount(1);
						}
					}
				}
			}
		}

		// Token: 0x0600376A RID: 14186 RVA: 0x00235830 File Offset: 0x00233A30
		private void OnPrepareSkillBegin(DataContext context, int charId, bool isAlly, short skillId)
		{
			bool flag = charId != base.CharacterId || base.EffectCount <= 0 || Config.CombatSkill.Instance[skillId].EquipType != 1;
			if (!flag)
			{
				this._reduceMovedDist = 0;
				DomainManager.Combat.AddMoveDistInSkillPrepare(base.CombatChar, (short)(base.EffectCount * 10), !base.IsDirect);
				base.ShowSpecialEffectTips(0);
			}
		}

		// Token: 0x0600376B RID: 14187 RVA: 0x002358A4 File Offset: 0x00233AA4
		public override int GetModifyValue(AffectedDataKey dataKey, int currModifyValue)
		{
			bool flag = dataKey.CharId != base.CharacterId || base.CombatChar.GetPreparingSkillId() < 0 || Config.CombatSkill.Instance[base.CombatChar.GetPreparingSkillId()].EquipType != 1;
			int result;
			if (flag)
			{
				result = 0;
			}
			else
			{
				bool flag2 = dataKey.FieldId == 175;
				if (flag2)
				{
					result = 50;
				}
				else
				{
					result = 0;
				}
			}
			return result;
		}

		// Token: 0x0400102D RID: 4141
		private const sbyte AddNeedDist = 10;

		// Token: 0x0400102E RID: 4142
		private const sbyte ReduceNeedDist = 20;

		// Token: 0x0400102F RID: 4143
		private const sbyte CostMobilityReducePercent = 50;

		// Token: 0x04001030 RID: 4144
		private short _addMovedDist;

		// Token: 0x04001031 RID: 4145
		private short _reduceMovedDist;
	}
}
