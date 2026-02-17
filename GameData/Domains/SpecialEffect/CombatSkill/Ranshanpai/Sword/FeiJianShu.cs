using System;
using System.Collections.Generic;
using System.Linq;
using GameData.Common;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Attack;
using GameData.Utilities;

namespace GameData.Domains.SpecialEffect.CombatSkill.Ranshanpai.Sword
{
	// Token: 0x02000440 RID: 1088
	public class FeiJianShu : AttackBodyPart
	{
		// Token: 0x06003A0F RID: 14863 RVA: 0x00241D95 File Offset: 0x0023FF95
		public FeiJianShu()
		{
		}

		// Token: 0x06003A10 RID: 14864 RVA: 0x00241D9F File Offset: 0x0023FF9F
		public FeiJianShu(CombatSkillKey skillKey) : base(skillKey, 7202)
		{
			this.BodyParts = new sbyte[]
			{
				2
			};
			this.ReverseAddDamagePercent = 30;
		}

		// Token: 0x06003A11 RID: 14865 RVA: 0x00241DC8 File Offset: 0x0023FFC8
		protected override void OnCastAffectPower(DataContext context)
		{
			CombatCharacter enemyChar = DomainManager.Combat.GetCombatCharacter(!base.CombatChar.IsAlly, false);
			int enemyCharId = enemyChar.GetId();
			List<short> skillList = enemyChar.GetAttackSkillList();
			SkillEffectKey effectKey = new SkillEffectKey(base.SkillTemplateId, base.IsDirect);
			List<short> pool = ObjectPool<List<short>>.Instance.Get();
			pool.Clear();
			pool.AddRange(from x in skillList
			where x >= 0 && DomainManager.Combat.GetReduceSkillPowerInCombat(new CombatSkillKey(enemyCharId, x), effectKey) == 0
			select x);
			short skillId = (pool.Count > 0) ? pool.GetRandom(context.Random) : -1;
			ObjectPool<List<short>>.Instance.Return(pool);
			bool flag = skillId < 0;
			if (!flag)
			{
				DomainManager.Combat.ReduceSkillPowerInCombat(context, new CombatSkillKey(enemyCharId, skillId), effectKey, -20);
				base.ShowSpecialEffectTips(1);
			}
		}

		// Token: 0x040010FF RID: 4351
		private const int ReducePowerValue = -20;
	}
}
