using System;
using System.Collections.Generic;
using Config;
using GameData.Combat.Math;
using GameData.Common;
using GameData.Domains.CombatSkill;
using GameData.GameDataBridge;

namespace GameData.Domains.SpecialEffect.LegendaryBook.NpcEffect
{
	// Token: 0x02000149 RID: 329
	public class Neigong : FeatureEffectBase
	{
		// Token: 0x06002AB6 RID: 10934 RVA: 0x00203636 File Offset: 0x00201836
		public Neigong()
		{
		}

		// Token: 0x06002AB7 RID: 10935 RVA: 0x00203640 File Offset: 0x00201840
		public Neigong(int charId, short featureId) : base(charId, featureId, 41400)
		{
		}

		// Token: 0x06002AB8 RID: 10936 RVA: 0x00203654 File Offset: 0x00201854
		public override void OnEnable(DataContext context)
		{
			this._neiliTypeUid = new DataUid(4, 0, (ulong)((long)base.CharacterId), 112U);
			GameDataBridge.AddPostDataModificationHandler(this._neiliTypeUid, base.DataHandlerKey, new Action<DataContext, DataUid>(this.OnNeiliTypeChanged));
			this.OnNeiliTypeChanged(context, default(DataUid));
			this.AffectDatas = new Dictionary<AffectedDataKey, EDataModifyType>();
			this.AffectDatas.Add(new AffectedDataKey(base.CharacterId, 199, -1, -1, -1, -1), EDataModifyType.Add);
		}

		// Token: 0x06002AB9 RID: 10937 RVA: 0x002036D3 File Offset: 0x002018D3
		public override void OnDisable(DataContext context)
		{
			GameDataBridge.RemovePostDataModificationHandler(this._neiliTypeUid, base.DataHandlerKey);
		}

		// Token: 0x06002ABA RID: 10938 RVA: 0x002036E8 File Offset: 0x002018E8
		private void OnNeiliTypeChanged(DataContext context, DataUid dataUid)
		{
			this._neiliFiveElementsType = (sbyte)NeiliType.Instance[this.CharObj.GetNeiliType()].FiveElements;
			bool flag = this.AffectDatas != null;
			if (flag)
			{
				DomainManager.SpecialEffect.InvalidateCache(context, base.CharacterId, 199);
			}
		}

		// Token: 0x06002ABB RID: 10939 RVA: 0x0020373C File Offset: 0x0020193C
		public override int GetModifyValue(AffectedDataKey dataKey, int currModifyValue)
		{
			bool flag = dataKey.CharId != base.CharacterId;
			int result;
			if (flag)
			{
				result = 0;
			}
			else
			{
				sbyte otherSkillType = Config.CombatSkill.Instance[dataKey.CombatSkillId].FiveElements;
				bool flag2 = dataKey.FieldId == 199;
				if (flag2)
				{
					bool flag3 = this._neiliFiveElementsType == 5;
					if (flag3)
					{
						return (this._neiliFiveElementsType == otherSkillType) ? 80 : 0;
					}
					bool flag4 = otherSkillType != 5;
					if (flag4)
					{
						return (this._neiliFiveElementsType == otherSkillType) ? 80 : ((this._neiliFiveElementsType == FiveElementsType.Producing[(int)otherSkillType] || this._neiliFiveElementsType == FiveElementsType.Produced[(int)otherSkillType]) ? 40 : -40);
					}
				}
				result = 0;
			}
			return result;
		}

		// Token: 0x04000D0E RID: 3342
		private const sbyte SameAddPower = 80;

		// Token: 0x04000D0F RID: 3343
		private const sbyte ProduceAddPower = 40;

		// Token: 0x04000D10 RID: 3344
		private const sbyte CounterReducePower = -40;

		// Token: 0x04000D11 RID: 3345
		private sbyte _neiliFiveElementsType;

		// Token: 0x04000D12 RID: 3346
		private DataUid _neiliTypeUid;
	}
}
