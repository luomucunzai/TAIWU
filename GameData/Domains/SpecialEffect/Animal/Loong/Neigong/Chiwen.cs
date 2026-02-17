using System;
using System.Collections.Generic;
using GameData.Combat.Math;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Character;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;
using GameData.Utilities;

namespace GameData.Domains.SpecialEffect.Animal.Loong.Neigong
{
	// Token: 0x02000600 RID: 1536
	public class Chiwen : AnimalEffectBase
	{
		// Token: 0x06004509 RID: 17673 RVA: 0x0027126F File Offset: 0x0026F46F
		public Chiwen()
		{
		}

		// Token: 0x0600450A RID: 17674 RVA: 0x00271279 File Offset: 0x0026F479
		public Chiwen(CombatSkillKey skillKey) : base(skillKey)
		{
		}

		// Token: 0x0600450B RID: 17675 RVA: 0x00271284 File Offset: 0x0026F484
		public override void OnEnable(DataContext context)
		{
			base.OnEnable(context);
			base.CreateAffectedData(69, EDataModifyType.AddPercent, -1);
			Events.RegisterHandler_NormalAttackCalcHitEnd(new Events.OnNormalAttackCalcHitEnd(this.OnNormalAttackCalcHitEnd));
			Events.RegisterHandler_NormalAttackAllEnd(new Events.OnNormalAttackAllEnd(this.OnNormalAttackAllEnd));
		}

		// Token: 0x0600450C RID: 17676 RVA: 0x002712BE File Offset: 0x0026F4BE
		public override void OnDisable(DataContext context)
		{
			Events.UnRegisterHandler_NormalAttackCalcHitEnd(new Events.OnNormalAttackCalcHitEnd(this.OnNormalAttackCalcHitEnd));
			Events.UnRegisterHandler_NormalAttackAllEnd(new Events.OnNormalAttackAllEnd(this.OnNormalAttackAllEnd));
			base.OnDisable(context);
		}

		// Token: 0x0600450D RID: 17677 RVA: 0x002712F0 File Offset: 0x0026F4F0
		private void OnNormalAttackCalcHitEnd(DataContext context, CombatCharacter attacker, CombatCharacter defender, int pursueIndex, bool hit, bool isFightback, bool isMind)
		{
			bool flag = attacker.GetId() != base.CharacterId || !hit || pursueIndex > 0;
			if (!flag)
			{
				this.TryChangeNeiliAllocation(context, attacker, true);
				this.TryChangeNeiliAllocation(context, defender, false);
			}
		}

		// Token: 0x0600450E RID: 17678 RVA: 0x00271334 File Offset: 0x0026F534
		private void OnNormalAttackAllEnd(DataContext context, CombatCharacter attacker, CombatCharacter defender)
		{
			bool flag = attacker.GetId() != base.CharacterId;
			if (!flag)
			{
				this._damageAddPercent = 0;
			}
		}

		// Token: 0x0600450F RID: 17679 RVA: 0x00271360 File Offset: 0x0026F560
		private unsafe void TryChangeNeiliAllocation(DataContext context, CombatCharacter targetChar, bool buff)
		{
			NeiliAllocation neiliAllocation = targetChar.GetNeiliAllocation();
			NeiliAllocation originNeiliAllocation = targetChar.GetOriginNeiliAllocation();
			List<byte> neiliAllocationTypes = ObjectPool<List<byte>>.Instance.Get();
			for (byte i = 0; i < 4; i += 1)
			{
				bool flag = buff ? (*neiliAllocation[(int)i] >= *originNeiliAllocation[(int)i]) : (*neiliAllocation[(int)i] <= *originNeiliAllocation[(int)i]);
				if (!flag)
				{
					neiliAllocationTypes.Add(i);
				}
			}
			bool flag2 = neiliAllocationTypes.Count > 0;
			if (flag2)
			{
				byte type = neiliAllocationTypes.GetRandom(context.Random);
				int changeValue = (3 + (int)(*neiliAllocation[(int)type]) * Chiwen.AddOrCostPercent) * (buff ? 1 : -1);
				changeValue = targetChar.ChangeNeiliAllocation(context, type, changeValue, true, false);
				this._damageAddPercent += Math.Abs(changeValue);
				base.ShowSpecialEffectTips(buff, 1, 0);
			}
			ObjectPool<List<byte>>.Instance.Return(neiliAllocationTypes);
		}

		// Token: 0x06004510 RID: 17680 RVA: 0x00271458 File Offset: 0x0026F658
		public override int GetModifyValue(AffectedDataKey dataKey, int currModifyValue)
		{
			bool flag = dataKey.CharId != base.CharacterId || dataKey.FieldId != 69 || this._damageAddPercent <= 0;
			int result;
			if (flag)
			{
				result = 0;
			}
			else
			{
				base.ShowSpecialEffectTipsOnceInFrame(2);
				result = this._damageAddPercent;
			}
			return result;
		}

		// Token: 0x04001468 RID: 5224
		private const int AddOrCostBaseValue = 3;

		// Token: 0x04001469 RID: 5225
		private static readonly CValuePercent AddOrCostPercent = 25;

		// Token: 0x0400146A RID: 5226
		private int _damageAddPercent;
	}
}
