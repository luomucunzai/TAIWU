using System;
using GameData.Common;
using GameData.DomainEvents;

namespace GameData.Domains.SpecialEffect.Profession.Savage
{
	// Token: 0x02000111 RID: 273
	public class TaoHuaYuan : ProfessionEffectBase
	{
		// Token: 0x1700011A RID: 282
		// (get) Token: 0x060029F2 RID: 10738 RVA: 0x00201902 File Offset: 0x001FFB02
		protected override short CombatStateId
		{
			get
			{
				return 141;
			}
		}

		// Token: 0x060029F3 RID: 10739 RVA: 0x00201909 File Offset: 0x001FFB09
		public TaoHuaYuan()
		{
		}

		// Token: 0x060029F4 RID: 10740 RVA: 0x00201913 File Offset: 0x001FFB13
		public TaoHuaYuan(int charId) : base(charId)
		{
		}

		// Token: 0x060029F5 RID: 10741 RVA: 0x0020191E File Offset: 0x001FFB1E
		public override void OnEnable(DataContext context)
		{
			base.OnEnable(context);
			Events.RegisterHandler_CombatBegin(new Events.OnCombatBegin(this.OnCombatBegin));
		}

		// Token: 0x060029F6 RID: 10742 RVA: 0x0020193B File Offset: 0x001FFB3B
		public override void OnDisable(DataContext context)
		{
			Events.UnRegisterHandler_CombatBegin(new Events.OnCombatBegin(this.OnCombatBegin));
			base.OnDisable(context);
		}

		// Token: 0x060029F7 RID: 10743 RVA: 0x00201958 File Offset: 0x001FFB58
		private void OnCombatBegin(DataContext context)
		{
			this._originNeili = this.CharObj.GetCurrNeili();
		}

		// Token: 0x060029F8 RID: 10744 RVA: 0x0020196C File Offset: 0x001FFB6C
		protected override void BeforeRemove(DataContext context)
		{
			base.BeforeRemove(context);
			int currNeili = this.CharObj.GetCurrNeili();
			bool flag = currNeili < this._originNeili;
			if (flag)
			{
				this.CharObj.ChangeCurrNeili(context, this._originNeili - currNeili);
			}
		}

		// Token: 0x04000CDF RID: 3295
		private int _originNeili;
	}
}
