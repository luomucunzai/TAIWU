using System;
using GameData.Combat.Math;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Attack;

namespace GameData.Domains.SpecialEffect.CombatSkill.Jieqingmen.Finger
{
	// Token: 0x020004F2 RID: 1266
	public class CuoShenZhi : BuffByNeiliAllocation
	{
		// Token: 0x06003E30 RID: 15920 RVA: 0x00254DE3 File Offset: 0x00252FE3
		public CuoShenZhi()
		{
		}

		// Token: 0x06003E31 RID: 15921 RVA: 0x00254DED File Offset: 0x00252FED
		public CuoShenZhi(CombatSkillKey skillKey) : base(skillKey, 13107)
		{
			this.RequireNeiliAllocationType = 1;
		}

		// Token: 0x06003E32 RID: 15922 RVA: 0x00254E04 File Offset: 0x00253004
		public override void OnEnable(DataContext context)
		{
			base.OnEnable(context);
			Events.RegisterHandler_PrepareSkillBegin(new Events.OnPrepareSkillBegin(this.OnPrepareSkillBegin));
		}

		// Token: 0x06003E33 RID: 15923 RVA: 0x00254E21 File Offset: 0x00253021
		public override void OnDisable(DataContext context)
		{
			Events.UnRegisterHandler_PrepareSkillBegin(new Events.OnPrepareSkillBegin(this.OnPrepareSkillBegin));
			base.OnDisable(context);
		}

		// Token: 0x06003E34 RID: 15924 RVA: 0x00254E40 File Offset: 0x00253040
		private void OnPrepareSkillBegin(DataContext context, int charId, bool isAlly, short skillId)
		{
			bool flag = this.SkillKey.IsMatch(charId, skillId) && base.Affecting;
			if (flag)
			{
				DomainManager.Combat.ChangeSkillPrepareProgress(base.CombatChar, base.CombatChar.SkillPrepareTotalProgress * CuoShenZhi.ProgressPercent);
			}
		}

		// Token: 0x04001259 RID: 4697
		private static readonly CValuePercent ProgressPercent = 75;
	}
}
