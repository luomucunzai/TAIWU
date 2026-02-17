using System;
using System.Collections.Generic;
using System.Linq;
using GameData.Common;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Attack;
using GameData.Utilities;

namespace GameData.Domains.SpecialEffect.CombatSkill.Jieqingmen.Sword
{
	// Token: 0x020004E8 RID: 1256
	public class XianChiJianQi : AttackBodyPart
	{
		// Token: 0x06003E10 RID: 15888 RVA: 0x002549ED File Offset: 0x00252BED
		public XianChiJianQi()
		{
		}

		// Token: 0x06003E11 RID: 15889 RVA: 0x002549F7 File Offset: 0x00252BF7
		public XianChiJianQi(CombatSkillKey skillKey) : base(skillKey, 13202)
		{
			this.BodyParts = new sbyte[]
			{
				2
			};
			this.ReverseAddDamagePercent = 30;
		}

		// Token: 0x06003E12 RID: 15890 RVA: 0x00254A20 File Offset: 0x00252C20
		protected override void OnCastAffectPower(DataContext context)
		{
			CombatCharacter enemyChar = DomainManager.Combat.GetCombatCharacter(!base.CombatChar.IsAlly, false);
			int enemyCharId = enemyChar.GetId();
			List<short> skillList = enemyChar.GetDefenceSkillList();
			SkillEffectKey effectKey = new SkillEffectKey(base.SkillTemplateId, base.IsDirect);
			List<short> pool = ObjectPool<List<short>>.Instance.Get();
			pool.Clear();
			pool.AddRange(from x in skillList
			where x >= 0 && DomainManager.Combat.GetReduceSkillPowerInCombat(new CombatSkillKey(enemyCharId, x), effectKey) == 0
			select x);
			short skillId = (pool.Count > 0) ? pool.GetRandom(context.Random) : -1;
			ObjectPool<List<short>>.Instance.Return(pool);
			bool flag = skillId >= 0;
			if (flag)
			{
				DomainManager.Combat.ReduceSkillPowerInCombat(context, new CombatSkillKey(enemyCharId, skillId), effectKey, -20);
				base.ShowSpecialEffectTips(1);
			}
		}

		// Token: 0x04001256 RID: 4694
		private const int ReducePowerValue = -20;
	}
}
