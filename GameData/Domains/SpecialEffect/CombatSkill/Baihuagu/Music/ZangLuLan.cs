using System;
using System.Collections.Generic;
using GameData.Combat.Math;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.CombatSkill;
using GameData.Utilities;
using Redzen.Random;

namespace GameData.Domains.SpecialEffect.CombatSkill.Baihuagu.Music
{
	// Token: 0x020005CB RID: 1483
	public class ZangLuLan : CombatSkillEffectBase
	{
		// Token: 0x060043EA RID: 17386 RVA: 0x0026D4F7 File Offset: 0x0026B6F7
		private static int GetRandomCount(IRandomSource random)
		{
			return RandomUtils.GetRandomResult<int>(ZangLuLan.CountWeights, random);
		}

		// Token: 0x060043EB RID: 17387 RVA: 0x0026D504 File Offset: 0x0026B704
		public ZangLuLan()
		{
		}

		// Token: 0x060043EC RID: 17388 RVA: 0x0026D50E File Offset: 0x0026B70E
		public ZangLuLan(CombatSkillKey skillKey) : base(skillKey, 3305, -1)
		{
		}

		// Token: 0x060043ED RID: 17389 RVA: 0x0026D520 File Offset: 0x0026B720
		public override void OnEnable(DataContext context)
		{
			this._addPower = (base.IsDirect ? ((int)(10 * (base.CombatChar.GetTrickCount(20) + base.EnemyChar.GetTrickCount(20)))) : (5 * (base.CombatChar.GetDefeatMarkCollection().MindMarkList.Count + base.EnemyChar.GetDefeatMarkCollection().MindMarkList.Count)));
			bool flag = this._addPower > 0;
			if (flag)
			{
				this.AffectDatas = new Dictionary<AffectedDataKey, EDataModifyType>();
				this.AffectDatas.Add(new AffectedDataKey(base.CharacterId, 199, base.SkillTemplateId, -1, -1, -1), EDataModifyType.AddPercent);
				base.ShowSpecialEffectTips(0);
			}
			Events.RegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
		}

		// Token: 0x060043EE RID: 17390 RVA: 0x0026D5E3 File Offset: 0x0026B7E3
		public override void OnDisable(DataContext context)
		{
			Events.UnRegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
		}

		// Token: 0x060043EF RID: 17391 RVA: 0x0026D5F8 File Offset: 0x0026B7F8
		private void OnCastSkillEnd(DataContext context, int charId, bool isAlly, short skillId, sbyte power, bool interrupted)
		{
			bool flag = charId != base.CharacterId || skillId != base.SkillTemplateId;
			if (!flag)
			{
				bool flag2 = base.PowerMatchAffectRequire((int)power, 0);
				if (flag2)
				{
					bool removeAffected = base.IsDirect ? (base.CombatChar.GetTrickCount(20) > 0) : (base.CombatChar.GetDefeatMarkCollection().MindMarkList.Count > 0);
					bool isDirect = base.IsDirect;
					if (isDirect)
					{
						bool flag3 = removeAffected;
						if (flag3)
						{
							DomainManager.Combat.RemoveTrick(context, base.CombatChar, 20, (byte)Math.Min(ZangLuLan.GetRandomCount(context.Random), (int)base.CombatChar.GetTrickCount(20)), true, -1);
						}
						DomainManager.Combat.AddTrick(context, base.EnemyChar, 20, ZangLuLan.GetRandomCount(context.Random), false, false);
					}
					else
					{
						bool flag4 = removeAffected;
						if (flag4)
						{
							DomainManager.Combat.RemoveMindDefeatMark(context, base.CombatChar, Math.Min(ZangLuLan.GetRandomCount(context.Random), base.CombatChar.GetDefeatMarkCollection().MindMarkList.Count), true, 0);
						}
						DomainManager.Combat.AppendMindDefeatMark(context, base.EnemyChar, ZangLuLan.GetRandomCount(context.Random), -1, false);
					}
					bool flag5 = removeAffected;
					if (flag5)
					{
						base.ShowSpecialEffectTips(1);
					}
					base.ShowSpecialEffectTips(2);
				}
				base.RemoveSelf(context);
			}
		}

		// Token: 0x060043F0 RID: 17392 RVA: 0x0026D758 File Offset: 0x0026B958
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

		// Token: 0x0400142A RID: 5162
		private const sbyte DirectAddPowerUnit = 10;

		// Token: 0x0400142B RID: 5163
		private const sbyte ReverseAddPowerUnit = 5;

		// Token: 0x0400142C RID: 5164
		private static readonly List<ValueTuple<int, short>> CountWeights = new List<ValueTuple<int, short>>
		{
			new ValueTuple<int, short>(2, 25),
			new ValueTuple<int, short>(3, 50),
			new ValueTuple<int, short>(4, 25)
		};

		// Token: 0x0400142D RID: 5165
		private int _addPower;
	}
}
