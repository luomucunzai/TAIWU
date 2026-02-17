using System;
using GameData.Combat.Math;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Character;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;

namespace GameData.Domains.SpecialEffect.CombatSkill.Common.Agile
{
	// Token: 0x020005B4 RID: 1460
	public class ChangeLegSkillHit : AgileSkillBase
	{
		// Token: 0x06004366 RID: 17254 RVA: 0x0026B3C4 File Offset: 0x002695C4
		protected ChangeLegSkillHit()
		{
		}

		// Token: 0x06004367 RID: 17255 RVA: 0x0026B3CE File Offset: 0x002695CE
		protected ChangeLegSkillHit(CombatSkillKey skillKey, int type) : base(skillKey, type)
		{
		}

		// Token: 0x06004368 RID: 17256 RVA: 0x0026B3DA File Offset: 0x002695DA
		public override void OnEnable(DataContext context)
		{
			base.OnEnable(context);
			Events.RegisterHandler_CastLegSkillWithAgile(new Events.OnCastLegSkillWithAgile(this.OnCastLegSkillWithAgile));
		}

		// Token: 0x06004369 RID: 17257 RVA: 0x0026B3F7 File Offset: 0x002695F7
		public override void OnDisable(DataContext context)
		{
			base.OnDisable(context);
			Events.UnRegisterHandler_CastLegSkillWithAgile(new Events.OnCastLegSkillWithAgile(this.OnCastLegSkillWithAgile));
		}

		// Token: 0x0600436A RID: 17258 RVA: 0x0026B414 File Offset: 0x00269614
		private void OnCastLegSkillWithAgile(DataContext context, CombatCharacter combatChar, short legSkillId)
		{
			bool flag = !base.CanAffect || combatChar.GetId() != base.CharacterId;
			if (!flag)
			{
				this.AutoRemove = false;
				this._affectingLegSkill = legSkillId;
				bool flag2 = this.AffectDatas == null || this.AffectDatas.Count == 0;
				if (flag2)
				{
					bool isDirect = base.IsDirect;
					if (isDirect)
					{
						base.AppendAffectedData(context, base.CharacterId, (ushort)(56 + this.BuffHitType), EDataModifyType.TotalPercent, legSkillId);
					}
					else
					{
						base.AppendAffectedData(context, base.CharacterId, 224, EDataModifyType.Custom, legSkillId);
					}
				}
				base.ShowSpecialEffectTips(0);
				Events.RegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
			}
		}

		// Token: 0x0600436B RID: 17259 RVA: 0x0026B4CC File Offset: 0x002696CC
		private void OnCastSkillEnd(DataContext context, int charId, bool isAlly, short skillId, sbyte power, bool interrupted)
		{
			bool flag = charId != base.CharacterId || skillId != this._affectingLegSkill;
			if (!flag)
			{
				Events.UnRegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
				bool agileSkillChanged = this.AgileSkillChanged;
				if (agileSkillChanged)
				{
					base.RemoveSelf(context);
				}
				else
				{
					this.AutoRemove = true;
				}
			}
		}

		// Token: 0x0600436C RID: 17260 RVA: 0x0026B528 File Offset: 0x00269728
		public override int GetModifyValue(AffectedDataKey dataKey, int currModifyValue)
		{
			bool flag = dataKey.CharId != base.CharacterId || dataKey.CombatSkillId != this._affectingLegSkill;
			int result;
			if (flag)
			{
				result = 0;
			}
			else
			{
				bool flag2 = dataKey.FieldId == (ushort)(56 + this.BuffHitType);
				if (flag2)
				{
					result = 60;
				}
				else
				{
					result = 0;
				}
			}
			return result;
		}

		// Token: 0x0600436D RID: 17261 RVA: 0x0026B580 File Offset: 0x00269780
		public unsafe override HitOrAvoidInts GetModifiedValue(AffectedDataKey dataKey, HitOrAvoidInts dataValue)
		{
			bool flag = dataKey.CharId != base.CharacterId || dataKey.CombatSkillId != this._affectingLegSkill;
			HitOrAvoidInts result;
			if (flag)
			{
				result = dataValue;
			}
			else
			{
				bool flag2 = dataKey.FieldId == 224;
				if (flag2)
				{
					sbyte transferValue = 0;
					for (sbyte hitType = 0; hitType < 3; hitType += 1)
					{
						bool flag3 = hitType == this.BuffHitType;
						if (!flag3)
						{
							sbyte changedValue = (sbyte)Math.Max(*(ref dataValue.Items.FixedElementField + (IntPtr)hitType * 4) / 20 * 10, 10);
							transferValue = (sbyte)((int)transferValue + *(ref dataValue.Items.FixedElementField + (IntPtr)hitType * 4) - (int)changedValue);
							*(ref dataValue.Items.FixedElementField + (IntPtr)hitType * 4) = (int)changedValue;
						}
					}
					*(ref dataValue.Items.FixedElementField + (IntPtr)this.BuffHitType * 4) += (int)transferValue;
				}
				result = dataValue;
			}
			return result;
		}

		// Token: 0x04001401 RID: 5121
		private const sbyte DirectAddPercent = 60;

		// Token: 0x04001402 RID: 5122
		protected sbyte BuffHitType;

		// Token: 0x04001403 RID: 5123
		private short _affectingLegSkill;
	}
}
