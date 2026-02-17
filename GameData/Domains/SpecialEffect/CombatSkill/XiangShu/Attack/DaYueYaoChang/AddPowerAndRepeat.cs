using System;
using System.Collections.Generic;
using GameData.Combat.Math;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;

namespace GameData.Domains.SpecialEffect.CombatSkill.XiangShu.Attack.DaYueYaoChang
{
	// Token: 0x02000318 RID: 792
	public class AddPowerAndRepeat : CombatSkillEffectBase
	{
		// Token: 0x0600341C RID: 13340 RVA: 0x00227CC7 File Offset: 0x00225EC7
		protected AddPowerAndRepeat()
		{
		}

		// Token: 0x0600341D RID: 13341 RVA: 0x00227CE0 File Offset: 0x00225EE0
		protected AddPowerAndRepeat(CombatSkillKey skillKey, int type) : base(skillKey, type, -1)
		{
		}

		// Token: 0x0600341E RID: 13342 RVA: 0x00227CFC File Offset: 0x00225EFC
		public override void OnEnable(DataContext context)
		{
			this._autoCastIndex = 0;
			this.AffectDatas = new Dictionary<AffectedDataKey, EDataModifyType>();
			this.AffectDatas.Add(new AffectedDataKey(base.CharacterId, 199, base.SkillTemplateId, -1, -1, -1), EDataModifyType.TotalPercent);
			Events.RegisterHandler_PrepareSkillBegin(new Events.OnPrepareSkillBegin(this.OnPrepareSkillBegin));
			Events.RegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
		}

		// Token: 0x0600341F RID: 13343 RVA: 0x00227D66 File Offset: 0x00225F66
		public override void OnDisable(DataContext context)
		{
			Events.UnRegisterHandler_PrepareSkillBegin(new Events.OnPrepareSkillBegin(this.OnPrepareSkillBegin));
			Events.UnRegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
		}

		// Token: 0x06003420 RID: 13344 RVA: 0x00227D90 File Offset: 0x00225F90
		private void OnPrepareSkillBegin(DataContext context, int charId, bool isAlly, short skillId)
		{
			bool flag = charId != base.CharacterId || skillId != base.SkillTemplateId || this._autoCastIndex <= 0;
			if (!flag)
			{
				DomainManager.Combat.ChangeSkillPrepareProgress(base.CombatChar, base.CombatChar.SkillPrepareTotalProgress * 50 / 100);
			}
		}

		// Token: 0x06003421 RID: 13345 RVA: 0x00227DE8 File Offset: 0x00225FE8
		private void OnCastSkillEnd(DataContext context, int charId, bool isAlly, short skillId, sbyte power, bool interrupted)
		{
			bool flag = charId != base.CharacterId || skillId != base.SkillTemplateId;
			if (!flag)
			{
				bool flag2 = !interrupted;
				if (flag2)
				{
					DomainManager.Combat.AddSkillPowerInCombat(context, this.SkillKey, new SkillEffectKey(base.SkillTemplateId, base.IsDirect), (int)this.AddPowerUnit);
					base.ShowSpecialEffectTips(0);
					bool flag3 = this._autoCastIndex < (int)this.RepeatTimes;
					if (flag3)
					{
						this._autoCastIndex++;
						DomainManager.Combat.CastSkillFree(context, base.CombatChar, base.SkillTemplateId, ECombatCastFreePriority.Normal);
						base.ShowSpecialEffectTips(1);
					}
					else
					{
						this._autoCastIndex = 0;
					}
				}
				else
				{
					this._autoCastIndex = 0;
				}
			}
		}

		// Token: 0x06003422 RID: 13346 RVA: 0x00227EB0 File Offset: 0x002260B0
		public override int GetModifyValue(AffectedDataKey dataKey, int currModifyValue)
		{
			bool flag = dataKey.CharId != base.CharacterId || dataKey.CombatSkillId != base.SkillTemplateId || this._autoCastIndex <= 0;
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
					result = (int)this.AutoCastReducePower;
				}
				else
				{
					result = 0;
				}
			}
			return result;
		}

		// Token: 0x04000F65 RID: 3941
		private sbyte AddPowerUnit = 20;

		// Token: 0x04000F66 RID: 3942
		private sbyte RepeatTimes = 2;

		// Token: 0x04000F67 RID: 3943
		private const sbyte PrepareProgressPercent = 50;

		// Token: 0x04000F68 RID: 3944
		protected sbyte AutoCastReducePower;

		// Token: 0x04000F69 RID: 3945
		private int _autoCastIndex;
	}
}
