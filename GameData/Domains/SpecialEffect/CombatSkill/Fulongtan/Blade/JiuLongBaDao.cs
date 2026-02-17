using System;
using System.Collections.Generic;
using GameData.Combat.Math;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;
using GameData.Utilities;

namespace GameData.Domains.SpecialEffect.CombatSkill.Fulongtan.Blade
{
	// Token: 0x0200052E RID: 1326
	public class JiuLongBaDao : CombatSkillEffectBase
	{
		// Token: 0x06003F73 RID: 16243 RVA: 0x00259EAB File Offset: 0x002580AB
		public JiuLongBaDao()
		{
		}

		// Token: 0x06003F74 RID: 16244 RVA: 0x00259EB5 File Offset: 0x002580B5
		public JiuLongBaDao(CombatSkillKey skillKey) : base(skillKey, 14207, -1)
		{
		}

		// Token: 0x06003F75 RID: 16245 RVA: 0x00259EC8 File Offset: 0x002580C8
		public override void OnEnable(DataContext context)
		{
			this.AffectDatas = new Dictionary<AffectedDataKey, EDataModifyType>();
			this.AffectDatas.Add(new AffectedDataKey(base.CharacterId, 199, base.SkillTemplateId, -1, -1, -1), EDataModifyType.Add);
			this.ChangePower(context);
			Events.RegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
		}

		// Token: 0x06003F76 RID: 16246 RVA: 0x00259F21 File Offset: 0x00258121
		public override void OnDisable(DataContext context)
		{
			Events.UnRegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
		}

		// Token: 0x06003F77 RID: 16247 RVA: 0x00259F38 File Offset: 0x00258138
		private void OnCastSkillEnd(DataContext context, int charId, bool isAlly, short skillId, sbyte power, bool interrupted)
		{
			bool flag = charId != base.CharacterId || skillId != base.SkillTemplateId;
			if (!flag)
			{
				DomainManager.Combat.SilenceSkill(context, base.CombatChar, base.SkillTemplateId, 3000, -1);
				base.ShowSpecialEffectTips(1);
				base.RemoveSelf(context);
			}
		}

		// Token: 0x06003F78 RID: 16248 RVA: 0x00259F94 File Offset: 0x00258194
		private void ChangePower(DataContext context)
		{
			CombatCharacter enemyChar = base.CurrEnemyChar;
			Dictionary<CombatSkillKey, SkillPowerChangeCollection> powerDict = base.IsDirect ? DomainManager.Combat.GetAllSkillPowerAddInCombat() : DomainManager.Combat.GetAllSkillPowerReduceInCombat();
			List<CombatSkillKey> preferredRandomPool = ObjectPool<List<CombatSkillKey>>.Instance.Get();
			List<CombatSkillKey> normalRandomPool = ObjectPool<List<CombatSkillKey>>.Instance.Get();
			int preferredCharId = base.IsDirect ? enemyChar.GetId() : base.CombatChar.GetId();
			foreach (CombatSkillKey skillKey in powerDict.Keys)
			{
				bool flag = skillKey.CharId == preferredCharId;
				if (flag)
				{
					preferredRandomPool.Add(skillKey);
				}
				else
				{
					normalRandomPool.Add(skillKey);
				}
			}
			bool flag2 = preferredRandomPool.Count > 0 || normalRandomPool.Count > 0;
			if (flag2)
			{
				foreach (CombatSkillKey skillKey2 in RandomUtils.GetRandomUnrepeated<CombatSkillKey>(context.Random, 9, preferredRandomPool, normalRandomPool))
				{
					SkillPowerChangeCollection powerChangeCollection = base.IsDirect ? DomainManager.Combat.RemoveSkillPowerAddInCombat(context, skillKey2) : DomainManager.Combat.RemoveSkillPowerReduceInCombat(context, skillKey2);
					bool flag3 = powerChangeCollection == null;
					if (!flag3)
					{
						foreach (int powerChangeValue in powerChangeCollection.EffectDict.Values)
						{
							this._addedPower += Math.Abs(powerChangeValue) * 2;
						}
					}
				}
				DomainManager.SpecialEffect.InvalidateCache(context, base.CharacterId, 199);
				base.ShowSpecialEffectTips(0);
			}
			ObjectPool<List<CombatSkillKey>>.Instance.Return(preferredRandomPool);
			ObjectPool<List<CombatSkillKey>>.Instance.Return(normalRandomPool);
		}

		// Token: 0x06003F79 RID: 16249 RVA: 0x0025A198 File Offset: 0x00258398
		public override int GetModifyValue(AffectedDataKey dataKey, int currModifyValue)
		{
			bool flag = !dataKey.IsMatch(this.SkillKey) || dataKey.FieldId != 199;
			int result;
			if (flag)
			{
				result = 0;
			}
			else
			{
				result = this._addedPower;
			}
			return result;
		}

		// Token: 0x040012B2 RID: 4786
		private const sbyte MaxTransferCount = 9;

		// Token: 0x040012B3 RID: 4787
		private const int SilenceFrame = 3000;

		// Token: 0x040012B4 RID: 4788
		private const int TransferPowerRatio = 2;

		// Token: 0x040012B5 RID: 4789
		private int _addedPower;
	}
}
