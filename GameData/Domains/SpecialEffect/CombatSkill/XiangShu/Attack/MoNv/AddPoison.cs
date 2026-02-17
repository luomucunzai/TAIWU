using System;
using System.Collections.Generic;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;
using GameData.Domains.Item;
using GameData.Utilities;

namespace GameData.Domains.SpecialEffect.CombatSkill.XiangShu.Attack.MoNv
{
	// Token: 0x020002F0 RID: 752
	public class AddPoison : CombatSkillEffectBase
	{
		// Token: 0x0600336B RID: 13163 RVA: 0x00224E4E File Offset: 0x0022304E
		protected AddPoison()
		{
		}

		// Token: 0x0600336C RID: 13164 RVA: 0x00224E58 File Offset: 0x00223058
		protected AddPoison(CombatSkillKey skillKey, int type) : base(skillKey, type, -1)
		{
		}

		// Token: 0x0600336D RID: 13165 RVA: 0x00224E65 File Offset: 0x00223065
		public override void OnEnable(DataContext context)
		{
			Events.RegisterHandler_CombatStateMachineUpdateEnd(new Events.OnCombatStateMachineUpdateEnd(this.OnStateMachineUpdateEnd));
			Events.RegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
		}

		// Token: 0x0600336E RID: 13166 RVA: 0x00224E8C File Offset: 0x0022308C
		public override void OnDisable(DataContext context)
		{
			Events.UnRegisterHandler_CombatStateMachineUpdateEnd(new Events.OnCombatStateMachineUpdateEnd(this.OnStateMachineUpdateEnd));
			Events.UnRegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
		}

		// Token: 0x0600336F RID: 13167 RVA: 0x00224EB4 File Offset: 0x002230B4
		private unsafe void OnStateMachineUpdateEnd(DataContext context, CombatCharacter combatChar)
		{
			bool flag = base.CombatChar != combatChar || DomainManager.Combat.Pause;
			if (!flag)
			{
				this._frameCounter++;
				bool flag2 = this._frameCounter < 30 || !DomainManager.Combat.InAttackRange(base.CombatChar);
				if (!flag2)
				{
					this._frameCounter = 0;
					PoisonsAndLevels skillPoisons = base.SkillInstance.GetPoisons();
					int skillPower = (int)base.SkillInstance.GetPower();
					CombatCharacter enemyChar = DomainManager.Combat.GetCombatCharacter(!base.CombatChar.IsAlly, false);
					List<sbyte> typeList = ObjectPool<List<sbyte>>.Instance.Get();
					typeList.Clear();
					for (sbyte type = 0; type < 6; type += 1)
					{
						typeList.Add(type);
					}
					while (typeList.Count > (int)this.PoisonTypeCount)
					{
						typeList.RemoveAt(context.Random.Next(typeList.Count));
					}
					for (int i = 0; i < typeList.Count; i++)
					{
						sbyte type2 = typeList[i];
						DomainManager.Combat.AddPoison(context, base.CombatChar, enemyChar, type2, *(ref skillPoisons.Levels.FixedElementField + type2), (int)(*(ref skillPoisons.Values.FixedElementField + (IntPtr)type2 * 2)) * skillPower / 100 * 20 / 100, base.SkillTemplateId, true, true, default(ItemKey), false, false, false);
					}
					ObjectPool<List<sbyte>>.Instance.Return(typeList);
					DomainManager.Combat.AddToCheckFallenSet(enemyChar.GetId());
					base.ShowSpecialEffectTips(0);
				}
			}
		}

		// Token: 0x06003370 RID: 13168 RVA: 0x0022505C File Offset: 0x0022325C
		private void OnCastSkillEnd(DataContext context, int charId, bool isAlly, short skillId, sbyte power, bool interrupted)
		{
			bool flag = charId != base.CharacterId || skillId != base.SkillTemplateId;
			if (!flag)
			{
				base.RemoveSelf(context);
			}
		}

		// Token: 0x04000F34 RID: 3892
		private const short AffectFrameCount = 30;

		// Token: 0x04000F35 RID: 3893
		private const short PoisonPercent = 20;

		// Token: 0x04000F36 RID: 3894
		protected sbyte PoisonTypeCount;

		// Token: 0x04000F37 RID: 3895
		private int _frameCounter;
	}
}
