using System;
using System.Collections.Generic;
using GameData.Combat.Math;
using GameData.Common;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Defense;
using GameData.GameDataBridge;

namespace GameData.Domains.SpecialEffect.CombatSkill.Kongsangpai.DefenseAndAssist
{
	// Token: 0x0200049A RID: 1178
	public class QingJiaoShengGuGong : DefenseSkillBase
	{
		// Token: 0x06003C4E RID: 15438 RVA: 0x0024CEE7 File Offset: 0x0024B0E7
		public QingJiaoShengGuGong()
		{
		}

		// Token: 0x06003C4F RID: 15439 RVA: 0x0024CEF1 File Offset: 0x0024B0F1
		public QingJiaoShengGuGong(CombatSkillKey skillKey) : base(skillKey, 10607)
		{
		}

		// Token: 0x06003C50 RID: 15440 RVA: 0x0024CF04 File Offset: 0x0024B104
		public override void OnEnable(DataContext context)
		{
			base.OnEnable(context);
			base.CreateAffectedData(282, EDataModifyType.Custom, -1);
			this._defendSkillCanAffectUid = base.ParseCombatSkillDataUid(9);
			GameDataBridge.AddPostDataModificationHandler(this._defendSkillCanAffectUid, base.DataHandlerKey, new Action<DataContext, DataUid>(this.UpdateCanAffect));
			this._affecting = false;
			this.UpdateCanAffect(context, this._defendSkillCanAffectUid);
			base.ShowSpecialEffectTips(0);
		}

		// Token: 0x06003C51 RID: 15441 RVA: 0x0024CF74 File Offset: 0x0024B174
		public override void OnDisable(DataContext context)
		{
			base.OnDisable(context);
			GameDataBridge.RemovePostDataModificationHandler(this._defendSkillCanAffectUid, base.DataHandlerKey);
			List<short> speedList = base.IsDirect ? base.CombatChar.OuterInjuryAutoHealSpeeds : base.CombatChar.InnerInjuryAutoHealSpeeds;
			speedList.Remove(2);
			bool flag = speedList.Count <= 0;
			if (flag)
			{
				base.CombatChar.ClearInjuryAutoHealProgress(context, !base.IsDirect);
			}
			DomainManager.Combat.RemoveFatalDamageMark(context, base.CombatChar, 6);
			DomainManager.Combat.AddToCheckFallenSet(base.CombatChar.GetId());
		}

		// Token: 0x06003C52 RID: 15442 RVA: 0x0024D014 File Offset: 0x0024B214
		private void UpdateCanAffect(DataContext context, DataUid dataUid)
		{
			bool canAffect = base.CanAffect;
			bool flag = this._affecting == canAffect;
			if (!flag)
			{
				List<short> speedList = base.IsDirect ? base.CombatChar.OuterInjuryAutoHealSpeeds : base.CombatChar.InnerInjuryAutoHealSpeeds;
				this._affecting = canAffect;
				bool flag2 = canAffect;
				if (flag2)
				{
					speedList.Add(2);
				}
				else
				{
					speedList.Remove(2);
				}
			}
		}

		// Token: 0x06003C53 RID: 15443 RVA: 0x0024D078 File Offset: 0x0024B278
		public override bool GetModifiedValue(AffectedDataKey dataKey, bool dataValue)
		{
			bool flag = dataKey.CharId != base.CharacterId || dataKey.FieldId != 282;
			bool result;
			if (flag)
			{
				result = dataValue;
			}
			else
			{
				result = (dataValue || this._affecting);
			}
			return result;
		}

		// Token: 0x040011BC RID: 4540
		private const sbyte AutoHealSpeed = 2;

		// Token: 0x040011BD RID: 4541
		private const sbyte RemoveFatalDamageCount = 6;

		// Token: 0x040011BE RID: 4542
		private DataUid _defendSkillCanAffectUid;

		// Token: 0x040011BF RID: 4543
		private bool _affecting;
	}
}
