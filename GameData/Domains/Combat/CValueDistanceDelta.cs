using System;

namespace GameData.Domains.Combat
{
	// Token: 0x020006CA RID: 1738
	public struct CValueDistanceDelta
	{
		// Token: 0x17000403 RID: 1027
		// (get) Token: 0x060066F5 RID: 26357 RVA: 0x003AF600 File Offset: 0x003AD800
		public bool IsForward
		{
			get
			{
				return this._distanceDelta < 0;
			}
		}

		// Token: 0x17000404 RID: 1028
		// (get) Token: 0x060066F6 RID: 26358 RVA: 0x003AF60B File Offset: 0x003AD80B
		public bool IsBackward
		{
			get
			{
				return this._distanceDelta > 0;
			}
		}

		// Token: 0x060066F7 RID: 26359 RVA: 0x003AF618 File Offset: 0x003AD818
		public static implicit operator CValueDistanceDelta(int percentValue)
		{
			return new CValueDistanceDelta
			{
				_distanceDelta = percentValue
			};
		}

		// Token: 0x060066F8 RID: 26360 RVA: 0x003AF63C File Offset: 0x003AD83C
		public static explicit operator int(CValueDistanceDelta percent)
		{
			return percent._distanceDelta;
		}

		// Token: 0x060066F9 RID: 26361 RVA: 0x003AF654 File Offset: 0x003AD854
		public static short operator +(short distance, CValueDistanceDelta delta)
		{
			return (short)Math.Clamp((int)distance + delta._distanceDelta, 20, 120);
		}

		// Token: 0x04001C02 RID: 7170
		private int _distanceDelta;
	}
}
