using System;
using System.Collections.Generic;
using GameData.Combat.Math;
using GameData.Common;
using GameData.Domains.CombatSkill;

namespace GameData.Domains.SpecialEffect.CombatSkill.Jingangzong.Neigong
{
	// Token: 0x020004B8 RID: 1208
	public class MoHeJiaLuoHuFaGong : CombatSkillEffectBase
	{
		// Token: 0x06003CED RID: 15597 RVA: 0x0024F51B File Offset: 0x0024D71B
		public MoHeJiaLuoHuFaGong()
		{
		}

		// Token: 0x06003CEE RID: 15598 RVA: 0x0024F525 File Offset: 0x0024D725
		public MoHeJiaLuoHuFaGong(CombatSkillKey skillKey) : base(skillKey, 11007, -1)
		{
		}

		// Token: 0x06003CEF RID: 15599 RVA: 0x0024F538 File Offset: 0x0024D738
		public override void OnEnable(DataContext context)
		{
			this.AffectDatas = new Dictionary<AffectedDataKey, EDataModifyType>();
			bool isDirect = base.IsDirect;
			if (isDirect)
			{
				this.AffectDatas.Add(new AffectedDataKey(base.CharacterId, 135, -1, -1, -1, -1), EDataModifyType.AddPercent);
				this.AffectDatas.Add(new AffectedDataKey(base.CharacterId, 126, -1, -1, -1, -1), EDataModifyType.Custom);
			}
			else
			{
				this.AffectDatas.Add(new AffectedDataKey(base.CharacterId, 136, -1, -1, -1, -1), EDataModifyType.AddPercent);
				this.AffectDatas.Add(new AffectedDataKey(base.CharacterId, 131, -1, -1, -1, -1), EDataModifyType.Custom);
			}
		}

		// Token: 0x06003CF0 RID: 15600 RVA: 0x0024F5E4 File Offset: 0x0024D7E4
		public override bool GetModifiedValue(AffectedDataKey dataKey, bool dataValue)
		{
			bool flag = dataKey.CharId != base.CharacterId;
			bool result;
			if (flag)
			{
				result = dataValue;
			}
			else
			{
				ushort fieldId = dataKey.FieldId;
				bool flag2 = fieldId == 126 || fieldId == 131;
				bool flag3 = flag2;
				if (flag3)
				{
					bool canAffecting = this.CanAffecting();
					bool flag4 = canAffecting;
					if (flag4)
					{
						base.ShowSpecialEffectTipsOnceInFrame(0);
					}
					result = !canAffecting;
				}
				else
				{
					result = dataValue;
				}
			}
			return result;
		}

		// Token: 0x06003CF1 RID: 15601 RVA: 0x0024F658 File Offset: 0x0024D858
		public override int GetModifyValue(AffectedDataKey dataKey, int currModifyValue)
		{
			bool flag = dataKey.CharId != base.CharacterId || !this.CanAffecting();
			int result;
			if (flag)
			{
				result = 0;
			}
			else
			{
				bool flag2 = dataKey.FieldId == 135;
				if (flag2)
				{
					result = 60;
				}
				else
				{
					bool flag3 = dataKey.FieldId == 136;
					if (flag3)
					{
						result = -30;
					}
					else
					{
						result = 0;
					}
				}
			}
			return result;
		}

		// Token: 0x06003CF2 RID: 15602 RVA: 0x0024F6BC File Offset: 0x0024D8BC
		private bool CanAffecting()
		{
			int currMarkCount = base.CombatChar.GetDefeatMarkCollection().GetTotalCount();
			int needMarkCount = (int)(GlobalConfig.NeedDefeatMarkCount[(int)DomainManager.Combat.GetCombatType()] / 2);
			return currMarkCount > needMarkCount;
		}

		// Token: 0x040011EC RID: 4588
		private const sbyte DirectChangePercent = 60;

		// Token: 0x040011ED RID: 4589
		private const sbyte ReverseChangePercent = -30;
	}
}
