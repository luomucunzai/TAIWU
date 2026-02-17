using System;
using GameData.Combat.Math;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;

namespace GameData.Domains.SpecialEffect.CombatSkill.Jingangzong.FistAndPalm
{
	// Token: 0x020004BD RID: 1213
	public class DaRiRuLaiZhang : CombatSkillEffectBase
	{
		// Token: 0x06003CFC RID: 15612 RVA: 0x0024F78F File Offset: 0x0024D98F
		public DaRiRuLaiZhang()
		{
		}

		// Token: 0x06003CFD RID: 15613 RVA: 0x0024F799 File Offset: 0x0024D999
		public DaRiRuLaiZhang(CombatSkillKey skillKey) : base(skillKey, 11107, -1)
		{
		}

		// Token: 0x06003CFE RID: 15614 RVA: 0x0024F7AA File Offset: 0x0024D9AA
		public override void OnEnable(DataContext context)
		{
			Events.RegisterHandler_PrepareSkillBegin(new Events.OnPrepareSkillBegin(this.OnPrepareSkillBegin));
			Events.RegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
		}

		// Token: 0x06003CFF RID: 15615 RVA: 0x0024F7D1 File Offset: 0x0024D9D1
		public override void OnDisable(DataContext context)
		{
			Events.UnRegisterHandler_PrepareSkillBegin(new Events.OnPrepareSkillBegin(this.OnPrepareSkillBegin));
			Events.UnRegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
		}

		// Token: 0x06003D00 RID: 15616 RVA: 0x0024F7F8 File Offset: 0x0024D9F8
		private void OnPrepareSkillBegin(DataContext context, int charId, bool isAlly, short skillId)
		{
			bool flag = charId != base.CharacterId || skillId != base.SkillTemplateId;
			if (!flag)
			{
				bool isDirect = base.IsDirect;
				if (isDirect)
				{
					base.AppendAffectedData(context, base.CharacterId, 155, EDataModifyType.TotalPercent, -1);
				}
				else
				{
					base.AppendAffectedAllEnemyData(context, 155, EDataModifyType.TotalPercent, -1);
				}
				this.InvalidateCombatStateCache(context);
				base.ShowSpecialEffectTips(0);
				base.ShowSpecialEffectTips(1);
			}
		}

		// Token: 0x06003D01 RID: 15617 RVA: 0x0024F870 File Offset: 0x0024DA70
		private void OnCastSkillEnd(DataContext context, int charId, bool isAlly, short skillId, sbyte power, bool interrupted)
		{
			bool flag = charId != base.CharacterId || skillId != base.SkillTemplateId;
			if (!flag)
			{
				base.RemoveSelf(context);
				this.InvalidateCombatStateCache(context);
			}
		}

		// Token: 0x06003D02 RID: 15618 RVA: 0x0024F8B0 File Offset: 0x0024DAB0
		private void InvalidateCombatStateCache(DataContext context)
		{
			bool isDirect = base.IsDirect;
			if (isDirect)
			{
				DomainManager.Combat.InvalidateCombatStateCache(context, base.CombatChar, 1);
				DomainManager.Combat.InvalidateCombatStateCache(context, base.CombatChar, 2);
			}
			else
			{
				int[] charList = DomainManager.Combat.GetCharacterList(!base.CombatChar.IsAlly);
				for (int i = 0; i < charList.Length; i++)
				{
					bool flag = charList[i] >= 0;
					if (flag)
					{
						CombatCharacter enemyChar = DomainManager.Combat.GetElement_CombatCharacterDict(charList[i]);
						DomainManager.Combat.InvalidateCombatStateCache(context, enemyChar, 1);
						DomainManager.Combat.InvalidateCombatStateCache(context, enemyChar, 2);
					}
				}
			}
		}

		// Token: 0x06003D03 RID: 15619 RVA: 0x0024F960 File Offset: 0x0024DB60
		public override int GetModifyValue(AffectedDataKey dataKey, int currModifyValue)
		{
			bool flag = dataKey.FieldId == 155;
			int result;
			if (flag)
			{
				result = ((dataKey.CharId == base.CharacterId) ? ((dataKey.CustomParam0 == 1) ? 100 : -50) : ((dataKey.CustomParam0 == 1) ? -50 : 100));
			}
			else
			{
				result = 0;
			}
			return result;
		}

		// Token: 0x040011EE RID: 4590
		private const sbyte StateAddPercent = 100;

		// Token: 0x040011EF RID: 4591
		private const sbyte StateReducePercent = -50;
	}
}
