using System;
using System.Collections.Generic;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Character;
using GameData.Domains.Combat;
using GameData.Utilities;

namespace GameData.Domains.SpecialEffect.SectStory.Shaolin
{
	// Token: 0x020000ED RID: 237
	public class AddRandomInjury : DemonSlayerTrialEffectBase
	{
		// Token: 0x06002973 RID: 10611 RVA: 0x00200C18 File Offset: 0x001FEE18
		public AddRandomInjury(int charId, IReadOnlyList<int> parameters) : base(charId)
		{
			this._bodyPartCount = parameters[0];
			this._outerInjuryLevel = parameters[1];
			this._innerInjuryLevel = parameters[2];
		}

		// Token: 0x06002974 RID: 10612 RVA: 0x00200C4A File Offset: 0x001FEE4A
		public override void OnEnable(DataContext context)
		{
			base.OnEnable(context);
			Events.RegisterHandler_CombatBegin(new Events.OnCombatBegin(this.OnCombatBegin));
		}

		// Token: 0x06002975 RID: 10613 RVA: 0x00200C67 File Offset: 0x001FEE67
		public override void OnDisable(DataContext context)
		{
			Events.UnRegisterHandler_CombatBegin(new Events.OnCombatBegin(this.OnCombatBegin));
			base.OnDisable(context);
		}

		// Token: 0x06002976 RID: 10614 RVA: 0x00200C84 File Offset: 0x001FEE84
		private void OnCombatBegin(DataContext context)
		{
			List<sbyte> bodyParts = ObjectPool<List<sbyte>>.Instance.Get();
			bodyParts.Clear();
			for (sbyte i = 0; i < 7; i += 1)
			{
				bodyParts.Add(i);
			}
			Injuries injuries = base.CombatChar.GetInjuries();
			foreach (sbyte bodyPart in RandomUtils.GetRandomUnrepeated<sbyte>(context.Random, this._bodyPartCount, bodyParts, null))
			{
				ValueTuple<sbyte, sbyte> valueTuple = injuries.Get(bodyPart);
				sbyte outer = valueTuple.Item1;
				sbyte inner = valueTuple.Item2;
				sbyte addOuter = (sbyte)Math.Min((int)(6 - outer), this._outerInjuryLevel);
				sbyte addInner = (sbyte)Math.Min((int)(6 - inner), this._innerInjuryLevel);
				int addFatal = this._outerInjuryLevel + this._innerInjuryLevel - (int)addOuter - (int)addInner;
				bool flag = addOuter > 0;
				if (flag)
				{
					DomainManager.Combat.AddInjury(context, base.CombatChar, bodyPart, false, addOuter, false, false);
				}
				bool flag2 = addInner > 0;
				if (flag2)
				{
					DomainManager.Combat.AddInjury(context, base.CombatChar, bodyPart, true, addInner, false, false);
				}
				bool flag3 = addFatal > 0;
				if (flag3)
				{
					DomainManager.Combat.AppendFatalDamageMark(context, base.CombatChar, addFatal, -1, -1, false, EDamageType.None);
				}
			}
			DomainManager.Combat.UpdateBodyDefeatMark(context, base.CombatChar);
			ObjectPool<List<sbyte>>.Instance.Return(bodyParts);
		}

		// Token: 0x04000CC8 RID: 3272
		private readonly int _bodyPartCount;

		// Token: 0x04000CC9 RID: 3273
		private readonly int _outerInjuryLevel;

		// Token: 0x04000CCA RID: 3274
		private readonly int _innerInjuryLevel;
	}
}
