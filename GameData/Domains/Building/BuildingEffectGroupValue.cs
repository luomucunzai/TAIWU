using System;
using System.Runtime.CompilerServices;

namespace GameData.Domains.Building
{
	// Token: 0x020008C2 RID: 2242
	public class BuildingEffectGroupValue : BuildingEffectValue
	{
		// Token: 0x06007E7F RID: 32383 RVA: 0x004BC8FC File Offset: 0x004BAAFC
		public BuildingEffectGroupValue(int count)
		{
			this._group = new BuildingEffectValue[count];
			for (int i = 0; i < count; i++)
			{
				this._group[i] = new BuildingEffectValue();
			}
		}

		// Token: 0x06007E80 RID: 32384 RVA: 0x004BC93A File Offset: 0x004BAB3A
		public override int Get(int index)
		{
			return base.Get() + this._group[index].Get();
		}

		// Token: 0x06007E81 RID: 32385 RVA: 0x004BC950 File Offset: 0x004BAB50
		public override void Change(int index, int delta)
		{
			this._group[index].Change(delta);
		}

		// Token: 0x06007E82 RID: 32386 RVA: 0x004BC964 File Offset: 0x004BAB64
		public override void Clear()
		{
			base.Clear();
			for (int i = 0; i < this._group.Length; i++)
			{
				this._group[i].Clear();
			}
		}

		// Token: 0x06007E83 RID: 32387 RVA: 0x004BC9A0 File Offset: 0x004BABA0
		public override string ToString()
		{
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(4, 2);
			defaultInterpolatedStringHandler.AppendFormatted<int>(base.Get());
			defaultInterpolatedStringHandler.AppendLiteral(", {");
			defaultInterpolatedStringHandler.AppendFormatted(string.Join<BuildingEffectValue>(',', this._group));
			defaultInterpolatedStringHandler.AppendLiteral("}");
			return defaultInterpolatedStringHandler.ToStringAndClear();
		}

		// Token: 0x04002298 RID: 8856
		private readonly BuildingEffectValue[] _group;
	}
}
