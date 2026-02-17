using System;
using System.Linq;
using GameData.Combat.Math;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;
using GameData.Utilities;

namespace GameData.Domains.SpecialEffect.CombatSkill.Ranshanpai.Special
{
	// Token: 0x0200044C RID: 1100
	public class JingGuiFu : CombatSkillEffectBase
	{
		// Token: 0x1700020F RID: 527
		// (get) Token: 0x06003A56 RID: 14934 RVA: 0x00243101 File Offset: 0x00241301
		private unsafe int PersonalitiesSum
		{
			get
			{
				return this._personalityTypes.Sum((sbyte x) => (int)(*this.CharObj.GetPersonalities()[(int)x]));
			}
		}

		// Token: 0x17000210 RID: 528
		// (get) Token: 0x06003A57 RID: 14935 RVA: 0x0024311A File Offset: 0x0024131A
		private int FleeOdds
		{
			get
			{
				return 50 + this.PersonalitiesSum / 10;
			}
		}

		// Token: 0x17000211 RID: 529
		// (get) Token: 0x06003A58 RID: 14936 RVA: 0x00243128 File Offset: 0x00241328
		private int TrickOrMarkCount
		{
			get
			{
				return 3 + this.PersonalitiesSum / 75;
			}
		}

		// Token: 0x06003A59 RID: 14937 RVA: 0x00243135 File Offset: 0x00241335
		public JingGuiFu()
		{
		}

		// Token: 0x06003A5A RID: 14938 RVA: 0x0024315D File Offset: 0x0024135D
		public JingGuiFu(CombatSkillKey skillKey) : base(skillKey, 7302, -1)
		{
		}

		// Token: 0x06003A5B RID: 14939 RVA: 0x0024318C File Offset: 0x0024138C
		public override void OnEnable(DataContext context)
		{
			base.CreateAffectedAllEnemyData(124, EDataModifyType.AddPercent, -1);
			Events.RegisterHandler_PrepareSkillEnd(new Events.OnPrepareSkillEnd(this.OnPrepareSkillEnd));
			Events.RegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
			Events.RegisterHandler_InterruptOtherAction(new Events.OnInterruptOtherAction(this.OnInterruptOtherAction));
		}

		// Token: 0x06003A5C RID: 14940 RVA: 0x002431DB File Offset: 0x002413DB
		public override void OnDisable(DataContext context)
		{
			Events.UnRegisterHandler_PrepareSkillEnd(new Events.OnPrepareSkillEnd(this.OnPrepareSkillEnd));
			Events.UnRegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
			Events.UnRegisterHandler_InterruptOtherAction(new Events.OnInterruptOtherAction(this.OnInterruptOtherAction));
		}

		// Token: 0x06003A5D RID: 14941 RVA: 0x00243214 File Offset: 0x00241414
		private void OnPrepareSkillEnd(DataContext context, int charId, bool isAlly, short skillId)
		{
			bool flag = charId != base.CharacterId || skillId != base.SkillTemplateId;
			if (!flag)
			{
				CombatCharacter enemyChar = DomainManager.Combat.GetCombatCharacter(!base.CombatChar.IsAlly, false);
				this._affecting = !enemyChar.AiController.Memory.EnemyRecordDict[base.CharacterId].SkillRecord.ContainsKey(base.SkillTemplateId);
			}
		}

		// Token: 0x06003A5E RID: 14942 RVA: 0x00243290 File Offset: 0x00241490
		private void OnCastSkillEnd(DataContext context, int charId, bool isAlly, short skillId, sbyte power, bool interrupted)
		{
			bool flag = !this.SkillKey.IsMatch(charId, skillId) || !base.PowerMatchAffectRequire((int)power, 0);
			if (!flag)
			{
				CombatCharacter enemyChar = DomainManager.Combat.GetCombatCharacter(!base.CombatChar.IsAlly, false);
				bool flag2 = enemyChar.AiController.CanFlee();
				if (flag2)
				{
					this.DoFlee(context, enemyChar);
				}
				else
				{
					this.DoTrickOrMark(context);
				}
			}
		}

		// Token: 0x06003A5F RID: 14943 RVA: 0x00243300 File Offset: 0x00241500
		private void OnInterruptOtherAction(DataContext context, CombatCharacter combatChar, sbyte otherActionType)
		{
			bool flag = combatChar.GetId() == this._reduceFleeSpeedCharId && otherActionType == 2;
			if (flag)
			{
				this._reduceFleeSpeedCharId = -1;
			}
		}

		// Token: 0x06003A60 RID: 14944 RVA: 0x00243330 File Offset: 0x00241530
		public override int GetModifyValue(AffectedDataKey dataKey, int currModifyValue)
		{
			bool flag = dataKey.FieldId == 124 && dataKey.CharId == this._reduceFleeSpeedCharId;
			int result;
			if (flag)
			{
				result = -67;
			}
			else
			{
				result = 0;
			}
			return result;
		}

		// Token: 0x06003A61 RID: 14945 RVA: 0x00243368 File Offset: 0x00241568
		private void DoFlee(DataContext context, CombatCharacter enemyChar)
		{
			bool flag = !context.Random.CheckPercentProb(this.FleeOdds);
			if (!flag)
			{
				enemyChar.CanFleeOutOfRange = true;
				enemyChar.SetNeedUseOtherAction(context, 2);
				this._reduceFleeSpeedCharId = enemyChar.GetId();
				base.ShowSpecialEffectTips(0);
			}
		}

		// Token: 0x06003A62 RID: 14946 RVA: 0x002433B4 File Offset: 0x002415B4
		private void DoTrickOrMark(DataContext context)
		{
			bool flag = !this._affecting;
			if (!flag)
			{
				bool isDirect = base.IsDirect;
				if (isDirect)
				{
					DomainManager.Combat.AddTrick(context, base.CurrEnemyChar, 20, this.TrickOrMarkCount, false, false);
				}
				else
				{
					DomainManager.Combat.AppendMindDefeatMark(context, base.CurrEnemyChar, this.TrickOrMarkCount, -1, false);
				}
				base.ShowSpecialEffectTips(1);
			}
		}

		// Token: 0x04001113 RID: 4371
		private const int ReduceFleeSpeedPercent = -67;

		// Token: 0x04001114 RID: 4372
		private readonly sbyte[] _personalityTypes = new sbyte[]
		{
			0,
			1,
			2,
			3,
			4
		};

		// Token: 0x04001115 RID: 4373
		private bool _affecting;

		// Token: 0x04001116 RID: 4374
		private int _reduceFleeSpeedCharId = -1;
	}
}
