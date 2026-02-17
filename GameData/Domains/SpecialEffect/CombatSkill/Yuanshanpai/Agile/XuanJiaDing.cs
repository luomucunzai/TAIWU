using System;
using System.Collections.Generic;
using GameData.Combat.Math;
using GameData.Common;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Agile;

namespace GameData.Domains.SpecialEffect.CombatSkill.Yuanshanpai.Agile
{
	// Token: 0x02000217 RID: 535
	public class XuanJiaDing : AgileSkillBase
	{
		// Token: 0x06002F04 RID: 12036 RVA: 0x0021145D File Offset: 0x0020F65D
		private bool IsLeg(short skillTemplateId)
		{
			return DomainManager.CombatSkill.GetSkillType(base.CharacterId, skillTemplateId) == 5;
		}

		// Token: 0x06002F05 RID: 12037 RVA: 0x00211473 File Offset: 0x0020F673
		public XuanJiaDing()
		{
		}

		// Token: 0x06002F06 RID: 12038 RVA: 0x0021147D File Offset: 0x0020F67D
		public XuanJiaDing(CombatSkillKey skillKey) : base(skillKey, 5403)
		{
			this.ListenCanAffectChange = true;
		}

		// Token: 0x06002F07 RID: 12039 RVA: 0x00211494 File Offset: 0x0020F694
		public override void OnEnable(DataContext context)
		{
			base.OnEnable(context);
			this._affecting = false;
			this.OnMoveSkillCanAffectChanged(context, default(DataUid));
			this.AffectDatas = new Dictionary<AffectedDataKey, EDataModifyType>();
			this.AffectDatas.Add(new AffectedDataKey(base.CharacterId, 149, -1, -1, -1, -1), EDataModifyType.Custom);
			this.AffectDatas.Add(new AffectedDataKey(base.CharacterId, 215, -1, -1, -1, -1), EDataModifyType.Custom);
			bool isDirect = base.IsDirect;
			if (isDirect)
			{
				this.AffectDatas.Add(new AffectedDataKey(base.CharacterId, 207, -1, -1, -1, -1), EDataModifyType.TotalPercent);
			}
			else
			{
				this.AffectDatas.Add(new AffectedDataKey(base.CharacterId, 205, -1, -1, -1, -1), EDataModifyType.TotalPercent);
				this.AffectDatas.Add(new AffectedDataKey(base.CharacterId, 206, -1, -1, -1, -1), EDataModifyType.TotalPercent);
			}
			base.ShowSpecialEffectTips(0);
			base.ShowSpecialEffectTips(1);
		}

		// Token: 0x06002F08 RID: 12040 RVA: 0x00211590 File Offset: 0x0020F790
		protected override void OnMoveSkillCanAffectChanged(DataContext context, DataUid dataUid)
		{
			bool canAffect = base.CanAffect;
			bool flag = this._affecting == canAffect;
			if (!flag)
			{
				this._affecting = canAffect;
				bool isDirect = base.IsDirect;
				if (isDirect)
				{
					DomainManager.SpecialEffect.InvalidateCache(context, base.CharacterId, 207);
				}
				else
				{
					DomainManager.SpecialEffect.InvalidateCache(context, base.CharacterId, 205);
					DomainManager.SpecialEffect.InvalidateCache(context, base.CharacterId, 206);
				}
			}
		}

		// Token: 0x06002F09 RID: 12041 RVA: 0x00211610 File Offset: 0x0020F810
		public override int GetModifyValue(AffectedDataKey dataKey, int currModifyValue)
		{
			bool flag = !this._affecting || !this.IsLeg(dataKey.CombatSkillId);
			int result;
			if (flag)
			{
				result = 0;
			}
			else
			{
				ushort fieldId = dataKey.FieldId;
				bool flag2 = fieldId - 205 <= 2;
				bool flag3 = flag2;
				if (flag3)
				{
					result = -50;
				}
				else
				{
					result = 0;
				}
			}
			return result;
		}

		// Token: 0x06002F0A RID: 12042 RVA: 0x0021166C File Offset: 0x0020F86C
		public override bool GetModifiedValue(AffectedDataKey dataKey, bool dataValue)
		{
			bool flag = !this._affecting;
			bool result;
			if (flag)
			{
				result = dataValue;
			}
			else
			{
				bool flag2 = dataKey.FieldId == 149 && dataKey.CustomParam0 >= 0 && DomainManager.Combat.GetElement_CombatCharacterDict(dataKey.CustomParam0).IsAlly != base.CombatChar.IsAlly;
				if (flag2)
				{
					result = false;
				}
				else
				{
					bool flag3 = dataKey.FieldId == 215 && dataKey.CharId == base.CharacterId && this.IsLeg(dataKey.CombatSkillId);
					result = (!flag3 && dataValue);
				}
			}
			return result;
		}

		// Token: 0x04000DF6 RID: 3574
		private const int ReduceCostPercent = -50;

		// Token: 0x04000DF7 RID: 3575
		private bool _affecting;
	}
}
