using System;
using System.Collections.Generic;
using GameData.Combat.Math;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Character;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;
using GameData.Utilities;

namespace GameData.Domains.SpecialEffect.CombatSkill.XiangShu.Attack.LongYuFu
{
	// Token: 0x020002FA RID: 762
	public class ChiYiXiangENi : CombatSkillEffectBase
	{
		// Token: 0x06003392 RID: 13202 RVA: 0x00225899 File Offset: 0x00223A99
		public ChiYiXiangENi()
		{
		}

		// Token: 0x06003393 RID: 13203 RVA: 0x002258A3 File Offset: 0x00223AA3
		public ChiYiXiangENi(CombatSkillKey skillKey) : base(skillKey, 17124, -1)
		{
		}

		// Token: 0x06003394 RID: 13204 RVA: 0x002258B4 File Offset: 0x00223AB4
		public override void OnEnable(DataContext context)
		{
			int[] enemyList = DomainManager.Combat.GetCharacterList(!base.CombatChar.IsAlly);
			this.AffectDatas = new Dictionary<AffectedDataKey, EDataModifyType>();
			this.AffectDatas.Add(new AffectedDataKey(base.CharacterId, 219, base.SkillTemplateId, -1, -1, -1), EDataModifyType.Custom);
			for (int i = 0; i < enemyList.Length; i++)
			{
				bool flag = enemyList[i] >= 0;
				if (flag)
				{
					this.AffectDatas.Add(new AffectedDataKey(enemyList[i], 169, -1, -1, -1, -1), EDataModifyType.Custom);
				}
			}
			Events.RegisterHandler_PrepareSkillBegin(new Events.OnPrepareSkillBegin(this.OnPrepareSkillBegin));
			Events.RegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
		}

		// Token: 0x06003395 RID: 13205 RVA: 0x0022596F File Offset: 0x00223B6F
		public override void OnDisable(DataContext context)
		{
			Events.UnRegisterHandler_PrepareSkillBegin(new Events.OnPrepareSkillBegin(this.OnPrepareSkillBegin));
			Events.UnRegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
		}

		// Token: 0x06003396 RID: 13206 RVA: 0x00225998 File Offset: 0x00223B98
		private void OnPrepareSkillBegin(DataContext context, int charId, bool isAlly, short skillId)
		{
			bool flag = charId != base.CharacterId || skillId != base.SkillTemplateId;
			if (!flag)
			{
				DomainManager.Combat.UpdateSkillCanUse(context, DomainManager.Combat.GetCombatCharacter(!base.CombatChar.IsAlly, false));
				Injuries injuries = base.CombatChar.GetInjuries();
				bool canAffect = injuries.Get(5, false) >= 4 || injuries.Get(6, false) >= 4;
				bool flag2 = !canAffect;
				if (!flag2)
				{
					int enemyCharId = base.CurrEnemyChar.GetId();
					Dictionary<CombatSkillKey, SkillPowerChangeCollection> powerDict = DomainManager.Combat.GetAllSkillPowerAddInCombat();
					List<CombatSkillKey> srcSkillList = ObjectPool<List<CombatSkillKey>>.Instance.Get();
					srcSkillList.Clear();
					foreach (CombatSkillKey skillKey in powerDict.Keys)
					{
						bool flag3 = skillKey.CharId == enemyCharId;
						if (flag3)
						{
							srcSkillList.Add(skillKey);
						}
					}
					bool flag4 = srcSkillList.Count > 0;
					if (flag4)
					{
						for (int i = 0; i < srcSkillList.Count; i++)
						{
							CombatSkillKey srcKey = srcSkillList[i];
							SkillPowerChangeCollection powerAddCollection = DomainManager.Combat.RemoveSkillPowerAddInCombat(context, srcKey);
							bool flag5 = powerAddCollection != null;
							if (flag5)
							{
								DomainManager.Combat.AddSkillPowerInCombat(context, this.SkillKey, new SkillEffectKey(base.SkillTemplateId, true), powerAddCollection.GetTotalChangeValue());
							}
						}
						base.ShowSpecialEffectTips(0);
					}
					ObjectPool<List<CombatSkillKey>>.Instance.Return(srcSkillList);
				}
			}
		}

		// Token: 0x06003397 RID: 13207 RVA: 0x00225B44 File Offset: 0x00223D44
		private void OnCastSkillEnd(DataContext context, int charId, bool isAlly, short skillId, sbyte power, bool interrupted)
		{
			bool flag = charId != base.CharacterId || skillId != base.SkillTemplateId;
			if (!flag)
			{
				DomainManager.Combat.UpdateSkillCanUse(context, DomainManager.Combat.GetCombatCharacter(!base.CombatChar.IsAlly, false));
			}
		}

		// Token: 0x06003398 RID: 13208 RVA: 0x00225B98 File Offset: 0x00223D98
		public override bool GetModifiedValue(AffectedDataKey dataKey, bool dataValue)
		{
			bool flag = dataKey.FieldId == 219;
			bool result;
			if (flag)
			{
				result = true;
			}
			else
			{
				bool flag2 = dataKey.FieldId == 169 && !dataValue && (dataKey.CustomParam0 == 5 || dataKey.CustomParam0 == 6) && (base.CombatChar.GetPreparingSkillId() == base.SkillTemplateId || base.CombatChar.GetPerformingSkillId() == base.SkillTemplateId);
				result = (flag2 || dataValue);
			}
			return result;
		}

		// Token: 0x04000F3F RID: 3903
		private const sbyte RequireInjury = 4;
	}
}
