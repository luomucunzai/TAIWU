using System;
using System.Collections.Generic;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.CombatSkill;
using GameData.Utilities;

namespace GameData.Domains.SpecialEffect.SectStory.Shaolin
{
	// Token: 0x020000EC RID: 236
	public class AddRandomFlawAndAcupoint : DemonSlayerTrialEffectBase
	{
		// Token: 0x0600296F RID: 10607 RVA: 0x00200AA0 File Offset: 0x001FECA0
		public AddRandomFlawAndAcupoint(int charId, IReadOnlyList<int> parameters) : base(charId)
		{
			this._bodyPartCount = parameters[0];
			this._flawCount = parameters[1];
			this._flawLevel = parameters[2];
			this._acupointCount = parameters[3];
			this._acupointLevel = parameters[4];
		}

		// Token: 0x06002970 RID: 10608 RVA: 0x00200AF7 File Offset: 0x001FECF7
		public override void OnEnable(DataContext context)
		{
			base.OnEnable(context);
			Events.RegisterHandler_CombatBegin(new Events.OnCombatBegin(this.OnCombatBegin));
		}

		// Token: 0x06002971 RID: 10609 RVA: 0x00200B14 File Offset: 0x001FED14
		public override void OnDisable(DataContext context)
		{
			Events.UnRegisterHandler_CombatBegin(new Events.OnCombatBegin(this.OnCombatBegin));
			base.OnDisable(context);
		}

		// Token: 0x06002972 RID: 10610 RVA: 0x00200B34 File Offset: 0x001FED34
		private void OnCombatBegin(DataContext context)
		{
			List<sbyte> bodyParts = ObjectPool<List<sbyte>>.Instance.Get();
			bodyParts.Clear();
			for (sbyte i = 0; i < 7; i += 1)
			{
				bodyParts.Add(i);
			}
			foreach (sbyte bodyPart in RandomUtils.GetRandomUnrepeated<sbyte>(context.Random, this._bodyPartCount, bodyParts, null))
			{
				CombatSkillKey key = new CombatSkillKey(-1, -1);
				DomainManager.Combat.AddFlaw(context, base.CombatChar, (sbyte)this._flawLevel, key, bodyPart, this._flawCount, true);
				DomainManager.Combat.AddAcupoint(context, base.CombatChar, (sbyte)this._acupointLevel, key, bodyPart, this._acupointCount, true);
			}
			ObjectPool<List<sbyte>>.Instance.Return(bodyParts);
		}

		// Token: 0x04000CC3 RID: 3267
		private readonly int _bodyPartCount;

		// Token: 0x04000CC4 RID: 3268
		private readonly int _flawCount;

		// Token: 0x04000CC5 RID: 3269
		private readonly int _flawLevel;

		// Token: 0x04000CC6 RID: 3270
		private readonly int _acupointCount;

		// Token: 0x04000CC7 RID: 3271
		private readonly int _acupointLevel;
	}
}
