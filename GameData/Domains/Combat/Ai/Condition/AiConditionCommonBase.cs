using System;
using System.Runtime.CompilerServices;

namespace GameData.Domains.Combat.Ai.Condition
{
	// Token: 0x02000733 RID: 1843
	public abstract class AiConditionCommonBase : IAiCondition
	{
		// Token: 0x1700044B RID: 1099
		// (get) Token: 0x060068FF RID: 26879 RVA: 0x003B9234 File Offset: 0x003B7434
		public int GroupId
		{
			get
			{
				return 0;
			}
		}

		// Token: 0x1700044C RID: 1100
		// (get) Token: 0x06006900 RID: 26880 RVA: 0x003B9237 File Offset: 0x003B7437
		// (set) Token: 0x06006901 RID: 26881 RVA: 0x003B923F File Offset: 0x003B743F
		private protected string RuntimeIdStr { protected get; private set; }

		// Token: 0x1700044D RID: 1101
		// (get) Token: 0x06006902 RID: 26882 RVA: 0x003B9248 File Offset: 0x003B7448
		// (set) Token: 0x06006903 RID: 26883 RVA: 0x003B9250 File Offset: 0x003B7450
		public int RuntimeId
		{
			get
			{
				return this._runtimeId;
			}
			set
			{
				this._runtimeId = value;
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(19, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Conchship_Internal_");
				defaultInterpolatedStringHandler.AppendFormatted<int>(this._runtimeId);
				this.RuntimeIdStr = defaultInterpolatedStringHandler.ToStringAndClear();
			}
		}

		// Token: 0x06006904 RID: 26884
		public abstract bool Check(AiMemoryNew memory, IAiParticipant participant);

		// Token: 0x04001CE2 RID: 7394
		private int _runtimeId;
	}
}
