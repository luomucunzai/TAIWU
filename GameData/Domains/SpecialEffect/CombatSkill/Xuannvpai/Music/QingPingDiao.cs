using System;
using GameData.Combat.Math;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.CombatSkill;

namespace GameData.Domains.SpecialEffect.CombatSkill.Xuannvpai.Music
{
	// Token: 0x0200026C RID: 620
	public class QingPingDiao : CombatSkillEffectBase
	{
		// Token: 0x0600307E RID: 12414 RVA: 0x002175CB File Offset: 0x002157CB
		public QingPingDiao()
		{
		}

		// Token: 0x0600307F RID: 12415 RVA: 0x002175D5 File Offset: 0x002157D5
		public QingPingDiao(CombatSkillKey skillKey) : base(skillKey, 8301, -1)
		{
		}

		// Token: 0x06003080 RID: 12416 RVA: 0x002175E8 File Offset: 0x002157E8
		public override void OnEnable(DataContext context)
		{
			this._prevMatchAffect = ((base.IsDirect ? ((int)base.CurrEnemyChar.GetTrickCount(20)) : base.CurrEnemyChar.GetDefeatMarkCollection().MindMarkList.Count) == 0);
			bool prevMatchAffect = this._prevMatchAffect;
			if (prevMatchAffect)
			{
				base.CreateAffectedData(199, EDataModifyType.AddPercent, base.SkillTemplateId);
				base.ShowSpecialEffectTips(0);
			}
			Events.RegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
		}

		// Token: 0x06003081 RID: 12417 RVA: 0x00217664 File Offset: 0x00215864
		public override void OnDisable(DataContext context)
		{
			Events.UnRegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
		}

		// Token: 0x06003082 RID: 12418 RVA: 0x0021767C File Offset: 0x0021587C
		private void OnCastSkillEnd(DataContext context, int charId, bool isAlly, short skillId, sbyte power, bool interrupted)
		{
			bool flag = charId != base.CharacterId || skillId != base.SkillTemplateId;
			if (!flag)
			{
				bool flag2 = base.PowerMatchAffectRequire((int)power, 0);
				if (flag2)
				{
					int addCount = this._prevMatchAffect ? 4 : 1;
					bool isDirect = base.IsDirect;
					if (isDirect)
					{
						DomainManager.Combat.AddTrick(context, base.CurrEnemyChar, 20, addCount, false, false);
					}
					else
					{
						DomainManager.Combat.AppendMindDefeatMark(context, base.CurrEnemyChar, addCount, -1, false);
					}
					base.ShowSpecialEffectTips(1);
				}
				base.RemoveSelf(context);
			}
		}

		// Token: 0x06003083 RID: 12419 RVA: 0x00217710 File Offset: 0x00215910
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

		// Token: 0x04000E64 RID: 3684
		private const sbyte AddPower = 40;

		// Token: 0x04000E65 RID: 3685
		private const int ExtraCount = 3;

		// Token: 0x04000E66 RID: 3686
		private bool _prevMatchAffect;
	}
}
