using System;

namespace GameData.Domains.Building
{
	// Token: 0x020008C1 RID: 2241
	public class BuildingEffectValue : IBuildingEffectValue
	{
		// Token: 0x06007E78 RID: 32376 RVA: 0x004BC8B5 File Offset: 0x004BAAB5
		public void Change(int delta)
		{
			this._value += delta;
		}

		// Token: 0x06007E79 RID: 32377 RVA: 0x004BC8C5 File Offset: 0x004BAAC5
		public virtual void Change(int index, int delta)
		{
			throw new NotSupportedException();
		}

		// Token: 0x06007E7A RID: 32378 RVA: 0x004BC8CC File Offset: 0x004BAACC
		public virtual void Clear()
		{
			this._value = 0;
		}

		// Token: 0x06007E7B RID: 32379 RVA: 0x004BC8D5 File Offset: 0x004BAAD5
		public int Get()
		{
			return this._value;
		}

		// Token: 0x06007E7C RID: 32380 RVA: 0x004BC8DD File Offset: 0x004BAADD
		public virtual int Get(int index)
		{
			return this.Get();
		}

		// Token: 0x06007E7D RID: 32381 RVA: 0x004BC8E5 File Offset: 0x004BAAE5
		public override string ToString()
		{
			return this._value.ToString();
		}

		// Token: 0x04002297 RID: 8855
		private int _value;
	}
}
