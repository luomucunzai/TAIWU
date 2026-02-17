using System;
using System.Collections.Generic;
using Config;
using GameData.Combat.Math;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.CombatSkill;

namespace GameData.Domains.SpecialEffect.CombatSkill.Ranshanpai.Finger
{
	// Token: 0x0200045B RID: 1115
	public class ChiShuShi : CombatSkillEffectBase
	{
		// Token: 0x06003AC2 RID: 15042 RVA: 0x002450BE File Offset: 0x002432BE
		public ChiShuShi()
		{
		}

		// Token: 0x06003AC3 RID: 15043 RVA: 0x002450D3 File Offset: 0x002432D3
		public ChiShuShi(CombatSkillKey skillKey) : base(skillKey, 7103, -1)
		{
		}

		// Token: 0x06003AC4 RID: 15044 RVA: 0x002450EF File Offset: 0x002432EF
		public override void OnEnable(DataContext context)
		{
			Events.RegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
		}

		// Token: 0x06003AC5 RID: 15045 RVA: 0x00245104 File Offset: 0x00243304
		public override void OnDisable(DataContext context)
		{
			Events.UnRegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
		}

		// Token: 0x06003AC6 RID: 15046 RVA: 0x0024511C File Offset: 0x0024331C
		private void OnCastSkillEnd(DataContext context, int charId, bool isAlly, short skillId, sbyte power, bool interrupted)
		{
			bool flag = interrupted || Config.CombatSkill.Instance[skillId].EquipType != 1;
			if (!flag)
			{
				CombatSkillKey skillKey = new CombatSkillKey(charId, skillId);
				bool flag2 = base.EffectCount > 0 && (base.IsDirect ? (charId == base.CharacterId && !base.PowerMatchAffectRequire((int)power, 1)) : (isAlly != base.CombatChar.IsAlly && base.PowerMatchAffectRequire((int)power, 1)));
				if (flag2)
				{
					int changePower = base.IsDirect ? 20 : -20;
					bool flag3 = this._changePowerDict.ContainsKey(skillKey);
					if (flag3)
					{
						Dictionary<CombatSkillKey, int> changePowerDict = this._changePowerDict;
						CombatSkillKey key = skillKey;
						changePowerDict[key] += changePower;
					}
					else
					{
						this._changePowerDict.Add(skillKey, changePower);
					}
					bool flag4 = this.AffectDatas != null && this.AffectDatas.ContainsKey(new AffectedDataKey(charId, 199, -1, -1, -1, -1));
					if (flag4)
					{
						DomainManager.SpecialEffect.InvalidateCache(context, charId, 199);
					}
					else
					{
						base.AppendAffectedData(context, charId, 199, EDataModifyType.AddPercent, -1);
					}
					base.ReduceEffectCount(1);
					base.ShowSpecialEffectTips(0);
				}
				bool flag5 = this._changePowerDict.ContainsKey(skillKey) && (base.IsDirect ? (charId == base.CharacterId && base.PowerMatchAffectRequire((int)power, 1)) : (isAlly != base.CombatChar.IsAlly && !base.PowerMatchAffectRequire((int)power, 1)));
				if (flag5)
				{
					this._changePowerDict.Remove(skillKey);
					DomainManager.SpecialEffect.InvalidateCache(context, charId, 199);
				}
				bool flag6 = charId == base.CharacterId && skillId == base.SkillTemplateId && base.PowerMatchAffectRequire((int)power, 0);
				if (flag6)
				{
					base.AddMaxEffectCount(true);
				}
			}
		}

		// Token: 0x06003AC7 RID: 15047 RVA: 0x00245300 File Offset: 0x00243500
		public override int GetModifyValue(AffectedDataKey dataKey, int currModifyValue)
		{
			CombatSkillKey skillKey = new CombatSkillKey(dataKey.CharId, dataKey.CombatSkillId);
			bool flag = dataKey.FieldId == 199 && this._changePowerDict.ContainsKey(skillKey);
			int result;
			if (flag)
			{
				result = this._changePowerDict[skillKey];
			}
			else
			{
				result = 0;
			}
			return result;
		}

		// Token: 0x04001133 RID: 4403
		private const sbyte ChangePowerUnit = 20;

		// Token: 0x04001134 RID: 4404
		private readonly Dictionary<CombatSkillKey, int> _changePowerDict = new Dictionary<CombatSkillKey, int>();
	}
}
