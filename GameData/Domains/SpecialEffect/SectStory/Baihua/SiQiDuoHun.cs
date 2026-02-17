using System;
using System.Collections.Generic;
using GameData.Common;

namespace GameData.Domains.SpecialEffect.SectStory.Baihua
{
	// Token: 0x02000107 RID: 263
	public class SiQiDuoHun : XiongZhongSiQi
	{
		// Token: 0x17000111 RID: 273
		// (get) Token: 0x060029C8 RID: 10696 RVA: 0x00201501 File Offset: 0x001FF701
		protected override short CombatStateId
		{
			get
			{
				return 225;
			}
		}

		// Token: 0x060029C9 RID: 10697 RVA: 0x00201508 File Offset: 0x001FF708
		public SiQiDuoHun(int charId) : base(charId)
		{
		}

		// Token: 0x060029CA RID: 10698 RVA: 0x00201513 File Offset: 0x001FF713
		protected override IEnumerable<int> CalcFrameCounterPeriods()
		{
			yield return 60;
			yield break;
		}

		// Token: 0x060029CB RID: 10699 RVA: 0x00201523 File Offset: 0x001FF723
		public override void OnProcess(DataContext context, int counterType)
		{
			DomainManager.Combat.AppendFatalDamageMarkImmediate(context, base.CombatChar, 1);
			DomainManager.Combat.ShowSpecialEffectTips(base.CharacterId, 1712, 0);
		}
	}
}
