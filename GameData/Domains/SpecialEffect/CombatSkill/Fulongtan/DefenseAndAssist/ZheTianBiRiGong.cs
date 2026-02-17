using System;
using System.Collections.Generic;
using GameData.Combat.Math;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Defense;
using GameData.GameDataBridge;
using GameData.Utilities;

namespace GameData.Domains.SpecialEffect.CombatSkill.Fulongtan.DefenseAndAssist
{
	// Token: 0x02000529 RID: 1321
	public class ZheTianBiRiGong : DefenseSkillBase
	{
		// Token: 0x06003F52 RID: 16210 RVA: 0x00259568 File Offset: 0x00257768
		public ZheTianBiRiGong()
		{
		}

		// Token: 0x06003F53 RID: 16211 RVA: 0x00259572 File Offset: 0x00257772
		public ZheTianBiRiGong(CombatSkillKey skillKey) : base(skillKey, 14507)
		{
		}

		// Token: 0x06003F54 RID: 16212 RVA: 0x00259584 File Offset: 0x00257784
		public override void OnEnable(DataContext context)
		{
			this._defendSkillUid = new DataUid(8, 10, (ulong)((long)base.CharacterId), 63U);
			GameDataBridge.AddPostDataModificationHandler(this._defendSkillUid, base.DataHandlerKey, new Action<DataContext, DataUid>(this.OnDefendSkillChanged));
			Events.RegisterHandler_PrepareSkillBegin(new Events.OnPrepareSkillBegin(this.OnPrepareSkillBegin));
		}

		// Token: 0x06003F55 RID: 16213 RVA: 0x002595D9 File Offset: 0x002577D9
		public override void OnDisable(DataContext context)
		{
			GameDataBridge.RemovePostDataModificationHandler(this._defendSkillUid, base.DataHandlerKey);
			Events.UnRegisterHandler_PrepareSkillBegin(new Events.OnPrepareSkillBegin(this.OnPrepareSkillBegin));
		}

		// Token: 0x06003F56 RID: 16214 RVA: 0x00259600 File Offset: 0x00257800
		private void OnDefendSkillChanged(DataContext context, DataUid dataUid)
		{
			bool flag = base.CombatChar.GetAffectingDefendSkillId() == base.SkillTemplateId;
			if (flag)
			{
				base.AppendAffectedAllEnemyData(context, 89, EDataModifyType.Custom, -1);
				base.AppendAffectedAllEnemyData(context, 271, EDataModifyType.Custom, -1);
				DomainManager.Combat.UpdateAllTeammateCommandUsable(context, !base.CombatChar.IsAlly, -1);
			}
			else
			{
				base.ClearAffectedData(context);
				DomainManager.Combat.UpdateAllTeammateCommandUsable(context, !base.CombatChar.IsAlly, -1);
			}
		}

		// Token: 0x06003F57 RID: 16215 RVA: 0x00259684 File Offset: 0x00257884
		private void OnPrepareSkillBegin(DataContext context, int charId, bool isAlly, short skillId)
		{
			bool flag = charId != base.CharacterId || skillId != base.SkillTemplateId;
			if (!flag)
			{
				CombatCharacter enemyChar = DomainManager.Combat.GetCombatCharacter(!isAlly, false);
				bool flag2 = enemyChar.TeammateBeforeMainChar >= 0 || enemyChar.TeammateAfterMainChar >= 0 || !DomainManager.Combat.IsMainCharacter(enemyChar);
				if (flag2)
				{
					DomainManager.Combat.ForceAllTeammateLeaveCombatField(context, !isAlly);
					base.ShowSpecialEffectTips(0);
				}
			}
		}

		// Token: 0x06003F58 RID: 16216 RVA: 0x00259704 File Offset: 0x00257904
		public override long GetModifiedValue(AffectedDataKey dataKey, long dataValue)
		{
			EDamageType damageType = (EDamageType)dataKey.CustomParam0;
			bool flag = damageType != EDamageType.Bounce || dataKey.CustomParam1 != (base.IsDirect ? 0 : 1) || !base.CanAffect;
			long result;
			if (flag)
			{
				result = dataValue;
			}
			else
			{
				List<int> enemyList = ObjectPool<List<int>>.Instance.Get();
				enemyList.Clear();
				enemyList.AddRange(DomainManager.Combat.GetCharacterList(!base.CombatChar.IsAlly));
				enemyList.RemoveAll((int id) => id < 0);
				DataContext context = DomainManager.Combat.Context;
				sbyte bodyPart = (sbyte)dataKey.CustomParam2;
				int damagePercent = 100 + 160 * (enemyList.Count - 1);
				int damageValue = (int)(dataValue / (long)enemyList.Count * (long)damagePercent / 100L);
				for (int i = 1; i < enemyList.Count; i++)
				{
					CombatCharacter enemyChar = DomainManager.Combat.GetElement_CombatCharacterDict(enemyList[i]);
					DomainManager.Combat.AddInjuryDamageValue(base.CombatChar, enemyChar, bodyPart, base.IsDirect ? damageValue : 0, base.IsDirect ? 0 : damageValue, base.SkillTemplateId, true);
				}
				ObjectPool<List<int>>.Instance.Return(enemyList);
				base.ShowSpecialEffectTips(1);
				result = (long)damageValue;
			}
			return result;
		}

		// Token: 0x06003F59 RID: 16217 RVA: 0x00259860 File Offset: 0x00257A60
		public override bool GetModifiedValue(AffectedDataKey dataKey, bool dataValue)
		{
			bool flag = dataKey.CharId == base.CharacterId || !base.CanAffect || dataKey.FieldId != 271;
			return flag && dataValue;
		}

		// Token: 0x040012AB RID: 4779
		private const short AddDamageUnit = 160;

		// Token: 0x040012AC RID: 4780
		private DataUid _defendSkillUid;
	}
}
