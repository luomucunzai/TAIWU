using System;
using System.Collections.Generic;
using GameData.Combat.Math;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Character;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;
using GameData.Utilities;

namespace GameData.Domains.SpecialEffect.CombatSkill.Xuannvpai.Music
{
	// Token: 0x02000269 RID: 617
	public class GuangHanGe : CombatSkillEffectBase
	{
		// Token: 0x0600306C RID: 12396 RVA: 0x00216E93 File Offset: 0x00215093
		public GuangHanGe()
		{
		}

		// Token: 0x0600306D RID: 12397 RVA: 0x00216E9D File Offset: 0x0021509D
		public GuangHanGe(CombatSkillKey skillKey) : base(skillKey, 8306, -1)
		{
		}

		// Token: 0x0600306E RID: 12398 RVA: 0x00216EB0 File Offset: 0x002150B0
		public unsafe override void OnEnable(DataContext context)
		{
			CombatDomain combatDomain = DomainManager.Combat;
			bool selfAnyTeammate = combatDomain.AnyTeammateChar(base.CombatChar.IsAlly);
			bool enemyAnyTeammate = combatDomain.AnyTeammateChar(!base.CombatChar.IsAlly);
			bool addPower = base.IsDirect ? (!enemyAnyTeammate) : (!selfAnyTeammate);
			bool addNeiliAllocation = base.IsDirect ? (enemyAnyTeammate && !selfAnyTeammate) : (selfAnyTeammate && !enemyAnyTeammate);
			bool flag = addPower;
			if (flag)
			{
				this.AffectDatas = new Dictionary<AffectedDataKey, EDataModifyType>();
				this.AffectDatas.Add(new AffectedDataKey(base.CharacterId, 199, base.SkillTemplateId, -1, -1, -1), EDataModifyType.AddPercent);
				base.ShowSpecialEffectTips(0);
			}
			bool flag2 = addNeiliAllocation;
			if (flag2)
			{
				CombatCharacter targetChar = base.IsDirect ? base.CombatChar : base.CurrEnemyChar;
				int[] characterList = combatDomain.GetCharacterList(base.IsDirect ? (!base.CombatChar.IsAlly) : base.CombatChar.IsAlly);
				List<int> teammateCharIds = ObjectPool<List<int>>.Instance.Get();
				for (int i = 1; i < characterList.Length; i++)
				{
					bool flag3 = characterList[i] >= 0;
					if (flag3)
					{
						teammateCharIds.Add(characterList[i]);
					}
				}
				int teammateCharId = teammateCharIds.GetRandom(context.Random);
				CombatCharacter teammateChar = combatDomain.GetElement_CombatCharacterDict(teammateCharId);
				ObjectPool<List<int>>.Instance.Return(teammateCharIds);
				NeiliAllocation teammateNeiliAllocation = teammateChar.GetNeiliAllocation();
				for (byte type = 0; type < 4; type += 1)
				{
					int addValue = (int)(*teammateNeiliAllocation[(int)type] * 20 / 100);
					bool flag4 = addValue > 0;
					if (flag4)
					{
						targetChar.ChangeNeiliAllocation(context, type, base.IsDirect ? addValue : (-addValue), true, true);
					}
				}
				base.ShowSpecialEffectTips(2);
			}
			Events.RegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
		}

		// Token: 0x0600306F RID: 12399 RVA: 0x0021708E File Offset: 0x0021528E
		public override void OnDisable(DataContext context)
		{
			Events.UnRegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
		}

		// Token: 0x06003070 RID: 12400 RVA: 0x002170A4 File Offset: 0x002152A4
		private void OnCastSkillEnd(DataContext context, int charId, bool isAlly, short skillId, sbyte power, bool interrupted)
		{
			bool flag = charId != base.CharacterId || skillId != base.SkillTemplateId;
			if (!flag)
			{
				bool flag2 = base.PowerMatchAffectRequire((int)power, 0);
				if (flag2)
				{
					bool isDirect = base.IsDirect;
					if (isDirect)
					{
						DomainManager.Combat.AddTrick(context, base.CurrEnemyChar, 20, 3, false, false);
					}
					else
					{
						DomainManager.Combat.AppendMindDefeatMark(context, base.CurrEnemyChar, 3, -1, false);
					}
					base.ShowSpecialEffectTips(1);
				}
				base.RemoveSelf(context);
			}
		}

		// Token: 0x06003071 RID: 12401 RVA: 0x00217128 File Offset: 0x00215328
		public override int GetModifyValue(AffectedDataKey dataKey, int currModifyValue)
		{
			bool flag = dataKey.CharId != base.CharacterId || dataKey.CombatSkillId != base.SkillTemplateId;
			int result;
			if (flag)
			{
				result = 0;
			}
			else
			{
				bool flag2 = dataKey.FieldId == 199;
				if (flag2)
				{
					result = 40;
				}
				else
				{
					result = 0;
				}
			}
			return result;
		}

		// Token: 0x04000E5B RID: 3675
		private const sbyte AddPower = 40;

		// Token: 0x04000E5C RID: 3676
		private const sbyte AddTrickOrMarkCount = 3;

		// Token: 0x04000E5D RID: 3677
		private const int NeiliAllocationPercent = 20;
	}
}
