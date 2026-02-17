using System;
using System.Collections.Generic;
using Config;
using GameData.Combat.Math;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Assist;
using GameData.GameDataBridge;

namespace GameData.Domains.SpecialEffect.CombatSkill.Emeipai.DefenseAndAssist
{
	// Token: 0x02000562 RID: 1378
	public class QiXingHengLian : AssistSkillBase
	{
		// Token: 0x060040B6 RID: 16566 RVA: 0x0025F810 File Offset: 0x0025DA10
		public QiXingHengLian()
		{
		}

		// Token: 0x060040B7 RID: 16567 RVA: 0x0025F81A File Offset: 0x0025DA1A
		public QiXingHengLian(CombatSkillKey skillKey) : base(skillKey, 2707)
		{
		}

		// Token: 0x060040B8 RID: 16568 RVA: 0x0025F82C File Offset: 0x0025DA2C
		public override void OnEnable(DataContext context)
		{
			base.OnEnable(context);
			this._affecting = false;
			this.AffectDatas = new Dictionary<AffectedDataKey, EDataModifyType>();
			this.AffectDatas.Add(new AffectedDataKey(base.CharacterId, base.IsDirect ? 228 : 227, -1, -1, -1, -1), EDataModifyType.Custom);
			this._tricksUid = new DataUid(8, 10, (ulong)((long)base.CharacterId), 28U);
			GameDataBridge.AddPostDataModificationHandler(this._tricksUid, base.DataHandlerKey, new Action<DataContext, DataUid>(this.OnTrickChanged));
			Events.RegisterHandler_CombatBegin(new Events.OnCombatBegin(this.OnCombatBegin));
			Events.RegisterHandler_CostBreathAndStance(new Events.OnCostBreathAndStance(this.OnCostBreathAndStance));
			Events.RegisterHandler_PrepareSkillBegin(new Events.OnPrepareSkillBegin(this.OnPrepareSkillBegin));
			Events.RegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
		}

		// Token: 0x060040B9 RID: 16569 RVA: 0x0025F904 File Offset: 0x0025DB04
		public override void OnDisable(DataContext context)
		{
			base.OnDisable(context);
			GameDataBridge.RemovePostDataModificationHandler(this._tricksUid, base.DataHandlerKey);
			Events.UnRegisterHandler_CombatBegin(new Events.OnCombatBegin(this.OnCombatBegin));
			Events.UnRegisterHandler_CostBreathAndStance(new Events.OnCostBreathAndStance(this.OnCostBreathAndStance));
			Events.UnRegisterHandler_PrepareSkillBegin(new Events.OnPrepareSkillBegin(this.OnPrepareSkillBegin));
			Events.UnRegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
		}

		// Token: 0x060040BA RID: 16570 RVA: 0x0025F974 File Offset: 0x0025DB74
		private void OnCombatBegin(DataContext context)
		{
			this.UpdateCanAffect(context);
		}

		// Token: 0x060040BB RID: 16571 RVA: 0x0025F980 File Offset: 0x0025DB80
		private void OnCostBreathAndStance(DataContext context, int charId, bool isAlly, int costBreath, int costStance, short skillId)
		{
			bool flag = charId != base.CharacterId || !this._affecting || skillId < 0 || Config.CombatSkill.Instance[skillId].EquipType != 1;
			if (flag)
			{
				this.UpdateCanAffect(context);
			}
			else
			{
				GameData.Domains.CombatSkill.CombatSkill skill = DomainManager.CombatSkill.GetElement_CombatSkills(new CombatSkillKey(charId, skillId));
				this._preparingSkillId = skillId;
				this._preparingPercent = QiXingHengLian.FixedPrepareProgress + (int)(base.IsDirect ? skill.GetCostStancePercent() : skill.GetCostBreathPercent()) * QiXingHengLian.CostPercent * QiXingHengLian.PrepareProgressPercent;
				base.ShowSpecialEffectTips(0);
				base.ShowSpecialEffectTips(1);
			}
		}

