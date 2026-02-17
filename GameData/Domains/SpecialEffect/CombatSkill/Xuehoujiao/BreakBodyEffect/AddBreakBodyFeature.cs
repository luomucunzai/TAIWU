using System;
using System.Collections.Generic;
using GameData.Combat.Math;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Character;
using GameData.Domains.CombatSkill;
using GameData.Utilities;

namespace GameData.Domains.SpecialEffect.CombatSkill.Xuehoujiao.BreakBodyEffect
{
	// Token: 0x02000248 RID: 584
	public class AddBreakBodyFeature : CombatSkillEffectBase
	{
		// Token: 0x06002FF5 RID: 12277 RVA: 0x002154D1 File Offset: 0x002136D1
		protected AddBreakBodyFeature()
		{
		}

		// Token: 0x06002FF6 RID: 12278 RVA: 0x002154DB File Offset: 0x002136DB
		protected AddBreakBodyFeature(CombatSkillKey skillKey, int type) : base(skillKey, type, -1)
		{
		}

		// Token: 0x06002FF7 RID: 12279 RVA: 0x002154E8 File Offset: 0x002136E8
		public override void OnEnable(DataContext context)
		{
			List<short> featureIds = base.CurrEnemyChar.GetCharacter().GetFeatureIds();
			foreach (short featureId in featureIds)
			{
				bool flag = (base.IsDirect ? BreakFeatureHelper.AllCrashFeature : BreakFeatureHelper.AllHurtFeature).Exist(featureId);
				if (flag)
				{
					this._addPower += 20;
				}
			}
			bool flag2 = this._addPower > 0;
			if (flag2)
			{
				this.AffectDatas = new Dictionary<AffectedDataKey, EDataModifyType>();
				this.AffectDatas.Add(new AffectedDataKey(base.CharacterId, 199, base.SkillTemplateId, -1, -1, -1), EDataModifyType.AddPercent);
				base.ShowSpecialEffectTips(0);
			}
			Events.RegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
		}

		// Token: 0x06002FF8 RID: 12280 RVA: 0x002155D0 File Offset: 0x002137D0
		public override void OnDisable(DataContext context)
		{
			Events.UnRegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
		}

		// Token: 0x06002FF9 RID: 12281 RVA: 0x002155E8 File Offset: 0x002137E8
		private void OnCastSkillEnd(DataContext context, int charId, bool isAlly, short skillId, sbyte power, bool interrupted)
		{
			bool flag = charId != base.CharacterId || skillId != base.SkillTemplateId;
			if (!flag)
			{
				Character enemyChar = base.CurrEnemyChar.GetCharacter();
				short featureId = (base.IsDirect ? BreakFeatureHelper.BodyPart2CrashFeature : BreakFeatureHelper.BodyPart2HurtFeature)[this.AffectBodyParts[0]];
				Injuries injuries = enemyChar.GetInjuries();
				bool hasAnyInjury = false;
				for (int i = 0; i < this.AffectBodyParts.Length; i++)
				{
					bool flag2 = injuries.Get(this.AffectBodyParts[i], !base.IsDirect) > 0;
					if (flag2)
					{
						hasAnyInjury = true;
						break;
					}
				}
				bool flag3 = base.PowerMatchAffectRequire((int)power, 0) && hasAnyInjury && !enemyChar.GetFeatureIds().Contains(featureId);
				if (flag3)
				{
					enemyChar.AddFeature(context, featureId, false);
					DomainManager.SpecialEffect.Add(context, enemyChar.GetId(), SpecialEffectDomain.BreakBodyFeatureEffectClassName[featureId]);
					base.ShowSpecialEffectTips(1);
				}
				base.RemoveSelf(context);
			}
		}

		// Token: 0x06002FFA RID: 12282 RVA: 0x002156F8 File Offset: 0x002138F8
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
					result = this._addPower;
				}
				else
				{
					result = 0;
				}
			}
			return result;
		}

		// Token: 0x04000E3A RID: 3642
		private const sbyte AddPowerUnit = 20;

		// Token: 0x04000E3B RID: 3643
		protected sbyte[] AffectBodyParts;

		// Token: 0x04000E3C RID: 3644
		private int _addPower;
	}
}
