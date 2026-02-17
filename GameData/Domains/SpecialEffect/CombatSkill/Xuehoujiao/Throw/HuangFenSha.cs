using System;
using System.Collections.Generic;
using Config;
using GameData.Combat.Math;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;

namespace GameData.Domains.SpecialEffect.CombatSkill.Xuehoujiao.Throw
{
	// Token: 0x0200021A RID: 538
	public class HuangFenSha : CombatSkillEffectBase
	{
		// Token: 0x06002F15 RID: 12053 RVA: 0x00211A43 File Offset: 0x0020FC43
		public HuangFenSha()
		{
		}

		// Token: 0x06002F16 RID: 12054 RVA: 0x00211A4D File Offset: 0x0020FC4D
		public HuangFenSha(CombatSkillKey skillKey) : base(skillKey, 15403, -1)
		{
		}

		// Token: 0x06002F17 RID: 12055 RVA: 0x00211A60 File Offset: 0x0020FC60
		public override void OnEnable(DataContext context)
		{
			this._isLegSkill = true;
			base.CreateAffectedData(221, EDataModifyType.Custom, base.SkillTemplateId);
			base.CreateAffectedData(207, EDataModifyType.Custom, base.SkillTemplateId);
			base.CreateAffectedData(208, EDataModifyType.Custom, base.SkillTemplateId);
			Events.RegisterHandler_CombatStateMachineUpdateEnd(new Events.OnCombatStateMachineUpdateEnd(this.OnCombatStateMachineUpdateEnd));
			Events.RegisterHandler_PrepareSkillBegin(new Events.OnPrepareSkillBegin(this.OnPrepareSkillBegin));
			Events.RegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
			Events.RegisterHandler_DistanceChanged(new Events.OnDistanceChanged(this.OnDistanceChanged));
		}

		// Token: 0x06002F18 RID: 12056 RVA: 0x00211AF8 File Offset: 0x0020FCF8
		public override void OnDisable(DataContext context)
		{
			Events.UnRegisterHandler_CombatStateMachineUpdateEnd(new Events.OnCombatStateMachineUpdateEnd(this.OnCombatStateMachineUpdateEnd));
			Events.UnRegisterHandler_PrepareSkillBegin(new Events.OnPrepareSkillBegin(this.OnPrepareSkillBegin));
			Events.UnRegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
			Events.UnRegisterHandler_DistanceChanged(new Events.OnDistanceChanged(this.OnDistanceChanged));
		}

		// Token: 0x06002F19 RID: 12057 RVA: 0x00211B50 File Offset: 0x0020FD50
		private void OnCombatStateMachineUpdateEnd(DataContext context, CombatCharacter combatChar)
		{
			bool flag = combatChar.GetId() != base.CharacterId || !DomainManager.Combat.IsCharInCombat(base.CharacterId, true);
			if (!flag)
			{
				this.CheckIsLegSkill(context);
			}
		}

		// Token: 0x06002F1A RID: 12058 RVA: 0x00211B94 File Offset: 0x0020FD94
		private void OnPrepareSkillBegin(DataContext context, int charId, bool isAlly, short skillId)
		{
			bool flag = charId != base.CharacterId || skillId != base.SkillTemplateId;
			if (!flag)
			{
				bool autoCastingSkill = base.CombatChar.GetAutoCastingSkill();
				if (autoCastingSkill)
				{
					DomainManager.Combat.ChangeSkillPrepareProgress(base.CombatChar, base.CombatChar.SkillPrepareTotalProgress * 50 / 100);
				}
				else
				{
					bool isLegSkill = this._isLegSkill;
					if (isLegSkill)
					{
						base.ShowSpecialEffectTips(0);
					}
				}
			}
		}

		// Token: 0x06002F1B RID: 12059 RVA: 0x00211C08 File Offset: 0x0020FE08
		private void OnCastSkillEnd(DataContext context, int charId, bool isAlly, short skillId, sbyte power, bool interrupted)
		{
			bool flag = charId != base.CharacterId || skillId != base.SkillTemplateId;
			if (!flag)
			{
				bool flag2 = base.PowerMatchAffectRequire((int)power, 0) && !base.CombatChar.GetAutoCastingSkill();
				if (flag2)
				{
					this._movedDistance = 0;
					base.AddMaxEffectCount(true);
				}
				this.CheckIsLegSkill(context);
			}
		}

