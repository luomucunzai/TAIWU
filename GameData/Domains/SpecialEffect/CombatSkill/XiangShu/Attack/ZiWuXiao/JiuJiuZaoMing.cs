using System;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Character;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;
using GameData.GameDataBridge;
using GameData.Utilities;

namespace GameData.Domains.SpecialEffect.CombatSkill.XiangShu.Attack.ZiWuXiao
{
	// Token: 0x020002B8 RID: 696
	public class JiuJiuZaoMing : CombatSkillEffectBase
	{
		// Token: 0x0600322A RID: 12842 RVA: 0x0021E26F File Offset: 0x0021C46F
		public JiuJiuZaoMing()
		{
		}

		// Token: 0x0600322B RID: 12843 RVA: 0x0021E285 File Offset: 0x0021C485
		public JiuJiuZaoMing(CombatSkillKey skillKey) : base(skillKey, 17112, -1)
		{
		}

		// Token: 0x0600322C RID: 12844 RVA: 0x0021E2A4 File Offset: 0x0021C4A4
		public override void OnEnable(DataContext context)
		{
			Array.Copy(base.CombatChar.GetDefeatMarkCollection().PoisonMarkList, this._selfLastPoisonMark, 6);
			this._selfMarkUid = new DataUid(8, 10, (ulong)((long)base.CharacterId), 50U);
			GameDataBridge.AddPostDataModificationHandler(this._selfMarkUid, base.DataHandlerKey, new Action<DataContext, DataUid>(this.OnSelfMarkChanged));
			this.UpdateEnemyUid(true);
			Events.RegisterHandler_PrepareSkillBegin(new Events.OnPrepareSkillBegin(this.OnPrepareSkillBegin));
			Events.RegisterHandler_CombatCharChanged(new Events.OnCombatCharChanged(this.OnCombatCharChanged));
			Events.RegisterHandler_SkillEffectChange(new Events.OnSkillEffectChange(this.OnSkillEffectChange));
		}

		// Token: 0x0600322D RID: 12845 RVA: 0x0021E344 File Offset: 0x0021C544
		public override void OnDisable(DataContext context)
		{
			GameDataBridge.RemovePostDataModificationHandler(this._selfMarkUid, base.DataHandlerKey);
			GameDataBridge.RemovePostDataModificationHandler(this._enemyInjuriesUid, base.DataHandlerKey);
			Events.UnRegisterHandler_PrepareSkillBegin(new Events.OnPrepareSkillBegin(this.OnPrepareSkillBegin));
			Events.UnRegisterHandler_CombatCharChanged(new Events.OnCombatCharChanged(this.OnCombatCharChanged));
			Events.UnRegisterHandler_SkillEffectChange(new Events.OnSkillEffectChange(this.OnSkillEffectChange));
		}

		// Token: 0x0600322E RID: 12846 RVA: 0x0021E3AC File Offset: 0x0021C5AC
		private void OnPrepareSkillBegin(DataContext context, int charId, bool isAlly, short skillId)
		{
			bool flag = charId != base.CharacterId || skillId != base.SkillTemplateId;
			if (!flag)
			{
				bool flag2 = base.EffectCount <= 0;
				if (flag2)
				{
					this.IsSrcSkillPerformed = true;
					base.AddMaxEffectCount(true);
				}
				else
				{
					base.RemoveSelf(context);
				}
			}
		}

		// Token: 0x0600322F RID: 12847 RVA: 0x0021E408 File Offset: 0x0021C608
		private void OnCombatCharChanged(DataContext context, bool isAlly)
		{
			bool flag = isAlly == base.CombatChar.IsAlly;
			if (!flag)
			{
				this.UpdateEnemyUid(false);
			}
		}

		// Token: 0x06003230 RID: 12848 RVA: 0x0021E434 File Offset: 0x0021C634
		private void OnSkillEffectChange(DataContext context, int charId, SkillEffectKey key, short oldCount, short newCount, bool removed)
		{
			bool flag = removed && this.IsSrcSkillPerformed && charId == base.CharacterId && key.SkillId == base.SkillTemplateId && key.IsDirect == base.IsDirect;
			if (flag)
			{
				base.RemoveSelf(context);
			}
		}

