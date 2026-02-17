using System;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Wuxianjiao.WugEffect;

namespace GameData.Domains.SpecialEffect.CombatSkill.Wuxianjiao.Finger
{
	// Token: 0x0200039F RID: 927
	public class HeiXueGuJiang : AddWug
	{
		// Token: 0x170001EF RID: 495
		// (get) Token: 0x06003686 RID: 13958 RVA: 0x00230FB6 File Offset: 0x0022F1B6
		protected override int AddWugCount
		{
			get
			{
				return 6;
			}
		}

		// Token: 0x06003687 RID: 13959 RVA: 0x00230FB9 File Offset: 0x0022F1B9
		public HeiXueGuJiang()
		{
		}

		// Token: 0x06003688 RID: 13960 RVA: 0x00230FC3 File Offset: 0x0022F1C3
		public HeiXueGuJiang(CombatSkillKey skillKey) : base(skillKey, 12203)
		{
			this.WugType = 2;
		}
	}
}
