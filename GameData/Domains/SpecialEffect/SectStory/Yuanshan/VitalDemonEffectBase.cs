using System;
using GameData.Domains.Combat;

namespace GameData.Domains.SpecialEffect.SectStory.Yuanshan
{
	// Token: 0x020000EB RID: 235
	public class VitalDemonEffectBase : AutoCollectEffectBase
	{
		// Token: 0x0600296D RID: 10605 RVA: 0x00200A4C File Offset: 0x001FEC4C
		protected VitalDemonEffectBase(int charId, short effectId) : base(charId)
		{
			this._effectId = effectId;
		}

		// Token: 0x0600296E RID: 10606 RVA: 0x00200A60 File Offset: 0x001FEC60
		protected void ShowSpecialEffect(byte index = 0)
		{
			CombatCharacter character = DomainManager.Combat.GetCombatCharacter(base.CombatChar.IsAlly, false);
			DomainManager.Combat.ShowSpecialEffectTips(character.GetId(), (int)this._effectId, index);
		}

		// Token: 0x04000CC2 RID: 3266
		private readonly short _effectId;
	}
}