		// Token: 0x060040BC RID: 16572 RVA: 0x0025FA38 File Offset: 0x0025DC38
		private void OnPrepareSkillBegin(DataContext context, int charId, bool isAlly, short skillId)
		{
			bool flag = charId != base.CharacterId || skillId != this._preparingSkillId || this._preparingPercent <= 0 || base.CombatChar.GetAutoCastingSkill();
			if (!flag)
			{
				int addProgress = base.CombatChar.SkillPrepareTotalProgress * this._preparingPercent;
				int maxProgress = base.CombatChar.SkillPrepareTotalProgress;
				base.CombatChar.SkillPrepareCurrProgress = Math.Max(base.CombatChar.SkillPrepareCurrProgress, Math.Min(base.CombatChar.SkillPrepareCurrProgress + addProgress, maxProgress));
				base.CombatChar.SetSkillPreparePercent((byte)(base.CombatChar.SkillPrepareCurrProgress * 100 / base.CombatChar.SkillPrepareTotalProgress), context);
			}
		}

		// Token: 0x060040BD RID: 16573 RVA: 0x0025FAF4 File Offset: 0x0025DCF4
		private void OnCastSkillEnd(DataContext context, int charId, bool isAlly, short skillId, sbyte power, bool interrupted)
		{
			bool flag = charId != base.CharacterId || Config.CombatSkill.Instance[skillId].EquipType != 1;
			if (!flag)
			{
				this.UpdateCanAffect(context);
			}
		}

		// Token: 0x060040BE RID: 16574 RVA: 0x0025FB33 File Offset: 0x0025DD33
		protected override void OnCanUseChanged(DataContext context, DataUid dataUid)
		{
			this.UpdateCanAffect(context);
		}

		// Token: 0x060040BF RID: 16575 RVA: 0x0025FB3E File Offset: 0x0025DD3E
		private void OnTrickChanged(DataContext context, DataUid dataUid)
		{
			this.UpdateCanAffect(context);
		}

		// Token: 0x060040C0 RID: 16576 RVA: 0x0025FB4C File Offset: 0x0025DD4C
		private void UpdateCanAffect(DataContext context)
		{
			IReadOnlyDictionary<int, sbyte> trickDict = base.CombatChar.GetTricks().Tricks;
			bool canAffect = base.CanAffect && trickDict.Count >= 7;
			bool flag = canAffect;
			if (flag)
			{
				canAffect = (base.CombatChar.UsableTrickCount >= 7);
			}
			bool flag2 = this._affecting == canAffect;
			if (!flag2)
			{
				this._affecting = canAffect;
				base.SetConstAffecting(context, canAffect);
				DomainManager.Combat.UpdateSkillCostBreathStanceCanUse(context, base.CombatChar);
			}
		}

		// Token: 0x060040C1 RID: 16577 RVA: 0x0025FBCC File Offset: 0x0025DDCC
		public override int GetModifiedValue(AffectedDataKey dataKey, int dataValue)
		{
			bool flag = dataKey.CharId != base.CharacterId || !this._affecting || Config.CombatSkill.Instance[dataKey.CombatSkillId].EquipType != 1;
			int result;
			if (flag)
			{
				result = dataValue;
			}
			else
			{
				bool flag2 = dataKey.FieldId == (base.IsDirect ? 228 : 227);
				if (flag2)
				{
					result = dataValue * QiXingHengLian.CostPercent;
				}
				else
				{
					result = 0;
				}
			}
			return result;
		}

		// Token: 0x04001300 RID: 4864
		private const sbyte RequireTrickCount = 7;

		// Token: 0x04001301 RID: 4865
		private static readonly CValuePercent CostPercent = 50;

		// Token: 0x04001302 RID: 4866
		private static readonly CValuePercent PrepareProgressPercent = 50;

		// Token: 0x04001303 RID: 4867
		private static readonly CValuePercent FixedPrepareProgress = 15;

		// Token: 0x04001304 RID: 4868
		private DataUid _tricksUid;

		// Token: 0x04001305 RID: 4869
		private bool _affecting;

		// Token: 0x04001306 RID: 4870
		private short _preparingSkillId;

		// Token: 0x04001307 RID: 4871
		private CValuePercent _preparingPercent;
	}
}
