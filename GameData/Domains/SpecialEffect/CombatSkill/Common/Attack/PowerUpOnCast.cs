using System;
using GameData.Combat.Math;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.CombatSkill;

namespace GameData.Domains.SpecialEffect.CombatSkill.Common.Attack
{
	// Token: 0x020005A1 RID: 1441
	public abstract class PowerUpOnCast : CombatSkillEffectBase
	{
		// Token: 0x170002A0 RID: 672
		// (get) Token: 0x060042CE RID: 17102 RVA: 0x002686D0 File Offset: 0x002668D0
		protected virtual EDataModifyType ModifyType
		{
			get
			{
				return EDataModifyType.Add;
			}
		}

		// Token: 0x060042CF RID: 17103 RVA: 0x002686D3 File Offset: 0x002668D3
		protected PowerUpOnCast()
		{
		}

		// Token: 0x060042D0 RID: 17104 RVA: 0x002686DD File Offset: 0x002668DD
		protected PowerUpOnCast(CombatSkillKey skillKey, int type) : base(skillKey, type, -1)
		{
		}

		// Token: 0x060042D1 RID: 17105 RVA: 0x002686EC File Offset: 0x002668EC
		public override void OnEnable(DataContext context)
		{
			bool flag = this.PowerUpValue > 0;
			if (flag)
			{
				base.CreateAffectedData(199, this.ModifyType, base.SkillTemplateId);
				base.ShowSpecialEffectTips(0);
			}
			Events.RegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
		}

		// Token: 0x060042D2 RID: 17106 RVA: 0x0026873B File Offset: 0x0026693B
		public override void OnDisable(DataContext context)
		{
			Events.UnRegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
		}

		// Token: 0x060042D3 RID: 17107 RVA: 0x00268750 File Offset: 0x00266950
		private void OnCastSkillEnd(DataContext context, int charId, bool isAlly, short skillId, sbyte power, bool interrupted)
		{
			bool flag = charId != base.CharacterId || skillId != base.SkillTemplateId;
			if (!flag)
			{
				this.OnCastSelf(context, power, interrupted);
				base.RemoveSelf(context);
			}
		}

		// Token: 0x060042D4 RID: 17108 RVA: 0x00268794 File Offset: 0x00266994
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
					result = this.PowerUpValue;
				}
				else
				{
					result = 0;
				}
			}
			return result;
		}

		// Token: 0x060042D5 RID: 17109 RVA: 0x002687EB File Offset: 0x002669EB
		protected virtual void OnCastSelf(DataContext context, sbyte power, bool interrupted)
		{
		}

		// Token: 0x040013C4 RID: 5060
		protected int PowerUpValue;
	}
}
