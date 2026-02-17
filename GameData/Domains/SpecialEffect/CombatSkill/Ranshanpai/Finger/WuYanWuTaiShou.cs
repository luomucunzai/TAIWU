using System;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Character;
using GameData.Domains.CombatSkill;

namespace GameData.Domains.SpecialEffect.CombatSkill.Ranshanpai.Finger
{
	// Token: 0x02000461 RID: 1121
	public class WuYanWuTaiShou : CombatSkillEffectBase
	{
		// Token: 0x06003AEA RID: 15082 RVA: 0x00245C11 File Offset: 0x00243E11
		public WuYanWuTaiShou()
		{
		}

		// Token: 0x06003AEB RID: 15083 RVA: 0x00245C32 File Offset: 0x00243E32
		public WuYanWuTaiShou(CombatSkillKey skillKey) : base(skillKey, 7102, -1)
		{
		}

		// Token: 0x06003AEC RID: 15084 RVA: 0x00245C5A File Offset: 0x00243E5A
		public override void OnEnable(DataContext context)
		{
			Events.RegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
		}

		// Token: 0x06003AED RID: 15085 RVA: 0x00245C6F File Offset: 0x00243E6F
		public override void OnDisable(DataContext context)
		{
			Events.UnRegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
		}

		// Token: 0x06003AEE RID: 15086 RVA: 0x00245C84 File Offset: 0x00243E84
		private void OnCastSkillEnd(DataContext context, int charId, bool isAlly, short skillId, sbyte power, bool interrupted)
		{
			bool flag = charId != base.CharacterId || skillId != base.SkillTemplateId;
			if (!flag)
			{
				bool flag2 = base.PowerMatchAffectRequire((int)power, 0) && this.CalcAbovePersonalityCount() >= 3;
				if (flag2)
				{
					bool isDirect = base.IsDirect;
					if (isDirect)
					{
						DomainManager.Combat.AddTrick(context, base.CurrEnemyChar, 20, 2, false, false);
					}
					else
					{
						DomainManager.Combat.AppendMindDefeatMark(context, base.CurrEnemyChar, 2, -1, false);
					}
					this.RecordFameAction(context);
					base.ShowSpecialEffectTips(0);
					base.ShowSpecialEffectTips(1);
				}
				base.RemoveSelf(context);
			}
		}

		// Token: 0x06003AEF RID: 15087 RVA: 0x00245D2C File Offset: 0x00243F2C
		private unsafe int CalcAbovePersonalityCount()
		{
			Personalities selfPersonalities = this.CharObj.GetPersonalities();
			Personalities enemyPersonalities = base.CurrEnemyChar.GetCharacter().GetPersonalities();
			int personalityCount = 0;
			foreach (sbyte type in this._requirePersonalities)
			{
				bool flag = *selfPersonalities[(int)type] > *enemyPersonalities[(int)type];
				if (flag)
				{
					personalityCount++;
				}
			}
			return personalityCount;
		}

		// Token: 0x06003AF0 RID: 15088 RVA: 0x00245DA0 File Offset: 0x00243FA0
		private void RecordFameAction(DataContext context)
		{
			Character selfChar = base.CombatChar.GetCharacter();
			Character enemyChar = base.CurrEnemyChar.GetCharacter();
			short selfFameId = (short)(41 + (base.IsDirect ? 0 : 1));
			short enemyFameId = base.IsDirect ? 52 : 53;
			selfChar.RecordFameAction(context, selfFameId, -1, 3, true);
			enemyChar.RecordFameAction(context, enemyFameId, -1, 3, true);
		}

		// Token: 0x04001142 RID: 4418
		private readonly sbyte[] _requirePersonalities = new sbyte[]
		{
			0,
			1,
			2,
			3,
			4
		};

		// Token: 0x04001143 RID: 4419
		private const sbyte AddTrickOrMarkNeedPersonalityCount = 3;

		// Token: 0x04001144 RID: 4420
		private const sbyte AddTrickOrMarkCount = 2;

		// Token: 0x04001145 RID: 4421
		private const short FameMultiplier = 3;
	}
}