		// Token: 0x06003231 RID: 12849 RVA: 0x0021E484 File Offset: 0x0021C684
		private void OnSelfMarkChanged(DataContext context, DataUid dataUid)
		{
			byte[] poisonMarks = base.CombatChar.GetDefeatMarkCollection().PoisonMarkList;
			for (sbyte type = 0; type < 6; type += 1)
			{
				byte newCount = poisonMarks[(int)type];
				bool flag = newCount > this._selfLastPoisonMark[(int)type];
				if (flag)
				{
					base.ReduceEffectCount((int)(newCount - this._selfLastPoisonMark[(int)type]));
					bool flag2 = base.EffectCount <= 0;
					if (flag2)
					{
						break;
					}
				}
				this._selfLastPoisonMark[(int)type] = newCount;
			}
		}

		// Token: 0x06003232 RID: 12850 RVA: 0x0021E4FC File Offset: 0x0021C6FC
		private void OnEnemyInjuriesChanged(DataContext context, DataUid dataUid)
		{
			Injuries selfInjuries = base.CombatChar.GetInjuries();
			Injuries newInjuries = DomainManager.Combat.GetCombatCharacter(!base.CombatChar.IsAlly, false).GetInjuries();
			bool affected = false;
			for (sbyte part = 0; part < 7; part += 1)
			{
				ValueTuple<sbyte, sbyte> selfInjury = selfInjuries.Get(part);
				bool flag = selfInjury.Item1 == 0 && selfInjury.Item2 == 0;
				if (!flag)
				{
					ValueTuple<sbyte, sbyte> newInjury = newInjuries.Get(part);
					ValueTuple<sbyte, sbyte> lastInjury = this._enemyLastInjuries.Get(part);
					int healCount = 0;
					bool flag2 = newInjury.Item1 != lastInjury.Item1;
					if (flag2)
					{
						healCount += Math.Abs((int)(newInjury.Item1 - lastInjury.Item1));
					}
					bool flag3 = newInjury.Item2 != lastInjury.Item2;
					if (flag3)
					{
						healCount += Math.Abs((int)(newInjury.Item2 - lastInjury.Item2));
					}
					bool flag4 = healCount == 0;
					if (!flag4)
					{
						while ((int)(selfInjury.Item1 + selfInjury.Item2) > healCount)
						{
							bool flag5 = selfInjury.Item1 > 0 && (selfInjury.Item2 == 0 || context.Random.CheckPercentProb(50));
							if (flag5)
							{
								selfInjury.Item1 -= 1;
							}
							else
							{
								selfInjury.Item2 -= 1;
							}
						}
						bool flag6 = selfInjury.Item1 > 0;
						if (flag6)
						{
							DomainManager.Combat.RemoveInjury(context, base.CombatChar, part, false, selfInjury.Item1, false, false);
						}
						bool flag7 = selfInjury.Item2 > 0;
						if (flag7)
						{
							DomainManager.Combat.RemoveInjury(context, base.CombatChar, part, true, selfInjury.Item2, false, false);
						}
						affected = true;
					}
				}
			}
			this._enemyLastInjuries = newInjuries;
			bool flag8 = affected;
			if (flag8)
			{
				DomainManager.Combat.UpdateBodyDefeatMark(context, base.CombatChar);
				base.ShowSpecialEffectTips(0);
			}
		}

		// Token: 0x06003233 RID: 12851 RVA: 0x0021E6F8 File Offset: 0x0021C8F8
		private void UpdateEnemyUid(bool init)
		{
			CombatCharacter currEnemy = DomainManager.Combat.GetCombatCharacter(!base.CombatChar.IsAlly, false);
			this._enemyLastInjuries = currEnemy.GetInjuries();
			bool flag = !init;
			if (flag)
			{
				GameDataBridge.RemovePostDataModificationHandler(this._enemyInjuriesUid, base.DataHandlerKey);
			}
			this._enemyInjuriesUid = new DataUid(8, 10, (ulong)((long)currEnemy.GetId()), 29U);
			GameDataBridge.AddPostDataModificationHandler(this._enemyInjuriesUid, base.DataHandlerKey, new Action<DataContext, DataUid>(this.OnEnemyInjuriesChanged));
		}

		// Token: 0x04000EDB RID: 3803
		private DataUid _selfMarkUid;

		// Token: 0x04000EDC RID: 3804
		private readonly byte[] _selfLastPoisonMark = new byte[6];

		// Token: 0x04000EDD RID: 3805
		private DataUid _enemyInjuriesUid;

		// Token: 0x04000EDE RID: 3806
		private Injuries _enemyLastInjuries;
	}
}
