using System;
using System.Runtime.CompilerServices;
using GameData.Common;
using GameData.DomainEvents;

namespace GameData.Domains.Combat.Ai
{
	// Token: 0x0200071C RID: 1820
	public class AiEnvironment
	{
		// Token: 0x06006893 RID: 26771 RVA: 0x003B7444 File Offset: 0x003B5644
		public AiEnvironment(CombatCharacter combatChar)
		{
			this._combatChar = combatChar;
		}

		// Token: 0x06006894 RID: 26772 RVA: 0x003B7468 File Offset: 0x003B5668
		public void RegisterCallbacks()
		{
			Events.RegisterHandler_NormalAttackEnd(new Events.OnNormalAttackEnd(this.OnNormalAttackEnd));
			Events.RegisterHandler_NormalAttackOutOfRange(new Events.OnNormalAttackOutOfRange(this.OnNormalAttackOutOfRange));
			Events.RegisterHandler_PrepareSkillBegin(new Events.OnPrepareSkillBegin(this.OnPrepareSkillBegin));
		}

		// Token: 0x06006895 RID: 26773 RVA: 0x003B74A1 File Offset: 0x003B56A1
		public void UnregisterCallbacks()
		{
			Events.UnRegisterHandler_NormalAttackEnd(new Events.OnNormalAttackEnd(this.OnNormalAttackEnd));
			Events.UnRegisterHandler_NormalAttackOutOfRange(new Events.OnNormalAttackOutOfRange(this.OnNormalAttackOutOfRange));
			Events.UnRegisterHandler_PrepareSkillBegin(new Events.OnPrepareSkillBegin(this.OnPrepareSkillBegin));
		}

		// Token: 0x06006896 RID: 26774 RVA: 0x003B74DC File Offset: 0x003B56DC
		private void OnNormalAttackEnd(DataContext context, CombatCharacter attacker, CombatCharacter defender, sbyte trickType, int pursueIndex, bool hit, bool isFightback)
		{
			bool flag = attacker.GetId() != this._combatChar.GetId();
			if (!flag)
			{
				bool flag2 = pursueIndex == 0;
				if (flag2)
				{
					this.LastNormalAttack = new ValueTuple<int, AiEnvironment.ENormalAttackType>(attacker.GetUsingWeaponIndex(), hit ? AiEnvironment.ENormalAttackType.Hit : AiEnvironment.ENormalAttackType.Miss);
				}
				else
				{
					bool flag3 = this.LastNormalAttack.Item2 == AiEnvironment.ENormalAttackType.Hit && !hit;
					if (flag3)
					{
						this.LastNormalAttack.Item2 = AiEnvironment.ENormalAttackType.Miss;
					}
				}
			}
		}

		// Token: 0x06006897 RID: 26775 RVA: 0x003B7550 File Offset: 0x003B5750
		private void OnNormalAttackOutOfRange(DataContext context, int charId, bool isAlly)
		{
			bool flag = charId != this._combatChar.GetId();
			if (!flag)
			{
				this.LastNormalAttack = new ValueTuple<int, AiEnvironment.ENormalAttackType>(this._combatChar.GetUsingWeaponIndex(), AiEnvironment.ENormalAttackType.OutOfRange);
			}
		}

		// Token: 0x06006898 RID: 26776 RVA: 0x003B758C File Offset: 0x003B578C
		private void OnPrepareSkillBegin(DataContext context, int charId, bool isAlly, short skillId)
		{
			bool flag = charId != this._combatChar.GetId();
			if (!flag)
			{
				this.LastPrepareSkill = skillId;
			}
		}

		// Token: 0x04001CA6 RID: 7334
		private readonly CombatCharacter _combatChar;

		// Token: 0x04001CA7 RID: 7335
		[TupleElementNames(new string[]
		{
			"weaponIndex",
			"type"
		})]
		public ValueTuple<int, AiEnvironment.ENormalAttackType> LastNormalAttack = new ValueTuple<int, AiEnvironment.ENormalAttackType>(-1, AiEnvironment.ENormalAttackType.None);

		// Token: 0x04001CA8 RID: 7336
		public short LastPrepareSkill = -1;

		// Token: 0x02000B9F RID: 2975
		public enum ENormalAttackType
		{
			// Token: 0x040031F0 RID: 12784
			None,
			// Token: 0x040031F1 RID: 12785
			Hit,
			// Token: 0x040031F2 RID: 12786
			Miss,
			// Token: 0x040031F3 RID: 12787
			OutOfRange
		}
	}
}