		// Token: 0x06002F1C RID: 12060 RVA: 0x00211C70 File Offset: 0x0020FE70
		private void OnDistanceChanged(DataContext context, CombatCharacter mover, short distance, bool isMove, bool isForced)
		{
			bool flag = base.EffectCount <= 0 || mover != base.CombatChar || !isMove || isForced || !(base.IsDirect ? (distance < 0) : (distance > 0));
			if (!flag)
			{
				this._movedDistance += (int)Math.Abs(distance);
				bool flag2 = this._movedDistance < 10;
				if (!flag2)
				{
					this._movedDistance = 0;
					bool flag3 = DomainManager.Combat.CanCastSkill(base.CombatChar, base.SkillTemplateId, true, false);
					if (flag3)
					{
						DomainManager.Combat.CastSkillFree(context, base.CombatChar, base.SkillTemplateId, ECombatCastFreePriority.Normal);
						base.ShowSpecialEffectTips(1);
					}
					base.ReduceEffectCount(1);
				}
			}
		}

		// Token: 0x06002F1D RID: 12061 RVA: 0x00211D2C File Offset: 0x0020FF2C
		private void CheckIsLegSkill(DataContext context)
		{
			bool flag = base.CombatChar.GetPreparingSkillId() == base.SkillTemplateId || base.CombatChar.NeedUseSkillId == base.SkillTemplateId;
			if (!flag)
			{
				bool cache = this._isLegSkill;
				this._isLegSkill = false;
				bool isLegSkill = !DomainManager.Combat.HasNeedTrick(base.CombatChar, base.SkillInstance, false);
				this._isLegSkill = cache;
				bool flag2 = this._isLegSkill == isLegSkill;
				if (!flag2)
				{
					this._isLegSkill = isLegSkill;
					DomainManager.SpecialEffect.InvalidateCache(context, base.CharacterId, 221);
					DomainManager.SpecialEffect.InvalidateCache(context, base.CharacterId, 207);
					DomainManager.SpecialEffect.InvalidateCache(context, base.CharacterId, 208);
					DomainManager.Combat.UpdateSkillCanUse(context, base.CombatChar, base.SkillTemplateId);
				}
			}
		}

		// Token: 0x06002F1E RID: 12062 RVA: 0x00211E10 File Offset: 0x00210010
		public override int GetModifiedValue(AffectedDataKey dataKey, int dataValue)
		{
			bool flag = dataKey.SkillKey != this.SkillKey || !this._isLegSkill || base.CombatChar.GetAutoCastingSkill();
			int result;
			if (flag)
			{
				result = dataValue;
			}
			else
			{
				ushort fieldId = dataKey.FieldId;
				if (!true)
				{
				}
				int num;
				if (fieldId != 207)
				{
					if (fieldId != 221)
					{
						num = dataValue;
					}
					else
					{
						num = 5;
					}
				}
				else
				{
					num = 30;
				}
				if (!true)
				{
				}
				result = num;
			}
			return result;
		}

		// Token: 0x06002F1F RID: 12063 RVA: 0x00211E88 File Offset: 0x00210088
		public override List<NeedTrick> GetModifiedValue(AffectedDataKey dataKey, List<NeedTrick> dataValue)
		{
			bool flag = dataKey.SkillKey != this.SkillKey || !this._isLegSkill || base.CombatChar.GetAutoCastingSkill();
			List<NeedTrick> result;
			if (flag)
			{
				result = dataValue;
			}
			else
			{
				bool flag2 = dataKey.FieldId == 208;
				if (flag2)
				{
					dataValue.Clear();
				}
				result = dataValue;
			}
			return result;
		}

		// Token: 0x04000DFB RID: 3579
		private const sbyte CostMobilityAsLegSkill = 30;

		// Token: 0x04000DFC RID: 3580
		private const sbyte RequireMoveDistance = 10;

		// Token: 0x04000DFD RID: 3581
		private const sbyte PrepareProgressPercent = 50;

		// Token: 0x04000DFE RID: 3582
		private bool _isLegSkill;

		// Token: 0x04000DFF RID: 3583
		private int _movedDistance;
	}
}
