using System;
using System.Collections.Generic;
using GameData.Combat.Math;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;

namespace GameData.Domains.SpecialEffect.CombatSkill.Shixiangmen.Neigong
{
	// Token: 0x020003F3 RID: 1011
	public class ShiSanTaiBaoHengLianGong : CombatSkillEffectBase
	{
		// Token: 0x0600386A RID: 14442 RVA: 0x0023A44D File Offset: 0x0023864D
		public ShiSanTaiBaoHengLianGong()
		{
		}

		// Token: 0x0600386B RID: 14443 RVA: 0x0023A457 File Offset: 0x00238657
		public ShiSanTaiBaoHengLianGong(CombatSkillKey skillKey) : base(skillKey, 6001, -1)
		{
		}

		// Token: 0x0600386C RID: 14444 RVA: 0x0023A468 File Offset: 0x00238668
		public override void OnEnable(DataContext context)
		{
			this._lastCastSkillId = -1;
			this._accumulateCount = 0;
			this.AffectDatas = new Dictionary<AffectedDataKey, EDataModifyType>();
			this.AffectDatas.Add(new AffectedDataKey(base.CharacterId, base.IsDirect ? 102 : 69, -1, -1, -1, -1), EDataModifyType.TotalPercent);
			Events.RegisterHandler_CastAttackSkillBegin(new Events.OnCastAttackSkillBegin(this.OnCastAttackSkillBegin));
		}

		// Token: 0x0600386D RID: 14445 RVA: 0x0023A4CB File Offset: 0x002386CB
		public override void OnDisable(DataContext context)
		{
			Events.UnRegisterHandler_CastAttackSkillBegin(new Events.OnCastAttackSkillBegin(this.OnCastAttackSkillBegin));
		}

		// Token: 0x0600386E RID: 14446 RVA: 0x0023A4E0 File Offset: 0x002386E0
		private void OnCastAttackSkillBegin(DataContext context, CombatCharacter attacker, CombatCharacter defender, short skillId)
		{
			bool flag = base.CharacterId == (base.IsDirect ? defender.GetId() : attacker.GetId()) && DomainManager.Combat.InAttackRange(attacker);
			if (flag)
			{
				bool flag2 = skillId == this._lastCastSkillId;
				if (flag2)
				{
					bool flag3 = this._accumulateCount < 3;
					if (flag3)
					{
						this._accumulateCount += 1;
					}
					base.ShowSpecialEffectTips(0);
				}
				else
				{
					this._lastCastSkillId = skillId;
					this._accumulateCount = 0;
				}
			}
		}

		// Token: 0x0600386F RID: 14447 RVA: 0x0023A568 File Offset: 0x00238768
		public override int GetModifyValue(AffectedDataKey dataKey, int currModifyValue)
		{
			bool flag = dataKey.CharId != base.CharacterId || this._accumulateCount <= 0;
			int result;
			if (flag)
			{
				result = 0;
			}
			else
			{
				bool flag2 = dataKey.FieldId == 102;
				if (flag2)
				{
					result = (int)(-15 * this._accumulateCount);
				}
				else
				{
					bool flag3 = dataKey.FieldId == 69;
					if (flag3)
					{
						result = (int)(15 * this._accumulateCount);
					}
					else
					{
						result = 0;
					}
				}
			}
			return result;
		}

		// Token: 0x0400107F RID: 4223
		private const sbyte DamageChangePercentPerCast = 15;

		// Token: 0x04001080 RID: 4224
		private const sbyte MaxAccumulateCount = 3;

		// Token: 0x04001081 RID: 4225
		private short _lastCastSkillId;

		// Token: 0x04001082 RID: 4226
		private sbyte _accumulateCount;
	}
}
