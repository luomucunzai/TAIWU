using System;
using GameData.Combat.Math;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Attack;

namespace GameData.Domains.SpecialEffect.CombatSkill.Emeipai.Finger
{
	// Token: 0x02000557 RID: 1367
	public class DaGuangMingShanYiYuanZhi : PowerUpOnCast
	{
		// Token: 0x17000264 RID: 612
		// (get) Token: 0x0600406F RID: 16495 RVA: 0x0025E231 File Offset: 0x0025C431
		protected override EDataModifyType ModifyType
		{
			get
			{
				return EDataModifyType.AddPercent;
			}
		}

		// Token: 0x06004070 RID: 16496 RVA: 0x0025E234 File Offset: 0x0025C434
		public DaGuangMingShanYiYuanZhi()
		{
		}

		// Token: 0x06004071 RID: 16497 RVA: 0x0025E23E File Offset: 0x0025C43E
		public DaGuangMingShanYiYuanZhi(CombatSkillKey skillKey) : base(skillKey, 2206)
		{
		}

		// Token: 0x06004072 RID: 16498 RVA: 0x0025E250 File Offset: 0x0025C450
		public override void OnEnable(DataContext context)
		{
			this.PowerUpValue = (base.IsDirect ? ((int)this.CharObj.GetFame() * DaGuangMingShanYiYuanZhi.AddPower) : ((int)(-(int)this.CharObj.GetFame()) * DaGuangMingShanYiYuanZhi.AddPower));
			base.OnEnable(context);
			Events.RegisterHandler_PrepareSkillEnd(new Events.OnPrepareSkillEnd(this.OnPrepareSkillEnd));
		}

		// Token: 0x06004073 RID: 16499 RVA: 0x0025E2B3 File Offset: 0x0025C4B3
		public override void OnDisable(DataContext context)
		{
			Events.UnRegisterHandler_PrepareSkillEnd(new Events.OnPrepareSkillEnd(this.OnPrepareSkillEnd));
			base.OnDisable(context);
		}

		// Token: 0x06004074 RID: 16500 RVA: 0x0025E2D0 File Offset: 0x0025C4D0
		private void OnPrepareSkillEnd(DataContext context, int charId, bool isAlly, short skillId)
		{
			bool flag = charId != base.CharacterId || skillId != base.SkillTemplateId;
			if (!flag)
			{
				sbyte selfFame = this.CharObj.GetFame();
				sbyte enemyFame = base.CurrEnemyChar.GetCharacter().GetFame();
				bool flag2 = base.IsDirect ? (selfFame >= enemyFame) : (selfFame <= enemyFame);
				if (flag2)
				{
					base.AppendAffectedData(context, base.CharacterId, 251, EDataModifyType.Custom, base.SkillTemplateId);
				}
			}
		}

		// Token: 0x06004075 RID: 16501 RVA: 0x0025E350 File Offset: 0x0025C550
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
				bool flag2 = dataKey.FieldId == 251;
				result = (flag2 || dataValue);
			}
			return result;
		}

		// Token: 0x040012F0 RID: 4848
		private static readonly CValuePercent AddPower = 60;
	}
}
