using System;
using System.Collections.Generic;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;
using GameData.Utilities;

namespace GameData.Domains.SpecialEffect.CombatSkill.XiangShu.Attack.ShuFang
{
	// Token: 0x020002E5 RID: 741
	public class MoTongShu : CombatSkillEffectBase
	{
		// Token: 0x06003329 RID: 13097 RVA: 0x00223345 File Offset: 0x00221545
		public MoTongShu()
		{
		}

		// Token: 0x0600332A RID: 13098 RVA: 0x0022334F File Offset: 0x0022154F
		public MoTongShu(CombatSkillKey skillKey) : base(skillKey, 17084, -1)
		{
		}

		// Token: 0x0600332B RID: 13099 RVA: 0x00223360 File Offset: 0x00221560
		public override void OnEnable(DataContext context)
		{
			CombatCharacter enemyChar = DomainManager.Combat.GetCombatCharacter(!base.CombatChar.IsAlly, false);
			SkillEffectKey effectKey = new SkillEffectKey(base.SkillTemplateId, base.IsDirect);
			List<short> skillRandomPool = ObjectPool<List<short>>.Instance.Get();
			skillRandomPool.Clear();
			for (sbyte equipType = 1; equipType <= 4; equipType += 1)
			{
				skillRandomPool.AddRange(enemyChar.GetCombatSkillList(equipType));
			}
			skillRandomPool.RemoveAll((short id) => id < 0);
			int affectCount = Math.Min(4, skillRandomPool.Count);
			for (int i = 0; i < affectCount; i++)
			{
				int index = context.Random.Next(skillRandomPool.Count);
				DomainManager.Combat.ReduceSkillPowerInCombat(context, new CombatSkillKey(enemyChar.GetId(), skillRandomPool[index]), effectKey, -30);
				skillRandomPool.RemoveAt(index);
			}
			ObjectPool<List<short>>.Instance.Return(skillRandomPool);
			base.ShowSpecialEffectTips(0);
			Events.RegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
		}

		// Token: 0x0600332C RID: 13100 RVA: 0x00223485 File Offset: 0x00221685
		public override void OnDisable(DataContext context)
		{
			Events.UnRegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
		}

		// Token: 0x0600332D RID: 13101 RVA: 0x0022349C File Offset: 0x0022169C
		private void OnCastSkillEnd(DataContext context, int charId, bool isAlly, short skillId, sbyte power, bool interrupted)
		{
			bool flag = charId != base.CharacterId || skillId != base.SkillTemplateId;
			if (!flag)
			{
				base.RemoveSelf(context);
			}
		}

		// Token: 0x04000F1C RID: 3868
		private const sbyte AffectSkillCount = 4;

		// Token: 0x04000F1D RID: 3869
		private const sbyte ReducePower = -30;
	}
}
