using System;
using System.Collections.Generic;
using System.Linq;
using GameData.Common;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Attack;
using GameData.Utilities;

namespace GameData.Domains.SpecialEffect.CombatSkill.Emeipai.Sword
{
	// Token: 0x02000538 RID: 1336
	public class BaiMeiJianFa : AttackBodyPart
	{
		// Token: 0x06003FC7 RID: 16327 RVA: 0x0025B741 File Offset: 0x00259941
		public BaiMeiJianFa()
		{
		}

		// Token: 0x06003FC8 RID: 16328 RVA: 0x0025B74B File Offset: 0x0025994B
		public BaiMeiJianFa(CombatSkillKey skillKey) : base(skillKey, 2302)
		{
			this.BodyParts = new sbyte[]
			{
				2
			};
			this.ReverseAddDamagePercent = 30;
		}

		// Token: 0x06003FC9 RID: 16329 RVA: 0x0025B774 File Offset: 0x00259974
		protected override void OnCastAffectPower(DataContext context)
		{
			CombatCharacter enemyChar = DomainManager.Combat.GetCombatCharacter(!base.CombatChar.IsAlly, false);
			int enemyCharId = enemyChar.GetId();
			List<short> enemyCharAgileSkillList = enemyChar.GetAgileSkillList();
			SkillEffectKey effectKey = new SkillEffectKey(base.SkillTemplateId, base.IsDirect);
			List<short> agileSkillRandomPool = ObjectPool<List<short>>.Instance.Get();
			agileSkillRandomPool.Clear();
			agileSkillRandomPool.AddRange(from x in enemyCharAgileSkillList
			where x >= 0 && DomainManager.Combat.GetReduceSkillPowerInCombat(new CombatSkillKey(enemyCharId, x), effectKey) == 0
			select x);
			short agileSkillId = (agileSkillRandomPool.Count > 0) ? agileSkillRandomPool.GetRandom(context.Random) : -1;
			ObjectPool<List<short>>.Instance.Return(agileSkillRandomPool);
			bool flag = agileSkillId >= 0;
			if (flag)
			{
				DomainManager.Combat.ReduceSkillPowerInCombat(context, new CombatSkillKey(enemyCharId, agileSkillId), effectKey, -20);
				base.ShowSpecialEffectTips(1);
			}
		}

		// Token: 0x040012CA RID: 4810
		private const int ReducePowerValue = -20;
	}
}
