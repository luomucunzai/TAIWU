using System;
using System.Collections.Generic;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;
using GameData.Utilities;

namespace GameData.Domains.SpecialEffect.CombatSkill.XiangShu.Attack.ShuFang
{
	// Token: 0x020002E8 RID: 744
	public class TianYanShu : CombatSkillEffectBase
	{
		// Token: 0x06003336 RID: 13110 RVA: 0x002236A4 File Offset: 0x002218A4
		public TianYanShu()
		{
		}

		// Token: 0x06003337 RID: 13111 RVA: 0x002236AE File Offset: 0x002218AE
		public TianYanShu(CombatSkillKey skillKey) : base(skillKey, 17081, -1)
		{
		}

		// Token: 0x06003338 RID: 13112 RVA: 0x002236C0 File Offset: 0x002218C0
		public override void OnEnable(DataContext context)
		{
			SkillEffectKey effectKey = new SkillEffectKey(base.SkillTemplateId, base.IsDirect);
			List<short> skillRandomPool = ObjectPool<List<short>>.Instance.Get();
			skillRandomPool.Clear();
			for (sbyte equipType = 1; equipType <= 4; equipType += 1)
			{
				skillRandomPool.AddRange(base.CombatChar.GetCombatSkillList(equipType));
			}
			skillRandomPool.RemoveAll((short id) => id < 0);
			int affectCount = Math.Min(2, skillRandomPool.Count);
			for (int i = 0; i < affectCount; i++)
			{
				int index = context.Random.Next(skillRandomPool.Count);
				DomainManager.Combat.AddSkillPowerInCombat(context, new CombatSkillKey(base.CombatChar.GetId(), skillRandomPool[index]), effectKey, 30);
				skillRandomPool.RemoveAt(index);
			}
			ObjectPool<List<short>>.Instance.Return(skillRandomPool);
			base.ShowSpecialEffectTips(0);
			Events.RegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
		}

		// Token: 0x06003339 RID: 13113 RVA: 0x002237D0 File Offset: 0x002219D0
		public override void OnDisable(DataContext context)
		{
			Events.UnRegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
		}

		// Token: 0x0600333A RID: 13114 RVA: 0x002237E8 File Offset: 0x002219E8
		private void OnCastSkillEnd(DataContext context, int charId, bool isAlly, short skillId, sbyte power, bool interrupted)
		{
			bool flag = charId != base.CharacterId || skillId != base.SkillTemplateId;
			if (!flag)
			{
				base.RemoveSelf(context);
			}
		}

		// Token: 0x04000F20 RID: 3872
		private const sbyte AffectSkillCount = 2;

		// Token: 0x04000F21 RID: 3873
		private const sbyte AddPower = 30;
	}
}
