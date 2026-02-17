using System;
using GameData.Combat.Math;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill;

namespace GameData.Domains.SpecialEffect.LegendaryBook.Common
{
	// Token: 0x0200016F RID: 367
	public abstract class AddDamage : CombatSkillEffectBase
	{
		// Token: 0x06002B37 RID: 11063 RVA: 0x00204C2D File Offset: 0x00202E2D
		protected AddDamage()
		{
			this.IsLegendaryBookEffect = true;
		}

		// Token: 0x06002B38 RID: 11064 RVA: 0x00204C3E File Offset: 0x00202E3E
		protected AddDamage(CombatSkillKey skillKey, int type) : base(skillKey, type, -1)
		{
			this.IsLegendaryBookEffect = true;
		}

		// Token: 0x06002B39 RID: 11065 RVA: 0x00204C52 File Offset: 0x00202E52
		public override void OnEnable(DataContext context)
		{
			base.CreateAffectedData(275, EDataModifyType.AddPercent, base.SkillTemplateId);
			base.CreateAffectedData(69, EDataModifyType.AddPercent, base.SkillTemplateId);
			Events.RegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
		}

		// Token: 0x06002B3A RID: 11066 RVA: 0x00204C8A File Offset: 0x00202E8A
		public override void OnDisable(DataContext context)
		{
			Events.UnRegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
		}

		// Token: 0x06002B3B RID: 11067 RVA: 0x00204CA0 File Offset: 0x00202EA0
		private void OnCastSkillEnd(DataContext context, int charId, bool isAlly, short skillId, sbyte power, bool interrupted)
		{
			bool flag = !this.SkillKey.IsMatch(charId, skillId) || base.CombatChar.GetAutoCastingSkill();
			if (!flag)
			{
				DomainManager.Combat.SilenceSkill(context, base.CombatChar, base.SkillTemplateId, -1, -1);
			}
		}

		// Token: 0x06002B3C RID: 11068
		protected abstract int GetAddDamagePercent();

		// Token: 0x06002B3D RID: 11069 RVA: 0x00204CEC File Offset: 0x00202EEC
		public override int GetModifyValue(AffectedDataKey dataKey, int currModifyValue)
		{
			bool flag = dataKey.SkillKey != this.SkillKey || base.CombatChar.GetAutoCastingSkill();
			int result;
			if (flag)
			{
				result = 0;
			}
			else
			{
				ushort fieldId = dataKey.FieldId;
				bool flag2 = fieldId == 69 || fieldId == 275;
				bool flag3 = flag2;
				if (flag3)
				{
					result = Math.Min(this.GetAddDamagePercent(), 180);
				}
				else
				{
					result = 0;
				}
			}
			return result;
		}

		// Token: 0x04000D38 RID: 3384
		protected const short MaxAddDamage = 180;
	}
}
