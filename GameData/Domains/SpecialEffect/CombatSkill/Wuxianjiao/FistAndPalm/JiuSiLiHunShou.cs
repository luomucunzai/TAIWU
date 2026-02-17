using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Config;
using GameData.Common;
using GameData.Domains.Character;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;
using GameData.Domains.Information.Collection;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Attack;
using GameData.Utilities;

namespace GameData.Domains.SpecialEffect.CombatSkill.Wuxianjiao.FistAndPalm
{
	// Token: 0x02000394 RID: 916
	public class JiuSiLiHunShou : PoisonAddInjury
	{
		// Token: 0x06003658 RID: 13912 RVA: 0x002305C1 File Offset: 0x0022E7C1
		private static bool IsAffectChar(GameData.Domains.Character.Character character)
		{
			return character.GetCreatingType() == 1 && character.GetAgeGroup() == 2;
		}

		// Token: 0x06003659 RID: 13913 RVA: 0x002305D8 File Offset: 0x0022E7D8
		public JiuSiLiHunShou()
		{
		}

		// Token: 0x0600365A RID: 13914 RVA: 0x002305E2 File Offset: 0x0022E7E2
		public JiuSiLiHunShou(CombatSkillKey skillKey) : base(skillKey, 12107)
		{
			this.RequirePoisonType = 5;
		}

		// Token: 0x0600365B RID: 13915 RVA: 0x002305FC File Offset: 0x0022E7FC
		protected override void OnCastMaxPower(DataContext context)
		{
			CombatCharacter knowChar = base.CombatChar;
			bool flag = !JiuSiLiHunShou.IsAffectChar(knowChar.GetCharacter());
			if (!flag)
			{
				CombatCharacter enemyChar = DomainManager.Combat.GetCombatCharacter(!knowChar.IsAlly, false);
				bool flag2 = !JiuSiLiHunShou.IsAffectChar(enemyChar.GetCharacter());
				if (!flag2)
				{
					byte unit = (base.IsDirect ? enemyChar : knowChar).GetDefeatMarkCollection().PoisonMarkList[(int)this.RequirePoisonType];
					bool flag3 = !context.Random.CheckPercentProb((int)(unit * 30));
					if (!flag3)
					{
						int makeCharId = enemyChar.GetId();
						List<int> acceptCharIds = ObjectPool<List<int>>.Instance.Get();
						acceptCharIds.Clear();
						acceptCharIds.AddRange((from x in DomainManager.Combat.GetTeamCharacterIds()
						where x != enemyChar.GetId()
						select x).Where(delegate(int x)
						{
							GameData.Domains.Character.Character character;
							return DomainManager.Character.TryGetElement_Objects(x, out character) && JiuSiLiHunShou.IsAffectChar(character);
						}));
						int acceptCharId = (acceptCharIds.Count > 0) ? acceptCharIds.GetRandom(context.Random) : -1;
						ObjectPool<List<int>>.Instance.Return(acceptCharIds);
						bool flag4 = acceptCharId < 0;
						if (!flag4)
						{
							SecretInformationCollection secretInformationCollection = DomainManager.Information.GetSecretInformationCollection();
							int secretInfoType = context.Random.Next(4);
							if (!true)
							{
							}
							int num;
							switch (secretInfoType)
							{
							case 0:
								num = secretInformationCollection.AddKidnapInPrivate(makeCharId, acceptCharId);
								break;
							case 1:
								num = secretInformationCollection.AddPoisonEnemy(makeCharId, acceptCharId);
								break;
							case 2:
								num = secretInformationCollection.AddPlotHarmEnemy(makeCharId, acceptCharId);
								break;
							case 3:
								num = secretInformationCollection.AddRape(makeCharId, acceptCharId);
								break;
							default:
								num = -1;
								break;
							}
							if (!true)
							{
							}
							int secretInfoOffset = num;
							bool flag5 = secretInfoOffset < 0;
							if (flag5)
							{
								short predefinedLogId = 7;
								object arg = base.EffectId;
								DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(15, 1);
								defaultInterpolatedStringHandler.AppendLiteral("secretInfoType=");
								defaultInterpolatedStringHandler.AppendFormatted<int>(secretInfoType);
								PredefinedLog.Show(predefinedLogId, arg, defaultInterpolatedStringHandler.ToStringAndClear());
							}
							else
							{
								int secretInfoId = DomainManager.Information.AddSecretInformationMetaData(context, secretInfoOffset, false);
								DomainManager.Information.ReceiveSecretInformation(context, secretInfoId, knowChar.GetId(), -1);
								base.ShowSpecialEffectTips(1);
								base.CombatChar.SetSkillSoundToPlay("se_effect_secret_information", context);
								enemyChar.SetParticleToPlay(enemyChar.IsAlly ? "Particle_Effect_SecretInformation" : "Particle_Effect_SecretInformationReverse", context);
							}
						}
					}
				}
			}
		}

		// Token: 0x04000FD9 RID: 4057
		private const string SecretInformationParticleName = "Particle_Effect_SecretInformation";

		// Token: 0x04000FDA RID: 4058
		private const string SecretInformationParticleNameReverse = "Particle_Effect_SecretInformationReverse";

		// Token: 0x04000FDB RID: 4059
		private const string SecretInformationSoundName = "se_effect_secret_information";

		// Token: 0x04000FDC RID: 4060
		private const int SecretInformationOccurredRate = 30;
	}
}
