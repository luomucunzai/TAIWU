using System;
using System.Collections.Generic;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Assist;

namespace GameData.Domains.SpecialEffect.CombatSkill.Baihuagu.DefenseAndAssist
{
	// Token: 0x020005D7 RID: 1495
	public class LuanZhenCuoXue : AssistSkillBase
	{
		// Token: 0x0600442B RID: 17451 RVA: 0x0026E4BD File Offset: 0x0026C6BD
		public LuanZhenCuoXue()
		{
		}

		// Token: 0x0600442C RID: 17452 RVA: 0x0026E4D9 File Offset: 0x0026C6D9
		public LuanZhenCuoXue(CombatSkillKey skillKey) : base(skillKey, 3603)
		{
		}

		// Token: 0x0600442D RID: 17453 RVA: 0x0026E4FC File Offset: 0x0026C6FC
		public override void OnEnable(DataContext context)
		{
			base.OnEnable(context);
			this._affecting = false;
			bool isDirect = base.IsDirect;
			if (isDirect)
			{
				Events.RegisterHandler_AcuPointRemoved(new Events.OnAcuPointRemoved(this.OnAcuPointRemoved));
			}
			else
			{
				Events.RegisterHandler_AcuPointAdded(new Events.OnAcuPointAdded(this.OnAcupointAdded));
			}
			Events.RegisterHandler_CombatStateMachineUpdateEnd(new Events.OnCombatStateMachineUpdateEnd(this.OnStateMachineUpdateEnd));
		}

		// Token: 0x0600442E RID: 17454 RVA: 0x0026E55C File Offset: 0x0026C75C
		public override void OnDisable(DataContext context)
		{
			base.OnDisable(context);
			bool isDirect = base.IsDirect;
			if (isDirect)
			{
				Events.UnRegisterHandler_AcuPointRemoved(new Events.OnAcuPointRemoved(this.OnAcuPointRemoved));
			}
			else
			{
				Events.UnRegisterHandler_AcuPointAdded(new Events.OnAcuPointAdded(this.OnAcupointAdded));
			}
			Events.UnRegisterHandler_CombatStateMachineUpdateEnd(new Events.OnCombatStateMachineUpdateEnd(this.OnStateMachineUpdateEnd));
		}

		// Token: 0x0600442F RID: 17455 RVA: 0x0026E5B4 File Offset: 0x0026C7B4
		private void OnAcuPointRemoved(DataContext context, CombatCharacter combatChar, sbyte bodyPart, sbyte level)
		{
			bool flag = combatChar != base.CombatChar || this._affecting || !base.CanAffect;
			if (!flag)
			{
				this._bodyPart = bodyPart;
			}
		}

		// Token: 0x06004430 RID: 17456 RVA: 0x0026E5EC File Offset: 0x0026C7EC
		private void OnAcupointAdded(DataContext context, CombatCharacter combatChar, sbyte bodyPart, sbyte level)
		{
			bool flag = combatChar.IsAlly == base.CombatChar.IsAlly || this._affecting || !base.CanAffect || !DomainManager.Combat.IsCurrentCombatCharacter(base.CombatChar) || base.CombatChar.TeammateBeforeMainChar >= 0;
			if (!flag)
			{
				this._bodyPart = bodyPart;
				this._reverseEnemyChar = combatChar;
				this._reverseLevel = level;
			}
		}

		// Token: 0x06004431 RID: 17457 RVA: 0x0026E660 File Offset: 0x0026C860
		private void OnStateMachineUpdateEnd(DataContext context, CombatCharacter combatChar)
		{
			bool flag = combatChar.GetId() != base.CharacterId || this._bodyPart < 0;
			if (!flag)
			{
				CombatCharacter targetChar = base.IsDirect ? base.CombatChar : this._reverseEnemyChar;
				byte[] acupointCount = targetChar.GetAcupointCount();
				int maxCount = base.IsDirect ? -1 : this._reverseEnemyChar.GetMaxAcupointCount();
				this._bodyPartRandomPool.Clear();
				foreach (sbyte part in targetChar.GetAvailableBodyParts())
				{
					bool flag2 = part != this._bodyPart && (base.IsDirect ? (acupointCount[(int)part] > 0) : ((int)acupointCount[(int)part] < maxCount));
					if (flag2)
					{
						this._bodyPartRandomPool.Add(part);
					}
				}
				bool flag3 = this._bodyPartRandomPool.Count > 0;
				if (flag3)
				{
					sbyte part2 = this._bodyPartRandomPool[context.Random.Next(0, this._bodyPartRandomPool.Count)];
					this._affecting = true;
					bool isDirect = base.IsDirect;
					if (isDirect)
					{
						DomainManager.Combat.RemoveAcupoint(context, base.CombatChar, part2, 0, true, true);
					}
					else
					{
						DomainManager.Combat.AddAcupoint(context, this._reverseEnemyChar, this._reverseLevel, new CombatSkillKey(-1, -1), part2, 1, true);
					}
					this._affecting = false;
					base.ShowSpecialEffectTips(0);
				}
				this._bodyPart = -1;
			}
		}

		// Token: 0x0400143A RID: 5178
		private readonly List<sbyte> _bodyPartRandomPool = new List<sbyte>();

		// Token: 0x0400143B RID: 5179
		private bool _affecting;

		// Token: 0x0400143C RID: 5180
		private sbyte _bodyPart = -1;

		// Token: 0x0400143D RID: 5181
		private CombatCharacter _reverseEnemyChar;

		// Token: 0x0400143E RID: 5182
		private sbyte _reverseLevel;
	}
}
