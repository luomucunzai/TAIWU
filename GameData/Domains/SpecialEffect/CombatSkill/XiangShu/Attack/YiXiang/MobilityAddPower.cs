using System;
using GameData.Combat.Math;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;

namespace GameData.Domains.SpecialEffect.CombatSkill.XiangShu.Attack.YiXiang
{
	// Token: 0x020002C9 RID: 713
	public class MobilityAddPower : CombatSkillEffectBase
	{
		// Token: 0x0600327F RID: 12927 RVA: 0x0021FADF File Offset: 0x0021DCDF
		protected MobilityAddPower()
		{
		}

		// Token: 0x06003280 RID: 12928 RVA: 0x0021FAE9 File Offset: 0x0021DCE9
		protected MobilityAddPower(CombatSkillKey skillKey, int type) : base(skillKey, type, -1)
		{
		}

		// Token: 0x06003281 RID: 12929 RVA: 0x0021FAF6 File Offset: 0x0021DCF6
		public override void OnEnable(DataContext context)
		{
			Events.RegisterHandler_PrepareSkillEnd(new Events.OnPrepareSkillEnd(this.OnPrepareSkillEnd));
			Events.RegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
		}

		// Token: 0x06003282 RID: 12930 RVA: 0x0021FB1D File Offset: 0x0021DD1D
		public override void OnDisable(DataContext context)
		{
			Events.UnRegisterHandler_PrepareSkillEnd(new Events.OnPrepareSkillEnd(this.OnPrepareSkillEnd));
			Events.UnRegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
		}

		// Token: 0x06003283 RID: 12931 RVA: 0x0021FB44 File Offset: 0x0021DD44
		private void OnPrepareSkillEnd(DataContext context, int charId, bool isAlly, short skillId)
		{
			bool flag = charId != base.CharacterId || skillId != base.SkillTemplateId;
			if (!flag)
			{
				this._addPower = (int)this.AddPowerUnit * base.CombatChar.GetMobilityValue() * 100 / MoveSpecialConstants.MaxMobility;
				bool flag2 = this._addPower <= 0;
				if (!flag2)
				{
					base.AppendAffectedData(context, 199, EDataModifyType.AddPercent, base.SkillTemplateId);
					base.ShowSpecialEffectTips(1);
				}
			}
		}

		// Token: 0x06003284 RID: 12932 RVA: 0x0021FBC0 File Offset: 0x0021DDC0
		private void OnCastSkillEnd(DataContext context, int charId, bool isAlly, short skillId, sbyte power, bool interrupted)
		{
			bool flag = charId != base.CharacterId || skillId != base.SkillTemplateId;
			if (!flag)
			{
				bool flag2 = base.PowerMatchAffectRequire((int)power, 0);
				if (flag2)
				{
					base.ChangeMobilityValue(context, base.CombatChar, MoveSpecialConstants.MaxMobility);
					base.ShowSpecialEffectTips(0);
				}
				base.RemoveSelf(context);
			}
		}

		// Token: 0x06003285 RID: 12933 RVA: 0x0021FC20 File Offset: 0x0021DE20
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

		// Token: 0x04000EF2 RID: 3826
		protected sbyte AddPowerUnit;

		// Token: 0x04000EF3 RID: 3827
		private int _addPower;
	}
}
