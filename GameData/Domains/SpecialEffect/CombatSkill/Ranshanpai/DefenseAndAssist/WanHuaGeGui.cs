using System;
using System.Collections.Generic;
using GameData.Combat.Math;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Character;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Defense;
using GameData.Utilities;

namespace GameData.Domains.SpecialEffect.CombatSkill.Ranshanpai.DefenseAndAssist
{
	// Token: 0x02000466 RID: 1126
	public class WanHuaGeGui : DefenseSkillBase
	{
		// Token: 0x06003B08 RID: 15112 RVA: 0x00246469 File Offset: 0x00244669
		public WanHuaGeGui()
		{
		}

		// Token: 0x06003B09 RID: 15113 RVA: 0x00246473 File Offset: 0x00244673
		public WanHuaGeGui(CombatSkillKey skillKey) : base(skillKey, 7507)
		{
		}

		// Token: 0x06003B0A RID: 15114 RVA: 0x00246483 File Offset: 0x00244683
		public override void OnEnable(DataContext context)
		{
			base.OnEnable(context);
			base.CreateAffectedData(291, EDataModifyType.Custom, -1);
			Events.RegisterHandler_NormalAttackEnd(new Events.OnNormalAttackEnd(this.OnNormalAttackEnd));
		}

		// Token: 0x06003B0B RID: 15115 RVA: 0x002464AE File Offset: 0x002446AE
		public override void OnDisable(DataContext context)
		{
			Events.UnRegisterHandler_NormalAttackEnd(new Events.OnNormalAttackEnd(this.OnNormalAttackEnd));
			DomainManager.Combat.SilenceSkill(context, base.CombatChar, base.SkillTemplateId, 3000, -1);
			base.OnDisable(context);
		}

		// Token: 0x06003B0C RID: 15116 RVA: 0x002464EC File Offset: 0x002446EC
		private unsafe void OnNormalAttackEnd(DataContext context, CombatCharacter attacker, CombatCharacter defender, sbyte trickType, int pursueIndex, bool hit, bool isFightBack)
		{
			bool flag = !isFightBack || !hit || attacker != base.CombatChar || !base.CanAffect;
			if (!flag)
			{
				CombatCharacter affectChar = base.IsDirect ? attacker : defender;
				NeiliAllocation neiliAllocation = affectChar.GetNeiliAllocation();
				NeiliAllocation originNeiliAllocation = affectChar.GetOriginNeiliAllocation();
				List<byte> pool = ObjectPool<List<byte>>.Instance.Get();
				pool.Clear();
				for (byte i = 0; i < 4; i += 1)
				{
					bool flag2 = base.IsDirect ? (*neiliAllocation[(int)i] >= *originNeiliAllocation[(int)i]) : (*neiliAllocation[(int)i] <= *originNeiliAllocation[(int)i]);
					if (!flag2)
					{
						pool.Add(i);
					}
				}
				bool flag3 = pool.Count > 0;
				if (flag3)
				{
					byte type = pool.GetRandom(context.Random);
					int value = Math.Max(Math.Abs((int)(*originNeiliAllocation[(int)type] - *neiliAllocation[(int)type])) * WanHuaGeGui.ChangeNeiliAllocationPercent, 1) * (base.IsDirect ? 1 : -1);
					affectChar.ChangeNeiliAllocation(context, type, value, true, true);
					base.ShowSpecialEffectTipsOnceInFrame(1);
				}
				ObjectPool<List<byte>>.Instance.Return(pool);
			}
		}

		// Token: 0x06003B0D RID: 15117 RVA: 0x00246630 File Offset: 0x00244830
		public override bool GetModifiedValue(AffectedDataKey dataKey, bool dataValue)
		{
			bool flag = dataKey.CharId != base.CharacterId || dataKey.FieldId != 291 || !base.CanAffect;
			bool result;
			if (flag)
			{
				result = dataValue;
			}
			else
			{
				bool critical = dataKey.CustomParam0 == 1;
				bool flag2 = critical;
				if (flag2)
				{
					result = dataValue;
				}
				else
				{
					base.ShowSpecialEffectTipsOnceInFrame(0);
					result = true;
				}
			}
			return result;
		}

		// Token: 0x0400114D RID: 4429
		private static readonly CValuePercent ChangeNeiliAllocationPercent = 10;

		// Token: 0x0400114E RID: 4430
		private const int SilenceFrame = 3000;
	}
}
