using System;
using System.Collections.Generic;
using GameData.Combat.Math;
using GameData.Common;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;

namespace GameData.Domains.SpecialEffect.Animal.Beast.Neigong
{
	// Token: 0x0200061E RID: 1566
	public class TongTouTieBi : AnimalEffectBase
	{
		// Token: 0x170002EC RID: 748
		// (get) Token: 0x060045B0 RID: 17840 RVA: 0x00273296 File Offset: 0x00271496
		private int ReduceFatalPercent
		{
			get
			{
				return base.IsElite ? -60 : -30;
			}
		}

		// Token: 0x170002ED RID: 749
		// (get) Token: 0x060045B1 RID: 17841 RVA: 0x002732A6 File Offset: 0x002714A6
		private int AddFatalPercent
		{
			get
			{
				return base.IsElite ? 120 : 60;
			}
		}

		// Token: 0x060045B2 RID: 17842 RVA: 0x002732B6 File Offset: 0x002714B6
		public TongTouTieBi()
		{
		}

		// Token: 0x060045B3 RID: 17843 RVA: 0x002732C0 File Offset: 0x002714C0
		public TongTouTieBi(CombatSkillKey skillKey) : base(skillKey)
		{
		}

		// Token: 0x060045B4 RID: 17844 RVA: 0x002732CB File Offset: 0x002714CB
		public override void OnEnable(DataContext context)
		{
			this.AffectDatas = new Dictionary<AffectedDataKey, EDataModifyType>();
			this.AffectDatas.Add(new AffectedDataKey(base.CharacterId, 191, -1, -1, -1, -1), EDataModifyType.TotalPercent);
		}

		// Token: 0x060045B5 RID: 17845 RVA: 0x002732FA File Offset: 0x002714FA
		public override void OnDataAdded(DataContext context)
		{
			base.OnDataAdded(context);
			base.AppendAffectedAllEnemyData(context, 191, EDataModifyType.TotalPercent, -1);
		}

		// Token: 0x060045B6 RID: 17846 RVA: 0x00273314 File Offset: 0x00271514
		public override int GetModifyValue(AffectedDataKey dataKey, int currModifyValue)
		{
			bool flag = dataKey.FieldId != 191 || !base.IsCurrent;
			int result;
			if (flag)
			{
				result = 0;
			}
			else
			{
				EDamageType damageType = (EDamageType)dataKey.CustomParam1;
				bool flag2 = damageType != EDamageType.Direct;
				if (flag2)
				{
					result = 0;
				}
				else
				{
					bool flag3 = dataKey.CharId == base.CharacterId;
					if (flag3)
					{
						base.ShowSpecialEffectTipsOnceInFrame(0);
					}
					result = ((dataKey.CharId == base.CharacterId) ? this.ReduceFatalPercent : this.AddFatalPercent);
				}
			}
			return result;
		}
	}
}
