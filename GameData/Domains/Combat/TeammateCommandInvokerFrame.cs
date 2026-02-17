using System;
using GameData.Common;
using GameData.Domains.SpecialEffect;
using GameData.Utilities;

namespace GameData.Domains.Combat
{
	// Token: 0x02000701 RID: 1793
	public class TeammateCommandInvokerFrame : TeammateCommandInvokerBase, IFrameCounterHandler
	{
		// Token: 0x060067E1 RID: 26593 RVA: 0x003B291C File Offset: 0x003B0B1C
		public TeammateCommandInvokerFrame(int charId, int index) : base(charId, index)
		{
			Tester.Assert(base.CmdConfig.AutoFrame > 0, "CmdConfig?.AutoFrame > 0");
			this._counter = new FrameCounter(this, base.CmdConfig.AutoFrame, 0);
		}

		// Token: 0x060067E2 RID: 26594 RVA: 0x003B2959 File Offset: 0x003B0B59
		public override void Setup()
		{
			this._counter.Setup();
		}

		// Token: 0x060067E3 RID: 26595 RVA: 0x003B2967 File Offset: 0x003B0B67
		public override void Close()
		{
			this._counter.Close();
		}

		// Token: 0x17000428 RID: 1064
		// (get) Token: 0x060067E4 RID: 26596 RVA: 0x003B2975 File Offset: 0x003B0B75
		public int CharacterId
		{
			get
			{
				return this.MainCharId;
			}
		}

		// Token: 0x060067E5 RID: 26597 RVA: 0x003B2980 File Offset: 0x003B0B80
		public void OnProcess(DataContext context, int counterType)
		{
			bool flag = context.Random.CheckPercentProb(base.CmdConfig.AutoProb);
			if (flag)
			{
				base.Execute(context);
			}
		}

		// Token: 0x04001C56 RID: 7254
		private readonly FrameCounter _counter;
	}
}
