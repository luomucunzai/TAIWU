using System;
using GameData.Combat.Math;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;

namespace GameData.Domains.SpecialEffect.CombatSkill.Shixiangmen.FistAndPalm
{
	// Token: 0x020003F9 RID: 1017
	public class DaZhuoShou : CombatSkillEffectBase
	{
		// Token: 0x0600388C RID: 14476 RVA: 0x0023ADD3 File Offset: 0x00238FD3
		public DaZhuoShou()
		{
		}

		// Token: 0x0600388D RID: 14477 RVA: 0x0023ADDD File Offset: 0x00238FDD
		public DaZhuoShou(CombatSkillKey skillKey) : base(skillKey, 6108, -1)
		{
			this._hitAvoidChangeAccumulator = 0;
		}

		// Token: 0x0600388E RID: 14478 RVA: 0x0023ADF5 File Offset: 0x00238FF5
		public override void OnEnable(DataContext context)
		{
			Events.RegisterHandler_PrepareSkillBegin(new Events.OnPrepareSkillBegin(this.OnPrepareSkillBegin));
			Events.RegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
		}

		// Token: 0x0600388F RID: 14479 RVA: 0x0023AE1C File Offset: 0x0023901C
		public override void OnDisable(DataContext context)
		{
			Events.UnRegisterHandler_PrepareSkillBegin(new Events.OnPrepareSkillBegin(this.OnPrepareSkillBegin));
			Events.UnRegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
		}

		// Token: 0x06003890 RID: 14480 RVA: 0x0023AE44 File Offset: 0x00239044
		private void OnPrepareSkillBegin(DataContext context, int charId, bool isAlly, short skillId)
		{
			bool flag = charId != base.CharacterId || skillId != base.SkillTemplateId;
			if (!flag)
			{
				bool flag2 = this._hitAvoidChangeAccumulator != 0;
				if (flag2)
				{
					DomainManager.Combat.ChangeSkillPrepareProgress(base.CombatChar, base.CombatChar.SkillPrepareTotalProgress * 50 / 100);
				}
			}
		}

		// Token: 0x06003891 RID: 14481 RVA: 0x0023AEA0 File Offset: 0x002390A0
		private void OnCastSkillEnd(DataContext context, int charId, bool isAlly, short skillId, sbyte power, bool interrupted)
		{
			bool flag = charId != base.CharacterId || skillId != base.SkillTemplateId;
			if (!flag)
			{
				bool flag2 = !interrupted && DomainManager.Combat.CanCastSkill(base.CombatChar, base.SkillTemplateId, true, true);
				if (flag2)
				{
					ushort fieldId = base.IsDirect ? 56 : 60;
					bool flag3 = this._hitAvoidChangeAccumulator == 0 && (this.AffectDatas == null || !this.AffectDatas.ContainsKey(new AffectedDataKey(base.CharacterId, fieldId, -1, -1, -1, -1)));
					if (flag3)
					{
						base.AppendAffectedData(context, base.CharacterId, fieldId, EDataModifyType.AddPercent, -1);
					}
					bool isDirect = base.IsDirect;
					if (isDirect)
					{
						this._hitAvoidChangeAccumulator += 30;
					}
					else
					{
						this._hitAvoidChangeAccumulator -= 30;
					}
					DomainManager.Combat.CastSkillFree(context, base.CombatChar, base.SkillTemplateId, ECombatCastFreePriority.Normal);
					base.ShowSpecialEffectTips(0);
				}
				else
				{
					this._hitAvoidChangeAccumulator = 0;
					base.ClearAffectedData(context);
				}
			}
		}

		// Token: 0x06003892 RID: 14482 RVA: 0x0023AFB0 File Offset: 0x002391B0
		public override int GetModifyValue(AffectedDataKey dataKey, int currModifyValue)
		{
			bool flag = dataKey.CharId != base.CharacterId || dataKey.CustomParam0 != (int)base.SkillTemplateId;
			int result;
			if (flag)
			{
				result = 0;
			}
			else
			{
				bool flag2 = dataKey.FieldId == 56 || dataKey.FieldId == 60;
				if (flag2)
				{
					result = this._hitAvoidChangeAccumulator;
				}
				else
				{
					result = 0;
				}
			}
			return result;
		}

		// Token: 0x0400108E RID: 4238
		private const sbyte HitAvoidChangePercent = 30;

		// Token: 0x0400108F RID: 4239
		private const sbyte PrepareProgressPercent = 50;

		// Token: 0x04001090 RID: 4240
		private int _hitAvoidChangeAccumulator;
	}
}
