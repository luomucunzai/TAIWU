using System;
using System.Collections.Generic;
using GameData.Combat.Math;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;

namespace GameData.Domains.SpecialEffect.CombatSkill.Emeipai.Finger
{
	// Token: 0x0200055C RID: 1372
	public class TianGangZhiXueFa : CombatSkillEffectBase
	{
		// Token: 0x0600408E RID: 16526 RVA: 0x0025EBE2 File Offset: 0x0025CDE2
		public TianGangZhiXueFa()
		{
		}

		// Token: 0x0600408F RID: 16527 RVA: 0x0025EBEC File Offset: 0x0025CDEC
		public TianGangZhiXueFa(CombatSkillKey skillKey) : base(skillKey, 2207, -1)
		{
		}

		// Token: 0x06004090 RID: 16528 RVA: 0x0025EC00 File Offset: 0x0025CE00
		public override void OnEnable(DataContext context)
		{
			this.AffectDatas = new Dictionary<AffectedDataKey, EDataModifyType>();
			this.AffectDatas.Add(new AffectedDataKey(base.CharacterId, base.IsDirect ? 133 : 128, base.SkillTemplateId, -1, -1, -1), EDataModifyType.Custom);
			Events.RegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
			Events.RegisterHandler_SkillEffectChange(new Events.OnSkillEffectChange(this.OnSkillEffectChange));
			bool isDirect = base.IsDirect;
			if (isDirect)
			{
				Events.RegisterHandler_AcuPointRemoved(new Events.OnAcuPointRemoved(this.OnFlawOrAcuPointRemoved));
			}
			else
			{
				Events.RegisterHandler_FlawRemoved(new Events.OnFlawRemoved(this.OnFlawOrAcuPointRemoved));
			}
		}

		// Token: 0x06004091 RID: 16529 RVA: 0x0025ECA4 File Offset: 0x0025CEA4
		public override void OnDisable(DataContext context)
		{
			Events.UnRegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
			Events.UnRegisterHandler_SkillEffectChange(new Events.OnSkillEffectChange(this.OnSkillEffectChange));
			bool isDirect = base.IsDirect;
			if (isDirect)
			{
				Events.UnRegisterHandler_AcuPointRemoved(new Events.OnAcuPointRemoved(this.OnFlawOrAcuPointRemoved));
			}
			else
			{
				Events.UnRegisterHandler_FlawRemoved(new Events.OnFlawRemoved(this.OnFlawOrAcuPointRemoved));
			}
		}

		// Token: 0x06004092 RID: 16530 RVA: 0x0025ED08 File Offset: 0x0025CF08
		private void OnCastSkillEnd(DataContext context, int charId, bool isAlly, short skillId, sbyte power, bool interrupted)
		{
			bool flag = charId != base.CharacterId || skillId != base.SkillTemplateId;
			if (!flag)
			{
				bool flag2 = !this.IsSrcSkillPerformed;
				if (flag2)
				{
					bool flag3 = base.PowerMatchAffectRequire((int)power, 0);
					if (flag3)
					{
						this.IsSrcSkillPerformed = true;
						base.AddMaxEffectCount(true);
					}
					else
					{
						base.RemoveSelf(context);
					}
				}
				else
				{
					bool flag4 = base.PowerMatchAffectRequire((int)power, 0);
					if (flag4)
					{
						base.RemoveSelf(context);
					}
				}
			}
		}

		// Token: 0x06004093 RID: 16531 RVA: 0x0025ED88 File Offset: 0x0025CF88
		private void OnSkillEffectChange(DataContext context, int charId, SkillEffectKey key, short oldCount, short newCount, bool removed)
		{
			bool flag = removed && this.IsSrcSkillPerformed && charId == base.CharacterId && key.SkillId == base.SkillTemplateId && key.IsDirect == base.IsDirect;
			if (flag)
			{
				base.RemoveSelf(context);
			}
		}

		// Token: 0x06004094 RID: 16532 RVA: 0x0025EDD8 File Offset: 0x0025CFD8
		private void OnFlawOrAcuPointRemoved(DataContext context, CombatCharacter combatChar, sbyte bodyPart, sbyte level)
		{
			bool flag = !this.IsSrcSkillPerformed || combatChar.IsAlly == base.CombatChar.IsAlly || !DomainManager.Combat.IsCurrentCombatCharacter(combatChar) || (int)level >= GlobalConfig.Instance.AcupointBaseKeepTime.Length - 1;
			if (!flag)
			{
				bool isDirect = base.IsDirect;
				if (isDirect)
				{
					DomainManager.Combat.AddAcupoint(context, combatChar, level + 1, this.SkillKey, bodyPart, 1, false);
				}
				else
				{
					DomainManager.Combat.AddFlaw(context, combatChar, level + 1, this.SkillKey, bodyPart, 1, false);
				}
				base.ReduceEffectCount(1);
				base.ShowSpecialEffectTips(0);
			}
		}

		// Token: 0x06004095 RID: 16533 RVA: 0x0025EE80 File Offset: 0x0025D080
		public override bool GetModifiedValue(AffectedDataKey dataKey, bool dataValue)
		{
			bool flag = dataKey.CharId != base.CharacterId || dataKey.CombatSkillId != base.SkillTemplateId;
			bool result;
			if (flag)
			{
				result = dataValue;
			}
			else
			{
				bool flag2 = dataKey.FieldId == 133 || dataKey.FieldId == 128;
				result = (!flag2 && dataValue);
			}
			return result;
		}
	}
}
