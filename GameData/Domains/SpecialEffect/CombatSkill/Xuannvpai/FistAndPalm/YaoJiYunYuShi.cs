using System;
using GameData.Combat.Math;
using GameData.Common;
using GameData.Domains.Character;
using GameData.Domains.Character.Relation;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Attack;
using GameData.Utilities;

namespace GameData.Domains.SpecialEffect.CombatSkill.Xuannvpai.FistAndPalm
{
	// Token: 0x02000275 RID: 629
	public class YaoJiYunYuShi : PowerUpOnCast
	{
		// Token: 0x170001C4 RID: 452
		// (get) Token: 0x060030AB RID: 12459 RVA: 0x002181FD File Offset: 0x002163FD
		protected override EDataModifyType ModifyType
		{
			get
			{
				return EDataModifyType.AddPercent;
			}
		}

		// Token: 0x060030AC RID: 12460 RVA: 0x00218200 File Offset: 0x00216400
		public YaoJiYunYuShi()
		{
		}

		// Token: 0x060030AD RID: 12461 RVA: 0x0021820A File Offset: 0x0021640A
		public YaoJiYunYuShi(CombatSkillKey skillKey) : base(skillKey, 8105)
		{
		}

		// Token: 0x060030AE RID: 12462 RVA: 0x0021821C File Offset: 0x0021641C
		public override void OnEnable(DataContext context)
		{
			Character enemyChar = DomainManager.Combat.GetCombatCharacter(!base.CombatChar.IsAlly, true).GetCharacter();
			Character likeChar = base.IsDirect ? enemyChar : this.CharObj;
			Character charmChar = base.IsDirect ? this.CharObj : enemyChar;
			bool flag = enemyChar.GetCreatingType() == 1 && (likeChar.GetBisexual() || charmChar.GetAvatar().Gender == Gender.Flip(likeChar.GetGender()));
			if (flag)
			{
				this.PowerUpValue = (int)(charmChar.GetAttraction() / 15);
			}
			base.OnEnable(context);
		}

		// Token: 0x060030AF RID: 12463 RVA: 0x002182B8 File Offset: 0x002164B8
		protected override void OnCastSelf(DataContext context, sbyte power, bool interrupted)
		{
			bool flag = !base.PowerMatchAffectRequire((int)power, 0) || !this.AllowAddAdore(base.CurrEnemyChar.GetId());
			if (!flag)
			{
				Character enemyChar = DomainManager.Combat.GetCombatCharacter(!base.CombatChar.IsAlly, true).GetCharacter();
				Character likeChar = base.IsDirect ? enemyChar : this.CharObj;
				Character charmChar = base.IsDirect ? this.CharObj : enemyChar;
				int rate = (int)(charmChar.GetAttraction() / 100);
				bool flag2 = context.Random.CheckPercentProb(rate);
				if (flag2)
				{
					Character.ApplyAddRelation_Adore(context, likeChar, charmChar, likeChar.GetBehaviorType(), false, false, false);
					base.ShowSpecialEffectTips(1);
				}
			}
		}

		// Token: 0x060030B0 RID: 12464 RVA: 0x0021836C File Offset: 0x0021656C
		private bool AllowAddAdore(int relatedCharId)
		{
			int charId = base.CharacterId;
			Character relatedChar;
			bool flag = !DomainManager.Character.TryGetElement_Objects(relatedCharId, out relatedChar);
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				bool flag2 = relatedChar.GetAgeGroup() != 2 || relatedChar.GetCreatingType() != 1;
				if (flag2)
				{
					result = false;
				}
				else
				{
					RelatedCharacter relation;
					bool flag3 = !DomainManager.Character.TryGetRelation(charId, relatedCharId, out relation);
					if (flag3)
					{
						relation.RelationType = ushort.MaxValue;
					}
					bool flag4 = relation.RelationType == ushort.MaxValue;
					result = (flag4 || !RelationType.HasRelation(relation.RelationType, 511));
				}
			}
			return result;
		}
	}
}
