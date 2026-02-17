using System;
using System.Collections.Generic;
using GameData.Combat.Math;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;
using GameData.Utilities;

namespace GameData.Domains.SpecialEffect.CombatSkill.XiangShu.Attack.LongYuFu
{
	// Token: 0x020002FC RID: 764
	public class DanXinSheYaoLong : CombatSkillEffectBase
	{
		// Token: 0x0600339F RID: 13215 RVA: 0x00225DF8 File Offset: 0x00223FF8
		public DanXinSheYaoLong()
		{
		}

		// Token: 0x060033A0 RID: 13216 RVA: 0x00225E02 File Offset: 0x00224002
		public DanXinSheYaoLong(CombatSkillKey skillKey) : base(skillKey, 17125, -1)
		{
		}

		// Token: 0x060033A1 RID: 13217 RVA: 0x00225E14 File Offset: 0x00224014
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

		// Token: 0x060033A2 RID: 13218 RVA: 0x00225ECF File Offset: 0x002240CF
		public override void OnDisable(DataContext context)
		{
			Events.UnRegisterHandler_PrepareSkillBegin(new Events.OnPrepareSkillBegin(this.OnPrepareSkillBegin));
			Events.UnRegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
		}

		// Token: 0x060033A3 RID: 13219 RVA: 0x00225EF8 File Offset: 0x002240F8
		private void OnPrepareSkillBegin(DataContext context, int charId, bool isAlly, short skillId)
		{
			bool flag = charId != base.CharacterId || skillId != base.SkillTemplateId;
			if (!flag)
			{
				DomainManager.Combat.UpdateSkillCanUse(context, DomainManager.Combat.GetCombatCharacter(!base.CombatChar.IsAlly, false));
				CombatCharacter enemyChar = DomainManager.Combat.GetCombatCharacter(!base.CombatChar.IsAlly, false);
				Dictionary<SkillEffectKey, short> effectDict = enemyChar.GetSkillEffectCollection().EffectDict;
				bool flag2 = base.CombatChar.GetInjuries().Get(2, false) < 4 || effectDict == null || effectDict.Count <= 0;
				if (!flag2)
				{
					List<SkillEffectKey> effectRandomPool = ObjectPool<List<SkillEffectKey>>.Instance.Get();
					int affectCount = Math.Min(6, effectDict.Count);
					effectRandomPool.Clear();
					effectRandomPool.AddRange(effectDict.Keys);
					for (int i = 0; i < affectCount; i++)
					{
						int index = context.Random.Next(effectRandomPool.Count);
						SkillEffectKey effectKey = effectRandomPool[index];
						effectRandomPool.RemoveAt(index);
						DomainManager.Combat.ChangeSkillEffectToMinCount(context, enemyChar, effectKey);
						DomainManager.Combat.AddGoneMadInjury(context, enemyChar, effectKey.SkillId, 0);
					}
					ObjectPool<List<SkillEffectKey>>.Instance.Return(effectRandomPool);
					DomainManager.Combat.AddToCheckFallenSet(enemyChar.GetId());
					base.ShowSpecialEffectTips(0);
				}
			}
		}

		// Token: 0x060033A4 RID: 13220 RVA: 0x00226060 File Offset: 0x00224260
		private void OnCastSkillEnd(DataContext context, int charId, bool isAlly, short skillId, sbyte power, bool interrupted)
		{
			bool flag = charId != base.CharacterId || skillId != base.SkillTemplateId;
			if (!flag)
			{
				DomainManager.Combat.UpdateSkillCanUse(context, DomainManager.Combat.GetCombatCharacter(!base.CombatChar.IsAlly, false));
			}
		}

		// Token: 0x060033A5 RID: 13221 RVA: 0x002260B4 File Offset: 0x002242B4
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
				bool flag2 = dataKey.FieldId == 169 && !dataValue && dataKey.CustomParam0 == 2 && (base.CombatChar.GetPreparingSkillId() == base.SkillTemplateId || base.CombatChar.GetPerformingSkillId() == base.SkillTemplateId);
				result = (flag2 || dataValue);
			}
			return result;
		}

		// Token: 0x04000F44 RID: 3908
		private const sbyte RequireInjury = 4;

		// Token: 0x04000F45 RID: 3909
		private const sbyte AffectSkillCount = 6;
	}
}
