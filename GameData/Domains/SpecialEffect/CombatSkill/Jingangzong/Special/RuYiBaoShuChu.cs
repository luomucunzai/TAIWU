using System;
using System.Collections.Generic;
using Config;
using GameData.Combat.Math;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;
using GameData.Utilities;

namespace GameData.Domains.SpecialEffect.CombatSkill.Jingangzong.Special
{
	// Token: 0x020004AC RID: 1196
	public class RuYiBaoShuChu : CombatSkillEffectBase
	{
		// Token: 0x06003CA9 RID: 15529 RVA: 0x0024E5B1 File Offset: 0x0024C7B1
		public RuYiBaoShuChu()
		{
		}

		// Token: 0x06003CAA RID: 15530 RVA: 0x0024E5BB File Offset: 0x0024C7BB
		public RuYiBaoShuChu(CombatSkillKey skillKey) : base(skillKey, 11306, -1)
		{
		}

		// Token: 0x06003CAB RID: 15531 RVA: 0x0024E5CC File Offset: 0x0024C7CC
		public override void OnEnable(DataContext context)
		{
			SkillEffectKey effectKey = DomainManager.Combat.GetUsingWeaponData(base.CombatChar).GetPestleEffect();
			this._addPower = (int)((effectKey.SkillId >= 0) ? (10 * (Config.CombatSkill.Instance[effectKey.SkillId].Grade + 1)) : 0);
			bool flag = this._addPower > 0;
			if (flag)
			{
				base.ShowSpecialEffectTips(0);
			}
			this.AffectDatas = new Dictionary<AffectedDataKey, EDataModifyType>();
			this.AffectDatas.Add(new AffectedDataKey(base.CharacterId, 199, base.SkillTemplateId, -1, -1, -1), EDataModifyType.AddPercent);
			Events.RegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
		}

		// Token: 0x06003CAC RID: 15532 RVA: 0x0024E674 File Offset: 0x0024C874
		public override void OnDisable(DataContext context)
		{
			Events.UnRegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
		}

		// Token: 0x06003CAD RID: 15533 RVA: 0x0024E68C File Offset: 0x0024C88C
		private void OnCastSkillEnd(DataContext context, int charId, bool isAlly, short skillId, sbyte power, bool interrupted)
		{
			bool flag = charId != base.CharacterId || skillId != base.SkillTemplateId;
			if (!flag)
			{
				bool flag2 = !interrupted;
				if (flag2)
				{
					bool flag3 = base.PowerMatchAffectRequire((int)power, 0);
					if (flag3)
					{
						sbyte stateType = base.IsDirect ? 1 : 2;
						CombatStateCollection stateCollection = base.CombatChar.GetCombatStateCollection(stateType);
						bool flag4 = stateCollection.StateDict.Count > 0;
						if (flag4)
						{
							List<short> stateIdList = ObjectPool<List<short>>.Instance.Get();
							stateIdList.Clear();
							stateIdList.AddRange(stateCollection.StateDict.Keys);
							for (int i = 0; i < stateIdList.Count; i++)
							{
								short stateId = stateIdList[i];
								int changePower = (int)stateCollection.StateDict[stateId].Item1 * RuYiBaoShuChu.StatePowerChangePercent;
								bool flag5 = changePower > 0;
								if (flag5)
								{
									DomainManager.Combat.AddCombatState(context, base.CombatChar, stateType, stateId, base.IsDirect ? changePower : (-changePower), false, false);
								}
							}
							ObjectPool<List<short>>.Instance.Return(stateIdList);
							base.ShowSpecialEffectTips(1);
						}
					}
					DomainManager.Combat.GetUsingWeaponData(base.CombatChar).RemovePestleEffect(context);
				}
				base.RemoveSelf(context);
			}
		}

		// Token: 0x06003CAE RID: 15534 RVA: 0x0024E7E8 File Offset: 0x0024C9E8
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

		// Token: 0x040011D8 RID: 4568
		private const sbyte AddPowerUnit = 10;

		// Token: 0x040011D9 RID: 4569
		private static readonly CValuePercent StatePowerChangePercent = 75;

		// Token: 0x040011DA RID: 4570
		private int _addPower;
	}
}
