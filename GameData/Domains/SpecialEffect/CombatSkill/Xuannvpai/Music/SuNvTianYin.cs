using System;
using System.Collections.Generic;
using Config;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;
using GameData.Utilities;

namespace GameData.Domains.SpecialEffect.CombatSkill.Xuannvpai.Music
{
	// Token: 0x0200026F RID: 623
	public class SuNvTianYin : CombatSkillEffectBase
	{
		// Token: 0x0600308C RID: 12428 RVA: 0x00217952 File Offset: 0x00215B52
		public SuNvTianYin()
		{
		}

		// Token: 0x0600308D RID: 12429 RVA: 0x0021795C File Offset: 0x00215B5C
		public SuNvTianYin(CombatSkillKey skillKey) : base(skillKey, 8308, -1)
		{
		}

		// Token: 0x0600308E RID: 12430 RVA: 0x0021796D File Offset: 0x00215B6D
		public override void OnEnable(DataContext context)
		{
			Events.RegisterHandler_CastAttackSkillBegin(new Events.OnCastAttackSkillBegin(this.OnCastAttackSkillBegin));
			Events.RegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
		}

		// Token: 0x0600308F RID: 12431 RVA: 0x00217994 File Offset: 0x00215B94
		public override void OnDisable(DataContext context)
		{
			Events.UnRegisterHandler_CastAttackSkillBegin(new Events.OnCastAttackSkillBegin(this.OnCastAttackSkillBegin));
			Events.UnRegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
		}

		// Token: 0x06003090 RID: 12432 RVA: 0x002179BC File Offset: 0x00215BBC
		private void OnCastAttackSkillBegin(DataContext context, CombatCharacter attacker, CombatCharacter defender, short skillId)
		{
			bool flag = attacker.GetId() != base.CharacterId || skillId != base.SkillTemplateId;
			if (!flag)
			{
				bool flag2 = base.IsDirect ? (defender.GetTrickCount(20) >= 3) : (defender.GetDefeatMarkCollection().MindMarkList.Count >= 6);
				if (flag2)
				{
					base.ClearAffectingAgileSkill(context, defender);
					DomainManager.Combat.ClearAffectingDefenseSkill(context, defender);
					base.ShowSpecialEffectTips(0);
				}
				bool flag3 = !base.IsDirect;
				if (flag3)
				{
					this._affectMindMarkCount = base.CurrEnemyChar.GetDefeatMarkCollection().MindMarkList.Count;
				}
			}
		}

		// Token: 0x06003091 RID: 12433 RVA: 0x00217A68 File Offset: 0x00215C68
		private void OnCastSkillEnd(DataContext context, int charId, bool isAlly, short skillId, sbyte power, bool interrupted)
		{
			bool flag = charId != base.CharacterId || skillId != base.SkillTemplateId;
			if (!flag)
			{
				int markOrTrickCount = base.IsDirect ? ((int)base.CurrEnemyChar.GetTrickCount(20)) : this._affectMindMarkCount;
				bool flag2 = base.PowerMatchAffectRequire((int)power, 0) && markOrTrickCount > 0;
				if (flag2)
				{
					List<NeedTrick> removeTricks = ObjectPool<List<NeedTrick>>.Instance.Get();
					removeTricks.Clear();
					removeTricks.Add(new NeedTrick(20, (byte)markOrTrickCount));
					bool isDirect = base.IsDirect;
					if (isDirect)
					{
						DomainManager.Combat.RemoveTrick(context, base.CurrEnemyChar, removeTricks, false, false, -1);
					}
					else
					{
						DomainManager.Combat.RemoveMindDefeatMark(context, base.CurrEnemyChar, markOrTrickCount, false, 0);
					}
					ObjectPool<List<NeedTrick>>.Instance.Return(removeTricks);
					DomainManager.Combat.AppendFatalDamageMark(context, base.CurrEnemyChar, markOrTrickCount, -1, -1, false, EDamageType.None);
					base.ShowSpecialEffectTips(1);
				}
				base.RemoveSelf(context);
			}
		}

		// Token: 0x04000E6C RID: 3692
		private const sbyte DirectRequireTrickCount = 3;

		// Token: 0x04000E6D RID: 3693
		private const sbyte ReverseRequireMarkCount = 6;

		// Token: 0x04000E6E RID: 3694
		private int _affectMindMarkCount;
	}
}
