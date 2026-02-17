using System;
using System.Collections.Generic;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Agile;
using GameData.Utilities;

namespace GameData.Domains.SpecialEffect.CombatSkill.XiangShu.Agile
{
	// Token: 0x0200033E RID: 830
	public class XieNiWuSheng : AgileSkillBase
	{
		// Token: 0x060034C4 RID: 13508 RVA: 0x00229CD7 File Offset: 0x00227ED7
		public XieNiWuSheng()
		{
		}

		// Token: 0x060034C5 RID: 13509 RVA: 0x00229CE1 File Offset: 0x00227EE1
		public XieNiWuSheng(CombatSkillKey skillKey) : base(skillKey, 16201)
		{
			this.ListenCanAffectChange = true;
		}

		// Token: 0x060034C6 RID: 13510 RVA: 0x00229CF8 File Offset: 0x00227EF8
		public override void OnEnable(DataContext context)
		{
			base.OnEnable(context);
			Events.RegisterHandler_DistanceChanged(new Events.OnDistanceChanged(this.OnDistanceChanged));
			this._affecting = false;
			this.OnMoveSkillCanAffectChanged(context, default(DataUid));
		}

		// Token: 0x060034C7 RID: 13511 RVA: 0x00229D38 File Offset: 0x00227F38
		public override void OnDisable(DataContext context)
		{
			base.OnDisable(context);
			Events.UnRegisterHandler_DistanceChanged(new Events.OnDistanceChanged(this.OnDistanceChanged));
			DomainManager.Combat.DisableJumpMove(context, base.CombatChar, base.SkillTemplateId);
		}

		// Token: 0x060034C8 RID: 13512 RVA: 0x00229D70 File Offset: 0x00227F70
		private void OnDistanceChanged(DataContext context, CombatCharacter mover, short distance, bool isMove, bool isForced)
		{
			bool flag = mover != base.CombatChar || !isMove || isForced || !this._affecting;
			if (!flag)
			{
				int enemyCharId = DomainManager.Combat.GetCombatCharacter(!base.CombatChar.IsAlly, false).GetId();
				Dictionary<CombatSkillKey, SkillPowerChangeCollection> powerDict = DomainManager.Combat.GetAllSkillPowerAddInCombat();
				List<CombatSkillKey> skillRandomPool = ObjectPool<List<CombatSkillKey>>.Instance.Get();
				skillRandomPool.Clear();
				foreach (CombatSkillKey skillKey in powerDict.Keys)
				{
					bool flag2 = skillKey.CharId == enemyCharId;
					if (flag2)
					{
						skillRandomPool.Add(skillKey);
					}
				}
				bool flag3 = skillRandomPool.Count > 0;
				if (flag3)
				{
					int removeCount = Math.Min(3, skillRandomPool.Count);
					for (int i = 0; i < removeCount; i++)
					{
						int index = context.Random.Next(0, skillRandomPool.Count);
						DomainManager.Combat.RemoveSkillPowerAddInCombat(context, skillRandomPool[index]);
						skillRandomPool.RemoveAt(index);
					}
					DomainManager.SpecialEffect.InvalidateCache(context, enemyCharId, 199);
					base.ShowSpecialEffectTips(0);
				}
				ObjectPool<List<CombatSkillKey>>.Instance.Return(skillRandomPool);
			}
		}

		// Token: 0x060034C9 RID: 13513 RVA: 0x00229ED4 File Offset: 0x002280D4
		protected override void OnMoveSkillCanAffectChanged(DataContext context, DataUid dataUid)
		{
			bool canAffect = base.CanAffect;
			bool flag = this._affecting == canAffect;
			if (!flag)
			{
				this._affecting = canAffect;
				bool flag2 = canAffect;
				if (flag2)
				{
					DomainManager.Combat.EnableJumpMove(base.CombatChar, base.SkillTemplateId);
				}
				else
				{
					DomainManager.Combat.DisableJumpMove(context, base.CombatChar, base.SkillTemplateId);
				}
			}
		}

		// Token: 0x04000F88 RID: 3976
		private const sbyte AffectSkillCount = 3;

		// Token: 0x04000F89 RID: 3977
		private bool _affecting;
	}
}
