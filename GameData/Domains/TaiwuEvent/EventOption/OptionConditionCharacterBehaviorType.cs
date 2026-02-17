using System;
using System.Runtime.CompilerServices;
using GameData.Domains.Character;

namespace GameData.Domains.TaiwuEvent.EventOption
{
	// Token: 0x020000B9 RID: 185
	public class OptionConditionCharacterBehaviorType : TaiwuEventOptionConditionBase
	{
		// Token: 0x06001BB8 RID: 7096 RVA: 0x0017D940 File Offset: 0x0017BB40
		public OptionConditionCharacterBehaviorType(short id, sbyte type1, sbyte type2, Func<Character, sbyte, sbyte, bool> checker) : base(id)
		{
			this.ConditionChecker = checker;
			this.BehaviorType1 = type1;
			this.BehaviorType2 = type2;
		}

		// Token: 0x06001BB9 RID: 7097 RVA: 0x0017D964 File Offset: 0x0017BB64
		public override bool CheckCondition(EventArgBox box)
		{
			bool flag = box == null;
			return !flag && this.ConditionChecker(box.GetCharacter("CharacterId"), this.BehaviorType1, this.BehaviorType2);
		}

		// Token: 0x06001BBA RID: 7098 RVA: 0x0017D9A4 File Offset: 0x0017BBA4
		public override ValueTuple<short, string[]> GetDisplayData(EventArgBox box)
		{
			int charId = box.GetInt("CharacterId");
			ValueTuple<string, string> nameTuple = DomainManager.Character.GetNameRelatedData(charId).GetDisplayName(charId == DomainManager.Taiwu.GetTaiwuCharId());
			short id = this.Id;
			string[] array = new string[3];
			array[0] = nameTuple.Item1 + nameTuple.Item2;
			int num = 1;
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(28, 1);
			defaultInterpolatedStringHandler.AppendLiteral("<Language Key=LK_Goodness_");
			defaultInterpolatedStringHandler.AppendFormatted<sbyte>(this.BehaviorType1);
			defaultInterpolatedStringHandler.AppendLiteral("/>");
			array[num] = defaultInterpolatedStringHandler.ToStringAndClear();
			int num2 = 2;
			defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(28, 1);
			defaultInterpolatedStringHandler.AppendLiteral("<Language Key=LK_Goodness_");
			defaultInterpolatedStringHandler.AppendFormatted<sbyte>(this.BehaviorType2);
			defaultInterpolatedStringHandler.AppendLiteral("/>");
			array[num2] = defaultInterpolatedStringHandler.ToStringAndClear();
			return new ValueTuple<short, string[]>(id, array);
		}

		// Token: 0x04000665 RID: 1637
		public readonly Func<Character, sbyte, sbyte, bool> ConditionChecker;

		// Token: 0x04000666 RID: 1638
		public sbyte BehaviorType1;

		// Token: 0x04000667 RID: 1639
		public sbyte BehaviorType2;
	}
}
