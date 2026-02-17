using System;
using System.Collections.Generic;
using GameData.Combat.Math;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Character;
using GameData.Domains.CombatSkill;
using GameData.Utilities;

namespace GameData.Domains.SpecialEffect.CombatSkill.Fulongtan.Throw
{
	// Token: 0x0200050D RID: 1293
	public class FenXueGong : CombatSkillEffectBase
	{
		// Token: 0x06003ED0 RID: 16080 RVA: 0x0025737B File Offset: 0x0025557B
		public FenXueGong()
		{
		}

		// Token: 0x06003ED1 RID: 16081 RVA: 0x00257385 File Offset: 0x00255585
		public FenXueGong(CombatSkillKey skillKey) : base(skillKey, 14304, -1)
		{
		}

		// Token: 0x06003ED2 RID: 16082 RVA: 0x00257396 File Offset: 0x00255596
		public override void OnEnable(DataContext context)
		{
			Events.RegisterHandler_PrepareSkillBegin(new Events.OnPrepareSkillBegin(this.OnPrepareSkillBegin));
			Events.RegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
		}

		// Token: 0x06003ED3 RID: 16083 RVA: 0x002573BD File Offset: 0x002555BD
		public override void OnDisable(DataContext context)
		{
			Events.UnRegisterHandler_PrepareSkillBegin(new Events.OnPrepareSkillBegin(this.OnPrepareSkillBegin));
			Events.UnRegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
		}

		// Token: 0x06003ED4 RID: 16084 RVA: 0x002573E4 File Offset: 0x002555E4
		private void OnPrepareSkillBegin(DataContext context, int charId, bool isAlly, short skillId)
		{
			bool flag = charId != base.CharacterId || skillId != base.SkillTemplateId;
			if (!flag)
			{
				Injuries injuries = base.CombatChar.GetInjuries();
				Injuries oldInjuries = base.CombatChar.GetOldInjuries();
				List<sbyte> partRandomPool = ObjectPool<List<sbyte>>.Instance.Get();
				partRandomPool.Clear();
				for (sbyte part = 0; part < 7; part += 1)
				{
					int newInjury = (int)(injuries.Get(part, !base.IsDirect) - oldInjuries.Get(part, !base.IsDirect));
					bool flag2 = newInjury <= 0;
					if (!flag2)
					{
						for (int i = 0; i < newInjury; i++)
						{
							partRandomPool.Add(part);
						}
					}
				}
				bool flag3 = partRandomPool.Count > 0;
				if (flag3)
				{
					int oldInjuryCount = Math.Min(partRandomPool.Count, 1);
					int newInjuryCount = Math.Min(partRandomPool.Count, 3);
					CollectionUtils.Shuffle<sbyte>(context.Random, partRandomPool);
					for (int j = 0; j < oldInjuryCount; j++)
					{
						sbyte part2 = partRandomPool[j];
						DomainManager.Combat.ChangeToOldInjury(context, base.CombatChar, part2, !base.IsDirect, 1);
					}
					for (int k = oldInjuryCount; k < newInjuryCount; k++)
					{
						sbyte part3 = partRandomPool[k];
						DomainManager.Combat.RemoveInjury(context, base.CombatChar, part3, !base.IsDirect, 1, true, false);
					}
					base.AppendAffectedData(context, base.CharacterId, 199, EDataModifyType.AddPercent, base.SkillTemplateId);
					base.AppendAffectedData(context, base.CharacterId, 145, EDataModifyType.Add, base.SkillTemplateId);
					base.AppendAffectedData(context, base.CharacterId, 146, EDataModifyType.Add, base.SkillTemplateId);
					DomainManager.SpecialEffect.InvalidateCache(context, base.CharacterId, 199);
					DomainManager.SpecialEffect.InvalidateCache(context, base.CharacterId, 145);
					DomainManager.SpecialEffect.InvalidateCache(context, base.CharacterId, 146);
					base.ShowSpecialEffectTips(0);
				}
				ObjectPool<List<sbyte>>.Instance.Return(partRandomPool);
			}
		}

		// Token: 0x06003ED5 RID: 16085 RVA: 0x00257618 File Offset: 0x00255818
		private void OnCastSkillEnd(DataContext context, int charId, bool isAlly, short skillId, sbyte power, bool interrupted)
		{
			bool flag = charId != base.CharacterId || skillId != base.SkillTemplateId;
			if (!flag)
			{
				base.RemoveSelf(context);
			}
		}

		// Token: 0x06003ED6 RID: 16086 RVA: 0x00257650 File Offset: 0x00255850
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
				bool flag2 = dataKey.FieldId == 145 || dataKey.FieldId == 146;
				if (flag2)
				{
					result = 20;
				}
				else
				{
					bool flag3 = dataKey.FieldId == 199;
					if (flag3)
					{
						result = 60;
					}
					else
					{
						result = 0;
					}
				}
			}
			return result;
		}

		// Token: 0x04001289 RID: 4745
		private const int ChangeNewInjuryCount = 3;

		// Token: 0x0400128A RID: 4746
		private const int ChangeOldInjuryCount = 1;

		// Token: 0x0400128B RID: 4747
		private const sbyte AddPower = 60;

		// Token: 0x0400128C RID: 4748
		private const sbyte AddAttackRange = 20;
	}
}
