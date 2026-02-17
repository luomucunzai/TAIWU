using System;
using System.Collections.Generic;
using Config;
using GameData.Combat.Math;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.CombatSkill;

namespace GameData.Domains.SpecialEffect.CombatSkill.Zhujianshanzhuang.Shot
{
	// Token: 0x020001C3 RID: 451
	public class XiuLiFeiYan : CombatSkillEffectBase
	{
		// Token: 0x06002CCA RID: 11466 RVA: 0x002097A6 File Offset: 0x002079A6
		public XiuLiFeiYan()
		{
		}

		// Token: 0x06002CCB RID: 11467 RVA: 0x002097B0 File Offset: 0x002079B0
		public XiuLiFeiYan(CombatSkillKey skillKey) : base(skillKey, 9400, -1)
		{
		}

		// Token: 0x06002CCC RID: 11468 RVA: 0x002097C1 File Offset: 0x002079C1
		public override void OnEnable(DataContext context)
		{
			base.CreateAffectedData(164, EDataModifyType.Custom, -1);
			Events.RegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
			Events.RegisterHandler_GetTrick(new Events.OnGetTrick(this.OnGetTrick));
		}

		// Token: 0x06002CCD RID: 11469 RVA: 0x002097F6 File Offset: 0x002079F6
		public override void OnDisable(DataContext context)
		{
			Events.UnRegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
			Events.UnRegisterHandler_GetTrick(new Events.OnGetTrick(this.OnGetTrick));
		}

		// Token: 0x06002CCE RID: 11470 RVA: 0x00209820 File Offset: 0x00207A20
		private void OnCastSkillEnd(DataContext context, int charId, bool isAlly, short skillId, sbyte power, bool interrupted)
		{
			bool flag = this.SkillKey.IsMatch(charId, skillId) && base.PowerMatchAffectRequire((int)power, 0);
			if (flag)
			{
				base.AddMaxEffectCount(true);
			}
		}

		// Token: 0x06002CCF RID: 11471 RVA: 0x00209858 File Offset: 0x00207A58
		private void OnGetTrick(DataContext context, int charId, bool isAlly, sbyte trickType, bool usable)
		{
			bool flag = base.CharacterId != charId || !base.IsDirect || trickType != 12 || this._gettingTrick || base.EffectCount <= 0;
			if (!flag)
			{
				this._gettingTrick = true;
				DomainManager.Combat.AddTrick(context, base.CombatChar, 12, true);
				this._gettingTrick = false;
				base.ShowSpecialEffectTips(0);
				base.ReduceEffectCount(1);
			}
		}

		// Token: 0x06002CD0 RID: 11472 RVA: 0x002098D0 File Offset: 0x00207AD0
		public override List<NeedTrick> GetModifiedValue(AffectedDataKey dataKey, List<NeedTrick> dataValue)
		{
			bool flag = dataKey.CharId != base.CharacterId || dataKey.FieldId != 164 || base.EffectCount <= 0;
			List<NeedTrick> result;
			if (flag)
			{
				result = dataValue;
			}
			else
			{
				bool flag2 = !dataValue.Exists((NeedTrick needTrick) => needTrick.TrickType == 12);
				if (flag2)
				{
					result = dataValue;
				}
				else
				{
					for (int i = 0; i < dataValue.Count; i++)
					{
						NeedTrick costTrick = dataValue[i];
						bool flag3 = costTrick.TrickType != 12;
						if (!flag3)
						{
							costTrick.NeedCount -= 1;
							dataValue[i] = costTrick;
							break;
						}
					}
					base.ShowSpecialEffectTips(0);
					base.ReduceEffectCount(1);
					result = dataValue;
				}
			}
			return result;
		}

		// Token: 0x04000D82 RID: 3458
		private bool _gettingTrick;
	}
}
