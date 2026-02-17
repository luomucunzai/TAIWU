using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Config;
using Config.ConfigCells;
using GameData.Combat.Math;
using GameData.Common;
using GameData.Serializer;

namespace GameData.Domains.SpecialEffect.CombatSkill
{
	// Token: 0x020001B2 RID: 434
	[SerializableGameData(NotForDisplayModule = true)]
	public class CombatStateEffect : SpecialEffectBase
	{
		// Token: 0x06002C4A RID: 11338 RVA: 0x0020713D File Offset: 0x0020533D
		public CombatStateEffect()
		{
		}

		// Token: 0x06002C4B RID: 11339 RVA: 0x00207154 File Offset: 0x00205354
		public CombatStateEffect(int charId, sbyte stateType, short stateId, short power, bool reverse) : base(charId, -1)
		{
			this._stateType = stateType;
			this._reverse = reverse;
			List<CombatStateProperty> propertyList = CombatState.Instance[stateId].PropertyList;
			foreach (CombatStateProperty property in propertyList)
			{
				SpecialEffectDataFieldItem dataFieldConfig = SpecialEffectDataField.Instance[property.SpecialEffectDataId];
				ushort fieldId = AffectedDataHelper.FieldName2FieldId[dataFieldConfig.FieldName];
				this._fieldDict.Add(fieldId, new ValueTuple<short, EDataModifyType, int[]>(property.Value, (EDataModifyType)property.ModifyType, dataFieldConfig.RequireCustomParam));
			}
			this._statePower = power;
		}

		// Token: 0x06002C4C RID: 11340 RVA: 0x00207224 File Offset: 0x00205424
		public override void OnEnable(DataContext context)
		{
			this.AffectDatas = new Dictionary<AffectedDataKey, EDataModifyType>();
			foreach (KeyValuePair<ushort, ValueTuple<short, EDataModifyType, int[]>> property in this._fieldDict)
			{
				this.AffectDatas.Add(new AffectedDataKey(base.CharacterId, property.Key, -1, -1, -1, -1), property.Value.Item2);
			}
		}

		// Token: 0x06002C4D RID: 11341 RVA: 0x002072AC File Offset: 0x002054AC
		public void ChangePower(DataContext context, short power)
		{
			this._statePower = power;
			this.InvalidateCache(context);
		}

		// Token: 0x06002C4E RID: 11342 RVA: 0x002072C0 File Offset: 0x002054C0
		public void InvalidateCache(DataContext context)
		{
			foreach (ushort fieldId in this._fieldDict.Keys)
			{
				DomainManager.SpecialEffect.InvalidateCache(context, base.CharacterId, fieldId);
			}
		}

		// Token: 0x06002C4F RID: 11343 RVA: 0x00207328 File Offset: 0x00205528
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
				ValueTuple<short, EDataModifyType, int[]> fieldInfo = this._fieldDict[dataKey.FieldId];
				bool flag2 = (fieldInfo.Item3[0] >= 0 && fieldInfo.Item3[0] != dataKey.CustomParam0) || (fieldInfo.Item3[1] >= 0 && fieldInfo.Item3[1] != dataKey.CustomParam1) || (fieldInfo.Item3[2] >= 0 && fieldInfo.Item3[2] != dataKey.CustomParam2);
				if (flag2)
				{
					result = 0;
				}
				else
				{
					ValueTuple<int, int> totalPercentModify = DomainManager.SpecialEffect.GetTotalPercentModifyValue(base.CharacterId, -1, 155, (int)this._stateType, -1, -1);
					int totalPercent = Math.Max(100 + totalPercentModify.Item1 + totalPercentModify.Item2, 0);
					result = (int)(fieldInfo.Item1 * this._statePower / 100) * totalPercent / 100 * (this._reverse ? -1 : 1);
				}
			}
			return result;
		}

		// Token: 0x04000D66 RID: 3430
		private readonly sbyte _stateType;

		// Token: 0x04000D67 RID: 3431
		private readonly bool _reverse;

		// Token: 0x04000D68 RID: 3432
		[TupleElementNames(new string[]
		{
			"value",
			"modifyType",
			"requireCustomParam"
		})]
		private readonly Dictionary<ushort, ValueTuple<short, EDataModifyType, int[]>> _fieldDict = new Dictionary<ushort, ValueTuple<short, EDataModifyType, int[]>>();

		// Token: 0x04000D69 RID: 3433
		private short _statePower;
	}
}
