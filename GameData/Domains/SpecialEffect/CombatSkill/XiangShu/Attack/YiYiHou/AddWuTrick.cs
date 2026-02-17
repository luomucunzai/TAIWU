using System;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;

namespace GameData.Domains.SpecialEffect.CombatSkill.XiangShu.Attack.YiYiHou
{
	// Token: 0x020002BE RID: 702
	public class AddWuTrick : CombatSkillEffectBase
	{
		// Token: 0x0600325A RID: 12890 RVA: 0x0021F3C1 File Offset: 0x0021D5C1
		protected AddWuTrick()
		{
		}

		// Token: 0x0600325B RID: 12891 RVA: 0x0021F3CB File Offset: 0x0021D5CB
		protected AddWuTrick(CombatSkillKey skillKey, int type) : base(skillKey, type, -1)
		{
		}

		// Token: 0x0600325C RID: 12892 RVA: 0x0021F3D8 File Offset: 0x0021D5D8
		public override void OnEnable(DataContext context)
		{
			DomainManager.Combat.AddMoveDistInSkillPrepare(base.CombatChar, 1000, true);
			DomainManager.Combat.AddMoveDistInSkillPrepare(base.CombatChar, 1000, false);
			CombatCharacter enemyChar = DomainManager.Combat.GetCombatCharacter(!base.CombatChar.IsAlly, false);
			bool flag = DomainManager.Character.HasTwoWayRelation(enemyChar.GetId(), 16384) || DomainManager.Character.HasTwoWayRelation(enemyChar.GetId(), 32768);
			if (flag)
			{
				DomainManager.Combat.AddTrick(context, enemyChar, 20, (int)this.AddTrickCount, false, false);
				base.ShowSpecialEffectTips(0);
			}
			Events.RegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
		}

		// Token: 0x0600325D RID: 12893 RVA: 0x0021F494 File Offset: 0x0021D694
		public override void OnDisable(DataContext context)
		{
			Events.UnRegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
		}

		// Token: 0x0600325E RID: 12894 RVA: 0x0021F4AC File Offset: 0x0021D6AC
		private void OnCastSkillEnd(DataContext context, int charId, bool isAlly, short skillId, sbyte power, bool interrupted)
		{
			bool flag = charId != base.CharacterId || skillId != base.SkillTemplateId;
			if (!flag)
			{
				bool flag2 = base.PowerMatchAffectRequire((int)power, 0);
				if (flag2)
				{
					CombatCharacter enemyChar = DomainManager.Combat.GetCombatCharacter(!base.CombatChar.IsAlly, false);
					DomainManager.Combat.AddTrick(context, enemyChar, 20, (int)this.AddTrickCount, false, false);
					base.ShowSpecialEffectTips(0);
				}
				base.RemoveSelf(context);
			}
		}

		// Token: 0x04000EEB RID: 3819
		protected sbyte AddTrickCount;
	}
}
