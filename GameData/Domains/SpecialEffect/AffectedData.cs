using System;
using System.Runtime.CompilerServices;
using GameData.ArchiveData;
using GameData.Common;
using GameData.Serializer;

namespace GameData.Domains.SpecialEffect
{
	// Token: 0x020000D8 RID: 216
	[SerializableGameData(NotForDisplayModule = true)]
	public class AffectedData : BaseGameDataObject, ISerializableGameData
	{
		// Token: 0x060025D0 RID: 9680 RVA: 0x001CD6FF File Offset: 0x001CB8FF
		public AffectedData(int charId)
		{
			this._id = charId;
		}

		// Token: 0x060025D1 RID: 9681 RVA: 0x001CD710 File Offset: 0x001CB910
		public SpecialEffectList GetEffectList(ushort fieldId, bool createIfNull = false)
		{
			SpecialEffectList result;
			switch (fieldId)
			{
			case 1:
			{
				bool flag = this._maxStrength == null && createIfNull;
				if (flag)
				{
					this._maxStrength = new SpecialEffectList();
				}
				result = this._maxStrength;
				break;
			}
			case 2:
			{
				bool flag2 = this._maxDexterity == null && createIfNull;
				if (flag2)
				{
					this._maxDexterity = new SpecialEffectList();
				}
				result = this._maxDexterity;
				break;
			}
			case 3:
			{
				bool flag3 = this._maxConcentration == null && createIfNull;
				if (flag3)
				{
					this._maxConcentration = new SpecialEffectList();
				}
				result = this._maxConcentration;
				break;
			}
			case 4:
			{
				bool flag4 = this._maxVitality == null && createIfNull;
				if (flag4)
				{
					this._maxVitality = new SpecialEffectList();
				}
				result = this._maxVitality;
				break;
			}
			case 5:
			{
				bool flag5 = this._maxEnergy == null && createIfNull;
				if (flag5)
				{
					this._maxEnergy = new SpecialEffectList();
				}
				result = this._maxEnergy;
				break;
			}
			case 6:
			{
				bool flag6 = this._maxIntelligence == null && createIfNull;
				if (flag6)
				{
					this._maxIntelligence = new SpecialEffectList();
				}
				result = this._maxIntelligence;
				break;
			}
			case 7:
			{
				bool flag7 = this._recoveryOfStance == null && createIfNull;
				if (flag7)
				{
					this._recoveryOfStance = new SpecialEffectList();
				}
				result = this._recoveryOfStance;
				break;
			}
			case 8:
			{
				bool flag8 = this._recoveryOfBreath == null && createIfNull;
				if (flag8)
				{
					this._recoveryOfBreath = new SpecialEffectList();
				}
				result = this._recoveryOfBreath;
				break;
			}
			case 9:
			{
				bool flag9 = this._moveSpeed == null && createIfNull;
				if (flag9)
				{
					this._moveSpeed = new SpecialEffectList();
				}
				result = this._moveSpeed;
				break;
			}
			case 10:
			{
				bool flag10 = this._recoveryOfFlaw == null && createIfNull;
				if (flag10)
				{
					this._recoveryOfFlaw = new SpecialEffectList();
				}
				result = this._recoveryOfFlaw;
				break;
			}
			case 11:
			{
				bool flag11 = this._castSpeed == null && createIfNull;
				if (flag11)
				{
					this._castSpeed = new SpecialEffectList();
				}
				result = this._castSpeed;
				break;
			}
			case 12:
			{
				bool flag12 = this._recoveryOfBlockedAcupoint == null && createIfNull;
				if (flag12)
				{
					this._recoveryOfBlockedAcupoint = new SpecialEffectList();
				}
				result = this._recoveryOfBlockedAcupoint;
				break;
			}
			case 13:
			{
				bool flag13 = this._weaponSwitchSpeed == null && createIfNull;
				if (flag13)
				{
					this._weaponSwitchSpeed = new SpecialEffectList();
				}
				result = this._weaponSwitchSpeed;
				break;
			}
			case 14:
			{
				bool flag14 = this._attackSpeed == null && createIfNull;
				if (flag14)
				{
					this._attackSpeed = new SpecialEffectList();
				}
				result = this._attackSpeed;
				break;
			}
			case 15:
			{
				bool flag15 = this._innerRatio == null && createIfNull;
				if (flag15)
				{
					this._innerRatio = new SpecialEffectList();
				}
				result = this._innerRatio;
				break;
			}
			case 16:
			{
				bool flag16 = this._recoveryOfQiDisorder == null && createIfNull;
				if (flag16)
				{
					this._recoveryOfQiDisorder = new SpecialEffectList();
				}
				result = this._recoveryOfQiDisorder;
				break;
			}
			case 17:
			{
				bool flag17 = this._minorAttributeFixMaxValue == null && createIfNull;
				if (flag17)
				{
					this._minorAttributeFixMaxValue = new SpecialEffectList();
				}
				result = this._minorAttributeFixMaxValue;
				break;
			}
			case 18:
			{
				bool flag18 = this._minorAttributeFixMinValue == null && createIfNull;
				if (flag18)
				{
					this._minorAttributeFixMinValue = new SpecialEffectList();
				}
				result = this._minorAttributeFixMinValue;
				break;
			}
			case 19:
			{
				bool flag19 = this._resistOfHotPoison == null && createIfNull;
				if (flag19)
				{
					this._resistOfHotPoison = new SpecialEffectList();
				}
				result = this._resistOfHotPoison;
				break;
			}
			case 20:
			{
				bool flag20 = this._resistOfGloomyPoison == null && createIfNull;
				if (flag20)
				{
					this._resistOfGloomyPoison = new SpecialEffectList();
				}
				result = this._resistOfGloomyPoison;
				break;
			}
			case 21:
			{
				bool flag21 = this._resistOfColdPoison == null && createIfNull;
				if (flag21)
				{
					this._resistOfColdPoison = new SpecialEffectList();
				}
				result = this._resistOfColdPoison;
				break;
			}
			case 22:
			{
				bool flag22 = this._resistOfRedPoison == null && createIfNull;
				if (flag22)
				{
					this._resistOfRedPoison = new SpecialEffectList();
				}
				result = this._resistOfRedPoison;
				break;
			}
			case 23:
			{
				bool flag23 = this._resistOfRottenPoison == null && createIfNull;
				if (flag23)
				{
					this._resistOfRottenPoison = new SpecialEffectList();
				}
				result = this._resistOfRottenPoison;
				break;
			}
			case 24:
			{
				bool flag24 = this._resistOfIllusoryPoison == null && createIfNull;
				if (flag24)
				{
					this._resistOfIllusoryPoison = new SpecialEffectList();
				}
				result = this._resistOfIllusoryPoison;
				break;
			}
			case 25:
			{
				bool flag25 = this._displayAge == null && createIfNull;
				if (flag25)
				{
					this._displayAge = new SpecialEffectList();
				}
				result = this._displayAge;
				break;
			}
			case 26:
			{
				bool flag26 = this._neiliProportionOfFiveElements == null && createIfNull;
				if (flag26)
				{
					this._neiliProportionOfFiveElements = new SpecialEffectList();
				}
				result = this._neiliProportionOfFiveElements;
				break;
			}
			case 27:
			{
				bool flag27 = this._weaponMaxPower == null && createIfNull;
				if (flag27)
				{
					this._weaponMaxPower = new SpecialEffectList();
				}
				result = this._weaponMaxPower;
				break;
			}
			case 28:
			{
				bool flag28 = this._weaponUseRequirement == null && createIfNull;
				if (flag28)
				{
					this._weaponUseRequirement = new SpecialEffectList();
				}
				result = this._weaponUseRequirement;
				break;
			}
			case 29:
			{
				bool flag29 = this._weaponAttackRange == null && createIfNull;
				if (flag29)
				{
					this._weaponAttackRange = new SpecialEffectList();
				}
				result = this._weaponAttackRange;
				break;
			}
			case 30:
			{
				bool flag30 = this._armorMaxPower == null && createIfNull;
				if (flag30)
				{
					this._armorMaxPower = new SpecialEffectList();
				}
				result = this._armorMaxPower;
				break;
			}
			case 31:
			{
				bool flag31 = this._armorUseRequirement == null && createIfNull;
				if (flag31)
				{
					this._armorUseRequirement = new SpecialEffectList();
				}
				result = this._armorUseRequirement;
				break;
			}
			case 32:
			{
				bool flag32 = this._hitStrength == null && createIfNull;
				if (flag32)
				{
					this._hitStrength = new SpecialEffectList();
				}
				result = this._hitStrength;
				break;
			}
			case 33:
			{
				bool flag33 = this._hitTechnique == null && createIfNull;
				if (flag33)
				{
					this._hitTechnique = new SpecialEffectList();
				}
				result = this._hitTechnique;
				break;
			}
			case 34:
			{
				bool flag34 = this._hitSpeed == null && createIfNull;
				if (flag34)
				{
					this._hitSpeed = new SpecialEffectList();
				}
				result = this._hitSpeed;
				break;
			}
			case 35:
			{
				bool flag35 = this._hitMind == null && createIfNull;
				if (flag35)
				{
					this._hitMind = new SpecialEffectList();
				}
				result = this._hitMind;
				break;
			}
			case 36:
			{
				bool flag36 = this._hitCanChange == null && createIfNull;
				if (flag36)
				{
					this._hitCanChange = new SpecialEffectList();
				}
				result = this._hitCanChange;
				break;
			}
			case 37:
			{
				bool flag37 = this._hitChangeEffectPercent == null && createIfNull;
				if (flag37)
				{
					this._hitChangeEffectPercent = new SpecialEffectList();
				}
				result = this._hitChangeEffectPercent;
				break;
			}
			case 38:
			{
				bool flag38 = this._avoidStrength == null && createIfNull;
				if (flag38)
				{
					this._avoidStrength = new SpecialEffectList();
				}
				result = this._avoidStrength;
				break;
			}
			case 39:
			{
				bool flag39 = this._avoidTechnique == null && createIfNull;
				if (flag39)
				{
					this._avoidTechnique = new SpecialEffectList();
				}
				result = this._avoidTechnique;
				break;
			}
			case 40:
			{
				bool flag40 = this._avoidSpeed == null && createIfNull;
				if (flag40)
				{
					this._avoidSpeed = new SpecialEffectList();
				}
				result = this._avoidSpeed;
				break;
			}
			case 41:
			{
				bool flag41 = this._avoidMind == null && createIfNull;
				if (flag41)
				{
					this._avoidMind = new SpecialEffectList();
				}
				result = this._avoidMind;
				break;
			}
			case 42:
			{
				bool flag42 = this._avoidCanChange == null && createIfNull;
				if (flag42)
				{
					this._avoidCanChange = new SpecialEffectList();
				}
				result = this._avoidCanChange;
				break;
			}
			case 43:
			{
				bool flag43 = this._avoidChangeEffectPercent == null && createIfNull;
				if (flag43)
				{
					this._avoidChangeEffectPercent = new SpecialEffectList();
				}
				result = this._avoidChangeEffectPercent;
				break;
			}
			case 44:
			{
				bool flag44 = this._penetrateOuter == null && createIfNull;
				if (flag44)
				{
					this._penetrateOuter = new SpecialEffectList();
				}
				result = this._penetrateOuter;
				break;
			}
			case 45:
			{
				bool flag45 = this._penetrateInner == null && createIfNull;
				if (flag45)
				{
					this._penetrateInner = new SpecialEffectList();
				}
				result = this._penetrateInner;
				break;
			}
			case 46:
			{
				bool flag46 = this._penetrateResistOuter == null && createIfNull;
				if (flag46)
				{
					this._penetrateResistOuter = new SpecialEffectList();
				}
				result = this._penetrateResistOuter;
				break;
			}
			case 47:
			{
				bool flag47 = this._penetrateResistInner == null && createIfNull;
				if (flag47)
				{
					this._penetrateResistInner = new SpecialEffectList();
				}
				result = this._penetrateResistInner;
				break;
			}
			case 48:
			{
				bool flag48 = this._neiliAllocationAttack == null && createIfNull;
				if (flag48)
				{
					this._neiliAllocationAttack = new SpecialEffectList();
				}
				result = this._neiliAllocationAttack;
				break;
			}
			case 49:
			{
				bool flag49 = this._neiliAllocationAgile == null && createIfNull;
				if (flag49)
				{
					this._neiliAllocationAgile = new SpecialEffectList();
				}
				result = this._neiliAllocationAgile;
				break;
			}
			case 50:
			{
				bool flag50 = this._neiliAllocationDefense == null && createIfNull;
				if (flag50)
				{
					this._neiliAllocationDefense = new SpecialEffectList();
				}
				result = this._neiliAllocationDefense;
				break;
			}
			case 51:
			{
				bool flag51 = this._neiliAllocationAssist == null && createIfNull;
				if (flag51)
				{
					this._neiliAllocationAssist = new SpecialEffectList();
				}
				result = this._neiliAllocationAssist;
				break;
			}
			case 52:
			{
				bool flag52 = this._happiness == null && createIfNull;
				if (flag52)
				{
					this._happiness = new SpecialEffectList();
				}
				result = this._happiness;
				break;
			}
			case 53:
			{
				bool flag53 = this._maxHealth == null && createIfNull;
				if (flag53)
				{
					this._maxHealth = new SpecialEffectList();
				}
				result = this._maxHealth;
				break;
			}
			case 54:
			{
				bool flag54 = this._healthCost == null && createIfNull;
				if (flag54)
				{
					this._healthCost = new SpecialEffectList();
				}
				result = this._healthCost;
				break;
			}
			case 55:
			{
				bool flag55 = this._moveSpeedCanChange == null && createIfNull;
				if (flag55)
				{
					this._moveSpeedCanChange = new SpecialEffectList();
				}
				result = this._moveSpeedCanChange;
				break;
			}
			case 56:
			{
				bool flag56 = this._attackerHitStrength == null && createIfNull;
				if (flag56)
				{
					this._attackerHitStrength = new SpecialEffectList();
				}
				result = this._attackerHitStrength;
				break;
			}
			case 57:
			{
				bool flag57 = this._attackerHitTechnique == null && createIfNull;
				if (flag57)
				{
					this._attackerHitTechnique = new SpecialEffectList();
				}
				result = this._attackerHitTechnique;
				break;
			}
			case 58:
			{
				bool flag58 = this._attackerHitSpeed == null && createIfNull;
				if (flag58)
				{
					this._attackerHitSpeed = new SpecialEffectList();
				}
				result = this._attackerHitSpeed;
				break;
			}
			case 59:
			{
				bool flag59 = this._attackerHitMind == null && createIfNull;
				if (flag59)
				{
					this._attackerHitMind = new SpecialEffectList();
				}
				result = this._attackerHitMind;
				break;
			}
			case 60:
			{
				bool flag60 = this._attackerAvoidStrength == null && createIfNull;
				if (flag60)
				{
					this._attackerAvoidStrength = new SpecialEffectList();
				}
				result = this._attackerAvoidStrength;
				break;
			}
			case 61:
			{
				bool flag61 = this._attackerAvoidTechnique == null && createIfNull;
				if (flag61)
				{
					this._attackerAvoidTechnique = new SpecialEffectList();
				}
				result = this._attackerAvoidTechnique;
				break;
			}
			case 62:
			{
				bool flag62 = this._attackerAvoidSpeed == null && createIfNull;
				if (flag62)
				{
					this._attackerAvoidSpeed = new SpecialEffectList();
				}
				result = this._attackerAvoidSpeed;
				break;
			}
			case 63:
			{
				bool flag63 = this._attackerAvoidMind == null && createIfNull;
				if (flag63)
				{
					this._attackerAvoidMind = new SpecialEffectList();
				}
				result = this._attackerAvoidMind;
				break;
			}
			case 64:
			{
				bool flag64 = this._attackerPenetrateOuter == null && createIfNull;
				if (flag64)
				{
					this._attackerPenetrateOuter = new SpecialEffectList();
				}
				result = this._attackerPenetrateOuter;
				break;
			}
			case 65:
			{
				bool flag65 = this._attackerPenetrateInner == null && createIfNull;
				if (flag65)
				{
					this._attackerPenetrateInner = new SpecialEffectList();
				}
				result = this._attackerPenetrateInner;
				break;
			}
			case 66:
			{
				bool flag66 = this._attackerPenetrateResistOuter == null && createIfNull;
				if (flag66)
				{
					this._attackerPenetrateResistOuter = new SpecialEffectList();
				}
				result = this._attackerPenetrateResistOuter;
				break;
			}
			case 67:
			{
				bool flag67 = this._attackerPenetrateResistInner == null && createIfNull;
				if (flag67)
				{
					this._attackerPenetrateResistInner = new SpecialEffectList();
				}
				result = this._attackerPenetrateResistInner;
				break;
			}
			case 68:
			{
				bool flag68 = this._attackHitType == null && createIfNull;
				if (flag68)
				{
					this._attackHitType = new SpecialEffectList();
				}
				result = this._attackHitType;
				break;
			}
			case 69:
			{
				bool flag69 = this._makeDirectDamage == null && createIfNull;
				if (flag69)
				{
					this._makeDirectDamage = new SpecialEffectList();
				}
				result = this._makeDirectDamage;
				break;
			}
			case 70:
			{
				bool flag70 = this._makeBounceDamage == null && createIfNull;
				if (flag70)
				{
					this._makeBounceDamage = new SpecialEffectList();
				}
				result = this._makeBounceDamage;
				break;
			}
			case 71:
			{
				bool flag71 = this._makeFightBackDamage == null && createIfNull;
				if (flag71)
				{
					this._makeFightBackDamage = new SpecialEffectList();
				}
				result = this._makeFightBackDamage;
				break;
			}
			case 72:
			{
				bool flag72 = this._makePoisonLevel == null && createIfNull;
				if (flag72)
				{
					this._makePoisonLevel = new SpecialEffectList();
				}
				result = this._makePoisonLevel;
				break;
			}
			case 73:
			{
				bool flag73 = this._makePoisonValue == null && createIfNull;
				if (flag73)
				{
					this._makePoisonValue = new SpecialEffectList();
				}
				result = this._makePoisonValue;
				break;
			}
			case 74:
			{
				bool flag74 = this._attackerHitOdds == null && createIfNull;
				if (flag74)
				{
					this._attackerHitOdds = new SpecialEffectList();
				}
				result = this._attackerHitOdds;
				break;
			}
			case 75:
			{
				bool flag75 = this._attackerFightBackHitOdds == null && createIfNull;
				if (flag75)
				{
					this._attackerFightBackHitOdds = new SpecialEffectList();
				}
				result = this._attackerFightBackHitOdds;
				break;
			}
			case 76:
			{
				bool flag76 = this._attackerPursueOdds == null && createIfNull;
				if (flag76)
				{
					this._attackerPursueOdds = new SpecialEffectList();
				}
				result = this._attackerPursueOdds;
				break;
			}
			case 77:
			{
				bool flag77 = this._makedInjuryChangeToOld == null && createIfNull;
				if (flag77)
				{
					this._makedInjuryChangeToOld = new SpecialEffectList();
				}
				result = this._makedInjuryChangeToOld;
				break;
			}
			case 78:
			{
				bool flag78 = this._makedPoisonChangeToOld == null && createIfNull;
				if (flag78)
				{
					this._makedPoisonChangeToOld = new SpecialEffectList();
				}
				result = this._makedPoisonChangeToOld;
				break;
			}
			case 79:
			{
				bool flag79 = this._makeDamageType == null && createIfNull;
				if (flag79)
				{
					this._makeDamageType = new SpecialEffectList();
				}
				result = this._makeDamageType;
				break;
			}
			case 80:
			{
				bool flag80 = this._canMakeInjuryToNoInjuryPart == null && createIfNull;
				if (flag80)
				{
					this._canMakeInjuryToNoInjuryPart = new SpecialEffectList();
				}
				result = this._canMakeInjuryToNoInjuryPart;
				break;
			}
			case 81:
			{
				bool flag81 = this._makePoisonType == null && createIfNull;
				if (flag81)
				{
					this._makePoisonType = new SpecialEffectList();
				}
				result = this._makePoisonType;
				break;
			}
			case 82:
			{
				bool flag82 = this._normalAttackWeapon == null && createIfNull;
				if (flag82)
				{
					this._normalAttackWeapon = new SpecialEffectList();
				}
				result = this._normalAttackWeapon;
				break;
			}
			case 83:
			{
				bool flag83 = this._normalAttackTrick == null && createIfNull;
				if (flag83)
				{
					this._normalAttackTrick = new SpecialEffectList();
				}
				result = this._normalAttackTrick;
				break;
			}
			case 84:
			{
				bool flag84 = this._extraFlawCount == null && createIfNull;
				if (flag84)
				{
					this._extraFlawCount = new SpecialEffectList();
				}
				result = this._extraFlawCount;
				break;
			}
			case 85:
			{
				bool flag85 = this._attackCanBounce == null && createIfNull;
				if (flag85)
				{
					this._attackCanBounce = new SpecialEffectList();
				}
				result = this._attackCanBounce;
				break;
			}
			case 86:
			{
				bool flag86 = this._attackCanFightBack == null && createIfNull;
				if (flag86)
				{
					this._attackCanFightBack = new SpecialEffectList();
				}
				result = this._attackCanFightBack;
				break;
			}
			case 87:
			{
				bool flag87 = this._makeFightBackInjuryMark == null && createIfNull;
				if (flag87)
				{
					this._makeFightBackInjuryMark = new SpecialEffectList();
				}
				result = this._makeFightBackInjuryMark;
				break;
			}
			case 88:
			{
				bool flag88 = this._legSkillUseShoes == null && createIfNull;
				if (flag88)
				{
					this._legSkillUseShoes = new SpecialEffectList();
				}
				result = this._legSkillUseShoes;
				break;
			}
			case 89:
			{
				bool flag89 = this._attackerFinalDamageValue == null && createIfNull;
				if (flag89)
				{
					this._attackerFinalDamageValue = new SpecialEffectList();
				}
				result = this._attackerFinalDamageValue;
				break;
			}
			case 90:
			{
				bool flag90 = this._defenderHitStrength == null && createIfNull;
				if (flag90)
				{
					this._defenderHitStrength = new SpecialEffectList();
				}
				result = this._defenderHitStrength;
				break;
			}
			case 91:
			{
				bool flag91 = this._defenderHitTechnique == null && createIfNull;
				if (flag91)
				{
					this._defenderHitTechnique = new SpecialEffectList();
				}
				result = this._defenderHitTechnique;
				break;
			}
			case 92:
			{
				bool flag92 = this._defenderHitSpeed == null && createIfNull;
				if (flag92)
				{
					this._defenderHitSpeed = new SpecialEffectList();
				}
				result = this._defenderHitSpeed;
				break;
			}
			case 93:
			{
				bool flag93 = this._defenderHitMind == null && createIfNull;
				if (flag93)
				{
					this._defenderHitMind = new SpecialEffectList();
				}
				result = this._defenderHitMind;
				break;
			}
			case 94:
			{
				bool flag94 = this._defenderAvoidStrength == null && createIfNull;
				if (flag94)
				{
					this._defenderAvoidStrength = new SpecialEffectList();
				}
				result = this._defenderAvoidStrength;
				break;
			}
			case 95:
			{
				bool flag95 = this._defenderAvoidTechnique == null && createIfNull;
				if (flag95)
				{
					this._defenderAvoidTechnique = new SpecialEffectList();
				}
				result = this._defenderAvoidTechnique;
				break;
			}
			case 96:
			{
				bool flag96 = this._defenderAvoidSpeed == null && createIfNull;
				if (flag96)
				{
					this._defenderAvoidSpeed = new SpecialEffectList();
				}
				result = this._defenderAvoidSpeed;
				break;
			}
			case 97:
			{
				bool flag97 = this._defenderAvoidMind == null && createIfNull;
				if (flag97)
				{
					this._defenderAvoidMind = new SpecialEffectList();
				}
				result = this._defenderAvoidMind;
				break;
			}
			case 98:
			{
				bool flag98 = this._defenderPenetrateOuter == null && createIfNull;
				if (flag98)
				{
					this._defenderPenetrateOuter = new SpecialEffectList();
				}
				result = this._defenderPenetrateOuter;
				break;
			}
			case 99:
			{
				bool flag99 = this._defenderPenetrateInner == null && createIfNull;
				if (flag99)
				{
					this._defenderPenetrateInner = new SpecialEffectList();
				}
				result = this._defenderPenetrateInner;
				break;
			}
			case 100:
			{
				bool flag100 = this._defenderPenetrateResistOuter == null && createIfNull;
				if (flag100)
				{
					this._defenderPenetrateResistOuter = new SpecialEffectList();
				}
				result = this._defenderPenetrateResistOuter;
				break;
			}
			case 101:
			{
				bool flag101 = this._defenderPenetrateResistInner == null && createIfNull;
				if (flag101)
				{
					this._defenderPenetrateResistInner = new SpecialEffectList();
				}
				result = this._defenderPenetrateResistInner;
				break;
			}
			case 102:
			{
				bool flag102 = this._acceptDirectDamage == null && createIfNull;
				if (flag102)
				{
					this._acceptDirectDamage = new SpecialEffectList();
				}
				result = this._acceptDirectDamage;
				break;
			}
			case 103:
			{
				bool flag103 = this._acceptBounceDamage == null && createIfNull;
				if (flag103)
				{
					this._acceptBounceDamage = new SpecialEffectList();
				}
				result = this._acceptBounceDamage;
				break;
			}
			case 104:
			{
				bool flag104 = this._acceptFightBackDamage == null && createIfNull;
				if (flag104)
				{
					this._acceptFightBackDamage = new SpecialEffectList();
				}
				result = this._acceptFightBackDamage;
				break;
			}
			case 105:
			{
				bool flag105 = this._acceptPoisonLevel == null && createIfNull;
				if (flag105)
				{
					this._acceptPoisonLevel = new SpecialEffectList();
				}
				result = this._acceptPoisonLevel;
				break;
			}
			case 106:
			{
				bool flag106 = this._acceptPoisonValue == null && createIfNull;
				if (flag106)
				{
					this._acceptPoisonValue = new SpecialEffectList();
				}
				result = this._acceptPoisonValue;
				break;
			}
			case 107:
			{
				bool flag107 = this._defenderHitOdds == null && createIfNull;
				if (flag107)
				{
					this._defenderHitOdds = new SpecialEffectList();
				}
				result = this._defenderHitOdds;
				break;
			}
			case 108:
			{
				bool flag108 = this._defenderFightBackHitOdds == null && createIfNull;
				if (flag108)
				{
					this._defenderFightBackHitOdds = new SpecialEffectList();
				}
				result = this._defenderFightBackHitOdds;
				break;
			}
			case 109:
			{
				bool flag109 = this._defenderPursueOdds == null && createIfNull;
				if (flag109)
				{
					this._defenderPursueOdds = new SpecialEffectList();
				}
				result = this._defenderPursueOdds;
				break;
			}
			case 110:
			{
				bool flag110 = this._acceptMaxInjuryCount == null && createIfNull;
				if (flag110)
				{
					this._acceptMaxInjuryCount = new SpecialEffectList();
				}
				result = this._acceptMaxInjuryCount;
				break;
			}
			case 111:
			{
				bool flag111 = this._bouncePower == null && createIfNull;
				if (flag111)
				{
					this._bouncePower = new SpecialEffectList();
				}
				result = this._bouncePower;
				break;
			}
			case 112:
			{
				bool flag112 = this._fightBackPower == null && createIfNull;
				if (flag112)
				{
					this._fightBackPower = new SpecialEffectList();
				}
				result = this._fightBackPower;
				break;
			}
			case 113:
			{
				bool flag113 = this._directDamageInnerRatio == null && createIfNull;
				if (flag113)
				{
					this._directDamageInnerRatio = new SpecialEffectList();
				}
				result = this._directDamageInnerRatio;
				break;
			}
			case 114:
			{
				bool flag114 = this._defenderFinalDamageValue == null && createIfNull;
				if (flag114)
				{
					this._defenderFinalDamageValue = new SpecialEffectList();
				}
				result = this._defenderFinalDamageValue;
				break;
			}
			case 115:
			{
				bool flag115 = this._directDamageValue == null && createIfNull;
				if (flag115)
				{
					this._directDamageValue = new SpecialEffectList();
				}
				result = this._directDamageValue;
				break;
			}
			case 116:
			{
				bool flag116 = this._directInjuryMark == null && createIfNull;
				if (flag116)
				{
					this._directInjuryMark = new SpecialEffectList();
				}
				result = this._directInjuryMark;
				break;
			}
			case 117:
			{
				bool flag117 = this._goneMadInjury == null && createIfNull;
				if (flag117)
				{
					this._goneMadInjury = new SpecialEffectList();
				}
				result = this._goneMadInjury;
				break;
			}
			case 118:
			{
				bool flag118 = this._healInjurySpeed == null && createIfNull;
				if (flag118)
				{
					this._healInjurySpeed = new SpecialEffectList();
				}
				result = this._healInjurySpeed;
				break;
			}
			case 119:
			{
				bool flag119 = this._healInjuryBuff == null && createIfNull;
				if (flag119)
				{
					this._healInjuryBuff = new SpecialEffectList();
				}
				result = this._healInjuryBuff;
				break;
			}
			case 120:
			{
				bool flag120 = this._healInjuryDebuff == null && createIfNull;
				if (flag120)
				{
					this._healInjuryDebuff = new SpecialEffectList();
				}
				result = this._healInjuryDebuff;
				break;
			}
			case 121:
			{
				bool flag121 = this._healPoisonSpeed == null && createIfNull;
				if (flag121)
				{
					this._healPoisonSpeed = new SpecialEffectList();
				}
				result = this._healPoisonSpeed;
				break;
			}
			case 122:
			{
				bool flag122 = this._healPoisonBuff == null && createIfNull;
				if (flag122)
				{
					this._healPoisonBuff = new SpecialEffectList();
				}
				result = this._healPoisonBuff;
				break;
			}
			case 123:
			{
				bool flag123 = this._healPoisonDebuff == null && createIfNull;
				if (flag123)
				{
					this._healPoisonDebuff = new SpecialEffectList();
				}
				result = this._healPoisonDebuff;
				break;
			}
			case 124:
			{
				bool flag124 = this._fleeSpeed == null && createIfNull;
				if (flag124)
				{
					this._fleeSpeed = new SpecialEffectList();
				}
				result = this._fleeSpeed;
				break;
			}
			case 125:
			{
				bool flag125 = this._maxFlawCount == null && createIfNull;
				if (flag125)
				{
					this._maxFlawCount = new SpecialEffectList();
				}
				result = this._maxFlawCount;
				break;
			}
			case 126:
			{
				bool flag126 = this._canAddFlaw == null && createIfNull;
				if (flag126)
				{
					this._canAddFlaw = new SpecialEffectList();
				}
				result = this._canAddFlaw;
				break;
			}
			case 127:
			{
				bool flag127 = this._flawLevel == null && createIfNull;
				if (flag127)
				{
					this._flawLevel = new SpecialEffectList();
				}
				result = this._flawLevel;
				break;
			}
			case 128:
			{
				bool flag128 = this._flawLevelCanReduce == null && createIfNull;
				if (flag128)
				{
					this._flawLevelCanReduce = new SpecialEffectList();
				}
				result = this._flawLevelCanReduce;
				break;
			}
			case 129:
			{
				bool flag129 = this._flawCount == null && createIfNull;
				if (flag129)
				{
					this._flawCount = new SpecialEffectList();
				}
				result = this._flawCount;
				break;
			}
			case 130:
			{
				bool flag130 = this._maxAcupointCount == null && createIfNull;
				if (flag130)
				{
					this._maxAcupointCount = new SpecialEffectList();
				}
				result = this._maxAcupointCount;
				break;
			}
			case 131:
			{
				bool flag131 = this._canAddAcupoint == null && createIfNull;
				if (flag131)
				{
					this._canAddAcupoint = new SpecialEffectList();
				}
				result = this._canAddAcupoint;
				break;
			}
			case 132:
			{
				bool flag132 = this._acupointLevel == null && createIfNull;
				if (flag132)
				{
					this._acupointLevel = new SpecialEffectList();
				}
				result = this._acupointLevel;
				break;
			}
			case 133:
			{
				bool flag133 = this._acupointLevelCanReduce == null && createIfNull;
				if (flag133)
				{
					this._acupointLevelCanReduce = new SpecialEffectList();
				}
				result = this._acupointLevelCanReduce;
				break;
			}
			case 134:
			{
				bool flag134 = this._acupointCount == null && createIfNull;
				if (flag134)
				{
					this._acupointCount = new SpecialEffectList();
				}
				result = this._acupointCount;
				break;
			}
			case 135:
			{
				bool flag135 = this._addNeiliAllocation == null && createIfNull;
				if (flag135)
				{
					this._addNeiliAllocation = new SpecialEffectList();
				}
				result = this._addNeiliAllocation;
				break;
			}
			case 136:
			{
				bool flag136 = this._costNeiliAllocation == null && createIfNull;
				if (flag136)
				{
					this._costNeiliAllocation = new SpecialEffectList();
				}
				result = this._costNeiliAllocation;
				break;
			}
			case 137:
			{
				bool flag137 = this._canChangeNeiliAllocation == null && createIfNull;
				if (flag137)
				{
					this._canChangeNeiliAllocation = new SpecialEffectList();
				}
				result = this._canChangeNeiliAllocation;
				break;
			}
			case 138:
			{
				bool flag138 = this._canGetTrick == null && createIfNull;
				if (flag138)
				{
					this._canGetTrick = new SpecialEffectList();
				}
				result = this._canGetTrick;
				break;
			}
			case 139:
			{
				bool flag139 = this._getTrickType == null && createIfNull;
				if (flag139)
				{
					this._getTrickType = new SpecialEffectList();
				}
				result = this._getTrickType;
				break;
			}
			case 140:
			{
				bool flag140 = this._attackBodyPart == null && createIfNull;
				if (flag140)
				{
					this._attackBodyPart = new SpecialEffectList();
				}
				result = this._attackBodyPart;
				break;
			}
			case 141:
			{
				bool flag141 = this._weaponEquipAttack == null && createIfNull;
				if (flag141)
				{
					this._weaponEquipAttack = new SpecialEffectList();
				}
				result = this._weaponEquipAttack;
				break;
			}
			case 142:
			{
				bool flag142 = this._weaponEquipDefense == null && createIfNull;
				if (flag142)
				{
					this._weaponEquipDefense = new SpecialEffectList();
				}
				result = this._weaponEquipDefense;
				break;
			}
			case 143:
			{
				bool flag143 = this._armorEquipAttack == null && createIfNull;
				if (flag143)
				{
					this._armorEquipAttack = new SpecialEffectList();
				}
				result = this._armorEquipAttack;
				break;
			}
			case 144:
			{
				bool flag144 = this._armorEquipDefense == null && createIfNull;
				if (flag144)
				{
					this._armorEquipDefense = new SpecialEffectList();
				}
				result = this._armorEquipDefense;
				break;
			}
			case 145:
			{
				bool flag145 = this._attackRangeForward == null && createIfNull;
				if (flag145)
				{
					this._attackRangeForward = new SpecialEffectList();
				}
				result = this._attackRangeForward;
				break;
			}
			case 146:
			{
				bool flag146 = this._attackRangeBackward == null && createIfNull;
				if (flag146)
				{
					this._attackRangeBackward = new SpecialEffectList();
				}
				result = this._attackRangeBackward;
				break;
			}
			case 147:
			{
				bool flag147 = this._moveCanBeStopped == null && createIfNull;
				if (flag147)
				{
					this._moveCanBeStopped = new SpecialEffectList();
				}
				result = this._moveCanBeStopped;
				break;
			}
			case 148:
			{
				bool flag148 = this._canForcedMove == null && createIfNull;
				if (flag148)
				{
					this._canForcedMove = new SpecialEffectList();
				}
				result = this._canForcedMove;
				break;
			}
			case 149:
			{
				bool flag149 = this._mobilityCanBeRemoved == null && createIfNull;
				if (flag149)
				{
					this._mobilityCanBeRemoved = new SpecialEffectList();
				}
				result = this._mobilityCanBeRemoved;
				break;
			}
			case 150:
			{
				bool flag150 = this._mobilityCostByEffect == null && createIfNull;
				if (flag150)
				{
					this._mobilityCostByEffect = new SpecialEffectList();
				}
				result = this._mobilityCostByEffect;
				break;
			}
			case 151:
			{
				bool flag151 = this._moveDistance == null && createIfNull;
				if (flag151)
				{
					this._moveDistance = new SpecialEffectList();
				}
				result = this._moveDistance;
				break;
			}
			case 152:
			{
				bool flag152 = this._jumpPrepareFrame == null && createIfNull;
				if (flag152)
				{
					this._jumpPrepareFrame = new SpecialEffectList();
				}
				result = this._jumpPrepareFrame;
				break;
			}
			case 153:
			{
				bool flag153 = this._bounceInjuryMark == null && createIfNull;
				if (flag153)
				{
					this._bounceInjuryMark = new SpecialEffectList();
				}
				result = this._bounceInjuryMark;
				break;
			}
			case 154:
			{
				bool flag154 = this._skillHasCost == null && createIfNull;
				if (flag154)
				{
					this._skillHasCost = new SpecialEffectList();
				}
				result = this._skillHasCost;
				break;
			}
			case 155:
			{
				bool flag155 = this._combatStateEffect == null && createIfNull;
				if (flag155)
				{
					this._combatStateEffect = new SpecialEffectList();
				}
				result = this._combatStateEffect;
				break;
			}
			case 156:
			{
				bool flag156 = this._changeNeedUseSkill == null && createIfNull;
				if (flag156)
				{
					this._changeNeedUseSkill = new SpecialEffectList();
				}
				result = this._changeNeedUseSkill;
				break;
			}
			case 157:
			{
				bool flag157 = this._changeDistanceIsMove == null && createIfNull;
				if (flag157)
				{
					this._changeDistanceIsMove = new SpecialEffectList();
				}
				result = this._changeDistanceIsMove;
				break;
			}
			case 158:
			{
				bool flag158 = this._replaceCharHit == null && createIfNull;
				if (flag158)
				{
					this._replaceCharHit = new SpecialEffectList();
				}
				result = this._replaceCharHit;
				break;
			}
			case 159:
			{
				bool flag159 = this._canAddPoison == null && createIfNull;
				if (flag159)
				{
					this._canAddPoison = new SpecialEffectList();
				}
				result = this._canAddPoison;
				break;
			}
			case 160:
			{
				bool flag160 = this._canReducePoison == null && createIfNull;
				if (flag160)
				{
					this._canReducePoison = new SpecialEffectList();
				}
				result = this._canReducePoison;
				break;
			}
			case 161:
			{
				bool flag161 = this._reducePoisonValue == null && createIfNull;
				if (flag161)
				{
					this._reducePoisonValue = new SpecialEffectList();
				}
				result = this._reducePoisonValue;
				break;
			}
			case 162:
			{
				bool flag162 = this._poisonCanAffect == null && createIfNull;
				if (flag162)
				{
					this._poisonCanAffect = new SpecialEffectList();
				}
				result = this._poisonCanAffect;
				break;
			}
			case 163:
			{
				bool flag163 = this._poisonAffectCount == null && createIfNull;
				if (flag163)
				{
					this._poisonAffectCount = new SpecialEffectList();
				}
				result = this._poisonAffectCount;
				break;
			}
			case 164:
			{
				bool flag164 = this._costTricks == null && createIfNull;
				if (flag164)
				{
					this._costTricks = new SpecialEffectList();
				}
				result = this._costTricks;
				break;
			}
			case 165:
			{
				bool flag165 = this._jumpMoveDistance == null && createIfNull;
				if (flag165)
				{
					this._jumpMoveDistance = new SpecialEffectList();
				}
				result = this._jumpMoveDistance;
				break;
			}
			case 166:
			{
				bool flag166 = this._combatStateToAdd == null && createIfNull;
				if (flag166)
				{
					this._combatStateToAdd = new SpecialEffectList();
				}
				result = this._combatStateToAdd;
				break;
			}
			case 167:
			{
				bool flag167 = this._combatStatePower == null && createIfNull;
				if (flag167)
				{
					this._combatStatePower = new SpecialEffectList();
				}
				result = this._combatStatePower;
				break;
			}
			case 168:
			{
				bool flag168 = this._breakBodyPartInjuryCount == null && createIfNull;
				if (flag168)
				{
					this._breakBodyPartInjuryCount = new SpecialEffectList();
				}
				result = this._breakBodyPartInjuryCount;
				break;
			}
			case 169:
			{
				bool flag169 = this._bodyPartIsBroken == null && createIfNull;
				if (flag169)
				{
					this._bodyPartIsBroken = new SpecialEffectList();
				}
				result = this._bodyPartIsBroken;
				break;
			}
			case 170:
			{
				bool flag170 = this._maxTrickCount == null && createIfNull;
				if (flag170)
				{
					this._maxTrickCount = new SpecialEffectList();
				}
				result = this._maxTrickCount;
				break;
			}
			case 171:
			{
				bool flag171 = this._maxBreathPercent == null && createIfNull;
				if (flag171)
				{
					this._maxBreathPercent = new SpecialEffectList();
				}
				result = this._maxBreathPercent;
				break;
			}
			case 172:
			{
				bool flag172 = this._maxStancePercent == null && createIfNull;
				if (flag172)
				{
					this._maxStancePercent = new SpecialEffectList();
				}
				result = this._maxStancePercent;
				break;
			}
			case 173:
			{
				bool flag173 = this._extraBreathPercent == null && createIfNull;
				if (flag173)
				{
					this._extraBreathPercent = new SpecialEffectList();
				}
				result = this._extraBreathPercent;
				break;
			}
			case 174:
			{
				bool flag174 = this._extraStancePercent == null && createIfNull;
				if (flag174)
				{
					this._extraStancePercent = new SpecialEffectList();
				}
				result = this._extraStancePercent;
				break;
			}
			case 175:
			{
				bool flag175 = this._moveCostMobility == null && createIfNull;
				if (flag175)
				{
					this._moveCostMobility = new SpecialEffectList();
				}
				result = this._moveCostMobility;
				break;
			}
			case 176:
			{
				bool flag176 = this._defendSkillKeepTime == null && createIfNull;
				if (flag176)
				{
					this._defendSkillKeepTime = new SpecialEffectList();
				}
				result = this._defendSkillKeepTime;
				break;
			}
			case 177:
			{
				bool flag177 = this._bounceRange == null && createIfNull;
				if (flag177)
				{
					this._bounceRange = new SpecialEffectList();
				}
				result = this._bounceRange;
				break;
			}
			case 178:
			{
				bool flag178 = this._mindMarkKeepTime == null && createIfNull;
				if (flag178)
				{
					this._mindMarkKeepTime = new SpecialEffectList();
				}
				result = this._mindMarkKeepTime;
				break;
			}
			case 179:
			{
				bool flag179 = this._skillMobilityCostPerFrame == null && createIfNull;
				if (flag179)
				{
					this._skillMobilityCostPerFrame = new SpecialEffectList();
				}
				result = this._skillMobilityCostPerFrame;
				break;
			}
			case 180:
			{
				bool flag180 = this._canAddWug == null && createIfNull;
				if (flag180)
				{
					this._canAddWug = new SpecialEffectList();
				}
				result = this._canAddWug;
				break;
			}
			case 181:
			{
				bool flag181 = this._hasGodWeaponBuff == null && createIfNull;
				if (flag181)
				{
					this._hasGodWeaponBuff = new SpecialEffectList();
				}
				result = this._hasGodWeaponBuff;
				break;
			}
			case 182:
			{
				bool flag182 = this._hasGodArmorBuff == null && createIfNull;
				if (flag182)
				{
					this._hasGodArmorBuff = new SpecialEffectList();
				}
				result = this._hasGodArmorBuff;
				break;
			}
			case 183:
			{
				bool flag183 = this._teammateCmdRequireGenerateValue == null && createIfNull;
				if (flag183)
				{
					this._teammateCmdRequireGenerateValue = new SpecialEffectList();
				}
				result = this._teammateCmdRequireGenerateValue;
				break;
			}
			case 184:
			{
				bool flag184 = this._teammateCmdEffect == null && createIfNull;
				if (flag184)
				{
					this._teammateCmdEffect = new SpecialEffectList();
				}
				result = this._teammateCmdEffect;
				break;
			}
			case 185:
			{
				bool flag185 = this._flawRecoverSpeed == null && createIfNull;
				if (flag185)
				{
					this._flawRecoverSpeed = new SpecialEffectList();
				}
				result = this._flawRecoverSpeed;
				break;
			}
			case 186:
			{
				bool flag186 = this._acupointRecoverSpeed == null && createIfNull;
				if (flag186)
				{
					this._acupointRecoverSpeed = new SpecialEffectList();
				}
				result = this._acupointRecoverSpeed;
				break;
			}
			case 187:
			{
				bool flag187 = this._mindMarkRecoverSpeed == null && createIfNull;
				if (flag187)
				{
					this._mindMarkRecoverSpeed = new SpecialEffectList();
				}
				result = this._mindMarkRecoverSpeed;
				break;
			}
			case 188:
			{
				bool flag188 = this._injuryAutoHealSpeed == null && createIfNull;
				if (flag188)
				{
					this._injuryAutoHealSpeed = new SpecialEffectList();
				}
				result = this._injuryAutoHealSpeed;
				break;
			}
			case 189:
			{
				bool flag189 = this._canRecoverBreath == null && createIfNull;
				if (flag189)
				{
					this._canRecoverBreath = new SpecialEffectList();
				}
				result = this._canRecoverBreath;
				break;
			}
			case 190:
			{
				bool flag190 = this._canRecoverStance == null && createIfNull;
				if (flag190)
				{
					this._canRecoverStance = new SpecialEffectList();
				}
				result = this._canRecoverStance;
				break;
			}
			case 191:
			{
				bool flag191 = this._fatalDamageValue == null && createIfNull;
				if (flag191)
				{
					this._fatalDamageValue = new SpecialEffectList();
				}
				result = this._fatalDamageValue;
				break;
			}
			case 192:
			{
				bool flag192 = this._fatalDamageMarkCount == null && createIfNull;
				if (flag192)
				{
					this._fatalDamageMarkCount = new SpecialEffectList();
				}
				result = this._fatalDamageMarkCount;
				break;
			}
			case 193:
			{
				bool flag193 = this._canFightBackDuringPrepareSkill == null && createIfNull;
				if (flag193)
				{
					this._canFightBackDuringPrepareSkill = new SpecialEffectList();
				}
				result = this._canFightBackDuringPrepareSkill;
				break;
			}
			case 194:
			{
				bool flag194 = this._skillPrepareSpeed == null && createIfNull;
				if (flag194)
				{
					this._skillPrepareSpeed = new SpecialEffectList();
				}
				result = this._skillPrepareSpeed;
				break;
			}
			case 195:
			{
				bool flag195 = this._breathRecoverSpeed == null && createIfNull;
				if (flag195)
				{
					this._breathRecoverSpeed = new SpecialEffectList();
				}
				result = this._breathRecoverSpeed;
				break;
			}
			case 196:
			{
				bool flag196 = this._stanceRecoverSpeed == null && createIfNull;
				if (flag196)
				{
					this._stanceRecoverSpeed = new SpecialEffectList();
				}
				result = this._stanceRecoverSpeed;
				break;
			}
			case 197:
			{
				bool flag197 = this._mobilityRecoverSpeed == null && createIfNull;
				if (flag197)
				{
					this._mobilityRecoverSpeed = new SpecialEffectList();
				}
				result = this._mobilityRecoverSpeed;
				break;
			}
			case 198:
			{
				bool flag198 = this._changeTrickProgressAddValue == null && createIfNull;
				if (flag198)
				{
					this._changeTrickProgressAddValue = new SpecialEffectList();
				}
				result = this._changeTrickProgressAddValue;
				break;
			}
			case 199:
			{
				bool flag199 = this._power == null && createIfNull;
				if (flag199)
				{
					this._power = new SpecialEffectList();
				}
				result = this._power;
				break;
			}
			case 200:
			{
				bool flag200 = this._maxPower == null && createIfNull;
				if (flag200)
				{
					this._maxPower = new SpecialEffectList();
				}
				result = this._maxPower;
				break;
			}
			case 201:
			{
				bool flag201 = this._powerCanReduce == null && createIfNull;
				if (flag201)
				{
					this._powerCanReduce = new SpecialEffectList();
				}
				result = this._powerCanReduce;
				break;
			}
			case 202:
			{
				bool flag202 = this._useRequirement == null && createIfNull;
				if (flag202)
				{
					this._useRequirement = new SpecialEffectList();
				}
				result = this._useRequirement;
				break;
			}
			case 203:
			{
				bool flag203 = this._currInnerRatio == null && createIfNull;
				if (flag203)
				{
					this._currInnerRatio = new SpecialEffectList();
				}
				result = this._currInnerRatio;
				break;
			}
			case 204:
			{
				bool flag204 = this._costBreathAndStance == null && createIfNull;
				if (flag204)
				{
					this._costBreathAndStance = new SpecialEffectList();
				}
				result = this._costBreathAndStance;
				break;
			}
			case 205:
			{
				bool flag205 = this._costBreath == null && createIfNull;
				if (flag205)
				{
					this._costBreath = new SpecialEffectList();
				}
				result = this._costBreath;
				break;
			}
			case 206:
			{
				bool flag206 = this._costStance == null && createIfNull;
				if (flag206)
				{
					this._costStance = new SpecialEffectList();
				}
				result = this._costStance;
				break;
			}
			case 207:
			{
				bool flag207 = this._costMobility == null && createIfNull;
				if (flag207)
				{
					this._costMobility = new SpecialEffectList();
				}
				result = this._costMobility;
				break;
			}
			case 208:
			{
				bool flag208 = this._skillCostTricks == null && createIfNull;
				if (flag208)
				{
					this._skillCostTricks = new SpecialEffectList();
				}
				result = this._skillCostTricks;
				break;
			}
			case 209:
			{
				bool flag209 = this._effectDirection == null && createIfNull;
				if (flag209)
				{
					this._effectDirection = new SpecialEffectList();
				}
				result = this._effectDirection;
				break;
			}
			case 210:
			{
				bool flag210 = this._effectDirectionCanChange == null && createIfNull;
				if (flag210)
				{
					this._effectDirectionCanChange = new SpecialEffectList();
				}
				result = this._effectDirectionCanChange;
				break;
			}
			case 211:
			{
				bool flag211 = this._gridCost == null && createIfNull;
				if (flag211)
				{
					this._gridCost = new SpecialEffectList();
				}
				result = this._gridCost;
				break;
			}
			case 212:
			{
				bool flag212 = this._prepareTotalProgress == null && createIfNull;
				if (flag212)
				{
					this._prepareTotalProgress = new SpecialEffectList();
				}
				result = this._prepareTotalProgress;
				break;
			}
			case 213:
			{
				bool flag213 = this._specificGridCount == null && createIfNull;
				if (flag213)
				{
					this._specificGridCount = new SpecialEffectList();
				}
				result = this._specificGridCount;
				break;
			}
			case 214:
			{
				bool flag214 = this._genericGridCount == null && createIfNull;
				if (flag214)
				{
					this._genericGridCount = new SpecialEffectList();
				}
				result = this._genericGridCount;
				break;
			}
			case 215:
			{
				bool flag215 = this._canInterrupt == null && createIfNull;
				if (flag215)
				{
					this._canInterrupt = new SpecialEffectList();
				}
				result = this._canInterrupt;
				break;
			}
			case 216:
			{
				bool flag216 = this._interruptOdds == null && createIfNull;
				if (flag216)
				{
					this._interruptOdds = new SpecialEffectList();
				}
				result = this._interruptOdds;
				break;
			}
			case 217:
			{
				bool flag217 = this._canSilence == null && createIfNull;
				if (flag217)
				{
					this._canSilence = new SpecialEffectList();
				}
				result = this._canSilence;
				break;
			}
			case 218:
			{
				bool flag218 = this._silenceOdds == null && createIfNull;
				if (flag218)
				{
					this._silenceOdds = new SpecialEffectList();
				}
				result = this._silenceOdds;
				break;
			}
			case 219:
			{
				bool flag219 = this._canCastWithBrokenBodyPart == null && createIfNull;
				if (flag219)
				{
					this._canCastWithBrokenBodyPart = new SpecialEffectList();
				}
				result = this._canCastWithBrokenBodyPart;
				break;
			}
			case 220:
			{
				bool flag220 = this._addPowerCanBeRemoved == null && createIfNull;
				if (flag220)
				{
					this._addPowerCanBeRemoved = new SpecialEffectList();
				}
				result = this._addPowerCanBeRemoved;
				break;
			}
			case 221:
			{
				bool flag221 = this._skillType == null && createIfNull;
				if (flag221)
				{
					this._skillType = new SpecialEffectList();
				}
				result = this._skillType;
				break;
			}
			case 222:
			{
				bool flag222 = this._effectCountCanChange == null && createIfNull;
				if (flag222)
				{
					this._effectCountCanChange = new SpecialEffectList();
				}
				result = this._effectCountCanChange;
				break;
			}
			case 223:
			{
				bool flag223 = this._canCastInDefend == null && createIfNull;
				if (flag223)
				{
					this._canCastInDefend = new SpecialEffectList();
				}
				result = this._canCastInDefend;
				break;
			}
			case 224:
			{
				bool flag224 = this._hitDistribution == null && createIfNull;
				if (flag224)
				{
					this._hitDistribution = new SpecialEffectList();
				}
				result = this._hitDistribution;
				break;
			}
			case 225:
			{
				bool flag225 = this._canCastOnLackBreath == null && createIfNull;
				if (flag225)
				{
					this._canCastOnLackBreath = new SpecialEffectList();
				}
				result = this._canCastOnLackBreath;
				break;
			}
			case 226:
			{
				bool flag226 = this._canCastOnLackStance == null && createIfNull;
				if (flag226)
				{
					this._canCastOnLackStance = new SpecialEffectList();
				}
				result = this._canCastOnLackStance;
				break;
			}
			case 227:
			{
				bool flag227 = this._costBreathOnCast == null && createIfNull;
				if (flag227)
				{
					this._costBreathOnCast = new SpecialEffectList();
				}
				result = this._costBreathOnCast;
				break;
			}
			case 228:
			{
				bool flag228 = this._costStanceOnCast == null && createIfNull;
				if (flag228)
				{
					this._costStanceOnCast = new SpecialEffectList();
				}
				result = this._costStanceOnCast;
				break;
			}
			case 229:
			{
				bool flag229 = this._canUseMobilityAsBreath == null && createIfNull;
				if (flag229)
				{
					this._canUseMobilityAsBreath = new SpecialEffectList();
				}
				result = this._canUseMobilityAsBreath;
				break;
			}
			case 230:
			{
				bool flag230 = this._canUseMobilityAsStance == null && createIfNull;
				if (flag230)
				{
					this._canUseMobilityAsStance = new SpecialEffectList();
				}
				result = this._canUseMobilityAsStance;
				break;
			}
			case 231:
			{
				bool flag231 = this._castCostNeiliAllocation == null && createIfNull;
				if (flag231)
				{
					this._castCostNeiliAllocation = new SpecialEffectList();
				}
				result = this._castCostNeiliAllocation;
				break;
			}
			case 232:
			{
				bool flag232 = this._acceptPoisonResist == null && createIfNull;
				if (flag232)
				{
					this._acceptPoisonResist = new SpecialEffectList();
				}
				result = this._acceptPoisonResist;
				break;
			}
			case 233:
			{
				bool flag233 = this._makePoisonResist == null && createIfNull;
				if (flag233)
				{
					this._makePoisonResist = new SpecialEffectList();
				}
				result = this._makePoisonResist;
				break;
			}
			case 234:
			{
				bool flag234 = this._canCriticalHit == null && createIfNull;
				if (flag234)
				{
					this._canCriticalHit = new SpecialEffectList();
				}
				result = this._canCriticalHit;
				break;
			}
			case 235:
			{
				bool flag235 = this._canCostNeiliAllocationEffect == null && createIfNull;
				if (flag235)
				{
					this._canCostNeiliAllocationEffect = new SpecialEffectList();
				}
				result = this._canCostNeiliAllocationEffect;
				break;
			}
			case 236:
			{
				bool flag236 = this._consummateLevelRelatedMainAttributesHitValues == null && createIfNull;
				if (flag236)
				{
					this._consummateLevelRelatedMainAttributesHitValues = new SpecialEffectList();
				}
				result = this._consummateLevelRelatedMainAttributesHitValues;
				break;
			}
			case 237:
			{
				bool flag237 = this._consummateLevelRelatedMainAttributesAvoidValues == null && createIfNull;
				if (flag237)
				{
					this._consummateLevelRelatedMainAttributesAvoidValues = new SpecialEffectList();
				}
				result = this._consummateLevelRelatedMainAttributesAvoidValues;
				break;
			}
			case 238:
			{
				bool flag238 = this._consummateLevelRelatedMainAttributesPenetrations == null && createIfNull;
				if (flag238)
				{
					this._consummateLevelRelatedMainAttributesPenetrations = new SpecialEffectList();
				}
				result = this._consummateLevelRelatedMainAttributesPenetrations;
				break;
			}
			case 239:
			{
				bool flag239 = this._consummateLevelRelatedMainAttributesPenetrationResists == null && createIfNull;
				if (flag239)
				{
					this._consummateLevelRelatedMainAttributesPenetrationResists = new SpecialEffectList();
				}
				result = this._consummateLevelRelatedMainAttributesPenetrationResists;
				break;
			}
			case 240:
			{
				bool flag240 = this._skillAlsoAsFiveElements == null && createIfNull;
				if (flag240)
				{
					this._skillAlsoAsFiveElements = new SpecialEffectList();
				}
				result = this._skillAlsoAsFiveElements;
				break;
			}
			case 241:
			{
				bool flag241 = this._innerInjuryImmunity == null && createIfNull;
				if (flag241)
				{
					this._innerInjuryImmunity = new SpecialEffectList();
				}
				result = this._innerInjuryImmunity;
				break;
			}
			case 242:
			{
				bool flag242 = this._outerInjuryImmunity == null && createIfNull;
				if (flag242)
				{
					this._outerInjuryImmunity = new SpecialEffectList();
				}
				result = this._outerInjuryImmunity;
				break;
			}
			case 243:
			{
				bool flag243 = this._poisonAffectThreshold == null && createIfNull;
				if (flag243)
				{
					this._poisonAffectThreshold = new SpecialEffectList();
				}
				result = this._poisonAffectThreshold;
				break;
			}
			case 244:
			{
				bool flag244 = this._lockDistance == null && createIfNull;
				if (flag244)
				{
					this._lockDistance = new SpecialEffectList();
				}
				result = this._lockDistance;
				break;
			}
			case 245:
			{
				bool flag245 = this._resistOfAllPoison == null && createIfNull;
				if (flag245)
				{
					this._resistOfAllPoison = new SpecialEffectList();
				}
				result = this._resistOfAllPoison;
				break;
			}
			case 246:
			{
				bool flag246 = this._makePoisonTarget == null && createIfNull;
				if (flag246)
				{
					this._makePoisonTarget = new SpecialEffectList();
				}
				result = this._makePoisonTarget;
				break;
			}
			case 247:
			{
				bool flag247 = this._acceptPoisonTarget == null && createIfNull;
				if (flag247)
				{
					this._acceptPoisonTarget = new SpecialEffectList();
				}
				result = this._acceptPoisonTarget;
				break;
			}
			case 248:
			{
				bool flag248 = this._certainCriticalHit == null && createIfNull;
				if (flag248)
				{
					this._certainCriticalHit = new SpecialEffectList();
				}
				result = this._certainCriticalHit;
				break;
			}
			case 249:
			{
				bool flag249 = this._mindMarkCount == null && createIfNull;
				if (flag249)
				{
					this._mindMarkCount = new SpecialEffectList();
				}
				result = this._mindMarkCount;
				break;
			}
			case 250:
			{
				bool flag250 = this._canFightBackWithHit == null && createIfNull;
				if (flag250)
				{
					this._canFightBackWithHit = new SpecialEffectList();
				}
				result = this._canFightBackWithHit;
				break;
			}
			case 251:
			{
				bool flag251 = this._inevitableHit == null && createIfNull;
				if (flag251)
				{
					this._inevitableHit = new SpecialEffectList();
				}
				result = this._inevitableHit;
				break;
			}
			case 252:
			{
				bool flag252 = this._attackCanPursue == null && createIfNull;
				if (flag252)
				{
					this._attackCanPursue = new SpecialEffectList();
				}
				result = this._attackCanPursue;
				break;
			}
			case 253:
			{
				bool flag253 = this._combatSkillDataEffectList == null && createIfNull;
				if (flag253)
				{
					this._combatSkillDataEffectList = new SpecialEffectList();
				}
				result = this._combatSkillDataEffectList;
				break;
			}
			case 254:
			{
				bool flag254 = this._criticalOdds == null && createIfNull;
				if (flag254)
				{
					this._criticalOdds = new SpecialEffectList();
				}
				result = this._criticalOdds;
				break;
			}
			case 255:
			{
				bool flag255 = this._stanceCostByEffect == null && createIfNull;
				if (flag255)
				{
					this._stanceCostByEffect = new SpecialEffectList();
				}
				result = this._stanceCostByEffect;
				break;
			}
			case 256:
			{
				bool flag256 = this._breathCostByEffect == null && createIfNull;
				if (flag256)
				{
					this._breathCostByEffect = new SpecialEffectList();
				}
				result = this._breathCostByEffect;
				break;
			}
			case 257:
			{
				bool flag257 = this._powerAddRatio == null && createIfNull;
				if (flag257)
				{
					this._powerAddRatio = new SpecialEffectList();
				}
				result = this._powerAddRatio;
				break;
			}
			case 258:
			{
				bool flag258 = this._powerReduceRatio == null && createIfNull;
				if (flag258)
				{
					this._powerReduceRatio = new SpecialEffectList();
				}
				result = this._powerReduceRatio;
				break;
			}
			case 259:
			{
				bool flag259 = this._poisonAffectProduceValue == null && createIfNull;
				if (flag259)
				{
					this._poisonAffectProduceValue = new SpecialEffectList();
				}
				result = this._poisonAffectProduceValue;
				break;
			}
			case 260:
			{
				bool flag260 = this._canReadingOnMonthChange == null && createIfNull;
				if (flag260)
				{
					this._canReadingOnMonthChange = new SpecialEffectList();
				}
				result = this._canReadingOnMonthChange;
				break;
			}
			case 261:
			{
				bool flag261 = this._medicineEffect == null && createIfNull;
				if (flag261)
				{
					this._medicineEffect = new SpecialEffectList();
				}
				result = this._medicineEffect;
				break;
			}
			case 262:
			{
				bool flag262 = this._xiangshuInfectionDelta == null && createIfNull;
				if (flag262)
				{
					this._xiangshuInfectionDelta = new SpecialEffectList();
				}
				result = this._xiangshuInfectionDelta;
				break;
			}
			case 263:
			{
				bool flag263 = this._healthDelta == null && createIfNull;
				if (flag263)
				{
					this._healthDelta = new SpecialEffectList();
				}
				result = this._healthDelta;
				break;
			}
			case 264:
			{
				bool flag264 = this._weaponSilenceFrame == null && createIfNull;
				if (flag264)
				{
					this._weaponSilenceFrame = new SpecialEffectList();
				}
				result = this._weaponSilenceFrame;
				break;
			}
			case 265:
			{
				bool flag265 = this._silenceFrame == null && createIfNull;
				if (flag265)
				{
					this._silenceFrame = new SpecialEffectList();
				}
				result = this._silenceFrame;
				break;
			}
			case 266:
			{
				bool flag266 = this._currAgeDelta == null && createIfNull;
				if (flag266)
				{
					this._currAgeDelta = new SpecialEffectList();
				}
				result = this._currAgeDelta;
				break;
			}
			case 267:
			{
				bool flag267 = this._goneMadInAllBreak == null && createIfNull;
				if (flag267)
				{
					this._goneMadInAllBreak = new SpecialEffectList();
				}
				result = this._goneMadInAllBreak;
				break;
			}
			case 268:
			{
				bool flag268 = this._makeLoveRateOnMonthChange == null && createIfNull;
				if (flag268)
				{
					this._makeLoveRateOnMonthChange = new SpecialEffectList();
				}
				result = this._makeLoveRateOnMonthChange;
				break;
			}
			case 269:
			{
				bool flag269 = this._canAutoHealOnMonthChange == null && createIfNull;
				if (flag269)
				{
					this._canAutoHealOnMonthChange = new SpecialEffectList();
				}
				result = this._canAutoHealOnMonthChange;
				break;
			}
			case 270:
			{
				bool flag270 = this._happinessDelta == null && createIfNull;
				if (flag270)
				{
					this._happinessDelta = new SpecialEffectList();
				}
				result = this._happinessDelta;
				break;
			}
			case 271:
			{
				bool flag271 = this._teammateCmdCanUse == null && createIfNull;
				if (flag271)
				{
					this._teammateCmdCanUse = new SpecialEffectList();
				}
				result = this._teammateCmdCanUse;
				break;
			}
			case 272:
			{
				bool flag272 = this._mixPoisonInfinityAffect == null && createIfNull;
				if (flag272)
				{
					this._mixPoisonInfinityAffect = new SpecialEffectList();
				}
				result = this._mixPoisonInfinityAffect;
				break;
			}
			case 273:
			{
				bool flag273 = this._attackRangeMaxAcupoint == null && createIfNull;
				if (flag273)
				{
					this._attackRangeMaxAcupoint = new SpecialEffectList();
				}
				result = this._attackRangeMaxAcupoint;
				break;
			}
			case 274:
			{
				bool flag274 = this._maxMobilityPercent == null && createIfNull;
				if (flag274)
				{
					this._maxMobilityPercent = new SpecialEffectList();
				}
				result = this._maxMobilityPercent;
				break;
			}
			case 275:
			{
				bool flag275 = this._makeMindDamage == null && createIfNull;
				if (flag275)
				{
					this._makeMindDamage = new SpecialEffectList();
				}
				result = this._makeMindDamage;
				break;
			}
			case 276:
			{
				bool flag276 = this._acceptMindDamage == null && createIfNull;
				if (flag276)
				{
					this._acceptMindDamage = new SpecialEffectList();
				}
				result = this._acceptMindDamage;
				break;
			}
			case 277:
			{
				bool flag277 = this._hitAddByTempValue == null && createIfNull;
				if (flag277)
				{
					this._hitAddByTempValue = new SpecialEffectList();
				}
				result = this._hitAddByTempValue;
				break;
			}
			case 278:
			{
				bool flag278 = this._avoidAddByTempValue == null && createIfNull;
				if (flag278)
				{
					this._avoidAddByTempValue = new SpecialEffectList();
				}
				result = this._avoidAddByTempValue;
				break;
			}
			case 279:
			{
				bool flag279 = this._ignoreEquipmentOverload == null && createIfNull;
				if (flag279)
				{
					this._ignoreEquipmentOverload = new SpecialEffectList();
				}
				result = this._ignoreEquipmentOverload;
				break;
			}
			case 280:
			{
				bool flag280 = this._canCostEnemyUsableTricks == null && createIfNull;
				if (flag280)
				{
					this._canCostEnemyUsableTricks = new SpecialEffectList();
				}
				result = this._canCostEnemyUsableTricks;
				break;
			}
			case 281:
			{
				bool flag281 = this._ignoreArmor == null && createIfNull;
				if (flag281)
				{
					this._ignoreArmor = new SpecialEffectList();
				}
				result = this._ignoreArmor;
				break;
			}
			case 282:
			{
				bool flag282 = this._unyieldingFallen == null && createIfNull;
				if (flag282)
				{
					this._unyieldingFallen = new SpecialEffectList();
				}
				result = this._unyieldingFallen;
				break;
			}
			case 283:
			{
				bool flag283 = this._normalAttackPrepareFrame == null && createIfNull;
				if (flag283)
				{
					this._normalAttackPrepareFrame = new SpecialEffectList();
				}
				result = this._normalAttackPrepareFrame;
				break;
			}
			case 284:
			{
				bool flag284 = this._canCostUselessTricks == null && createIfNull;
				if (flag284)
				{
					this._canCostUselessTricks = new SpecialEffectList();
				}
				result = this._canCostUselessTricks;
				break;
			}
			case 285:
			{
				bool flag285 = this._defendSkillCanAffect == null && createIfNull;
				if (flag285)
				{
					this._defendSkillCanAffect = new SpecialEffectList();
				}
				result = this._defendSkillCanAffect;
				break;
			}
			case 286:
			{
				bool flag286 = this._assistSkillCanAffect == null && createIfNull;
				if (flag286)
				{
					this._assistSkillCanAffect = new SpecialEffectList();
				}
				result = this._assistSkillCanAffect;
				break;
			}
			case 287:
			{
				bool flag287 = this._agileSkillCanAffect == null && createIfNull;
				if (flag287)
				{
					this._agileSkillCanAffect = new SpecialEffectList();
				}
				result = this._agileSkillCanAffect;
				break;
			}
			case 288:
			{
				bool flag288 = this._allMarkChangeToMind == null && createIfNull;
				if (flag288)
				{
					this._allMarkChangeToMind = new SpecialEffectList();
				}
				result = this._allMarkChangeToMind;
				break;
			}
			case 289:
			{
				bool flag289 = this._mindMarkChangeToFatal == null && createIfNull;
				if (flag289)
				{
					this._mindMarkChangeToFatal = new SpecialEffectList();
				}
				result = this._mindMarkChangeToFatal;
				break;
			}
			case 290:
			{
				bool flag290 = this._canCast == null && createIfNull;
				if (flag290)
				{
					this._canCast = new SpecialEffectList();
				}
				result = this._canCast;
				break;
			}
			case 291:
			{
				bool flag291 = this._inevitableAvoid == null && createIfNull;
				if (flag291)
				{
					this._inevitableAvoid = new SpecialEffectList();
				}
				result = this._inevitableAvoid;
				break;
			}
			case 292:
			{
				bool flag292 = this._powerEffectReverse == null && createIfNull;
				if (flag292)
				{
					this._powerEffectReverse = new SpecialEffectList();
				}
				result = this._powerEffectReverse;
				break;
			}
			case 293:
			{
				bool flag293 = this._featureBonusReverse == null && createIfNull;
				if (flag293)
				{
					this._featureBonusReverse = new SpecialEffectList();
				}
				result = this._featureBonusReverse;
				break;
			}
			case 294:
			{
				bool flag294 = this._wugFatalDamageValue == null && createIfNull;
				if (flag294)
				{
					this._wugFatalDamageValue = new SpecialEffectList();
				}
				result = this._wugFatalDamageValue;
				break;
			}
			case 295:
			{
				bool flag295 = this._canRecoverHealthOnMonthChange == null && createIfNull;
				if (flag295)
				{
					this._canRecoverHealthOnMonthChange = new SpecialEffectList();
				}
				result = this._canRecoverHealthOnMonthChange;
				break;
			}
			case 296:
			{
				bool flag296 = this._takeRevengeRateOnMonthChange == null && createIfNull;
				if (flag296)
				{
					this._takeRevengeRateOnMonthChange = new SpecialEffectList();
				}
				result = this._takeRevengeRateOnMonthChange;
				break;
			}
			case 297:
			{
				bool flag297 = this._consummateLevelBonus == null && createIfNull;
				if (flag297)
				{
					this._consummateLevelBonus = new SpecialEffectList();
				}
				result = this._consummateLevelBonus;
				break;
			}
			case 298:
			{
				bool flag298 = this._neiliDelta == null && createIfNull;
				if (flag298)
				{
					this._neiliDelta = new SpecialEffectList();
				}
				result = this._neiliDelta;
				break;
			}
			case 299:
			{
				bool flag299 = this._canMakeLoveSpecialOnMonthChange == null && createIfNull;
				if (flag299)
				{
					this._canMakeLoveSpecialOnMonthChange = new SpecialEffectList();
				}
				result = this._canMakeLoveSpecialOnMonthChange;
				break;
			}
			case 300:
			{
				bool flag300 = this._healAcupointSpeed == null && createIfNull;
				if (flag300)
				{
					this._healAcupointSpeed = new SpecialEffectList();
				}
				result = this._healAcupointSpeed;
				break;
			}
			case 301:
			{
				bool flag301 = this._maxChangeTrickCount == null && createIfNull;
				if (flag301)
				{
					this._maxChangeTrickCount = new SpecialEffectList();
				}
				result = this._maxChangeTrickCount;
				break;
			}
			case 302:
			{
				bool flag302 = this._convertCostBreathAndStance == null && createIfNull;
				if (flag302)
				{
					this._convertCostBreathAndStance = new SpecialEffectList();
				}
				result = this._convertCostBreathAndStance;
				break;
			}
			case 303:
			{
				bool flag303 = this._personalitiesAll == null && createIfNull;
				if (flag303)
				{
					this._personalitiesAll = new SpecialEffectList();
				}
				result = this._personalitiesAll;
				break;
			}
			case 304:
			{
				bool flag304 = this._finalFatalDamageMarkCount == null && createIfNull;
				if (flag304)
				{
					this._finalFatalDamageMarkCount = new SpecialEffectList();
				}
				result = this._finalFatalDamageMarkCount;
				break;
			}
			case 305:
			{
				bool flag305 = this._infinityMindMarkProgress == null && createIfNull;
				if (flag305)
				{
					this._infinityMindMarkProgress = new SpecialEffectList();
				}
				result = this._infinityMindMarkProgress;
				break;
			}
			case 306:
			{
				bool flag306 = this._combatSkillAiScorePower == null && createIfNull;
				if (flag306)
				{
					this._combatSkillAiScorePower = new SpecialEffectList();
				}
				result = this._combatSkillAiScorePower;
				break;
			}
			case 307:
			{
				bool flag307 = this._normalAttackChangeToUnlockAttack == null && createIfNull;
				if (flag307)
				{
					this._normalAttackChangeToUnlockAttack = new SpecialEffectList();
				}
				result = this._normalAttackChangeToUnlockAttack;
				break;
			}
			case 308:
			{
				bool flag308 = this._attackBodyPartOdds == null && createIfNull;
				if (flag308)
				{
					this._attackBodyPartOdds = new SpecialEffectList();
				}
				result = this._attackBodyPartOdds;
				break;
			}
			case 309:
			{
				bool flag309 = this._changeDurability == null && createIfNull;
				if (flag309)
				{
					this._changeDurability = new SpecialEffectList();
				}
				result = this._changeDurability;
				break;
			}
			case 310:
			{
				bool flag310 = this._equipmentBonus == null && createIfNull;
				if (flag310)
				{
					this._equipmentBonus = new SpecialEffectList();
				}
				result = this._equipmentBonus;
				break;
			}
			case 311:
			{
				bool flag311 = this._equipmentWeight == null && createIfNull;
				if (flag311)
				{
					this._equipmentWeight = new SpecialEffectList();
				}
				result = this._equipmentWeight;
				break;
			}
			case 312:
			{
				bool flag312 = this._rawCreateEffectList == null && createIfNull;
				if (flag312)
				{
					this._rawCreateEffectList = new SpecialEffectList();
				}
				result = this._rawCreateEffectList;
				break;
			}
			case 313:
			{
				bool flag313 = this._jiTrickAsWeaponTrickCount == null && createIfNull;
				if (flag313)
				{
					this._jiTrickAsWeaponTrickCount = new SpecialEffectList();
				}
				result = this._jiTrickAsWeaponTrickCount;
				break;
			}
			case 314:
			{
				bool flag314 = this._uselessTrickAsJiTrickCount == null && createIfNull;
				if (flag314)
				{
					this._uselessTrickAsJiTrickCount = new SpecialEffectList();
				}
				result = this._uselessTrickAsJiTrickCount;
				break;
			}
			case 315:
			{
				bool flag315 = this._equipmentPower == null && createIfNull;
				if (flag315)
				{
					this._equipmentPower = new SpecialEffectList();
				}
				result = this._equipmentPower;
				break;
			}
			case 316:
			{
				bool flag316 = this._healFlawSpeed == null && createIfNull;
				if (flag316)
				{
					this._healFlawSpeed = new SpecialEffectList();
				}
				result = this._healFlawSpeed;
				break;
			}
			case 317:
			{
				bool flag317 = this._unlockSpeed == null && createIfNull;
				if (flag317)
				{
					this._unlockSpeed = new SpecialEffectList();
				}
				result = this._unlockSpeed;
				break;
			}
			case 318:
			{
				bool flag318 = this._flawBonusFactor == null && createIfNull;
				if (flag318)
				{
					this._flawBonusFactor = new SpecialEffectList();
				}
				result = this._flawBonusFactor;
				break;
			}
			case 319:
			{
				bool flag319 = this._canCostShaTricks == null && createIfNull;
				if (flag319)
				{
					this._canCostShaTricks = new SpecialEffectList();
				}
				result = this._canCostShaTricks;
				break;
			}
			case 320:
			{
				bool flag320 = this._defenderDirectFinalDamageValue == null && createIfNull;
				if (flag320)
				{
					this._defenderDirectFinalDamageValue = new SpecialEffectList();
				}
				result = this._defenderDirectFinalDamageValue;
				break;
			}
			case 321:
			{
				bool flag321 = this._normalAttackRecoveryFrame == null && createIfNull;
				if (flag321)
				{
					this._normalAttackRecoveryFrame = new SpecialEffectList();
				}
				result = this._normalAttackRecoveryFrame;
				break;
			}
			case 322:
			{
				bool flag322 = this._finalGoneMadInjury == null && createIfNull;
				if (flag322)
				{
					this._finalGoneMadInjury = new SpecialEffectList();
				}
				result = this._finalGoneMadInjury;
				break;
			}
			case 323:
			{
				bool flag323 = this._attackerDirectFinalDamageValue == null && createIfNull;
				if (flag323)
				{
					this._attackerDirectFinalDamageValue = new SpecialEffectList();
				}
				result = this._attackerDirectFinalDamageValue;
				break;
			}
			case 324:
			{
				bool flag324 = this._canCostTrickDuringPreparingSkill == null && createIfNull;
				if (flag324)
				{
					this._canCostTrickDuringPreparingSkill = new SpecialEffectList();
				}
				result = this._canCostTrickDuringPreparingSkill;
				break;
			}
			case 325:
			{
				bool flag325 = this._validItemList == null && createIfNull;
				if (flag325)
				{
					this._validItemList = new SpecialEffectList();
				}
				result = this._validItemList;
				break;
			}
			case 326:
			{
				bool flag326 = this._acceptDamageCanAdd == null && createIfNull;
				if (flag326)
				{
					this._acceptDamageCanAdd = new SpecialEffectList();
				}
				result = this._acceptDamageCanAdd;
				break;
			}
			case 327:
			{
				bool flag327 = this._makeDamageCanReduce == null && createIfNull;
				if (flag327)
				{
					this._makeDamageCanReduce = new SpecialEffectList();
				}
				result = this._makeDamageCanReduce;
				break;
			}
			case 328:
			{
				bool flag328 = this._normalAttackGetTrickCount == null && createIfNull;
				if (flag328)
				{
					this._normalAttackGetTrickCount = new SpecialEffectList();
				}
				result = this._normalAttackGetTrickCount;
				break;
			}
			default:
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(37, 1);
				defaultInterpolatedStringHandler.AppendLiteral("AffectedData filed with id ");
				defaultInterpolatedStringHandler.AppendFormatted<ushort>(fieldId);
				defaultInterpolatedStringHandler.AppendLiteral(" not found");
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			}
			return result;
		}

		// Token: 0x060025D2 RID: 9682 RVA: 0x001D1238 File Offset: 0x001CF438
		public void SetEffectList(DataContext context, ushort fieldId, SpecialEffectList effectList)
		{
			switch (fieldId)
			{
			case 1:
				this.SetMaxStrength(effectList, context);
				break;
			case 2:
				this.SetMaxDexterity(effectList, context);
				break;
			case 3:
				this.SetMaxConcentration(effectList, context);
				break;
			case 4:
				this.SetMaxVitality(effectList, context);
				break;
			case 5:
				this.SetMaxEnergy(effectList, context);
				break;
			case 6:
				this.SetMaxIntelligence(effectList, context);
				break;
			case 7:
				this.SetRecoveryOfStance(effectList, context);
				break;
			case 8:
				this.SetRecoveryOfBreath(effectList, context);
				break;
			case 9:
				this.SetMoveSpeed(effectList, context);
				break;
			case 10:
				this.SetRecoveryOfFlaw(effectList, context);
				break;
			case 11:
				this.SetCastSpeed(effectList, context);
				break;
			case 12:
				this.SetRecoveryOfBlockedAcupoint(effectList, context);
				break;
			case 13:
				this.SetWeaponSwitchSpeed(effectList, context);
				break;
			case 14:
				this.SetAttackSpeed(effectList, context);
				break;
			case 15:
				this.SetInnerRatio(effectList, context);
				break;
			case 16:
				this.SetRecoveryOfQiDisorder(effectList, context);
				break;
			case 17:
				this.SetMinorAttributeFixMaxValue(effectList, context);
				break;
			case 18:
				this.SetMinorAttributeFixMinValue(effectList, context);
				break;
			case 19:
				this.SetResistOfHotPoison(effectList, context);
				break;
			case 20:
				this.SetResistOfGloomyPoison(effectList, context);
				break;
			case 21:
				this.SetResistOfColdPoison(effectList, context);
				break;
			case 22:
				this.SetResistOfRedPoison(effectList, context);
				break;
			case 23:
				this.SetResistOfRottenPoison(effectList, context);
				break;
			case 24:
				this.SetResistOfIllusoryPoison(effectList, context);
				break;
			case 25:
				this.SetDisplayAge(effectList, context);
				break;
			case 26:
				this.SetNeiliProportionOfFiveElements(effectList, context);
				break;
			case 27:
				this.SetWeaponMaxPower(effectList, context);
				break;
			case 28:
				this.SetWeaponUseRequirement(effectList, context);
				break;
			case 29:
				this.SetWeaponAttackRange(effectList, context);
				break;
			case 30:
				this.SetArmorMaxPower(effectList, context);
				break;
			case 31:
				this.SetArmorUseRequirement(effectList, context);
				break;
			case 32:
				this.SetHitStrength(effectList, context);
				break;
			case 33:
				this.SetHitTechnique(effectList, context);
				break;
			case 34:
				this.SetHitSpeed(effectList, context);
				break;
			case 35:
				this.SetHitMind(effectList, context);
				break;
			case 36:
				this.SetHitCanChange(effectList, context);
				break;
			case 37:
				this.SetHitChangeEffectPercent(effectList, context);
				break;
			case 38:
				this.SetAvoidStrength(effectList, context);
				break;
			case 39:
				this.SetAvoidTechnique(effectList, context);
				break;
			case 40:
				this.SetAvoidSpeed(effectList, context);
				break;
			case 41:
				this.SetAvoidMind(effectList, context);
				break;
			case 42:
				this.SetAvoidCanChange(effectList, context);
				break;
			case 43:
				this.SetAvoidChangeEffectPercent(effectList, context);
				break;
			case 44:
				this.SetPenetrateOuter(effectList, context);
				break;
			case 45:
				this.SetPenetrateInner(effectList, context);
				break;
			case 46:
				this.SetPenetrateResistOuter(effectList, context);
				break;
			case 47:
				this.SetPenetrateResistInner(effectList, context);
				break;
			case 48:
				this.SetNeiliAllocationAttack(effectList, context);
				break;
			case 49:
				this.SetNeiliAllocationAgile(effectList, context);
				break;
			case 50:
				this.SetNeiliAllocationDefense(effectList, context);
				break;
			case 51:
				this.SetNeiliAllocationAssist(effectList, context);
				break;
			case 52:
				this.SetHappiness(effectList, context);
				break;
			case 53:
				this.SetMaxHealth(effectList, context);
				break;
			case 54:
				this.SetHealthCost(effectList, context);
				break;
			case 55:
				this.SetMoveSpeedCanChange(effectList, context);
				break;
			case 56:
				this.SetAttackerHitStrength(effectList, context);
				break;
			case 57:
				this.SetAttackerHitTechnique(effectList, context);
				break;
			case 58:
				this.SetAttackerHitSpeed(effectList, context);
				break;
			case 59:
				this.SetAttackerHitMind(effectList, context);
				break;
			case 60:
				this.SetAttackerAvoidStrength(effectList, context);
				break;
			case 61:
				this.SetAttackerAvoidTechnique(effectList, context);
				break;
			case 62:
				this.SetAttackerAvoidSpeed(effectList, context);
				break;
			case 63:
				this.SetAttackerAvoidMind(effectList, context);
				break;
			case 64:
				this.SetAttackerPenetrateOuter(effectList, context);
				break;
			case 65:
				this.SetAttackerPenetrateInner(effectList, context);
				break;
			case 66:
				this.SetAttackerPenetrateResistOuter(effectList, context);
				break;
			case 67:
				this.SetAttackerPenetrateResistInner(effectList, context);
				break;
			case 68:
				this.SetAttackHitType(effectList, context);
				break;
			case 69:
				this.SetMakeDirectDamage(effectList, context);
				break;
			case 70:
				this.SetMakeBounceDamage(effectList, context);
				break;
			case 71:
				this.SetMakeFightBackDamage(effectList, context);
				break;
			case 72:
				this.SetMakePoisonLevel(effectList, context);
				break;
			case 73:
				this.SetMakePoisonValue(effectList, context);
				break;
			case 74:
				this.SetAttackerHitOdds(effectList, context);
				break;
			case 75:
				this.SetAttackerFightBackHitOdds(effectList, context);
				break;
			case 76:
				this.SetAttackerPursueOdds(effectList, context);
				break;
			case 77:
				this.SetMakedInjuryChangeToOld(effectList, context);
				break;
			case 78:
				this.SetMakedPoisonChangeToOld(effectList, context);
				break;
			case 79:
				this.SetMakeDamageType(effectList, context);
				break;
			case 80:
				this.SetCanMakeInjuryToNoInjuryPart(effectList, context);
				break;
			case 81:
				this.SetMakePoisonType(effectList, context);
				break;
			case 82:
				this.SetNormalAttackWeapon(effectList, context);
				break;
			case 83:
				this.SetNormalAttackTrick(effectList, context);
				break;
			case 84:
				this.SetExtraFlawCount(effectList, context);
				break;
			case 85:
				this.SetAttackCanBounce(effectList, context);
				break;
			case 86:
				this.SetAttackCanFightBack(effectList, context);
				break;
			case 87:
				this.SetMakeFightBackInjuryMark(effectList, context);
				break;
			case 88:
				this.SetLegSkillUseShoes(effectList, context);
				break;
			case 89:
				this.SetAttackerFinalDamageValue(effectList, context);
				break;
			case 90:
				this.SetDefenderHitStrength(effectList, context);
				break;
			case 91:
				this.SetDefenderHitTechnique(effectList, context);
				break;
			case 92:
				this.SetDefenderHitSpeed(effectList, context);
				break;
			case 93:
				this.SetDefenderHitMind(effectList, context);
				break;
			case 94:
				this.SetDefenderAvoidStrength(effectList, context);
				break;
			case 95:
				this.SetDefenderAvoidTechnique(effectList, context);
				break;
			case 96:
				this.SetDefenderAvoidSpeed(effectList, context);
				break;
			case 97:
				this.SetDefenderAvoidMind(effectList, context);
				break;
			case 98:
				this.SetDefenderPenetrateOuter(effectList, context);
				break;
			case 99:
				this.SetDefenderPenetrateInner(effectList, context);
				break;
			case 100:
				this.SetDefenderPenetrateResistOuter(effectList, context);
				break;
			case 101:
				this.SetDefenderPenetrateResistInner(effectList, context);
				break;
			case 102:
				this.SetAcceptDirectDamage(effectList, context);
				break;
			case 103:
				this.SetAcceptBounceDamage(effectList, context);
				break;
			case 104:
				this.SetAcceptFightBackDamage(effectList, context);
				break;
			case 105:
				this.SetAcceptPoisonLevel(effectList, context);
				break;
			case 106:
				this.SetAcceptPoisonValue(effectList, context);
				break;
			case 107:
				this.SetDefenderHitOdds(effectList, context);
				break;
			case 108:
				this.SetDefenderFightBackHitOdds(effectList, context);
				break;
			case 109:
				this.SetDefenderPursueOdds(effectList, context);
				break;
			case 110:
				this.SetAcceptMaxInjuryCount(effectList, context);
				break;
			case 111:
				this.SetBouncePower(effectList, context);
				break;
			case 112:
				this.SetFightBackPower(effectList, context);
				break;
			case 113:
				this.SetDirectDamageInnerRatio(effectList, context);
				break;
			case 114:
				this.SetDefenderFinalDamageValue(effectList, context);
				break;
			case 115:
				this.SetDirectDamageValue(effectList, context);
				break;
			case 116:
				this.SetDirectInjuryMark(effectList, context);
				break;
			case 117:
				this.SetGoneMadInjury(effectList, context);
				break;
			case 118:
				this.SetHealInjurySpeed(effectList, context);
				break;
			case 119:
				this.SetHealInjuryBuff(effectList, context);
				break;
			case 120:
				this.SetHealInjuryDebuff(effectList, context);
				break;
			case 121:
				this.SetHealPoisonSpeed(effectList, context);
				break;
			case 122:
				this.SetHealPoisonBuff(effectList, context);
				break;
			case 123:
				this.SetHealPoisonDebuff(effectList, context);
				break;
			case 124:
				this.SetFleeSpeed(effectList, context);
				break;
			case 125:
				this.SetMaxFlawCount(effectList, context);
				break;
			case 126:
				this.SetCanAddFlaw(effectList, context);
				break;
			case 127:
				this.SetFlawLevel(effectList, context);
				break;
			case 128:
				this.SetFlawLevelCanReduce(effectList, context);
				break;
			case 129:
				this.SetFlawCount(effectList, context);
				break;
			case 130:
				this.SetMaxAcupointCount(effectList, context);
				break;
			case 131:
				this.SetCanAddAcupoint(effectList, context);
				break;
			case 132:
				this.SetAcupointLevel(effectList, context);
				break;
			case 133:
				this.SetAcupointLevelCanReduce(effectList, context);
				break;
			case 134:
				this.SetAcupointCount(effectList, context);
				break;
			case 135:
				this.SetAddNeiliAllocation(effectList, context);
				break;
			case 136:
				this.SetCostNeiliAllocation(effectList, context);
				break;
			case 137:
				this.SetCanChangeNeiliAllocation(effectList, context);
				break;
			case 138:
				this.SetCanGetTrick(effectList, context);
				break;
			case 139:
				this.SetGetTrickType(effectList, context);
				break;
			case 140:
				this.SetAttackBodyPart(effectList, context);
				break;
			case 141:
				this.SetWeaponEquipAttack(effectList, context);
				break;
			case 142:
				this.SetWeaponEquipDefense(effectList, context);
				break;
			case 143:
				this.SetArmorEquipAttack(effectList, context);
				break;
			case 144:
				this.SetArmorEquipDefense(effectList, context);
				break;
			case 145:
				this.SetAttackRangeForward(effectList, context);
				break;
			case 146:
				this.SetAttackRangeBackward(effectList, context);
				break;
			case 147:
				this.SetMoveCanBeStopped(effectList, context);
				break;
			case 148:
				this.SetCanForcedMove(effectList, context);
				break;
			case 149:
				this.SetMobilityCanBeRemoved(effectList, context);
				break;
			case 150:
				this.SetMobilityCostByEffect(effectList, context);
				break;
			case 151:
				this.SetMoveDistance(effectList, context);
				break;
			case 152:
				this.SetJumpPrepareFrame(effectList, context);
				break;
			case 153:
				this.SetBounceInjuryMark(effectList, context);
				break;
			case 154:
				this.SetSkillHasCost(effectList, context);
				break;
			case 155:
				this.SetCombatStateEffect(effectList, context);
				break;
			case 156:
				this.SetChangeNeedUseSkill(effectList, context);
				break;
			case 157:
				this.SetChangeDistanceIsMove(effectList, context);
				break;
			case 158:
				this.SetReplaceCharHit(effectList, context);
				break;
			case 159:
				this.SetCanAddPoison(effectList, context);
				break;
			case 160:
				this.SetCanReducePoison(effectList, context);
				break;
			case 161:
				this.SetReducePoisonValue(effectList, context);
				break;
			case 162:
				this.SetPoisonCanAffect(effectList, context);
				break;
			case 163:
				this.SetPoisonAffectCount(effectList, context);
				break;
			case 164:
				this.SetCostTricks(effectList, context);
				break;
			case 165:
				this.SetJumpMoveDistance(effectList, context);
				break;
			case 166:
				this.SetCombatStateToAdd(effectList, context);
				break;
			case 167:
				this.SetCombatStatePower(effectList, context);
				break;
			case 168:
				this.SetBreakBodyPartInjuryCount(effectList, context);
				break;
			case 169:
				this.SetBodyPartIsBroken(effectList, context);
				break;
			case 170:
				this.SetMaxTrickCount(effectList, context);
				break;
			case 171:
				this.SetMaxBreathPercent(effectList, context);
				break;
			case 172:
				this.SetMaxStancePercent(effectList, context);
				break;
			case 173:
				this.SetExtraBreathPercent(effectList, context);
				break;
			case 174:
				this.SetExtraStancePercent(effectList, context);
				break;
			case 175:
				this.SetMoveCostMobility(effectList, context);
				break;
			case 176:
				this.SetDefendSkillKeepTime(effectList, context);
				break;
			case 177:
				this.SetBounceRange(effectList, context);
				break;
			case 178:
				this.SetMindMarkKeepTime(effectList, context);
				break;
			case 179:
				this.SetSkillMobilityCostPerFrame(effectList, context);
				break;
			case 180:
				this.SetCanAddWug(effectList, context);
				break;
			case 181:
				this.SetHasGodWeaponBuff(effectList, context);
				break;
			case 182:
				this.SetHasGodArmorBuff(effectList, context);
				break;
			case 183:
				this.SetTeammateCmdRequireGenerateValue(effectList, context);
				break;
			case 184:
				this.SetTeammateCmdEffect(effectList, context);
				break;
			case 185:
				this.SetFlawRecoverSpeed(effectList, context);
				break;
			case 186:
				this.SetAcupointRecoverSpeed(effectList, context);
				break;
			case 187:
				this.SetMindMarkRecoverSpeed(effectList, context);
				break;
			case 188:
				this.SetInjuryAutoHealSpeed(effectList, context);
				break;
			case 189:
				this.SetCanRecoverBreath(effectList, context);
				break;
			case 190:
				this.SetCanRecoverStance(effectList, context);
				break;
			case 191:
				this.SetFatalDamageValue(effectList, context);
				break;
			case 192:
				this.SetFatalDamageMarkCount(effectList, context);
				break;
			case 193:
				this.SetCanFightBackDuringPrepareSkill(effectList, context);
				break;
			case 194:
				this.SetSkillPrepareSpeed(effectList, context);
				break;
			case 195:
				this.SetBreathRecoverSpeed(effectList, context);
				break;
			case 196:
				this.SetStanceRecoverSpeed(effectList, context);
				break;
			case 197:
				this.SetMobilityRecoverSpeed(effectList, context);
				break;
			case 198:
				this.SetChangeTrickProgressAddValue(effectList, context);
				break;
			case 199:
				this.SetPower(effectList, context);
				break;
			case 200:
				this.SetMaxPower(effectList, context);
				break;
			case 201:
				this.SetPowerCanReduce(effectList, context);
				break;
			case 202:
				this.SetUseRequirement(effectList, context);
				break;
			case 203:
				this.SetCurrInnerRatio(effectList, context);
				break;
			case 204:
				this.SetCostBreathAndStance(effectList, context);
				break;
			case 205:
				this.SetCostBreath(effectList, context);
				break;
			case 206:
				this.SetCostStance(effectList, context);
				break;
			case 207:
				this.SetCostMobility(effectList, context);
				break;
			case 208:
				this.SetSkillCostTricks(effectList, context);
				break;
			case 209:
				this.SetEffectDirection(effectList, context);
				break;
			case 210:
				this.SetEffectDirectionCanChange(effectList, context);
				break;
			case 211:
				this.SetGridCost(effectList, context);
				break;
			case 212:
				this.SetPrepareTotalProgress(effectList, context);
				break;
			case 213:
				this.SetSpecificGridCount(effectList, context);
				break;
			case 214:
				this.SetGenericGridCount(effectList, context);
				break;
			case 215:
				this.SetCanInterrupt(effectList, context);
				break;
			case 216:
				this.SetInterruptOdds(effectList, context);
				break;
			case 217:
				this.SetCanSilence(effectList, context);
				break;
			case 218:
				this.SetSilenceOdds(effectList, context);
				break;
			case 219:
				this.SetCanCastWithBrokenBodyPart(effectList, context);
				break;
			case 220:
				this.SetAddPowerCanBeRemoved(effectList, context);
				break;
			case 221:
				this.SetSkillType(effectList, context);
				break;
			case 222:
				this.SetEffectCountCanChange(effectList, context);
				break;
			case 223:
				this.SetCanCastInDefend(effectList, context);
				break;
			case 224:
				this.SetHitDistribution(effectList, context);
				break;
			case 225:
				this.SetCanCastOnLackBreath(effectList, context);
				break;
			case 226:
				this.SetCanCastOnLackStance(effectList, context);
				break;
			case 227:
				this.SetCostBreathOnCast(effectList, context);
				break;
			case 228:
				this.SetCostStanceOnCast(effectList, context);
				break;
			case 229:
				this.SetCanUseMobilityAsBreath(effectList, context);
				break;
			case 230:
				this.SetCanUseMobilityAsStance(effectList, context);
				break;
			case 231:
				this.SetCastCostNeiliAllocation(effectList, context);
				break;
			case 232:
				this.SetAcceptPoisonResist(effectList, context);
				break;
			case 233:
				this.SetMakePoisonResist(effectList, context);
				break;
			case 234:
				this.SetCanCriticalHit(effectList, context);
				break;
			case 235:
				this.SetCanCostNeiliAllocationEffect(effectList, context);
				break;
			case 236:
				this.SetConsummateLevelRelatedMainAttributesHitValues(effectList, context);
				break;
			case 237:
				this.SetConsummateLevelRelatedMainAttributesAvoidValues(effectList, context);
				break;
			case 238:
				this.SetConsummateLevelRelatedMainAttributesPenetrations(effectList, context);
				break;
			case 239:
				this.SetConsummateLevelRelatedMainAttributesPenetrationResists(effectList, context);
				break;
			case 240:
				this.SetSkillAlsoAsFiveElements(effectList, context);
				break;
			case 241:
				this.SetInnerInjuryImmunity(effectList, context);
				break;
			case 242:
				this.SetOuterInjuryImmunity(effectList, context);
				break;
			case 243:
				this.SetPoisonAffectThreshold(effectList, context);
				break;
			case 244:
				this.SetLockDistance(effectList, context);
				break;
			case 245:
				this.SetResistOfAllPoison(effectList, context);
				break;
			case 246:
				this.SetMakePoisonTarget(effectList, context);
				break;
			case 247:
				this.SetAcceptPoisonTarget(effectList, context);
				break;
			case 248:
				this.SetCertainCriticalHit(effectList, context);
				break;
			case 249:
				this.SetMindMarkCount(effectList, context);
				break;
			case 250:
				this.SetCanFightBackWithHit(effectList, context);
				break;
			case 251:
				this.SetInevitableHit(effectList, context);
				break;
			case 252:
				this.SetAttackCanPursue(effectList, context);
				break;
			case 253:
				this.SetCombatSkillDataEffectList(effectList, context);
				break;
			case 254:
				this.SetCriticalOdds(effectList, context);
				break;
			case 255:
				this.SetStanceCostByEffect(effectList, context);
				break;
			case 256:
				this.SetBreathCostByEffect(effectList, context);
				break;
			case 257:
				this.SetPowerAddRatio(effectList, context);
				break;
			case 258:
				this.SetPowerReduceRatio(effectList, context);
				break;
			case 259:
				this.SetPoisonAffectProduceValue(effectList, context);
				break;
			case 260:
				this.SetCanReadingOnMonthChange(effectList, context);
				break;
			case 261:
				this.SetMedicineEffect(effectList, context);
				break;
			case 262:
				this.SetXiangshuInfectionDelta(effectList, context);
				break;
			case 263:
				this.SetHealthDelta(effectList, context);
				break;
			case 264:
				this.SetWeaponSilenceFrame(effectList, context);
				break;
			case 265:
				this.SetSilenceFrame(effectList, context);
				break;
			case 266:
				this.SetCurrAgeDelta(effectList, context);
				break;
			case 267:
				this.SetGoneMadInAllBreak(effectList, context);
				break;
			case 268:
				this.SetMakeLoveRateOnMonthChange(effectList, context);
				break;
			case 269:
				this.SetCanAutoHealOnMonthChange(effectList, context);
				break;
			case 270:
				this.SetHappinessDelta(effectList, context);
				break;
			case 271:
				this.SetTeammateCmdCanUse(effectList, context);
				break;
			case 272:
				this.SetMixPoisonInfinityAffect(effectList, context);
				break;
			case 273:
				this.SetAttackRangeMaxAcupoint(effectList, context);
				break;
			case 274:
				this.SetMaxMobilityPercent(effectList, context);
				break;
			case 275:
				this.SetMakeMindDamage(effectList, context);
				break;
			case 276:
				this.SetAcceptMindDamage(effectList, context);
				break;
			case 277:
				this.SetHitAddByTempValue(effectList, context);
				break;
			case 278:
				this.SetAvoidAddByTempValue(effectList, context);
				break;
			case 279:
				this.SetIgnoreEquipmentOverload(effectList, context);
				break;
			case 280:
				this.SetCanCostEnemyUsableTricks(effectList, context);
				break;
			case 281:
				this.SetIgnoreArmor(effectList, context);
				break;
			case 282:
				this.SetUnyieldingFallen(effectList, context);
				break;
			case 283:
				this.SetNormalAttackPrepareFrame(effectList, context);
				break;
			case 284:
				this.SetCanCostUselessTricks(effectList, context);
				break;
			case 285:
				this.SetDefendSkillCanAffect(effectList, context);
				break;
			case 286:
				this.SetAssistSkillCanAffect(effectList, context);
				break;
			case 287:
				this.SetAgileSkillCanAffect(effectList, context);
				break;
			case 288:
				this.SetAllMarkChangeToMind(effectList, context);
				break;
			case 289:
				this.SetMindMarkChangeToFatal(effectList, context);
				break;
			case 290:
				this.SetCanCast(effectList, context);
				break;
			case 291:
				this.SetInevitableAvoid(effectList, context);
				break;
			case 292:
				this.SetPowerEffectReverse(effectList, context);
				break;
			case 293:
				this.SetFeatureBonusReverse(effectList, context);
				break;
			case 294:
				this.SetWugFatalDamageValue(effectList, context);
				break;
			case 295:
				this.SetCanRecoverHealthOnMonthChange(effectList, context);
				break;
			case 296:
				this.SetTakeRevengeRateOnMonthChange(effectList, context);
				break;
			case 297:
				this.SetConsummateLevelBonus(effectList, context);
				break;
			case 298:
				this.SetNeiliDelta(effectList, context);
				break;
			case 299:
				this.SetCanMakeLoveSpecialOnMonthChange(effectList, context);
				break;
			case 300:
				this.SetHealAcupointSpeed(effectList, context);
				break;
			case 301:
				this.SetMaxChangeTrickCount(effectList, context);
				break;
			case 302:
				this.SetConvertCostBreathAndStance(effectList, context);
				break;
			case 303:
				this.SetPersonalitiesAll(effectList, context);
				break;
			case 304:
				this.SetFinalFatalDamageMarkCount(effectList, context);
				break;
			case 305:
				this.SetInfinityMindMarkProgress(effectList, context);
				break;
			case 306:
				this.SetCombatSkillAiScorePower(effectList, context);
				break;
			case 307:
				this.SetNormalAttackChangeToUnlockAttack(effectList, context);
				break;
			case 308:
				this.SetAttackBodyPartOdds(effectList, context);
				break;
			case 309:
				this.SetChangeDurability(effectList, context);
				break;
			case 310:
				this.SetEquipmentBonus(effectList, context);
				break;
			case 311:
				this.SetEquipmentWeight(effectList, context);
				break;
			case 312:
				this.SetRawCreateEffectList(effectList, context);
				break;
			case 313:
				this.SetJiTrickAsWeaponTrickCount(effectList, context);
				break;
			case 314:
				this.SetUselessTrickAsJiTrickCount(effectList, context);
				break;
			case 315:
				this.SetEquipmentPower(effectList, context);
				break;
			case 316:
				this.SetHealFlawSpeed(effectList, context);
				break;
			case 317:
				this.SetUnlockSpeed(effectList, context);
				break;
			case 318:
				this.SetFlawBonusFactor(effectList, context);
				break;
			case 319:
				this.SetCanCostShaTricks(effectList, context);
				break;
			case 320:
				this.SetDefenderDirectFinalDamageValue(effectList, context);
				break;
			case 321:
				this.SetNormalAttackRecoveryFrame(effectList, context);
				break;
			case 322:
				this.SetFinalGoneMadInjury(effectList, context);
				break;
			case 323:
				this.SetAttackerDirectFinalDamageValue(effectList, context);
				break;
			case 324:
				this.SetCanCostTrickDuringPreparingSkill(effectList, context);
				break;
			case 325:
				this.SetValidItemList(effectList, context);
				break;
			case 326:
				this.SetAcceptDamageCanAdd(effectList, context);
				break;
			case 327:
				this.SetMakeDamageCanReduce(effectList, context);
				break;
			case 328:
				this.SetNormalAttackGetTrickCount(effectList, context);
				break;
			default:
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(33, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Effect list of fieldId ");
				defaultInterpolatedStringHandler.AppendFormatted<ushort>(fieldId);
				defaultInterpolatedStringHandler.AppendLiteral(" not found");
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			}
		}

		// Token: 0x060025D3 RID: 9683 RVA: 0x001D298C File Offset: 0x001D0B8C
		public int GetId()
		{
			return this._id;
		}

		// Token: 0x060025D4 RID: 9684 RVA: 0x001D29A4 File Offset: 0x001D0BA4
		public unsafe void SetId(int id, DataContext context)
		{
			this._id = id;
			base.SetModifiedAndInvalidateInfluencedCache(0, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				byte* pData = OperationAdder.DynamicObjectCollection_SetFixedField<int>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this._id, 0U, 4);
				*(int*)pData = this._id;
				pData += 4;
			}
		}

		// Token: 0x060025D5 RID: 9685 RVA: 0x001D2A04 File Offset: 0x001D0C04
		public SpecialEffectList GetMaxStrength()
		{
			return this._maxStrength;
		}

		// Token: 0x060025D6 RID: 9686 RVA: 0x001D2A1C File Offset: 0x001D0C1C
		public unsafe void SetMaxStrength(SpecialEffectList maxStrength, DataContext context)
		{
			this._maxStrength = maxStrength;
			base.SetModifiedAndInvalidateInfluencedCache(1, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				int dataSize = this._maxStrength.GetSerializedSize();
				byte* pData = OperationAdder.DynamicObjectCollection_SetDynamicField<int>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this._id, 0, dataSize);
				pData += this._maxStrength.Serialize(pData);
			}
		}

		// Token: 0x060025D7 RID: 9687 RVA: 0x001D2A8C File Offset: 0x001D0C8C
		public SpecialEffectList GetMaxDexterity()
		{
			return this._maxDexterity;
		}

		// Token: 0x060025D8 RID: 9688 RVA: 0x001D2AA4 File Offset: 0x001D0CA4
		public unsafe void SetMaxDexterity(SpecialEffectList maxDexterity, DataContext context)
		{
			this._maxDexterity = maxDexterity;
			base.SetModifiedAndInvalidateInfluencedCache(2, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				int dataSize = this._maxDexterity.GetSerializedSize();
				byte* pData = OperationAdder.DynamicObjectCollection_SetDynamicField<int>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this._id, 1, dataSize);
				pData += this._maxDexterity.Serialize(pData);
			}
		}

		// Token: 0x060025D9 RID: 9689 RVA: 0x001D2B14 File Offset: 0x001D0D14
		public SpecialEffectList GetMaxConcentration()
		{
			return this._maxConcentration;
		}

		// Token: 0x060025DA RID: 9690 RVA: 0x001D2B2C File Offset: 0x001D0D2C
		public unsafe void SetMaxConcentration(SpecialEffectList maxConcentration, DataContext context)
		{
			this._maxConcentration = maxConcentration;
			base.SetModifiedAndInvalidateInfluencedCache(3, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				int dataSize = this._maxConcentration.GetSerializedSize();
				byte* pData = OperationAdder.DynamicObjectCollection_SetDynamicField<int>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this._id, 2, dataSize);
				pData += this._maxConcentration.Serialize(pData);
			}
		}

		// Token: 0x060025DB RID: 9691 RVA: 0x001D2B9C File Offset: 0x001D0D9C
		public SpecialEffectList GetMaxVitality()
		{
			return this._maxVitality;
		}

		// Token: 0x060025DC RID: 9692 RVA: 0x001D2BB4 File Offset: 0x001D0DB4
		public unsafe void SetMaxVitality(SpecialEffectList maxVitality, DataContext context)
		{
			this._maxVitality = maxVitality;
			base.SetModifiedAndInvalidateInfluencedCache(4, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				int dataSize = this._maxVitality.GetSerializedSize();
				byte* pData = OperationAdder.DynamicObjectCollection_SetDynamicField<int>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this._id, 3, dataSize);
				pData += this._maxVitality.Serialize(pData);
			}
		}

		// Token: 0x060025DD RID: 9693 RVA: 0x001D2C24 File Offset: 0x001D0E24
		public SpecialEffectList GetMaxEnergy()
		{
			return this._maxEnergy;
		}

		// Token: 0x060025DE RID: 9694 RVA: 0x001D2C3C File Offset: 0x001D0E3C
		public unsafe void SetMaxEnergy(SpecialEffectList maxEnergy, DataContext context)
		{
			this._maxEnergy = maxEnergy;
			base.SetModifiedAndInvalidateInfluencedCache(5, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				int dataSize = this._maxEnergy.GetSerializedSize();
				byte* pData = OperationAdder.DynamicObjectCollection_SetDynamicField<int>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this._id, 4, dataSize);
				pData += this._maxEnergy.Serialize(pData);
			}
		}

		// Token: 0x060025DF RID: 9695 RVA: 0x001D2CAC File Offset: 0x001D0EAC
		public SpecialEffectList GetMaxIntelligence()
		{
			return this._maxIntelligence;
		}

		// Token: 0x060025E0 RID: 9696 RVA: 0x001D2CC4 File Offset: 0x001D0EC4
		public unsafe void SetMaxIntelligence(SpecialEffectList maxIntelligence, DataContext context)
		{
			this._maxIntelligence = maxIntelligence;
			base.SetModifiedAndInvalidateInfluencedCache(6, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				int dataSize = this._maxIntelligence.GetSerializedSize();
				byte* pData = OperationAdder.DynamicObjectCollection_SetDynamicField<int>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this._id, 5, dataSize);
				pData += this._maxIntelligence.Serialize(pData);
			}
		}

		// Token: 0x060025E1 RID: 9697 RVA: 0x001D2D34 File Offset: 0x001D0F34
		public SpecialEffectList GetRecoveryOfStance()
		{
			return this._recoveryOfStance;
		}

		// Token: 0x060025E2 RID: 9698 RVA: 0x001D2D4C File Offset: 0x001D0F4C
		public unsafe void SetRecoveryOfStance(SpecialEffectList recoveryOfStance, DataContext context)
		{
			this._recoveryOfStance = recoveryOfStance;
			base.SetModifiedAndInvalidateInfluencedCache(7, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				int dataSize = this._recoveryOfStance.GetSerializedSize();
				byte* pData = OperationAdder.DynamicObjectCollection_SetDynamicField<int>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this._id, 6, dataSize);
				pData += this._recoveryOfStance.Serialize(pData);
			}
		}

		// Token: 0x060025E3 RID: 9699 RVA: 0x001D2DBC File Offset: 0x001D0FBC
		public SpecialEffectList GetRecoveryOfBreath()
		{
			return this._recoveryOfBreath;
		}

		// Token: 0x060025E4 RID: 9700 RVA: 0x001D2DD4 File Offset: 0x001D0FD4
		public unsafe void SetRecoveryOfBreath(SpecialEffectList recoveryOfBreath, DataContext context)
		{
			this._recoveryOfBreath = recoveryOfBreath;
			base.SetModifiedAndInvalidateInfluencedCache(8, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				int dataSize = this._recoveryOfBreath.GetSerializedSize();
				byte* pData = OperationAdder.DynamicObjectCollection_SetDynamicField<int>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this._id, 7, dataSize);
				pData += this._recoveryOfBreath.Serialize(pData);
			}
		}

		// Token: 0x060025E5 RID: 9701 RVA: 0x001D2E44 File Offset: 0x001D1044
		public SpecialEffectList GetMoveSpeed()
		{
			return this._moveSpeed;
		}

		// Token: 0x060025E6 RID: 9702 RVA: 0x001D2E5C File Offset: 0x001D105C
		public unsafe void SetMoveSpeed(SpecialEffectList moveSpeed, DataContext context)
		{
			this._moveSpeed = moveSpeed;
			base.SetModifiedAndInvalidateInfluencedCache(9, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				int dataSize = this._moveSpeed.GetSerializedSize();
				byte* pData = OperationAdder.DynamicObjectCollection_SetDynamicField<int>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this._id, 8, dataSize);
				pData += this._moveSpeed.Serialize(pData);
			}
		}

		// Token: 0x060025E7 RID: 9703 RVA: 0x001D2ECC File Offset: 0x001D10CC
		public SpecialEffectList GetRecoveryOfFlaw()
		{
			return this._recoveryOfFlaw;
		}

		// Token: 0x060025E8 RID: 9704 RVA: 0x001D2EE4 File Offset: 0x001D10E4
		public unsafe void SetRecoveryOfFlaw(SpecialEffectList recoveryOfFlaw, DataContext context)
		{
			this._recoveryOfFlaw = recoveryOfFlaw;
			base.SetModifiedAndInvalidateInfluencedCache(10, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				int dataSize = this._recoveryOfFlaw.GetSerializedSize();
				byte* pData = OperationAdder.DynamicObjectCollection_SetDynamicField<int>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this._id, 9, dataSize);
				pData += this._recoveryOfFlaw.Serialize(pData);
			}
		}

		// Token: 0x060025E9 RID: 9705 RVA: 0x001D2F54 File Offset: 0x001D1154
		public SpecialEffectList GetCastSpeed()
		{
			return this._castSpeed;
		}

		// Token: 0x060025EA RID: 9706 RVA: 0x001D2F6C File Offset: 0x001D116C
		public unsafe void SetCastSpeed(SpecialEffectList castSpeed, DataContext context)
		{
			this._castSpeed = castSpeed;
			base.SetModifiedAndInvalidateInfluencedCache(11, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				int dataSize = this._castSpeed.GetSerializedSize();
				byte* pData = OperationAdder.DynamicObjectCollection_SetDynamicField<int>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this._id, 10, dataSize);
				pData += this._castSpeed.Serialize(pData);
			}
		}

		// Token: 0x060025EB RID: 9707 RVA: 0x001D2FDC File Offset: 0x001D11DC
		public SpecialEffectList GetRecoveryOfBlockedAcupoint()
		{
			return this._recoveryOfBlockedAcupoint;
		}

		// Token: 0x060025EC RID: 9708 RVA: 0x001D2FF4 File Offset: 0x001D11F4
		public unsafe void SetRecoveryOfBlockedAcupoint(SpecialEffectList recoveryOfBlockedAcupoint, DataContext context)
		{
			this._recoveryOfBlockedAcupoint = recoveryOfBlockedAcupoint;
			base.SetModifiedAndInvalidateInfluencedCache(12, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				int dataSize = this._recoveryOfBlockedAcupoint.GetSerializedSize();
				byte* pData = OperationAdder.DynamicObjectCollection_SetDynamicField<int>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this._id, 11, dataSize);
				pData += this._recoveryOfBlockedAcupoint.Serialize(pData);
			}
		}

		// Token: 0x060025ED RID: 9709 RVA: 0x001D3064 File Offset: 0x001D1264
		public SpecialEffectList GetWeaponSwitchSpeed()
		{
			return this._weaponSwitchSpeed;
		}

		// Token: 0x060025EE RID: 9710 RVA: 0x001D307C File Offset: 0x001D127C
		public unsafe void SetWeaponSwitchSpeed(SpecialEffectList weaponSwitchSpeed, DataContext context)
		{
			this._weaponSwitchSpeed = weaponSwitchSpeed;
			base.SetModifiedAndInvalidateInfluencedCache(13, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				int dataSize = this._weaponSwitchSpeed.GetSerializedSize();
				byte* pData = OperationAdder.DynamicObjectCollection_SetDynamicField<int>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this._id, 12, dataSize);
				pData += this._weaponSwitchSpeed.Serialize(pData);
			}
		}

		// Token: 0x060025EF RID: 9711 RVA: 0x001D30EC File Offset: 0x001D12EC
		public SpecialEffectList GetAttackSpeed()
		{
			return this._attackSpeed;
		}

		// Token: 0x060025F0 RID: 9712 RVA: 0x001D3104 File Offset: 0x001D1304
		public unsafe void SetAttackSpeed(SpecialEffectList attackSpeed, DataContext context)
		{
			this._attackSpeed = attackSpeed;
			base.SetModifiedAndInvalidateInfluencedCache(14, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				int dataSize = this._attackSpeed.GetSerializedSize();
				byte* pData = OperationAdder.DynamicObjectCollection_SetDynamicField<int>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this._id, 13, dataSize);
				pData += this._attackSpeed.Serialize(pData);
			}
		}

		// Token: 0x060025F1 RID: 9713 RVA: 0x001D3174 File Offset: 0x001D1374
		public SpecialEffectList GetInnerRatio()
		{
			return this._innerRatio;
		}

		// Token: 0x060025F2 RID: 9714 RVA: 0x001D318C File Offset: 0x001D138C
		public unsafe void SetInnerRatio(SpecialEffectList innerRatio, DataContext context)
		{
			this._innerRatio = innerRatio;
			base.SetModifiedAndInvalidateInfluencedCache(15, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				int dataSize = this._innerRatio.GetSerializedSize();
				byte* pData = OperationAdder.DynamicObjectCollection_SetDynamicField<int>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this._id, 14, dataSize);
				pData += this._innerRatio.Serialize(pData);
			}
		}

		// Token: 0x060025F3 RID: 9715 RVA: 0x001D31FC File Offset: 0x001D13FC
		public SpecialEffectList GetRecoveryOfQiDisorder()
		{
			return this._recoveryOfQiDisorder;
		}

		// Token: 0x060025F4 RID: 9716 RVA: 0x001D3214 File Offset: 0x001D1414
		public unsafe void SetRecoveryOfQiDisorder(SpecialEffectList recoveryOfQiDisorder, DataContext context)
		{
			this._recoveryOfQiDisorder = recoveryOfQiDisorder;
			base.SetModifiedAndInvalidateInfluencedCache(16, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				int dataSize = this._recoveryOfQiDisorder.GetSerializedSize();
				byte* pData = OperationAdder.DynamicObjectCollection_SetDynamicField<int>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this._id, 15, dataSize);
				pData += this._recoveryOfQiDisorder.Serialize(pData);
			}
		}

		// Token: 0x060025F5 RID: 9717 RVA: 0x001D3284 File Offset: 0x001D1484
		public SpecialEffectList GetMinorAttributeFixMaxValue()
		{
			return this._minorAttributeFixMaxValue;
		}

		// Token: 0x060025F6 RID: 9718 RVA: 0x001D329C File Offset: 0x001D149C
		public unsafe void SetMinorAttributeFixMaxValue(SpecialEffectList minorAttributeFixMaxValue, DataContext context)
		{
			this._minorAttributeFixMaxValue = minorAttributeFixMaxValue;
			base.SetModifiedAndInvalidateInfluencedCache(17, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				int dataSize = this._minorAttributeFixMaxValue.GetSerializedSize();
				byte* pData = OperationAdder.DynamicObjectCollection_SetDynamicField<int>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this._id, 16, dataSize);
				pData += this._minorAttributeFixMaxValue.Serialize(pData);
			}
		}

		// Token: 0x060025F7 RID: 9719 RVA: 0x001D330C File Offset: 0x001D150C
		public SpecialEffectList GetMinorAttributeFixMinValue()
		{
			return this._minorAttributeFixMinValue;
		}

		// Token: 0x060025F8 RID: 9720 RVA: 0x001D3324 File Offset: 0x001D1524
		public unsafe void SetMinorAttributeFixMinValue(SpecialEffectList minorAttributeFixMinValue, DataContext context)
		{
			this._minorAttributeFixMinValue = minorAttributeFixMinValue;
			base.SetModifiedAndInvalidateInfluencedCache(18, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				int dataSize = this._minorAttributeFixMinValue.GetSerializedSize();
				byte* pData = OperationAdder.DynamicObjectCollection_SetDynamicField<int>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this._id, 17, dataSize);
				pData += this._minorAttributeFixMinValue.Serialize(pData);
			}
		}

		// Token: 0x060025F9 RID: 9721 RVA: 0x001D3394 File Offset: 0x001D1594
		public SpecialEffectList GetResistOfHotPoison()
		{
			return this._resistOfHotPoison;
		}

		// Token: 0x060025FA RID: 9722 RVA: 0x001D33AC File Offset: 0x001D15AC
		public unsafe void SetResistOfHotPoison(SpecialEffectList resistOfHotPoison, DataContext context)
		{
			this._resistOfHotPoison = resistOfHotPoison;
			base.SetModifiedAndInvalidateInfluencedCache(19, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				int dataSize = this._resistOfHotPoison.GetSerializedSize();
				byte* pData = OperationAdder.DynamicObjectCollection_SetDynamicField<int>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this._id, 18, dataSize);
				pData += this._resistOfHotPoison.Serialize(pData);
			}
		}

		// Token: 0x060025FB RID: 9723 RVA: 0x001D341C File Offset: 0x001D161C
		public SpecialEffectList GetResistOfGloomyPoison()
		{
			return this._resistOfGloomyPoison;
		}

		// Token: 0x060025FC RID: 9724 RVA: 0x001D3434 File Offset: 0x001D1634
		public unsafe void SetResistOfGloomyPoison(SpecialEffectList resistOfGloomyPoison, DataContext context)
		{
			this._resistOfGloomyPoison = resistOfGloomyPoison;
			base.SetModifiedAndInvalidateInfluencedCache(20, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				int dataSize = this._resistOfGloomyPoison.GetSerializedSize();
				byte* pData = OperationAdder.DynamicObjectCollection_SetDynamicField<int>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this._id, 19, dataSize);
				pData += this._resistOfGloomyPoison.Serialize(pData);
			}
		}

		// Token: 0x060025FD RID: 9725 RVA: 0x001D34A4 File Offset: 0x001D16A4
		public SpecialEffectList GetResistOfColdPoison()
		{
			return this._resistOfColdPoison;
		}

		// Token: 0x060025FE RID: 9726 RVA: 0x001D34BC File Offset: 0x001D16BC
		public unsafe void SetResistOfColdPoison(SpecialEffectList resistOfColdPoison, DataContext context)
		{
			this._resistOfColdPoison = resistOfColdPoison;
			base.SetModifiedAndInvalidateInfluencedCache(21, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				int dataSize = this._resistOfColdPoison.GetSerializedSize();
				byte* pData = OperationAdder.DynamicObjectCollection_SetDynamicField<int>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this._id, 20, dataSize);
				pData += this._resistOfColdPoison.Serialize(pData);
			}
		}

		// Token: 0x060025FF RID: 9727 RVA: 0x001D352C File Offset: 0x001D172C
		public SpecialEffectList GetResistOfRedPoison()
		{
			return this._resistOfRedPoison;
		}

		// Token: 0x06002600 RID: 9728 RVA: 0x001D3544 File Offset: 0x001D1744
		public unsafe void SetResistOfRedPoison(SpecialEffectList resistOfRedPoison, DataContext context)
		{
			this._resistOfRedPoison = resistOfRedPoison;
			base.SetModifiedAndInvalidateInfluencedCache(22, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				int dataSize = this._resistOfRedPoison.GetSerializedSize();
				byte* pData = OperationAdder.DynamicObjectCollection_SetDynamicField<int>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this._id, 21, dataSize);
				pData += this._resistOfRedPoison.Serialize(pData);
			}
		}

		// Token: 0x06002601 RID: 9729 RVA: 0x001D35B4 File Offset: 0x001D17B4
		public SpecialEffectList GetResistOfRottenPoison()
		{
			return this._resistOfRottenPoison;
		}

		// Token: 0x06002602 RID: 9730 RVA: 0x001D35CC File Offset: 0x001D17CC
		public unsafe void SetResistOfRottenPoison(SpecialEffectList resistOfRottenPoison, DataContext context)
		{
			this._resistOfRottenPoison = resistOfRottenPoison;
			base.SetModifiedAndInvalidateInfluencedCache(23, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				int dataSize = this._resistOfRottenPoison.GetSerializedSize();
				byte* pData = OperationAdder.DynamicObjectCollection_SetDynamicField<int>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this._id, 22, dataSize);
				pData += this._resistOfRottenPoison.Serialize(pData);
			}
		}

		// Token: 0x06002603 RID: 9731 RVA: 0x001D363C File Offset: 0x001D183C
		public SpecialEffectList GetResistOfIllusoryPoison()
		{
			return this._resistOfIllusoryPoison;
		}

		// Token: 0x06002604 RID: 9732 RVA: 0x001D3654 File Offset: 0x001D1854
		public unsafe void SetResistOfIllusoryPoison(SpecialEffectList resistOfIllusoryPoison, DataContext context)
		{
			this._resistOfIllusoryPoison = resistOfIllusoryPoison;
			base.SetModifiedAndInvalidateInfluencedCache(24, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				int dataSize = this._resistOfIllusoryPoison.GetSerializedSize();
				byte* pData = OperationAdder.DynamicObjectCollection_SetDynamicField<int>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this._id, 23, dataSize);
				pData += this._resistOfIllusoryPoison.Serialize(pData);
			}
		}

		// Token: 0x06002605 RID: 9733 RVA: 0x001D36C4 File Offset: 0x001D18C4
		public SpecialEffectList GetDisplayAge()
		{
			return this._displayAge;
		}

		// Token: 0x06002606 RID: 9734 RVA: 0x001D36DC File Offset: 0x001D18DC
		public unsafe void SetDisplayAge(SpecialEffectList displayAge, DataContext context)
		{
			this._displayAge = displayAge;
			base.SetModifiedAndInvalidateInfluencedCache(25, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				int dataSize = this._displayAge.GetSerializedSize();
				byte* pData = OperationAdder.DynamicObjectCollection_SetDynamicField<int>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this._id, 24, dataSize);
				pData += this._displayAge.Serialize(pData);
			}
		}

		// Token: 0x06002607 RID: 9735 RVA: 0x001D374C File Offset: 0x001D194C
		public SpecialEffectList GetNeiliProportionOfFiveElements()
		{
			return this._neiliProportionOfFiveElements;
		}

		// Token: 0x06002608 RID: 9736 RVA: 0x001D3764 File Offset: 0x001D1964
		public unsafe void SetNeiliProportionOfFiveElements(SpecialEffectList neiliProportionOfFiveElements, DataContext context)
		{
			this._neiliProportionOfFiveElements = neiliProportionOfFiveElements;
			base.SetModifiedAndInvalidateInfluencedCache(26, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				int dataSize = this._neiliProportionOfFiveElements.GetSerializedSize();
				byte* pData = OperationAdder.DynamicObjectCollection_SetDynamicField<int>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this._id, 25, dataSize);
				pData += this._neiliProportionOfFiveElements.Serialize(pData);
			}
		}

		// Token: 0x06002609 RID: 9737 RVA: 0x001D37D4 File Offset: 0x001D19D4
		public SpecialEffectList GetWeaponMaxPower()
		{
			return this._weaponMaxPower;
		}

		// Token: 0x0600260A RID: 9738 RVA: 0x001D37EC File Offset: 0x001D19EC
		public unsafe void SetWeaponMaxPower(SpecialEffectList weaponMaxPower, DataContext context)
		{
			this._weaponMaxPower = weaponMaxPower;
			base.SetModifiedAndInvalidateInfluencedCache(27, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				int dataSize = this._weaponMaxPower.GetSerializedSize();
				byte* pData = OperationAdder.DynamicObjectCollection_SetDynamicField<int>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this._id, 26, dataSize);
				pData += this._weaponMaxPower.Serialize(pData);
			}
		}

		// Token: 0x0600260B RID: 9739 RVA: 0x001D385C File Offset: 0x001D1A5C
		public SpecialEffectList GetWeaponUseRequirement()
		{
			return this._weaponUseRequirement;
		}

		// Token: 0x0600260C RID: 9740 RVA: 0x001D3874 File Offset: 0x001D1A74
		public unsafe void SetWeaponUseRequirement(SpecialEffectList weaponUseRequirement, DataContext context)
		{
			this._weaponUseRequirement = weaponUseRequirement;
			base.SetModifiedAndInvalidateInfluencedCache(28, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				int dataSize = this._weaponUseRequirement.GetSerializedSize();
				byte* pData = OperationAdder.DynamicObjectCollection_SetDynamicField<int>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this._id, 27, dataSize);
				pData += this._weaponUseRequirement.Serialize(pData);
			}
		}

		// Token: 0x0600260D RID: 9741 RVA: 0x001D38E4 File Offset: 0x001D1AE4
		public SpecialEffectList GetWeaponAttackRange()
		{
			return this._weaponAttackRange;
		}

		// Token: 0x0600260E RID: 9742 RVA: 0x001D38FC File Offset: 0x001D1AFC
		public unsafe void SetWeaponAttackRange(SpecialEffectList weaponAttackRange, DataContext context)
		{
			this._weaponAttackRange = weaponAttackRange;
			base.SetModifiedAndInvalidateInfluencedCache(29, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				int dataSize = this._weaponAttackRange.GetSerializedSize();
				byte* pData = OperationAdder.DynamicObjectCollection_SetDynamicField<int>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this._id, 28, dataSize);
				pData += this._weaponAttackRange.Serialize(pData);
			}
		}

		// Token: 0x0600260F RID: 9743 RVA: 0x001D396C File Offset: 0x001D1B6C
		public SpecialEffectList GetArmorMaxPower()
		{
			return this._armorMaxPower;
		}

		// Token: 0x06002610 RID: 9744 RVA: 0x001D3984 File Offset: 0x001D1B84
		public unsafe void SetArmorMaxPower(SpecialEffectList armorMaxPower, DataContext context)
		{
			this._armorMaxPower = armorMaxPower;
			base.SetModifiedAndInvalidateInfluencedCache(30, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				int dataSize = this._armorMaxPower.GetSerializedSize();
				byte* pData = OperationAdder.DynamicObjectCollection_SetDynamicField<int>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this._id, 29, dataSize);
				pData += this._armorMaxPower.Serialize(pData);
			}
		}

		// Token: 0x06002611 RID: 9745 RVA: 0x001D39F4 File Offset: 0x001D1BF4
		public SpecialEffectList GetArmorUseRequirement()
		{
			return this._armorUseRequirement;
		}

		// Token: 0x06002612 RID: 9746 RVA: 0x001D3A0C File Offset: 0x001D1C0C
		public unsafe void SetArmorUseRequirement(SpecialEffectList armorUseRequirement, DataContext context)
		{
			this._armorUseRequirement = armorUseRequirement;
			base.SetModifiedAndInvalidateInfluencedCache(31, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				int dataSize = this._armorUseRequirement.GetSerializedSize();
				byte* pData = OperationAdder.DynamicObjectCollection_SetDynamicField<int>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this._id, 30, dataSize);
				pData += this._armorUseRequirement.Serialize(pData);
			}
		}

		// Token: 0x06002613 RID: 9747 RVA: 0x001D3A7C File Offset: 0x001D1C7C
		public SpecialEffectList GetHitStrength()
		{
			return this._hitStrength;
		}

		// Token: 0x06002614 RID: 9748 RVA: 0x001D3A94 File Offset: 0x001D1C94
		public unsafe void SetHitStrength(SpecialEffectList hitStrength, DataContext context)
		{
			this._hitStrength = hitStrength;
			base.SetModifiedAndInvalidateInfluencedCache(32, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				int dataSize = this._hitStrength.GetSerializedSize();
				byte* pData = OperationAdder.DynamicObjectCollection_SetDynamicField<int>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this._id, 31, dataSize);
				pData += this._hitStrength.Serialize(pData);
			}
		}

		// Token: 0x06002615 RID: 9749 RVA: 0x001D3B04 File Offset: 0x001D1D04
		public SpecialEffectList GetHitTechnique()
		{
			return this._hitTechnique;
		}

		// Token: 0x06002616 RID: 9750 RVA: 0x001D3B1C File Offset: 0x001D1D1C
		public unsafe void SetHitTechnique(SpecialEffectList hitTechnique, DataContext context)
		{
			this._hitTechnique = hitTechnique;
			base.SetModifiedAndInvalidateInfluencedCache(33, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				int dataSize = this._hitTechnique.GetSerializedSize();
				byte* pData = OperationAdder.DynamicObjectCollection_SetDynamicField<int>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this._id, 32, dataSize);
				pData += this._hitTechnique.Serialize(pData);
			}
		}

		// Token: 0x06002617 RID: 9751 RVA: 0x001D3B8C File Offset: 0x001D1D8C
		public SpecialEffectList GetHitSpeed()
		{
			return this._hitSpeed;
		}

		// Token: 0x06002618 RID: 9752 RVA: 0x001D3BA4 File Offset: 0x001D1DA4
		public unsafe void SetHitSpeed(SpecialEffectList hitSpeed, DataContext context)
		{
			this._hitSpeed = hitSpeed;
			base.SetModifiedAndInvalidateInfluencedCache(34, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				int dataSize = this._hitSpeed.GetSerializedSize();
				byte* pData = OperationAdder.DynamicObjectCollection_SetDynamicField<int>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this._id, 33, dataSize);
				pData += this._hitSpeed.Serialize(pData);
			}
		}

		// Token: 0x06002619 RID: 9753 RVA: 0x001D3C14 File Offset: 0x001D1E14
		public SpecialEffectList GetHitMind()
		{
			return this._hitMind;
		}

		// Token: 0x0600261A RID: 9754 RVA: 0x001D3C2C File Offset: 0x001D1E2C
		public unsafe void SetHitMind(SpecialEffectList hitMind, DataContext context)
		{
			this._hitMind = hitMind;
			base.SetModifiedAndInvalidateInfluencedCache(35, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				int dataSize = this._hitMind.GetSerializedSize();
				byte* pData = OperationAdder.DynamicObjectCollection_SetDynamicField<int>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this._id, 34, dataSize);
				pData += this._hitMind.Serialize(pData);
			}
		}

		// Token: 0x0600261B RID: 9755 RVA: 0x001D3C9C File Offset: 0x001D1E9C
		public SpecialEffectList GetHitCanChange()
		{
			return this._hitCanChange;
		}

		// Token: 0x0600261C RID: 9756 RVA: 0x001D3CB4 File Offset: 0x001D1EB4
		public unsafe void SetHitCanChange(SpecialEffectList hitCanChange, DataContext context)
		{
			this._hitCanChange = hitCanChange;
			base.SetModifiedAndInvalidateInfluencedCache(36, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				int dataSize = this._hitCanChange.GetSerializedSize();
				byte* pData = OperationAdder.DynamicObjectCollection_SetDynamicField<int>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this._id, 35, dataSize);
				pData += this._hitCanChange.Serialize(pData);
			}
		}

		// Token: 0x0600261D RID: 9757 RVA: 0x001D3D24 File Offset: 0x001D1F24
		public SpecialEffectList GetHitChangeEffectPercent()
		{
			return this._hitChangeEffectPercent;
		}

		// Token: 0x0600261E RID: 9758 RVA: 0x001D3D3C File Offset: 0x001D1F3C
		public unsafe void SetHitChangeEffectPercent(SpecialEffectList hitChangeEffectPercent, DataContext context)
		{
			this._hitChangeEffectPercent = hitChangeEffectPercent;
			base.SetModifiedAndInvalidateInfluencedCache(37, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				int dataSize = this._hitChangeEffectPercent.GetSerializedSize();
				byte* pData = OperationAdder.DynamicObjectCollection_SetDynamicField<int>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this._id, 36, dataSize);
				pData += this._hitChangeEffectPercent.Serialize(pData);
			}
		}

		// Token: 0x0600261F RID: 9759 RVA: 0x001D3DAC File Offset: 0x001D1FAC
		public SpecialEffectList GetAvoidStrength()
		{
			return this._avoidStrength;
		}

		// Token: 0x06002620 RID: 9760 RVA: 0x001D3DC4 File Offset: 0x001D1FC4
		public unsafe void SetAvoidStrength(SpecialEffectList avoidStrength, DataContext context)
		{
			this._avoidStrength = avoidStrength;
			base.SetModifiedAndInvalidateInfluencedCache(38, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				int dataSize = this._avoidStrength.GetSerializedSize();
				byte* pData = OperationAdder.DynamicObjectCollection_SetDynamicField<int>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this._id, 37, dataSize);
				pData += this._avoidStrength.Serialize(pData);
			}
		}

		// Token: 0x06002621 RID: 9761 RVA: 0x001D3E34 File Offset: 0x001D2034
		public SpecialEffectList GetAvoidTechnique()
		{
			return this._avoidTechnique;
		}

		// Token: 0x06002622 RID: 9762 RVA: 0x001D3E4C File Offset: 0x001D204C
		public unsafe void SetAvoidTechnique(SpecialEffectList avoidTechnique, DataContext context)
		{
			this._avoidTechnique = avoidTechnique;
			base.SetModifiedAndInvalidateInfluencedCache(39, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				int dataSize = this._avoidTechnique.GetSerializedSize();
				byte* pData = OperationAdder.DynamicObjectCollection_SetDynamicField<int>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this._id, 38, dataSize);
				pData += this._avoidTechnique.Serialize(pData);
			}
		}

		// Token: 0x06002623 RID: 9763 RVA: 0x001D3EBC File Offset: 0x001D20BC
		public SpecialEffectList GetAvoidSpeed()
		{
			return this._avoidSpeed;
		}

		// Token: 0x06002624 RID: 9764 RVA: 0x001D3ED4 File Offset: 0x001D20D4
		public unsafe void SetAvoidSpeed(SpecialEffectList avoidSpeed, DataContext context)
		{
			this._avoidSpeed = avoidSpeed;
			base.SetModifiedAndInvalidateInfluencedCache(40, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				int dataSize = this._avoidSpeed.GetSerializedSize();
				byte* pData = OperationAdder.DynamicObjectCollection_SetDynamicField<int>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this._id, 39, dataSize);
				pData += this._avoidSpeed.Serialize(pData);
			}
		}

		// Token: 0x06002625 RID: 9765 RVA: 0x001D3F44 File Offset: 0x001D2144
		public SpecialEffectList GetAvoidMind()
		{
			return this._avoidMind;
		}

		// Token: 0x06002626 RID: 9766 RVA: 0x001D3F5C File Offset: 0x001D215C
		public unsafe void SetAvoidMind(SpecialEffectList avoidMind, DataContext context)
		{
			this._avoidMind = avoidMind;
			base.SetModifiedAndInvalidateInfluencedCache(41, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				int dataSize = this._avoidMind.GetSerializedSize();
				byte* pData = OperationAdder.DynamicObjectCollection_SetDynamicField<int>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this._id, 40, dataSize);
				pData += this._avoidMind.Serialize(pData);
			}
		}

		// Token: 0x06002627 RID: 9767 RVA: 0x001D3FCC File Offset: 0x001D21CC
		public SpecialEffectList GetAvoidCanChange()
		{
			return this._avoidCanChange;
		}

		// Token: 0x06002628 RID: 9768 RVA: 0x001D3FE4 File Offset: 0x001D21E4
		public unsafe void SetAvoidCanChange(SpecialEffectList avoidCanChange, DataContext context)
		{
			this._avoidCanChange = avoidCanChange;
			base.SetModifiedAndInvalidateInfluencedCache(42, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				int dataSize = this._avoidCanChange.GetSerializedSize();
				byte* pData = OperationAdder.DynamicObjectCollection_SetDynamicField<int>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this._id, 41, dataSize);
				pData += this._avoidCanChange.Serialize(pData);
			}
		}

		// Token: 0x06002629 RID: 9769 RVA: 0x001D4054 File Offset: 0x001D2254
		public SpecialEffectList GetAvoidChangeEffectPercent()
		{
			return this._avoidChangeEffectPercent;
		}

		// Token: 0x0600262A RID: 9770 RVA: 0x001D406C File Offset: 0x001D226C
		public unsafe void SetAvoidChangeEffectPercent(SpecialEffectList avoidChangeEffectPercent, DataContext context)
		{
			this._avoidChangeEffectPercent = avoidChangeEffectPercent;
			base.SetModifiedAndInvalidateInfluencedCache(43, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				int dataSize = this._avoidChangeEffectPercent.GetSerializedSize();
				byte* pData = OperationAdder.DynamicObjectCollection_SetDynamicField<int>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this._id, 42, dataSize);
				pData += this._avoidChangeEffectPercent.Serialize(pData);
			}
		}

		// Token: 0x0600262B RID: 9771 RVA: 0x001D40DC File Offset: 0x001D22DC
		public SpecialEffectList GetPenetrateOuter()
		{
			return this._penetrateOuter;
		}

		// Token: 0x0600262C RID: 9772 RVA: 0x001D40F4 File Offset: 0x001D22F4
		public unsafe void SetPenetrateOuter(SpecialEffectList penetrateOuter, DataContext context)
		{
			this._penetrateOuter = penetrateOuter;
			base.SetModifiedAndInvalidateInfluencedCache(44, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				int dataSize = this._penetrateOuter.GetSerializedSize();
				byte* pData = OperationAdder.DynamicObjectCollection_SetDynamicField<int>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this._id, 43, dataSize);
				pData += this._penetrateOuter.Serialize(pData);
			}
		}

		// Token: 0x0600262D RID: 9773 RVA: 0x001D4164 File Offset: 0x001D2364
		public SpecialEffectList GetPenetrateInner()
		{
			return this._penetrateInner;
		}

		// Token: 0x0600262E RID: 9774 RVA: 0x001D417C File Offset: 0x001D237C
		public unsafe void SetPenetrateInner(SpecialEffectList penetrateInner, DataContext context)
		{
			this._penetrateInner = penetrateInner;
			base.SetModifiedAndInvalidateInfluencedCache(45, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				int dataSize = this._penetrateInner.GetSerializedSize();
				byte* pData = OperationAdder.DynamicObjectCollection_SetDynamicField<int>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this._id, 44, dataSize);
				pData += this._penetrateInner.Serialize(pData);
			}
		}

		// Token: 0x0600262F RID: 9775 RVA: 0x001D41EC File Offset: 0x001D23EC
		public SpecialEffectList GetPenetrateResistOuter()
		{
			return this._penetrateResistOuter;
		}

		// Token: 0x06002630 RID: 9776 RVA: 0x001D4204 File Offset: 0x001D2404
		public unsafe void SetPenetrateResistOuter(SpecialEffectList penetrateResistOuter, DataContext context)
		{
			this._penetrateResistOuter = penetrateResistOuter;
			base.SetModifiedAndInvalidateInfluencedCache(46, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				int dataSize = this._penetrateResistOuter.GetSerializedSize();
				byte* pData = OperationAdder.DynamicObjectCollection_SetDynamicField<int>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this._id, 45, dataSize);
				pData += this._penetrateResistOuter.Serialize(pData);
			}
		}

		// Token: 0x06002631 RID: 9777 RVA: 0x001D4274 File Offset: 0x001D2474
		public SpecialEffectList GetPenetrateResistInner()
		{
			return this._penetrateResistInner;
		}

		// Token: 0x06002632 RID: 9778 RVA: 0x001D428C File Offset: 0x001D248C
		public unsafe void SetPenetrateResistInner(SpecialEffectList penetrateResistInner, DataContext context)
		{
			this._penetrateResistInner = penetrateResistInner;
			base.SetModifiedAndInvalidateInfluencedCache(47, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				int dataSize = this._penetrateResistInner.GetSerializedSize();
				byte* pData = OperationAdder.DynamicObjectCollection_SetDynamicField<int>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this._id, 46, dataSize);
				pData += this._penetrateResistInner.Serialize(pData);
			}
		}

		// Token: 0x06002633 RID: 9779 RVA: 0x001D42FC File Offset: 0x001D24FC
		public SpecialEffectList GetNeiliAllocationAttack()
		{
			return this._neiliAllocationAttack;
		}

		// Token: 0x06002634 RID: 9780 RVA: 0x001D4314 File Offset: 0x001D2514
		public unsafe void SetNeiliAllocationAttack(SpecialEffectList neiliAllocationAttack, DataContext context)
		{
			this._neiliAllocationAttack = neiliAllocationAttack;
			base.SetModifiedAndInvalidateInfluencedCache(48, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				int dataSize = this._neiliAllocationAttack.GetSerializedSize();
				byte* pData = OperationAdder.DynamicObjectCollection_SetDynamicField<int>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this._id, 47, dataSize);
				pData += this._neiliAllocationAttack.Serialize(pData);
			}
		}

		// Token: 0x06002635 RID: 9781 RVA: 0x001D4384 File Offset: 0x001D2584
		public SpecialEffectList GetNeiliAllocationAgile()
		{
			return this._neiliAllocationAgile;
		}

		// Token: 0x06002636 RID: 9782 RVA: 0x001D439C File Offset: 0x001D259C
		public unsafe void SetNeiliAllocationAgile(SpecialEffectList neiliAllocationAgile, DataContext context)
		{
			this._neiliAllocationAgile = neiliAllocationAgile;
			base.SetModifiedAndInvalidateInfluencedCache(49, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				int dataSize = this._neiliAllocationAgile.GetSerializedSize();
				byte* pData = OperationAdder.DynamicObjectCollection_SetDynamicField<int>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this._id, 48, dataSize);
				pData += this._neiliAllocationAgile.Serialize(pData);
			}
		}

		// Token: 0x06002637 RID: 9783 RVA: 0x001D440C File Offset: 0x001D260C
		public SpecialEffectList GetNeiliAllocationDefense()
		{
			return this._neiliAllocationDefense;
		}

		// Token: 0x06002638 RID: 9784 RVA: 0x001D4424 File Offset: 0x001D2624
		public unsafe void SetNeiliAllocationDefense(SpecialEffectList neiliAllocationDefense, DataContext context)
		{
			this._neiliAllocationDefense = neiliAllocationDefense;
			base.SetModifiedAndInvalidateInfluencedCache(50, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				int dataSize = this._neiliAllocationDefense.GetSerializedSize();
				byte* pData = OperationAdder.DynamicObjectCollection_SetDynamicField<int>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this._id, 49, dataSize);
				pData += this._neiliAllocationDefense.Serialize(pData);
			}
		}

		// Token: 0x06002639 RID: 9785 RVA: 0x001D4494 File Offset: 0x001D2694
		public SpecialEffectList GetNeiliAllocationAssist()
		{
			return this._neiliAllocationAssist;
		}

		// Token: 0x0600263A RID: 9786 RVA: 0x001D44AC File Offset: 0x001D26AC
		public unsafe void SetNeiliAllocationAssist(SpecialEffectList neiliAllocationAssist, DataContext context)
		{
			this._neiliAllocationAssist = neiliAllocationAssist;
			base.SetModifiedAndInvalidateInfluencedCache(51, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				int dataSize = this._neiliAllocationAssist.GetSerializedSize();
				byte* pData = OperationAdder.DynamicObjectCollection_SetDynamicField<int>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this._id, 50, dataSize);
				pData += this._neiliAllocationAssist.Serialize(pData);
			}
		}

		// Token: 0x0600263B RID: 9787 RVA: 0x001D451C File Offset: 0x001D271C
		public SpecialEffectList GetHappiness()
		{
			return this._happiness;
		}

		// Token: 0x0600263C RID: 9788 RVA: 0x001D4534 File Offset: 0x001D2734
		public unsafe void SetHappiness(SpecialEffectList happiness, DataContext context)
		{
			this._happiness = happiness;
			base.SetModifiedAndInvalidateInfluencedCache(52, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				int dataSize = this._happiness.GetSerializedSize();
				byte* pData = OperationAdder.DynamicObjectCollection_SetDynamicField<int>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this._id, 51, dataSize);
				pData += this._happiness.Serialize(pData);
			}
		}

		// Token: 0x0600263D RID: 9789 RVA: 0x001D45A4 File Offset: 0x001D27A4
		public SpecialEffectList GetMaxHealth()
		{
			return this._maxHealth;
		}

		// Token: 0x0600263E RID: 9790 RVA: 0x001D45BC File Offset: 0x001D27BC
		public unsafe void SetMaxHealth(SpecialEffectList maxHealth, DataContext context)
		{
			this._maxHealth = maxHealth;
			base.SetModifiedAndInvalidateInfluencedCache(53, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				int dataSize = this._maxHealth.GetSerializedSize();
				byte* pData = OperationAdder.DynamicObjectCollection_SetDynamicField<int>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this._id, 52, dataSize);
				pData += this._maxHealth.Serialize(pData);
			}
		}

		// Token: 0x0600263F RID: 9791 RVA: 0x001D462C File Offset: 0x001D282C
		public SpecialEffectList GetHealthCost()
		{
			return this._healthCost;
		}

		// Token: 0x06002640 RID: 9792 RVA: 0x001D4644 File Offset: 0x001D2844
		public unsafe void SetHealthCost(SpecialEffectList healthCost, DataContext context)
		{
			this._healthCost = healthCost;
			base.SetModifiedAndInvalidateInfluencedCache(54, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				int dataSize = this._healthCost.GetSerializedSize();
				byte* pData = OperationAdder.DynamicObjectCollection_SetDynamicField<int>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this._id, 53, dataSize);
				pData += this._healthCost.Serialize(pData);
			}
		}

		// Token: 0x06002641 RID: 9793 RVA: 0x001D46B4 File Offset: 0x001D28B4
		public SpecialEffectList GetMoveSpeedCanChange()
		{
			return this._moveSpeedCanChange;
		}

		// Token: 0x06002642 RID: 9794 RVA: 0x001D46CC File Offset: 0x001D28CC
		public unsafe void SetMoveSpeedCanChange(SpecialEffectList moveSpeedCanChange, DataContext context)
		{
			this._moveSpeedCanChange = moveSpeedCanChange;
			base.SetModifiedAndInvalidateInfluencedCache(55, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				int dataSize = this._moveSpeedCanChange.GetSerializedSize();
				byte* pData = OperationAdder.DynamicObjectCollection_SetDynamicField<int>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this._id, 54, dataSize);
				pData += this._moveSpeedCanChange.Serialize(pData);
			}
		}

		// Token: 0x06002643 RID: 9795 RVA: 0x001D473C File Offset: 0x001D293C
		public SpecialEffectList GetAttackerHitStrength()
		{
			return this._attackerHitStrength;
		}

		// Token: 0x06002644 RID: 9796 RVA: 0x001D4754 File Offset: 0x001D2954
		public unsafe void SetAttackerHitStrength(SpecialEffectList attackerHitStrength, DataContext context)
		{
			this._attackerHitStrength = attackerHitStrength;
			base.SetModifiedAndInvalidateInfluencedCache(56, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				int dataSize = this._attackerHitStrength.GetSerializedSize();
				byte* pData = OperationAdder.DynamicObjectCollection_SetDynamicField<int>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this._id, 55, dataSize);
				pData += this._attackerHitStrength.Serialize(pData);
			}
		}

		// Token: 0x06002645 RID: 9797 RVA: 0x001D47C4 File Offset: 0x001D29C4
		public SpecialEffectList GetAttackerHitTechnique()
		{
			return this._attackerHitTechnique;
		}

		// Token: 0x06002646 RID: 9798 RVA: 0x001D47DC File Offset: 0x001D29DC
		public unsafe void SetAttackerHitTechnique(SpecialEffectList attackerHitTechnique, DataContext context)
		{
			this._attackerHitTechnique = attackerHitTechnique;
			base.SetModifiedAndInvalidateInfluencedCache(57, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				int dataSize = this._attackerHitTechnique.GetSerializedSize();
				byte* pData = OperationAdder.DynamicObjectCollection_SetDynamicField<int>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this._id, 56, dataSize);
				pData += this._attackerHitTechnique.Serialize(pData);
			}
		}

		// Token: 0x06002647 RID: 9799 RVA: 0x001D484C File Offset: 0x001D2A4C
		public SpecialEffectList GetAttackerHitSpeed()
		{
			return this._attackerHitSpeed;
		}

		// Token: 0x06002648 RID: 9800 RVA: 0x001D4864 File Offset: 0x001D2A64
		public unsafe void SetAttackerHitSpeed(SpecialEffectList attackerHitSpeed, DataContext context)
		{
			this._attackerHitSpeed = attackerHitSpeed;
			base.SetModifiedAndInvalidateInfluencedCache(58, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				int dataSize = this._attackerHitSpeed.GetSerializedSize();
				byte* pData = OperationAdder.DynamicObjectCollection_SetDynamicField<int>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this._id, 57, dataSize);
				pData += this._attackerHitSpeed.Serialize(pData);
			}
		}

		// Token: 0x06002649 RID: 9801 RVA: 0x001D48D4 File Offset: 0x001D2AD4
		public SpecialEffectList GetAttackerHitMind()
		{
			return this._attackerHitMind;
		}

		// Token: 0x0600264A RID: 9802 RVA: 0x001D48EC File Offset: 0x001D2AEC
		public unsafe void SetAttackerHitMind(SpecialEffectList attackerHitMind, DataContext context)
		{
			this._attackerHitMind = attackerHitMind;
			base.SetModifiedAndInvalidateInfluencedCache(59, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				int dataSize = this._attackerHitMind.GetSerializedSize();
				byte* pData = OperationAdder.DynamicObjectCollection_SetDynamicField<int>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this._id, 58, dataSize);
				pData += this._attackerHitMind.Serialize(pData);
			}
		}

		// Token: 0x0600264B RID: 9803 RVA: 0x001D495C File Offset: 0x001D2B5C
		public SpecialEffectList GetAttackerAvoidStrength()
		{
			return this._attackerAvoidStrength;
		}

		// Token: 0x0600264C RID: 9804 RVA: 0x001D4974 File Offset: 0x001D2B74
		public unsafe void SetAttackerAvoidStrength(SpecialEffectList attackerAvoidStrength, DataContext context)
		{
			this._attackerAvoidStrength = attackerAvoidStrength;
			base.SetModifiedAndInvalidateInfluencedCache(60, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				int dataSize = this._attackerAvoidStrength.GetSerializedSize();
				byte* pData = OperationAdder.DynamicObjectCollection_SetDynamicField<int>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this._id, 59, dataSize);
				pData += this._attackerAvoidStrength.Serialize(pData);
			}
		}

		// Token: 0x0600264D RID: 9805 RVA: 0x001D49E4 File Offset: 0x001D2BE4
		public SpecialEffectList GetAttackerAvoidTechnique()
		{
			return this._attackerAvoidTechnique;
		}

		// Token: 0x0600264E RID: 9806 RVA: 0x001D49FC File Offset: 0x001D2BFC
		public unsafe void SetAttackerAvoidTechnique(SpecialEffectList attackerAvoidTechnique, DataContext context)
		{
			this._attackerAvoidTechnique = attackerAvoidTechnique;
			base.SetModifiedAndInvalidateInfluencedCache(61, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				int dataSize = this._attackerAvoidTechnique.GetSerializedSize();
				byte* pData = OperationAdder.DynamicObjectCollection_SetDynamicField<int>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this._id, 60, dataSize);
				pData += this._attackerAvoidTechnique.Serialize(pData);
			}
		}

		// Token: 0x0600264F RID: 9807 RVA: 0x001D4A6C File Offset: 0x001D2C6C
		public SpecialEffectList GetAttackerAvoidSpeed()
		{
			return this._attackerAvoidSpeed;
		}

		// Token: 0x06002650 RID: 9808 RVA: 0x001D4A84 File Offset: 0x001D2C84
		public unsafe void SetAttackerAvoidSpeed(SpecialEffectList attackerAvoidSpeed, DataContext context)
		{
			this._attackerAvoidSpeed = attackerAvoidSpeed;
			base.SetModifiedAndInvalidateInfluencedCache(62, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				int dataSize = this._attackerAvoidSpeed.GetSerializedSize();
				byte* pData = OperationAdder.DynamicObjectCollection_SetDynamicField<int>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this._id, 61, dataSize);
				pData += this._attackerAvoidSpeed.Serialize(pData);
			}
		}

		// Token: 0x06002651 RID: 9809 RVA: 0x001D4AF4 File Offset: 0x001D2CF4
		public SpecialEffectList GetAttackerAvoidMind()
		{
			return this._attackerAvoidMind;
		}

		// Token: 0x06002652 RID: 9810 RVA: 0x001D4B0C File Offset: 0x001D2D0C
		public unsafe void SetAttackerAvoidMind(SpecialEffectList attackerAvoidMind, DataContext context)
		{
			this._attackerAvoidMind = attackerAvoidMind;
			base.SetModifiedAndInvalidateInfluencedCache(63, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				int dataSize = this._attackerAvoidMind.GetSerializedSize();
				byte* pData = OperationAdder.DynamicObjectCollection_SetDynamicField<int>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this._id, 62, dataSize);
				pData += this._attackerAvoidMind.Serialize(pData);
			}
		}

		// Token: 0x06002653 RID: 9811 RVA: 0x001D4B7C File Offset: 0x001D2D7C
		public SpecialEffectList GetAttackerPenetrateOuter()
		{
			return this._attackerPenetrateOuter;
		}

		// Token: 0x06002654 RID: 9812 RVA: 0x001D4B94 File Offset: 0x001D2D94
		public unsafe void SetAttackerPenetrateOuter(SpecialEffectList attackerPenetrateOuter, DataContext context)
		{
			this._attackerPenetrateOuter = attackerPenetrateOuter;
			base.SetModifiedAndInvalidateInfluencedCache(64, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				int dataSize = this._attackerPenetrateOuter.GetSerializedSize();
				byte* pData = OperationAdder.DynamicObjectCollection_SetDynamicField<int>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this._id, 63, dataSize);
				pData += this._attackerPenetrateOuter.Serialize(pData);
			}
		}

		// Token: 0x06002655 RID: 9813 RVA: 0x001D4C04 File Offset: 0x001D2E04
		public SpecialEffectList GetAttackerPenetrateInner()
		{
			return this._attackerPenetrateInner;
		}

		// Token: 0x06002656 RID: 9814 RVA: 0x001D4C1C File Offset: 0x001D2E1C
		public unsafe void SetAttackerPenetrateInner(SpecialEffectList attackerPenetrateInner, DataContext context)
		{
			this._attackerPenetrateInner = attackerPenetrateInner;
			base.SetModifiedAndInvalidateInfluencedCache(65, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				int dataSize = this._attackerPenetrateInner.GetSerializedSize();
				byte* pData = OperationAdder.DynamicObjectCollection_SetDynamicField<int>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this._id, 64, dataSize);
				pData += this._attackerPenetrateInner.Serialize(pData);
			}
		}

		// Token: 0x06002657 RID: 9815 RVA: 0x001D4C8C File Offset: 0x001D2E8C
		public SpecialEffectList GetAttackerPenetrateResistOuter()
		{
			return this._attackerPenetrateResistOuter;
		}

		// Token: 0x06002658 RID: 9816 RVA: 0x001D4CA4 File Offset: 0x001D2EA4
		public unsafe void SetAttackerPenetrateResistOuter(SpecialEffectList attackerPenetrateResistOuter, DataContext context)
		{
			this._attackerPenetrateResistOuter = attackerPenetrateResistOuter;
			base.SetModifiedAndInvalidateInfluencedCache(66, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				int dataSize = this._attackerPenetrateResistOuter.GetSerializedSize();
				byte* pData = OperationAdder.DynamicObjectCollection_SetDynamicField<int>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this._id, 65, dataSize);
				pData += this._attackerPenetrateResistOuter.Serialize(pData);
			}
		}

		// Token: 0x06002659 RID: 9817 RVA: 0x001D4D14 File Offset: 0x001D2F14
		public SpecialEffectList GetAttackerPenetrateResistInner()
		{
			return this._attackerPenetrateResistInner;
		}

		// Token: 0x0600265A RID: 9818 RVA: 0x001D4D2C File Offset: 0x001D2F2C
		public unsafe void SetAttackerPenetrateResistInner(SpecialEffectList attackerPenetrateResistInner, DataContext context)
		{
			this._attackerPenetrateResistInner = attackerPenetrateResistInner;
			base.SetModifiedAndInvalidateInfluencedCache(67, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				int dataSize = this._attackerPenetrateResistInner.GetSerializedSize();
				byte* pData = OperationAdder.DynamicObjectCollection_SetDynamicField<int>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this._id, 66, dataSize);
				pData += this._attackerPenetrateResistInner.Serialize(pData);
			}
		}

		// Token: 0x0600265B RID: 9819 RVA: 0x001D4D9C File Offset: 0x001D2F9C
		public SpecialEffectList GetAttackHitType()
		{
			return this._attackHitType;
		}

		// Token: 0x0600265C RID: 9820 RVA: 0x001D4DB4 File Offset: 0x001D2FB4
		public unsafe void SetAttackHitType(SpecialEffectList attackHitType, DataContext context)
		{
			this._attackHitType = attackHitType;
			base.SetModifiedAndInvalidateInfluencedCache(68, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				int dataSize = this._attackHitType.GetSerializedSize();
				byte* pData = OperationAdder.DynamicObjectCollection_SetDynamicField<int>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this._id, 67, dataSize);
				pData += this._attackHitType.Serialize(pData);
			}
		}

		// Token: 0x0600265D RID: 9821 RVA: 0x001D4E24 File Offset: 0x001D3024
		public SpecialEffectList GetMakeDirectDamage()
		{
			return this._makeDirectDamage;
		}

		// Token: 0x0600265E RID: 9822 RVA: 0x001D4E3C File Offset: 0x001D303C
		public unsafe void SetMakeDirectDamage(SpecialEffectList makeDirectDamage, DataContext context)
		{
			this._makeDirectDamage = makeDirectDamage;
			base.SetModifiedAndInvalidateInfluencedCache(69, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				int dataSize = this._makeDirectDamage.GetSerializedSize();
				byte* pData = OperationAdder.DynamicObjectCollection_SetDynamicField<int>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this._id, 68, dataSize);
				pData += this._makeDirectDamage.Serialize(pData);
			}
		}

		// Token: 0x0600265F RID: 9823 RVA: 0x001D4EAC File Offset: 0x001D30AC
		public SpecialEffectList GetMakeBounceDamage()
		{
			return this._makeBounceDamage;
		}

		// Token: 0x06002660 RID: 9824 RVA: 0x001D4EC4 File Offset: 0x001D30C4
		public unsafe void SetMakeBounceDamage(SpecialEffectList makeBounceDamage, DataContext context)
		{
			this._makeBounceDamage = makeBounceDamage;
			base.SetModifiedAndInvalidateInfluencedCache(70, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				int dataSize = this._makeBounceDamage.GetSerializedSize();
				byte* pData = OperationAdder.DynamicObjectCollection_SetDynamicField<int>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this._id, 69, dataSize);
				pData += this._makeBounceDamage.Serialize(pData);
			}
		}

		// Token: 0x06002661 RID: 9825 RVA: 0x001D4F34 File Offset: 0x001D3134
		public SpecialEffectList GetMakeFightBackDamage()
		{
			return this._makeFightBackDamage;
		}

		// Token: 0x06002662 RID: 9826 RVA: 0x001D4F4C File Offset: 0x001D314C
		public unsafe void SetMakeFightBackDamage(SpecialEffectList makeFightBackDamage, DataContext context)
		{
			this._makeFightBackDamage = makeFightBackDamage;
			base.SetModifiedAndInvalidateInfluencedCache(71, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				int dataSize = this._makeFightBackDamage.GetSerializedSize();
				byte* pData = OperationAdder.DynamicObjectCollection_SetDynamicField<int>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this._id, 70, dataSize);
				pData += this._makeFightBackDamage.Serialize(pData);
			}
		}

		// Token: 0x06002663 RID: 9827 RVA: 0x001D4FBC File Offset: 0x001D31BC
		public SpecialEffectList GetMakePoisonLevel()
		{
			return this._makePoisonLevel;
		}

		// Token: 0x06002664 RID: 9828 RVA: 0x001D4FD4 File Offset: 0x001D31D4
		public unsafe void SetMakePoisonLevel(SpecialEffectList makePoisonLevel, DataContext context)
		{
			this._makePoisonLevel = makePoisonLevel;
			base.SetModifiedAndInvalidateInfluencedCache(72, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				int dataSize = this._makePoisonLevel.GetSerializedSize();
				byte* pData = OperationAdder.DynamicObjectCollection_SetDynamicField<int>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this._id, 71, dataSize);
				pData += this._makePoisonLevel.Serialize(pData);
			}
		}

		// Token: 0x06002665 RID: 9829 RVA: 0x001D5044 File Offset: 0x001D3244
		public SpecialEffectList GetMakePoisonValue()
		{
			return this._makePoisonValue;
		}

		// Token: 0x06002666 RID: 9830 RVA: 0x001D505C File Offset: 0x001D325C
		public unsafe void SetMakePoisonValue(SpecialEffectList makePoisonValue, DataContext context)
		{
			this._makePoisonValue = makePoisonValue;
			base.SetModifiedAndInvalidateInfluencedCache(73, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				int dataSize = this._makePoisonValue.GetSerializedSize();
				byte* pData = OperationAdder.DynamicObjectCollection_SetDynamicField<int>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this._id, 72, dataSize);
				pData += this._makePoisonValue.Serialize(pData);
			}
		}

		// Token: 0x06002667 RID: 9831 RVA: 0x001D50CC File Offset: 0x001D32CC
		public SpecialEffectList GetAttackerHitOdds()
		{
			return this._attackerHitOdds;
		}

		// Token: 0x06002668 RID: 9832 RVA: 0x001D50E4 File Offset: 0x001D32E4
		public unsafe void SetAttackerHitOdds(SpecialEffectList attackerHitOdds, DataContext context)
		{
			this._attackerHitOdds = attackerHitOdds;
			base.SetModifiedAndInvalidateInfluencedCache(74, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				int dataSize = this._attackerHitOdds.GetSerializedSize();
				byte* pData = OperationAdder.DynamicObjectCollection_SetDynamicField<int>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this._id, 73, dataSize);
				pData += this._attackerHitOdds.Serialize(pData);
			}
		}

		// Token: 0x06002669 RID: 9833 RVA: 0x001D5154 File Offset: 0x001D3354
		public SpecialEffectList GetAttackerFightBackHitOdds()
		{
			return this._attackerFightBackHitOdds;
		}

		// Token: 0x0600266A RID: 9834 RVA: 0x001D516C File Offset: 0x001D336C
		public unsafe void SetAttackerFightBackHitOdds(SpecialEffectList attackerFightBackHitOdds, DataContext context)
		{
			this._attackerFightBackHitOdds = attackerFightBackHitOdds;
			base.SetModifiedAndInvalidateInfluencedCache(75, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				int dataSize = this._attackerFightBackHitOdds.GetSerializedSize();
				byte* pData = OperationAdder.DynamicObjectCollection_SetDynamicField<int>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this._id, 74, dataSize);
				pData += this._attackerFightBackHitOdds.Serialize(pData);
			}
		}

		// Token: 0x0600266B RID: 9835 RVA: 0x001D51DC File Offset: 0x001D33DC
		public SpecialEffectList GetAttackerPursueOdds()
		{
			return this._attackerPursueOdds;
		}

		// Token: 0x0600266C RID: 9836 RVA: 0x001D51F4 File Offset: 0x001D33F4
		public unsafe void SetAttackerPursueOdds(SpecialEffectList attackerPursueOdds, DataContext context)
		{
			this._attackerPursueOdds = attackerPursueOdds;
			base.SetModifiedAndInvalidateInfluencedCache(76, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				int dataSize = this._attackerPursueOdds.GetSerializedSize();
				byte* pData = OperationAdder.DynamicObjectCollection_SetDynamicField<int>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this._id, 75, dataSize);
				pData += this._attackerPursueOdds.Serialize(pData);
			}
		}

		// Token: 0x0600266D RID: 9837 RVA: 0x001D5264 File Offset: 0x001D3464
		public SpecialEffectList GetMakedInjuryChangeToOld()
		{
			return this._makedInjuryChangeToOld;
		}

		// Token: 0x0600266E RID: 9838 RVA: 0x001D527C File Offset: 0x001D347C
		public unsafe void SetMakedInjuryChangeToOld(SpecialEffectList makedInjuryChangeToOld, DataContext context)
		{
			this._makedInjuryChangeToOld = makedInjuryChangeToOld;
			base.SetModifiedAndInvalidateInfluencedCache(77, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				int dataSize = this._makedInjuryChangeToOld.GetSerializedSize();
				byte* pData = OperationAdder.DynamicObjectCollection_SetDynamicField<int>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this._id, 76, dataSize);
				pData += this._makedInjuryChangeToOld.Serialize(pData);
			}
		}

		// Token: 0x0600266F RID: 9839 RVA: 0x001D52EC File Offset: 0x001D34EC
		public SpecialEffectList GetMakedPoisonChangeToOld()
		{
			return this._makedPoisonChangeToOld;
		}

		// Token: 0x06002670 RID: 9840 RVA: 0x001D5304 File Offset: 0x001D3504
		public unsafe void SetMakedPoisonChangeToOld(SpecialEffectList makedPoisonChangeToOld, DataContext context)
		{
			this._makedPoisonChangeToOld = makedPoisonChangeToOld;
			base.SetModifiedAndInvalidateInfluencedCache(78, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				int dataSize = this._makedPoisonChangeToOld.GetSerializedSize();
				byte* pData = OperationAdder.DynamicObjectCollection_SetDynamicField<int>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this._id, 77, dataSize);
				pData += this._makedPoisonChangeToOld.Serialize(pData);
			}
		}

		// Token: 0x06002671 RID: 9841 RVA: 0x001D5374 File Offset: 0x001D3574
		public SpecialEffectList GetMakeDamageType()
		{
			return this._makeDamageType;
		}

		// Token: 0x06002672 RID: 9842 RVA: 0x001D538C File Offset: 0x001D358C
		public unsafe void SetMakeDamageType(SpecialEffectList makeDamageType, DataContext context)
		{
			this._makeDamageType = makeDamageType;
			base.SetModifiedAndInvalidateInfluencedCache(79, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				int dataSize = this._makeDamageType.GetSerializedSize();
				byte* pData = OperationAdder.DynamicObjectCollection_SetDynamicField<int>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this._id, 78, dataSize);
				pData += this._makeDamageType.Serialize(pData);
			}
		}

		// Token: 0x06002673 RID: 9843 RVA: 0x001D53FC File Offset: 0x001D35FC
		public SpecialEffectList GetCanMakeInjuryToNoInjuryPart()
		{
			return this._canMakeInjuryToNoInjuryPart;
		}

		// Token: 0x06002674 RID: 9844 RVA: 0x001D5414 File Offset: 0x001D3614
		public unsafe void SetCanMakeInjuryToNoInjuryPart(SpecialEffectList canMakeInjuryToNoInjuryPart, DataContext context)
		{
			this._canMakeInjuryToNoInjuryPart = canMakeInjuryToNoInjuryPart;
			base.SetModifiedAndInvalidateInfluencedCache(80, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				int dataSize = this._canMakeInjuryToNoInjuryPart.GetSerializedSize();
				byte* pData = OperationAdder.DynamicObjectCollection_SetDynamicField<int>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this._id, 79, dataSize);
				pData += this._canMakeInjuryToNoInjuryPart.Serialize(pData);
			}
		}

		// Token: 0x06002675 RID: 9845 RVA: 0x001D5484 File Offset: 0x001D3684
		public SpecialEffectList GetMakePoisonType()
		{
			return this._makePoisonType;
		}

		// Token: 0x06002676 RID: 9846 RVA: 0x001D549C File Offset: 0x001D369C
		public unsafe void SetMakePoisonType(SpecialEffectList makePoisonType, DataContext context)
		{
			this._makePoisonType = makePoisonType;
			base.SetModifiedAndInvalidateInfluencedCache(81, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				int dataSize = this._makePoisonType.GetSerializedSize();
				byte* pData = OperationAdder.DynamicObjectCollection_SetDynamicField<int>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this._id, 80, dataSize);
				pData += this._makePoisonType.Serialize(pData);
			}
		}

		// Token: 0x06002677 RID: 9847 RVA: 0x001D550C File Offset: 0x001D370C
		public SpecialEffectList GetNormalAttackWeapon()
		{
			return this._normalAttackWeapon;
		}

		// Token: 0x06002678 RID: 9848 RVA: 0x001D5524 File Offset: 0x001D3724
		public unsafe void SetNormalAttackWeapon(SpecialEffectList normalAttackWeapon, DataContext context)
		{
			this._normalAttackWeapon = normalAttackWeapon;
			base.SetModifiedAndInvalidateInfluencedCache(82, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				int dataSize = this._normalAttackWeapon.GetSerializedSize();
				byte* pData = OperationAdder.DynamicObjectCollection_SetDynamicField<int>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this._id, 81, dataSize);
				pData += this._normalAttackWeapon.Serialize(pData);
			}
		}

		// Token: 0x06002679 RID: 9849 RVA: 0x001D5594 File Offset: 0x001D3794
		public SpecialEffectList GetNormalAttackTrick()
		{
			return this._normalAttackTrick;
		}

		// Token: 0x0600267A RID: 9850 RVA: 0x001D55AC File Offset: 0x001D37AC
		public unsafe void SetNormalAttackTrick(SpecialEffectList normalAttackTrick, DataContext context)
		{
			this._normalAttackTrick = normalAttackTrick;
			base.SetModifiedAndInvalidateInfluencedCache(83, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				int dataSize = this._normalAttackTrick.GetSerializedSize();
				byte* pData = OperationAdder.DynamicObjectCollection_SetDynamicField<int>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this._id, 82, dataSize);
				pData += this._normalAttackTrick.Serialize(pData);
			}
		}

		// Token: 0x0600267B RID: 9851 RVA: 0x001D561C File Offset: 0x001D381C
		public SpecialEffectList GetExtraFlawCount()
		{
			return this._extraFlawCount;
		}

		// Token: 0x0600267C RID: 9852 RVA: 0x001D5634 File Offset: 0x001D3834
		public unsafe void SetExtraFlawCount(SpecialEffectList extraFlawCount, DataContext context)
		{
			this._extraFlawCount = extraFlawCount;
			base.SetModifiedAndInvalidateInfluencedCache(84, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				int dataSize = this._extraFlawCount.GetSerializedSize();
				byte* pData = OperationAdder.DynamicObjectCollection_SetDynamicField<int>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this._id, 83, dataSize);
				pData += this._extraFlawCount.Serialize(pData);
			}
		}

		// Token: 0x0600267D RID: 9853 RVA: 0x001D56A4 File Offset: 0x001D38A4
		public SpecialEffectList GetAttackCanBounce()
		{
			return this._attackCanBounce;
		}

		// Token: 0x0600267E RID: 9854 RVA: 0x001D56BC File Offset: 0x001D38BC
		public unsafe void SetAttackCanBounce(SpecialEffectList attackCanBounce, DataContext context)
		{
			this._attackCanBounce = attackCanBounce;
			base.SetModifiedAndInvalidateInfluencedCache(85, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				int dataSize = this._attackCanBounce.GetSerializedSize();
				byte* pData = OperationAdder.DynamicObjectCollection_SetDynamicField<int>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this._id, 84, dataSize);
				pData += this._attackCanBounce.Serialize(pData);
			}
		}

		// Token: 0x0600267F RID: 9855 RVA: 0x001D572C File Offset: 0x001D392C
		public SpecialEffectList GetAttackCanFightBack()
		{
			return this._attackCanFightBack;
		}

		// Token: 0x06002680 RID: 9856 RVA: 0x001D5744 File Offset: 0x001D3944
		public unsafe void SetAttackCanFightBack(SpecialEffectList attackCanFightBack, DataContext context)
		{
			this._attackCanFightBack = attackCanFightBack;
			base.SetModifiedAndInvalidateInfluencedCache(86, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				int dataSize = this._attackCanFightBack.GetSerializedSize();
				byte* pData = OperationAdder.DynamicObjectCollection_SetDynamicField<int>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this._id, 85, dataSize);
				pData += this._attackCanFightBack.Serialize(pData);
			}
		}

		// Token: 0x06002681 RID: 9857 RVA: 0x001D57B4 File Offset: 0x001D39B4
		public SpecialEffectList GetMakeFightBackInjuryMark()
		{
			return this._makeFightBackInjuryMark;
		}

		// Token: 0x06002682 RID: 9858 RVA: 0x001D57CC File Offset: 0x001D39CC
		public unsafe void SetMakeFightBackInjuryMark(SpecialEffectList makeFightBackInjuryMark, DataContext context)
		{
			this._makeFightBackInjuryMark = makeFightBackInjuryMark;
			base.SetModifiedAndInvalidateInfluencedCache(87, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				int dataSize = this._makeFightBackInjuryMark.GetSerializedSize();
				byte* pData = OperationAdder.DynamicObjectCollection_SetDynamicField<int>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this._id, 86, dataSize);
				pData += this._makeFightBackInjuryMark.Serialize(pData);
			}
		}

		// Token: 0x06002683 RID: 9859 RVA: 0x001D583C File Offset: 0x001D3A3C
		public SpecialEffectList GetLegSkillUseShoes()
		{
			return this._legSkillUseShoes;
		}

		// Token: 0x06002684 RID: 9860 RVA: 0x001D5854 File Offset: 0x001D3A54
		public unsafe void SetLegSkillUseShoes(SpecialEffectList legSkillUseShoes, DataContext context)
		{
			this._legSkillUseShoes = legSkillUseShoes;
			base.SetModifiedAndInvalidateInfluencedCache(88, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				int dataSize = this._legSkillUseShoes.GetSerializedSize();
				byte* pData = OperationAdder.DynamicObjectCollection_SetDynamicField<int>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this._id, 87, dataSize);
				pData += this._legSkillUseShoes.Serialize(pData);
			}
		}

		// Token: 0x06002685 RID: 9861 RVA: 0x001D58C4 File Offset: 0x001D3AC4
		public SpecialEffectList GetAttackerFinalDamageValue()
		{
			return this._attackerFinalDamageValue;
		}

		// Token: 0x06002686 RID: 9862 RVA: 0x001D58DC File Offset: 0x001D3ADC
		public unsafe void SetAttackerFinalDamageValue(SpecialEffectList attackerFinalDamageValue, DataContext context)
		{
			this._attackerFinalDamageValue = attackerFinalDamageValue;
			base.SetModifiedAndInvalidateInfluencedCache(89, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				int dataSize = this._attackerFinalDamageValue.GetSerializedSize();
				byte* pData = OperationAdder.DynamicObjectCollection_SetDynamicField<int>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this._id, 88, dataSize);
				pData += this._attackerFinalDamageValue.Serialize(pData);
			}
		}

		// Token: 0x06002687 RID: 9863 RVA: 0x001D594C File Offset: 0x001D3B4C
		public SpecialEffectList GetDefenderHitStrength()
		{
			return this._defenderHitStrength;
		}

		// Token: 0x06002688 RID: 9864 RVA: 0x001D5964 File Offset: 0x001D3B64
		public unsafe void SetDefenderHitStrength(SpecialEffectList defenderHitStrength, DataContext context)
		{
			this._defenderHitStrength = defenderHitStrength;
			base.SetModifiedAndInvalidateInfluencedCache(90, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				int dataSize = this._defenderHitStrength.GetSerializedSize();
				byte* pData = OperationAdder.DynamicObjectCollection_SetDynamicField<int>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this._id, 89, dataSize);
				pData += this._defenderHitStrength.Serialize(pData);
			}
		}

		// Token: 0x06002689 RID: 9865 RVA: 0x001D59D4 File Offset: 0x001D3BD4
		public SpecialEffectList GetDefenderHitTechnique()
		{
			return this._defenderHitTechnique;
		}

		// Token: 0x0600268A RID: 9866 RVA: 0x001D59EC File Offset: 0x001D3BEC
		public unsafe void SetDefenderHitTechnique(SpecialEffectList defenderHitTechnique, DataContext context)
		{
			this._defenderHitTechnique = defenderHitTechnique;
			base.SetModifiedAndInvalidateInfluencedCache(91, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				int dataSize = this._defenderHitTechnique.GetSerializedSize();
				byte* pData = OperationAdder.DynamicObjectCollection_SetDynamicField<int>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this._id, 90, dataSize);
				pData += this._defenderHitTechnique.Serialize(pData);
			}
		}

		// Token: 0x0600268B RID: 9867 RVA: 0x001D5A5C File Offset: 0x001D3C5C
		public SpecialEffectList GetDefenderHitSpeed()
		{
			return this._defenderHitSpeed;
		}

		// Token: 0x0600268C RID: 9868 RVA: 0x001D5A74 File Offset: 0x001D3C74
		public unsafe void SetDefenderHitSpeed(SpecialEffectList defenderHitSpeed, DataContext context)
		{
			this._defenderHitSpeed = defenderHitSpeed;
			base.SetModifiedAndInvalidateInfluencedCache(92, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				int dataSize = this._defenderHitSpeed.GetSerializedSize();
				byte* pData = OperationAdder.DynamicObjectCollection_SetDynamicField<int>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this._id, 91, dataSize);
				pData += this._defenderHitSpeed.Serialize(pData);
			}
		}

		// Token: 0x0600268D RID: 9869 RVA: 0x001D5AE4 File Offset: 0x001D3CE4
		public SpecialEffectList GetDefenderHitMind()
		{
			return this._defenderHitMind;
		}

		// Token: 0x0600268E RID: 9870 RVA: 0x001D5AFC File Offset: 0x001D3CFC
		public unsafe void SetDefenderHitMind(SpecialEffectList defenderHitMind, DataContext context)
		{
			this._defenderHitMind = defenderHitMind;
			base.SetModifiedAndInvalidateInfluencedCache(93, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				int dataSize = this._defenderHitMind.GetSerializedSize();
				byte* pData = OperationAdder.DynamicObjectCollection_SetDynamicField<int>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this._id, 92, dataSize);
				pData += this._defenderHitMind.Serialize(pData);
			}
		}

		// Token: 0x0600268F RID: 9871 RVA: 0x001D5B6C File Offset: 0x001D3D6C
		public SpecialEffectList GetDefenderAvoidStrength()
		{
			return this._defenderAvoidStrength;
		}

		// Token: 0x06002690 RID: 9872 RVA: 0x001D5B84 File Offset: 0x001D3D84
		public unsafe void SetDefenderAvoidStrength(SpecialEffectList defenderAvoidStrength, DataContext context)
		{
			this._defenderAvoidStrength = defenderAvoidStrength;
			base.SetModifiedAndInvalidateInfluencedCache(94, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				int dataSize = this._defenderAvoidStrength.GetSerializedSize();
				byte* pData = OperationAdder.DynamicObjectCollection_SetDynamicField<int>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this._id, 93, dataSize);
				pData += this._defenderAvoidStrength.Serialize(pData);
			}
		}

		// Token: 0x06002691 RID: 9873 RVA: 0x001D5BF4 File Offset: 0x001D3DF4
		public SpecialEffectList GetDefenderAvoidTechnique()
		{
			return this._defenderAvoidTechnique;
		}

		// Token: 0x06002692 RID: 9874 RVA: 0x001D5C0C File Offset: 0x001D3E0C
		public unsafe void SetDefenderAvoidTechnique(SpecialEffectList defenderAvoidTechnique, DataContext context)
		{
			this._defenderAvoidTechnique = defenderAvoidTechnique;
			base.SetModifiedAndInvalidateInfluencedCache(95, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				int dataSize = this._defenderAvoidTechnique.GetSerializedSize();
				byte* pData = OperationAdder.DynamicObjectCollection_SetDynamicField<int>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this._id, 94, dataSize);
				pData += this._defenderAvoidTechnique.Serialize(pData);
			}
		}

		// Token: 0x06002693 RID: 9875 RVA: 0x001D5C7C File Offset: 0x001D3E7C
		public SpecialEffectList GetDefenderAvoidSpeed()
		{
			return this._defenderAvoidSpeed;
		}

		// Token: 0x06002694 RID: 9876 RVA: 0x001D5C94 File Offset: 0x001D3E94
		public unsafe void SetDefenderAvoidSpeed(SpecialEffectList defenderAvoidSpeed, DataContext context)
		{
			this._defenderAvoidSpeed = defenderAvoidSpeed;
			base.SetModifiedAndInvalidateInfluencedCache(96, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				int dataSize = this._defenderAvoidSpeed.GetSerializedSize();
				byte* pData = OperationAdder.DynamicObjectCollection_SetDynamicField<int>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this._id, 95, dataSize);
				pData += this._defenderAvoidSpeed.Serialize(pData);
			}
		}

		// Token: 0x06002695 RID: 9877 RVA: 0x001D5D04 File Offset: 0x001D3F04
		public SpecialEffectList GetDefenderAvoidMind()
		{
			return this._defenderAvoidMind;
		}

		// Token: 0x06002696 RID: 9878 RVA: 0x001D5D1C File Offset: 0x001D3F1C
		public unsafe void SetDefenderAvoidMind(SpecialEffectList defenderAvoidMind, DataContext context)
		{
			this._defenderAvoidMind = defenderAvoidMind;
			base.SetModifiedAndInvalidateInfluencedCache(97, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				int dataSize = this._defenderAvoidMind.GetSerializedSize();
				byte* pData = OperationAdder.DynamicObjectCollection_SetDynamicField<int>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this._id, 96, dataSize);
				pData += this._defenderAvoidMind.Serialize(pData);
			}
		}

		// Token: 0x06002697 RID: 9879 RVA: 0x001D5D8C File Offset: 0x001D3F8C
		public SpecialEffectList GetDefenderPenetrateOuter()
		{
			return this._defenderPenetrateOuter;
		}

		// Token: 0x06002698 RID: 9880 RVA: 0x001D5DA4 File Offset: 0x001D3FA4
		public unsafe void SetDefenderPenetrateOuter(SpecialEffectList defenderPenetrateOuter, DataContext context)
		{
			this._defenderPenetrateOuter = defenderPenetrateOuter;
			base.SetModifiedAndInvalidateInfluencedCache(98, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				int dataSize = this._defenderPenetrateOuter.GetSerializedSize();
				byte* pData = OperationAdder.DynamicObjectCollection_SetDynamicField<int>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this._id, 97, dataSize);
				pData += this._defenderPenetrateOuter.Serialize(pData);
			}
		}

		// Token: 0x06002699 RID: 9881 RVA: 0x001D5E14 File Offset: 0x001D4014
		public SpecialEffectList GetDefenderPenetrateInner()
		{
			return this._defenderPenetrateInner;
		}

		// Token: 0x0600269A RID: 9882 RVA: 0x001D5E2C File Offset: 0x001D402C
		public unsafe void SetDefenderPenetrateInner(SpecialEffectList defenderPenetrateInner, DataContext context)
		{
			this._defenderPenetrateInner = defenderPenetrateInner;
			base.SetModifiedAndInvalidateInfluencedCache(99, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				int dataSize = this._defenderPenetrateInner.GetSerializedSize();
				byte* pData = OperationAdder.DynamicObjectCollection_SetDynamicField<int>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this._id, 98, dataSize);
				pData += this._defenderPenetrateInner.Serialize(pData);
			}
		}

		// Token: 0x0600269B RID: 9883 RVA: 0x001D5E9C File Offset: 0x001D409C
		public SpecialEffectList GetDefenderPenetrateResistOuter()
		{
			return this._defenderPenetrateResistOuter;
		}

		// Token: 0x0600269C RID: 9884 RVA: 0x001D5EB4 File Offset: 0x001D40B4
		public unsafe void SetDefenderPenetrateResistOuter(SpecialEffectList defenderPenetrateResistOuter, DataContext context)
		{
			this._defenderPenetrateResistOuter = defenderPenetrateResistOuter;
			base.SetModifiedAndInvalidateInfluencedCache(100, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				int dataSize = this._defenderPenetrateResistOuter.GetSerializedSize();
				byte* pData = OperationAdder.DynamicObjectCollection_SetDynamicField<int>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this._id, 99, dataSize);
				pData += this._defenderPenetrateResistOuter.Serialize(pData);
			}
		}

		// Token: 0x0600269D RID: 9885 RVA: 0x001D5F24 File Offset: 0x001D4124
		public SpecialEffectList GetDefenderPenetrateResistInner()
		{
			return this._defenderPenetrateResistInner;
		}

		// Token: 0x0600269E RID: 9886 RVA: 0x001D5F3C File Offset: 0x001D413C
		public unsafe void SetDefenderPenetrateResistInner(SpecialEffectList defenderPenetrateResistInner, DataContext context)
		{
			this._defenderPenetrateResistInner = defenderPenetrateResistInner;
			base.SetModifiedAndInvalidateInfluencedCache(101, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				int dataSize = this._defenderPenetrateResistInner.GetSerializedSize();
				byte* pData = OperationAdder.DynamicObjectCollection_SetDynamicField<int>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this._id, 100, dataSize);
				pData += this._defenderPenetrateResistInner.Serialize(pData);
			}
		}

		// Token: 0x0600269F RID: 9887 RVA: 0x001D5FAC File Offset: 0x001D41AC
		public SpecialEffectList GetAcceptDirectDamage()
		{
			return this._acceptDirectDamage;
		}

		// Token: 0x060026A0 RID: 9888 RVA: 0x001D5FC4 File Offset: 0x001D41C4
		public unsafe void SetAcceptDirectDamage(SpecialEffectList acceptDirectDamage, DataContext context)
		{
			this._acceptDirectDamage = acceptDirectDamage;
			base.SetModifiedAndInvalidateInfluencedCache(102, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				int dataSize = this._acceptDirectDamage.GetSerializedSize();
				byte* pData = OperationAdder.DynamicObjectCollection_SetDynamicField<int>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this._id, 101, dataSize);
				pData += this._acceptDirectDamage.Serialize(pData);
			}
		}

		// Token: 0x060026A1 RID: 9889 RVA: 0x001D6034 File Offset: 0x001D4234
		public SpecialEffectList GetAcceptBounceDamage()
		{
			return this._acceptBounceDamage;
		}

		// Token: 0x060026A2 RID: 9890 RVA: 0x001D604C File Offset: 0x001D424C
		public unsafe void SetAcceptBounceDamage(SpecialEffectList acceptBounceDamage, DataContext context)
		{
			this._acceptBounceDamage = acceptBounceDamage;
			base.SetModifiedAndInvalidateInfluencedCache(103, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				int dataSize = this._acceptBounceDamage.GetSerializedSize();
				byte* pData = OperationAdder.DynamicObjectCollection_SetDynamicField<int>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this._id, 102, dataSize);
				pData += this._acceptBounceDamage.Serialize(pData);
			}
		}

		// Token: 0x060026A3 RID: 9891 RVA: 0x001D60BC File Offset: 0x001D42BC
		public SpecialEffectList GetAcceptFightBackDamage()
		{
			return this._acceptFightBackDamage;
		}

		// Token: 0x060026A4 RID: 9892 RVA: 0x001D60D4 File Offset: 0x001D42D4
		public unsafe void SetAcceptFightBackDamage(SpecialEffectList acceptFightBackDamage, DataContext context)
		{
			this._acceptFightBackDamage = acceptFightBackDamage;
			base.SetModifiedAndInvalidateInfluencedCache(104, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				int dataSize = this._acceptFightBackDamage.GetSerializedSize();
				byte* pData = OperationAdder.DynamicObjectCollection_SetDynamicField<int>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this._id, 103, dataSize);
				pData += this._acceptFightBackDamage.Serialize(pData);
			}
		}

		// Token: 0x060026A5 RID: 9893 RVA: 0x001D6144 File Offset: 0x001D4344
		public SpecialEffectList GetAcceptPoisonLevel()
		{
			return this._acceptPoisonLevel;
		}

		// Token: 0x060026A6 RID: 9894 RVA: 0x001D615C File Offset: 0x001D435C
		public unsafe void SetAcceptPoisonLevel(SpecialEffectList acceptPoisonLevel, DataContext context)
		{
			this._acceptPoisonLevel = acceptPoisonLevel;
			base.SetModifiedAndInvalidateInfluencedCache(105, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				int dataSize = this._acceptPoisonLevel.GetSerializedSize();
				byte* pData = OperationAdder.DynamicObjectCollection_SetDynamicField<int>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this._id, 104, dataSize);
				pData += this._acceptPoisonLevel.Serialize(pData);
			}
		}

		// Token: 0x060026A7 RID: 9895 RVA: 0x001D61CC File Offset: 0x001D43CC
		public SpecialEffectList GetAcceptPoisonValue()
		{
			return this._acceptPoisonValue;
		}

		// Token: 0x060026A8 RID: 9896 RVA: 0x001D61E4 File Offset: 0x001D43E4
		public unsafe void SetAcceptPoisonValue(SpecialEffectList acceptPoisonValue, DataContext context)
		{
			this._acceptPoisonValue = acceptPoisonValue;
			base.SetModifiedAndInvalidateInfluencedCache(106, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				int dataSize = this._acceptPoisonValue.GetSerializedSize();
				byte* pData = OperationAdder.DynamicObjectCollection_SetDynamicField<int>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this._id, 105, dataSize);
				pData += this._acceptPoisonValue.Serialize(pData);
			}
		}

		// Token: 0x060026A9 RID: 9897 RVA: 0x001D6254 File Offset: 0x001D4454
		public SpecialEffectList GetDefenderHitOdds()
		{
			return this._defenderHitOdds;
		}

		// Token: 0x060026AA RID: 9898 RVA: 0x001D626C File Offset: 0x001D446C
		public unsafe void SetDefenderHitOdds(SpecialEffectList defenderHitOdds, DataContext context)
		{
			this._defenderHitOdds = defenderHitOdds;
			base.SetModifiedAndInvalidateInfluencedCache(107, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				int dataSize = this._defenderHitOdds.GetSerializedSize();
				byte* pData = OperationAdder.DynamicObjectCollection_SetDynamicField<int>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this._id, 106, dataSize);
				pData += this._defenderHitOdds.Serialize(pData);
			}
		}

		// Token: 0x060026AB RID: 9899 RVA: 0x001D62DC File Offset: 0x001D44DC
		public SpecialEffectList GetDefenderFightBackHitOdds()
		{
			return this._defenderFightBackHitOdds;
		}

		// Token: 0x060026AC RID: 9900 RVA: 0x001D62F4 File Offset: 0x001D44F4
		public unsafe void SetDefenderFightBackHitOdds(SpecialEffectList defenderFightBackHitOdds, DataContext context)
		{
			this._defenderFightBackHitOdds = defenderFightBackHitOdds;
			base.SetModifiedAndInvalidateInfluencedCache(108, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				int dataSize = this._defenderFightBackHitOdds.GetSerializedSize();
				byte* pData = OperationAdder.DynamicObjectCollection_SetDynamicField<int>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this._id, 107, dataSize);
				pData += this._defenderFightBackHitOdds.Serialize(pData);
			}
		}

		// Token: 0x060026AD RID: 9901 RVA: 0x001D6364 File Offset: 0x001D4564
		public SpecialEffectList GetDefenderPursueOdds()
		{
			return this._defenderPursueOdds;
		}

		// Token: 0x060026AE RID: 9902 RVA: 0x001D637C File Offset: 0x001D457C
		public unsafe void SetDefenderPursueOdds(SpecialEffectList defenderPursueOdds, DataContext context)
		{
			this._defenderPursueOdds = defenderPursueOdds;
			base.SetModifiedAndInvalidateInfluencedCache(109, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				int dataSize = this._defenderPursueOdds.GetSerializedSize();
				byte* pData = OperationAdder.DynamicObjectCollection_SetDynamicField<int>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this._id, 108, dataSize);
				pData += this._defenderPursueOdds.Serialize(pData);
			}
		}

		// Token: 0x060026AF RID: 9903 RVA: 0x001D63EC File Offset: 0x001D45EC
		public SpecialEffectList GetAcceptMaxInjuryCount()
		{
			return this._acceptMaxInjuryCount;
		}

		// Token: 0x060026B0 RID: 9904 RVA: 0x001D6404 File Offset: 0x001D4604
		public unsafe void SetAcceptMaxInjuryCount(SpecialEffectList acceptMaxInjuryCount, DataContext context)
		{
			this._acceptMaxInjuryCount = acceptMaxInjuryCount;
			base.SetModifiedAndInvalidateInfluencedCache(110, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				int dataSize = this._acceptMaxInjuryCount.GetSerializedSize();
				byte* pData = OperationAdder.DynamicObjectCollection_SetDynamicField<int>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this._id, 109, dataSize);
				pData += this._acceptMaxInjuryCount.Serialize(pData);
			}
		}

		// Token: 0x060026B1 RID: 9905 RVA: 0x001D6474 File Offset: 0x001D4674
		public SpecialEffectList GetBouncePower()
		{
			return this._bouncePower;
		}

		// Token: 0x060026B2 RID: 9906 RVA: 0x001D648C File Offset: 0x001D468C
		public unsafe void SetBouncePower(SpecialEffectList bouncePower, DataContext context)
		{
			this._bouncePower = bouncePower;
			base.SetModifiedAndInvalidateInfluencedCache(111, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				int dataSize = this._bouncePower.GetSerializedSize();
				byte* pData = OperationAdder.DynamicObjectCollection_SetDynamicField<int>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this._id, 110, dataSize);
				pData += this._bouncePower.Serialize(pData);
			}
		}

		// Token: 0x060026B3 RID: 9907 RVA: 0x001D64FC File Offset: 0x001D46FC
		public SpecialEffectList GetFightBackPower()
		{
			return this._fightBackPower;
		}

		// Token: 0x060026B4 RID: 9908 RVA: 0x001D6514 File Offset: 0x001D4714
		public unsafe void SetFightBackPower(SpecialEffectList fightBackPower, DataContext context)
		{
			this._fightBackPower = fightBackPower;
			base.SetModifiedAndInvalidateInfluencedCache(112, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				int dataSize = this._fightBackPower.GetSerializedSize();
				byte* pData = OperationAdder.DynamicObjectCollection_SetDynamicField<int>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this._id, 111, dataSize);
				pData += this._fightBackPower.Serialize(pData);
			}
		}

		// Token: 0x060026B5 RID: 9909 RVA: 0x001D6584 File Offset: 0x001D4784
		public SpecialEffectList GetDirectDamageInnerRatio()
		{
			return this._directDamageInnerRatio;
		}

		// Token: 0x060026B6 RID: 9910 RVA: 0x001D659C File Offset: 0x001D479C
		public unsafe void SetDirectDamageInnerRatio(SpecialEffectList directDamageInnerRatio, DataContext context)
		{
			this._directDamageInnerRatio = directDamageInnerRatio;
			base.SetModifiedAndInvalidateInfluencedCache(113, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				int dataSize = this._directDamageInnerRatio.GetSerializedSize();
				byte* pData = OperationAdder.DynamicObjectCollection_SetDynamicField<int>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this._id, 112, dataSize);
				pData += this._directDamageInnerRatio.Serialize(pData);
			}
		}

		// Token: 0x060026B7 RID: 9911 RVA: 0x001D660C File Offset: 0x001D480C
		public SpecialEffectList GetDefenderFinalDamageValue()
		{
			return this._defenderFinalDamageValue;
		}

		// Token: 0x060026B8 RID: 9912 RVA: 0x001D6624 File Offset: 0x001D4824
		public unsafe void SetDefenderFinalDamageValue(SpecialEffectList defenderFinalDamageValue, DataContext context)
		{
			this._defenderFinalDamageValue = defenderFinalDamageValue;
			base.SetModifiedAndInvalidateInfluencedCache(114, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				int dataSize = this._defenderFinalDamageValue.GetSerializedSize();
				byte* pData = OperationAdder.DynamicObjectCollection_SetDynamicField<int>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this._id, 113, dataSize);
				pData += this._defenderFinalDamageValue.Serialize(pData);
			}
		}

		// Token: 0x060026B9 RID: 9913 RVA: 0x001D6694 File Offset: 0x001D4894
		public SpecialEffectList GetDirectDamageValue()
		{
			return this._directDamageValue;
		}

		// Token: 0x060026BA RID: 9914 RVA: 0x001D66AC File Offset: 0x001D48AC
		public unsafe void SetDirectDamageValue(SpecialEffectList directDamageValue, DataContext context)
		{
			this._directDamageValue = directDamageValue;
			base.SetModifiedAndInvalidateInfluencedCache(115, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				int dataSize = this._directDamageValue.GetSerializedSize();
				byte* pData = OperationAdder.DynamicObjectCollection_SetDynamicField<int>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this._id, 114, dataSize);
				pData += this._directDamageValue.Serialize(pData);
			}
		}

		// Token: 0x060026BB RID: 9915 RVA: 0x001D671C File Offset: 0x001D491C
		public SpecialEffectList GetDirectInjuryMark()
		{
			return this._directInjuryMark;
		}

		// Token: 0x060026BC RID: 9916 RVA: 0x001D6734 File Offset: 0x001D4934
		public unsafe void SetDirectInjuryMark(SpecialEffectList directInjuryMark, DataContext context)
		{
			this._directInjuryMark = directInjuryMark;
			base.SetModifiedAndInvalidateInfluencedCache(116, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				int dataSize = this._directInjuryMark.GetSerializedSize();
				byte* pData = OperationAdder.DynamicObjectCollection_SetDynamicField<int>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this._id, 115, dataSize);
				pData += this._directInjuryMark.Serialize(pData);
			}
		}

		// Token: 0x060026BD RID: 9917 RVA: 0x001D67A4 File Offset: 0x001D49A4
		public SpecialEffectList GetGoneMadInjury()
		{
			return this._goneMadInjury;
		}

		// Token: 0x060026BE RID: 9918 RVA: 0x001D67BC File Offset: 0x001D49BC
		public unsafe void SetGoneMadInjury(SpecialEffectList goneMadInjury, DataContext context)
		{
			this._goneMadInjury = goneMadInjury;
			base.SetModifiedAndInvalidateInfluencedCache(117, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				int dataSize = this._goneMadInjury.GetSerializedSize();
				byte* pData = OperationAdder.DynamicObjectCollection_SetDynamicField<int>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this._id, 116, dataSize);
				pData += this._goneMadInjury.Serialize(pData);
			}
		}

		// Token: 0x060026BF RID: 9919 RVA: 0x001D682C File Offset: 0x001D4A2C
		public SpecialEffectList GetHealInjurySpeed()
		{
			return this._healInjurySpeed;
		}

		// Token: 0x060026C0 RID: 9920 RVA: 0x001D6844 File Offset: 0x001D4A44
		public unsafe void SetHealInjurySpeed(SpecialEffectList healInjurySpeed, DataContext context)
		{
			this._healInjurySpeed = healInjurySpeed;
			base.SetModifiedAndInvalidateInfluencedCache(118, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				int dataSize = this._healInjurySpeed.GetSerializedSize();
				byte* pData = OperationAdder.DynamicObjectCollection_SetDynamicField<int>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this._id, 117, dataSize);
				pData += this._healInjurySpeed.Serialize(pData);
			}
		}

		// Token: 0x060026C1 RID: 9921 RVA: 0x001D68B4 File Offset: 0x001D4AB4
		public SpecialEffectList GetHealInjuryBuff()
		{
			return this._healInjuryBuff;
		}

		// Token: 0x060026C2 RID: 9922 RVA: 0x001D68CC File Offset: 0x001D4ACC
		public unsafe void SetHealInjuryBuff(SpecialEffectList healInjuryBuff, DataContext context)
		{
			this._healInjuryBuff = healInjuryBuff;
			base.SetModifiedAndInvalidateInfluencedCache(119, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				int dataSize = this._healInjuryBuff.GetSerializedSize();
				byte* pData = OperationAdder.DynamicObjectCollection_SetDynamicField<int>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this._id, 118, dataSize);
				pData += this._healInjuryBuff.Serialize(pData);
			}
		}

		// Token: 0x060026C3 RID: 9923 RVA: 0x001D693C File Offset: 0x001D4B3C
		public SpecialEffectList GetHealInjuryDebuff()
		{
			return this._healInjuryDebuff;
		}

		// Token: 0x060026C4 RID: 9924 RVA: 0x001D6954 File Offset: 0x001D4B54
		public unsafe void SetHealInjuryDebuff(SpecialEffectList healInjuryDebuff, DataContext context)
		{
			this._healInjuryDebuff = healInjuryDebuff;
			base.SetModifiedAndInvalidateInfluencedCache(120, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				int dataSize = this._healInjuryDebuff.GetSerializedSize();
				byte* pData = OperationAdder.DynamicObjectCollection_SetDynamicField<int>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this._id, 119, dataSize);
				pData += this._healInjuryDebuff.Serialize(pData);
			}
		}

		// Token: 0x060026C5 RID: 9925 RVA: 0x001D69C4 File Offset: 0x001D4BC4
		public SpecialEffectList GetHealPoisonSpeed()
		{
			return this._healPoisonSpeed;
		}

		// Token: 0x060026C6 RID: 9926 RVA: 0x001D69DC File Offset: 0x001D4BDC
		public unsafe void SetHealPoisonSpeed(SpecialEffectList healPoisonSpeed, DataContext context)
		{
			this._healPoisonSpeed = healPoisonSpeed;
			base.SetModifiedAndInvalidateInfluencedCache(121, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				int dataSize = this._healPoisonSpeed.GetSerializedSize();
				byte* pData = OperationAdder.DynamicObjectCollection_SetDynamicField<int>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this._id, 120, dataSize);
				pData += this._healPoisonSpeed.Serialize(pData);
			}
		}

		// Token: 0x060026C7 RID: 9927 RVA: 0x001D6A4C File Offset: 0x001D4C4C
		public SpecialEffectList GetHealPoisonBuff()
		{
			return this._healPoisonBuff;
		}

		// Token: 0x060026C8 RID: 9928 RVA: 0x001D6A64 File Offset: 0x001D4C64
		public unsafe void SetHealPoisonBuff(SpecialEffectList healPoisonBuff, DataContext context)
		{
			this._healPoisonBuff = healPoisonBuff;
			base.SetModifiedAndInvalidateInfluencedCache(122, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				int dataSize = this._healPoisonBuff.GetSerializedSize();
				byte* pData = OperationAdder.DynamicObjectCollection_SetDynamicField<int>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this._id, 121, dataSize);
				pData += this._healPoisonBuff.Serialize(pData);
			}
		}

		// Token: 0x060026C9 RID: 9929 RVA: 0x001D6AD4 File Offset: 0x001D4CD4
		public SpecialEffectList GetHealPoisonDebuff()
		{
			return this._healPoisonDebuff;
		}

		// Token: 0x060026CA RID: 9930 RVA: 0x001D6AEC File Offset: 0x001D4CEC
		public unsafe void SetHealPoisonDebuff(SpecialEffectList healPoisonDebuff, DataContext context)
		{
			this._healPoisonDebuff = healPoisonDebuff;
			base.SetModifiedAndInvalidateInfluencedCache(123, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				int dataSize = this._healPoisonDebuff.GetSerializedSize();
				byte* pData = OperationAdder.DynamicObjectCollection_SetDynamicField<int>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this._id, 122, dataSize);
				pData += this._healPoisonDebuff.Serialize(pData);
			}
		}

		// Token: 0x060026CB RID: 9931 RVA: 0x001D6B5C File Offset: 0x001D4D5C
		public SpecialEffectList GetFleeSpeed()
		{
			return this._fleeSpeed;
		}

		// Token: 0x060026CC RID: 9932 RVA: 0x001D6B74 File Offset: 0x001D4D74
		public unsafe void SetFleeSpeed(SpecialEffectList fleeSpeed, DataContext context)
		{
			this._fleeSpeed = fleeSpeed;
			base.SetModifiedAndInvalidateInfluencedCache(124, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				int dataSize = this._fleeSpeed.GetSerializedSize();
				byte* pData = OperationAdder.DynamicObjectCollection_SetDynamicField<int>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this._id, 123, dataSize);
				pData += this._fleeSpeed.Serialize(pData);
			}
		}

		// Token: 0x060026CD RID: 9933 RVA: 0x001D6BE4 File Offset: 0x001D4DE4
		public SpecialEffectList GetMaxFlawCount()
		{
			return this._maxFlawCount;
		}

		// Token: 0x060026CE RID: 9934 RVA: 0x001D6BFC File Offset: 0x001D4DFC
		public unsafe void SetMaxFlawCount(SpecialEffectList maxFlawCount, DataContext context)
		{
			this._maxFlawCount = maxFlawCount;
			base.SetModifiedAndInvalidateInfluencedCache(125, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				int dataSize = this._maxFlawCount.GetSerializedSize();
				byte* pData = OperationAdder.DynamicObjectCollection_SetDynamicField<int>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this._id, 124, dataSize);
				pData += this._maxFlawCount.Serialize(pData);
			}
		}

		// Token: 0x060026CF RID: 9935 RVA: 0x001D6C6C File Offset: 0x001D4E6C
		public SpecialEffectList GetCanAddFlaw()
		{
			return this._canAddFlaw;
		}

		// Token: 0x060026D0 RID: 9936 RVA: 0x001D6C84 File Offset: 0x001D4E84
		public unsafe void SetCanAddFlaw(SpecialEffectList canAddFlaw, DataContext context)
		{
			this._canAddFlaw = canAddFlaw;
			base.SetModifiedAndInvalidateInfluencedCache(126, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				int dataSize = this._canAddFlaw.GetSerializedSize();
				byte* pData = OperationAdder.DynamicObjectCollection_SetDynamicField<int>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this._id, 125, dataSize);
				pData += this._canAddFlaw.Serialize(pData);
			}
		}

		// Token: 0x060026D1 RID: 9937 RVA: 0x001D6CF4 File Offset: 0x001D4EF4
		public SpecialEffectList GetFlawLevel()
		{
			return this._flawLevel;
		}

		// Token: 0x060026D2 RID: 9938 RVA: 0x001D6D0C File Offset: 0x001D4F0C
		public unsafe void SetFlawLevel(SpecialEffectList flawLevel, DataContext context)
		{
			this._flawLevel = flawLevel;
			base.SetModifiedAndInvalidateInfluencedCache(127, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				int dataSize = this._flawLevel.GetSerializedSize();
				byte* pData = OperationAdder.DynamicObjectCollection_SetDynamicField<int>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this._id, 126, dataSize);
				pData += this._flawLevel.Serialize(pData);
			}
		}

		// Token: 0x060026D3 RID: 9939 RVA: 0x001D6D7C File Offset: 0x001D4F7C
		public SpecialEffectList GetFlawLevelCanReduce()
		{
			return this._flawLevelCanReduce;
		}

		// Token: 0x060026D4 RID: 9940 RVA: 0x001D6D94 File Offset: 0x001D4F94
		public unsafe void SetFlawLevelCanReduce(SpecialEffectList flawLevelCanReduce, DataContext context)
		{
			this._flawLevelCanReduce = flawLevelCanReduce;
			base.SetModifiedAndInvalidateInfluencedCache(128, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				int dataSize = this._flawLevelCanReduce.GetSerializedSize();
				byte* pData = OperationAdder.DynamicObjectCollection_SetDynamicField<int>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this._id, 127, dataSize);
				pData += this._flawLevelCanReduce.Serialize(pData);
			}
		}

		// Token: 0x060026D5 RID: 9941 RVA: 0x001D6E08 File Offset: 0x001D5008
		public SpecialEffectList GetFlawCount()
		{
			return this._flawCount;
		}

		// Token: 0x060026D6 RID: 9942 RVA: 0x001D6E20 File Offset: 0x001D5020
		public unsafe void SetFlawCount(SpecialEffectList flawCount, DataContext context)
		{
			this._flawCount = flawCount;
			base.SetModifiedAndInvalidateInfluencedCache(129, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				int dataSize = this._flawCount.GetSerializedSize();
				byte* pData = OperationAdder.DynamicObjectCollection_SetDynamicField<int>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this._id, 128, dataSize);
				pData += this._flawCount.Serialize(pData);
			}
		}

		// Token: 0x060026D7 RID: 9943 RVA: 0x001D6E98 File Offset: 0x001D5098
		public SpecialEffectList GetMaxAcupointCount()
		{
			return this._maxAcupointCount;
		}

		// Token: 0x060026D8 RID: 9944 RVA: 0x001D6EB0 File Offset: 0x001D50B0
		public unsafe void SetMaxAcupointCount(SpecialEffectList maxAcupointCount, DataContext context)
		{
			this._maxAcupointCount = maxAcupointCount;
			base.SetModifiedAndInvalidateInfluencedCache(130, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				int dataSize = this._maxAcupointCount.GetSerializedSize();
				byte* pData = OperationAdder.DynamicObjectCollection_SetDynamicField<int>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this._id, 129, dataSize);
				pData += this._maxAcupointCount.Serialize(pData);
			}
		}

		// Token: 0x060026D9 RID: 9945 RVA: 0x001D6F28 File Offset: 0x001D5128
		public SpecialEffectList GetCanAddAcupoint()
		{
			return this._canAddAcupoint;
		}

		// Token: 0x060026DA RID: 9946 RVA: 0x001D6F40 File Offset: 0x001D5140
		public unsafe void SetCanAddAcupoint(SpecialEffectList canAddAcupoint, DataContext context)
		{
			this._canAddAcupoint = canAddAcupoint;
			base.SetModifiedAndInvalidateInfluencedCache(131, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				int dataSize = this._canAddAcupoint.GetSerializedSize();
				byte* pData = OperationAdder.DynamicObjectCollection_SetDynamicField<int>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this._id, 130, dataSize);
				pData += this._canAddAcupoint.Serialize(pData);
			}
		}

		// Token: 0x060026DB RID: 9947 RVA: 0x001D6FB8 File Offset: 0x001D51B8
		public SpecialEffectList GetAcupointLevel()
		{
			return this._acupointLevel;
		}

		// Token: 0x060026DC RID: 9948 RVA: 0x001D6FD0 File Offset: 0x001D51D0
		public unsafe void SetAcupointLevel(SpecialEffectList acupointLevel, DataContext context)
		{
			this._acupointLevel = acupointLevel;
			base.SetModifiedAndInvalidateInfluencedCache(132, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				int dataSize = this._acupointLevel.GetSerializedSize();
				byte* pData = OperationAdder.DynamicObjectCollection_SetDynamicField<int>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this._id, 131, dataSize);
				pData += this._acupointLevel.Serialize(pData);
			}
		}

		// Token: 0x060026DD RID: 9949 RVA: 0x001D7048 File Offset: 0x001D5248
		public SpecialEffectList GetAcupointLevelCanReduce()
		{
			return this._acupointLevelCanReduce;
		}

		// Token: 0x060026DE RID: 9950 RVA: 0x001D7060 File Offset: 0x001D5260
		public unsafe void SetAcupointLevelCanReduce(SpecialEffectList acupointLevelCanReduce, DataContext context)
		{
			this._acupointLevelCanReduce = acupointLevelCanReduce;
			base.SetModifiedAndInvalidateInfluencedCache(133, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				int dataSize = this._acupointLevelCanReduce.GetSerializedSize();
				byte* pData = OperationAdder.DynamicObjectCollection_SetDynamicField<int>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this._id, 132, dataSize);
				pData += this._acupointLevelCanReduce.Serialize(pData);
			}
		}

		// Token: 0x060026DF RID: 9951 RVA: 0x001D70D8 File Offset: 0x001D52D8
		public SpecialEffectList GetAcupointCount()
		{
			return this._acupointCount;
		}

		// Token: 0x060026E0 RID: 9952 RVA: 0x001D70F0 File Offset: 0x001D52F0
		public unsafe void SetAcupointCount(SpecialEffectList acupointCount, DataContext context)
		{
			this._acupointCount = acupointCount;
			base.SetModifiedAndInvalidateInfluencedCache(134, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				int dataSize = this._acupointCount.GetSerializedSize();
				byte* pData = OperationAdder.DynamicObjectCollection_SetDynamicField<int>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this._id, 133, dataSize);
				pData += this._acupointCount.Serialize(pData);
			}
		}

		// Token: 0x060026E1 RID: 9953 RVA: 0x001D7168 File Offset: 0x001D5368
		public SpecialEffectList GetAddNeiliAllocation()
		{
			return this._addNeiliAllocation;
		}

		// Token: 0x060026E2 RID: 9954 RVA: 0x001D7180 File Offset: 0x001D5380
		public unsafe void SetAddNeiliAllocation(SpecialEffectList addNeiliAllocation, DataContext context)
		{
			this._addNeiliAllocation = addNeiliAllocation;
			base.SetModifiedAndInvalidateInfluencedCache(135, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				int dataSize = this._addNeiliAllocation.GetSerializedSize();
				byte* pData = OperationAdder.DynamicObjectCollection_SetDynamicField<int>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this._id, 134, dataSize);
				pData += this._addNeiliAllocation.Serialize(pData);
			}
		}

		// Token: 0x060026E3 RID: 9955 RVA: 0x001D71F8 File Offset: 0x001D53F8
		public SpecialEffectList GetCostNeiliAllocation()
		{
			return this._costNeiliAllocation;
		}

		// Token: 0x060026E4 RID: 9956 RVA: 0x001D7210 File Offset: 0x001D5410
		public unsafe void SetCostNeiliAllocation(SpecialEffectList costNeiliAllocation, DataContext context)
		{
			this._costNeiliAllocation = costNeiliAllocation;
			base.SetModifiedAndInvalidateInfluencedCache(136, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				int dataSize = this._costNeiliAllocation.GetSerializedSize();
				byte* pData = OperationAdder.DynamicObjectCollection_SetDynamicField<int>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this._id, 135, dataSize);
				pData += this._costNeiliAllocation.Serialize(pData);
			}
		}

		// Token: 0x060026E5 RID: 9957 RVA: 0x001D7288 File Offset: 0x001D5488
		public SpecialEffectList GetCanChangeNeiliAllocation()
		{
			return this._canChangeNeiliAllocation;
		}

		// Token: 0x060026E6 RID: 9958 RVA: 0x001D72A0 File Offset: 0x001D54A0
		public unsafe void SetCanChangeNeiliAllocation(SpecialEffectList canChangeNeiliAllocation, DataContext context)
		{
			this._canChangeNeiliAllocation = canChangeNeiliAllocation;
			base.SetModifiedAndInvalidateInfluencedCache(137, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				int dataSize = this._canChangeNeiliAllocation.GetSerializedSize();
				byte* pData = OperationAdder.DynamicObjectCollection_SetDynamicField<int>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this._id, 136, dataSize);
				pData += this._canChangeNeiliAllocation.Serialize(pData);
			}
		}

		// Token: 0x060026E7 RID: 9959 RVA: 0x001D7318 File Offset: 0x001D5518
		public SpecialEffectList GetCanGetTrick()
		{
			return this._canGetTrick;
		}

		// Token: 0x060026E8 RID: 9960 RVA: 0x001D7330 File Offset: 0x001D5530
		public unsafe void SetCanGetTrick(SpecialEffectList canGetTrick, DataContext context)
		{
			this._canGetTrick = canGetTrick;
			base.SetModifiedAndInvalidateInfluencedCache(138, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				int dataSize = this._canGetTrick.GetSerializedSize();
				byte* pData = OperationAdder.DynamicObjectCollection_SetDynamicField<int>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this._id, 137, dataSize);
				pData += this._canGetTrick.Serialize(pData);
			}
		}

		// Token: 0x060026E9 RID: 9961 RVA: 0x001D73A8 File Offset: 0x001D55A8
		public SpecialEffectList GetGetTrickType()
		{
			return this._getTrickType;
		}

		// Token: 0x060026EA RID: 9962 RVA: 0x001D73C0 File Offset: 0x001D55C0
		public unsafe void SetGetTrickType(SpecialEffectList getTrickType, DataContext context)
		{
			this._getTrickType = getTrickType;
			base.SetModifiedAndInvalidateInfluencedCache(139, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				int dataSize = this._getTrickType.GetSerializedSize();
				byte* pData = OperationAdder.DynamicObjectCollection_SetDynamicField<int>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this._id, 138, dataSize);
				pData += this._getTrickType.Serialize(pData);
			}
		}

		// Token: 0x060026EB RID: 9963 RVA: 0x001D7438 File Offset: 0x001D5638
		public SpecialEffectList GetAttackBodyPart()
		{
			return this._attackBodyPart;
		}

		// Token: 0x060026EC RID: 9964 RVA: 0x001D7450 File Offset: 0x001D5650
		public unsafe void SetAttackBodyPart(SpecialEffectList attackBodyPart, DataContext context)
		{
			this._attackBodyPart = attackBodyPart;
			base.SetModifiedAndInvalidateInfluencedCache(140, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				int dataSize = this._attackBodyPart.GetSerializedSize();
				byte* pData = OperationAdder.DynamicObjectCollection_SetDynamicField<int>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this._id, 139, dataSize);
				pData += this._attackBodyPart.Serialize(pData);
			}
		}

		// Token: 0x060026ED RID: 9965 RVA: 0x001D74C8 File Offset: 0x001D56C8
		public SpecialEffectList GetWeaponEquipAttack()
		{
			return this._weaponEquipAttack;
		}

		// Token: 0x060026EE RID: 9966 RVA: 0x001D74E0 File Offset: 0x001D56E0
		public unsafe void SetWeaponEquipAttack(SpecialEffectList weaponEquipAttack, DataContext context)
		{
			this._weaponEquipAttack = weaponEquipAttack;
			base.SetModifiedAndInvalidateInfluencedCache(141, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				int dataSize = this._weaponEquipAttack.GetSerializedSize();
				byte* pData = OperationAdder.DynamicObjectCollection_SetDynamicField<int>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this._id, 140, dataSize);
				pData += this._weaponEquipAttack.Serialize(pData);
			}
		}

		// Token: 0x060026EF RID: 9967 RVA: 0x001D7558 File Offset: 0x001D5758
		public SpecialEffectList GetWeaponEquipDefense()
		{
			return this._weaponEquipDefense;
		}

		// Token: 0x060026F0 RID: 9968 RVA: 0x001D7570 File Offset: 0x001D5770
		public unsafe void SetWeaponEquipDefense(SpecialEffectList weaponEquipDefense, DataContext context)
		{
			this._weaponEquipDefense = weaponEquipDefense;
			base.SetModifiedAndInvalidateInfluencedCache(142, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				int dataSize = this._weaponEquipDefense.GetSerializedSize();
				byte* pData = OperationAdder.DynamicObjectCollection_SetDynamicField<int>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this._id, 141, dataSize);
				pData += this._weaponEquipDefense.Serialize(pData);
			}
		}

		// Token: 0x060026F1 RID: 9969 RVA: 0x001D75E8 File Offset: 0x001D57E8
		public SpecialEffectList GetArmorEquipAttack()
		{
			return this._armorEquipAttack;
		}

		// Token: 0x060026F2 RID: 9970 RVA: 0x001D7600 File Offset: 0x001D5800
		public unsafe void SetArmorEquipAttack(SpecialEffectList armorEquipAttack, DataContext context)
		{
			this._armorEquipAttack = armorEquipAttack;
			base.SetModifiedAndInvalidateInfluencedCache(143, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				int dataSize = this._armorEquipAttack.GetSerializedSize();
				byte* pData = OperationAdder.DynamicObjectCollection_SetDynamicField<int>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this._id, 142, dataSize);
				pData += this._armorEquipAttack.Serialize(pData);
			}
		}

		// Token: 0x060026F3 RID: 9971 RVA: 0x001D7678 File Offset: 0x001D5878
		public SpecialEffectList GetArmorEquipDefense()
		{
			return this._armorEquipDefense;
		}

		// Token: 0x060026F4 RID: 9972 RVA: 0x001D7690 File Offset: 0x001D5890
		public unsafe void SetArmorEquipDefense(SpecialEffectList armorEquipDefense, DataContext context)
		{
			this._armorEquipDefense = armorEquipDefense;
			base.SetModifiedAndInvalidateInfluencedCache(144, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				int dataSize = this._armorEquipDefense.GetSerializedSize();
				byte* pData = OperationAdder.DynamicObjectCollection_SetDynamicField<int>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this._id, 143, dataSize);
				pData += this._armorEquipDefense.Serialize(pData);
			}
		}

		// Token: 0x060026F5 RID: 9973 RVA: 0x001D7708 File Offset: 0x001D5908
		public SpecialEffectList GetAttackRangeForward()
		{
			return this._attackRangeForward;
		}

		// Token: 0x060026F6 RID: 9974 RVA: 0x001D7720 File Offset: 0x001D5920
		public unsafe void SetAttackRangeForward(SpecialEffectList attackRangeForward, DataContext context)
		{
			this._attackRangeForward = attackRangeForward;
			base.SetModifiedAndInvalidateInfluencedCache(145, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				int dataSize = this._attackRangeForward.GetSerializedSize();
				byte* pData = OperationAdder.DynamicObjectCollection_SetDynamicField<int>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this._id, 144, dataSize);
				pData += this._attackRangeForward.Serialize(pData);
			}
		}

		// Token: 0x060026F7 RID: 9975 RVA: 0x001D7798 File Offset: 0x001D5998
		public SpecialEffectList GetAttackRangeBackward()
		{
			return this._attackRangeBackward;
		}

		// Token: 0x060026F8 RID: 9976 RVA: 0x001D77B0 File Offset: 0x001D59B0
		public unsafe void SetAttackRangeBackward(SpecialEffectList attackRangeBackward, DataContext context)
		{
			this._attackRangeBackward = attackRangeBackward;
			base.SetModifiedAndInvalidateInfluencedCache(146, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				int dataSize = this._attackRangeBackward.GetSerializedSize();
				byte* pData = OperationAdder.DynamicObjectCollection_SetDynamicField<int>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this._id, 145, dataSize);
				pData += this._attackRangeBackward.Serialize(pData);
			}
		}

		// Token: 0x060026F9 RID: 9977 RVA: 0x001D7828 File Offset: 0x001D5A28
		public SpecialEffectList GetMoveCanBeStopped()
		{
			return this._moveCanBeStopped;
		}

		// Token: 0x060026FA RID: 9978 RVA: 0x001D7840 File Offset: 0x001D5A40
		public unsafe void SetMoveCanBeStopped(SpecialEffectList moveCanBeStopped, DataContext context)
		{
			this._moveCanBeStopped = moveCanBeStopped;
			base.SetModifiedAndInvalidateInfluencedCache(147, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				int dataSize = this._moveCanBeStopped.GetSerializedSize();
				byte* pData = OperationAdder.DynamicObjectCollection_SetDynamicField<int>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this._id, 146, dataSize);
				pData += this._moveCanBeStopped.Serialize(pData);
			}
		}

		// Token: 0x060026FB RID: 9979 RVA: 0x001D78B8 File Offset: 0x001D5AB8
		public SpecialEffectList GetCanForcedMove()
		{
			return this._canForcedMove;
		}

		// Token: 0x060026FC RID: 9980 RVA: 0x001D78D0 File Offset: 0x001D5AD0
		public unsafe void SetCanForcedMove(SpecialEffectList canForcedMove, DataContext context)
		{
			this._canForcedMove = canForcedMove;
			base.SetModifiedAndInvalidateInfluencedCache(148, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				int dataSize = this._canForcedMove.GetSerializedSize();
				byte* pData = OperationAdder.DynamicObjectCollection_SetDynamicField<int>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this._id, 147, dataSize);
				pData += this._canForcedMove.Serialize(pData);
			}
		}

		// Token: 0x060026FD RID: 9981 RVA: 0x001D7948 File Offset: 0x001D5B48
		public SpecialEffectList GetMobilityCanBeRemoved()
		{
			return this._mobilityCanBeRemoved;
		}

		// Token: 0x060026FE RID: 9982 RVA: 0x001D7960 File Offset: 0x001D5B60
		public unsafe void SetMobilityCanBeRemoved(SpecialEffectList mobilityCanBeRemoved, DataContext context)
		{
			this._mobilityCanBeRemoved = mobilityCanBeRemoved;
			base.SetModifiedAndInvalidateInfluencedCache(149, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				int dataSize = this._mobilityCanBeRemoved.GetSerializedSize();
				byte* pData = OperationAdder.DynamicObjectCollection_SetDynamicField<int>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this._id, 148, dataSize);
				pData += this._mobilityCanBeRemoved.Serialize(pData);
			}
		}

		// Token: 0x060026FF RID: 9983 RVA: 0x001D79D8 File Offset: 0x001D5BD8
		public SpecialEffectList GetMobilityCostByEffect()
		{
			return this._mobilityCostByEffect;
		}

		// Token: 0x06002700 RID: 9984 RVA: 0x001D79F0 File Offset: 0x001D5BF0
		public unsafe void SetMobilityCostByEffect(SpecialEffectList mobilityCostByEffect, DataContext context)
		{
			this._mobilityCostByEffect = mobilityCostByEffect;
			base.SetModifiedAndInvalidateInfluencedCache(150, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				int dataSize = this._mobilityCostByEffect.GetSerializedSize();
				byte* pData = OperationAdder.DynamicObjectCollection_SetDynamicField<int>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this._id, 149, dataSize);
				pData += this._mobilityCostByEffect.Serialize(pData);
			}
		}

		// Token: 0x06002701 RID: 9985 RVA: 0x001D7A68 File Offset: 0x001D5C68
		public SpecialEffectList GetMoveDistance()
		{
			return this._moveDistance;
		}

		// Token: 0x06002702 RID: 9986 RVA: 0x001D7A80 File Offset: 0x001D5C80
		public unsafe void SetMoveDistance(SpecialEffectList moveDistance, DataContext context)
		{
			this._moveDistance = moveDistance;
			base.SetModifiedAndInvalidateInfluencedCache(151, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				int dataSize = this._moveDistance.GetSerializedSize();
				byte* pData = OperationAdder.DynamicObjectCollection_SetDynamicField<int>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this._id, 150, dataSize);
				pData += this._moveDistance.Serialize(pData);
			}
		}

		// Token: 0x06002703 RID: 9987 RVA: 0x001D7AF8 File Offset: 0x001D5CF8
		public SpecialEffectList GetJumpPrepareFrame()
		{
			return this._jumpPrepareFrame;
		}

		// Token: 0x06002704 RID: 9988 RVA: 0x001D7B10 File Offset: 0x001D5D10
		public unsafe void SetJumpPrepareFrame(SpecialEffectList jumpPrepareFrame, DataContext context)
		{
			this._jumpPrepareFrame = jumpPrepareFrame;
			base.SetModifiedAndInvalidateInfluencedCache(152, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				int dataSize = this._jumpPrepareFrame.GetSerializedSize();
				byte* pData = OperationAdder.DynamicObjectCollection_SetDynamicField<int>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this._id, 151, dataSize);
				pData += this._jumpPrepareFrame.Serialize(pData);
			}
		}

		// Token: 0x06002705 RID: 9989 RVA: 0x001D7B88 File Offset: 0x001D5D88
		public SpecialEffectList GetBounceInjuryMark()
		{
			return this._bounceInjuryMark;
		}

		// Token: 0x06002706 RID: 9990 RVA: 0x001D7BA0 File Offset: 0x001D5DA0
		public unsafe void SetBounceInjuryMark(SpecialEffectList bounceInjuryMark, DataContext context)
		{
			this._bounceInjuryMark = bounceInjuryMark;
			base.SetModifiedAndInvalidateInfluencedCache(153, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				int dataSize = this._bounceInjuryMark.GetSerializedSize();
				byte* pData = OperationAdder.DynamicObjectCollection_SetDynamicField<int>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this._id, 152, dataSize);
				pData += this._bounceInjuryMark.Serialize(pData);
			}
		}

		// Token: 0x06002707 RID: 9991 RVA: 0x001D7C18 File Offset: 0x001D5E18
		public SpecialEffectList GetSkillHasCost()
		{
			return this._skillHasCost;
		}

		// Token: 0x06002708 RID: 9992 RVA: 0x001D7C30 File Offset: 0x001D5E30
		public unsafe void SetSkillHasCost(SpecialEffectList skillHasCost, DataContext context)
		{
			this._skillHasCost = skillHasCost;
			base.SetModifiedAndInvalidateInfluencedCache(154, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				int dataSize = this._skillHasCost.GetSerializedSize();
				byte* pData = OperationAdder.DynamicObjectCollection_SetDynamicField<int>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this._id, 153, dataSize);
				pData += this._skillHasCost.Serialize(pData);
			}
		}

		// Token: 0x06002709 RID: 9993 RVA: 0x001D7CA8 File Offset: 0x001D5EA8
		public SpecialEffectList GetCombatStateEffect()
		{
			return this._combatStateEffect;
		}

		// Token: 0x0600270A RID: 9994 RVA: 0x001D7CC0 File Offset: 0x001D5EC0
		public unsafe void SetCombatStateEffect(SpecialEffectList combatStateEffect, DataContext context)
		{
			this._combatStateEffect = combatStateEffect;
			base.SetModifiedAndInvalidateInfluencedCache(155, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				int dataSize = this._combatStateEffect.GetSerializedSize();
				byte* pData = OperationAdder.DynamicObjectCollection_SetDynamicField<int>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this._id, 154, dataSize);
				pData += this._combatStateEffect.Serialize(pData);
			}
		}

		// Token: 0x0600270B RID: 9995 RVA: 0x001D7D38 File Offset: 0x001D5F38
		public SpecialEffectList GetChangeNeedUseSkill()
		{
			return this._changeNeedUseSkill;
		}

		// Token: 0x0600270C RID: 9996 RVA: 0x001D7D50 File Offset: 0x001D5F50
		public unsafe void SetChangeNeedUseSkill(SpecialEffectList changeNeedUseSkill, DataContext context)
		{
			this._changeNeedUseSkill = changeNeedUseSkill;
			base.SetModifiedAndInvalidateInfluencedCache(156, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				int dataSize = this._changeNeedUseSkill.GetSerializedSize();
				byte* pData = OperationAdder.DynamicObjectCollection_SetDynamicField<int>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this._id, 155, dataSize);
				pData += this._changeNeedUseSkill.Serialize(pData);
			}
		}

		// Token: 0x0600270D RID: 9997 RVA: 0x001D7DC8 File Offset: 0x001D5FC8
		public SpecialEffectList GetChangeDistanceIsMove()
		{
			return this._changeDistanceIsMove;
		}

		// Token: 0x0600270E RID: 9998 RVA: 0x001D7DE0 File Offset: 0x001D5FE0
		public unsafe void SetChangeDistanceIsMove(SpecialEffectList changeDistanceIsMove, DataContext context)
		{
			this._changeDistanceIsMove = changeDistanceIsMove;
			base.SetModifiedAndInvalidateInfluencedCache(157, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				int dataSize = this._changeDistanceIsMove.GetSerializedSize();
				byte* pData = OperationAdder.DynamicObjectCollection_SetDynamicField<int>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this._id, 156, dataSize);
				pData += this._changeDistanceIsMove.Serialize(pData);
			}
		}

		// Token: 0x0600270F RID: 9999 RVA: 0x001D7E58 File Offset: 0x001D6058
		public SpecialEffectList GetReplaceCharHit()
		{
			return this._replaceCharHit;
		}

		// Token: 0x06002710 RID: 10000 RVA: 0x001D7E70 File Offset: 0x001D6070
		public unsafe void SetReplaceCharHit(SpecialEffectList replaceCharHit, DataContext context)
		{
			this._replaceCharHit = replaceCharHit;
			base.SetModifiedAndInvalidateInfluencedCache(158, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				int dataSize = this._replaceCharHit.GetSerializedSize();
				byte* pData = OperationAdder.DynamicObjectCollection_SetDynamicField<int>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this._id, 157, dataSize);
				pData += this._replaceCharHit.Serialize(pData);
			}
		}

		// Token: 0x06002711 RID: 10001 RVA: 0x001D7EE8 File Offset: 0x001D60E8
		public SpecialEffectList GetCanAddPoison()
		{
			return this._canAddPoison;
		}

		// Token: 0x06002712 RID: 10002 RVA: 0x001D7F00 File Offset: 0x001D6100
		public unsafe void SetCanAddPoison(SpecialEffectList canAddPoison, DataContext context)
		{
			this._canAddPoison = canAddPoison;
			base.SetModifiedAndInvalidateInfluencedCache(159, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				int dataSize = this._canAddPoison.GetSerializedSize();
				byte* pData = OperationAdder.DynamicObjectCollection_SetDynamicField<int>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this._id, 158, dataSize);
				pData += this._canAddPoison.Serialize(pData);
			}
		}

		// Token: 0x06002713 RID: 10003 RVA: 0x001D7F78 File Offset: 0x001D6178
		public SpecialEffectList GetCanReducePoison()
		{
			return this._canReducePoison;
		}

		// Token: 0x06002714 RID: 10004 RVA: 0x001D7F90 File Offset: 0x001D6190
		public unsafe void SetCanReducePoison(SpecialEffectList canReducePoison, DataContext context)
		{
			this._canReducePoison = canReducePoison;
			base.SetModifiedAndInvalidateInfluencedCache(160, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				int dataSize = this._canReducePoison.GetSerializedSize();
				byte* pData = OperationAdder.DynamicObjectCollection_SetDynamicField<int>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this._id, 159, dataSize);
				pData += this._canReducePoison.Serialize(pData);
			}
		}

		// Token: 0x06002715 RID: 10005 RVA: 0x001D8008 File Offset: 0x001D6208
		public SpecialEffectList GetReducePoisonValue()
		{
			return this._reducePoisonValue;
		}

		// Token: 0x06002716 RID: 10006 RVA: 0x001D8020 File Offset: 0x001D6220
		public unsafe void SetReducePoisonValue(SpecialEffectList reducePoisonValue, DataContext context)
		{
			this._reducePoisonValue = reducePoisonValue;
			base.SetModifiedAndInvalidateInfluencedCache(161, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				int dataSize = this._reducePoisonValue.GetSerializedSize();
				byte* pData = OperationAdder.DynamicObjectCollection_SetDynamicField<int>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this._id, 160, dataSize);
				pData += this._reducePoisonValue.Serialize(pData);
			}
		}

		// Token: 0x06002717 RID: 10007 RVA: 0x001D8098 File Offset: 0x001D6298
		public SpecialEffectList GetPoisonCanAffect()
		{
			return this._poisonCanAffect;
		}

		// Token: 0x06002718 RID: 10008 RVA: 0x001D80B0 File Offset: 0x001D62B0
		public unsafe void SetPoisonCanAffect(SpecialEffectList poisonCanAffect, DataContext context)
		{
			this._poisonCanAffect = poisonCanAffect;
			base.SetModifiedAndInvalidateInfluencedCache(162, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				int dataSize = this._poisonCanAffect.GetSerializedSize();
				byte* pData = OperationAdder.DynamicObjectCollection_SetDynamicField<int>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this._id, 161, dataSize);
				pData += this._poisonCanAffect.Serialize(pData);
			}
		}

		// Token: 0x06002719 RID: 10009 RVA: 0x001D8128 File Offset: 0x001D6328
		public SpecialEffectList GetPoisonAffectCount()
		{
			return this._poisonAffectCount;
		}

		// Token: 0x0600271A RID: 10010 RVA: 0x001D8140 File Offset: 0x001D6340
		public unsafe void SetPoisonAffectCount(SpecialEffectList poisonAffectCount, DataContext context)
		{
			this._poisonAffectCount = poisonAffectCount;
			base.SetModifiedAndInvalidateInfluencedCache(163, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				int dataSize = this._poisonAffectCount.GetSerializedSize();
				byte* pData = OperationAdder.DynamicObjectCollection_SetDynamicField<int>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this._id, 162, dataSize);
				pData += this._poisonAffectCount.Serialize(pData);
			}
		}

		// Token: 0x0600271B RID: 10011 RVA: 0x001D81B8 File Offset: 0x001D63B8
		public SpecialEffectList GetCostTricks()
		{
			return this._costTricks;
		}

		// Token: 0x0600271C RID: 10012 RVA: 0x001D81D0 File Offset: 0x001D63D0
		public unsafe void SetCostTricks(SpecialEffectList costTricks, DataContext context)
		{
			this._costTricks = costTricks;
			base.SetModifiedAndInvalidateInfluencedCache(164, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				int dataSize = this._costTricks.GetSerializedSize();
				byte* pData = OperationAdder.DynamicObjectCollection_SetDynamicField<int>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this._id, 163, dataSize);
				pData += this._costTricks.Serialize(pData);
			}
		}

		// Token: 0x0600271D RID: 10013 RVA: 0x001D8248 File Offset: 0x001D6448
		public SpecialEffectList GetJumpMoveDistance()
		{
			return this._jumpMoveDistance;
		}

		// Token: 0x0600271E RID: 10014 RVA: 0x001D8260 File Offset: 0x001D6460
		public unsafe void SetJumpMoveDistance(SpecialEffectList jumpMoveDistance, DataContext context)
		{
			this._jumpMoveDistance = jumpMoveDistance;
			base.SetModifiedAndInvalidateInfluencedCache(165, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				int dataSize = this._jumpMoveDistance.GetSerializedSize();
				byte* pData = OperationAdder.DynamicObjectCollection_SetDynamicField<int>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this._id, 164, dataSize);
				pData += this._jumpMoveDistance.Serialize(pData);
			}
		}

		// Token: 0x0600271F RID: 10015 RVA: 0x001D82D8 File Offset: 0x001D64D8
		public SpecialEffectList GetCombatStateToAdd()
		{
			return this._combatStateToAdd;
		}

		// Token: 0x06002720 RID: 10016 RVA: 0x001D82F0 File Offset: 0x001D64F0
		public unsafe void SetCombatStateToAdd(SpecialEffectList combatStateToAdd, DataContext context)
		{
			this._combatStateToAdd = combatStateToAdd;
			base.SetModifiedAndInvalidateInfluencedCache(166, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				int dataSize = this._combatStateToAdd.GetSerializedSize();
				byte* pData = OperationAdder.DynamicObjectCollection_SetDynamicField<int>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this._id, 165, dataSize);
				pData += this._combatStateToAdd.Serialize(pData);
			}
		}

		// Token: 0x06002721 RID: 10017 RVA: 0x001D8368 File Offset: 0x001D6568
		public SpecialEffectList GetCombatStatePower()
		{
			return this._combatStatePower;
		}

		// Token: 0x06002722 RID: 10018 RVA: 0x001D8380 File Offset: 0x001D6580
		public unsafe void SetCombatStatePower(SpecialEffectList combatStatePower, DataContext context)
		{
			this._combatStatePower = combatStatePower;
			base.SetModifiedAndInvalidateInfluencedCache(167, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				int dataSize = this._combatStatePower.GetSerializedSize();
				byte* pData = OperationAdder.DynamicObjectCollection_SetDynamicField<int>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this._id, 166, dataSize);
				pData += this._combatStatePower.Serialize(pData);
			}
		}

		// Token: 0x06002723 RID: 10019 RVA: 0x001D83F8 File Offset: 0x001D65F8
		public SpecialEffectList GetBreakBodyPartInjuryCount()
		{
			return this._breakBodyPartInjuryCount;
		}

		// Token: 0x06002724 RID: 10020 RVA: 0x001D8410 File Offset: 0x001D6610
		public unsafe void SetBreakBodyPartInjuryCount(SpecialEffectList breakBodyPartInjuryCount, DataContext context)
		{
			this._breakBodyPartInjuryCount = breakBodyPartInjuryCount;
			base.SetModifiedAndInvalidateInfluencedCache(168, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				int dataSize = this._breakBodyPartInjuryCount.GetSerializedSize();
				byte* pData = OperationAdder.DynamicObjectCollection_SetDynamicField<int>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this._id, 167, dataSize);
				pData += this._breakBodyPartInjuryCount.Serialize(pData);
			}
		}

		// Token: 0x06002725 RID: 10021 RVA: 0x001D8488 File Offset: 0x001D6688
		public SpecialEffectList GetBodyPartIsBroken()
		{
			return this._bodyPartIsBroken;
		}

		// Token: 0x06002726 RID: 10022 RVA: 0x001D84A0 File Offset: 0x001D66A0
		public unsafe void SetBodyPartIsBroken(SpecialEffectList bodyPartIsBroken, DataContext context)
		{
			this._bodyPartIsBroken = bodyPartIsBroken;
			base.SetModifiedAndInvalidateInfluencedCache(169, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				int dataSize = this._bodyPartIsBroken.GetSerializedSize();
				byte* pData = OperationAdder.DynamicObjectCollection_SetDynamicField<int>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this._id, 168, dataSize);
				pData += this._bodyPartIsBroken.Serialize(pData);
			}
		}

		// Token: 0x06002727 RID: 10023 RVA: 0x001D8518 File Offset: 0x001D6718
		public SpecialEffectList GetMaxTrickCount()
		{
			return this._maxTrickCount;
		}

		// Token: 0x06002728 RID: 10024 RVA: 0x001D8530 File Offset: 0x001D6730
		public unsafe void SetMaxTrickCount(SpecialEffectList maxTrickCount, DataContext context)
		{
			this._maxTrickCount = maxTrickCount;
			base.SetModifiedAndInvalidateInfluencedCache(170, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				int dataSize = this._maxTrickCount.GetSerializedSize();
				byte* pData = OperationAdder.DynamicObjectCollection_SetDynamicField<int>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this._id, 169, dataSize);
				pData += this._maxTrickCount.Serialize(pData);
			}
		}

		// Token: 0x06002729 RID: 10025 RVA: 0x001D85A8 File Offset: 0x001D67A8
		public SpecialEffectList GetMaxBreathPercent()
		{
			return this._maxBreathPercent;
		}

		// Token: 0x0600272A RID: 10026 RVA: 0x001D85C0 File Offset: 0x001D67C0
		public unsafe void SetMaxBreathPercent(SpecialEffectList maxBreathPercent, DataContext context)
		{
			this._maxBreathPercent = maxBreathPercent;
			base.SetModifiedAndInvalidateInfluencedCache(171, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				int dataSize = this._maxBreathPercent.GetSerializedSize();
				byte* pData = OperationAdder.DynamicObjectCollection_SetDynamicField<int>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this._id, 170, dataSize);
				pData += this._maxBreathPercent.Serialize(pData);
			}
		}

		// Token: 0x0600272B RID: 10027 RVA: 0x001D8638 File Offset: 0x001D6838
		public SpecialEffectList GetMaxStancePercent()
		{
			return this._maxStancePercent;
		}

		// Token: 0x0600272C RID: 10028 RVA: 0x001D8650 File Offset: 0x001D6850
		public unsafe void SetMaxStancePercent(SpecialEffectList maxStancePercent, DataContext context)
		{
			this._maxStancePercent = maxStancePercent;
			base.SetModifiedAndInvalidateInfluencedCache(172, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				int dataSize = this._maxStancePercent.GetSerializedSize();
				byte* pData = OperationAdder.DynamicObjectCollection_SetDynamicField<int>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this._id, 171, dataSize);
				pData += this._maxStancePercent.Serialize(pData);
			}
		}

		// Token: 0x0600272D RID: 10029 RVA: 0x001D86C8 File Offset: 0x001D68C8
		public SpecialEffectList GetExtraBreathPercent()
		{
			return this._extraBreathPercent;
		}

		// Token: 0x0600272E RID: 10030 RVA: 0x001D86E0 File Offset: 0x001D68E0
		public unsafe void SetExtraBreathPercent(SpecialEffectList extraBreathPercent, DataContext context)
		{
			this._extraBreathPercent = extraBreathPercent;
			base.SetModifiedAndInvalidateInfluencedCache(173, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				int dataSize = this._extraBreathPercent.GetSerializedSize();
				byte* pData = OperationAdder.DynamicObjectCollection_SetDynamicField<int>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this._id, 172, dataSize);
				pData += this._extraBreathPercent.Serialize(pData);
			}
		}

		// Token: 0x0600272F RID: 10031 RVA: 0x001D8758 File Offset: 0x001D6958
		public SpecialEffectList GetExtraStancePercent()
		{
			return this._extraStancePercent;
		}

		// Token: 0x06002730 RID: 10032 RVA: 0x001D8770 File Offset: 0x001D6970
		public unsafe void SetExtraStancePercent(SpecialEffectList extraStancePercent, DataContext context)
		{
			this._extraStancePercent = extraStancePercent;
			base.SetModifiedAndInvalidateInfluencedCache(174, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				int dataSize = this._extraStancePercent.GetSerializedSize();
				byte* pData = OperationAdder.DynamicObjectCollection_SetDynamicField<int>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this._id, 173, dataSize);
				pData += this._extraStancePercent.Serialize(pData);
			}
		}

		// Token: 0x06002731 RID: 10033 RVA: 0x001D87E8 File Offset: 0x001D69E8
		public SpecialEffectList GetMoveCostMobility()
		{
			return this._moveCostMobility;
		}

		// Token: 0x06002732 RID: 10034 RVA: 0x001D8800 File Offset: 0x001D6A00
		public unsafe void SetMoveCostMobility(SpecialEffectList moveCostMobility, DataContext context)
		{
			this._moveCostMobility = moveCostMobility;
			base.SetModifiedAndInvalidateInfluencedCache(175, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				int dataSize = this._moveCostMobility.GetSerializedSize();
				byte* pData = OperationAdder.DynamicObjectCollection_SetDynamicField<int>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this._id, 174, dataSize);
				pData += this._moveCostMobility.Serialize(pData);
			}
		}

		// Token: 0x06002733 RID: 10035 RVA: 0x001D8878 File Offset: 0x001D6A78
		public SpecialEffectList GetDefendSkillKeepTime()
		{
			return this._defendSkillKeepTime;
		}

		// Token: 0x06002734 RID: 10036 RVA: 0x001D8890 File Offset: 0x001D6A90
		public unsafe void SetDefendSkillKeepTime(SpecialEffectList defendSkillKeepTime, DataContext context)
		{
			this._defendSkillKeepTime = defendSkillKeepTime;
			base.SetModifiedAndInvalidateInfluencedCache(176, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				int dataSize = this._defendSkillKeepTime.GetSerializedSize();
				byte* pData = OperationAdder.DynamicObjectCollection_SetDynamicField<int>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this._id, 175, dataSize);
				pData += this._defendSkillKeepTime.Serialize(pData);
			}
		}

		// Token: 0x06002735 RID: 10037 RVA: 0x001D8908 File Offset: 0x001D6B08
		public SpecialEffectList GetBounceRange()
		{
			return this._bounceRange;
		}

		// Token: 0x06002736 RID: 10038 RVA: 0x001D8920 File Offset: 0x001D6B20
		public unsafe void SetBounceRange(SpecialEffectList bounceRange, DataContext context)
		{
			this._bounceRange = bounceRange;
			base.SetModifiedAndInvalidateInfluencedCache(177, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				int dataSize = this._bounceRange.GetSerializedSize();
				byte* pData = OperationAdder.DynamicObjectCollection_SetDynamicField<int>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this._id, 176, dataSize);
				pData += this._bounceRange.Serialize(pData);
			}
		}

		// Token: 0x06002737 RID: 10039 RVA: 0x001D8998 File Offset: 0x001D6B98
		public SpecialEffectList GetMindMarkKeepTime()
		{
			return this._mindMarkKeepTime;
		}

		// Token: 0x06002738 RID: 10040 RVA: 0x001D89B0 File Offset: 0x001D6BB0
		public unsafe void SetMindMarkKeepTime(SpecialEffectList mindMarkKeepTime, DataContext context)
		{
			this._mindMarkKeepTime = mindMarkKeepTime;
			base.SetModifiedAndInvalidateInfluencedCache(178, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				int dataSize = this._mindMarkKeepTime.GetSerializedSize();
				byte* pData = OperationAdder.DynamicObjectCollection_SetDynamicField<int>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this._id, 177, dataSize);
				pData += this._mindMarkKeepTime.Serialize(pData);
			}
		}

		// Token: 0x06002739 RID: 10041 RVA: 0x001D8A28 File Offset: 0x001D6C28
		public SpecialEffectList GetSkillMobilityCostPerFrame()
		{
			return this._skillMobilityCostPerFrame;
		}

		// Token: 0x0600273A RID: 10042 RVA: 0x001D8A40 File Offset: 0x001D6C40
		public unsafe void SetSkillMobilityCostPerFrame(SpecialEffectList skillMobilityCostPerFrame, DataContext context)
		{
			this._skillMobilityCostPerFrame = skillMobilityCostPerFrame;
			base.SetModifiedAndInvalidateInfluencedCache(179, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				int dataSize = this._skillMobilityCostPerFrame.GetSerializedSize();
				byte* pData = OperationAdder.DynamicObjectCollection_SetDynamicField<int>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this._id, 178, dataSize);
				pData += this._skillMobilityCostPerFrame.Serialize(pData);
			}
		}

		// Token: 0x0600273B RID: 10043 RVA: 0x001D8AB8 File Offset: 0x001D6CB8
		public SpecialEffectList GetCanAddWug()
		{
			return this._canAddWug;
		}

		// Token: 0x0600273C RID: 10044 RVA: 0x001D8AD0 File Offset: 0x001D6CD0
		public unsafe void SetCanAddWug(SpecialEffectList canAddWug, DataContext context)
		{
			this._canAddWug = canAddWug;
			base.SetModifiedAndInvalidateInfluencedCache(180, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				int dataSize = this._canAddWug.GetSerializedSize();
				byte* pData = OperationAdder.DynamicObjectCollection_SetDynamicField<int>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this._id, 179, dataSize);
				pData += this._canAddWug.Serialize(pData);
			}
		}

		// Token: 0x0600273D RID: 10045 RVA: 0x001D8B48 File Offset: 0x001D6D48
		public SpecialEffectList GetHasGodWeaponBuff()
		{
			return this._hasGodWeaponBuff;
		}

		// Token: 0x0600273E RID: 10046 RVA: 0x001D8B60 File Offset: 0x001D6D60
		public unsafe void SetHasGodWeaponBuff(SpecialEffectList hasGodWeaponBuff, DataContext context)
		{
			this._hasGodWeaponBuff = hasGodWeaponBuff;
			base.SetModifiedAndInvalidateInfluencedCache(181, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				int dataSize = this._hasGodWeaponBuff.GetSerializedSize();
				byte* pData = OperationAdder.DynamicObjectCollection_SetDynamicField<int>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this._id, 180, dataSize);
				pData += this._hasGodWeaponBuff.Serialize(pData);
			}
		}

		// Token: 0x0600273F RID: 10047 RVA: 0x001D8BD8 File Offset: 0x001D6DD8
		public SpecialEffectList GetHasGodArmorBuff()
		{
			return this._hasGodArmorBuff;
		}

		// Token: 0x06002740 RID: 10048 RVA: 0x001D8BF0 File Offset: 0x001D6DF0
		public unsafe void SetHasGodArmorBuff(SpecialEffectList hasGodArmorBuff, DataContext context)
		{
			this._hasGodArmorBuff = hasGodArmorBuff;
			base.SetModifiedAndInvalidateInfluencedCache(182, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				int dataSize = this._hasGodArmorBuff.GetSerializedSize();
				byte* pData = OperationAdder.DynamicObjectCollection_SetDynamicField<int>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this._id, 181, dataSize);
				pData += this._hasGodArmorBuff.Serialize(pData);
			}
		}

		// Token: 0x06002741 RID: 10049 RVA: 0x001D8C68 File Offset: 0x001D6E68
		public SpecialEffectList GetTeammateCmdRequireGenerateValue()
		{
			return this._teammateCmdRequireGenerateValue;
		}

		// Token: 0x06002742 RID: 10050 RVA: 0x001D8C80 File Offset: 0x001D6E80
		public unsafe void SetTeammateCmdRequireGenerateValue(SpecialEffectList teammateCmdRequireGenerateValue, DataContext context)
		{
			this._teammateCmdRequireGenerateValue = teammateCmdRequireGenerateValue;
			base.SetModifiedAndInvalidateInfluencedCache(183, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				int dataSize = this._teammateCmdRequireGenerateValue.GetSerializedSize();
				byte* pData = OperationAdder.DynamicObjectCollection_SetDynamicField<int>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this._id, 182, dataSize);
				pData += this._teammateCmdRequireGenerateValue.Serialize(pData);
			}
		}

		// Token: 0x06002743 RID: 10051 RVA: 0x001D8CF8 File Offset: 0x001D6EF8
		public SpecialEffectList GetTeammateCmdEffect()
		{
			return this._teammateCmdEffect;
		}

		// Token: 0x06002744 RID: 10052 RVA: 0x001D8D10 File Offset: 0x001D6F10
		public unsafe void SetTeammateCmdEffect(SpecialEffectList teammateCmdEffect, DataContext context)
		{
			this._teammateCmdEffect = teammateCmdEffect;
			base.SetModifiedAndInvalidateInfluencedCache(184, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				int dataSize = this._teammateCmdEffect.GetSerializedSize();
				byte* pData = OperationAdder.DynamicObjectCollection_SetDynamicField<int>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this._id, 183, dataSize);
				pData += this._teammateCmdEffect.Serialize(pData);
			}
		}

		// Token: 0x06002745 RID: 10053 RVA: 0x001D8D88 File Offset: 0x001D6F88
		public SpecialEffectList GetFlawRecoverSpeed()
		{
			return this._flawRecoverSpeed;
		}

		// Token: 0x06002746 RID: 10054 RVA: 0x001D8DA0 File Offset: 0x001D6FA0
		public unsafe void SetFlawRecoverSpeed(SpecialEffectList flawRecoverSpeed, DataContext context)
		{
			this._flawRecoverSpeed = flawRecoverSpeed;
			base.SetModifiedAndInvalidateInfluencedCache(185, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				int dataSize = this._flawRecoverSpeed.GetSerializedSize();
				byte* pData = OperationAdder.DynamicObjectCollection_SetDynamicField<int>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this._id, 184, dataSize);
				pData += this._flawRecoverSpeed.Serialize(pData);
			}
		}

		// Token: 0x06002747 RID: 10055 RVA: 0x001D8E18 File Offset: 0x001D7018
		public SpecialEffectList GetAcupointRecoverSpeed()
		{
			return this._acupointRecoverSpeed;
		}

		// Token: 0x06002748 RID: 10056 RVA: 0x001D8E30 File Offset: 0x001D7030
		public unsafe void SetAcupointRecoverSpeed(SpecialEffectList acupointRecoverSpeed, DataContext context)
		{
			this._acupointRecoverSpeed = acupointRecoverSpeed;
			base.SetModifiedAndInvalidateInfluencedCache(186, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				int dataSize = this._acupointRecoverSpeed.GetSerializedSize();
				byte* pData = OperationAdder.DynamicObjectCollection_SetDynamicField<int>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this._id, 185, dataSize);
				pData += this._acupointRecoverSpeed.Serialize(pData);
			}
		}

		// Token: 0x06002749 RID: 10057 RVA: 0x001D8EA8 File Offset: 0x001D70A8
		public SpecialEffectList GetMindMarkRecoverSpeed()
		{
			return this._mindMarkRecoverSpeed;
		}

		// Token: 0x0600274A RID: 10058 RVA: 0x001D8EC0 File Offset: 0x001D70C0
		public unsafe void SetMindMarkRecoverSpeed(SpecialEffectList mindMarkRecoverSpeed, DataContext context)
		{
			this._mindMarkRecoverSpeed = mindMarkRecoverSpeed;
			base.SetModifiedAndInvalidateInfluencedCache(187, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				int dataSize = this._mindMarkRecoverSpeed.GetSerializedSize();
				byte* pData = OperationAdder.DynamicObjectCollection_SetDynamicField<int>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this._id, 186, dataSize);
				pData += this._mindMarkRecoverSpeed.Serialize(pData);
			}
		}

		// Token: 0x0600274B RID: 10059 RVA: 0x001D8F38 File Offset: 0x001D7138
		public SpecialEffectList GetInjuryAutoHealSpeed()
		{
			return this._injuryAutoHealSpeed;
		}

		// Token: 0x0600274C RID: 10060 RVA: 0x001D8F50 File Offset: 0x001D7150
		public unsafe void SetInjuryAutoHealSpeed(SpecialEffectList injuryAutoHealSpeed, DataContext context)
		{
			this._injuryAutoHealSpeed = injuryAutoHealSpeed;
			base.SetModifiedAndInvalidateInfluencedCache(188, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				int dataSize = this._injuryAutoHealSpeed.GetSerializedSize();
				byte* pData = OperationAdder.DynamicObjectCollection_SetDynamicField<int>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this._id, 187, dataSize);
				pData += this._injuryAutoHealSpeed.Serialize(pData);
			}
		}

		// Token: 0x0600274D RID: 10061 RVA: 0x001D8FC8 File Offset: 0x001D71C8
		public SpecialEffectList GetCanRecoverBreath()
		{
			return this._canRecoverBreath;
		}

		// Token: 0x0600274E RID: 10062 RVA: 0x001D8FE0 File Offset: 0x001D71E0
		public unsafe void SetCanRecoverBreath(SpecialEffectList canRecoverBreath, DataContext context)
		{
			this._canRecoverBreath = canRecoverBreath;
			base.SetModifiedAndInvalidateInfluencedCache(189, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				int dataSize = this._canRecoverBreath.GetSerializedSize();
				byte* pData = OperationAdder.DynamicObjectCollection_SetDynamicField<int>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this._id, 188, dataSize);
				pData += this._canRecoverBreath.Serialize(pData);
			}
		}

		// Token: 0x0600274F RID: 10063 RVA: 0x001D9058 File Offset: 0x001D7258
		public SpecialEffectList GetCanRecoverStance()
		{
			return this._canRecoverStance;
		}

		// Token: 0x06002750 RID: 10064 RVA: 0x001D9070 File Offset: 0x001D7270
		public unsafe void SetCanRecoverStance(SpecialEffectList canRecoverStance, DataContext context)
		{
			this._canRecoverStance = canRecoverStance;
			base.SetModifiedAndInvalidateInfluencedCache(190, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				int dataSize = this._canRecoverStance.GetSerializedSize();
				byte* pData = OperationAdder.DynamicObjectCollection_SetDynamicField<int>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this._id, 189, dataSize);
				pData += this._canRecoverStance.Serialize(pData);
			}
		}

		// Token: 0x06002751 RID: 10065 RVA: 0x001D90E8 File Offset: 0x001D72E8
		public SpecialEffectList GetFatalDamageValue()
		{
			return this._fatalDamageValue;
		}

		// Token: 0x06002752 RID: 10066 RVA: 0x001D9100 File Offset: 0x001D7300
		public unsafe void SetFatalDamageValue(SpecialEffectList fatalDamageValue, DataContext context)
		{
			this._fatalDamageValue = fatalDamageValue;
			base.SetModifiedAndInvalidateInfluencedCache(191, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				int dataSize = this._fatalDamageValue.GetSerializedSize();
				byte* pData = OperationAdder.DynamicObjectCollection_SetDynamicField<int>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this._id, 190, dataSize);
				pData += this._fatalDamageValue.Serialize(pData);
			}
		}

		// Token: 0x06002753 RID: 10067 RVA: 0x001D9178 File Offset: 0x001D7378
		public SpecialEffectList GetFatalDamageMarkCount()
		{
			return this._fatalDamageMarkCount;
		}

		// Token: 0x06002754 RID: 10068 RVA: 0x001D9190 File Offset: 0x001D7390
		public unsafe void SetFatalDamageMarkCount(SpecialEffectList fatalDamageMarkCount, DataContext context)
		{
			this._fatalDamageMarkCount = fatalDamageMarkCount;
			base.SetModifiedAndInvalidateInfluencedCache(192, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				int dataSize = this._fatalDamageMarkCount.GetSerializedSize();
				byte* pData = OperationAdder.DynamicObjectCollection_SetDynamicField<int>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this._id, 191, dataSize);
				pData += this._fatalDamageMarkCount.Serialize(pData);
			}
		}

		// Token: 0x06002755 RID: 10069 RVA: 0x001D9208 File Offset: 0x001D7408
		public SpecialEffectList GetCanFightBackDuringPrepareSkill()
		{
			return this._canFightBackDuringPrepareSkill;
		}

		// Token: 0x06002756 RID: 10070 RVA: 0x001D9220 File Offset: 0x001D7420
		public unsafe void SetCanFightBackDuringPrepareSkill(SpecialEffectList canFightBackDuringPrepareSkill, DataContext context)
		{
			this._canFightBackDuringPrepareSkill = canFightBackDuringPrepareSkill;
			base.SetModifiedAndInvalidateInfluencedCache(193, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				int dataSize = this._canFightBackDuringPrepareSkill.GetSerializedSize();
				byte* pData = OperationAdder.DynamicObjectCollection_SetDynamicField<int>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this._id, 192, dataSize);
				pData += this._canFightBackDuringPrepareSkill.Serialize(pData);
			}
		}

		// Token: 0x06002757 RID: 10071 RVA: 0x001D9298 File Offset: 0x001D7498
		public SpecialEffectList GetSkillPrepareSpeed()
		{
			return this._skillPrepareSpeed;
		}

		// Token: 0x06002758 RID: 10072 RVA: 0x001D92B0 File Offset: 0x001D74B0
		public unsafe void SetSkillPrepareSpeed(SpecialEffectList skillPrepareSpeed, DataContext context)
		{
			this._skillPrepareSpeed = skillPrepareSpeed;
			base.SetModifiedAndInvalidateInfluencedCache(194, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				int dataSize = this._skillPrepareSpeed.GetSerializedSize();
				byte* pData = OperationAdder.DynamicObjectCollection_SetDynamicField<int>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this._id, 193, dataSize);
				pData += this._skillPrepareSpeed.Serialize(pData);
			}
		}

		// Token: 0x06002759 RID: 10073 RVA: 0x001D9328 File Offset: 0x001D7528
		public SpecialEffectList GetBreathRecoverSpeed()
		{
			return this._breathRecoverSpeed;
		}

		// Token: 0x0600275A RID: 10074 RVA: 0x001D9340 File Offset: 0x001D7540
		public unsafe void SetBreathRecoverSpeed(SpecialEffectList breathRecoverSpeed, DataContext context)
		{
			this._breathRecoverSpeed = breathRecoverSpeed;
			base.SetModifiedAndInvalidateInfluencedCache(195, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				int dataSize = this._breathRecoverSpeed.GetSerializedSize();
				byte* pData = OperationAdder.DynamicObjectCollection_SetDynamicField<int>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this._id, 194, dataSize);
				pData += this._breathRecoverSpeed.Serialize(pData);
			}
		}

		// Token: 0x0600275B RID: 10075 RVA: 0x001D93B8 File Offset: 0x001D75B8
		public SpecialEffectList GetStanceRecoverSpeed()
		{
			return this._stanceRecoverSpeed;
		}

		// Token: 0x0600275C RID: 10076 RVA: 0x001D93D0 File Offset: 0x001D75D0
		public unsafe void SetStanceRecoverSpeed(SpecialEffectList stanceRecoverSpeed, DataContext context)
		{
			this._stanceRecoverSpeed = stanceRecoverSpeed;
			base.SetModifiedAndInvalidateInfluencedCache(196, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				int dataSize = this._stanceRecoverSpeed.GetSerializedSize();
				byte* pData = OperationAdder.DynamicObjectCollection_SetDynamicField<int>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this._id, 195, dataSize);
				pData += this._stanceRecoverSpeed.Serialize(pData);
			}
		}

		// Token: 0x0600275D RID: 10077 RVA: 0x001D9448 File Offset: 0x001D7648
		public SpecialEffectList GetMobilityRecoverSpeed()
		{
			return this._mobilityRecoverSpeed;
		}

		// Token: 0x0600275E RID: 10078 RVA: 0x001D9460 File Offset: 0x001D7660
		public unsafe void SetMobilityRecoverSpeed(SpecialEffectList mobilityRecoverSpeed, DataContext context)
		{
			this._mobilityRecoverSpeed = mobilityRecoverSpeed;
			base.SetModifiedAndInvalidateInfluencedCache(197, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				int dataSize = this._mobilityRecoverSpeed.GetSerializedSize();
				byte* pData = OperationAdder.DynamicObjectCollection_SetDynamicField<int>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this._id, 196, dataSize);
				pData += this._mobilityRecoverSpeed.Serialize(pData);
			}
		}

		// Token: 0x0600275F RID: 10079 RVA: 0x001D94D8 File Offset: 0x001D76D8
		public SpecialEffectList GetChangeTrickProgressAddValue()
		{
			return this._changeTrickProgressAddValue;
		}

		// Token: 0x06002760 RID: 10080 RVA: 0x001D94F0 File Offset: 0x001D76F0
		public unsafe void SetChangeTrickProgressAddValue(SpecialEffectList changeTrickProgressAddValue, DataContext context)
		{
			this._changeTrickProgressAddValue = changeTrickProgressAddValue;
			base.SetModifiedAndInvalidateInfluencedCache(198, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				int dataSize = this._changeTrickProgressAddValue.GetSerializedSize();
				byte* pData = OperationAdder.DynamicObjectCollection_SetDynamicField<int>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this._id, 197, dataSize);
				pData += this._changeTrickProgressAddValue.Serialize(pData);
			}
		}

		// Token: 0x06002761 RID: 10081 RVA: 0x001D9568 File Offset: 0x001D7768
		public SpecialEffectList GetPower()
		{
			return this._power;
		}

		// Token: 0x06002762 RID: 10082 RVA: 0x001D9580 File Offset: 0x001D7780
		public unsafe void SetPower(SpecialEffectList power, DataContext context)
		{
			this._power = power;
			base.SetModifiedAndInvalidateInfluencedCache(199, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				int dataSize = this._power.GetSerializedSize();
				byte* pData = OperationAdder.DynamicObjectCollection_SetDynamicField<int>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this._id, 198, dataSize);
				pData += this._power.Serialize(pData);
			}
		}

		// Token: 0x06002763 RID: 10083 RVA: 0x001D95F8 File Offset: 0x001D77F8
		public SpecialEffectList GetMaxPower()
		{
			return this._maxPower;
		}

		// Token: 0x06002764 RID: 10084 RVA: 0x001D9610 File Offset: 0x001D7810
		public unsafe void SetMaxPower(SpecialEffectList maxPower, DataContext context)
		{
			this._maxPower = maxPower;
			base.SetModifiedAndInvalidateInfluencedCache(200, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				int dataSize = this._maxPower.GetSerializedSize();
				byte* pData = OperationAdder.DynamicObjectCollection_SetDynamicField<int>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this._id, 199, dataSize);
				pData += this._maxPower.Serialize(pData);
			}
		}

		// Token: 0x06002765 RID: 10085 RVA: 0x001D9688 File Offset: 0x001D7888
		public SpecialEffectList GetPowerCanReduce()
		{
			return this._powerCanReduce;
		}

		// Token: 0x06002766 RID: 10086 RVA: 0x001D96A0 File Offset: 0x001D78A0
		public unsafe void SetPowerCanReduce(SpecialEffectList powerCanReduce, DataContext context)
		{
			this._powerCanReduce = powerCanReduce;
			base.SetModifiedAndInvalidateInfluencedCache(201, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				int dataSize = this._powerCanReduce.GetSerializedSize();
				byte* pData = OperationAdder.DynamicObjectCollection_SetDynamicField<int>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this._id, 200, dataSize);
				pData += this._powerCanReduce.Serialize(pData);
			}
		}

		// Token: 0x06002767 RID: 10087 RVA: 0x001D9718 File Offset: 0x001D7918
		public SpecialEffectList GetUseRequirement()
		{
			return this._useRequirement;
		}

		// Token: 0x06002768 RID: 10088 RVA: 0x001D9730 File Offset: 0x001D7930
		public unsafe void SetUseRequirement(SpecialEffectList useRequirement, DataContext context)
		{
			this._useRequirement = useRequirement;
			base.SetModifiedAndInvalidateInfluencedCache(202, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				int dataSize = this._useRequirement.GetSerializedSize();
				byte* pData = OperationAdder.DynamicObjectCollection_SetDynamicField<int>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this._id, 201, dataSize);
				pData += this._useRequirement.Serialize(pData);
			}
		}

		// Token: 0x06002769 RID: 10089 RVA: 0x001D97A8 File Offset: 0x001D79A8
		public SpecialEffectList GetCurrInnerRatio()
		{
			return this._currInnerRatio;
		}

		// Token: 0x0600276A RID: 10090 RVA: 0x001D97C0 File Offset: 0x001D79C0
		public unsafe void SetCurrInnerRatio(SpecialEffectList currInnerRatio, DataContext context)
		{
			this._currInnerRatio = currInnerRatio;
			base.SetModifiedAndInvalidateInfluencedCache(203, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				int dataSize = this._currInnerRatio.GetSerializedSize();
				byte* pData = OperationAdder.DynamicObjectCollection_SetDynamicField<int>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this._id, 202, dataSize);
				pData += this._currInnerRatio.Serialize(pData);
			}
		}

		// Token: 0x0600276B RID: 10091 RVA: 0x001D9838 File Offset: 0x001D7A38
		public SpecialEffectList GetCostBreathAndStance()
		{
			return this._costBreathAndStance;
		}

		// Token: 0x0600276C RID: 10092 RVA: 0x001D9850 File Offset: 0x001D7A50
		public unsafe void SetCostBreathAndStance(SpecialEffectList costBreathAndStance, DataContext context)
		{
			this._costBreathAndStance = costBreathAndStance;
			base.SetModifiedAndInvalidateInfluencedCache(204, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				int dataSize = this._costBreathAndStance.GetSerializedSize();
				byte* pData = OperationAdder.DynamicObjectCollection_SetDynamicField<int>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this._id, 203, dataSize);
				pData += this._costBreathAndStance.Serialize(pData);
			}
		}

		// Token: 0x0600276D RID: 10093 RVA: 0x001D98C8 File Offset: 0x001D7AC8
		public SpecialEffectList GetCostBreath()
		{
			return this._costBreath;
		}

		// Token: 0x0600276E RID: 10094 RVA: 0x001D98E0 File Offset: 0x001D7AE0
		public unsafe void SetCostBreath(SpecialEffectList costBreath, DataContext context)
		{
			this._costBreath = costBreath;
			base.SetModifiedAndInvalidateInfluencedCache(205, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				int dataSize = this._costBreath.GetSerializedSize();
				byte* pData = OperationAdder.DynamicObjectCollection_SetDynamicField<int>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this._id, 204, dataSize);
				pData += this._costBreath.Serialize(pData);
			}
		}

		// Token: 0x0600276F RID: 10095 RVA: 0x001D9958 File Offset: 0x001D7B58
		public SpecialEffectList GetCostStance()
		{
			return this._costStance;
		}

		// Token: 0x06002770 RID: 10096 RVA: 0x001D9970 File Offset: 0x001D7B70
		public unsafe void SetCostStance(SpecialEffectList costStance, DataContext context)
		{
			this._costStance = costStance;
			base.SetModifiedAndInvalidateInfluencedCache(206, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				int dataSize = this._costStance.GetSerializedSize();
				byte* pData = OperationAdder.DynamicObjectCollection_SetDynamicField<int>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this._id, 205, dataSize);
				pData += this._costStance.Serialize(pData);
			}
		}

		// Token: 0x06002771 RID: 10097 RVA: 0x001D99E8 File Offset: 0x001D7BE8
		public SpecialEffectList GetCostMobility()
		{
			return this._costMobility;
		}

		// Token: 0x06002772 RID: 10098 RVA: 0x001D9A00 File Offset: 0x001D7C00
		public unsafe void SetCostMobility(SpecialEffectList costMobility, DataContext context)
		{
			this._costMobility = costMobility;
			base.SetModifiedAndInvalidateInfluencedCache(207, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				int dataSize = this._costMobility.GetSerializedSize();
				byte* pData = OperationAdder.DynamicObjectCollection_SetDynamicField<int>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this._id, 206, dataSize);
				pData += this._costMobility.Serialize(pData);
			}
		}

		// Token: 0x06002773 RID: 10099 RVA: 0x001D9A78 File Offset: 0x001D7C78
		public SpecialEffectList GetSkillCostTricks()
		{
			return this._skillCostTricks;
		}

		// Token: 0x06002774 RID: 10100 RVA: 0x001D9A90 File Offset: 0x001D7C90
		public unsafe void SetSkillCostTricks(SpecialEffectList skillCostTricks, DataContext context)
		{
			this._skillCostTricks = skillCostTricks;
			base.SetModifiedAndInvalidateInfluencedCache(208, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				int dataSize = this._skillCostTricks.GetSerializedSize();
				byte* pData = OperationAdder.DynamicObjectCollection_SetDynamicField<int>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this._id, 207, dataSize);
				pData += this._skillCostTricks.Serialize(pData);
			}
		}

		// Token: 0x06002775 RID: 10101 RVA: 0x001D9B08 File Offset: 0x001D7D08
		public SpecialEffectList GetEffectDirection()
		{
			return this._effectDirection;
		}

		// Token: 0x06002776 RID: 10102 RVA: 0x001D9B20 File Offset: 0x001D7D20
		public unsafe void SetEffectDirection(SpecialEffectList effectDirection, DataContext context)
		{
			this._effectDirection = effectDirection;
			base.SetModifiedAndInvalidateInfluencedCache(209, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				int dataSize = this._effectDirection.GetSerializedSize();
				byte* pData = OperationAdder.DynamicObjectCollection_SetDynamicField<int>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this._id, 208, dataSize);
				pData += this._effectDirection.Serialize(pData);
			}
		}

		// Token: 0x06002777 RID: 10103 RVA: 0x001D9B98 File Offset: 0x001D7D98
		public SpecialEffectList GetEffectDirectionCanChange()
		{
			return this._effectDirectionCanChange;
		}

		// Token: 0x06002778 RID: 10104 RVA: 0x001D9BB0 File Offset: 0x001D7DB0
		public unsafe void SetEffectDirectionCanChange(SpecialEffectList effectDirectionCanChange, DataContext context)
		{
			this._effectDirectionCanChange = effectDirectionCanChange;
			base.SetModifiedAndInvalidateInfluencedCache(210, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				int dataSize = this._effectDirectionCanChange.GetSerializedSize();
				byte* pData = OperationAdder.DynamicObjectCollection_SetDynamicField<int>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this._id, 209, dataSize);
				pData += this._effectDirectionCanChange.Serialize(pData);
			}
		}

		// Token: 0x06002779 RID: 10105 RVA: 0x001D9C28 File Offset: 0x001D7E28
		public SpecialEffectList GetGridCost()
		{
			return this._gridCost;
		}

		// Token: 0x0600277A RID: 10106 RVA: 0x001D9C40 File Offset: 0x001D7E40
		public unsafe void SetGridCost(SpecialEffectList gridCost, DataContext context)
		{
			this._gridCost = gridCost;
			base.SetModifiedAndInvalidateInfluencedCache(211, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				int dataSize = this._gridCost.GetSerializedSize();
				byte* pData = OperationAdder.DynamicObjectCollection_SetDynamicField<int>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this._id, 210, dataSize);
				pData += this._gridCost.Serialize(pData);
			}
		}

		// Token: 0x0600277B RID: 10107 RVA: 0x001D9CB8 File Offset: 0x001D7EB8
		public SpecialEffectList GetPrepareTotalProgress()
		{
			return this._prepareTotalProgress;
		}

		// Token: 0x0600277C RID: 10108 RVA: 0x001D9CD0 File Offset: 0x001D7ED0
		public unsafe void SetPrepareTotalProgress(SpecialEffectList prepareTotalProgress, DataContext context)
		{
			this._prepareTotalProgress = prepareTotalProgress;
			base.SetModifiedAndInvalidateInfluencedCache(212, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				int dataSize = this._prepareTotalProgress.GetSerializedSize();
				byte* pData = OperationAdder.DynamicObjectCollection_SetDynamicField<int>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this._id, 211, dataSize);
				pData += this._prepareTotalProgress.Serialize(pData);
			}
		}

		// Token: 0x0600277D RID: 10109 RVA: 0x001D9D48 File Offset: 0x001D7F48
		public SpecialEffectList GetSpecificGridCount()
		{
			return this._specificGridCount;
		}

		// Token: 0x0600277E RID: 10110 RVA: 0x001D9D60 File Offset: 0x001D7F60
		public unsafe void SetSpecificGridCount(SpecialEffectList specificGridCount, DataContext context)
		{
			this._specificGridCount = specificGridCount;
			base.SetModifiedAndInvalidateInfluencedCache(213, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				int dataSize = this._specificGridCount.GetSerializedSize();
				byte* pData = OperationAdder.DynamicObjectCollection_SetDynamicField<int>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this._id, 212, dataSize);
				pData += this._specificGridCount.Serialize(pData);
			}
		}

		// Token: 0x0600277F RID: 10111 RVA: 0x001D9DD8 File Offset: 0x001D7FD8
		public SpecialEffectList GetGenericGridCount()
		{
			return this._genericGridCount;
		}

		// Token: 0x06002780 RID: 10112 RVA: 0x001D9DF0 File Offset: 0x001D7FF0
		public unsafe void SetGenericGridCount(SpecialEffectList genericGridCount, DataContext context)
		{
			this._genericGridCount = genericGridCount;
			base.SetModifiedAndInvalidateInfluencedCache(214, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				int dataSize = this._genericGridCount.GetSerializedSize();
				byte* pData = OperationAdder.DynamicObjectCollection_SetDynamicField<int>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this._id, 213, dataSize);
				pData += this._genericGridCount.Serialize(pData);
			}
		}

		// Token: 0x06002781 RID: 10113 RVA: 0x001D9E68 File Offset: 0x001D8068
		public SpecialEffectList GetCanInterrupt()
		{
			return this._canInterrupt;
		}

		// Token: 0x06002782 RID: 10114 RVA: 0x001D9E80 File Offset: 0x001D8080
		public unsafe void SetCanInterrupt(SpecialEffectList canInterrupt, DataContext context)
		{
			this._canInterrupt = canInterrupt;
			base.SetModifiedAndInvalidateInfluencedCache(215, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				int dataSize = this._canInterrupt.GetSerializedSize();
				byte* pData = OperationAdder.DynamicObjectCollection_SetDynamicField<int>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this._id, 214, dataSize);
				pData += this._canInterrupt.Serialize(pData);
			}
		}

		// Token: 0x06002783 RID: 10115 RVA: 0x001D9EF8 File Offset: 0x001D80F8
		public SpecialEffectList GetInterruptOdds()
		{
			return this._interruptOdds;
		}

		// Token: 0x06002784 RID: 10116 RVA: 0x001D9F10 File Offset: 0x001D8110
		public unsafe void SetInterruptOdds(SpecialEffectList interruptOdds, DataContext context)
		{
			this._interruptOdds = interruptOdds;
			base.SetModifiedAndInvalidateInfluencedCache(216, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				int dataSize = this._interruptOdds.GetSerializedSize();
				byte* pData = OperationAdder.DynamicObjectCollection_SetDynamicField<int>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this._id, 215, dataSize);
				pData += this._interruptOdds.Serialize(pData);
			}
		}

		// Token: 0x06002785 RID: 10117 RVA: 0x001D9F88 File Offset: 0x001D8188
		public SpecialEffectList GetCanSilence()
		{
			return this._canSilence;
		}

		// Token: 0x06002786 RID: 10118 RVA: 0x001D9FA0 File Offset: 0x001D81A0
		public unsafe void SetCanSilence(SpecialEffectList canSilence, DataContext context)
		{
			this._canSilence = canSilence;
			base.SetModifiedAndInvalidateInfluencedCache(217, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				int dataSize = this._canSilence.GetSerializedSize();
				byte* pData = OperationAdder.DynamicObjectCollection_SetDynamicField<int>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this._id, 216, dataSize);
				pData += this._canSilence.Serialize(pData);
			}
		}

		// Token: 0x06002787 RID: 10119 RVA: 0x001DA018 File Offset: 0x001D8218
		public SpecialEffectList GetSilenceOdds()
		{
			return this._silenceOdds;
		}

		// Token: 0x06002788 RID: 10120 RVA: 0x001DA030 File Offset: 0x001D8230
		public unsafe void SetSilenceOdds(SpecialEffectList silenceOdds, DataContext context)
		{
			this._silenceOdds = silenceOdds;
			base.SetModifiedAndInvalidateInfluencedCache(218, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				int dataSize = this._silenceOdds.GetSerializedSize();
				byte* pData = OperationAdder.DynamicObjectCollection_SetDynamicField<int>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this._id, 217, dataSize);
				pData += this._silenceOdds.Serialize(pData);
			}
		}

		// Token: 0x06002789 RID: 10121 RVA: 0x001DA0A8 File Offset: 0x001D82A8
		public SpecialEffectList GetCanCastWithBrokenBodyPart()
		{
			return this._canCastWithBrokenBodyPart;
		}

		// Token: 0x0600278A RID: 10122 RVA: 0x001DA0C0 File Offset: 0x001D82C0
		public unsafe void SetCanCastWithBrokenBodyPart(SpecialEffectList canCastWithBrokenBodyPart, DataContext context)
		{
			this._canCastWithBrokenBodyPart = canCastWithBrokenBodyPart;
			base.SetModifiedAndInvalidateInfluencedCache(219, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				int dataSize = this._canCastWithBrokenBodyPart.GetSerializedSize();
				byte* pData = OperationAdder.DynamicObjectCollection_SetDynamicField<int>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this._id, 218, dataSize);
				pData += this._canCastWithBrokenBodyPart.Serialize(pData);
			}
		}

		// Token: 0x0600278B RID: 10123 RVA: 0x001DA138 File Offset: 0x001D8338
		public SpecialEffectList GetAddPowerCanBeRemoved()
		{
			return this._addPowerCanBeRemoved;
		}

		// Token: 0x0600278C RID: 10124 RVA: 0x001DA150 File Offset: 0x001D8350
		public unsafe void SetAddPowerCanBeRemoved(SpecialEffectList addPowerCanBeRemoved, DataContext context)
		{
			this._addPowerCanBeRemoved = addPowerCanBeRemoved;
			base.SetModifiedAndInvalidateInfluencedCache(220, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				int dataSize = this._addPowerCanBeRemoved.GetSerializedSize();
				byte* pData = OperationAdder.DynamicObjectCollection_SetDynamicField<int>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this._id, 219, dataSize);
				pData += this._addPowerCanBeRemoved.Serialize(pData);
			}
		}

		// Token: 0x0600278D RID: 10125 RVA: 0x001DA1C8 File Offset: 0x001D83C8
		public SpecialEffectList GetSkillType()
		{
			return this._skillType;
		}

		// Token: 0x0600278E RID: 10126 RVA: 0x001DA1E0 File Offset: 0x001D83E0
		public unsafe void SetSkillType(SpecialEffectList skillType, DataContext context)
		{
			this._skillType = skillType;
			base.SetModifiedAndInvalidateInfluencedCache(221, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				int dataSize = this._skillType.GetSerializedSize();
				byte* pData = OperationAdder.DynamicObjectCollection_SetDynamicField<int>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this._id, 220, dataSize);
				pData += this._skillType.Serialize(pData);
			}
		}

		// Token: 0x0600278F RID: 10127 RVA: 0x001DA258 File Offset: 0x001D8458
		public SpecialEffectList GetEffectCountCanChange()
		{
			return this._effectCountCanChange;
		}

		// Token: 0x06002790 RID: 10128 RVA: 0x001DA270 File Offset: 0x001D8470
		public unsafe void SetEffectCountCanChange(SpecialEffectList effectCountCanChange, DataContext context)
		{
			this._effectCountCanChange = effectCountCanChange;
			base.SetModifiedAndInvalidateInfluencedCache(222, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				int dataSize = this._effectCountCanChange.GetSerializedSize();
				byte* pData = OperationAdder.DynamicObjectCollection_SetDynamicField<int>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this._id, 221, dataSize);
				pData += this._effectCountCanChange.Serialize(pData);
			}
		}

		// Token: 0x06002791 RID: 10129 RVA: 0x001DA2E8 File Offset: 0x001D84E8
		public SpecialEffectList GetCanCastInDefend()
		{
			return this._canCastInDefend;
		}

		// Token: 0x06002792 RID: 10130 RVA: 0x001DA300 File Offset: 0x001D8500
		public unsafe void SetCanCastInDefend(SpecialEffectList canCastInDefend, DataContext context)
		{
			this._canCastInDefend = canCastInDefend;
			base.SetModifiedAndInvalidateInfluencedCache(223, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				int dataSize = this._canCastInDefend.GetSerializedSize();
				byte* pData = OperationAdder.DynamicObjectCollection_SetDynamicField<int>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this._id, 222, dataSize);
				pData += this._canCastInDefend.Serialize(pData);
			}
		}

		// Token: 0x06002793 RID: 10131 RVA: 0x001DA378 File Offset: 0x001D8578
		public SpecialEffectList GetHitDistribution()
		{
			return this._hitDistribution;
		}

		// Token: 0x06002794 RID: 10132 RVA: 0x001DA390 File Offset: 0x001D8590
		public unsafe void SetHitDistribution(SpecialEffectList hitDistribution, DataContext context)
		{
			this._hitDistribution = hitDistribution;
			base.SetModifiedAndInvalidateInfluencedCache(224, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				int dataSize = this._hitDistribution.GetSerializedSize();
				byte* pData = OperationAdder.DynamicObjectCollection_SetDynamicField<int>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this._id, 223, dataSize);
				pData += this._hitDistribution.Serialize(pData);
			}
		}

		// Token: 0x06002795 RID: 10133 RVA: 0x001DA408 File Offset: 0x001D8608
		public SpecialEffectList GetCanCastOnLackBreath()
		{
			return this._canCastOnLackBreath;
		}

		// Token: 0x06002796 RID: 10134 RVA: 0x001DA420 File Offset: 0x001D8620
		public unsafe void SetCanCastOnLackBreath(SpecialEffectList canCastOnLackBreath, DataContext context)
		{
			this._canCastOnLackBreath = canCastOnLackBreath;
			base.SetModifiedAndInvalidateInfluencedCache(225, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				int dataSize = this._canCastOnLackBreath.GetSerializedSize();
				byte* pData = OperationAdder.DynamicObjectCollection_SetDynamicField<int>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this._id, 224, dataSize);
				pData += this._canCastOnLackBreath.Serialize(pData);
			}
		}

		// Token: 0x06002797 RID: 10135 RVA: 0x001DA498 File Offset: 0x001D8698
		public SpecialEffectList GetCanCastOnLackStance()
		{
			return this._canCastOnLackStance;
		}

		// Token: 0x06002798 RID: 10136 RVA: 0x001DA4B0 File Offset: 0x001D86B0
		public unsafe void SetCanCastOnLackStance(SpecialEffectList canCastOnLackStance, DataContext context)
		{
			this._canCastOnLackStance = canCastOnLackStance;
			base.SetModifiedAndInvalidateInfluencedCache(226, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				int dataSize = this._canCastOnLackStance.GetSerializedSize();
				byte* pData = OperationAdder.DynamicObjectCollection_SetDynamicField<int>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this._id, 225, dataSize);
				pData += this._canCastOnLackStance.Serialize(pData);
			}
		}

		// Token: 0x06002799 RID: 10137 RVA: 0x001DA528 File Offset: 0x001D8728
		public SpecialEffectList GetCostBreathOnCast()
		{
			return this._costBreathOnCast;
		}

		// Token: 0x0600279A RID: 10138 RVA: 0x001DA540 File Offset: 0x001D8740
		public unsafe void SetCostBreathOnCast(SpecialEffectList costBreathOnCast, DataContext context)
		{
			this._costBreathOnCast = costBreathOnCast;
			base.SetModifiedAndInvalidateInfluencedCache(227, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				int dataSize = this._costBreathOnCast.GetSerializedSize();
				byte* pData = OperationAdder.DynamicObjectCollection_SetDynamicField<int>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this._id, 226, dataSize);
				pData += this._costBreathOnCast.Serialize(pData);
			}
		}

		// Token: 0x0600279B RID: 10139 RVA: 0x001DA5B8 File Offset: 0x001D87B8
		public SpecialEffectList GetCostStanceOnCast()
		{
			return this._costStanceOnCast;
		}

		// Token: 0x0600279C RID: 10140 RVA: 0x001DA5D0 File Offset: 0x001D87D0
		public unsafe void SetCostStanceOnCast(SpecialEffectList costStanceOnCast, DataContext context)
		{
			this._costStanceOnCast = costStanceOnCast;
			base.SetModifiedAndInvalidateInfluencedCache(228, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				int dataSize = this._costStanceOnCast.GetSerializedSize();
				byte* pData = OperationAdder.DynamicObjectCollection_SetDynamicField<int>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this._id, 227, dataSize);
				pData += this._costStanceOnCast.Serialize(pData);
			}
		}

		// Token: 0x0600279D RID: 10141 RVA: 0x001DA648 File Offset: 0x001D8848
		public SpecialEffectList GetCanUseMobilityAsBreath()
		{
			return this._canUseMobilityAsBreath;
		}

		// Token: 0x0600279E RID: 10142 RVA: 0x001DA660 File Offset: 0x001D8860
		public unsafe void SetCanUseMobilityAsBreath(SpecialEffectList canUseMobilityAsBreath, DataContext context)
		{
			this._canUseMobilityAsBreath = canUseMobilityAsBreath;
			base.SetModifiedAndInvalidateInfluencedCache(229, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				int dataSize = this._canUseMobilityAsBreath.GetSerializedSize();
				byte* pData = OperationAdder.DynamicObjectCollection_SetDynamicField<int>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this._id, 228, dataSize);
				pData += this._canUseMobilityAsBreath.Serialize(pData);
			}
		}

		// Token: 0x0600279F RID: 10143 RVA: 0x001DA6D8 File Offset: 0x001D88D8
		public SpecialEffectList GetCanUseMobilityAsStance()
		{
			return this._canUseMobilityAsStance;
		}

		// Token: 0x060027A0 RID: 10144 RVA: 0x001DA6F0 File Offset: 0x001D88F0
		public unsafe void SetCanUseMobilityAsStance(SpecialEffectList canUseMobilityAsStance, DataContext context)
		{
			this._canUseMobilityAsStance = canUseMobilityAsStance;
			base.SetModifiedAndInvalidateInfluencedCache(230, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				int dataSize = this._canUseMobilityAsStance.GetSerializedSize();
				byte* pData = OperationAdder.DynamicObjectCollection_SetDynamicField<int>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this._id, 229, dataSize);
				pData += this._canUseMobilityAsStance.Serialize(pData);
			}
		}

		// Token: 0x060027A1 RID: 10145 RVA: 0x001DA768 File Offset: 0x001D8968
		public SpecialEffectList GetCastCostNeiliAllocation()
		{
			return this._castCostNeiliAllocation;
		}

		// Token: 0x060027A2 RID: 10146 RVA: 0x001DA780 File Offset: 0x001D8980
		public unsafe void SetCastCostNeiliAllocation(SpecialEffectList castCostNeiliAllocation, DataContext context)
		{
			this._castCostNeiliAllocation = castCostNeiliAllocation;
			base.SetModifiedAndInvalidateInfluencedCache(231, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				int dataSize = this._castCostNeiliAllocation.GetSerializedSize();
				byte* pData = OperationAdder.DynamicObjectCollection_SetDynamicField<int>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this._id, 230, dataSize);
				pData += this._castCostNeiliAllocation.Serialize(pData);
			}
		}

		// Token: 0x060027A3 RID: 10147 RVA: 0x001DA7F8 File Offset: 0x001D89F8
		public SpecialEffectList GetAcceptPoisonResist()
		{
			return this._acceptPoisonResist;
		}

		// Token: 0x060027A4 RID: 10148 RVA: 0x001DA810 File Offset: 0x001D8A10
		public unsafe void SetAcceptPoisonResist(SpecialEffectList acceptPoisonResist, DataContext context)
		{
			this._acceptPoisonResist = acceptPoisonResist;
			base.SetModifiedAndInvalidateInfluencedCache(232, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				int dataSize = this._acceptPoisonResist.GetSerializedSize();
				byte* pData = OperationAdder.DynamicObjectCollection_SetDynamicField<int>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this._id, 231, dataSize);
				pData += this._acceptPoisonResist.Serialize(pData);
			}
		}

		// Token: 0x060027A5 RID: 10149 RVA: 0x001DA888 File Offset: 0x001D8A88
		public SpecialEffectList GetMakePoisonResist()
		{
			return this._makePoisonResist;
		}

		// Token: 0x060027A6 RID: 10150 RVA: 0x001DA8A0 File Offset: 0x001D8AA0
		public unsafe void SetMakePoisonResist(SpecialEffectList makePoisonResist, DataContext context)
		{
			this._makePoisonResist = makePoisonResist;
			base.SetModifiedAndInvalidateInfluencedCache(233, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				int dataSize = this._makePoisonResist.GetSerializedSize();
				byte* pData = OperationAdder.DynamicObjectCollection_SetDynamicField<int>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this._id, 232, dataSize);
				pData += this._makePoisonResist.Serialize(pData);
			}
		}

		// Token: 0x060027A7 RID: 10151 RVA: 0x001DA918 File Offset: 0x001D8B18
		public SpecialEffectList GetCanCriticalHit()
		{
			return this._canCriticalHit;
		}

		// Token: 0x060027A8 RID: 10152 RVA: 0x001DA930 File Offset: 0x001D8B30
		public unsafe void SetCanCriticalHit(SpecialEffectList canCriticalHit, DataContext context)
		{
			this._canCriticalHit = canCriticalHit;
			base.SetModifiedAndInvalidateInfluencedCache(234, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				int dataSize = this._canCriticalHit.GetSerializedSize();
				byte* pData = OperationAdder.DynamicObjectCollection_SetDynamicField<int>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this._id, 233, dataSize);
				pData += this._canCriticalHit.Serialize(pData);
			}
		}

		// Token: 0x060027A9 RID: 10153 RVA: 0x001DA9A8 File Offset: 0x001D8BA8
		public SpecialEffectList GetCanCostNeiliAllocationEffect()
		{
			return this._canCostNeiliAllocationEffect;
		}

		// Token: 0x060027AA RID: 10154 RVA: 0x001DA9C0 File Offset: 0x001D8BC0
		public unsafe void SetCanCostNeiliAllocationEffect(SpecialEffectList canCostNeiliAllocationEffect, DataContext context)
		{
			this._canCostNeiliAllocationEffect = canCostNeiliAllocationEffect;
			base.SetModifiedAndInvalidateInfluencedCache(235, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				int dataSize = this._canCostNeiliAllocationEffect.GetSerializedSize();
				byte* pData = OperationAdder.DynamicObjectCollection_SetDynamicField<int>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this._id, 234, dataSize);
				pData += this._canCostNeiliAllocationEffect.Serialize(pData);
			}
		}

		// Token: 0x060027AB RID: 10155 RVA: 0x001DAA38 File Offset: 0x001D8C38
		public SpecialEffectList GetConsummateLevelRelatedMainAttributesHitValues()
		{
			return this._consummateLevelRelatedMainAttributesHitValues;
		}

		// Token: 0x060027AC RID: 10156 RVA: 0x001DAA50 File Offset: 0x001D8C50
		public unsafe void SetConsummateLevelRelatedMainAttributesHitValues(SpecialEffectList consummateLevelRelatedMainAttributesHitValues, DataContext context)
		{
			this._consummateLevelRelatedMainAttributesHitValues = consummateLevelRelatedMainAttributesHitValues;
			base.SetModifiedAndInvalidateInfluencedCache(236, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				int dataSize = this._consummateLevelRelatedMainAttributesHitValues.GetSerializedSize();
				byte* pData = OperationAdder.DynamicObjectCollection_SetDynamicField<int>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this._id, 235, dataSize);
				pData += this._consummateLevelRelatedMainAttributesHitValues.Serialize(pData);
			}
		}

		// Token: 0x060027AD RID: 10157 RVA: 0x001DAAC8 File Offset: 0x001D8CC8
		public SpecialEffectList GetConsummateLevelRelatedMainAttributesAvoidValues()
		{
			return this._consummateLevelRelatedMainAttributesAvoidValues;
		}

		// Token: 0x060027AE RID: 10158 RVA: 0x001DAAE0 File Offset: 0x001D8CE0
		public unsafe void SetConsummateLevelRelatedMainAttributesAvoidValues(SpecialEffectList consummateLevelRelatedMainAttributesAvoidValues, DataContext context)
		{
			this._consummateLevelRelatedMainAttributesAvoidValues = consummateLevelRelatedMainAttributesAvoidValues;
			base.SetModifiedAndInvalidateInfluencedCache(237, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				int dataSize = this._consummateLevelRelatedMainAttributesAvoidValues.GetSerializedSize();
				byte* pData = OperationAdder.DynamicObjectCollection_SetDynamicField<int>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this._id, 236, dataSize);
				pData += this._consummateLevelRelatedMainAttributesAvoidValues.Serialize(pData);
			}
		}

		// Token: 0x060027AF RID: 10159 RVA: 0x001DAB58 File Offset: 0x001D8D58
		public SpecialEffectList GetConsummateLevelRelatedMainAttributesPenetrations()
		{
			return this._consummateLevelRelatedMainAttributesPenetrations;
		}

		// Token: 0x060027B0 RID: 10160 RVA: 0x001DAB70 File Offset: 0x001D8D70
		public unsafe void SetConsummateLevelRelatedMainAttributesPenetrations(SpecialEffectList consummateLevelRelatedMainAttributesPenetrations, DataContext context)
		{
			this._consummateLevelRelatedMainAttributesPenetrations = consummateLevelRelatedMainAttributesPenetrations;
			base.SetModifiedAndInvalidateInfluencedCache(238, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				int dataSize = this._consummateLevelRelatedMainAttributesPenetrations.GetSerializedSize();
				byte* pData = OperationAdder.DynamicObjectCollection_SetDynamicField<int>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this._id, 237, dataSize);
				pData += this._consummateLevelRelatedMainAttributesPenetrations.Serialize(pData);
			}
		}

		// Token: 0x060027B1 RID: 10161 RVA: 0x001DABE8 File Offset: 0x001D8DE8
		public SpecialEffectList GetConsummateLevelRelatedMainAttributesPenetrationResists()
		{
			return this._consummateLevelRelatedMainAttributesPenetrationResists;
		}

		// Token: 0x060027B2 RID: 10162 RVA: 0x001DAC00 File Offset: 0x001D8E00
		public unsafe void SetConsummateLevelRelatedMainAttributesPenetrationResists(SpecialEffectList consummateLevelRelatedMainAttributesPenetrationResists, DataContext context)
		{
			this._consummateLevelRelatedMainAttributesPenetrationResists = consummateLevelRelatedMainAttributesPenetrationResists;
			base.SetModifiedAndInvalidateInfluencedCache(239, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				int dataSize = this._consummateLevelRelatedMainAttributesPenetrationResists.GetSerializedSize();
				byte* pData = OperationAdder.DynamicObjectCollection_SetDynamicField<int>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this._id, 238, dataSize);
				pData += this._consummateLevelRelatedMainAttributesPenetrationResists.Serialize(pData);
			}
		}

		// Token: 0x060027B3 RID: 10163 RVA: 0x001DAC78 File Offset: 0x001D8E78
		public SpecialEffectList GetSkillAlsoAsFiveElements()
		{
			return this._skillAlsoAsFiveElements;
		}

		// Token: 0x060027B4 RID: 10164 RVA: 0x001DAC90 File Offset: 0x001D8E90
		public unsafe void SetSkillAlsoAsFiveElements(SpecialEffectList skillAlsoAsFiveElements, DataContext context)
		{
			this._skillAlsoAsFiveElements = skillAlsoAsFiveElements;
			base.SetModifiedAndInvalidateInfluencedCache(240, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				int dataSize = this._skillAlsoAsFiveElements.GetSerializedSize();
				byte* pData = OperationAdder.DynamicObjectCollection_SetDynamicField<int>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this._id, 239, dataSize);
				pData += this._skillAlsoAsFiveElements.Serialize(pData);
			}
		}

		// Token: 0x060027B5 RID: 10165 RVA: 0x001DAD08 File Offset: 0x001D8F08
		public SpecialEffectList GetInnerInjuryImmunity()
		{
			return this._innerInjuryImmunity;
		}

		// Token: 0x060027B6 RID: 10166 RVA: 0x001DAD20 File Offset: 0x001D8F20
		public unsafe void SetInnerInjuryImmunity(SpecialEffectList innerInjuryImmunity, DataContext context)
		{
			this._innerInjuryImmunity = innerInjuryImmunity;
			base.SetModifiedAndInvalidateInfluencedCache(241, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				int dataSize = this._innerInjuryImmunity.GetSerializedSize();
				byte* pData = OperationAdder.DynamicObjectCollection_SetDynamicField<int>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this._id, 240, dataSize);
				pData += this._innerInjuryImmunity.Serialize(pData);
			}
		}

		// Token: 0x060027B7 RID: 10167 RVA: 0x001DAD98 File Offset: 0x001D8F98
		public SpecialEffectList GetOuterInjuryImmunity()
		{
			return this._outerInjuryImmunity;
		}

		// Token: 0x060027B8 RID: 10168 RVA: 0x001DADB0 File Offset: 0x001D8FB0
		public unsafe void SetOuterInjuryImmunity(SpecialEffectList outerInjuryImmunity, DataContext context)
		{
			this._outerInjuryImmunity = outerInjuryImmunity;
			base.SetModifiedAndInvalidateInfluencedCache(242, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				int dataSize = this._outerInjuryImmunity.GetSerializedSize();
				byte* pData = OperationAdder.DynamicObjectCollection_SetDynamicField<int>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this._id, 241, dataSize);
				pData += this._outerInjuryImmunity.Serialize(pData);
			}
		}

		// Token: 0x060027B9 RID: 10169 RVA: 0x001DAE28 File Offset: 0x001D9028
		public SpecialEffectList GetPoisonAffectThreshold()
		{
			return this._poisonAffectThreshold;
		}

		// Token: 0x060027BA RID: 10170 RVA: 0x001DAE40 File Offset: 0x001D9040
		public unsafe void SetPoisonAffectThreshold(SpecialEffectList poisonAffectThreshold, DataContext context)
		{
			this._poisonAffectThreshold = poisonAffectThreshold;
			base.SetModifiedAndInvalidateInfluencedCache(243, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				int dataSize = this._poisonAffectThreshold.GetSerializedSize();
				byte* pData = OperationAdder.DynamicObjectCollection_SetDynamicField<int>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this._id, 242, dataSize);
				pData += this._poisonAffectThreshold.Serialize(pData);
			}
		}

		// Token: 0x060027BB RID: 10171 RVA: 0x001DAEB8 File Offset: 0x001D90B8
		public SpecialEffectList GetLockDistance()
		{
			return this._lockDistance;
		}

		// Token: 0x060027BC RID: 10172 RVA: 0x001DAED0 File Offset: 0x001D90D0
		public unsafe void SetLockDistance(SpecialEffectList lockDistance, DataContext context)
		{
			this._lockDistance = lockDistance;
			base.SetModifiedAndInvalidateInfluencedCache(244, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				int dataSize = this._lockDistance.GetSerializedSize();
				byte* pData = OperationAdder.DynamicObjectCollection_SetDynamicField<int>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this._id, 243, dataSize);
				pData += this._lockDistance.Serialize(pData);
			}
		}

		// Token: 0x060027BD RID: 10173 RVA: 0x001DAF48 File Offset: 0x001D9148
		public SpecialEffectList GetResistOfAllPoison()
		{
			return this._resistOfAllPoison;
		}

		// Token: 0x060027BE RID: 10174 RVA: 0x001DAF60 File Offset: 0x001D9160
		public unsafe void SetResistOfAllPoison(SpecialEffectList resistOfAllPoison, DataContext context)
		{
			this._resistOfAllPoison = resistOfAllPoison;
			base.SetModifiedAndInvalidateInfluencedCache(245, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				int dataSize = this._resistOfAllPoison.GetSerializedSize();
				byte* pData = OperationAdder.DynamicObjectCollection_SetDynamicField<int>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this._id, 244, dataSize);
				pData += this._resistOfAllPoison.Serialize(pData);
			}
		}

		// Token: 0x060027BF RID: 10175 RVA: 0x001DAFD8 File Offset: 0x001D91D8
		public SpecialEffectList GetMakePoisonTarget()
		{
			return this._makePoisonTarget;
		}

		// Token: 0x060027C0 RID: 10176 RVA: 0x001DAFF0 File Offset: 0x001D91F0
		public unsafe void SetMakePoisonTarget(SpecialEffectList makePoisonTarget, DataContext context)
		{
			this._makePoisonTarget = makePoisonTarget;
			base.SetModifiedAndInvalidateInfluencedCache(246, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				int dataSize = this._makePoisonTarget.GetSerializedSize();
				byte* pData = OperationAdder.DynamicObjectCollection_SetDynamicField<int>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this._id, 245, dataSize);
				pData += this._makePoisonTarget.Serialize(pData);
			}
		}

		// Token: 0x060027C1 RID: 10177 RVA: 0x001DB068 File Offset: 0x001D9268
		public SpecialEffectList GetAcceptPoisonTarget()
		{
			return this._acceptPoisonTarget;
		}

		// Token: 0x060027C2 RID: 10178 RVA: 0x001DB080 File Offset: 0x001D9280
		public unsafe void SetAcceptPoisonTarget(SpecialEffectList acceptPoisonTarget, DataContext context)
		{
			this._acceptPoisonTarget = acceptPoisonTarget;
			base.SetModifiedAndInvalidateInfluencedCache(247, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				int dataSize = this._acceptPoisonTarget.GetSerializedSize();
				byte* pData = OperationAdder.DynamicObjectCollection_SetDynamicField<int>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this._id, 246, dataSize);
				pData += this._acceptPoisonTarget.Serialize(pData);
			}
		}

		// Token: 0x060027C3 RID: 10179 RVA: 0x001DB0F8 File Offset: 0x001D92F8
		public SpecialEffectList GetCertainCriticalHit()
		{
			return this._certainCriticalHit;
		}

		// Token: 0x060027C4 RID: 10180 RVA: 0x001DB110 File Offset: 0x001D9310
		public unsafe void SetCertainCriticalHit(SpecialEffectList certainCriticalHit, DataContext context)
		{
			this._certainCriticalHit = certainCriticalHit;
			base.SetModifiedAndInvalidateInfluencedCache(248, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				int dataSize = this._certainCriticalHit.GetSerializedSize();
				byte* pData = OperationAdder.DynamicObjectCollection_SetDynamicField<int>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this._id, 247, dataSize);
				pData += this._certainCriticalHit.Serialize(pData);
			}
		}

		// Token: 0x060027C5 RID: 10181 RVA: 0x001DB188 File Offset: 0x001D9388
		public SpecialEffectList GetMindMarkCount()
		{
			return this._mindMarkCount;
		}

		// Token: 0x060027C6 RID: 10182 RVA: 0x001DB1A0 File Offset: 0x001D93A0
		public unsafe void SetMindMarkCount(SpecialEffectList mindMarkCount, DataContext context)
		{
			this._mindMarkCount = mindMarkCount;
			base.SetModifiedAndInvalidateInfluencedCache(249, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				int dataSize = this._mindMarkCount.GetSerializedSize();
				byte* pData = OperationAdder.DynamicObjectCollection_SetDynamicField<int>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this._id, 248, dataSize);
				pData += this._mindMarkCount.Serialize(pData);
			}
		}

		// Token: 0x060027C7 RID: 10183 RVA: 0x001DB218 File Offset: 0x001D9418
		public SpecialEffectList GetCanFightBackWithHit()
		{
			return this._canFightBackWithHit;
		}

		// Token: 0x060027C8 RID: 10184 RVA: 0x001DB230 File Offset: 0x001D9430
		public unsafe void SetCanFightBackWithHit(SpecialEffectList canFightBackWithHit, DataContext context)
		{
			this._canFightBackWithHit = canFightBackWithHit;
			base.SetModifiedAndInvalidateInfluencedCache(250, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				int dataSize = this._canFightBackWithHit.GetSerializedSize();
				byte* pData = OperationAdder.DynamicObjectCollection_SetDynamicField<int>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this._id, 249, dataSize);
				pData += this._canFightBackWithHit.Serialize(pData);
			}
		}

		// Token: 0x060027C9 RID: 10185 RVA: 0x001DB2A8 File Offset: 0x001D94A8
		public SpecialEffectList GetInevitableHit()
		{
			return this._inevitableHit;
		}

		// Token: 0x060027CA RID: 10186 RVA: 0x001DB2C0 File Offset: 0x001D94C0
		public unsafe void SetInevitableHit(SpecialEffectList inevitableHit, DataContext context)
		{
			this._inevitableHit = inevitableHit;
			base.SetModifiedAndInvalidateInfluencedCache(251, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				int dataSize = this._inevitableHit.GetSerializedSize();
				byte* pData = OperationAdder.DynamicObjectCollection_SetDynamicField<int>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this._id, 250, dataSize);
				pData += this._inevitableHit.Serialize(pData);
			}
		}

		// Token: 0x060027CB RID: 10187 RVA: 0x001DB338 File Offset: 0x001D9538
		public SpecialEffectList GetAttackCanPursue()
		{
			return this._attackCanPursue;
		}

		// Token: 0x060027CC RID: 10188 RVA: 0x001DB350 File Offset: 0x001D9550
		public unsafe void SetAttackCanPursue(SpecialEffectList attackCanPursue, DataContext context)
		{
			this._attackCanPursue = attackCanPursue;
			base.SetModifiedAndInvalidateInfluencedCache(252, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				int dataSize = this._attackCanPursue.GetSerializedSize();
				byte* pData = OperationAdder.DynamicObjectCollection_SetDynamicField<int>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this._id, 251, dataSize);
				pData += this._attackCanPursue.Serialize(pData);
			}
		}

		// Token: 0x060027CD RID: 10189 RVA: 0x001DB3C8 File Offset: 0x001D95C8
		public SpecialEffectList GetCombatSkillDataEffectList()
		{
			return this._combatSkillDataEffectList;
		}

		// Token: 0x060027CE RID: 10190 RVA: 0x001DB3E0 File Offset: 0x001D95E0
		public unsafe void SetCombatSkillDataEffectList(SpecialEffectList combatSkillDataEffectList, DataContext context)
		{
			this._combatSkillDataEffectList = combatSkillDataEffectList;
			base.SetModifiedAndInvalidateInfluencedCache(253, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				int dataSize = this._combatSkillDataEffectList.GetSerializedSize();
				byte* pData = OperationAdder.DynamicObjectCollection_SetDynamicField<int>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this._id, 252, dataSize);
				pData += this._combatSkillDataEffectList.Serialize(pData);
			}
		}

		// Token: 0x060027CF RID: 10191 RVA: 0x001DB458 File Offset: 0x001D9658
		public SpecialEffectList GetCriticalOdds()
		{
			return this._criticalOdds;
		}

		// Token: 0x060027D0 RID: 10192 RVA: 0x001DB470 File Offset: 0x001D9670
		public unsafe void SetCriticalOdds(SpecialEffectList criticalOdds, DataContext context)
		{
			this._criticalOdds = criticalOdds;
			base.SetModifiedAndInvalidateInfluencedCache(254, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				int dataSize = this._criticalOdds.GetSerializedSize();
				byte* pData = OperationAdder.DynamicObjectCollection_SetDynamicField<int>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this._id, 253, dataSize);
				pData += this._criticalOdds.Serialize(pData);
			}
		}

		// Token: 0x060027D1 RID: 10193 RVA: 0x001DB4E8 File Offset: 0x001D96E8
		public SpecialEffectList GetStanceCostByEffect()
		{
			return this._stanceCostByEffect;
		}

		// Token: 0x060027D2 RID: 10194 RVA: 0x001DB500 File Offset: 0x001D9700
		public unsafe void SetStanceCostByEffect(SpecialEffectList stanceCostByEffect, DataContext context)
		{
			this._stanceCostByEffect = stanceCostByEffect;
			base.SetModifiedAndInvalidateInfluencedCache(255, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				int dataSize = this._stanceCostByEffect.GetSerializedSize();
				byte* pData = OperationAdder.DynamicObjectCollection_SetDynamicField<int>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this._id, 254, dataSize);
				pData += this._stanceCostByEffect.Serialize(pData);
			}
		}

		// Token: 0x060027D3 RID: 10195 RVA: 0x001DB578 File Offset: 0x001D9778
		public SpecialEffectList GetBreathCostByEffect()
		{
			return this._breathCostByEffect;
		}

		// Token: 0x060027D4 RID: 10196 RVA: 0x001DB590 File Offset: 0x001D9790
		public unsafe void SetBreathCostByEffect(SpecialEffectList breathCostByEffect, DataContext context)
		{
			this._breathCostByEffect = breathCostByEffect;
			base.SetModifiedAndInvalidateInfluencedCache(256, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				int dataSize = this._breathCostByEffect.GetSerializedSize();
				byte* pData = OperationAdder.DynamicObjectCollection_SetDynamicField<int>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this._id, 255, dataSize);
				pData += this._breathCostByEffect.Serialize(pData);
			}
		}

		// Token: 0x060027D5 RID: 10197 RVA: 0x001DB608 File Offset: 0x001D9808
		public SpecialEffectList GetPowerAddRatio()
		{
			return this._powerAddRatio;
		}

		// Token: 0x060027D6 RID: 10198 RVA: 0x001DB620 File Offset: 0x001D9820
		public unsafe void SetPowerAddRatio(SpecialEffectList powerAddRatio, DataContext context)
		{
			this._powerAddRatio = powerAddRatio;
			base.SetModifiedAndInvalidateInfluencedCache(257, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				int dataSize = this._powerAddRatio.GetSerializedSize();
				byte* pData = OperationAdder.DynamicObjectCollection_SetDynamicField<int>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this._id, 256, dataSize);
				pData += this._powerAddRatio.Serialize(pData);
			}
		}

		// Token: 0x060027D7 RID: 10199 RVA: 0x001DB698 File Offset: 0x001D9898
		public SpecialEffectList GetPowerReduceRatio()
		{
			return this._powerReduceRatio;
		}

		// Token: 0x060027D8 RID: 10200 RVA: 0x001DB6B0 File Offset: 0x001D98B0
		public unsafe void SetPowerReduceRatio(SpecialEffectList powerReduceRatio, DataContext context)
		{
			this._powerReduceRatio = powerReduceRatio;
			base.SetModifiedAndInvalidateInfluencedCache(258, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				int dataSize = this._powerReduceRatio.GetSerializedSize();
				byte* pData = OperationAdder.DynamicObjectCollection_SetDynamicField<int>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this._id, 257, dataSize);
				pData += this._powerReduceRatio.Serialize(pData);
			}
		}

		// Token: 0x060027D9 RID: 10201 RVA: 0x001DB728 File Offset: 0x001D9928
		public SpecialEffectList GetPoisonAffectProduceValue()
		{
			return this._poisonAffectProduceValue;
		}

		// Token: 0x060027DA RID: 10202 RVA: 0x001DB740 File Offset: 0x001D9940
		public unsafe void SetPoisonAffectProduceValue(SpecialEffectList poisonAffectProduceValue, DataContext context)
		{
			this._poisonAffectProduceValue = poisonAffectProduceValue;
			base.SetModifiedAndInvalidateInfluencedCache(259, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				int dataSize = this._poisonAffectProduceValue.GetSerializedSize();
				byte* pData = OperationAdder.DynamicObjectCollection_SetDynamicField<int>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this._id, 258, dataSize);
				pData += this._poisonAffectProduceValue.Serialize(pData);
			}
		}

		// Token: 0x060027DB RID: 10203 RVA: 0x001DB7B8 File Offset: 0x001D99B8
		public SpecialEffectList GetCanReadingOnMonthChange()
		{
			return this._canReadingOnMonthChange;
		}

		// Token: 0x060027DC RID: 10204 RVA: 0x001DB7D0 File Offset: 0x001D99D0
		public unsafe void SetCanReadingOnMonthChange(SpecialEffectList canReadingOnMonthChange, DataContext context)
		{
			this._canReadingOnMonthChange = canReadingOnMonthChange;
			base.SetModifiedAndInvalidateInfluencedCache(260, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				int dataSize = this._canReadingOnMonthChange.GetSerializedSize();
				byte* pData = OperationAdder.DynamicObjectCollection_SetDynamicField<int>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this._id, 259, dataSize);
				pData += this._canReadingOnMonthChange.Serialize(pData);
			}
		}

		// Token: 0x060027DD RID: 10205 RVA: 0x001DB848 File Offset: 0x001D9A48
		public SpecialEffectList GetMedicineEffect()
		{
			return this._medicineEffect;
		}

		// Token: 0x060027DE RID: 10206 RVA: 0x001DB860 File Offset: 0x001D9A60
		public unsafe void SetMedicineEffect(SpecialEffectList medicineEffect, DataContext context)
		{
			this._medicineEffect = medicineEffect;
			base.SetModifiedAndInvalidateInfluencedCache(261, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				int dataSize = this._medicineEffect.GetSerializedSize();
				byte* pData = OperationAdder.DynamicObjectCollection_SetDynamicField<int>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this._id, 260, dataSize);
				pData += this._medicineEffect.Serialize(pData);
			}
		}

		// Token: 0x060027DF RID: 10207 RVA: 0x001DB8D8 File Offset: 0x001D9AD8
		public SpecialEffectList GetXiangshuInfectionDelta()
		{
			return this._xiangshuInfectionDelta;
		}

		// Token: 0x060027E0 RID: 10208 RVA: 0x001DB8F0 File Offset: 0x001D9AF0
		public unsafe void SetXiangshuInfectionDelta(SpecialEffectList xiangshuInfectionDelta, DataContext context)
		{
			this._xiangshuInfectionDelta = xiangshuInfectionDelta;
			base.SetModifiedAndInvalidateInfluencedCache(262, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				int dataSize = this._xiangshuInfectionDelta.GetSerializedSize();
				byte* pData = OperationAdder.DynamicObjectCollection_SetDynamicField<int>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this._id, 261, dataSize);
				pData += this._xiangshuInfectionDelta.Serialize(pData);
			}
		}

		// Token: 0x060027E1 RID: 10209 RVA: 0x001DB968 File Offset: 0x001D9B68
		public SpecialEffectList GetHealthDelta()
		{
			return this._healthDelta;
		}

		// Token: 0x060027E2 RID: 10210 RVA: 0x001DB980 File Offset: 0x001D9B80
		public unsafe void SetHealthDelta(SpecialEffectList healthDelta, DataContext context)
		{
			this._healthDelta = healthDelta;
			base.SetModifiedAndInvalidateInfluencedCache(263, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				int dataSize = this._healthDelta.GetSerializedSize();
				byte* pData = OperationAdder.DynamicObjectCollection_SetDynamicField<int>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this._id, 262, dataSize);
				pData += this._healthDelta.Serialize(pData);
			}
		}

		// Token: 0x060027E3 RID: 10211 RVA: 0x001DB9F8 File Offset: 0x001D9BF8
		public SpecialEffectList GetWeaponSilenceFrame()
		{
			return this._weaponSilenceFrame;
		}

		// Token: 0x060027E4 RID: 10212 RVA: 0x001DBA10 File Offset: 0x001D9C10
		public unsafe void SetWeaponSilenceFrame(SpecialEffectList weaponSilenceFrame, DataContext context)
		{
			this._weaponSilenceFrame = weaponSilenceFrame;
			base.SetModifiedAndInvalidateInfluencedCache(264, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				int dataSize = this._weaponSilenceFrame.GetSerializedSize();
				byte* pData = OperationAdder.DynamicObjectCollection_SetDynamicField<int>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this._id, 263, dataSize);
				pData += this._weaponSilenceFrame.Serialize(pData);
			}
		}

		// Token: 0x060027E5 RID: 10213 RVA: 0x001DBA88 File Offset: 0x001D9C88
		public SpecialEffectList GetSilenceFrame()
		{
			return this._silenceFrame;
		}

		// Token: 0x060027E6 RID: 10214 RVA: 0x001DBAA0 File Offset: 0x001D9CA0
		public unsafe void SetSilenceFrame(SpecialEffectList silenceFrame, DataContext context)
		{
			this._silenceFrame = silenceFrame;
			base.SetModifiedAndInvalidateInfluencedCache(265, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				int dataSize = this._silenceFrame.GetSerializedSize();
				byte* pData = OperationAdder.DynamicObjectCollection_SetDynamicField<int>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this._id, 264, dataSize);
				pData += this._silenceFrame.Serialize(pData);
			}
		}

		// Token: 0x060027E7 RID: 10215 RVA: 0x001DBB18 File Offset: 0x001D9D18
		public SpecialEffectList GetCurrAgeDelta()
		{
			return this._currAgeDelta;
		}

		// Token: 0x060027E8 RID: 10216 RVA: 0x001DBB30 File Offset: 0x001D9D30
		public unsafe void SetCurrAgeDelta(SpecialEffectList currAgeDelta, DataContext context)
		{
			this._currAgeDelta = currAgeDelta;
			base.SetModifiedAndInvalidateInfluencedCache(266, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				int dataSize = this._currAgeDelta.GetSerializedSize();
				byte* pData = OperationAdder.DynamicObjectCollection_SetDynamicField<int>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this._id, 265, dataSize);
				pData += this._currAgeDelta.Serialize(pData);
			}
		}

		// Token: 0x060027E9 RID: 10217 RVA: 0x001DBBA8 File Offset: 0x001D9DA8
		public SpecialEffectList GetGoneMadInAllBreak()
		{
			return this._goneMadInAllBreak;
		}

		// Token: 0x060027EA RID: 10218 RVA: 0x001DBBC0 File Offset: 0x001D9DC0
		public unsafe void SetGoneMadInAllBreak(SpecialEffectList goneMadInAllBreak, DataContext context)
		{
			this._goneMadInAllBreak = goneMadInAllBreak;
			base.SetModifiedAndInvalidateInfluencedCache(267, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				int dataSize = this._goneMadInAllBreak.GetSerializedSize();
				byte* pData = OperationAdder.DynamicObjectCollection_SetDynamicField<int>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this._id, 266, dataSize);
				pData += this._goneMadInAllBreak.Serialize(pData);
			}
		}

		// Token: 0x060027EB RID: 10219 RVA: 0x001DBC38 File Offset: 0x001D9E38
		public SpecialEffectList GetMakeLoveRateOnMonthChange()
		{
			return this._makeLoveRateOnMonthChange;
		}

		// Token: 0x060027EC RID: 10220 RVA: 0x001DBC50 File Offset: 0x001D9E50
		public unsafe void SetMakeLoveRateOnMonthChange(SpecialEffectList makeLoveRateOnMonthChange, DataContext context)
		{
			this._makeLoveRateOnMonthChange = makeLoveRateOnMonthChange;
			base.SetModifiedAndInvalidateInfluencedCache(268, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				int dataSize = this._makeLoveRateOnMonthChange.GetSerializedSize();
				byte* pData = OperationAdder.DynamicObjectCollection_SetDynamicField<int>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this._id, 267, dataSize);
				pData += this._makeLoveRateOnMonthChange.Serialize(pData);
			}
		}

		// Token: 0x060027ED RID: 10221 RVA: 0x001DBCC8 File Offset: 0x001D9EC8
		public SpecialEffectList GetCanAutoHealOnMonthChange()
		{
			return this._canAutoHealOnMonthChange;
		}

		// Token: 0x060027EE RID: 10222 RVA: 0x001DBCE0 File Offset: 0x001D9EE0
		public unsafe void SetCanAutoHealOnMonthChange(SpecialEffectList canAutoHealOnMonthChange, DataContext context)
		{
			this._canAutoHealOnMonthChange = canAutoHealOnMonthChange;
			base.SetModifiedAndInvalidateInfluencedCache(269, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				int dataSize = this._canAutoHealOnMonthChange.GetSerializedSize();
				byte* pData = OperationAdder.DynamicObjectCollection_SetDynamicField<int>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this._id, 268, dataSize);
				pData += this._canAutoHealOnMonthChange.Serialize(pData);
			}
		}

		// Token: 0x060027EF RID: 10223 RVA: 0x001DBD58 File Offset: 0x001D9F58
		public SpecialEffectList GetHappinessDelta()
		{
			return this._happinessDelta;
		}

		// Token: 0x060027F0 RID: 10224 RVA: 0x001DBD70 File Offset: 0x001D9F70
		public unsafe void SetHappinessDelta(SpecialEffectList happinessDelta, DataContext context)
		{
			this._happinessDelta = happinessDelta;
			base.SetModifiedAndInvalidateInfluencedCache(270, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				int dataSize = this._happinessDelta.GetSerializedSize();
				byte* pData = OperationAdder.DynamicObjectCollection_SetDynamicField<int>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this._id, 269, dataSize);
				pData += this._happinessDelta.Serialize(pData);
			}
		}

		// Token: 0x060027F1 RID: 10225 RVA: 0x001DBDE8 File Offset: 0x001D9FE8
		public SpecialEffectList GetTeammateCmdCanUse()
		{
			return this._teammateCmdCanUse;
		}

		// Token: 0x060027F2 RID: 10226 RVA: 0x001DBE00 File Offset: 0x001DA000
		public unsafe void SetTeammateCmdCanUse(SpecialEffectList teammateCmdCanUse, DataContext context)
		{
			this._teammateCmdCanUse = teammateCmdCanUse;
			base.SetModifiedAndInvalidateInfluencedCache(271, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				int dataSize = this._teammateCmdCanUse.GetSerializedSize();
				byte* pData = OperationAdder.DynamicObjectCollection_SetDynamicField<int>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this._id, 270, dataSize);
				pData += this._teammateCmdCanUse.Serialize(pData);
			}
		}

		// Token: 0x060027F3 RID: 10227 RVA: 0x001DBE78 File Offset: 0x001DA078
		public SpecialEffectList GetMixPoisonInfinityAffect()
		{
			return this._mixPoisonInfinityAffect;
		}

		// Token: 0x060027F4 RID: 10228 RVA: 0x001DBE90 File Offset: 0x001DA090
		public unsafe void SetMixPoisonInfinityAffect(SpecialEffectList mixPoisonInfinityAffect, DataContext context)
		{
			this._mixPoisonInfinityAffect = mixPoisonInfinityAffect;
			base.SetModifiedAndInvalidateInfluencedCache(272, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				int dataSize = this._mixPoisonInfinityAffect.GetSerializedSize();
				byte* pData = OperationAdder.DynamicObjectCollection_SetDynamicField<int>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this._id, 271, dataSize);
				pData += this._mixPoisonInfinityAffect.Serialize(pData);
			}
		}

		// Token: 0x060027F5 RID: 10229 RVA: 0x001DBF08 File Offset: 0x001DA108
		public SpecialEffectList GetAttackRangeMaxAcupoint()
		{
			return this._attackRangeMaxAcupoint;
		}

		// Token: 0x060027F6 RID: 10230 RVA: 0x001DBF20 File Offset: 0x001DA120
		public unsafe void SetAttackRangeMaxAcupoint(SpecialEffectList attackRangeMaxAcupoint, DataContext context)
		{
			this._attackRangeMaxAcupoint = attackRangeMaxAcupoint;
			base.SetModifiedAndInvalidateInfluencedCache(273, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				int dataSize = this._attackRangeMaxAcupoint.GetSerializedSize();
				byte* pData = OperationAdder.DynamicObjectCollection_SetDynamicField<int>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this._id, 272, dataSize);
				pData += this._attackRangeMaxAcupoint.Serialize(pData);
			}
		}

		// Token: 0x060027F7 RID: 10231 RVA: 0x001DBF98 File Offset: 0x001DA198
		public SpecialEffectList GetMaxMobilityPercent()
		{
			return this._maxMobilityPercent;
		}

		// Token: 0x060027F8 RID: 10232 RVA: 0x001DBFB0 File Offset: 0x001DA1B0
		public unsafe void SetMaxMobilityPercent(SpecialEffectList maxMobilityPercent, DataContext context)
		{
			this._maxMobilityPercent = maxMobilityPercent;
			base.SetModifiedAndInvalidateInfluencedCache(274, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				int dataSize = this._maxMobilityPercent.GetSerializedSize();
				byte* pData = OperationAdder.DynamicObjectCollection_SetDynamicField<int>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this._id, 273, dataSize);
				pData += this._maxMobilityPercent.Serialize(pData);
			}
		}

		// Token: 0x060027F9 RID: 10233 RVA: 0x001DC028 File Offset: 0x001DA228
		public SpecialEffectList GetMakeMindDamage()
		{
			return this._makeMindDamage;
		}

		// Token: 0x060027FA RID: 10234 RVA: 0x001DC040 File Offset: 0x001DA240
		public unsafe void SetMakeMindDamage(SpecialEffectList makeMindDamage, DataContext context)
		{
			this._makeMindDamage = makeMindDamage;
			base.SetModifiedAndInvalidateInfluencedCache(275, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				int dataSize = this._makeMindDamage.GetSerializedSize();
				byte* pData = OperationAdder.DynamicObjectCollection_SetDynamicField<int>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this._id, 274, dataSize);
				pData += this._makeMindDamage.Serialize(pData);
			}
		}

		// Token: 0x060027FB RID: 10235 RVA: 0x001DC0B8 File Offset: 0x001DA2B8
		public SpecialEffectList GetAcceptMindDamage()
		{
			return this._acceptMindDamage;
		}

		// Token: 0x060027FC RID: 10236 RVA: 0x001DC0D0 File Offset: 0x001DA2D0
		public unsafe void SetAcceptMindDamage(SpecialEffectList acceptMindDamage, DataContext context)
		{
			this._acceptMindDamage = acceptMindDamage;
			base.SetModifiedAndInvalidateInfluencedCache(276, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				int dataSize = this._acceptMindDamage.GetSerializedSize();
				byte* pData = OperationAdder.DynamicObjectCollection_SetDynamicField<int>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this._id, 275, dataSize);
				pData += this._acceptMindDamage.Serialize(pData);
			}
		}

		// Token: 0x060027FD RID: 10237 RVA: 0x001DC148 File Offset: 0x001DA348
		public SpecialEffectList GetHitAddByTempValue()
		{
			return this._hitAddByTempValue;
		}

		// Token: 0x060027FE RID: 10238 RVA: 0x001DC160 File Offset: 0x001DA360
		public unsafe void SetHitAddByTempValue(SpecialEffectList hitAddByTempValue, DataContext context)
		{
			this._hitAddByTempValue = hitAddByTempValue;
			base.SetModifiedAndInvalidateInfluencedCache(277, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				int dataSize = this._hitAddByTempValue.GetSerializedSize();
				byte* pData = OperationAdder.DynamicObjectCollection_SetDynamicField<int>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this._id, 276, dataSize);
				pData += this._hitAddByTempValue.Serialize(pData);
			}
		}

		// Token: 0x060027FF RID: 10239 RVA: 0x001DC1D8 File Offset: 0x001DA3D8
		public SpecialEffectList GetAvoidAddByTempValue()
		{
			return this._avoidAddByTempValue;
		}

		// Token: 0x06002800 RID: 10240 RVA: 0x001DC1F0 File Offset: 0x001DA3F0
		public unsafe void SetAvoidAddByTempValue(SpecialEffectList avoidAddByTempValue, DataContext context)
		{
			this._avoidAddByTempValue = avoidAddByTempValue;
			base.SetModifiedAndInvalidateInfluencedCache(278, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				int dataSize = this._avoidAddByTempValue.GetSerializedSize();
				byte* pData = OperationAdder.DynamicObjectCollection_SetDynamicField<int>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this._id, 277, dataSize);
				pData += this._avoidAddByTempValue.Serialize(pData);
			}
		}

		// Token: 0x06002801 RID: 10241 RVA: 0x001DC268 File Offset: 0x001DA468
		public SpecialEffectList GetIgnoreEquipmentOverload()
		{
			return this._ignoreEquipmentOverload;
		}

		// Token: 0x06002802 RID: 10242 RVA: 0x001DC280 File Offset: 0x001DA480
		public unsafe void SetIgnoreEquipmentOverload(SpecialEffectList ignoreEquipmentOverload, DataContext context)
		{
			this._ignoreEquipmentOverload = ignoreEquipmentOverload;
			base.SetModifiedAndInvalidateInfluencedCache(279, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				int dataSize = this._ignoreEquipmentOverload.GetSerializedSize();
				byte* pData = OperationAdder.DynamicObjectCollection_SetDynamicField<int>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this._id, 278, dataSize);
				pData += this._ignoreEquipmentOverload.Serialize(pData);
			}
		}

		// Token: 0x06002803 RID: 10243 RVA: 0x001DC2F8 File Offset: 0x001DA4F8
		public SpecialEffectList GetCanCostEnemyUsableTricks()
		{
			return this._canCostEnemyUsableTricks;
		}

		// Token: 0x06002804 RID: 10244 RVA: 0x001DC310 File Offset: 0x001DA510
		public unsafe void SetCanCostEnemyUsableTricks(SpecialEffectList canCostEnemyUsableTricks, DataContext context)
		{
			this._canCostEnemyUsableTricks = canCostEnemyUsableTricks;
			base.SetModifiedAndInvalidateInfluencedCache(280, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				int dataSize = this._canCostEnemyUsableTricks.GetSerializedSize();
				byte* pData = OperationAdder.DynamicObjectCollection_SetDynamicField<int>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this._id, 279, dataSize);
				pData += this._canCostEnemyUsableTricks.Serialize(pData);
			}
		}

		// Token: 0x06002805 RID: 10245 RVA: 0x001DC388 File Offset: 0x001DA588
		public SpecialEffectList GetIgnoreArmor()
		{
			return this._ignoreArmor;
		}

		// Token: 0x06002806 RID: 10246 RVA: 0x001DC3A0 File Offset: 0x001DA5A0
		public unsafe void SetIgnoreArmor(SpecialEffectList ignoreArmor, DataContext context)
		{
			this._ignoreArmor = ignoreArmor;
			base.SetModifiedAndInvalidateInfluencedCache(281, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				int dataSize = this._ignoreArmor.GetSerializedSize();
				byte* pData = OperationAdder.DynamicObjectCollection_SetDynamicField<int>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this._id, 280, dataSize);
				pData += this._ignoreArmor.Serialize(pData);
			}
		}

		// Token: 0x06002807 RID: 10247 RVA: 0x001DC418 File Offset: 0x001DA618
		public SpecialEffectList GetUnyieldingFallen()
		{
			return this._unyieldingFallen;
		}

		// Token: 0x06002808 RID: 10248 RVA: 0x001DC430 File Offset: 0x001DA630
		public unsafe void SetUnyieldingFallen(SpecialEffectList unyieldingFallen, DataContext context)
		{
			this._unyieldingFallen = unyieldingFallen;
			base.SetModifiedAndInvalidateInfluencedCache(282, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				int dataSize = this._unyieldingFallen.GetSerializedSize();
				byte* pData = OperationAdder.DynamicObjectCollection_SetDynamicField<int>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this._id, 281, dataSize);
				pData += this._unyieldingFallen.Serialize(pData);
			}
		}

		// Token: 0x06002809 RID: 10249 RVA: 0x001DC4A8 File Offset: 0x001DA6A8
		public SpecialEffectList GetNormalAttackPrepareFrame()
		{
			return this._normalAttackPrepareFrame;
		}

		// Token: 0x0600280A RID: 10250 RVA: 0x001DC4C0 File Offset: 0x001DA6C0
		public unsafe void SetNormalAttackPrepareFrame(SpecialEffectList normalAttackPrepareFrame, DataContext context)
		{
			this._normalAttackPrepareFrame = normalAttackPrepareFrame;
			base.SetModifiedAndInvalidateInfluencedCache(283, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				int dataSize = this._normalAttackPrepareFrame.GetSerializedSize();
				byte* pData = OperationAdder.DynamicObjectCollection_SetDynamicField<int>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this._id, 282, dataSize);
				pData += this._normalAttackPrepareFrame.Serialize(pData);
			}
		}

		// Token: 0x0600280B RID: 10251 RVA: 0x001DC538 File Offset: 0x001DA738
		public SpecialEffectList GetCanCostUselessTricks()
		{
			return this._canCostUselessTricks;
		}

		// Token: 0x0600280C RID: 10252 RVA: 0x001DC550 File Offset: 0x001DA750
		public unsafe void SetCanCostUselessTricks(SpecialEffectList canCostUselessTricks, DataContext context)
		{
			this._canCostUselessTricks = canCostUselessTricks;
			base.SetModifiedAndInvalidateInfluencedCache(284, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				int dataSize = this._canCostUselessTricks.GetSerializedSize();
				byte* pData = OperationAdder.DynamicObjectCollection_SetDynamicField<int>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this._id, 283, dataSize);
				pData += this._canCostUselessTricks.Serialize(pData);
			}
		}

		// Token: 0x0600280D RID: 10253 RVA: 0x001DC5C8 File Offset: 0x001DA7C8
		public SpecialEffectList GetDefendSkillCanAffect()
		{
			return this._defendSkillCanAffect;
		}

		// Token: 0x0600280E RID: 10254 RVA: 0x001DC5E0 File Offset: 0x001DA7E0
		public unsafe void SetDefendSkillCanAffect(SpecialEffectList defendSkillCanAffect, DataContext context)
		{
			this._defendSkillCanAffect = defendSkillCanAffect;
			base.SetModifiedAndInvalidateInfluencedCache(285, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				int dataSize = this._defendSkillCanAffect.GetSerializedSize();
				byte* pData = OperationAdder.DynamicObjectCollection_SetDynamicField<int>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this._id, 284, dataSize);
				pData += this._defendSkillCanAffect.Serialize(pData);
			}
		}

		// Token: 0x0600280F RID: 10255 RVA: 0x001DC658 File Offset: 0x001DA858
		public SpecialEffectList GetAssistSkillCanAffect()
		{
			return this._assistSkillCanAffect;
		}

		// Token: 0x06002810 RID: 10256 RVA: 0x001DC670 File Offset: 0x001DA870
		public unsafe void SetAssistSkillCanAffect(SpecialEffectList assistSkillCanAffect, DataContext context)
		{
			this._assistSkillCanAffect = assistSkillCanAffect;
			base.SetModifiedAndInvalidateInfluencedCache(286, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				int dataSize = this._assistSkillCanAffect.GetSerializedSize();
				byte* pData = OperationAdder.DynamicObjectCollection_SetDynamicField<int>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this._id, 285, dataSize);
				pData += this._assistSkillCanAffect.Serialize(pData);
			}
		}

		// Token: 0x06002811 RID: 10257 RVA: 0x001DC6E8 File Offset: 0x001DA8E8
		public SpecialEffectList GetAgileSkillCanAffect()
		{
			return this._agileSkillCanAffect;
		}

		// Token: 0x06002812 RID: 10258 RVA: 0x001DC700 File Offset: 0x001DA900
		public unsafe void SetAgileSkillCanAffect(SpecialEffectList agileSkillCanAffect, DataContext context)
		{
			this._agileSkillCanAffect = agileSkillCanAffect;
			base.SetModifiedAndInvalidateInfluencedCache(287, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				int dataSize = this._agileSkillCanAffect.GetSerializedSize();
				byte* pData = OperationAdder.DynamicObjectCollection_SetDynamicField<int>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this._id, 286, dataSize);
				pData += this._agileSkillCanAffect.Serialize(pData);
			}
		}

		// Token: 0x06002813 RID: 10259 RVA: 0x001DC778 File Offset: 0x001DA978
		public SpecialEffectList GetAllMarkChangeToMind()
		{
			return this._allMarkChangeToMind;
		}

		// Token: 0x06002814 RID: 10260 RVA: 0x001DC790 File Offset: 0x001DA990
		public unsafe void SetAllMarkChangeToMind(SpecialEffectList allMarkChangeToMind, DataContext context)
		{
			this._allMarkChangeToMind = allMarkChangeToMind;
			base.SetModifiedAndInvalidateInfluencedCache(288, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				int dataSize = this._allMarkChangeToMind.GetSerializedSize();
				byte* pData = OperationAdder.DynamicObjectCollection_SetDynamicField<int>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this._id, 287, dataSize);
				pData += this._allMarkChangeToMind.Serialize(pData);
			}
		}

		// Token: 0x06002815 RID: 10261 RVA: 0x001DC808 File Offset: 0x001DAA08
		public SpecialEffectList GetMindMarkChangeToFatal()
		{
			return this._mindMarkChangeToFatal;
		}

		// Token: 0x06002816 RID: 10262 RVA: 0x001DC820 File Offset: 0x001DAA20
		public unsafe void SetMindMarkChangeToFatal(SpecialEffectList mindMarkChangeToFatal, DataContext context)
		{
			this._mindMarkChangeToFatal = mindMarkChangeToFatal;
			base.SetModifiedAndInvalidateInfluencedCache(289, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				int dataSize = this._mindMarkChangeToFatal.GetSerializedSize();
				byte* pData = OperationAdder.DynamicObjectCollection_SetDynamicField<int>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this._id, 288, dataSize);
				pData += this._mindMarkChangeToFatal.Serialize(pData);
			}
		}

		// Token: 0x06002817 RID: 10263 RVA: 0x001DC898 File Offset: 0x001DAA98
		public SpecialEffectList GetCanCast()
		{
			return this._canCast;
		}

		// Token: 0x06002818 RID: 10264 RVA: 0x001DC8B0 File Offset: 0x001DAAB0
		public unsafe void SetCanCast(SpecialEffectList canCast, DataContext context)
		{
			this._canCast = canCast;
			base.SetModifiedAndInvalidateInfluencedCache(290, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				int dataSize = this._canCast.GetSerializedSize();
				byte* pData = OperationAdder.DynamicObjectCollection_SetDynamicField<int>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this._id, 289, dataSize);
				pData += this._canCast.Serialize(pData);
			}
		}

		// Token: 0x06002819 RID: 10265 RVA: 0x001DC928 File Offset: 0x001DAB28
		public SpecialEffectList GetInevitableAvoid()
		{
			return this._inevitableAvoid;
		}

		// Token: 0x0600281A RID: 10266 RVA: 0x001DC940 File Offset: 0x001DAB40
		public unsafe void SetInevitableAvoid(SpecialEffectList inevitableAvoid, DataContext context)
		{
			this._inevitableAvoid = inevitableAvoid;
			base.SetModifiedAndInvalidateInfluencedCache(291, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				int dataSize = this._inevitableAvoid.GetSerializedSize();
				byte* pData = OperationAdder.DynamicObjectCollection_SetDynamicField<int>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this._id, 290, dataSize);
				pData += this._inevitableAvoid.Serialize(pData);
			}
		}

		// Token: 0x0600281B RID: 10267 RVA: 0x001DC9B8 File Offset: 0x001DABB8
		public SpecialEffectList GetPowerEffectReverse()
		{
			return this._powerEffectReverse;
		}

		// Token: 0x0600281C RID: 10268 RVA: 0x001DC9D0 File Offset: 0x001DABD0
		public unsafe void SetPowerEffectReverse(SpecialEffectList powerEffectReverse, DataContext context)
		{
			this._powerEffectReverse = powerEffectReverse;
			base.SetModifiedAndInvalidateInfluencedCache(292, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				int dataSize = this._powerEffectReverse.GetSerializedSize();
				byte* pData = OperationAdder.DynamicObjectCollection_SetDynamicField<int>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this._id, 291, dataSize);
				pData += this._powerEffectReverse.Serialize(pData);
			}
		}

		// Token: 0x0600281D RID: 10269 RVA: 0x001DCA48 File Offset: 0x001DAC48
		public SpecialEffectList GetFeatureBonusReverse()
		{
			return this._featureBonusReverse;
		}

		// Token: 0x0600281E RID: 10270 RVA: 0x001DCA60 File Offset: 0x001DAC60
		public unsafe void SetFeatureBonusReverse(SpecialEffectList featureBonusReverse, DataContext context)
		{
			this._featureBonusReverse = featureBonusReverse;
			base.SetModifiedAndInvalidateInfluencedCache(293, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				int dataSize = this._featureBonusReverse.GetSerializedSize();
				byte* pData = OperationAdder.DynamicObjectCollection_SetDynamicField<int>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this._id, 292, dataSize);
				pData += this._featureBonusReverse.Serialize(pData);
			}
		}

		// Token: 0x0600281F RID: 10271 RVA: 0x001DCAD8 File Offset: 0x001DACD8
		public SpecialEffectList GetWugFatalDamageValue()
		{
			return this._wugFatalDamageValue;
		}

		// Token: 0x06002820 RID: 10272 RVA: 0x001DCAF0 File Offset: 0x001DACF0
		public unsafe void SetWugFatalDamageValue(SpecialEffectList wugFatalDamageValue, DataContext context)
		{
			this._wugFatalDamageValue = wugFatalDamageValue;
			base.SetModifiedAndInvalidateInfluencedCache(294, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				int dataSize = this._wugFatalDamageValue.GetSerializedSize();
				byte* pData = OperationAdder.DynamicObjectCollection_SetDynamicField<int>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this._id, 293, dataSize);
				pData += this._wugFatalDamageValue.Serialize(pData);
			}
		}

		// Token: 0x06002821 RID: 10273 RVA: 0x001DCB68 File Offset: 0x001DAD68
		public SpecialEffectList GetCanRecoverHealthOnMonthChange()
		{
			return this._canRecoverHealthOnMonthChange;
		}

		// Token: 0x06002822 RID: 10274 RVA: 0x001DCB80 File Offset: 0x001DAD80
		public unsafe void SetCanRecoverHealthOnMonthChange(SpecialEffectList canRecoverHealthOnMonthChange, DataContext context)
		{
			this._canRecoverHealthOnMonthChange = canRecoverHealthOnMonthChange;
			base.SetModifiedAndInvalidateInfluencedCache(295, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				int dataSize = this._canRecoverHealthOnMonthChange.GetSerializedSize();
				byte* pData = OperationAdder.DynamicObjectCollection_SetDynamicField<int>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this._id, 294, dataSize);
				pData += this._canRecoverHealthOnMonthChange.Serialize(pData);
			}
		}

		// Token: 0x06002823 RID: 10275 RVA: 0x001DCBF8 File Offset: 0x001DADF8
		public SpecialEffectList GetTakeRevengeRateOnMonthChange()
		{
			return this._takeRevengeRateOnMonthChange;
		}

		// Token: 0x06002824 RID: 10276 RVA: 0x001DCC10 File Offset: 0x001DAE10
		public unsafe void SetTakeRevengeRateOnMonthChange(SpecialEffectList takeRevengeRateOnMonthChange, DataContext context)
		{
			this._takeRevengeRateOnMonthChange = takeRevengeRateOnMonthChange;
			base.SetModifiedAndInvalidateInfluencedCache(296, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				int dataSize = this._takeRevengeRateOnMonthChange.GetSerializedSize();
				byte* pData = OperationAdder.DynamicObjectCollection_SetDynamicField<int>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this._id, 295, dataSize);
				pData += this._takeRevengeRateOnMonthChange.Serialize(pData);
			}
		}

		// Token: 0x06002825 RID: 10277 RVA: 0x001DCC88 File Offset: 0x001DAE88
		public SpecialEffectList GetConsummateLevelBonus()
		{
			return this._consummateLevelBonus;
		}

		// Token: 0x06002826 RID: 10278 RVA: 0x001DCCA0 File Offset: 0x001DAEA0
		public unsafe void SetConsummateLevelBonus(SpecialEffectList consummateLevelBonus, DataContext context)
		{
			this._consummateLevelBonus = consummateLevelBonus;
			base.SetModifiedAndInvalidateInfluencedCache(297, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				int dataSize = this._consummateLevelBonus.GetSerializedSize();
				byte* pData = OperationAdder.DynamicObjectCollection_SetDynamicField<int>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this._id, 296, dataSize);
				pData += this._consummateLevelBonus.Serialize(pData);
			}
		}

		// Token: 0x06002827 RID: 10279 RVA: 0x001DCD18 File Offset: 0x001DAF18
		public SpecialEffectList GetNeiliDelta()
		{
			return this._neiliDelta;
		}

		// Token: 0x06002828 RID: 10280 RVA: 0x001DCD30 File Offset: 0x001DAF30
		public unsafe void SetNeiliDelta(SpecialEffectList neiliDelta, DataContext context)
		{
			this._neiliDelta = neiliDelta;
			base.SetModifiedAndInvalidateInfluencedCache(298, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				int dataSize = this._neiliDelta.GetSerializedSize();
				byte* pData = OperationAdder.DynamicObjectCollection_SetDynamicField<int>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this._id, 297, dataSize);
				pData += this._neiliDelta.Serialize(pData);
			}
		}

		// Token: 0x06002829 RID: 10281 RVA: 0x001DCDA8 File Offset: 0x001DAFA8
		public SpecialEffectList GetCanMakeLoveSpecialOnMonthChange()
		{
			return this._canMakeLoveSpecialOnMonthChange;
		}

		// Token: 0x0600282A RID: 10282 RVA: 0x001DCDC0 File Offset: 0x001DAFC0
		public unsafe void SetCanMakeLoveSpecialOnMonthChange(SpecialEffectList canMakeLoveSpecialOnMonthChange, DataContext context)
		{
			this._canMakeLoveSpecialOnMonthChange = canMakeLoveSpecialOnMonthChange;
			base.SetModifiedAndInvalidateInfluencedCache(299, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				int dataSize = this._canMakeLoveSpecialOnMonthChange.GetSerializedSize();
				byte* pData = OperationAdder.DynamicObjectCollection_SetDynamicField<int>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this._id, 298, dataSize);
				pData += this._canMakeLoveSpecialOnMonthChange.Serialize(pData);
			}
		}

		// Token: 0x0600282B RID: 10283 RVA: 0x001DCE38 File Offset: 0x001DB038
		public SpecialEffectList GetHealAcupointSpeed()
		{
			return this._healAcupointSpeed;
		}

		// Token: 0x0600282C RID: 10284 RVA: 0x001DCE50 File Offset: 0x001DB050
		public unsafe void SetHealAcupointSpeed(SpecialEffectList healAcupointSpeed, DataContext context)
		{
			this._healAcupointSpeed = healAcupointSpeed;
			base.SetModifiedAndInvalidateInfluencedCache(300, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				int dataSize = this._healAcupointSpeed.GetSerializedSize();
				byte* pData = OperationAdder.DynamicObjectCollection_SetDynamicField<int>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this._id, 299, dataSize);
				pData += this._healAcupointSpeed.Serialize(pData);
			}
		}

		// Token: 0x0600282D RID: 10285 RVA: 0x001DCEC8 File Offset: 0x001DB0C8
		public SpecialEffectList GetMaxChangeTrickCount()
		{
			return this._maxChangeTrickCount;
		}

		// Token: 0x0600282E RID: 10286 RVA: 0x001DCEE0 File Offset: 0x001DB0E0
		public unsafe void SetMaxChangeTrickCount(SpecialEffectList maxChangeTrickCount, DataContext context)
		{
			this._maxChangeTrickCount = maxChangeTrickCount;
			base.SetModifiedAndInvalidateInfluencedCache(301, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				int dataSize = this._maxChangeTrickCount.GetSerializedSize();
				byte* pData = OperationAdder.DynamicObjectCollection_SetDynamicField<int>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this._id, 300, dataSize);
				pData += this._maxChangeTrickCount.Serialize(pData);
			}
		}

		// Token: 0x0600282F RID: 10287 RVA: 0x001DCF58 File Offset: 0x001DB158
		public SpecialEffectList GetConvertCostBreathAndStance()
		{
			return this._convertCostBreathAndStance;
		}

		// Token: 0x06002830 RID: 10288 RVA: 0x001DCF70 File Offset: 0x001DB170
		public unsafe void SetConvertCostBreathAndStance(SpecialEffectList convertCostBreathAndStance, DataContext context)
		{
			this._convertCostBreathAndStance = convertCostBreathAndStance;
			base.SetModifiedAndInvalidateInfluencedCache(302, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				int dataSize = this._convertCostBreathAndStance.GetSerializedSize();
				byte* pData = OperationAdder.DynamicObjectCollection_SetDynamicField<int>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this._id, 301, dataSize);
				pData += this._convertCostBreathAndStance.Serialize(pData);
			}
		}

		// Token: 0x06002831 RID: 10289 RVA: 0x001DCFE8 File Offset: 0x001DB1E8
		public SpecialEffectList GetPersonalitiesAll()
		{
			return this._personalitiesAll;
		}

		// Token: 0x06002832 RID: 10290 RVA: 0x001DD000 File Offset: 0x001DB200
		public unsafe void SetPersonalitiesAll(SpecialEffectList personalitiesAll, DataContext context)
		{
			this._personalitiesAll = personalitiesAll;
			base.SetModifiedAndInvalidateInfluencedCache(303, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				int dataSize = this._personalitiesAll.GetSerializedSize();
				byte* pData = OperationAdder.DynamicObjectCollection_SetDynamicField<int>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this._id, 302, dataSize);
				pData += this._personalitiesAll.Serialize(pData);
			}
		}

		// Token: 0x06002833 RID: 10291 RVA: 0x001DD078 File Offset: 0x001DB278
		public SpecialEffectList GetFinalFatalDamageMarkCount()
		{
			return this._finalFatalDamageMarkCount;
		}

		// Token: 0x06002834 RID: 10292 RVA: 0x001DD090 File Offset: 0x001DB290
		public unsafe void SetFinalFatalDamageMarkCount(SpecialEffectList finalFatalDamageMarkCount, DataContext context)
		{
			this._finalFatalDamageMarkCount = finalFatalDamageMarkCount;
			base.SetModifiedAndInvalidateInfluencedCache(304, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				int dataSize = this._finalFatalDamageMarkCount.GetSerializedSize();
				byte* pData = OperationAdder.DynamicObjectCollection_SetDynamicField<int>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this._id, 303, dataSize);
				pData += this._finalFatalDamageMarkCount.Serialize(pData);
			}
		}

		// Token: 0x06002835 RID: 10293 RVA: 0x001DD108 File Offset: 0x001DB308
		public SpecialEffectList GetInfinityMindMarkProgress()
		{
			return this._infinityMindMarkProgress;
		}

		// Token: 0x06002836 RID: 10294 RVA: 0x001DD120 File Offset: 0x001DB320
		public unsafe void SetInfinityMindMarkProgress(SpecialEffectList infinityMindMarkProgress, DataContext context)
		{
			this._infinityMindMarkProgress = infinityMindMarkProgress;
			base.SetModifiedAndInvalidateInfluencedCache(305, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				int dataSize = this._infinityMindMarkProgress.GetSerializedSize();
				byte* pData = OperationAdder.DynamicObjectCollection_SetDynamicField<int>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this._id, 304, dataSize);
				pData += this._infinityMindMarkProgress.Serialize(pData);
			}
		}

		// Token: 0x06002837 RID: 10295 RVA: 0x001DD198 File Offset: 0x001DB398
		public SpecialEffectList GetCombatSkillAiScorePower()
		{
			return this._combatSkillAiScorePower;
		}

		// Token: 0x06002838 RID: 10296 RVA: 0x001DD1B0 File Offset: 0x001DB3B0
		public unsafe void SetCombatSkillAiScorePower(SpecialEffectList combatSkillAiScorePower, DataContext context)
		{
			this._combatSkillAiScorePower = combatSkillAiScorePower;
			base.SetModifiedAndInvalidateInfluencedCache(306, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				int dataSize = this._combatSkillAiScorePower.GetSerializedSize();
				byte* pData = OperationAdder.DynamicObjectCollection_SetDynamicField<int>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this._id, 305, dataSize);
				pData += this._combatSkillAiScorePower.Serialize(pData);
			}
		}

		// Token: 0x06002839 RID: 10297 RVA: 0x001DD228 File Offset: 0x001DB428
		public SpecialEffectList GetNormalAttackChangeToUnlockAttack()
		{
			return this._normalAttackChangeToUnlockAttack;
		}

		// Token: 0x0600283A RID: 10298 RVA: 0x001DD240 File Offset: 0x001DB440
		public unsafe void SetNormalAttackChangeToUnlockAttack(SpecialEffectList normalAttackChangeToUnlockAttack, DataContext context)
		{
			this._normalAttackChangeToUnlockAttack = normalAttackChangeToUnlockAttack;
			base.SetModifiedAndInvalidateInfluencedCache(307, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				int dataSize = this._normalAttackChangeToUnlockAttack.GetSerializedSize();
				byte* pData = OperationAdder.DynamicObjectCollection_SetDynamicField<int>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this._id, 306, dataSize);
				pData += this._normalAttackChangeToUnlockAttack.Serialize(pData);
			}
		}

		// Token: 0x0600283B RID: 10299 RVA: 0x001DD2B8 File Offset: 0x001DB4B8
		public SpecialEffectList GetAttackBodyPartOdds()
		{
			return this._attackBodyPartOdds;
		}

		// Token: 0x0600283C RID: 10300 RVA: 0x001DD2D0 File Offset: 0x001DB4D0
		public unsafe void SetAttackBodyPartOdds(SpecialEffectList attackBodyPartOdds, DataContext context)
		{
			this._attackBodyPartOdds = attackBodyPartOdds;
			base.SetModifiedAndInvalidateInfluencedCache(308, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				int dataSize = this._attackBodyPartOdds.GetSerializedSize();
				byte* pData = OperationAdder.DynamicObjectCollection_SetDynamicField<int>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this._id, 307, dataSize);
				pData += this._attackBodyPartOdds.Serialize(pData);
			}
		}

		// Token: 0x0600283D RID: 10301 RVA: 0x001DD348 File Offset: 0x001DB548
		public SpecialEffectList GetChangeDurability()
		{
			return this._changeDurability;
		}

		// Token: 0x0600283E RID: 10302 RVA: 0x001DD360 File Offset: 0x001DB560
		public unsafe void SetChangeDurability(SpecialEffectList changeDurability, DataContext context)
		{
			this._changeDurability = changeDurability;
			base.SetModifiedAndInvalidateInfluencedCache(309, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				int dataSize = this._changeDurability.GetSerializedSize();
				byte* pData = OperationAdder.DynamicObjectCollection_SetDynamicField<int>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this._id, 308, dataSize);
				pData += this._changeDurability.Serialize(pData);
			}
		}

		// Token: 0x0600283F RID: 10303 RVA: 0x001DD3D8 File Offset: 0x001DB5D8
		public SpecialEffectList GetEquipmentBonus()
		{
			return this._equipmentBonus;
		}

		// Token: 0x06002840 RID: 10304 RVA: 0x001DD3F0 File Offset: 0x001DB5F0
		public unsafe void SetEquipmentBonus(SpecialEffectList equipmentBonus, DataContext context)
		{
			this._equipmentBonus = equipmentBonus;
			base.SetModifiedAndInvalidateInfluencedCache(310, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				int dataSize = this._equipmentBonus.GetSerializedSize();
				byte* pData = OperationAdder.DynamicObjectCollection_SetDynamicField<int>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this._id, 309, dataSize);
				pData += this._equipmentBonus.Serialize(pData);
			}
		}

		// Token: 0x06002841 RID: 10305 RVA: 0x001DD468 File Offset: 0x001DB668
		public SpecialEffectList GetEquipmentWeight()
		{
			return this._equipmentWeight;
		}

		// Token: 0x06002842 RID: 10306 RVA: 0x001DD480 File Offset: 0x001DB680
		public unsafe void SetEquipmentWeight(SpecialEffectList equipmentWeight, DataContext context)
		{
			this._equipmentWeight = equipmentWeight;
			base.SetModifiedAndInvalidateInfluencedCache(311, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				int dataSize = this._equipmentWeight.GetSerializedSize();
				byte* pData = OperationAdder.DynamicObjectCollection_SetDynamicField<int>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this._id, 310, dataSize);
				pData += this._equipmentWeight.Serialize(pData);
			}
		}

		// Token: 0x06002843 RID: 10307 RVA: 0x001DD4F8 File Offset: 0x001DB6F8
		public SpecialEffectList GetRawCreateEffectList()
		{
			return this._rawCreateEffectList;
		}

		// Token: 0x06002844 RID: 10308 RVA: 0x001DD510 File Offset: 0x001DB710
		public unsafe void SetRawCreateEffectList(SpecialEffectList rawCreateEffectList, DataContext context)
		{
			this._rawCreateEffectList = rawCreateEffectList;
			base.SetModifiedAndInvalidateInfluencedCache(312, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				int dataSize = this._rawCreateEffectList.GetSerializedSize();
				byte* pData = OperationAdder.DynamicObjectCollection_SetDynamicField<int>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this._id, 311, dataSize);
				pData += this._rawCreateEffectList.Serialize(pData);
			}
		}

		// Token: 0x06002845 RID: 10309 RVA: 0x001DD588 File Offset: 0x001DB788
		public SpecialEffectList GetJiTrickAsWeaponTrickCount()
		{
			return this._jiTrickAsWeaponTrickCount;
		}

		// Token: 0x06002846 RID: 10310 RVA: 0x001DD5A0 File Offset: 0x001DB7A0
		public unsafe void SetJiTrickAsWeaponTrickCount(SpecialEffectList jiTrickAsWeaponTrickCount, DataContext context)
		{
			this._jiTrickAsWeaponTrickCount = jiTrickAsWeaponTrickCount;
			base.SetModifiedAndInvalidateInfluencedCache(313, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				int dataSize = this._jiTrickAsWeaponTrickCount.GetSerializedSize();
				byte* pData = OperationAdder.DynamicObjectCollection_SetDynamicField<int>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this._id, 312, dataSize);
				pData += this._jiTrickAsWeaponTrickCount.Serialize(pData);
			}
		}

		// Token: 0x06002847 RID: 10311 RVA: 0x001DD618 File Offset: 0x001DB818
		public SpecialEffectList GetUselessTrickAsJiTrickCount()
		{
			return this._uselessTrickAsJiTrickCount;
		}

		// Token: 0x06002848 RID: 10312 RVA: 0x001DD630 File Offset: 0x001DB830
		public unsafe void SetUselessTrickAsJiTrickCount(SpecialEffectList uselessTrickAsJiTrickCount, DataContext context)
		{
			this._uselessTrickAsJiTrickCount = uselessTrickAsJiTrickCount;
			base.SetModifiedAndInvalidateInfluencedCache(314, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				int dataSize = this._uselessTrickAsJiTrickCount.GetSerializedSize();
				byte* pData = OperationAdder.DynamicObjectCollection_SetDynamicField<int>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this._id, 313, dataSize);
				pData += this._uselessTrickAsJiTrickCount.Serialize(pData);
			}
		}

		// Token: 0x06002849 RID: 10313 RVA: 0x001DD6A8 File Offset: 0x001DB8A8
		public SpecialEffectList GetEquipmentPower()
		{
			return this._equipmentPower;
		}

		// Token: 0x0600284A RID: 10314 RVA: 0x001DD6C0 File Offset: 0x001DB8C0
		public unsafe void SetEquipmentPower(SpecialEffectList equipmentPower, DataContext context)
		{
			this._equipmentPower = equipmentPower;
			base.SetModifiedAndInvalidateInfluencedCache(315, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				int dataSize = this._equipmentPower.GetSerializedSize();
				byte* pData = OperationAdder.DynamicObjectCollection_SetDynamicField<int>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this._id, 314, dataSize);
				pData += this._equipmentPower.Serialize(pData);
			}
		}

		// Token: 0x0600284B RID: 10315 RVA: 0x001DD738 File Offset: 0x001DB938
		public SpecialEffectList GetHealFlawSpeed()
		{
			return this._healFlawSpeed;
		}

		// Token: 0x0600284C RID: 10316 RVA: 0x001DD750 File Offset: 0x001DB950
		public unsafe void SetHealFlawSpeed(SpecialEffectList healFlawSpeed, DataContext context)
		{
			this._healFlawSpeed = healFlawSpeed;
			base.SetModifiedAndInvalidateInfluencedCache(316, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				int dataSize = this._healFlawSpeed.GetSerializedSize();
				byte* pData = OperationAdder.DynamicObjectCollection_SetDynamicField<int>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this._id, 315, dataSize);
				pData += this._healFlawSpeed.Serialize(pData);
			}
		}

		// Token: 0x0600284D RID: 10317 RVA: 0x001DD7C8 File Offset: 0x001DB9C8
		public SpecialEffectList GetUnlockSpeed()
		{
			return this._unlockSpeed;
		}

		// Token: 0x0600284E RID: 10318 RVA: 0x001DD7E0 File Offset: 0x001DB9E0
		public unsafe void SetUnlockSpeed(SpecialEffectList unlockSpeed, DataContext context)
		{
			this._unlockSpeed = unlockSpeed;
			base.SetModifiedAndInvalidateInfluencedCache(317, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				int dataSize = this._unlockSpeed.GetSerializedSize();
				byte* pData = OperationAdder.DynamicObjectCollection_SetDynamicField<int>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this._id, 316, dataSize);
				pData += this._unlockSpeed.Serialize(pData);
			}
		}

		// Token: 0x0600284F RID: 10319 RVA: 0x001DD858 File Offset: 0x001DBA58
		public SpecialEffectList GetFlawBonusFactor()
		{
			return this._flawBonusFactor;
		}

		// Token: 0x06002850 RID: 10320 RVA: 0x001DD870 File Offset: 0x001DBA70
		public unsafe void SetFlawBonusFactor(SpecialEffectList flawBonusFactor, DataContext context)
		{
			this._flawBonusFactor = flawBonusFactor;
			base.SetModifiedAndInvalidateInfluencedCache(318, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				int dataSize = this._flawBonusFactor.GetSerializedSize();
				byte* pData = OperationAdder.DynamicObjectCollection_SetDynamicField<int>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this._id, 317, dataSize);
				pData += this._flawBonusFactor.Serialize(pData);
			}
		}

		// Token: 0x06002851 RID: 10321 RVA: 0x001DD8E8 File Offset: 0x001DBAE8
		public SpecialEffectList GetCanCostShaTricks()
		{
			return this._canCostShaTricks;
		}

		// Token: 0x06002852 RID: 10322 RVA: 0x001DD900 File Offset: 0x001DBB00
		public unsafe void SetCanCostShaTricks(SpecialEffectList canCostShaTricks, DataContext context)
		{
			this._canCostShaTricks = canCostShaTricks;
			base.SetModifiedAndInvalidateInfluencedCache(319, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				int dataSize = this._canCostShaTricks.GetSerializedSize();
				byte* pData = OperationAdder.DynamicObjectCollection_SetDynamicField<int>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this._id, 318, dataSize);
				pData += this._canCostShaTricks.Serialize(pData);
			}
		}

		// Token: 0x06002853 RID: 10323 RVA: 0x001DD978 File Offset: 0x001DBB78
		public SpecialEffectList GetDefenderDirectFinalDamageValue()
		{
			return this._defenderDirectFinalDamageValue;
		}

		// Token: 0x06002854 RID: 10324 RVA: 0x001DD990 File Offset: 0x001DBB90
		public unsafe void SetDefenderDirectFinalDamageValue(SpecialEffectList defenderDirectFinalDamageValue, DataContext context)
		{
			this._defenderDirectFinalDamageValue = defenderDirectFinalDamageValue;
			base.SetModifiedAndInvalidateInfluencedCache(320, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				int dataSize = this._defenderDirectFinalDamageValue.GetSerializedSize();
				byte* pData = OperationAdder.DynamicObjectCollection_SetDynamicField<int>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this._id, 319, dataSize);
				pData += this._defenderDirectFinalDamageValue.Serialize(pData);
			}
		}

		// Token: 0x06002855 RID: 10325 RVA: 0x001DDA08 File Offset: 0x001DBC08
		public SpecialEffectList GetNormalAttackRecoveryFrame()
		{
			return this._normalAttackRecoveryFrame;
		}

		// Token: 0x06002856 RID: 10326 RVA: 0x001DDA20 File Offset: 0x001DBC20
		public unsafe void SetNormalAttackRecoveryFrame(SpecialEffectList normalAttackRecoveryFrame, DataContext context)
		{
			this._normalAttackRecoveryFrame = normalAttackRecoveryFrame;
			base.SetModifiedAndInvalidateInfluencedCache(321, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				int dataSize = this._normalAttackRecoveryFrame.GetSerializedSize();
				byte* pData = OperationAdder.DynamicObjectCollection_SetDynamicField<int>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this._id, 320, dataSize);
				pData += this._normalAttackRecoveryFrame.Serialize(pData);
			}
		}

		// Token: 0x06002857 RID: 10327 RVA: 0x001DDA98 File Offset: 0x001DBC98
		public SpecialEffectList GetFinalGoneMadInjury()
		{
			return this._finalGoneMadInjury;
		}

		// Token: 0x06002858 RID: 10328 RVA: 0x001DDAB0 File Offset: 0x001DBCB0
		public unsafe void SetFinalGoneMadInjury(SpecialEffectList finalGoneMadInjury, DataContext context)
		{
			this._finalGoneMadInjury = finalGoneMadInjury;
			base.SetModifiedAndInvalidateInfluencedCache(322, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				int dataSize = this._finalGoneMadInjury.GetSerializedSize();
				byte* pData = OperationAdder.DynamicObjectCollection_SetDynamicField<int>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this._id, 321, dataSize);
				pData += this._finalGoneMadInjury.Serialize(pData);
			}
		}

		// Token: 0x06002859 RID: 10329 RVA: 0x001DDB28 File Offset: 0x001DBD28
		public SpecialEffectList GetAttackerDirectFinalDamageValue()
		{
			return this._attackerDirectFinalDamageValue;
		}

		// Token: 0x0600285A RID: 10330 RVA: 0x001DDB40 File Offset: 0x001DBD40
		public unsafe void SetAttackerDirectFinalDamageValue(SpecialEffectList attackerDirectFinalDamageValue, DataContext context)
		{
			this._attackerDirectFinalDamageValue = attackerDirectFinalDamageValue;
			base.SetModifiedAndInvalidateInfluencedCache(323, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				int dataSize = this._attackerDirectFinalDamageValue.GetSerializedSize();
				byte* pData = OperationAdder.DynamicObjectCollection_SetDynamicField<int>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this._id, 322, dataSize);
				pData += this._attackerDirectFinalDamageValue.Serialize(pData);
			}
		}

		// Token: 0x0600285B RID: 10331 RVA: 0x001DDBB8 File Offset: 0x001DBDB8
		public SpecialEffectList GetCanCostTrickDuringPreparingSkill()
		{
			return this._canCostTrickDuringPreparingSkill;
		}

		// Token: 0x0600285C RID: 10332 RVA: 0x001DDBD0 File Offset: 0x001DBDD0
		public unsafe void SetCanCostTrickDuringPreparingSkill(SpecialEffectList canCostTrickDuringPreparingSkill, DataContext context)
		{
			this._canCostTrickDuringPreparingSkill = canCostTrickDuringPreparingSkill;
			base.SetModifiedAndInvalidateInfluencedCache(324, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				int dataSize = this._canCostTrickDuringPreparingSkill.GetSerializedSize();
				byte* pData = OperationAdder.DynamicObjectCollection_SetDynamicField<int>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this._id, 323, dataSize);
				pData += this._canCostTrickDuringPreparingSkill.Serialize(pData);
			}
		}

		// Token: 0x0600285D RID: 10333 RVA: 0x001DDC48 File Offset: 0x001DBE48
		public SpecialEffectList GetValidItemList()
		{
			return this._validItemList;
		}

		// Token: 0x0600285E RID: 10334 RVA: 0x001DDC60 File Offset: 0x001DBE60
		public unsafe void SetValidItemList(SpecialEffectList validItemList, DataContext context)
		{
			this._validItemList = validItemList;
			base.SetModifiedAndInvalidateInfluencedCache(325, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				int dataSize = this._validItemList.GetSerializedSize();
				byte* pData = OperationAdder.DynamicObjectCollection_SetDynamicField<int>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this._id, 324, dataSize);
				pData += this._validItemList.Serialize(pData);
			}
		}

		// Token: 0x0600285F RID: 10335 RVA: 0x001DDCD8 File Offset: 0x001DBED8
		public SpecialEffectList GetAcceptDamageCanAdd()
		{
			return this._acceptDamageCanAdd;
		}

		// Token: 0x06002860 RID: 10336 RVA: 0x001DDCF0 File Offset: 0x001DBEF0
		public unsafe void SetAcceptDamageCanAdd(SpecialEffectList acceptDamageCanAdd, DataContext context)
		{
			this._acceptDamageCanAdd = acceptDamageCanAdd;
			base.SetModifiedAndInvalidateInfluencedCache(326, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				int dataSize = this._acceptDamageCanAdd.GetSerializedSize();
				byte* pData = OperationAdder.DynamicObjectCollection_SetDynamicField<int>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this._id, 325, dataSize);
				pData += this._acceptDamageCanAdd.Serialize(pData);
			}
		}

		// Token: 0x06002861 RID: 10337 RVA: 0x001DDD68 File Offset: 0x001DBF68
		public SpecialEffectList GetMakeDamageCanReduce()
		{
			return this._makeDamageCanReduce;
		}

		// Token: 0x06002862 RID: 10338 RVA: 0x001DDD80 File Offset: 0x001DBF80
		public unsafe void SetMakeDamageCanReduce(SpecialEffectList makeDamageCanReduce, DataContext context)
		{
			this._makeDamageCanReduce = makeDamageCanReduce;
			base.SetModifiedAndInvalidateInfluencedCache(327, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				int dataSize = this._makeDamageCanReduce.GetSerializedSize();
				byte* pData = OperationAdder.DynamicObjectCollection_SetDynamicField<int>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this._id, 326, dataSize);
				pData += this._makeDamageCanReduce.Serialize(pData);
			}
		}

		// Token: 0x06002863 RID: 10339 RVA: 0x001DDDF8 File Offset: 0x001DBFF8
		public SpecialEffectList GetNormalAttackGetTrickCount()
		{
			return this._normalAttackGetTrickCount;
		}

		// Token: 0x06002864 RID: 10340 RVA: 0x001DDE10 File Offset: 0x001DC010
		public unsafe void SetNormalAttackGetTrickCount(SpecialEffectList normalAttackGetTrickCount, DataContext context)
		{
			this._normalAttackGetTrickCount = normalAttackGetTrickCount;
			base.SetModifiedAndInvalidateInfluencedCache(328, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				int dataSize = this._normalAttackGetTrickCount.GetSerializedSize();
				byte* pData = OperationAdder.DynamicObjectCollection_SetDynamicField<int>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this._id, 327, dataSize);
				pData += this._normalAttackGetTrickCount.Serialize(pData);
			}
		}

		// Token: 0x06002865 RID: 10341 RVA: 0x001DDE88 File Offset: 0x001DC088
		public AffectedData()
		{
			this._maxStrength = new SpecialEffectList();
			this._maxDexterity = new SpecialEffectList();
			this._maxConcentration = new SpecialEffectList();
			this._maxVitality = new SpecialEffectList();
			this._maxEnergy = new SpecialEffectList();
			this._maxIntelligence = new SpecialEffectList();
			this._recoveryOfStance = new SpecialEffectList();
			this._recoveryOfBreath = new SpecialEffectList();
			this._moveSpeed = new SpecialEffectList();
			this._recoveryOfFlaw = new SpecialEffectList();
			this._castSpeed = new SpecialEffectList();
			this._recoveryOfBlockedAcupoint = new SpecialEffectList();
			this._weaponSwitchSpeed = new SpecialEffectList();
			this._attackSpeed = new SpecialEffectList();
			this._innerRatio = new SpecialEffectList();
			this._recoveryOfQiDisorder = new SpecialEffectList();
			this._minorAttributeFixMaxValue = new SpecialEffectList();
			this._minorAttributeFixMinValue = new SpecialEffectList();
			this._resistOfHotPoison = new SpecialEffectList();
			this._resistOfGloomyPoison = new SpecialEffectList();
			this._resistOfColdPoison = new SpecialEffectList();
			this._resistOfRedPoison = new SpecialEffectList();
			this._resistOfRottenPoison = new SpecialEffectList();
			this._resistOfIllusoryPoison = new SpecialEffectList();
			this._displayAge = new SpecialEffectList();
			this._neiliProportionOfFiveElements = new SpecialEffectList();
			this._weaponMaxPower = new SpecialEffectList();
			this._weaponUseRequirement = new SpecialEffectList();
			this._weaponAttackRange = new SpecialEffectList();
			this._armorMaxPower = new SpecialEffectList();
			this._armorUseRequirement = new SpecialEffectList();
			this._hitStrength = new SpecialEffectList();
			this._hitTechnique = new SpecialEffectList();
			this._hitSpeed = new SpecialEffectList();
			this._hitMind = new SpecialEffectList();
			this._hitCanChange = new SpecialEffectList();
			this._hitChangeEffectPercent = new SpecialEffectList();
			this._avoidStrength = new SpecialEffectList();
			this._avoidTechnique = new SpecialEffectList();
			this._avoidSpeed = new SpecialEffectList();
			this._avoidMind = new SpecialEffectList();
			this._avoidCanChange = new SpecialEffectList();
			this._avoidChangeEffectPercent = new SpecialEffectList();
			this._penetrateOuter = new SpecialEffectList();
			this._penetrateInner = new SpecialEffectList();
			this._penetrateResistOuter = new SpecialEffectList();
			this._penetrateResistInner = new SpecialEffectList();
			this._neiliAllocationAttack = new SpecialEffectList();
			this._neiliAllocationAgile = new SpecialEffectList();
			this._neiliAllocationDefense = new SpecialEffectList();
			this._neiliAllocationAssist = new SpecialEffectList();
			this._happiness = new SpecialEffectList();
			this._maxHealth = new SpecialEffectList();
			this._healthCost = new SpecialEffectList();
			this._moveSpeedCanChange = new SpecialEffectList();
			this._attackerHitStrength = new SpecialEffectList();
			this._attackerHitTechnique = new SpecialEffectList();
			this._attackerHitSpeed = new SpecialEffectList();
			this._attackerHitMind = new SpecialEffectList();
			this._attackerAvoidStrength = new SpecialEffectList();
			this._attackerAvoidTechnique = new SpecialEffectList();
			this._attackerAvoidSpeed = new SpecialEffectList();
			this._attackerAvoidMind = new SpecialEffectList();
			this._attackerPenetrateOuter = new SpecialEffectList();
			this._attackerPenetrateInner = new SpecialEffectList();
			this._attackerPenetrateResistOuter = new SpecialEffectList();
			this._attackerPenetrateResistInner = new SpecialEffectList();
			this._attackHitType = new SpecialEffectList();
			this._makeDirectDamage = new SpecialEffectList();
			this._makeBounceDamage = new SpecialEffectList();
			this._makeFightBackDamage = new SpecialEffectList();
			this._makePoisonLevel = new SpecialEffectList();
			this._makePoisonValue = new SpecialEffectList();
			this._attackerHitOdds = new SpecialEffectList();
			this._attackerFightBackHitOdds = new SpecialEffectList();
			this._attackerPursueOdds = new SpecialEffectList();
			this._makedInjuryChangeToOld = new SpecialEffectList();
			this._makedPoisonChangeToOld = new SpecialEffectList();
			this._makeDamageType = new SpecialEffectList();
			this._canMakeInjuryToNoInjuryPart = new SpecialEffectList();
			this._makePoisonType = new SpecialEffectList();
			this._normalAttackWeapon = new SpecialEffectList();
			this._normalAttackTrick = new SpecialEffectList();
			this._extraFlawCount = new SpecialEffectList();
			this._attackCanBounce = new SpecialEffectList();
			this._attackCanFightBack = new SpecialEffectList();
			this._makeFightBackInjuryMark = new SpecialEffectList();
			this._legSkillUseShoes = new SpecialEffectList();
			this._attackerFinalDamageValue = new SpecialEffectList();
			this._defenderHitStrength = new SpecialEffectList();
			this._defenderHitTechnique = new SpecialEffectList();
			this._defenderHitSpeed = new SpecialEffectList();
			this._defenderHitMind = new SpecialEffectList();
			this._defenderAvoidStrength = new SpecialEffectList();
			this._defenderAvoidTechnique = new SpecialEffectList();
			this._defenderAvoidSpeed = new SpecialEffectList();
			this._defenderAvoidMind = new SpecialEffectList();
			this._defenderPenetrateOuter = new SpecialEffectList();
			this._defenderPenetrateInner = new SpecialEffectList();
			this._defenderPenetrateResistOuter = new SpecialEffectList();
			this._defenderPenetrateResistInner = new SpecialEffectList();
			this._acceptDirectDamage = new SpecialEffectList();
			this._acceptBounceDamage = new SpecialEffectList();
			this._acceptFightBackDamage = new SpecialEffectList();
			this._acceptPoisonLevel = new SpecialEffectList();
			this._acceptPoisonValue = new SpecialEffectList();
			this._defenderHitOdds = new SpecialEffectList();
			this._defenderFightBackHitOdds = new SpecialEffectList();
			this._defenderPursueOdds = new SpecialEffectList();
			this._acceptMaxInjuryCount = new SpecialEffectList();
			this._bouncePower = new SpecialEffectList();
			this._fightBackPower = new SpecialEffectList();
			this._directDamageInnerRatio = new SpecialEffectList();
			this._defenderFinalDamageValue = new SpecialEffectList();
			this._directDamageValue = new SpecialEffectList();
			this._directInjuryMark = new SpecialEffectList();
			this._goneMadInjury = new SpecialEffectList();
			this._healInjurySpeed = new SpecialEffectList();
			this._healInjuryBuff = new SpecialEffectList();
			this._healInjuryDebuff = new SpecialEffectList();
			this._healPoisonSpeed = new SpecialEffectList();
			this._healPoisonBuff = new SpecialEffectList();
			this._healPoisonDebuff = new SpecialEffectList();
			this._fleeSpeed = new SpecialEffectList();
			this._maxFlawCount = new SpecialEffectList();
			this._canAddFlaw = new SpecialEffectList();
			this._flawLevel = new SpecialEffectList();
			this._flawLevelCanReduce = new SpecialEffectList();
			this._flawCount = new SpecialEffectList();
			this._maxAcupointCount = new SpecialEffectList();
			this._canAddAcupoint = new SpecialEffectList();
			this._acupointLevel = new SpecialEffectList();
			this._acupointLevelCanReduce = new SpecialEffectList();
			this._acupointCount = new SpecialEffectList();
			this._addNeiliAllocation = new SpecialEffectList();
			this._costNeiliAllocation = new SpecialEffectList();
			this._canChangeNeiliAllocation = new SpecialEffectList();
			this._canGetTrick = new SpecialEffectList();
			this._getTrickType = new SpecialEffectList();
			this._attackBodyPart = new SpecialEffectList();
			this._weaponEquipAttack = new SpecialEffectList();
			this._weaponEquipDefense = new SpecialEffectList();
			this._armorEquipAttack = new SpecialEffectList();
			this._armorEquipDefense = new SpecialEffectList();
			this._attackRangeForward = new SpecialEffectList();
			this._attackRangeBackward = new SpecialEffectList();
			this._moveCanBeStopped = new SpecialEffectList();
			this._canForcedMove = new SpecialEffectList();
			this._mobilityCanBeRemoved = new SpecialEffectList();
			this._mobilityCostByEffect = new SpecialEffectList();
			this._moveDistance = new SpecialEffectList();
			this._jumpPrepareFrame = new SpecialEffectList();
			this._bounceInjuryMark = new SpecialEffectList();
			this._skillHasCost = new SpecialEffectList();
			this._combatStateEffect = new SpecialEffectList();
			this._changeNeedUseSkill = new SpecialEffectList();
			this._changeDistanceIsMove = new SpecialEffectList();
			this._replaceCharHit = new SpecialEffectList();
			this._canAddPoison = new SpecialEffectList();
			this._canReducePoison = new SpecialEffectList();
			this._reducePoisonValue = new SpecialEffectList();
			this._poisonCanAffect = new SpecialEffectList();
			this._poisonAffectCount = new SpecialEffectList();
			this._costTricks = new SpecialEffectList();
			this._jumpMoveDistance = new SpecialEffectList();
			this._combatStateToAdd = new SpecialEffectList();
			this._combatStatePower = new SpecialEffectList();
			this._breakBodyPartInjuryCount = new SpecialEffectList();
			this._bodyPartIsBroken = new SpecialEffectList();
			this._maxTrickCount = new SpecialEffectList();
			this._maxBreathPercent = new SpecialEffectList();
			this._maxStancePercent = new SpecialEffectList();
			this._extraBreathPercent = new SpecialEffectList();
			this._extraStancePercent = new SpecialEffectList();
			this._moveCostMobility = new SpecialEffectList();
			this._defendSkillKeepTime = new SpecialEffectList();
			this._bounceRange = new SpecialEffectList();
			this._mindMarkKeepTime = new SpecialEffectList();
			this._skillMobilityCostPerFrame = new SpecialEffectList();
			this._canAddWug = new SpecialEffectList();
			this._hasGodWeaponBuff = new SpecialEffectList();
			this._hasGodArmorBuff = new SpecialEffectList();
			this._teammateCmdRequireGenerateValue = new SpecialEffectList();
			this._teammateCmdEffect = new SpecialEffectList();
			this._flawRecoverSpeed = new SpecialEffectList();
			this._acupointRecoverSpeed = new SpecialEffectList();
			this._mindMarkRecoverSpeed = new SpecialEffectList();
			this._injuryAutoHealSpeed = new SpecialEffectList();
			this._canRecoverBreath = new SpecialEffectList();
			this._canRecoverStance = new SpecialEffectList();
			this._fatalDamageValue = new SpecialEffectList();
			this._fatalDamageMarkCount = new SpecialEffectList();
			this._canFightBackDuringPrepareSkill = new SpecialEffectList();
			this._skillPrepareSpeed = new SpecialEffectList();
			this._breathRecoverSpeed = new SpecialEffectList();
			this._stanceRecoverSpeed = new SpecialEffectList();
			this._mobilityRecoverSpeed = new SpecialEffectList();
			this._changeTrickProgressAddValue = new SpecialEffectList();
			this._power = new SpecialEffectList();
			this._maxPower = new SpecialEffectList();
			this._powerCanReduce = new SpecialEffectList();
			this._useRequirement = new SpecialEffectList();
			this._currInnerRatio = new SpecialEffectList();
			this._costBreathAndStance = new SpecialEffectList();
			this._costBreath = new SpecialEffectList();
			this._costStance = new SpecialEffectList();
			this._costMobility = new SpecialEffectList();
			this._skillCostTricks = new SpecialEffectList();
			this._effectDirection = new SpecialEffectList();
			this._effectDirectionCanChange = new SpecialEffectList();
			this._gridCost = new SpecialEffectList();
			this._prepareTotalProgress = new SpecialEffectList();
			this._specificGridCount = new SpecialEffectList();
			this._genericGridCount = new SpecialEffectList();
			this._canInterrupt = new SpecialEffectList();
			this._interruptOdds = new SpecialEffectList();
			this._canSilence = new SpecialEffectList();
			this._silenceOdds = new SpecialEffectList();
			this._canCastWithBrokenBodyPart = new SpecialEffectList();
			this._addPowerCanBeRemoved = new SpecialEffectList();
			this._skillType = new SpecialEffectList();
			this._effectCountCanChange = new SpecialEffectList();
			this._canCastInDefend = new SpecialEffectList();
			this._hitDistribution = new SpecialEffectList();
			this._canCastOnLackBreath = new SpecialEffectList();
			this._canCastOnLackStance = new SpecialEffectList();
			this._costBreathOnCast = new SpecialEffectList();
			this._costStanceOnCast = new SpecialEffectList();
			this._canUseMobilityAsBreath = new SpecialEffectList();
			this._canUseMobilityAsStance = new SpecialEffectList();
			this._castCostNeiliAllocation = new SpecialEffectList();
			this._acceptPoisonResist = new SpecialEffectList();
			this._makePoisonResist = new SpecialEffectList();
			this._canCriticalHit = new SpecialEffectList();
			this._canCostNeiliAllocationEffect = new SpecialEffectList();
			this._consummateLevelRelatedMainAttributesHitValues = new SpecialEffectList();
			this._consummateLevelRelatedMainAttributesAvoidValues = new SpecialEffectList();
			this._consummateLevelRelatedMainAttributesPenetrations = new SpecialEffectList();
			this._consummateLevelRelatedMainAttributesPenetrationResists = new SpecialEffectList();
			this._skillAlsoAsFiveElements = new SpecialEffectList();
			this._innerInjuryImmunity = new SpecialEffectList();
			this._outerInjuryImmunity = new SpecialEffectList();
			this._poisonAffectThreshold = new SpecialEffectList();
			this._lockDistance = new SpecialEffectList();
			this._resistOfAllPoison = new SpecialEffectList();
			this._makePoisonTarget = new SpecialEffectList();
			this._acceptPoisonTarget = new SpecialEffectList();
			this._certainCriticalHit = new SpecialEffectList();
			this._mindMarkCount = new SpecialEffectList();
			this._canFightBackWithHit = new SpecialEffectList();
			this._inevitableHit = new SpecialEffectList();
			this._attackCanPursue = new SpecialEffectList();
			this._combatSkillDataEffectList = new SpecialEffectList();
			this._criticalOdds = new SpecialEffectList();
			this._stanceCostByEffect = new SpecialEffectList();
			this._breathCostByEffect = new SpecialEffectList();
			this._powerAddRatio = new SpecialEffectList();
			this._powerReduceRatio = new SpecialEffectList();
			this._poisonAffectProduceValue = new SpecialEffectList();
			this._canReadingOnMonthChange = new SpecialEffectList();
			this._medicineEffect = new SpecialEffectList();
			this._xiangshuInfectionDelta = new SpecialEffectList();
			this._healthDelta = new SpecialEffectList();
			this._weaponSilenceFrame = new SpecialEffectList();
			this._silenceFrame = new SpecialEffectList();
			this._currAgeDelta = new SpecialEffectList();
			this._goneMadInAllBreak = new SpecialEffectList();
			this._makeLoveRateOnMonthChange = new SpecialEffectList();
			this._canAutoHealOnMonthChange = new SpecialEffectList();
			this._happinessDelta = new SpecialEffectList();
			this._teammateCmdCanUse = new SpecialEffectList();
			this._mixPoisonInfinityAffect = new SpecialEffectList();
			this._attackRangeMaxAcupoint = new SpecialEffectList();
			this._maxMobilityPercent = new SpecialEffectList();
			this._makeMindDamage = new SpecialEffectList();
			this._acceptMindDamage = new SpecialEffectList();
			this._hitAddByTempValue = new SpecialEffectList();
			this._avoidAddByTempValue = new SpecialEffectList();
			this._ignoreEquipmentOverload = new SpecialEffectList();
			this._canCostEnemyUsableTricks = new SpecialEffectList();
			this._ignoreArmor = new SpecialEffectList();
			this._unyieldingFallen = new SpecialEffectList();
			this._normalAttackPrepareFrame = new SpecialEffectList();
			this._canCostUselessTricks = new SpecialEffectList();
			this._defendSkillCanAffect = new SpecialEffectList();
			this._assistSkillCanAffect = new SpecialEffectList();
			this._agileSkillCanAffect = new SpecialEffectList();
			this._allMarkChangeToMind = new SpecialEffectList();
			this._mindMarkChangeToFatal = new SpecialEffectList();
			this._canCast = new SpecialEffectList();
			this._inevitableAvoid = new SpecialEffectList();
			this._powerEffectReverse = new SpecialEffectList();
			this._featureBonusReverse = new SpecialEffectList();
			this._wugFatalDamageValue = new SpecialEffectList();
			this._canRecoverHealthOnMonthChange = new SpecialEffectList();
			this._takeRevengeRateOnMonthChange = new SpecialEffectList();
			this._consummateLevelBonus = new SpecialEffectList();
			this._neiliDelta = new SpecialEffectList();
			this._canMakeLoveSpecialOnMonthChange = new SpecialEffectList();
			this._healAcupointSpeed = new SpecialEffectList();
			this._maxChangeTrickCount = new SpecialEffectList();
			this._convertCostBreathAndStance = new SpecialEffectList();
			this._personalitiesAll = new SpecialEffectList();
			this._finalFatalDamageMarkCount = new SpecialEffectList();
			this._infinityMindMarkProgress = new SpecialEffectList();
			this._combatSkillAiScorePower = new SpecialEffectList();
			this._normalAttackChangeToUnlockAttack = new SpecialEffectList();
			this._attackBodyPartOdds = new SpecialEffectList();
			this._changeDurability = new SpecialEffectList();
			this._equipmentBonus = new SpecialEffectList();
			this._equipmentWeight = new SpecialEffectList();
			this._rawCreateEffectList = new SpecialEffectList();
			this._jiTrickAsWeaponTrickCount = new SpecialEffectList();
			this._uselessTrickAsJiTrickCount = new SpecialEffectList();
			this._equipmentPower = new SpecialEffectList();
			this._healFlawSpeed = new SpecialEffectList();
			this._unlockSpeed = new SpecialEffectList();
			this._flawBonusFactor = new SpecialEffectList();
			this._canCostShaTricks = new SpecialEffectList();
			this._defenderDirectFinalDamageValue = new SpecialEffectList();
			this._normalAttackRecoveryFrame = new SpecialEffectList();
			this._finalGoneMadInjury = new SpecialEffectList();
			this._attackerDirectFinalDamageValue = new SpecialEffectList();
			this._canCostTrickDuringPreparingSkill = new SpecialEffectList();
			this._validItemList = new SpecialEffectList();
			this._acceptDamageCanAdd = new SpecialEffectList();
			this._makeDamageCanReduce = new SpecialEffectList();
			this._normalAttackGetTrickCount = new SpecialEffectList();
		}

		// Token: 0x06002866 RID: 10342 RVA: 0x001DECB8 File Offset: 0x001DCEB8
		public bool IsSerializedSizeFixed()
		{
			return false;
		}

		// Token: 0x06002867 RID: 10343 RVA: 0x001DECCC File Offset: 0x001DCECC
		public int GetSerializedSize()
		{
			int totalSize = 1316;
			int dataSize = this._maxStrength.GetSerializedSize();
			totalSize += dataSize;
			int dataSize2 = this._maxDexterity.GetSerializedSize();
			totalSize += dataSize2;
			int dataSize3 = this._maxConcentration.GetSerializedSize();
			totalSize += dataSize3;
			int dataSize4 = this._maxVitality.GetSerializedSize();
			totalSize += dataSize4;
			int dataSize5 = this._maxEnergy.GetSerializedSize();
			totalSize += dataSize5;
			int dataSize6 = this._maxIntelligence.GetSerializedSize();
			totalSize += dataSize6;
			int dataSize7 = this._recoveryOfStance.GetSerializedSize();
			totalSize += dataSize7;
			int dataSize8 = this._recoveryOfBreath.GetSerializedSize();
			totalSize += dataSize8;
			int dataSize9 = this._moveSpeed.GetSerializedSize();
			totalSize += dataSize9;
			int dataSize10 = this._recoveryOfFlaw.GetSerializedSize();
			totalSize += dataSize10;
			int dataSize11 = this._castSpeed.GetSerializedSize();
			totalSize += dataSize11;
			int dataSize12 = this._recoveryOfBlockedAcupoint.GetSerializedSize();
			totalSize += dataSize12;
			int dataSize13 = this._weaponSwitchSpeed.GetSerializedSize();
			totalSize += dataSize13;
			int dataSize14 = this._attackSpeed.GetSerializedSize();
			totalSize += dataSize14;
			int dataSize15 = this._innerRatio.GetSerializedSize();
			totalSize += dataSize15;
			int dataSize16 = this._recoveryOfQiDisorder.GetSerializedSize();
			totalSize += dataSize16;
			int dataSize17 = this._minorAttributeFixMaxValue.GetSerializedSize();
			totalSize += dataSize17;
			int dataSize18 = this._minorAttributeFixMinValue.GetSerializedSize();
			totalSize += dataSize18;
			int dataSize19 = this._resistOfHotPoison.GetSerializedSize();
			totalSize += dataSize19;
			int dataSize20 = this._resistOfGloomyPoison.GetSerializedSize();
			totalSize += dataSize20;
			int dataSize21 = this._resistOfColdPoison.GetSerializedSize();
			totalSize += dataSize21;
			int dataSize22 = this._resistOfRedPoison.GetSerializedSize();
			totalSize += dataSize22;
			int dataSize23 = this._resistOfRottenPoison.GetSerializedSize();
			totalSize += dataSize23;
			int dataSize24 = this._resistOfIllusoryPoison.GetSerializedSize();
			totalSize += dataSize24;
			int dataSize25 = this._displayAge.GetSerializedSize();
			totalSize += dataSize25;
			int dataSize26 = this._neiliProportionOfFiveElements.GetSerializedSize();
			totalSize += dataSize26;
			int dataSize27 = this._weaponMaxPower.GetSerializedSize();
			totalSize += dataSize27;
			int dataSize28 = this._weaponUseRequirement.GetSerializedSize();
			totalSize += dataSize28;
			int dataSize29 = this._weaponAttackRange.GetSerializedSize();
			totalSize += dataSize29;
			int dataSize30 = this._armorMaxPower.GetSerializedSize();
			totalSize += dataSize30;
			int dataSize31 = this._armorUseRequirement.GetSerializedSize();
			totalSize += dataSize31;
			int dataSize32 = this._hitStrength.GetSerializedSize();
			totalSize += dataSize32;
			int dataSize33 = this._hitTechnique.GetSerializedSize();
			totalSize += dataSize33;
			int dataSize34 = this._hitSpeed.GetSerializedSize();
			totalSize += dataSize34;
			int dataSize35 = this._hitMind.GetSerializedSize();
			totalSize += dataSize35;
			int dataSize36 = this._hitCanChange.GetSerializedSize();
			totalSize += dataSize36;
			int dataSize37 = this._hitChangeEffectPercent.GetSerializedSize();
			totalSize += dataSize37;
			int dataSize38 = this._avoidStrength.GetSerializedSize();
			totalSize += dataSize38;
			int dataSize39 = this._avoidTechnique.GetSerializedSize();
			totalSize += dataSize39;
			int dataSize40 = this._avoidSpeed.GetSerializedSize();
			totalSize += dataSize40;
			int dataSize41 = this._avoidMind.GetSerializedSize();
			totalSize += dataSize41;
			int dataSize42 = this._avoidCanChange.GetSerializedSize();
			totalSize += dataSize42;
			int dataSize43 = this._avoidChangeEffectPercent.GetSerializedSize();
			totalSize += dataSize43;
			int dataSize44 = this._penetrateOuter.GetSerializedSize();
			totalSize += dataSize44;
			int dataSize45 = this._penetrateInner.GetSerializedSize();
			totalSize += dataSize45;
			int dataSize46 = this._penetrateResistOuter.GetSerializedSize();
			totalSize += dataSize46;
			int dataSize47 = this._penetrateResistInner.GetSerializedSize();
			totalSize += dataSize47;
			int dataSize48 = this._neiliAllocationAttack.GetSerializedSize();
			totalSize += dataSize48;
			int dataSize49 = this._neiliAllocationAgile.GetSerializedSize();
			totalSize += dataSize49;
			int dataSize50 = this._neiliAllocationDefense.GetSerializedSize();
			totalSize += dataSize50;
			int dataSize51 = this._neiliAllocationAssist.GetSerializedSize();
			totalSize += dataSize51;
			int dataSize52 = this._happiness.GetSerializedSize();
			totalSize += dataSize52;
			int dataSize53 = this._maxHealth.GetSerializedSize();
			totalSize += dataSize53;
			int dataSize54 = this._healthCost.GetSerializedSize();
			totalSize += dataSize54;
			int dataSize55 = this._moveSpeedCanChange.GetSerializedSize();
			totalSize += dataSize55;
			int dataSize56 = this._attackerHitStrength.GetSerializedSize();
			totalSize += dataSize56;
			int dataSize57 = this._attackerHitTechnique.GetSerializedSize();
			totalSize += dataSize57;
			int dataSize58 = this._attackerHitSpeed.GetSerializedSize();
			totalSize += dataSize58;
			int dataSize59 = this._attackerHitMind.GetSerializedSize();
			totalSize += dataSize59;
			int dataSize60 = this._attackerAvoidStrength.GetSerializedSize();
			totalSize += dataSize60;
			int dataSize61 = this._attackerAvoidTechnique.GetSerializedSize();
			totalSize += dataSize61;
			int dataSize62 = this._attackerAvoidSpeed.GetSerializedSize();
			totalSize += dataSize62;
			int dataSize63 = this._attackerAvoidMind.GetSerializedSize();
			totalSize += dataSize63;
			int dataSize64 = this._attackerPenetrateOuter.GetSerializedSize();
			totalSize += dataSize64;
			int dataSize65 = this._attackerPenetrateInner.GetSerializedSize();
			totalSize += dataSize65;
			int dataSize66 = this._attackerPenetrateResistOuter.GetSerializedSize();
			totalSize += dataSize66;
			int dataSize67 = this._attackerPenetrateResistInner.GetSerializedSize();
			totalSize += dataSize67;
			int dataSize68 = this._attackHitType.GetSerializedSize();
			totalSize += dataSize68;
			int dataSize69 = this._makeDirectDamage.GetSerializedSize();
			totalSize += dataSize69;
			int dataSize70 = this._makeBounceDamage.GetSerializedSize();
			totalSize += dataSize70;
			int dataSize71 = this._makeFightBackDamage.GetSerializedSize();
			totalSize += dataSize71;
			int dataSize72 = this._makePoisonLevel.GetSerializedSize();
			totalSize += dataSize72;
			int dataSize73 = this._makePoisonValue.GetSerializedSize();
			totalSize += dataSize73;
			int dataSize74 = this._attackerHitOdds.GetSerializedSize();
			totalSize += dataSize74;
			int dataSize75 = this._attackerFightBackHitOdds.GetSerializedSize();
			totalSize += dataSize75;
			int dataSize76 = this._attackerPursueOdds.GetSerializedSize();
			totalSize += dataSize76;
			int dataSize77 = this._makedInjuryChangeToOld.GetSerializedSize();
			totalSize += dataSize77;
			int dataSize78 = this._makedPoisonChangeToOld.GetSerializedSize();
			totalSize += dataSize78;
			int dataSize79 = this._makeDamageType.GetSerializedSize();
			totalSize += dataSize79;
			int dataSize80 = this._canMakeInjuryToNoInjuryPart.GetSerializedSize();
			totalSize += dataSize80;
			int dataSize81 = this._makePoisonType.GetSerializedSize();
			totalSize += dataSize81;
			int dataSize82 = this._normalAttackWeapon.GetSerializedSize();
			totalSize += dataSize82;
			int dataSize83 = this._normalAttackTrick.GetSerializedSize();
			totalSize += dataSize83;
			int dataSize84 = this._extraFlawCount.GetSerializedSize();
			totalSize += dataSize84;
			int dataSize85 = this._attackCanBounce.GetSerializedSize();
			totalSize += dataSize85;
			int dataSize86 = this._attackCanFightBack.GetSerializedSize();
			totalSize += dataSize86;
			int dataSize87 = this._makeFightBackInjuryMark.GetSerializedSize();
			totalSize += dataSize87;
			int dataSize88 = this._legSkillUseShoes.GetSerializedSize();
			totalSize += dataSize88;
			int dataSize89 = this._attackerFinalDamageValue.GetSerializedSize();
			totalSize += dataSize89;
			int dataSize90 = this._defenderHitStrength.GetSerializedSize();
			totalSize += dataSize90;
			int dataSize91 = this._defenderHitTechnique.GetSerializedSize();
			totalSize += dataSize91;
			int dataSize92 = this._defenderHitSpeed.GetSerializedSize();
			totalSize += dataSize92;
			int dataSize93 = this._defenderHitMind.GetSerializedSize();
			totalSize += dataSize93;
			int dataSize94 = this._defenderAvoidStrength.GetSerializedSize();
			totalSize += dataSize94;
			int dataSize95 = this._defenderAvoidTechnique.GetSerializedSize();
			totalSize += dataSize95;
			int dataSize96 = this._defenderAvoidSpeed.GetSerializedSize();
			totalSize += dataSize96;
			int dataSize97 = this._defenderAvoidMind.GetSerializedSize();
			totalSize += dataSize97;
			int dataSize98 = this._defenderPenetrateOuter.GetSerializedSize();
			totalSize += dataSize98;
			int dataSize99 = this._defenderPenetrateInner.GetSerializedSize();
			totalSize += dataSize99;
			int dataSize100 = this._defenderPenetrateResistOuter.GetSerializedSize();
			totalSize += dataSize100;
			int dataSize101 = this._defenderPenetrateResistInner.GetSerializedSize();
			totalSize += dataSize101;
			int dataSize102 = this._acceptDirectDamage.GetSerializedSize();
			totalSize += dataSize102;
			int dataSize103 = this._acceptBounceDamage.GetSerializedSize();
			totalSize += dataSize103;
			int dataSize104 = this._acceptFightBackDamage.GetSerializedSize();
			totalSize += dataSize104;
			int dataSize105 = this._acceptPoisonLevel.GetSerializedSize();
			totalSize += dataSize105;
			int dataSize106 = this._acceptPoisonValue.GetSerializedSize();
			totalSize += dataSize106;
			int dataSize107 = this._defenderHitOdds.GetSerializedSize();
			totalSize += dataSize107;
			int dataSize108 = this._defenderFightBackHitOdds.GetSerializedSize();
			totalSize += dataSize108;
			int dataSize109 = this._defenderPursueOdds.GetSerializedSize();
			totalSize += dataSize109;
			int dataSize110 = this._acceptMaxInjuryCount.GetSerializedSize();
			totalSize += dataSize110;
			int dataSize111 = this._bouncePower.GetSerializedSize();
			totalSize += dataSize111;
			int dataSize112 = this._fightBackPower.GetSerializedSize();
			totalSize += dataSize112;
			int dataSize113 = this._directDamageInnerRatio.GetSerializedSize();
			totalSize += dataSize113;
			int dataSize114 = this._defenderFinalDamageValue.GetSerializedSize();
			totalSize += dataSize114;
			int dataSize115 = this._directDamageValue.GetSerializedSize();
			totalSize += dataSize115;
			int dataSize116 = this._directInjuryMark.GetSerializedSize();
			totalSize += dataSize116;
			int dataSize117 = this._goneMadInjury.GetSerializedSize();
			totalSize += dataSize117;
			int dataSize118 = this._healInjurySpeed.GetSerializedSize();
			totalSize += dataSize118;
			int dataSize119 = this._healInjuryBuff.GetSerializedSize();
			totalSize += dataSize119;
			int dataSize120 = this._healInjuryDebuff.GetSerializedSize();
			totalSize += dataSize120;
			int dataSize121 = this._healPoisonSpeed.GetSerializedSize();
			totalSize += dataSize121;
			int dataSize122 = this._healPoisonBuff.GetSerializedSize();
			totalSize += dataSize122;
			int dataSize123 = this._healPoisonDebuff.GetSerializedSize();
			totalSize += dataSize123;
			int dataSize124 = this._fleeSpeed.GetSerializedSize();
			totalSize += dataSize124;
			int dataSize125 = this._maxFlawCount.GetSerializedSize();
			totalSize += dataSize125;
			int dataSize126 = this._canAddFlaw.GetSerializedSize();
			totalSize += dataSize126;
			int dataSize127 = this._flawLevel.GetSerializedSize();
			totalSize += dataSize127;
			int dataSize128 = this._flawLevelCanReduce.GetSerializedSize();
			totalSize += dataSize128;
			int dataSize129 = this._flawCount.GetSerializedSize();
			totalSize += dataSize129;
			int dataSize130 = this._maxAcupointCount.GetSerializedSize();
			totalSize += dataSize130;
			int dataSize131 = this._canAddAcupoint.GetSerializedSize();
			totalSize += dataSize131;
			int dataSize132 = this._acupointLevel.GetSerializedSize();
			totalSize += dataSize132;
			int dataSize133 = this._acupointLevelCanReduce.GetSerializedSize();
			totalSize += dataSize133;
			int dataSize134 = this._acupointCount.GetSerializedSize();
			totalSize += dataSize134;
			int dataSize135 = this._addNeiliAllocation.GetSerializedSize();
			totalSize += dataSize135;
			int dataSize136 = this._costNeiliAllocation.GetSerializedSize();
			totalSize += dataSize136;
			int dataSize137 = this._canChangeNeiliAllocation.GetSerializedSize();
			totalSize += dataSize137;
			int dataSize138 = this._canGetTrick.GetSerializedSize();
			totalSize += dataSize138;
			int dataSize139 = this._getTrickType.GetSerializedSize();
			totalSize += dataSize139;
			int dataSize140 = this._attackBodyPart.GetSerializedSize();
			totalSize += dataSize140;
			int dataSize141 = this._weaponEquipAttack.GetSerializedSize();
			totalSize += dataSize141;
			int dataSize142 = this._weaponEquipDefense.GetSerializedSize();
			totalSize += dataSize142;
			int dataSize143 = this._armorEquipAttack.GetSerializedSize();
			totalSize += dataSize143;
			int dataSize144 = this._armorEquipDefense.GetSerializedSize();
			totalSize += dataSize144;
			int dataSize145 = this._attackRangeForward.GetSerializedSize();
			totalSize += dataSize145;
			int dataSize146 = this._attackRangeBackward.GetSerializedSize();
			totalSize += dataSize146;
			int dataSize147 = this._moveCanBeStopped.GetSerializedSize();
			totalSize += dataSize147;
			int dataSize148 = this._canForcedMove.GetSerializedSize();
			totalSize += dataSize148;
			int dataSize149 = this._mobilityCanBeRemoved.GetSerializedSize();
			totalSize += dataSize149;
			int dataSize150 = this._mobilityCostByEffect.GetSerializedSize();
			totalSize += dataSize150;
			int dataSize151 = this._moveDistance.GetSerializedSize();
			totalSize += dataSize151;
			int dataSize152 = this._jumpPrepareFrame.GetSerializedSize();
			totalSize += dataSize152;
			int dataSize153 = this._bounceInjuryMark.GetSerializedSize();
			totalSize += dataSize153;
			int dataSize154 = this._skillHasCost.GetSerializedSize();
			totalSize += dataSize154;
			int dataSize155 = this._combatStateEffect.GetSerializedSize();
			totalSize += dataSize155;
			int dataSize156 = this._changeNeedUseSkill.GetSerializedSize();
			totalSize += dataSize156;
			int dataSize157 = this._changeDistanceIsMove.GetSerializedSize();
			totalSize += dataSize157;
			int dataSize158 = this._replaceCharHit.GetSerializedSize();
			totalSize += dataSize158;
			int dataSize159 = this._canAddPoison.GetSerializedSize();
			totalSize += dataSize159;
			int dataSize160 = this._canReducePoison.GetSerializedSize();
			totalSize += dataSize160;
			int dataSize161 = this._reducePoisonValue.GetSerializedSize();
			totalSize += dataSize161;
			int dataSize162 = this._poisonCanAffect.GetSerializedSize();
			totalSize += dataSize162;
			int dataSize163 = this._poisonAffectCount.GetSerializedSize();
			totalSize += dataSize163;
			int dataSize164 = this._costTricks.GetSerializedSize();
			totalSize += dataSize164;
			int dataSize165 = this._jumpMoveDistance.GetSerializedSize();
			totalSize += dataSize165;
			int dataSize166 = this._combatStateToAdd.GetSerializedSize();
			totalSize += dataSize166;
			int dataSize167 = this._combatStatePower.GetSerializedSize();
			totalSize += dataSize167;
			int dataSize168 = this._breakBodyPartInjuryCount.GetSerializedSize();
			totalSize += dataSize168;
			int dataSize169 = this._bodyPartIsBroken.GetSerializedSize();
			totalSize += dataSize169;
			int dataSize170 = this._maxTrickCount.GetSerializedSize();
			totalSize += dataSize170;
			int dataSize171 = this._maxBreathPercent.GetSerializedSize();
			totalSize += dataSize171;
			int dataSize172 = this._maxStancePercent.GetSerializedSize();
			totalSize += dataSize172;
			int dataSize173 = this._extraBreathPercent.GetSerializedSize();
			totalSize += dataSize173;
			int dataSize174 = this._extraStancePercent.GetSerializedSize();
			totalSize += dataSize174;
			int dataSize175 = this._moveCostMobility.GetSerializedSize();
			totalSize += dataSize175;
			int dataSize176 = this._defendSkillKeepTime.GetSerializedSize();
			totalSize += dataSize176;
			int dataSize177 = this._bounceRange.GetSerializedSize();
			totalSize += dataSize177;
			int dataSize178 = this._mindMarkKeepTime.GetSerializedSize();
			totalSize += dataSize178;
			int dataSize179 = this._skillMobilityCostPerFrame.GetSerializedSize();
			totalSize += dataSize179;
			int dataSize180 = this._canAddWug.GetSerializedSize();
			totalSize += dataSize180;
			int dataSize181 = this._hasGodWeaponBuff.GetSerializedSize();
			totalSize += dataSize181;
			int dataSize182 = this._hasGodArmorBuff.GetSerializedSize();
			totalSize += dataSize182;
			int dataSize183 = this._teammateCmdRequireGenerateValue.GetSerializedSize();
			totalSize += dataSize183;
			int dataSize184 = this._teammateCmdEffect.GetSerializedSize();
			totalSize += dataSize184;
			int dataSize185 = this._flawRecoverSpeed.GetSerializedSize();
			totalSize += dataSize185;
			int dataSize186 = this._acupointRecoverSpeed.GetSerializedSize();
			totalSize += dataSize186;
			int dataSize187 = this._mindMarkRecoverSpeed.GetSerializedSize();
			totalSize += dataSize187;
			int dataSize188 = this._injuryAutoHealSpeed.GetSerializedSize();
			totalSize += dataSize188;
			int dataSize189 = this._canRecoverBreath.GetSerializedSize();
			totalSize += dataSize189;
			int dataSize190 = this._canRecoverStance.GetSerializedSize();
			totalSize += dataSize190;
			int dataSize191 = this._fatalDamageValue.GetSerializedSize();
			totalSize += dataSize191;
			int dataSize192 = this._fatalDamageMarkCount.GetSerializedSize();
			totalSize += dataSize192;
			int dataSize193 = this._canFightBackDuringPrepareSkill.GetSerializedSize();
			totalSize += dataSize193;
			int dataSize194 = this._skillPrepareSpeed.GetSerializedSize();
			totalSize += dataSize194;
			int dataSize195 = this._breathRecoverSpeed.GetSerializedSize();
			totalSize += dataSize195;
			int dataSize196 = this._stanceRecoverSpeed.GetSerializedSize();
			totalSize += dataSize196;
			int dataSize197 = this._mobilityRecoverSpeed.GetSerializedSize();
			totalSize += dataSize197;
			int dataSize198 = this._changeTrickProgressAddValue.GetSerializedSize();
			totalSize += dataSize198;
			int dataSize199 = this._power.GetSerializedSize();
			totalSize += dataSize199;
			int dataSize200 = this._maxPower.GetSerializedSize();
			totalSize += dataSize200;
			int dataSize201 = this._powerCanReduce.GetSerializedSize();
			totalSize += dataSize201;
			int dataSize202 = this._useRequirement.GetSerializedSize();
			totalSize += dataSize202;
			int dataSize203 = this._currInnerRatio.GetSerializedSize();
			totalSize += dataSize203;
			int dataSize204 = this._costBreathAndStance.GetSerializedSize();
			totalSize += dataSize204;
			int dataSize205 = this._costBreath.GetSerializedSize();
			totalSize += dataSize205;
			int dataSize206 = this._costStance.GetSerializedSize();
			totalSize += dataSize206;
			int dataSize207 = this._costMobility.GetSerializedSize();
			totalSize += dataSize207;
			int dataSize208 = this._skillCostTricks.GetSerializedSize();
			totalSize += dataSize208;
			int dataSize209 = this._effectDirection.GetSerializedSize();
			totalSize += dataSize209;
			int dataSize210 = this._effectDirectionCanChange.GetSerializedSize();
			totalSize += dataSize210;
			int dataSize211 = this._gridCost.GetSerializedSize();
			totalSize += dataSize211;
			int dataSize212 = this._prepareTotalProgress.GetSerializedSize();
			totalSize += dataSize212;
			int dataSize213 = this._specificGridCount.GetSerializedSize();
			totalSize += dataSize213;
			int dataSize214 = this._genericGridCount.GetSerializedSize();
			totalSize += dataSize214;
			int dataSize215 = this._canInterrupt.GetSerializedSize();
			totalSize += dataSize215;
			int dataSize216 = this._interruptOdds.GetSerializedSize();
			totalSize += dataSize216;
			int dataSize217 = this._canSilence.GetSerializedSize();
			totalSize += dataSize217;
			int dataSize218 = this._silenceOdds.GetSerializedSize();
			totalSize += dataSize218;
			int dataSize219 = this._canCastWithBrokenBodyPart.GetSerializedSize();
			totalSize += dataSize219;
			int dataSize220 = this._addPowerCanBeRemoved.GetSerializedSize();
			totalSize += dataSize220;
			int dataSize221 = this._skillType.GetSerializedSize();
			totalSize += dataSize221;
			int dataSize222 = this._effectCountCanChange.GetSerializedSize();
			totalSize += dataSize222;
			int dataSize223 = this._canCastInDefend.GetSerializedSize();
			totalSize += dataSize223;
			int dataSize224 = this._hitDistribution.GetSerializedSize();
			totalSize += dataSize224;
			int dataSize225 = this._canCastOnLackBreath.GetSerializedSize();
			totalSize += dataSize225;
			int dataSize226 = this._canCastOnLackStance.GetSerializedSize();
			totalSize += dataSize226;
			int dataSize227 = this._costBreathOnCast.GetSerializedSize();
			totalSize += dataSize227;
			int dataSize228 = this._costStanceOnCast.GetSerializedSize();
			totalSize += dataSize228;
			int dataSize229 = this._canUseMobilityAsBreath.GetSerializedSize();
			totalSize += dataSize229;
			int dataSize230 = this._canUseMobilityAsStance.GetSerializedSize();
			totalSize += dataSize230;
			int dataSize231 = this._castCostNeiliAllocation.GetSerializedSize();
			totalSize += dataSize231;
			int dataSize232 = this._acceptPoisonResist.GetSerializedSize();
			totalSize += dataSize232;
			int dataSize233 = this._makePoisonResist.GetSerializedSize();
			totalSize += dataSize233;
			int dataSize234 = this._canCriticalHit.GetSerializedSize();
			totalSize += dataSize234;
			int dataSize235 = this._canCostNeiliAllocationEffect.GetSerializedSize();
			totalSize += dataSize235;
			int dataSize236 = this._consummateLevelRelatedMainAttributesHitValues.GetSerializedSize();
			totalSize += dataSize236;
			int dataSize237 = this._consummateLevelRelatedMainAttributesAvoidValues.GetSerializedSize();
			totalSize += dataSize237;
			int dataSize238 = this._consummateLevelRelatedMainAttributesPenetrations.GetSerializedSize();
			totalSize += dataSize238;
			int dataSize239 = this._consummateLevelRelatedMainAttributesPenetrationResists.GetSerializedSize();
			totalSize += dataSize239;
			int dataSize240 = this._skillAlsoAsFiveElements.GetSerializedSize();
			totalSize += dataSize240;
			int dataSize241 = this._innerInjuryImmunity.GetSerializedSize();
			totalSize += dataSize241;
			int dataSize242 = this._outerInjuryImmunity.GetSerializedSize();
			totalSize += dataSize242;
			int dataSize243 = this._poisonAffectThreshold.GetSerializedSize();
			totalSize += dataSize243;
			int dataSize244 = this._lockDistance.GetSerializedSize();
			totalSize += dataSize244;
			int dataSize245 = this._resistOfAllPoison.GetSerializedSize();
			totalSize += dataSize245;
			int dataSize246 = this._makePoisonTarget.GetSerializedSize();
			totalSize += dataSize246;
			int dataSize247 = this._acceptPoisonTarget.GetSerializedSize();
			totalSize += dataSize247;
			int dataSize248 = this._certainCriticalHit.GetSerializedSize();
			totalSize += dataSize248;
			int dataSize249 = this._mindMarkCount.GetSerializedSize();
			totalSize += dataSize249;
			int dataSize250 = this._canFightBackWithHit.GetSerializedSize();
			totalSize += dataSize250;
			int dataSize251 = this._inevitableHit.GetSerializedSize();
			totalSize += dataSize251;
			int dataSize252 = this._attackCanPursue.GetSerializedSize();
			totalSize += dataSize252;
			int dataSize253 = this._combatSkillDataEffectList.GetSerializedSize();
			totalSize += dataSize253;
			int dataSize254 = this._criticalOdds.GetSerializedSize();
			totalSize += dataSize254;
			int dataSize255 = this._stanceCostByEffect.GetSerializedSize();
			totalSize += dataSize255;
			int dataSize256 = this._breathCostByEffect.GetSerializedSize();
			totalSize += dataSize256;
			int dataSize257 = this._powerAddRatio.GetSerializedSize();
			totalSize += dataSize257;
			int dataSize258 = this._powerReduceRatio.GetSerializedSize();
			totalSize += dataSize258;
			int dataSize259 = this._poisonAffectProduceValue.GetSerializedSize();
			totalSize += dataSize259;
			int dataSize260 = this._canReadingOnMonthChange.GetSerializedSize();
			totalSize += dataSize260;
			int dataSize261 = this._medicineEffect.GetSerializedSize();
			totalSize += dataSize261;
			int dataSize262 = this._xiangshuInfectionDelta.GetSerializedSize();
			totalSize += dataSize262;
			int dataSize263 = this._healthDelta.GetSerializedSize();
			totalSize += dataSize263;
			int dataSize264 = this._weaponSilenceFrame.GetSerializedSize();
			totalSize += dataSize264;
			int dataSize265 = this._silenceFrame.GetSerializedSize();
			totalSize += dataSize265;
			int dataSize266 = this._currAgeDelta.GetSerializedSize();
			totalSize += dataSize266;
			int dataSize267 = this._goneMadInAllBreak.GetSerializedSize();
			totalSize += dataSize267;
			int dataSize268 = this._makeLoveRateOnMonthChange.GetSerializedSize();
			totalSize += dataSize268;
			int dataSize269 = this._canAutoHealOnMonthChange.GetSerializedSize();
			totalSize += dataSize269;
			int dataSize270 = this._happinessDelta.GetSerializedSize();
			totalSize += dataSize270;
			int dataSize271 = this._teammateCmdCanUse.GetSerializedSize();
			totalSize += dataSize271;
			int dataSize272 = this._mixPoisonInfinityAffect.GetSerializedSize();
			totalSize += dataSize272;
			int dataSize273 = this._attackRangeMaxAcupoint.GetSerializedSize();
			totalSize += dataSize273;
			int dataSize274 = this._maxMobilityPercent.GetSerializedSize();
			totalSize += dataSize274;
			int dataSize275 = this._makeMindDamage.GetSerializedSize();
			totalSize += dataSize275;
			int dataSize276 = this._acceptMindDamage.GetSerializedSize();
			totalSize += dataSize276;
			int dataSize277 = this._hitAddByTempValue.GetSerializedSize();
			totalSize += dataSize277;
			int dataSize278 = this._avoidAddByTempValue.GetSerializedSize();
			totalSize += dataSize278;
			int dataSize279 = this._ignoreEquipmentOverload.GetSerializedSize();
			totalSize += dataSize279;
			int dataSize280 = this._canCostEnemyUsableTricks.GetSerializedSize();
			totalSize += dataSize280;
			int dataSize281 = this._ignoreArmor.GetSerializedSize();
			totalSize += dataSize281;
			int dataSize282 = this._unyieldingFallen.GetSerializedSize();
			totalSize += dataSize282;
			int dataSize283 = this._normalAttackPrepareFrame.GetSerializedSize();
			totalSize += dataSize283;
			int dataSize284 = this._canCostUselessTricks.GetSerializedSize();
			totalSize += dataSize284;
			int dataSize285 = this._defendSkillCanAffect.GetSerializedSize();
			totalSize += dataSize285;
			int dataSize286 = this._assistSkillCanAffect.GetSerializedSize();
			totalSize += dataSize286;
			int dataSize287 = this._agileSkillCanAffect.GetSerializedSize();
			totalSize += dataSize287;
			int dataSize288 = this._allMarkChangeToMind.GetSerializedSize();
			totalSize += dataSize288;
			int dataSize289 = this._mindMarkChangeToFatal.GetSerializedSize();
			totalSize += dataSize289;
			int dataSize290 = this._canCast.GetSerializedSize();
			totalSize += dataSize290;
			int dataSize291 = this._inevitableAvoid.GetSerializedSize();
			totalSize += dataSize291;
			int dataSize292 = this._powerEffectReverse.GetSerializedSize();
			totalSize += dataSize292;
			int dataSize293 = this._featureBonusReverse.GetSerializedSize();
			totalSize += dataSize293;
			int dataSize294 = this._wugFatalDamageValue.GetSerializedSize();
			totalSize += dataSize294;
			int dataSize295 = this._canRecoverHealthOnMonthChange.GetSerializedSize();
			totalSize += dataSize295;
			int dataSize296 = this._takeRevengeRateOnMonthChange.GetSerializedSize();
			totalSize += dataSize296;
			int dataSize297 = this._consummateLevelBonus.GetSerializedSize();
			totalSize += dataSize297;
			int dataSize298 = this._neiliDelta.GetSerializedSize();
			totalSize += dataSize298;
			int dataSize299 = this._canMakeLoveSpecialOnMonthChange.GetSerializedSize();
			totalSize += dataSize299;
			int dataSize300 = this._healAcupointSpeed.GetSerializedSize();
			totalSize += dataSize300;
			int dataSize301 = this._maxChangeTrickCount.GetSerializedSize();
			totalSize += dataSize301;
			int dataSize302 = this._convertCostBreathAndStance.GetSerializedSize();
			totalSize += dataSize302;
			int dataSize303 = this._personalitiesAll.GetSerializedSize();
			totalSize += dataSize303;
			int dataSize304 = this._finalFatalDamageMarkCount.GetSerializedSize();
			totalSize += dataSize304;
			int dataSize305 = this._infinityMindMarkProgress.GetSerializedSize();
			totalSize += dataSize305;
			int dataSize306 = this._combatSkillAiScorePower.GetSerializedSize();
			totalSize += dataSize306;
			int dataSize307 = this._normalAttackChangeToUnlockAttack.GetSerializedSize();
			totalSize += dataSize307;
			int dataSize308 = this._attackBodyPartOdds.GetSerializedSize();
			totalSize += dataSize308;
			int dataSize309 = this._changeDurability.GetSerializedSize();
			totalSize += dataSize309;
			int dataSize310 = this._equipmentBonus.GetSerializedSize();
			totalSize += dataSize310;
			int dataSize311 = this._equipmentWeight.GetSerializedSize();
			totalSize += dataSize311;
			int dataSize312 = this._rawCreateEffectList.GetSerializedSize();
			totalSize += dataSize312;
			int dataSize313 = this._jiTrickAsWeaponTrickCount.GetSerializedSize();
			totalSize += dataSize313;
			int dataSize314 = this._uselessTrickAsJiTrickCount.GetSerializedSize();
			totalSize += dataSize314;
			int dataSize315 = this._equipmentPower.GetSerializedSize();
			totalSize += dataSize315;
			int dataSize316 = this._healFlawSpeed.GetSerializedSize();
			totalSize += dataSize316;
			int dataSize317 = this._unlockSpeed.GetSerializedSize();
			totalSize += dataSize317;
			int dataSize318 = this._flawBonusFactor.GetSerializedSize();
			totalSize += dataSize318;
			int dataSize319 = this._canCostShaTricks.GetSerializedSize();
			totalSize += dataSize319;
			int dataSize320 = this._defenderDirectFinalDamageValue.GetSerializedSize();
			totalSize += dataSize320;
			int dataSize321 = this._normalAttackRecoveryFrame.GetSerializedSize();
			totalSize += dataSize321;
			int dataSize322 = this._finalGoneMadInjury.GetSerializedSize();
			totalSize += dataSize322;
			int dataSize323 = this._attackerDirectFinalDamageValue.GetSerializedSize();
			totalSize += dataSize323;
			int dataSize324 = this._canCostTrickDuringPreparingSkill.GetSerializedSize();
			totalSize += dataSize324;
			int dataSize325 = this._validItemList.GetSerializedSize();
			totalSize += dataSize325;
			int dataSize326 = this._acceptDamageCanAdd.GetSerializedSize();
			totalSize += dataSize326;
			int dataSize327 = this._makeDamageCanReduce.GetSerializedSize();
			totalSize += dataSize327;
			int dataSize328 = this._normalAttackGetTrickCount.GetSerializedSize();
			return totalSize + dataSize328;
		}

		// Token: 0x06002868 RID: 10344 RVA: 0x001E08DC File Offset: 0x001DEADC
		public unsafe int Serialize(byte* pData)
		{
			*(int*)pData = this._id;
			byte* pCurrData = pData + 4;
			byte* pBegin = pCurrData;
			pCurrData += 4;
			pCurrData += this._maxStrength.Serialize(pCurrData);
			int fieldSize = (int)((long)(pCurrData - pBegin) - 4L);
			bool flag = fieldSize > 4194304;
			if (flag)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Size of field ");
				defaultInterpolatedStringHandler.AppendFormatted("_maxStrength");
				defaultInterpolatedStringHandler.AppendLiteral(" must be less than ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(4096);
				defaultInterpolatedStringHandler.AppendLiteral("KB");
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			*(int*)pBegin = fieldSize;
			byte* pBegin2 = pCurrData;
			pCurrData += 4;
			pCurrData += this._maxDexterity.Serialize(pCurrData);
			int fieldSize2 = (int)((long)(pCurrData - pBegin2) - 4L);
			bool flag2 = fieldSize2 > 4194304;
			if (flag2)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Size of field ");
				defaultInterpolatedStringHandler.AppendFormatted("_maxDexterity");
				defaultInterpolatedStringHandler.AppendLiteral(" must be less than ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(4096);
				defaultInterpolatedStringHandler.AppendLiteral("KB");
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			*(int*)pBegin2 = fieldSize2;
			byte* pBegin3 = pCurrData;
			pCurrData += 4;
			pCurrData += this._maxConcentration.Serialize(pCurrData);
			int fieldSize3 = (int)((long)(pCurrData - pBegin3) - 4L);
			bool flag3 = fieldSize3 > 4194304;
			if (flag3)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Size of field ");
				defaultInterpolatedStringHandler.AppendFormatted("_maxConcentration");
				defaultInterpolatedStringHandler.AppendLiteral(" must be less than ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(4096);
				defaultInterpolatedStringHandler.AppendLiteral("KB");
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			*(int*)pBegin3 = fieldSize3;
			byte* pBegin4 = pCurrData;
			pCurrData += 4;
			pCurrData += this._maxVitality.Serialize(pCurrData);
			int fieldSize4 = (int)((long)(pCurrData - pBegin4) - 4L);
			bool flag4 = fieldSize4 > 4194304;
			if (flag4)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Size of field ");
				defaultInterpolatedStringHandler.AppendFormatted("_maxVitality");
				defaultInterpolatedStringHandler.AppendLiteral(" must be less than ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(4096);
				defaultInterpolatedStringHandler.AppendLiteral("KB");
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			*(int*)pBegin4 = fieldSize4;
			byte* pBegin5 = pCurrData;
			pCurrData += 4;
			pCurrData += this._maxEnergy.Serialize(pCurrData);
			int fieldSize5 = (int)((long)(pCurrData - pBegin5) - 4L);
			bool flag5 = fieldSize5 > 4194304;
			if (flag5)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Size of field ");
				defaultInterpolatedStringHandler.AppendFormatted("_maxEnergy");
				defaultInterpolatedStringHandler.AppendLiteral(" must be less than ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(4096);
				defaultInterpolatedStringHandler.AppendLiteral("KB");
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			*(int*)pBegin5 = fieldSize5;
			byte* pBegin6 = pCurrData;
			pCurrData += 4;
			pCurrData += this._maxIntelligence.Serialize(pCurrData);
			int fieldSize6 = (int)((long)(pCurrData - pBegin6) - 4L);
			bool flag6 = fieldSize6 > 4194304;
			if (flag6)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Size of field ");
				defaultInterpolatedStringHandler.AppendFormatted("_maxIntelligence");
				defaultInterpolatedStringHandler.AppendLiteral(" must be less than ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(4096);
				defaultInterpolatedStringHandler.AppendLiteral("KB");
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			*(int*)pBegin6 = fieldSize6;
			byte* pBegin7 = pCurrData;
			pCurrData += 4;
			pCurrData += this._recoveryOfStance.Serialize(pCurrData);
			int fieldSize7 = (int)((long)(pCurrData - pBegin7) - 4L);
			bool flag7 = fieldSize7 > 4194304;
			if (flag7)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Size of field ");
				defaultInterpolatedStringHandler.AppendFormatted("_recoveryOfStance");
				defaultInterpolatedStringHandler.AppendLiteral(" must be less than ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(4096);
				defaultInterpolatedStringHandler.AppendLiteral("KB");
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			*(int*)pBegin7 = fieldSize7;
			byte* pBegin8 = pCurrData;
			pCurrData += 4;
			pCurrData += this._recoveryOfBreath.Serialize(pCurrData);
			int fieldSize8 = (int)((long)(pCurrData - pBegin8) - 4L);
			bool flag8 = fieldSize8 > 4194304;
			if (flag8)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Size of field ");
				defaultInterpolatedStringHandler.AppendFormatted("_recoveryOfBreath");
				defaultInterpolatedStringHandler.AppendLiteral(" must be less than ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(4096);
				defaultInterpolatedStringHandler.AppendLiteral("KB");
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			*(int*)pBegin8 = fieldSize8;
			byte* pBegin9 = pCurrData;
			pCurrData += 4;
			pCurrData += this._moveSpeed.Serialize(pCurrData);
			int fieldSize9 = (int)((long)(pCurrData - pBegin9) - 4L);
			bool flag9 = fieldSize9 > 4194304;
			if (flag9)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Size of field ");
				defaultInterpolatedStringHandler.AppendFormatted("_moveSpeed");
				defaultInterpolatedStringHandler.AppendLiteral(" must be less than ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(4096);
				defaultInterpolatedStringHandler.AppendLiteral("KB");
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			*(int*)pBegin9 = fieldSize9;
			byte* pBegin10 = pCurrData;
			pCurrData += 4;
			pCurrData += this._recoveryOfFlaw.Serialize(pCurrData);
			int fieldSize10 = (int)((long)(pCurrData - pBegin10) - 4L);
			bool flag10 = fieldSize10 > 4194304;
			if (flag10)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Size of field ");
				defaultInterpolatedStringHandler.AppendFormatted("_recoveryOfFlaw");
				defaultInterpolatedStringHandler.AppendLiteral(" must be less than ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(4096);
				defaultInterpolatedStringHandler.AppendLiteral("KB");
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			*(int*)pBegin10 = fieldSize10;
			byte* pBegin11 = pCurrData;
			pCurrData += 4;
			pCurrData += this._castSpeed.Serialize(pCurrData);
			int fieldSize11 = (int)((long)(pCurrData - pBegin11) - 4L);
			bool flag11 = fieldSize11 > 4194304;
			if (flag11)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Size of field ");
				defaultInterpolatedStringHandler.AppendFormatted("_castSpeed");
				defaultInterpolatedStringHandler.AppendLiteral(" must be less than ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(4096);
				defaultInterpolatedStringHandler.AppendLiteral("KB");
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			*(int*)pBegin11 = fieldSize11;
			byte* pBegin12 = pCurrData;
			pCurrData += 4;
			pCurrData += this._recoveryOfBlockedAcupoint.Serialize(pCurrData);
			int fieldSize12 = (int)((long)(pCurrData - pBegin12) - 4L);
			bool flag12 = fieldSize12 > 4194304;
			if (flag12)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Size of field ");
				defaultInterpolatedStringHandler.AppendFormatted("_recoveryOfBlockedAcupoint");
				defaultInterpolatedStringHandler.AppendLiteral(" must be less than ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(4096);
				defaultInterpolatedStringHandler.AppendLiteral("KB");
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			*(int*)pBegin12 = fieldSize12;
			byte* pBegin13 = pCurrData;
			pCurrData += 4;
			pCurrData += this._weaponSwitchSpeed.Serialize(pCurrData);
			int fieldSize13 = (int)((long)(pCurrData - pBegin13) - 4L);
			bool flag13 = fieldSize13 > 4194304;
			if (flag13)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Size of field ");
				defaultInterpolatedStringHandler.AppendFormatted("_weaponSwitchSpeed");
				defaultInterpolatedStringHandler.AppendLiteral(" must be less than ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(4096);
				defaultInterpolatedStringHandler.AppendLiteral("KB");
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			*(int*)pBegin13 = fieldSize13;
			byte* pBegin14 = pCurrData;
			pCurrData += 4;
			pCurrData += this._attackSpeed.Serialize(pCurrData);
			int fieldSize14 = (int)((long)(pCurrData - pBegin14) - 4L);
			bool flag14 = fieldSize14 > 4194304;
			if (flag14)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Size of field ");
				defaultInterpolatedStringHandler.AppendFormatted("_attackSpeed");
				defaultInterpolatedStringHandler.AppendLiteral(" must be less than ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(4096);
				defaultInterpolatedStringHandler.AppendLiteral("KB");
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			*(int*)pBegin14 = fieldSize14;
			byte* pBegin15 = pCurrData;
			pCurrData += 4;
			pCurrData += this._innerRatio.Serialize(pCurrData);
			int fieldSize15 = (int)((long)(pCurrData - pBegin15) - 4L);
			bool flag15 = fieldSize15 > 4194304;
			if (flag15)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Size of field ");
				defaultInterpolatedStringHandler.AppendFormatted("_innerRatio");
				defaultInterpolatedStringHandler.AppendLiteral(" must be less than ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(4096);
				defaultInterpolatedStringHandler.AppendLiteral("KB");
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			*(int*)pBegin15 = fieldSize15;
			byte* pBegin16 = pCurrData;
			pCurrData += 4;
			pCurrData += this._recoveryOfQiDisorder.Serialize(pCurrData);
			int fieldSize16 = (int)((long)(pCurrData - pBegin16) - 4L);
			bool flag16 = fieldSize16 > 4194304;
			if (flag16)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Size of field ");
				defaultInterpolatedStringHandler.AppendFormatted("_recoveryOfQiDisorder");
				defaultInterpolatedStringHandler.AppendLiteral(" must be less than ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(4096);
				defaultInterpolatedStringHandler.AppendLiteral("KB");
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			*(int*)pBegin16 = fieldSize16;
			byte* pBegin17 = pCurrData;
			pCurrData += 4;
			pCurrData += this._minorAttributeFixMaxValue.Serialize(pCurrData);
			int fieldSize17 = (int)((long)(pCurrData - pBegin17) - 4L);
			bool flag17 = fieldSize17 > 4194304;
			if (flag17)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Size of field ");
				defaultInterpolatedStringHandler.AppendFormatted("_minorAttributeFixMaxValue");
				defaultInterpolatedStringHandler.AppendLiteral(" must be less than ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(4096);
				defaultInterpolatedStringHandler.AppendLiteral("KB");
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			*(int*)pBegin17 = fieldSize17;
			byte* pBegin18 = pCurrData;
			pCurrData += 4;
			pCurrData += this._minorAttributeFixMinValue.Serialize(pCurrData);
			int fieldSize18 = (int)((long)(pCurrData - pBegin18) - 4L);
			bool flag18 = fieldSize18 > 4194304;
			if (flag18)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Size of field ");
				defaultInterpolatedStringHandler.AppendFormatted("_minorAttributeFixMinValue");
				defaultInterpolatedStringHandler.AppendLiteral(" must be less than ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(4096);
				defaultInterpolatedStringHandler.AppendLiteral("KB");
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			*(int*)pBegin18 = fieldSize18;
			byte* pBegin19 = pCurrData;
			pCurrData += 4;
			pCurrData += this._resistOfHotPoison.Serialize(pCurrData);
			int fieldSize19 = (int)((long)(pCurrData - pBegin19) - 4L);
			bool flag19 = fieldSize19 > 4194304;
			if (flag19)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Size of field ");
				defaultInterpolatedStringHandler.AppendFormatted("_resistOfHotPoison");
				defaultInterpolatedStringHandler.AppendLiteral(" must be less than ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(4096);
				defaultInterpolatedStringHandler.AppendLiteral("KB");
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			*(int*)pBegin19 = fieldSize19;
			byte* pBegin20 = pCurrData;
			pCurrData += 4;
			pCurrData += this._resistOfGloomyPoison.Serialize(pCurrData);
			int fieldSize20 = (int)((long)(pCurrData - pBegin20) - 4L);
			bool flag20 = fieldSize20 > 4194304;
			if (flag20)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Size of field ");
				defaultInterpolatedStringHandler.AppendFormatted("_resistOfGloomyPoison");
				defaultInterpolatedStringHandler.AppendLiteral(" must be less than ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(4096);
				defaultInterpolatedStringHandler.AppendLiteral("KB");
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			*(int*)pBegin20 = fieldSize20;
			byte* pBegin21 = pCurrData;
			pCurrData += 4;
			pCurrData += this._resistOfColdPoison.Serialize(pCurrData);
			int fieldSize21 = (int)((long)(pCurrData - pBegin21) - 4L);
			bool flag21 = fieldSize21 > 4194304;
			if (flag21)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Size of field ");
				defaultInterpolatedStringHandler.AppendFormatted("_resistOfColdPoison");
				defaultInterpolatedStringHandler.AppendLiteral(" must be less than ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(4096);
				defaultInterpolatedStringHandler.AppendLiteral("KB");
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			*(int*)pBegin21 = fieldSize21;
			byte* pBegin22 = pCurrData;
			pCurrData += 4;
			pCurrData += this._resistOfRedPoison.Serialize(pCurrData);
			int fieldSize22 = (int)((long)(pCurrData - pBegin22) - 4L);
			bool flag22 = fieldSize22 > 4194304;
			if (flag22)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Size of field ");
				defaultInterpolatedStringHandler.AppendFormatted("_resistOfRedPoison");
				defaultInterpolatedStringHandler.AppendLiteral(" must be less than ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(4096);
				defaultInterpolatedStringHandler.AppendLiteral("KB");
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			*(int*)pBegin22 = fieldSize22;
			byte* pBegin23 = pCurrData;
			pCurrData += 4;
			pCurrData += this._resistOfRottenPoison.Serialize(pCurrData);
			int fieldSize23 = (int)((long)(pCurrData - pBegin23) - 4L);
			bool flag23 = fieldSize23 > 4194304;
			if (flag23)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Size of field ");
				defaultInterpolatedStringHandler.AppendFormatted("_resistOfRottenPoison");
				defaultInterpolatedStringHandler.AppendLiteral(" must be less than ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(4096);
				defaultInterpolatedStringHandler.AppendLiteral("KB");
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			*(int*)pBegin23 = fieldSize23;
			byte* pBegin24 = pCurrData;
			pCurrData += 4;
			pCurrData += this._resistOfIllusoryPoison.Serialize(pCurrData);
			int fieldSize24 = (int)((long)(pCurrData - pBegin24) - 4L);
			bool flag24 = fieldSize24 > 4194304;
			if (flag24)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Size of field ");
				defaultInterpolatedStringHandler.AppendFormatted("_resistOfIllusoryPoison");
				defaultInterpolatedStringHandler.AppendLiteral(" must be less than ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(4096);
				defaultInterpolatedStringHandler.AppendLiteral("KB");
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			*(int*)pBegin24 = fieldSize24;
			byte* pBegin25 = pCurrData;
			pCurrData += 4;
			pCurrData += this._displayAge.Serialize(pCurrData);
			int fieldSize25 = (int)((long)(pCurrData - pBegin25) - 4L);
			bool flag25 = fieldSize25 > 4194304;
			if (flag25)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Size of field ");
				defaultInterpolatedStringHandler.AppendFormatted("_displayAge");
				defaultInterpolatedStringHandler.AppendLiteral(" must be less than ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(4096);
				defaultInterpolatedStringHandler.AppendLiteral("KB");
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			*(int*)pBegin25 = fieldSize25;
			byte* pBegin26 = pCurrData;
			pCurrData += 4;
			pCurrData += this._neiliProportionOfFiveElements.Serialize(pCurrData);
			int fieldSize26 = (int)((long)(pCurrData - pBegin26) - 4L);
			bool flag26 = fieldSize26 > 4194304;
			if (flag26)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Size of field ");
				defaultInterpolatedStringHandler.AppendFormatted("_neiliProportionOfFiveElements");
				defaultInterpolatedStringHandler.AppendLiteral(" must be less than ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(4096);
				defaultInterpolatedStringHandler.AppendLiteral("KB");
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			*(int*)pBegin26 = fieldSize26;
			byte* pBegin27 = pCurrData;
			pCurrData += 4;
			pCurrData += this._weaponMaxPower.Serialize(pCurrData);
			int fieldSize27 = (int)((long)(pCurrData - pBegin27) - 4L);
			bool flag27 = fieldSize27 > 4194304;
			if (flag27)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Size of field ");
				defaultInterpolatedStringHandler.AppendFormatted("_weaponMaxPower");
				defaultInterpolatedStringHandler.AppendLiteral(" must be less than ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(4096);
				defaultInterpolatedStringHandler.AppendLiteral("KB");
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			*(int*)pBegin27 = fieldSize27;
			byte* pBegin28 = pCurrData;
			pCurrData += 4;
			pCurrData += this._weaponUseRequirement.Serialize(pCurrData);
			int fieldSize28 = (int)((long)(pCurrData - pBegin28) - 4L);
			bool flag28 = fieldSize28 > 4194304;
			if (flag28)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Size of field ");
				defaultInterpolatedStringHandler.AppendFormatted("_weaponUseRequirement");
				defaultInterpolatedStringHandler.AppendLiteral(" must be less than ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(4096);
				defaultInterpolatedStringHandler.AppendLiteral("KB");
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			*(int*)pBegin28 = fieldSize28;
			byte* pBegin29 = pCurrData;
			pCurrData += 4;
			pCurrData += this._weaponAttackRange.Serialize(pCurrData);
			int fieldSize29 = (int)((long)(pCurrData - pBegin29) - 4L);
			bool flag29 = fieldSize29 > 4194304;
			if (flag29)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Size of field ");
				defaultInterpolatedStringHandler.AppendFormatted("_weaponAttackRange");
				defaultInterpolatedStringHandler.AppendLiteral(" must be less than ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(4096);
				defaultInterpolatedStringHandler.AppendLiteral("KB");
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			*(int*)pBegin29 = fieldSize29;
			byte* pBegin30 = pCurrData;
			pCurrData += 4;
			pCurrData += this._armorMaxPower.Serialize(pCurrData);
			int fieldSize30 = (int)((long)(pCurrData - pBegin30) - 4L);
			bool flag30 = fieldSize30 > 4194304;
			if (flag30)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Size of field ");
				defaultInterpolatedStringHandler.AppendFormatted("_armorMaxPower");
				defaultInterpolatedStringHandler.AppendLiteral(" must be less than ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(4096);
				defaultInterpolatedStringHandler.AppendLiteral("KB");
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			*(int*)pBegin30 = fieldSize30;
			byte* pBegin31 = pCurrData;
			pCurrData += 4;
			pCurrData += this._armorUseRequirement.Serialize(pCurrData);
			int fieldSize31 = (int)((long)(pCurrData - pBegin31) - 4L);
			bool flag31 = fieldSize31 > 4194304;
			if (flag31)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Size of field ");
				defaultInterpolatedStringHandler.AppendFormatted("_armorUseRequirement");
				defaultInterpolatedStringHandler.AppendLiteral(" must be less than ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(4096);
				defaultInterpolatedStringHandler.AppendLiteral("KB");
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			*(int*)pBegin31 = fieldSize31;
			byte* pBegin32 = pCurrData;
			pCurrData += 4;
			pCurrData += this._hitStrength.Serialize(pCurrData);
			int fieldSize32 = (int)((long)(pCurrData - pBegin32) - 4L);
			bool flag32 = fieldSize32 > 4194304;
			if (flag32)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Size of field ");
				defaultInterpolatedStringHandler.AppendFormatted("_hitStrength");
				defaultInterpolatedStringHandler.AppendLiteral(" must be less than ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(4096);
				defaultInterpolatedStringHandler.AppendLiteral("KB");
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			*(int*)pBegin32 = fieldSize32;
			byte* pBegin33 = pCurrData;
			pCurrData += 4;
			pCurrData += this._hitTechnique.Serialize(pCurrData);
			int fieldSize33 = (int)((long)(pCurrData - pBegin33) - 4L);
			bool flag33 = fieldSize33 > 4194304;
			if (flag33)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Size of field ");
				defaultInterpolatedStringHandler.AppendFormatted("_hitTechnique");
				defaultInterpolatedStringHandler.AppendLiteral(" must be less than ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(4096);
				defaultInterpolatedStringHandler.AppendLiteral("KB");
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			*(int*)pBegin33 = fieldSize33;
			byte* pBegin34 = pCurrData;
			pCurrData += 4;
			pCurrData += this._hitSpeed.Serialize(pCurrData);
			int fieldSize34 = (int)((long)(pCurrData - pBegin34) - 4L);
			bool flag34 = fieldSize34 > 4194304;
			if (flag34)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Size of field ");
				defaultInterpolatedStringHandler.AppendFormatted("_hitSpeed");
				defaultInterpolatedStringHandler.AppendLiteral(" must be less than ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(4096);
				defaultInterpolatedStringHandler.AppendLiteral("KB");
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			*(int*)pBegin34 = fieldSize34;
			byte* pBegin35 = pCurrData;
			pCurrData += 4;
			pCurrData += this._hitMind.Serialize(pCurrData);
			int fieldSize35 = (int)((long)(pCurrData - pBegin35) - 4L);
			bool flag35 = fieldSize35 > 4194304;
			if (flag35)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Size of field ");
				defaultInterpolatedStringHandler.AppendFormatted("_hitMind");
				defaultInterpolatedStringHandler.AppendLiteral(" must be less than ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(4096);
				defaultInterpolatedStringHandler.AppendLiteral("KB");
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			*(int*)pBegin35 = fieldSize35;
			byte* pBegin36 = pCurrData;
			pCurrData += 4;
			pCurrData += this._hitCanChange.Serialize(pCurrData);
			int fieldSize36 = (int)((long)(pCurrData - pBegin36) - 4L);
			bool flag36 = fieldSize36 > 4194304;
			if (flag36)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Size of field ");
				defaultInterpolatedStringHandler.AppendFormatted("_hitCanChange");
				defaultInterpolatedStringHandler.AppendLiteral(" must be less than ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(4096);
				defaultInterpolatedStringHandler.AppendLiteral("KB");
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			*(int*)pBegin36 = fieldSize36;
			byte* pBegin37 = pCurrData;
			pCurrData += 4;
			pCurrData += this._hitChangeEffectPercent.Serialize(pCurrData);
			int fieldSize37 = (int)((long)(pCurrData - pBegin37) - 4L);
			bool flag37 = fieldSize37 > 4194304;
			if (flag37)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Size of field ");
				defaultInterpolatedStringHandler.AppendFormatted("_hitChangeEffectPercent");
				defaultInterpolatedStringHandler.AppendLiteral(" must be less than ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(4096);
				defaultInterpolatedStringHandler.AppendLiteral("KB");
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			*(int*)pBegin37 = fieldSize37;
			byte* pBegin38 = pCurrData;
			pCurrData += 4;
			pCurrData += this._avoidStrength.Serialize(pCurrData);
			int fieldSize38 = (int)((long)(pCurrData - pBegin38) - 4L);
			bool flag38 = fieldSize38 > 4194304;
			if (flag38)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Size of field ");
				defaultInterpolatedStringHandler.AppendFormatted("_avoidStrength");
				defaultInterpolatedStringHandler.AppendLiteral(" must be less than ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(4096);
				defaultInterpolatedStringHandler.AppendLiteral("KB");
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			*(int*)pBegin38 = fieldSize38;
			byte* pBegin39 = pCurrData;
			pCurrData += 4;
			pCurrData += this._avoidTechnique.Serialize(pCurrData);
			int fieldSize39 = (int)((long)(pCurrData - pBegin39) - 4L);
			bool flag39 = fieldSize39 > 4194304;
			if (flag39)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Size of field ");
				defaultInterpolatedStringHandler.AppendFormatted("_avoidTechnique");
				defaultInterpolatedStringHandler.AppendLiteral(" must be less than ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(4096);
				defaultInterpolatedStringHandler.AppendLiteral("KB");
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			*(int*)pBegin39 = fieldSize39;
			byte* pBegin40 = pCurrData;
			pCurrData += 4;
			pCurrData += this._avoidSpeed.Serialize(pCurrData);
			int fieldSize40 = (int)((long)(pCurrData - pBegin40) - 4L);
			bool flag40 = fieldSize40 > 4194304;
			if (flag40)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Size of field ");
				defaultInterpolatedStringHandler.AppendFormatted("_avoidSpeed");
				defaultInterpolatedStringHandler.AppendLiteral(" must be less than ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(4096);
				defaultInterpolatedStringHandler.AppendLiteral("KB");
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			*(int*)pBegin40 = fieldSize40;
			byte* pBegin41 = pCurrData;
			pCurrData += 4;
			pCurrData += this._avoidMind.Serialize(pCurrData);
			int fieldSize41 = (int)((long)(pCurrData - pBegin41) - 4L);
			bool flag41 = fieldSize41 > 4194304;
			if (flag41)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Size of field ");
				defaultInterpolatedStringHandler.AppendFormatted("_avoidMind");
				defaultInterpolatedStringHandler.AppendLiteral(" must be less than ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(4096);
				defaultInterpolatedStringHandler.AppendLiteral("KB");
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			*(int*)pBegin41 = fieldSize41;
			byte* pBegin42 = pCurrData;
			pCurrData += 4;
			pCurrData += this._avoidCanChange.Serialize(pCurrData);
			int fieldSize42 = (int)((long)(pCurrData - pBegin42) - 4L);
			bool flag42 = fieldSize42 > 4194304;
			if (flag42)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Size of field ");
				defaultInterpolatedStringHandler.AppendFormatted("_avoidCanChange");
				defaultInterpolatedStringHandler.AppendLiteral(" must be less than ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(4096);
				defaultInterpolatedStringHandler.AppendLiteral("KB");
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			*(int*)pBegin42 = fieldSize42;
			byte* pBegin43 = pCurrData;
			pCurrData += 4;
			pCurrData += this._avoidChangeEffectPercent.Serialize(pCurrData);
			int fieldSize43 = (int)((long)(pCurrData - pBegin43) - 4L);
			bool flag43 = fieldSize43 > 4194304;
			if (flag43)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Size of field ");
				defaultInterpolatedStringHandler.AppendFormatted("_avoidChangeEffectPercent");
				defaultInterpolatedStringHandler.AppendLiteral(" must be less than ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(4096);
				defaultInterpolatedStringHandler.AppendLiteral("KB");
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			*(int*)pBegin43 = fieldSize43;
			byte* pBegin44 = pCurrData;
			pCurrData += 4;
			pCurrData += this._penetrateOuter.Serialize(pCurrData);
			int fieldSize44 = (int)((long)(pCurrData - pBegin44) - 4L);
			bool flag44 = fieldSize44 > 4194304;
			if (flag44)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Size of field ");
				defaultInterpolatedStringHandler.AppendFormatted("_penetrateOuter");
				defaultInterpolatedStringHandler.AppendLiteral(" must be less than ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(4096);
				defaultInterpolatedStringHandler.AppendLiteral("KB");
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			*(int*)pBegin44 = fieldSize44;
			byte* pBegin45 = pCurrData;
			pCurrData += 4;
			pCurrData += this._penetrateInner.Serialize(pCurrData);
			int fieldSize45 = (int)((long)(pCurrData - pBegin45) - 4L);
			bool flag45 = fieldSize45 > 4194304;
			if (flag45)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Size of field ");
				defaultInterpolatedStringHandler.AppendFormatted("_penetrateInner");
				defaultInterpolatedStringHandler.AppendLiteral(" must be less than ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(4096);
				defaultInterpolatedStringHandler.AppendLiteral("KB");
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			*(int*)pBegin45 = fieldSize45;
			byte* pBegin46 = pCurrData;
			pCurrData += 4;
			pCurrData += this._penetrateResistOuter.Serialize(pCurrData);
			int fieldSize46 = (int)((long)(pCurrData - pBegin46) - 4L);
			bool flag46 = fieldSize46 > 4194304;
			if (flag46)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Size of field ");
				defaultInterpolatedStringHandler.AppendFormatted("_penetrateResistOuter");
				defaultInterpolatedStringHandler.AppendLiteral(" must be less than ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(4096);
				defaultInterpolatedStringHandler.AppendLiteral("KB");
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			*(int*)pBegin46 = fieldSize46;
			byte* pBegin47 = pCurrData;
			pCurrData += 4;
			pCurrData += this._penetrateResistInner.Serialize(pCurrData);
			int fieldSize47 = (int)((long)(pCurrData - pBegin47) - 4L);
			bool flag47 = fieldSize47 > 4194304;
			if (flag47)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Size of field ");
				defaultInterpolatedStringHandler.AppendFormatted("_penetrateResistInner");
				defaultInterpolatedStringHandler.AppendLiteral(" must be less than ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(4096);
				defaultInterpolatedStringHandler.AppendLiteral("KB");
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			*(int*)pBegin47 = fieldSize47;
			byte* pBegin48 = pCurrData;
			pCurrData += 4;
			pCurrData += this._neiliAllocationAttack.Serialize(pCurrData);
			int fieldSize48 = (int)((long)(pCurrData - pBegin48) - 4L);
			bool flag48 = fieldSize48 > 4194304;
			if (flag48)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Size of field ");
				defaultInterpolatedStringHandler.AppendFormatted("_neiliAllocationAttack");
				defaultInterpolatedStringHandler.AppendLiteral(" must be less than ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(4096);
				defaultInterpolatedStringHandler.AppendLiteral("KB");
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			*(int*)pBegin48 = fieldSize48;
			byte* pBegin49 = pCurrData;
			pCurrData += 4;
			pCurrData += this._neiliAllocationAgile.Serialize(pCurrData);
			int fieldSize49 = (int)((long)(pCurrData - pBegin49) - 4L);
			bool flag49 = fieldSize49 > 4194304;
			if (flag49)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Size of field ");
				defaultInterpolatedStringHandler.AppendFormatted("_neiliAllocationAgile");
				defaultInterpolatedStringHandler.AppendLiteral(" must be less than ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(4096);
				defaultInterpolatedStringHandler.AppendLiteral("KB");
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			*(int*)pBegin49 = fieldSize49;
			byte* pBegin50 = pCurrData;
			pCurrData += 4;
			pCurrData += this._neiliAllocationDefense.Serialize(pCurrData);
			int fieldSize50 = (int)((long)(pCurrData - pBegin50) - 4L);
			bool flag50 = fieldSize50 > 4194304;
			if (flag50)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Size of field ");
				defaultInterpolatedStringHandler.AppendFormatted("_neiliAllocationDefense");
				defaultInterpolatedStringHandler.AppendLiteral(" must be less than ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(4096);
				defaultInterpolatedStringHandler.AppendLiteral("KB");
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			*(int*)pBegin50 = fieldSize50;
			byte* pBegin51 = pCurrData;
			pCurrData += 4;
			pCurrData += this._neiliAllocationAssist.Serialize(pCurrData);
			int fieldSize51 = (int)((long)(pCurrData - pBegin51) - 4L);
			bool flag51 = fieldSize51 > 4194304;
			if (flag51)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Size of field ");
				defaultInterpolatedStringHandler.AppendFormatted("_neiliAllocationAssist");
				defaultInterpolatedStringHandler.AppendLiteral(" must be less than ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(4096);
				defaultInterpolatedStringHandler.AppendLiteral("KB");
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			*(int*)pBegin51 = fieldSize51;
			byte* pBegin52 = pCurrData;
			pCurrData += 4;
			pCurrData += this._happiness.Serialize(pCurrData);
			int fieldSize52 = (int)((long)(pCurrData - pBegin52) - 4L);
			bool flag52 = fieldSize52 > 4194304;
			if (flag52)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Size of field ");
				defaultInterpolatedStringHandler.AppendFormatted("_happiness");
				defaultInterpolatedStringHandler.AppendLiteral(" must be less than ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(4096);
				defaultInterpolatedStringHandler.AppendLiteral("KB");
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			*(int*)pBegin52 = fieldSize52;
			byte* pBegin53 = pCurrData;
			pCurrData += 4;
			pCurrData += this._maxHealth.Serialize(pCurrData);
			int fieldSize53 = (int)((long)(pCurrData - pBegin53) - 4L);
			bool flag53 = fieldSize53 > 4194304;
			if (flag53)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Size of field ");
				defaultInterpolatedStringHandler.AppendFormatted("_maxHealth");
				defaultInterpolatedStringHandler.AppendLiteral(" must be less than ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(4096);
				defaultInterpolatedStringHandler.AppendLiteral("KB");
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			*(int*)pBegin53 = fieldSize53;
			byte* pBegin54 = pCurrData;
			pCurrData += 4;
			pCurrData += this._healthCost.Serialize(pCurrData);
			int fieldSize54 = (int)((long)(pCurrData - pBegin54) - 4L);
			bool flag54 = fieldSize54 > 4194304;
			if (flag54)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Size of field ");
				defaultInterpolatedStringHandler.AppendFormatted("_healthCost");
				defaultInterpolatedStringHandler.AppendLiteral(" must be less than ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(4096);
				defaultInterpolatedStringHandler.AppendLiteral("KB");
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			*(int*)pBegin54 = fieldSize54;
			byte* pBegin55 = pCurrData;
			pCurrData += 4;
			pCurrData += this._moveSpeedCanChange.Serialize(pCurrData);
			int fieldSize55 = (int)((long)(pCurrData - pBegin55) - 4L);
			bool flag55 = fieldSize55 > 4194304;
			if (flag55)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Size of field ");
				defaultInterpolatedStringHandler.AppendFormatted("_moveSpeedCanChange");
				defaultInterpolatedStringHandler.AppendLiteral(" must be less than ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(4096);
				defaultInterpolatedStringHandler.AppendLiteral("KB");
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			*(int*)pBegin55 = fieldSize55;
			byte* pBegin56 = pCurrData;
			pCurrData += 4;
			pCurrData += this._attackerHitStrength.Serialize(pCurrData);
			int fieldSize56 = (int)((long)(pCurrData - pBegin56) - 4L);
			bool flag56 = fieldSize56 > 4194304;
			if (flag56)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Size of field ");
				defaultInterpolatedStringHandler.AppendFormatted("_attackerHitStrength");
				defaultInterpolatedStringHandler.AppendLiteral(" must be less than ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(4096);
				defaultInterpolatedStringHandler.AppendLiteral("KB");
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			*(int*)pBegin56 = fieldSize56;
			byte* pBegin57 = pCurrData;
			pCurrData += 4;
			pCurrData += this._attackerHitTechnique.Serialize(pCurrData);
			int fieldSize57 = (int)((long)(pCurrData - pBegin57) - 4L);
			bool flag57 = fieldSize57 > 4194304;
			if (flag57)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Size of field ");
				defaultInterpolatedStringHandler.AppendFormatted("_attackerHitTechnique");
				defaultInterpolatedStringHandler.AppendLiteral(" must be less than ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(4096);
				defaultInterpolatedStringHandler.AppendLiteral("KB");
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			*(int*)pBegin57 = fieldSize57;
			byte* pBegin58 = pCurrData;
			pCurrData += 4;
			pCurrData += this._attackerHitSpeed.Serialize(pCurrData);
			int fieldSize58 = (int)((long)(pCurrData - pBegin58) - 4L);
			bool flag58 = fieldSize58 > 4194304;
			if (flag58)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Size of field ");
				defaultInterpolatedStringHandler.AppendFormatted("_attackerHitSpeed");
				defaultInterpolatedStringHandler.AppendLiteral(" must be less than ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(4096);
				defaultInterpolatedStringHandler.AppendLiteral("KB");
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			*(int*)pBegin58 = fieldSize58;
			byte* pBegin59 = pCurrData;
			pCurrData += 4;
			pCurrData += this._attackerHitMind.Serialize(pCurrData);
			int fieldSize59 = (int)((long)(pCurrData - pBegin59) - 4L);
			bool flag59 = fieldSize59 > 4194304;
			if (flag59)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Size of field ");
				defaultInterpolatedStringHandler.AppendFormatted("_attackerHitMind");
				defaultInterpolatedStringHandler.AppendLiteral(" must be less than ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(4096);
				defaultInterpolatedStringHandler.AppendLiteral("KB");
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			*(int*)pBegin59 = fieldSize59;
			byte* pBegin60 = pCurrData;
			pCurrData += 4;
			pCurrData += this._attackerAvoidStrength.Serialize(pCurrData);
			int fieldSize60 = (int)((long)(pCurrData - pBegin60) - 4L);
			bool flag60 = fieldSize60 > 4194304;
			if (flag60)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Size of field ");
				defaultInterpolatedStringHandler.AppendFormatted("_attackerAvoidStrength");
				defaultInterpolatedStringHandler.AppendLiteral(" must be less than ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(4096);
				defaultInterpolatedStringHandler.AppendLiteral("KB");
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			*(int*)pBegin60 = fieldSize60;
			byte* pBegin61 = pCurrData;
			pCurrData += 4;
			pCurrData += this._attackerAvoidTechnique.Serialize(pCurrData);
			int fieldSize61 = (int)((long)(pCurrData - pBegin61) - 4L);
			bool flag61 = fieldSize61 > 4194304;
			if (flag61)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Size of field ");
				defaultInterpolatedStringHandler.AppendFormatted("_attackerAvoidTechnique");
				defaultInterpolatedStringHandler.AppendLiteral(" must be less than ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(4096);
				defaultInterpolatedStringHandler.AppendLiteral("KB");
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			*(int*)pBegin61 = fieldSize61;
			byte* pBegin62 = pCurrData;
			pCurrData += 4;
			pCurrData += this._attackerAvoidSpeed.Serialize(pCurrData);
			int fieldSize62 = (int)((long)(pCurrData - pBegin62) - 4L);
			bool flag62 = fieldSize62 > 4194304;
			if (flag62)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Size of field ");
				defaultInterpolatedStringHandler.AppendFormatted("_attackerAvoidSpeed");
				defaultInterpolatedStringHandler.AppendLiteral(" must be less than ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(4096);
				defaultInterpolatedStringHandler.AppendLiteral("KB");
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			*(int*)pBegin62 = fieldSize62;
			byte* pBegin63 = pCurrData;
			pCurrData += 4;
			pCurrData += this._attackerAvoidMind.Serialize(pCurrData);
			int fieldSize63 = (int)((long)(pCurrData - pBegin63) - 4L);
			bool flag63 = fieldSize63 > 4194304;
			if (flag63)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Size of field ");
				defaultInterpolatedStringHandler.AppendFormatted("_attackerAvoidMind");
				defaultInterpolatedStringHandler.AppendLiteral(" must be less than ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(4096);
				defaultInterpolatedStringHandler.AppendLiteral("KB");
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			*(int*)pBegin63 = fieldSize63;
			byte* pBegin64 = pCurrData;
			pCurrData += 4;
			pCurrData += this._attackerPenetrateOuter.Serialize(pCurrData);
			int fieldSize64 = (int)((long)(pCurrData - pBegin64) - 4L);
			bool flag64 = fieldSize64 > 4194304;
			if (flag64)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Size of field ");
				defaultInterpolatedStringHandler.AppendFormatted("_attackerPenetrateOuter");
				defaultInterpolatedStringHandler.AppendLiteral(" must be less than ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(4096);
				defaultInterpolatedStringHandler.AppendLiteral("KB");
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			*(int*)pBegin64 = fieldSize64;
			byte* pBegin65 = pCurrData;
			pCurrData += 4;
			pCurrData += this._attackerPenetrateInner.Serialize(pCurrData);
			int fieldSize65 = (int)((long)(pCurrData - pBegin65) - 4L);
			bool flag65 = fieldSize65 > 4194304;
			if (flag65)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Size of field ");
				defaultInterpolatedStringHandler.AppendFormatted("_attackerPenetrateInner");
				defaultInterpolatedStringHandler.AppendLiteral(" must be less than ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(4096);
				defaultInterpolatedStringHandler.AppendLiteral("KB");
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			*(int*)pBegin65 = fieldSize65;
			byte* pBegin66 = pCurrData;
			pCurrData += 4;
			pCurrData += this._attackerPenetrateResistOuter.Serialize(pCurrData);
			int fieldSize66 = (int)((long)(pCurrData - pBegin66) - 4L);
			bool flag66 = fieldSize66 > 4194304;
			if (flag66)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Size of field ");
				defaultInterpolatedStringHandler.AppendFormatted("_attackerPenetrateResistOuter");
				defaultInterpolatedStringHandler.AppendLiteral(" must be less than ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(4096);
				defaultInterpolatedStringHandler.AppendLiteral("KB");
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			*(int*)pBegin66 = fieldSize66;
			byte* pBegin67 = pCurrData;
			pCurrData += 4;
			pCurrData += this._attackerPenetrateResistInner.Serialize(pCurrData);
			int fieldSize67 = (int)((long)(pCurrData - pBegin67) - 4L);
			bool flag67 = fieldSize67 > 4194304;
			if (flag67)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Size of field ");
				defaultInterpolatedStringHandler.AppendFormatted("_attackerPenetrateResistInner");
				defaultInterpolatedStringHandler.AppendLiteral(" must be less than ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(4096);
				defaultInterpolatedStringHandler.AppendLiteral("KB");
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			*(int*)pBegin67 = fieldSize67;
			byte* pBegin68 = pCurrData;
			pCurrData += 4;
			pCurrData += this._attackHitType.Serialize(pCurrData);
			int fieldSize68 = (int)((long)(pCurrData - pBegin68) - 4L);
			bool flag68 = fieldSize68 > 4194304;
			if (flag68)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Size of field ");
				defaultInterpolatedStringHandler.AppendFormatted("_attackHitType");
				defaultInterpolatedStringHandler.AppendLiteral(" must be less than ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(4096);
				defaultInterpolatedStringHandler.AppendLiteral("KB");
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			*(int*)pBegin68 = fieldSize68;
			byte* pBegin69 = pCurrData;
			pCurrData += 4;
			pCurrData += this._makeDirectDamage.Serialize(pCurrData);
			int fieldSize69 = (int)((long)(pCurrData - pBegin69) - 4L);
			bool flag69 = fieldSize69 > 4194304;
			if (flag69)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Size of field ");
				defaultInterpolatedStringHandler.AppendFormatted("_makeDirectDamage");
				defaultInterpolatedStringHandler.AppendLiteral(" must be less than ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(4096);
				defaultInterpolatedStringHandler.AppendLiteral("KB");
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			*(int*)pBegin69 = fieldSize69;
			byte* pBegin70 = pCurrData;
			pCurrData += 4;
			pCurrData += this._makeBounceDamage.Serialize(pCurrData);
			int fieldSize70 = (int)((long)(pCurrData - pBegin70) - 4L);
			bool flag70 = fieldSize70 > 4194304;
			if (flag70)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Size of field ");
				defaultInterpolatedStringHandler.AppendFormatted("_makeBounceDamage");
				defaultInterpolatedStringHandler.AppendLiteral(" must be less than ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(4096);
				defaultInterpolatedStringHandler.AppendLiteral("KB");
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			*(int*)pBegin70 = fieldSize70;
			byte* pBegin71 = pCurrData;
			pCurrData += 4;
			pCurrData += this._makeFightBackDamage.Serialize(pCurrData);
			int fieldSize71 = (int)((long)(pCurrData - pBegin71) - 4L);
			bool flag71 = fieldSize71 > 4194304;
			if (flag71)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Size of field ");
				defaultInterpolatedStringHandler.AppendFormatted("_makeFightBackDamage");
				defaultInterpolatedStringHandler.AppendLiteral(" must be less than ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(4096);
				defaultInterpolatedStringHandler.AppendLiteral("KB");
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			*(int*)pBegin71 = fieldSize71;
			byte* pBegin72 = pCurrData;
			pCurrData += 4;
			pCurrData += this._makePoisonLevel.Serialize(pCurrData);
			int fieldSize72 = (int)((long)(pCurrData - pBegin72) - 4L);
			bool flag72 = fieldSize72 > 4194304;
			if (flag72)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Size of field ");
				defaultInterpolatedStringHandler.AppendFormatted("_makePoisonLevel");
				defaultInterpolatedStringHandler.AppendLiteral(" must be less than ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(4096);
				defaultInterpolatedStringHandler.AppendLiteral("KB");
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			*(int*)pBegin72 = fieldSize72;
			byte* pBegin73 = pCurrData;
			pCurrData += 4;
			pCurrData += this._makePoisonValue.Serialize(pCurrData);
			int fieldSize73 = (int)((long)(pCurrData - pBegin73) - 4L);
			bool flag73 = fieldSize73 > 4194304;
			if (flag73)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Size of field ");
				defaultInterpolatedStringHandler.AppendFormatted("_makePoisonValue");
				defaultInterpolatedStringHandler.AppendLiteral(" must be less than ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(4096);
				defaultInterpolatedStringHandler.AppendLiteral("KB");
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			*(int*)pBegin73 = fieldSize73;
			byte* pBegin74 = pCurrData;
			pCurrData += 4;
			pCurrData += this._attackerHitOdds.Serialize(pCurrData);
			int fieldSize74 = (int)((long)(pCurrData - pBegin74) - 4L);
			bool flag74 = fieldSize74 > 4194304;
			if (flag74)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Size of field ");
				defaultInterpolatedStringHandler.AppendFormatted("_attackerHitOdds");
				defaultInterpolatedStringHandler.AppendLiteral(" must be less than ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(4096);
				defaultInterpolatedStringHandler.AppendLiteral("KB");
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			*(int*)pBegin74 = fieldSize74;
			byte* pBegin75 = pCurrData;
			pCurrData += 4;
			pCurrData += this._attackerFightBackHitOdds.Serialize(pCurrData);
			int fieldSize75 = (int)((long)(pCurrData - pBegin75) - 4L);
			bool flag75 = fieldSize75 > 4194304;
			if (flag75)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Size of field ");
				defaultInterpolatedStringHandler.AppendFormatted("_attackerFightBackHitOdds");
				defaultInterpolatedStringHandler.AppendLiteral(" must be less than ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(4096);
				defaultInterpolatedStringHandler.AppendLiteral("KB");
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			*(int*)pBegin75 = fieldSize75;
			byte* pBegin76 = pCurrData;
			pCurrData += 4;
			pCurrData += this._attackerPursueOdds.Serialize(pCurrData);
			int fieldSize76 = (int)((long)(pCurrData - pBegin76) - 4L);
			bool flag76 = fieldSize76 > 4194304;
			if (flag76)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Size of field ");
				defaultInterpolatedStringHandler.AppendFormatted("_attackerPursueOdds");
				defaultInterpolatedStringHandler.AppendLiteral(" must be less than ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(4096);
				defaultInterpolatedStringHandler.AppendLiteral("KB");
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			*(int*)pBegin76 = fieldSize76;
			byte* pBegin77 = pCurrData;
			pCurrData += 4;
			pCurrData += this._makedInjuryChangeToOld.Serialize(pCurrData);
			int fieldSize77 = (int)((long)(pCurrData - pBegin77) - 4L);
			bool flag77 = fieldSize77 > 4194304;
			if (flag77)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Size of field ");
				defaultInterpolatedStringHandler.AppendFormatted("_makedInjuryChangeToOld");
				defaultInterpolatedStringHandler.AppendLiteral(" must be less than ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(4096);
				defaultInterpolatedStringHandler.AppendLiteral("KB");
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			*(int*)pBegin77 = fieldSize77;
			byte* pBegin78 = pCurrData;
			pCurrData += 4;
			pCurrData += this._makedPoisonChangeToOld.Serialize(pCurrData);
			int fieldSize78 = (int)((long)(pCurrData - pBegin78) - 4L);
			bool flag78 = fieldSize78 > 4194304;
			if (flag78)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Size of field ");
				defaultInterpolatedStringHandler.AppendFormatted("_makedPoisonChangeToOld");
				defaultInterpolatedStringHandler.AppendLiteral(" must be less than ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(4096);
				defaultInterpolatedStringHandler.AppendLiteral("KB");
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			*(int*)pBegin78 = fieldSize78;
			byte* pBegin79 = pCurrData;
			pCurrData += 4;
			pCurrData += this._makeDamageType.Serialize(pCurrData);
			int fieldSize79 = (int)((long)(pCurrData - pBegin79) - 4L);
			bool flag79 = fieldSize79 > 4194304;
			if (flag79)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Size of field ");
				defaultInterpolatedStringHandler.AppendFormatted("_makeDamageType");
				defaultInterpolatedStringHandler.AppendLiteral(" must be less than ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(4096);
				defaultInterpolatedStringHandler.AppendLiteral("KB");
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			*(int*)pBegin79 = fieldSize79;
			byte* pBegin80 = pCurrData;
			pCurrData += 4;
			pCurrData += this._canMakeInjuryToNoInjuryPart.Serialize(pCurrData);
			int fieldSize80 = (int)((long)(pCurrData - pBegin80) - 4L);
			bool flag80 = fieldSize80 > 4194304;
			if (flag80)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Size of field ");
				defaultInterpolatedStringHandler.AppendFormatted("_canMakeInjuryToNoInjuryPart");
				defaultInterpolatedStringHandler.AppendLiteral(" must be less than ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(4096);
				defaultInterpolatedStringHandler.AppendLiteral("KB");
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			*(int*)pBegin80 = fieldSize80;
			byte* pBegin81 = pCurrData;
			pCurrData += 4;
			pCurrData += this._makePoisonType.Serialize(pCurrData);
			int fieldSize81 = (int)((long)(pCurrData - pBegin81) - 4L);
			bool flag81 = fieldSize81 > 4194304;
			if (flag81)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Size of field ");
				defaultInterpolatedStringHandler.AppendFormatted("_makePoisonType");
				defaultInterpolatedStringHandler.AppendLiteral(" must be less than ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(4096);
				defaultInterpolatedStringHandler.AppendLiteral("KB");
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			*(int*)pBegin81 = fieldSize81;
			byte* pBegin82 = pCurrData;
			pCurrData += 4;
			pCurrData += this._normalAttackWeapon.Serialize(pCurrData);
			int fieldSize82 = (int)((long)(pCurrData - pBegin82) - 4L);
			bool flag82 = fieldSize82 > 4194304;
			if (flag82)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Size of field ");
				defaultInterpolatedStringHandler.AppendFormatted("_normalAttackWeapon");
				defaultInterpolatedStringHandler.AppendLiteral(" must be less than ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(4096);
				defaultInterpolatedStringHandler.AppendLiteral("KB");
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			*(int*)pBegin82 = fieldSize82;
			byte* pBegin83 = pCurrData;
			pCurrData += 4;
			pCurrData += this._normalAttackTrick.Serialize(pCurrData);
			int fieldSize83 = (int)((long)(pCurrData - pBegin83) - 4L);
			bool flag83 = fieldSize83 > 4194304;
			if (flag83)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Size of field ");
				defaultInterpolatedStringHandler.AppendFormatted("_normalAttackTrick");
				defaultInterpolatedStringHandler.AppendLiteral(" must be less than ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(4096);
				defaultInterpolatedStringHandler.AppendLiteral("KB");
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			*(int*)pBegin83 = fieldSize83;
			byte* pBegin84 = pCurrData;
			pCurrData += 4;
			pCurrData += this._extraFlawCount.Serialize(pCurrData);
			int fieldSize84 = (int)((long)(pCurrData - pBegin84) - 4L);
			bool flag84 = fieldSize84 > 4194304;
			if (flag84)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Size of field ");
				defaultInterpolatedStringHandler.AppendFormatted("_extraFlawCount");
				defaultInterpolatedStringHandler.AppendLiteral(" must be less than ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(4096);
				defaultInterpolatedStringHandler.AppendLiteral("KB");
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			*(int*)pBegin84 = fieldSize84;
			byte* pBegin85 = pCurrData;
			pCurrData += 4;
			pCurrData += this._attackCanBounce.Serialize(pCurrData);
			int fieldSize85 = (int)((long)(pCurrData - pBegin85) - 4L);
			bool flag85 = fieldSize85 > 4194304;
			if (flag85)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Size of field ");
				defaultInterpolatedStringHandler.AppendFormatted("_attackCanBounce");
				defaultInterpolatedStringHandler.AppendLiteral(" must be less than ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(4096);
				defaultInterpolatedStringHandler.AppendLiteral("KB");
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			*(int*)pBegin85 = fieldSize85;
			byte* pBegin86 = pCurrData;
			pCurrData += 4;
			pCurrData += this._attackCanFightBack.Serialize(pCurrData);
			int fieldSize86 = (int)((long)(pCurrData - pBegin86) - 4L);
			bool flag86 = fieldSize86 > 4194304;
			if (flag86)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Size of field ");
				defaultInterpolatedStringHandler.AppendFormatted("_attackCanFightBack");
				defaultInterpolatedStringHandler.AppendLiteral(" must be less than ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(4096);
				defaultInterpolatedStringHandler.AppendLiteral("KB");
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			*(int*)pBegin86 = fieldSize86;
			byte* pBegin87 = pCurrData;
			pCurrData += 4;
			pCurrData += this._makeFightBackInjuryMark.Serialize(pCurrData);
			int fieldSize87 = (int)((long)(pCurrData - pBegin87) - 4L);
			bool flag87 = fieldSize87 > 4194304;
			if (flag87)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Size of field ");
				defaultInterpolatedStringHandler.AppendFormatted("_makeFightBackInjuryMark");
				defaultInterpolatedStringHandler.AppendLiteral(" must be less than ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(4096);
				defaultInterpolatedStringHandler.AppendLiteral("KB");
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			*(int*)pBegin87 = fieldSize87;
			byte* pBegin88 = pCurrData;
			pCurrData += 4;
			pCurrData += this._legSkillUseShoes.Serialize(pCurrData);
			int fieldSize88 = (int)((long)(pCurrData - pBegin88) - 4L);
			bool flag88 = fieldSize88 > 4194304;
			if (flag88)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Size of field ");
				defaultInterpolatedStringHandler.AppendFormatted("_legSkillUseShoes");
				defaultInterpolatedStringHandler.AppendLiteral(" must be less than ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(4096);
				defaultInterpolatedStringHandler.AppendLiteral("KB");
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			*(int*)pBegin88 = fieldSize88;
			byte* pBegin89 = pCurrData;
			pCurrData += 4;
			pCurrData += this._attackerFinalDamageValue.Serialize(pCurrData);
			int fieldSize89 = (int)((long)(pCurrData - pBegin89) - 4L);
			bool flag89 = fieldSize89 > 4194304;
			if (flag89)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Size of field ");
				defaultInterpolatedStringHandler.AppendFormatted("_attackerFinalDamageValue");
				defaultInterpolatedStringHandler.AppendLiteral(" must be less than ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(4096);
				defaultInterpolatedStringHandler.AppendLiteral("KB");
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			*(int*)pBegin89 = fieldSize89;
			byte* pBegin90 = pCurrData;
			pCurrData += 4;
			pCurrData += this._defenderHitStrength.Serialize(pCurrData);
			int fieldSize90 = (int)((long)(pCurrData - pBegin90) - 4L);
			bool flag90 = fieldSize90 > 4194304;
			if (flag90)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Size of field ");
				defaultInterpolatedStringHandler.AppendFormatted("_defenderHitStrength");
				defaultInterpolatedStringHandler.AppendLiteral(" must be less than ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(4096);
				defaultInterpolatedStringHandler.AppendLiteral("KB");
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			*(int*)pBegin90 = fieldSize90;
			byte* pBegin91 = pCurrData;
			pCurrData += 4;
			pCurrData += this._defenderHitTechnique.Serialize(pCurrData);
			int fieldSize91 = (int)((long)(pCurrData - pBegin91) - 4L);
			bool flag91 = fieldSize91 > 4194304;
			if (flag91)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Size of field ");
				defaultInterpolatedStringHandler.AppendFormatted("_defenderHitTechnique");
				defaultInterpolatedStringHandler.AppendLiteral(" must be less than ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(4096);
				defaultInterpolatedStringHandler.AppendLiteral("KB");
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			*(int*)pBegin91 = fieldSize91;
			byte* pBegin92 = pCurrData;
			pCurrData += 4;
			pCurrData += this._defenderHitSpeed.Serialize(pCurrData);
			int fieldSize92 = (int)((long)(pCurrData - pBegin92) - 4L);
			bool flag92 = fieldSize92 > 4194304;
			if (flag92)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Size of field ");
				defaultInterpolatedStringHandler.AppendFormatted("_defenderHitSpeed");
				defaultInterpolatedStringHandler.AppendLiteral(" must be less than ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(4096);
				defaultInterpolatedStringHandler.AppendLiteral("KB");
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			*(int*)pBegin92 = fieldSize92;
			byte* pBegin93 = pCurrData;
			pCurrData += 4;
			pCurrData += this._defenderHitMind.Serialize(pCurrData);
			int fieldSize93 = (int)((long)(pCurrData - pBegin93) - 4L);
			bool flag93 = fieldSize93 > 4194304;
			if (flag93)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Size of field ");
				defaultInterpolatedStringHandler.AppendFormatted("_defenderHitMind");
				defaultInterpolatedStringHandler.AppendLiteral(" must be less than ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(4096);
				defaultInterpolatedStringHandler.AppendLiteral("KB");
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			*(int*)pBegin93 = fieldSize93;
			byte* pBegin94 = pCurrData;
			pCurrData += 4;
			pCurrData += this._defenderAvoidStrength.Serialize(pCurrData);
			int fieldSize94 = (int)((long)(pCurrData - pBegin94) - 4L);
			bool flag94 = fieldSize94 > 4194304;
			if (flag94)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Size of field ");
				defaultInterpolatedStringHandler.AppendFormatted("_defenderAvoidStrength");
				defaultInterpolatedStringHandler.AppendLiteral(" must be less than ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(4096);
				defaultInterpolatedStringHandler.AppendLiteral("KB");
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			*(int*)pBegin94 = fieldSize94;
			byte* pBegin95 = pCurrData;
			pCurrData += 4;
			pCurrData += this._defenderAvoidTechnique.Serialize(pCurrData);
			int fieldSize95 = (int)((long)(pCurrData - pBegin95) - 4L);
			bool flag95 = fieldSize95 > 4194304;
			if (flag95)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Size of field ");
				defaultInterpolatedStringHandler.AppendFormatted("_defenderAvoidTechnique");
				defaultInterpolatedStringHandler.AppendLiteral(" must be less than ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(4096);
				defaultInterpolatedStringHandler.AppendLiteral("KB");
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			*(int*)pBegin95 = fieldSize95;
			byte* pBegin96 = pCurrData;
			pCurrData += 4;
			pCurrData += this._defenderAvoidSpeed.Serialize(pCurrData);
			int fieldSize96 = (int)((long)(pCurrData - pBegin96) - 4L);
			bool flag96 = fieldSize96 > 4194304;
			if (flag96)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Size of field ");
				defaultInterpolatedStringHandler.AppendFormatted("_defenderAvoidSpeed");
				defaultInterpolatedStringHandler.AppendLiteral(" must be less than ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(4096);
				defaultInterpolatedStringHandler.AppendLiteral("KB");
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			*(int*)pBegin96 = fieldSize96;
			byte* pBegin97 = pCurrData;
			pCurrData += 4;
			pCurrData += this._defenderAvoidMind.Serialize(pCurrData);
			int fieldSize97 = (int)((long)(pCurrData - pBegin97) - 4L);
			bool flag97 = fieldSize97 > 4194304;
			if (flag97)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Size of field ");
				defaultInterpolatedStringHandler.AppendFormatted("_defenderAvoidMind");
				defaultInterpolatedStringHandler.AppendLiteral(" must be less than ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(4096);
				defaultInterpolatedStringHandler.AppendLiteral("KB");
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			*(int*)pBegin97 = fieldSize97;
			byte* pBegin98 = pCurrData;
			pCurrData += 4;
			pCurrData += this._defenderPenetrateOuter.Serialize(pCurrData);
			int fieldSize98 = (int)((long)(pCurrData - pBegin98) - 4L);
			bool flag98 = fieldSize98 > 4194304;
			if (flag98)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Size of field ");
				defaultInterpolatedStringHandler.AppendFormatted("_defenderPenetrateOuter");
				defaultInterpolatedStringHandler.AppendLiteral(" must be less than ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(4096);
				defaultInterpolatedStringHandler.AppendLiteral("KB");
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			*(int*)pBegin98 = fieldSize98;
			byte* pBegin99 = pCurrData;
			pCurrData += 4;
			pCurrData += this._defenderPenetrateInner.Serialize(pCurrData);
			int fieldSize99 = (int)((long)(pCurrData - pBegin99) - 4L);
			bool flag99 = fieldSize99 > 4194304;
			if (flag99)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Size of field ");
				defaultInterpolatedStringHandler.AppendFormatted("_defenderPenetrateInner");
				defaultInterpolatedStringHandler.AppendLiteral(" must be less than ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(4096);
				defaultInterpolatedStringHandler.AppendLiteral("KB");
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			*(int*)pBegin99 = fieldSize99;
			byte* pBegin100 = pCurrData;
			pCurrData += 4;
			pCurrData += this._defenderPenetrateResistOuter.Serialize(pCurrData);
			int fieldSize100 = (int)((long)(pCurrData - pBegin100) - 4L);
			bool flag100 = fieldSize100 > 4194304;
			if (flag100)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Size of field ");
				defaultInterpolatedStringHandler.AppendFormatted("_defenderPenetrateResistOuter");
				defaultInterpolatedStringHandler.AppendLiteral(" must be less than ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(4096);
				defaultInterpolatedStringHandler.AppendLiteral("KB");
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			*(int*)pBegin100 = fieldSize100;
			byte* pBegin101 = pCurrData;
			pCurrData += 4;
			pCurrData += this._defenderPenetrateResistInner.Serialize(pCurrData);
			int fieldSize101 = (int)((long)(pCurrData - pBegin101) - 4L);
			bool flag101 = fieldSize101 > 4194304;
			if (flag101)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Size of field ");
				defaultInterpolatedStringHandler.AppendFormatted("_defenderPenetrateResistInner");
				defaultInterpolatedStringHandler.AppendLiteral(" must be less than ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(4096);
				defaultInterpolatedStringHandler.AppendLiteral("KB");
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			*(int*)pBegin101 = fieldSize101;
			byte* pBegin102 = pCurrData;
			pCurrData += 4;
			pCurrData += this._acceptDirectDamage.Serialize(pCurrData);
			int fieldSize102 = (int)((long)(pCurrData - pBegin102) - 4L);
			bool flag102 = fieldSize102 > 4194304;
			if (flag102)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Size of field ");
				defaultInterpolatedStringHandler.AppendFormatted("_acceptDirectDamage");
				defaultInterpolatedStringHandler.AppendLiteral(" must be less than ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(4096);
				defaultInterpolatedStringHandler.AppendLiteral("KB");
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			*(int*)pBegin102 = fieldSize102;
			byte* pBegin103 = pCurrData;
			pCurrData += 4;
			pCurrData += this._acceptBounceDamage.Serialize(pCurrData);
			int fieldSize103 = (int)((long)(pCurrData - pBegin103) - 4L);
			bool flag103 = fieldSize103 > 4194304;
			if (flag103)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Size of field ");
				defaultInterpolatedStringHandler.AppendFormatted("_acceptBounceDamage");
				defaultInterpolatedStringHandler.AppendLiteral(" must be less than ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(4096);
				defaultInterpolatedStringHandler.AppendLiteral("KB");
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			*(int*)pBegin103 = fieldSize103;
			byte* pBegin104 = pCurrData;
			pCurrData += 4;
			pCurrData += this._acceptFightBackDamage.Serialize(pCurrData);
			int fieldSize104 = (int)((long)(pCurrData - pBegin104) - 4L);
			bool flag104 = fieldSize104 > 4194304;
			if (flag104)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Size of field ");
				defaultInterpolatedStringHandler.AppendFormatted("_acceptFightBackDamage");
				defaultInterpolatedStringHandler.AppendLiteral(" must be less than ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(4096);
				defaultInterpolatedStringHandler.AppendLiteral("KB");
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			*(int*)pBegin104 = fieldSize104;
			byte* pBegin105 = pCurrData;
			pCurrData += 4;
			pCurrData += this._acceptPoisonLevel.Serialize(pCurrData);
			int fieldSize105 = (int)((long)(pCurrData - pBegin105) - 4L);
			bool flag105 = fieldSize105 > 4194304;
			if (flag105)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Size of field ");
				defaultInterpolatedStringHandler.AppendFormatted("_acceptPoisonLevel");
				defaultInterpolatedStringHandler.AppendLiteral(" must be less than ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(4096);
				defaultInterpolatedStringHandler.AppendLiteral("KB");
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			*(int*)pBegin105 = fieldSize105;
			byte* pBegin106 = pCurrData;
			pCurrData += 4;
			pCurrData += this._acceptPoisonValue.Serialize(pCurrData);
			int fieldSize106 = (int)((long)(pCurrData - pBegin106) - 4L);
			bool flag106 = fieldSize106 > 4194304;
			if (flag106)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Size of field ");
				defaultInterpolatedStringHandler.AppendFormatted("_acceptPoisonValue");
				defaultInterpolatedStringHandler.AppendLiteral(" must be less than ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(4096);
				defaultInterpolatedStringHandler.AppendLiteral("KB");
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			*(int*)pBegin106 = fieldSize106;
			byte* pBegin107 = pCurrData;
			pCurrData += 4;
			pCurrData += this._defenderHitOdds.Serialize(pCurrData);
			int fieldSize107 = (int)((long)(pCurrData - pBegin107) - 4L);
			bool flag107 = fieldSize107 > 4194304;
			if (flag107)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Size of field ");
				defaultInterpolatedStringHandler.AppendFormatted("_defenderHitOdds");
				defaultInterpolatedStringHandler.AppendLiteral(" must be less than ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(4096);
				defaultInterpolatedStringHandler.AppendLiteral("KB");
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			*(int*)pBegin107 = fieldSize107;
			byte* pBegin108 = pCurrData;
			pCurrData += 4;
			pCurrData += this._defenderFightBackHitOdds.Serialize(pCurrData);
			int fieldSize108 = (int)((long)(pCurrData - pBegin108) - 4L);
			bool flag108 = fieldSize108 > 4194304;
			if (flag108)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Size of field ");
				defaultInterpolatedStringHandler.AppendFormatted("_defenderFightBackHitOdds");
				defaultInterpolatedStringHandler.AppendLiteral(" must be less than ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(4096);
				defaultInterpolatedStringHandler.AppendLiteral("KB");
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			*(int*)pBegin108 = fieldSize108;
			byte* pBegin109 = pCurrData;
			pCurrData += 4;
			pCurrData += this._defenderPursueOdds.Serialize(pCurrData);
			int fieldSize109 = (int)((long)(pCurrData - pBegin109) - 4L);
			bool flag109 = fieldSize109 > 4194304;
			if (flag109)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Size of field ");
				defaultInterpolatedStringHandler.AppendFormatted("_defenderPursueOdds");
				defaultInterpolatedStringHandler.AppendLiteral(" must be less than ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(4096);
				defaultInterpolatedStringHandler.AppendLiteral("KB");
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			*(int*)pBegin109 = fieldSize109;
			byte* pBegin110 = pCurrData;
			pCurrData += 4;
			pCurrData += this._acceptMaxInjuryCount.Serialize(pCurrData);
			int fieldSize110 = (int)((long)(pCurrData - pBegin110) - 4L);
			bool flag110 = fieldSize110 > 4194304;
			if (flag110)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Size of field ");
				defaultInterpolatedStringHandler.AppendFormatted("_acceptMaxInjuryCount");
				defaultInterpolatedStringHandler.AppendLiteral(" must be less than ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(4096);
				defaultInterpolatedStringHandler.AppendLiteral("KB");
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			*(int*)pBegin110 = fieldSize110;
			byte* pBegin111 = pCurrData;
			pCurrData += 4;
			pCurrData += this._bouncePower.Serialize(pCurrData);
			int fieldSize111 = (int)((long)(pCurrData - pBegin111) - 4L);
			bool flag111 = fieldSize111 > 4194304;
			if (flag111)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Size of field ");
				defaultInterpolatedStringHandler.AppendFormatted("_bouncePower");
				defaultInterpolatedStringHandler.AppendLiteral(" must be less than ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(4096);
				defaultInterpolatedStringHandler.AppendLiteral("KB");
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			*(int*)pBegin111 = fieldSize111;
			byte* pBegin112 = pCurrData;
			pCurrData += 4;
			pCurrData += this._fightBackPower.Serialize(pCurrData);
			int fieldSize112 = (int)((long)(pCurrData - pBegin112) - 4L);
			bool flag112 = fieldSize112 > 4194304;
			if (flag112)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Size of field ");
				defaultInterpolatedStringHandler.AppendFormatted("_fightBackPower");
				defaultInterpolatedStringHandler.AppendLiteral(" must be less than ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(4096);
				defaultInterpolatedStringHandler.AppendLiteral("KB");
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			*(int*)pBegin112 = fieldSize112;
			byte* pBegin113 = pCurrData;
			pCurrData += 4;
			pCurrData += this._directDamageInnerRatio.Serialize(pCurrData);
			int fieldSize113 = (int)((long)(pCurrData - pBegin113) - 4L);
			bool flag113 = fieldSize113 > 4194304;
			if (flag113)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Size of field ");
				defaultInterpolatedStringHandler.AppendFormatted("_directDamageInnerRatio");
				defaultInterpolatedStringHandler.AppendLiteral(" must be less than ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(4096);
				defaultInterpolatedStringHandler.AppendLiteral("KB");
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			*(int*)pBegin113 = fieldSize113;
			byte* pBegin114 = pCurrData;
			pCurrData += 4;
			pCurrData += this._defenderFinalDamageValue.Serialize(pCurrData);
			int fieldSize114 = (int)((long)(pCurrData - pBegin114) - 4L);
			bool flag114 = fieldSize114 > 4194304;
			if (flag114)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Size of field ");
				defaultInterpolatedStringHandler.AppendFormatted("_defenderFinalDamageValue");
				defaultInterpolatedStringHandler.AppendLiteral(" must be less than ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(4096);
				defaultInterpolatedStringHandler.AppendLiteral("KB");
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			*(int*)pBegin114 = fieldSize114;
			byte* pBegin115 = pCurrData;
			pCurrData += 4;
			pCurrData += this._directDamageValue.Serialize(pCurrData);
			int fieldSize115 = (int)((long)(pCurrData - pBegin115) - 4L);
			bool flag115 = fieldSize115 > 4194304;
			if (flag115)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Size of field ");
				defaultInterpolatedStringHandler.AppendFormatted("_directDamageValue");
				defaultInterpolatedStringHandler.AppendLiteral(" must be less than ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(4096);
				defaultInterpolatedStringHandler.AppendLiteral("KB");
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			*(int*)pBegin115 = fieldSize115;
			byte* pBegin116 = pCurrData;
			pCurrData += 4;
			pCurrData += this._directInjuryMark.Serialize(pCurrData);
			int fieldSize116 = (int)((long)(pCurrData - pBegin116) - 4L);
			bool flag116 = fieldSize116 > 4194304;
			if (flag116)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Size of field ");
				defaultInterpolatedStringHandler.AppendFormatted("_directInjuryMark");
				defaultInterpolatedStringHandler.AppendLiteral(" must be less than ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(4096);
				defaultInterpolatedStringHandler.AppendLiteral("KB");
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			*(int*)pBegin116 = fieldSize116;
			byte* pBegin117 = pCurrData;
			pCurrData += 4;
			pCurrData += this._goneMadInjury.Serialize(pCurrData);
			int fieldSize117 = (int)((long)(pCurrData - pBegin117) - 4L);
			bool flag117 = fieldSize117 > 4194304;
			if (flag117)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Size of field ");
				defaultInterpolatedStringHandler.AppendFormatted("_goneMadInjury");
				defaultInterpolatedStringHandler.AppendLiteral(" must be less than ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(4096);
				defaultInterpolatedStringHandler.AppendLiteral("KB");
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			*(int*)pBegin117 = fieldSize117;
			byte* pBegin118 = pCurrData;
			pCurrData += 4;
			pCurrData += this._healInjurySpeed.Serialize(pCurrData);
			int fieldSize118 = (int)((long)(pCurrData - pBegin118) - 4L);
			bool flag118 = fieldSize118 > 4194304;
			if (flag118)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Size of field ");
				defaultInterpolatedStringHandler.AppendFormatted("_healInjurySpeed");
				defaultInterpolatedStringHandler.AppendLiteral(" must be less than ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(4096);
				defaultInterpolatedStringHandler.AppendLiteral("KB");
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			*(int*)pBegin118 = fieldSize118;
			byte* pBegin119 = pCurrData;
			pCurrData += 4;
			pCurrData += this._healInjuryBuff.Serialize(pCurrData);
			int fieldSize119 = (int)((long)(pCurrData - pBegin119) - 4L);
			bool flag119 = fieldSize119 > 4194304;
			if (flag119)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Size of field ");
				defaultInterpolatedStringHandler.AppendFormatted("_healInjuryBuff");
				defaultInterpolatedStringHandler.AppendLiteral(" must be less than ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(4096);
				defaultInterpolatedStringHandler.AppendLiteral("KB");
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			*(int*)pBegin119 = fieldSize119;
			byte* pBegin120 = pCurrData;
			pCurrData += 4;
			pCurrData += this._healInjuryDebuff.Serialize(pCurrData);
			int fieldSize120 = (int)((long)(pCurrData - pBegin120) - 4L);
			bool flag120 = fieldSize120 > 4194304;
			if (flag120)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Size of field ");
				defaultInterpolatedStringHandler.AppendFormatted("_healInjuryDebuff");
				defaultInterpolatedStringHandler.AppendLiteral(" must be less than ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(4096);
				defaultInterpolatedStringHandler.AppendLiteral("KB");
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			*(int*)pBegin120 = fieldSize120;
			byte* pBegin121 = pCurrData;
			pCurrData += 4;
			pCurrData += this._healPoisonSpeed.Serialize(pCurrData);
			int fieldSize121 = (int)((long)(pCurrData - pBegin121) - 4L);
			bool flag121 = fieldSize121 > 4194304;
			if (flag121)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Size of field ");
				defaultInterpolatedStringHandler.AppendFormatted("_healPoisonSpeed");
				defaultInterpolatedStringHandler.AppendLiteral(" must be less than ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(4096);
				defaultInterpolatedStringHandler.AppendLiteral("KB");
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			*(int*)pBegin121 = fieldSize121;
			byte* pBegin122 = pCurrData;
			pCurrData += 4;
			pCurrData += this._healPoisonBuff.Serialize(pCurrData);
			int fieldSize122 = (int)((long)(pCurrData - pBegin122) - 4L);
			bool flag122 = fieldSize122 > 4194304;
			if (flag122)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Size of field ");
				defaultInterpolatedStringHandler.AppendFormatted("_healPoisonBuff");
				defaultInterpolatedStringHandler.AppendLiteral(" must be less than ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(4096);
				defaultInterpolatedStringHandler.AppendLiteral("KB");
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			*(int*)pBegin122 = fieldSize122;
			byte* pBegin123 = pCurrData;
			pCurrData += 4;
			pCurrData += this._healPoisonDebuff.Serialize(pCurrData);
			int fieldSize123 = (int)((long)(pCurrData - pBegin123) - 4L);
			bool flag123 = fieldSize123 > 4194304;
			if (flag123)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Size of field ");
				defaultInterpolatedStringHandler.AppendFormatted("_healPoisonDebuff");
				defaultInterpolatedStringHandler.AppendLiteral(" must be less than ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(4096);
				defaultInterpolatedStringHandler.AppendLiteral("KB");
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			*(int*)pBegin123 = fieldSize123;
			byte* pBegin124 = pCurrData;
			pCurrData += 4;
			pCurrData += this._fleeSpeed.Serialize(pCurrData);
			int fieldSize124 = (int)((long)(pCurrData - pBegin124) - 4L);
			bool flag124 = fieldSize124 > 4194304;
			if (flag124)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Size of field ");
				defaultInterpolatedStringHandler.AppendFormatted("_fleeSpeed");
				defaultInterpolatedStringHandler.AppendLiteral(" must be less than ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(4096);
				defaultInterpolatedStringHandler.AppendLiteral("KB");
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			*(int*)pBegin124 = fieldSize124;
			byte* pBegin125 = pCurrData;
			pCurrData += 4;
			pCurrData += this._maxFlawCount.Serialize(pCurrData);
			int fieldSize125 = (int)((long)(pCurrData - pBegin125) - 4L);
			bool flag125 = fieldSize125 > 4194304;
			if (flag125)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Size of field ");
				defaultInterpolatedStringHandler.AppendFormatted("_maxFlawCount");
				defaultInterpolatedStringHandler.AppendLiteral(" must be less than ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(4096);
				defaultInterpolatedStringHandler.AppendLiteral("KB");
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			*(int*)pBegin125 = fieldSize125;
			byte* pBegin126 = pCurrData;
			pCurrData += 4;
			pCurrData += this._canAddFlaw.Serialize(pCurrData);
			int fieldSize126 = (int)((long)(pCurrData - pBegin126) - 4L);
			bool flag126 = fieldSize126 > 4194304;
			if (flag126)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Size of field ");
				defaultInterpolatedStringHandler.AppendFormatted("_canAddFlaw");
				defaultInterpolatedStringHandler.AppendLiteral(" must be less than ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(4096);
				defaultInterpolatedStringHandler.AppendLiteral("KB");
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			*(int*)pBegin126 = fieldSize126;
			byte* pBegin127 = pCurrData;
			pCurrData += 4;
			pCurrData += this._flawLevel.Serialize(pCurrData);
			int fieldSize127 = (int)((long)(pCurrData - pBegin127) - 4L);
			bool flag127 = fieldSize127 > 4194304;
			if (flag127)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Size of field ");
				defaultInterpolatedStringHandler.AppendFormatted("_flawLevel");
				defaultInterpolatedStringHandler.AppendLiteral(" must be less than ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(4096);
				defaultInterpolatedStringHandler.AppendLiteral("KB");
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			*(int*)pBegin127 = fieldSize127;
			byte* pBegin128 = pCurrData;
			pCurrData += 4;
			pCurrData += this._flawLevelCanReduce.Serialize(pCurrData);
			int fieldSize128 = (int)((long)(pCurrData - pBegin128) - 4L);
			bool flag128 = fieldSize128 > 4194304;
			if (flag128)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Size of field ");
				defaultInterpolatedStringHandler.AppendFormatted("_flawLevelCanReduce");
				defaultInterpolatedStringHandler.AppendLiteral(" must be less than ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(4096);
				defaultInterpolatedStringHandler.AppendLiteral("KB");
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			*(int*)pBegin128 = fieldSize128;
			byte* pBegin129 = pCurrData;
			pCurrData += 4;
			pCurrData += this._flawCount.Serialize(pCurrData);
			int fieldSize129 = (int)((long)(pCurrData - pBegin129) - 4L);
			bool flag129 = fieldSize129 > 4194304;
			if (flag129)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Size of field ");
				defaultInterpolatedStringHandler.AppendFormatted("_flawCount");
				defaultInterpolatedStringHandler.AppendLiteral(" must be less than ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(4096);
				defaultInterpolatedStringHandler.AppendLiteral("KB");
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			*(int*)pBegin129 = fieldSize129;
			byte* pBegin130 = pCurrData;
			pCurrData += 4;
			pCurrData += this._maxAcupointCount.Serialize(pCurrData);
			int fieldSize130 = (int)((long)(pCurrData - pBegin130) - 4L);
			bool flag130 = fieldSize130 > 4194304;
			if (flag130)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Size of field ");
				defaultInterpolatedStringHandler.AppendFormatted("_maxAcupointCount");
				defaultInterpolatedStringHandler.AppendLiteral(" must be less than ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(4096);
				defaultInterpolatedStringHandler.AppendLiteral("KB");
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			*(int*)pBegin130 = fieldSize130;
			byte* pBegin131 = pCurrData;
			pCurrData += 4;
			pCurrData += this._canAddAcupoint.Serialize(pCurrData);
			int fieldSize131 = (int)((long)(pCurrData - pBegin131) - 4L);
			bool flag131 = fieldSize131 > 4194304;
			if (flag131)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Size of field ");
				defaultInterpolatedStringHandler.AppendFormatted("_canAddAcupoint");
				defaultInterpolatedStringHandler.AppendLiteral(" must be less than ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(4096);
				defaultInterpolatedStringHandler.AppendLiteral("KB");
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			*(int*)pBegin131 = fieldSize131;
			byte* pBegin132 = pCurrData;
			pCurrData += 4;
			pCurrData += this._acupointLevel.Serialize(pCurrData);
			int fieldSize132 = (int)((long)(pCurrData - pBegin132) - 4L);
			bool flag132 = fieldSize132 > 4194304;
			if (flag132)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Size of field ");
				defaultInterpolatedStringHandler.AppendFormatted("_acupointLevel");
				defaultInterpolatedStringHandler.AppendLiteral(" must be less than ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(4096);
				defaultInterpolatedStringHandler.AppendLiteral("KB");
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			*(int*)pBegin132 = fieldSize132;
			byte* pBegin133 = pCurrData;
			pCurrData += 4;
			pCurrData += this._acupointLevelCanReduce.Serialize(pCurrData);
			int fieldSize133 = (int)((long)(pCurrData - pBegin133) - 4L);
			bool flag133 = fieldSize133 > 4194304;
			if (flag133)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Size of field ");
				defaultInterpolatedStringHandler.AppendFormatted("_acupointLevelCanReduce");
				defaultInterpolatedStringHandler.AppendLiteral(" must be less than ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(4096);
				defaultInterpolatedStringHandler.AppendLiteral("KB");
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			*(int*)pBegin133 = fieldSize133;
			byte* pBegin134 = pCurrData;
			pCurrData += 4;
			pCurrData += this._acupointCount.Serialize(pCurrData);
			int fieldSize134 = (int)((long)(pCurrData - pBegin134) - 4L);
			bool flag134 = fieldSize134 > 4194304;
			if (flag134)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Size of field ");
				defaultInterpolatedStringHandler.AppendFormatted("_acupointCount");
				defaultInterpolatedStringHandler.AppendLiteral(" must be less than ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(4096);
				defaultInterpolatedStringHandler.AppendLiteral("KB");
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			*(int*)pBegin134 = fieldSize134;
			byte* pBegin135 = pCurrData;
			pCurrData += 4;
			pCurrData += this._addNeiliAllocation.Serialize(pCurrData);
			int fieldSize135 = (int)((long)(pCurrData - pBegin135) - 4L);
			bool flag135 = fieldSize135 > 4194304;
			if (flag135)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Size of field ");
				defaultInterpolatedStringHandler.AppendFormatted("_addNeiliAllocation");
				defaultInterpolatedStringHandler.AppendLiteral(" must be less than ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(4096);
				defaultInterpolatedStringHandler.AppendLiteral("KB");
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			*(int*)pBegin135 = fieldSize135;
			byte* pBegin136 = pCurrData;
			pCurrData += 4;
			pCurrData += this._costNeiliAllocation.Serialize(pCurrData);
			int fieldSize136 = (int)((long)(pCurrData - pBegin136) - 4L);
			bool flag136 = fieldSize136 > 4194304;
			if (flag136)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Size of field ");
				defaultInterpolatedStringHandler.AppendFormatted("_costNeiliAllocation");
				defaultInterpolatedStringHandler.AppendLiteral(" must be less than ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(4096);
				defaultInterpolatedStringHandler.AppendLiteral("KB");
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			*(int*)pBegin136 = fieldSize136;
			byte* pBegin137 = pCurrData;
			pCurrData += 4;
			pCurrData += this._canChangeNeiliAllocation.Serialize(pCurrData);
			int fieldSize137 = (int)((long)(pCurrData - pBegin137) - 4L);
			bool flag137 = fieldSize137 > 4194304;
			if (flag137)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Size of field ");
				defaultInterpolatedStringHandler.AppendFormatted("_canChangeNeiliAllocation");
				defaultInterpolatedStringHandler.AppendLiteral(" must be less than ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(4096);
				defaultInterpolatedStringHandler.AppendLiteral("KB");
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			*(int*)pBegin137 = fieldSize137;
			byte* pBegin138 = pCurrData;
			pCurrData += 4;
			pCurrData += this._canGetTrick.Serialize(pCurrData);
			int fieldSize138 = (int)((long)(pCurrData - pBegin138) - 4L);
			bool flag138 = fieldSize138 > 4194304;
			if (flag138)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Size of field ");
				defaultInterpolatedStringHandler.AppendFormatted("_canGetTrick");
				defaultInterpolatedStringHandler.AppendLiteral(" must be less than ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(4096);
				defaultInterpolatedStringHandler.AppendLiteral("KB");
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			*(int*)pBegin138 = fieldSize138;
			byte* pBegin139 = pCurrData;
			pCurrData += 4;
			pCurrData += this._getTrickType.Serialize(pCurrData);
			int fieldSize139 = (int)((long)(pCurrData - pBegin139) - 4L);
			bool flag139 = fieldSize139 > 4194304;
			if (flag139)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Size of field ");
				defaultInterpolatedStringHandler.AppendFormatted("_getTrickType");
				defaultInterpolatedStringHandler.AppendLiteral(" must be less than ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(4096);
				defaultInterpolatedStringHandler.AppendLiteral("KB");
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			*(int*)pBegin139 = fieldSize139;
			byte* pBegin140 = pCurrData;
			pCurrData += 4;
			pCurrData += this._attackBodyPart.Serialize(pCurrData);
			int fieldSize140 = (int)((long)(pCurrData - pBegin140) - 4L);
			bool flag140 = fieldSize140 > 4194304;
			if (flag140)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Size of field ");
				defaultInterpolatedStringHandler.AppendFormatted("_attackBodyPart");
				defaultInterpolatedStringHandler.AppendLiteral(" must be less than ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(4096);
				defaultInterpolatedStringHandler.AppendLiteral("KB");
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			*(int*)pBegin140 = fieldSize140;
			byte* pBegin141 = pCurrData;
			pCurrData += 4;
			pCurrData += this._weaponEquipAttack.Serialize(pCurrData);
			int fieldSize141 = (int)((long)(pCurrData - pBegin141) - 4L);
			bool flag141 = fieldSize141 > 4194304;
			if (flag141)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Size of field ");
				defaultInterpolatedStringHandler.AppendFormatted("_weaponEquipAttack");
				defaultInterpolatedStringHandler.AppendLiteral(" must be less than ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(4096);
				defaultInterpolatedStringHandler.AppendLiteral("KB");
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			*(int*)pBegin141 = fieldSize141;
			byte* pBegin142 = pCurrData;
			pCurrData += 4;
			pCurrData += this._weaponEquipDefense.Serialize(pCurrData);
			int fieldSize142 = (int)((long)(pCurrData - pBegin142) - 4L);
			bool flag142 = fieldSize142 > 4194304;
			if (flag142)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Size of field ");
				defaultInterpolatedStringHandler.AppendFormatted("_weaponEquipDefense");
				defaultInterpolatedStringHandler.AppendLiteral(" must be less than ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(4096);
				defaultInterpolatedStringHandler.AppendLiteral("KB");
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			*(int*)pBegin142 = fieldSize142;
			byte* pBegin143 = pCurrData;
			pCurrData += 4;
			pCurrData += this._armorEquipAttack.Serialize(pCurrData);
			int fieldSize143 = (int)((long)(pCurrData - pBegin143) - 4L);
			bool flag143 = fieldSize143 > 4194304;
			if (flag143)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Size of field ");
				defaultInterpolatedStringHandler.AppendFormatted("_armorEquipAttack");
				defaultInterpolatedStringHandler.AppendLiteral(" must be less than ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(4096);
				defaultInterpolatedStringHandler.AppendLiteral("KB");
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			*(int*)pBegin143 = fieldSize143;
			byte* pBegin144 = pCurrData;
			pCurrData += 4;
			pCurrData += this._armorEquipDefense.Serialize(pCurrData);
			int fieldSize144 = (int)((long)(pCurrData - pBegin144) - 4L);
			bool flag144 = fieldSize144 > 4194304;
			if (flag144)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Size of field ");
				defaultInterpolatedStringHandler.AppendFormatted("_armorEquipDefense");
				defaultInterpolatedStringHandler.AppendLiteral(" must be less than ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(4096);
				defaultInterpolatedStringHandler.AppendLiteral("KB");
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			*(int*)pBegin144 = fieldSize144;
			byte* pBegin145 = pCurrData;
			pCurrData += 4;
			pCurrData += this._attackRangeForward.Serialize(pCurrData);
			int fieldSize145 = (int)((long)(pCurrData - pBegin145) - 4L);
			bool flag145 = fieldSize145 > 4194304;
			if (flag145)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Size of field ");
				defaultInterpolatedStringHandler.AppendFormatted("_attackRangeForward");
				defaultInterpolatedStringHandler.AppendLiteral(" must be less than ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(4096);
				defaultInterpolatedStringHandler.AppendLiteral("KB");
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			*(int*)pBegin145 = fieldSize145;
			byte* pBegin146 = pCurrData;
			pCurrData += 4;
			pCurrData += this._attackRangeBackward.Serialize(pCurrData);
			int fieldSize146 = (int)((long)(pCurrData - pBegin146) - 4L);
			bool flag146 = fieldSize146 > 4194304;
			if (flag146)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Size of field ");
				defaultInterpolatedStringHandler.AppendFormatted("_attackRangeBackward");
				defaultInterpolatedStringHandler.AppendLiteral(" must be less than ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(4096);
				defaultInterpolatedStringHandler.AppendLiteral("KB");
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			*(int*)pBegin146 = fieldSize146;
			byte* pBegin147 = pCurrData;
			pCurrData += 4;
			pCurrData += this._moveCanBeStopped.Serialize(pCurrData);
			int fieldSize147 = (int)((long)(pCurrData - pBegin147) - 4L);
			bool flag147 = fieldSize147 > 4194304;
			if (flag147)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Size of field ");
				defaultInterpolatedStringHandler.AppendFormatted("_moveCanBeStopped");
				defaultInterpolatedStringHandler.AppendLiteral(" must be less than ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(4096);
				defaultInterpolatedStringHandler.AppendLiteral("KB");
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			*(int*)pBegin147 = fieldSize147;
			byte* pBegin148 = pCurrData;
			pCurrData += 4;
			pCurrData += this._canForcedMove.Serialize(pCurrData);
			int fieldSize148 = (int)((long)(pCurrData - pBegin148) - 4L);
			bool flag148 = fieldSize148 > 4194304;
			if (flag148)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Size of field ");
				defaultInterpolatedStringHandler.AppendFormatted("_canForcedMove");
				defaultInterpolatedStringHandler.AppendLiteral(" must be less than ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(4096);
				defaultInterpolatedStringHandler.AppendLiteral("KB");
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			*(int*)pBegin148 = fieldSize148;
			byte* pBegin149 = pCurrData;
			pCurrData += 4;
			pCurrData += this._mobilityCanBeRemoved.Serialize(pCurrData);
			int fieldSize149 = (int)((long)(pCurrData - pBegin149) - 4L);
			bool flag149 = fieldSize149 > 4194304;
			if (flag149)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Size of field ");
				defaultInterpolatedStringHandler.AppendFormatted("_mobilityCanBeRemoved");
				defaultInterpolatedStringHandler.AppendLiteral(" must be less than ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(4096);
				defaultInterpolatedStringHandler.AppendLiteral("KB");
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			*(int*)pBegin149 = fieldSize149;
			byte* pBegin150 = pCurrData;
			pCurrData += 4;
			pCurrData += this._mobilityCostByEffect.Serialize(pCurrData);
			int fieldSize150 = (int)((long)(pCurrData - pBegin150) - 4L);
			bool flag150 = fieldSize150 > 4194304;
			if (flag150)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Size of field ");
				defaultInterpolatedStringHandler.AppendFormatted("_mobilityCostByEffect");
				defaultInterpolatedStringHandler.AppendLiteral(" must be less than ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(4096);
				defaultInterpolatedStringHandler.AppendLiteral("KB");
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			*(int*)pBegin150 = fieldSize150;
			byte* pBegin151 = pCurrData;
			pCurrData += 4;
			pCurrData += this._moveDistance.Serialize(pCurrData);
			int fieldSize151 = (int)((long)(pCurrData - pBegin151) - 4L);
			bool flag151 = fieldSize151 > 4194304;
			if (flag151)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Size of field ");
				defaultInterpolatedStringHandler.AppendFormatted("_moveDistance");
				defaultInterpolatedStringHandler.AppendLiteral(" must be less than ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(4096);
				defaultInterpolatedStringHandler.AppendLiteral("KB");
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			*(int*)pBegin151 = fieldSize151;
			byte* pBegin152 = pCurrData;
			pCurrData += 4;
			pCurrData += this._jumpPrepareFrame.Serialize(pCurrData);
			int fieldSize152 = (int)((long)(pCurrData - pBegin152) - 4L);
			bool flag152 = fieldSize152 > 4194304;
			if (flag152)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Size of field ");
				defaultInterpolatedStringHandler.AppendFormatted("_jumpPrepareFrame");
				defaultInterpolatedStringHandler.AppendLiteral(" must be less than ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(4096);
				defaultInterpolatedStringHandler.AppendLiteral("KB");
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			*(int*)pBegin152 = fieldSize152;
			byte* pBegin153 = pCurrData;
			pCurrData += 4;
			pCurrData += this._bounceInjuryMark.Serialize(pCurrData);
			int fieldSize153 = (int)((long)(pCurrData - pBegin153) - 4L);
			bool flag153 = fieldSize153 > 4194304;
			if (flag153)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Size of field ");
				defaultInterpolatedStringHandler.AppendFormatted("_bounceInjuryMark");
				defaultInterpolatedStringHandler.AppendLiteral(" must be less than ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(4096);
				defaultInterpolatedStringHandler.AppendLiteral("KB");
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			*(int*)pBegin153 = fieldSize153;
			byte* pBegin154 = pCurrData;
			pCurrData += 4;
			pCurrData += this._skillHasCost.Serialize(pCurrData);
			int fieldSize154 = (int)((long)(pCurrData - pBegin154) - 4L);
			bool flag154 = fieldSize154 > 4194304;
			if (flag154)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Size of field ");
				defaultInterpolatedStringHandler.AppendFormatted("_skillHasCost");
				defaultInterpolatedStringHandler.AppendLiteral(" must be less than ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(4096);
				defaultInterpolatedStringHandler.AppendLiteral("KB");
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			*(int*)pBegin154 = fieldSize154;
			byte* pBegin155 = pCurrData;
			pCurrData += 4;
			pCurrData += this._combatStateEffect.Serialize(pCurrData);
			int fieldSize155 = (int)((long)(pCurrData - pBegin155) - 4L);
			bool flag155 = fieldSize155 > 4194304;
			if (flag155)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Size of field ");
				defaultInterpolatedStringHandler.AppendFormatted("_combatStateEffect");
				defaultInterpolatedStringHandler.AppendLiteral(" must be less than ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(4096);
				defaultInterpolatedStringHandler.AppendLiteral("KB");
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			*(int*)pBegin155 = fieldSize155;
			byte* pBegin156 = pCurrData;
			pCurrData += 4;
			pCurrData += this._changeNeedUseSkill.Serialize(pCurrData);
			int fieldSize156 = (int)((long)(pCurrData - pBegin156) - 4L);
			bool flag156 = fieldSize156 > 4194304;
			if (flag156)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Size of field ");
				defaultInterpolatedStringHandler.AppendFormatted("_changeNeedUseSkill");
				defaultInterpolatedStringHandler.AppendLiteral(" must be less than ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(4096);
				defaultInterpolatedStringHandler.AppendLiteral("KB");
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			*(int*)pBegin156 = fieldSize156;
			byte* pBegin157 = pCurrData;
			pCurrData += 4;
			pCurrData += this._changeDistanceIsMove.Serialize(pCurrData);
			int fieldSize157 = (int)((long)(pCurrData - pBegin157) - 4L);
			bool flag157 = fieldSize157 > 4194304;
			if (flag157)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Size of field ");
				defaultInterpolatedStringHandler.AppendFormatted("_changeDistanceIsMove");
				defaultInterpolatedStringHandler.AppendLiteral(" must be less than ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(4096);
				defaultInterpolatedStringHandler.AppendLiteral("KB");
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			*(int*)pBegin157 = fieldSize157;
			byte* pBegin158 = pCurrData;
			pCurrData += 4;
			pCurrData += this._replaceCharHit.Serialize(pCurrData);
			int fieldSize158 = (int)((long)(pCurrData - pBegin158) - 4L);
			bool flag158 = fieldSize158 > 4194304;
			if (flag158)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Size of field ");
				defaultInterpolatedStringHandler.AppendFormatted("_replaceCharHit");
				defaultInterpolatedStringHandler.AppendLiteral(" must be less than ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(4096);
				defaultInterpolatedStringHandler.AppendLiteral("KB");
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			*(int*)pBegin158 = fieldSize158;
			byte* pBegin159 = pCurrData;
			pCurrData += 4;
			pCurrData += this._canAddPoison.Serialize(pCurrData);
			int fieldSize159 = (int)((long)(pCurrData - pBegin159) - 4L);
			bool flag159 = fieldSize159 > 4194304;
			if (flag159)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Size of field ");
				defaultInterpolatedStringHandler.AppendFormatted("_canAddPoison");
				defaultInterpolatedStringHandler.AppendLiteral(" must be less than ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(4096);
				defaultInterpolatedStringHandler.AppendLiteral("KB");
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			*(int*)pBegin159 = fieldSize159;
			byte* pBegin160 = pCurrData;
			pCurrData += 4;
			pCurrData += this._canReducePoison.Serialize(pCurrData);
			int fieldSize160 = (int)((long)(pCurrData - pBegin160) - 4L);
			bool flag160 = fieldSize160 > 4194304;
			if (flag160)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Size of field ");
				defaultInterpolatedStringHandler.AppendFormatted("_canReducePoison");
				defaultInterpolatedStringHandler.AppendLiteral(" must be less than ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(4096);
				defaultInterpolatedStringHandler.AppendLiteral("KB");
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			*(int*)pBegin160 = fieldSize160;
			byte* pBegin161 = pCurrData;
			pCurrData += 4;
			pCurrData += this._reducePoisonValue.Serialize(pCurrData);
			int fieldSize161 = (int)((long)(pCurrData - pBegin161) - 4L);
			bool flag161 = fieldSize161 > 4194304;
			if (flag161)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Size of field ");
				defaultInterpolatedStringHandler.AppendFormatted("_reducePoisonValue");
				defaultInterpolatedStringHandler.AppendLiteral(" must be less than ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(4096);
				defaultInterpolatedStringHandler.AppendLiteral("KB");
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			*(int*)pBegin161 = fieldSize161;
			byte* pBegin162 = pCurrData;
			pCurrData += 4;
			pCurrData += this._poisonCanAffect.Serialize(pCurrData);
			int fieldSize162 = (int)((long)(pCurrData - pBegin162) - 4L);
			bool flag162 = fieldSize162 > 4194304;
			if (flag162)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Size of field ");
				defaultInterpolatedStringHandler.AppendFormatted("_poisonCanAffect");
				defaultInterpolatedStringHandler.AppendLiteral(" must be less than ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(4096);
				defaultInterpolatedStringHandler.AppendLiteral("KB");
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			*(int*)pBegin162 = fieldSize162;
			byte* pBegin163 = pCurrData;
			pCurrData += 4;
			pCurrData += this._poisonAffectCount.Serialize(pCurrData);
			int fieldSize163 = (int)((long)(pCurrData - pBegin163) - 4L);
			bool flag163 = fieldSize163 > 4194304;
			if (flag163)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Size of field ");
				defaultInterpolatedStringHandler.AppendFormatted("_poisonAffectCount");
				defaultInterpolatedStringHandler.AppendLiteral(" must be less than ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(4096);
				defaultInterpolatedStringHandler.AppendLiteral("KB");
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			*(int*)pBegin163 = fieldSize163;
			byte* pBegin164 = pCurrData;
			pCurrData += 4;
			pCurrData += this._costTricks.Serialize(pCurrData);
			int fieldSize164 = (int)((long)(pCurrData - pBegin164) - 4L);
			bool flag164 = fieldSize164 > 4194304;
			if (flag164)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Size of field ");
				defaultInterpolatedStringHandler.AppendFormatted("_costTricks");
				defaultInterpolatedStringHandler.AppendLiteral(" must be less than ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(4096);
				defaultInterpolatedStringHandler.AppendLiteral("KB");
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			*(int*)pBegin164 = fieldSize164;
			byte* pBegin165 = pCurrData;
			pCurrData += 4;
			pCurrData += this._jumpMoveDistance.Serialize(pCurrData);
			int fieldSize165 = (int)((long)(pCurrData - pBegin165) - 4L);
			bool flag165 = fieldSize165 > 4194304;
			if (flag165)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Size of field ");
				defaultInterpolatedStringHandler.AppendFormatted("_jumpMoveDistance");
				defaultInterpolatedStringHandler.AppendLiteral(" must be less than ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(4096);
				defaultInterpolatedStringHandler.AppendLiteral("KB");
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			*(int*)pBegin165 = fieldSize165;
			byte* pBegin166 = pCurrData;
			pCurrData += 4;
			pCurrData += this._combatStateToAdd.Serialize(pCurrData);
			int fieldSize166 = (int)((long)(pCurrData - pBegin166) - 4L);
			bool flag166 = fieldSize166 > 4194304;
			if (flag166)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Size of field ");
				defaultInterpolatedStringHandler.AppendFormatted("_combatStateToAdd");
				defaultInterpolatedStringHandler.AppendLiteral(" must be less than ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(4096);
				defaultInterpolatedStringHandler.AppendLiteral("KB");
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			*(int*)pBegin166 = fieldSize166;
			byte* pBegin167 = pCurrData;
			pCurrData += 4;
			pCurrData += this._combatStatePower.Serialize(pCurrData);
			int fieldSize167 = (int)((long)(pCurrData - pBegin167) - 4L);
			bool flag167 = fieldSize167 > 4194304;
			if (flag167)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Size of field ");
				defaultInterpolatedStringHandler.AppendFormatted("_combatStatePower");
				defaultInterpolatedStringHandler.AppendLiteral(" must be less than ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(4096);
				defaultInterpolatedStringHandler.AppendLiteral("KB");
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			*(int*)pBegin167 = fieldSize167;
			byte* pBegin168 = pCurrData;
			pCurrData += 4;
			pCurrData += this._breakBodyPartInjuryCount.Serialize(pCurrData);
			int fieldSize168 = (int)((long)(pCurrData - pBegin168) - 4L);
			bool flag168 = fieldSize168 > 4194304;
			if (flag168)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Size of field ");
				defaultInterpolatedStringHandler.AppendFormatted("_breakBodyPartInjuryCount");
				defaultInterpolatedStringHandler.AppendLiteral(" must be less than ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(4096);
				defaultInterpolatedStringHandler.AppendLiteral("KB");
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			*(int*)pBegin168 = fieldSize168;
			byte* pBegin169 = pCurrData;
			pCurrData += 4;
			pCurrData += this._bodyPartIsBroken.Serialize(pCurrData);
			int fieldSize169 = (int)((long)(pCurrData - pBegin169) - 4L);
			bool flag169 = fieldSize169 > 4194304;
			if (flag169)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Size of field ");
				defaultInterpolatedStringHandler.AppendFormatted("_bodyPartIsBroken");
				defaultInterpolatedStringHandler.AppendLiteral(" must be less than ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(4096);
				defaultInterpolatedStringHandler.AppendLiteral("KB");
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			*(int*)pBegin169 = fieldSize169;
			byte* pBegin170 = pCurrData;
			pCurrData += 4;
			pCurrData += this._maxTrickCount.Serialize(pCurrData);
			int fieldSize170 = (int)((long)(pCurrData - pBegin170) - 4L);
			bool flag170 = fieldSize170 > 4194304;
			if (flag170)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Size of field ");
				defaultInterpolatedStringHandler.AppendFormatted("_maxTrickCount");
				defaultInterpolatedStringHandler.AppendLiteral(" must be less than ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(4096);
				defaultInterpolatedStringHandler.AppendLiteral("KB");
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			*(int*)pBegin170 = fieldSize170;
			byte* pBegin171 = pCurrData;
			pCurrData += 4;
			pCurrData += this._maxBreathPercent.Serialize(pCurrData);
			int fieldSize171 = (int)((long)(pCurrData - pBegin171) - 4L);
			bool flag171 = fieldSize171 > 4194304;
			if (flag171)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Size of field ");
				defaultInterpolatedStringHandler.AppendFormatted("_maxBreathPercent");
				defaultInterpolatedStringHandler.AppendLiteral(" must be less than ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(4096);
				defaultInterpolatedStringHandler.AppendLiteral("KB");
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			*(int*)pBegin171 = fieldSize171;
			byte* pBegin172 = pCurrData;
			pCurrData += 4;
			pCurrData += this._maxStancePercent.Serialize(pCurrData);
			int fieldSize172 = (int)((long)(pCurrData - pBegin172) - 4L);
			bool flag172 = fieldSize172 > 4194304;
			if (flag172)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Size of field ");
				defaultInterpolatedStringHandler.AppendFormatted("_maxStancePercent");
				defaultInterpolatedStringHandler.AppendLiteral(" must be less than ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(4096);
				defaultInterpolatedStringHandler.AppendLiteral("KB");
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			*(int*)pBegin172 = fieldSize172;
			byte* pBegin173 = pCurrData;
			pCurrData += 4;
			pCurrData += this._extraBreathPercent.Serialize(pCurrData);
			int fieldSize173 = (int)((long)(pCurrData - pBegin173) - 4L);
			bool flag173 = fieldSize173 > 4194304;
			if (flag173)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Size of field ");
				defaultInterpolatedStringHandler.AppendFormatted("_extraBreathPercent");
				defaultInterpolatedStringHandler.AppendLiteral(" must be less than ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(4096);
				defaultInterpolatedStringHandler.AppendLiteral("KB");
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			*(int*)pBegin173 = fieldSize173;
			byte* pBegin174 = pCurrData;
			pCurrData += 4;
			pCurrData += this._extraStancePercent.Serialize(pCurrData);
			int fieldSize174 = (int)((long)(pCurrData - pBegin174) - 4L);
			bool flag174 = fieldSize174 > 4194304;
			if (flag174)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Size of field ");
				defaultInterpolatedStringHandler.AppendFormatted("_extraStancePercent");
				defaultInterpolatedStringHandler.AppendLiteral(" must be less than ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(4096);
				defaultInterpolatedStringHandler.AppendLiteral("KB");
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			*(int*)pBegin174 = fieldSize174;
			byte* pBegin175 = pCurrData;
			pCurrData += 4;
			pCurrData += this._moveCostMobility.Serialize(pCurrData);
			int fieldSize175 = (int)((long)(pCurrData - pBegin175) - 4L);
			bool flag175 = fieldSize175 > 4194304;
			if (flag175)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Size of field ");
				defaultInterpolatedStringHandler.AppendFormatted("_moveCostMobility");
				defaultInterpolatedStringHandler.AppendLiteral(" must be less than ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(4096);
				defaultInterpolatedStringHandler.AppendLiteral("KB");
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			*(int*)pBegin175 = fieldSize175;
			byte* pBegin176 = pCurrData;
			pCurrData += 4;
			pCurrData += this._defendSkillKeepTime.Serialize(pCurrData);
			int fieldSize176 = (int)((long)(pCurrData - pBegin176) - 4L);
			bool flag176 = fieldSize176 > 4194304;
			if (flag176)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Size of field ");
				defaultInterpolatedStringHandler.AppendFormatted("_defendSkillKeepTime");
				defaultInterpolatedStringHandler.AppendLiteral(" must be less than ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(4096);
				defaultInterpolatedStringHandler.AppendLiteral("KB");
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			*(int*)pBegin176 = fieldSize176;
			byte* pBegin177 = pCurrData;
			pCurrData += 4;
			pCurrData += this._bounceRange.Serialize(pCurrData);
			int fieldSize177 = (int)((long)(pCurrData - pBegin177) - 4L);
			bool flag177 = fieldSize177 > 4194304;
			if (flag177)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Size of field ");
				defaultInterpolatedStringHandler.AppendFormatted("_bounceRange");
				defaultInterpolatedStringHandler.AppendLiteral(" must be less than ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(4096);
				defaultInterpolatedStringHandler.AppendLiteral("KB");
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			*(int*)pBegin177 = fieldSize177;
			byte* pBegin178 = pCurrData;
			pCurrData += 4;
			pCurrData += this._mindMarkKeepTime.Serialize(pCurrData);
			int fieldSize178 = (int)((long)(pCurrData - pBegin178) - 4L);
			bool flag178 = fieldSize178 > 4194304;
			if (flag178)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Size of field ");
				defaultInterpolatedStringHandler.AppendFormatted("_mindMarkKeepTime");
				defaultInterpolatedStringHandler.AppendLiteral(" must be less than ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(4096);
				defaultInterpolatedStringHandler.AppendLiteral("KB");
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			*(int*)pBegin178 = fieldSize178;
			byte* pBegin179 = pCurrData;
			pCurrData += 4;
			pCurrData += this._skillMobilityCostPerFrame.Serialize(pCurrData);
			int fieldSize179 = (int)((long)(pCurrData - pBegin179) - 4L);
			bool flag179 = fieldSize179 > 4194304;
			if (flag179)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Size of field ");
				defaultInterpolatedStringHandler.AppendFormatted("_skillMobilityCostPerFrame");
				defaultInterpolatedStringHandler.AppendLiteral(" must be less than ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(4096);
				defaultInterpolatedStringHandler.AppendLiteral("KB");
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			*(int*)pBegin179 = fieldSize179;
			byte* pBegin180 = pCurrData;
			pCurrData += 4;
			pCurrData += this._canAddWug.Serialize(pCurrData);
			int fieldSize180 = (int)((long)(pCurrData - pBegin180) - 4L);
			bool flag180 = fieldSize180 > 4194304;
			if (flag180)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Size of field ");
				defaultInterpolatedStringHandler.AppendFormatted("_canAddWug");
				defaultInterpolatedStringHandler.AppendLiteral(" must be less than ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(4096);
				defaultInterpolatedStringHandler.AppendLiteral("KB");
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			*(int*)pBegin180 = fieldSize180;
			byte* pBegin181 = pCurrData;
			pCurrData += 4;
			pCurrData += this._hasGodWeaponBuff.Serialize(pCurrData);
			int fieldSize181 = (int)((long)(pCurrData - pBegin181) - 4L);
			bool flag181 = fieldSize181 > 4194304;
			if (flag181)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Size of field ");
				defaultInterpolatedStringHandler.AppendFormatted("_hasGodWeaponBuff");
				defaultInterpolatedStringHandler.AppendLiteral(" must be less than ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(4096);
				defaultInterpolatedStringHandler.AppendLiteral("KB");
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			*(int*)pBegin181 = fieldSize181;
			byte* pBegin182 = pCurrData;
			pCurrData += 4;
			pCurrData += this._hasGodArmorBuff.Serialize(pCurrData);
			int fieldSize182 = (int)((long)(pCurrData - pBegin182) - 4L);
			bool flag182 = fieldSize182 > 4194304;
			if (flag182)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Size of field ");
				defaultInterpolatedStringHandler.AppendFormatted("_hasGodArmorBuff");
				defaultInterpolatedStringHandler.AppendLiteral(" must be less than ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(4096);
				defaultInterpolatedStringHandler.AppendLiteral("KB");
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			*(int*)pBegin182 = fieldSize182;
			byte* pBegin183 = pCurrData;
			pCurrData += 4;
			pCurrData += this._teammateCmdRequireGenerateValue.Serialize(pCurrData);
			int fieldSize183 = (int)((long)(pCurrData - pBegin183) - 4L);
			bool flag183 = fieldSize183 > 4194304;
			if (flag183)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Size of field ");
				defaultInterpolatedStringHandler.AppendFormatted("_teammateCmdRequireGenerateValue");
				defaultInterpolatedStringHandler.AppendLiteral(" must be less than ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(4096);
				defaultInterpolatedStringHandler.AppendLiteral("KB");
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			*(int*)pBegin183 = fieldSize183;
			byte* pBegin184 = pCurrData;
			pCurrData += 4;
			pCurrData += this._teammateCmdEffect.Serialize(pCurrData);
			int fieldSize184 = (int)((long)(pCurrData - pBegin184) - 4L);
			bool flag184 = fieldSize184 > 4194304;
			if (flag184)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Size of field ");
				defaultInterpolatedStringHandler.AppendFormatted("_teammateCmdEffect");
				defaultInterpolatedStringHandler.AppendLiteral(" must be less than ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(4096);
				defaultInterpolatedStringHandler.AppendLiteral("KB");
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			*(int*)pBegin184 = fieldSize184;
			byte* pBegin185 = pCurrData;
			pCurrData += 4;
			pCurrData += this._flawRecoverSpeed.Serialize(pCurrData);
			int fieldSize185 = (int)((long)(pCurrData - pBegin185) - 4L);
			bool flag185 = fieldSize185 > 4194304;
			if (flag185)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Size of field ");
				defaultInterpolatedStringHandler.AppendFormatted("_flawRecoverSpeed");
				defaultInterpolatedStringHandler.AppendLiteral(" must be less than ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(4096);
				defaultInterpolatedStringHandler.AppendLiteral("KB");
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			*(int*)pBegin185 = fieldSize185;
			byte* pBegin186 = pCurrData;
			pCurrData += 4;
			pCurrData += this._acupointRecoverSpeed.Serialize(pCurrData);
			int fieldSize186 = (int)((long)(pCurrData - pBegin186) - 4L);
			bool flag186 = fieldSize186 > 4194304;
			if (flag186)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Size of field ");
				defaultInterpolatedStringHandler.AppendFormatted("_acupointRecoverSpeed");
				defaultInterpolatedStringHandler.AppendLiteral(" must be less than ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(4096);
				defaultInterpolatedStringHandler.AppendLiteral("KB");
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			*(int*)pBegin186 = fieldSize186;
			byte* pBegin187 = pCurrData;
			pCurrData += 4;
			pCurrData += this._mindMarkRecoverSpeed.Serialize(pCurrData);
			int fieldSize187 = (int)((long)(pCurrData - pBegin187) - 4L);
			bool flag187 = fieldSize187 > 4194304;
			if (flag187)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Size of field ");
				defaultInterpolatedStringHandler.AppendFormatted("_mindMarkRecoverSpeed");
				defaultInterpolatedStringHandler.AppendLiteral(" must be less than ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(4096);
				defaultInterpolatedStringHandler.AppendLiteral("KB");
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			*(int*)pBegin187 = fieldSize187;
			byte* pBegin188 = pCurrData;
			pCurrData += 4;
			pCurrData += this._injuryAutoHealSpeed.Serialize(pCurrData);
			int fieldSize188 = (int)((long)(pCurrData - pBegin188) - 4L);
			bool flag188 = fieldSize188 > 4194304;
			if (flag188)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Size of field ");
				defaultInterpolatedStringHandler.AppendFormatted("_injuryAutoHealSpeed");
				defaultInterpolatedStringHandler.AppendLiteral(" must be less than ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(4096);
				defaultInterpolatedStringHandler.AppendLiteral("KB");
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			*(int*)pBegin188 = fieldSize188;
			byte* pBegin189 = pCurrData;
			pCurrData += 4;
			pCurrData += this._canRecoverBreath.Serialize(pCurrData);
			int fieldSize189 = (int)((long)(pCurrData - pBegin189) - 4L);
			bool flag189 = fieldSize189 > 4194304;
			if (flag189)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Size of field ");
				defaultInterpolatedStringHandler.AppendFormatted("_canRecoverBreath");
				defaultInterpolatedStringHandler.AppendLiteral(" must be less than ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(4096);
				defaultInterpolatedStringHandler.AppendLiteral("KB");
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			*(int*)pBegin189 = fieldSize189;
			byte* pBegin190 = pCurrData;
			pCurrData += 4;
			pCurrData += this._canRecoverStance.Serialize(pCurrData);
			int fieldSize190 = (int)((long)(pCurrData - pBegin190) - 4L);
			bool flag190 = fieldSize190 > 4194304;
			if (flag190)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Size of field ");
				defaultInterpolatedStringHandler.AppendFormatted("_canRecoverStance");
				defaultInterpolatedStringHandler.AppendLiteral(" must be less than ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(4096);
				defaultInterpolatedStringHandler.AppendLiteral("KB");
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			*(int*)pBegin190 = fieldSize190;
			byte* pBegin191 = pCurrData;
			pCurrData += 4;
			pCurrData += this._fatalDamageValue.Serialize(pCurrData);
			int fieldSize191 = (int)((long)(pCurrData - pBegin191) - 4L);
			bool flag191 = fieldSize191 > 4194304;
			if (flag191)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Size of field ");
				defaultInterpolatedStringHandler.AppendFormatted("_fatalDamageValue");
				defaultInterpolatedStringHandler.AppendLiteral(" must be less than ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(4096);
				defaultInterpolatedStringHandler.AppendLiteral("KB");
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			*(int*)pBegin191 = fieldSize191;
			byte* pBegin192 = pCurrData;
			pCurrData += 4;
			pCurrData += this._fatalDamageMarkCount.Serialize(pCurrData);
			int fieldSize192 = (int)((long)(pCurrData - pBegin192) - 4L);
			bool flag192 = fieldSize192 > 4194304;
			if (flag192)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Size of field ");
				defaultInterpolatedStringHandler.AppendFormatted("_fatalDamageMarkCount");
				defaultInterpolatedStringHandler.AppendLiteral(" must be less than ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(4096);
				defaultInterpolatedStringHandler.AppendLiteral("KB");
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			*(int*)pBegin192 = fieldSize192;
			byte* pBegin193 = pCurrData;
			pCurrData += 4;
			pCurrData += this._canFightBackDuringPrepareSkill.Serialize(pCurrData);
			int fieldSize193 = (int)((long)(pCurrData - pBegin193) - 4L);
			bool flag193 = fieldSize193 > 4194304;
			if (flag193)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Size of field ");
				defaultInterpolatedStringHandler.AppendFormatted("_canFightBackDuringPrepareSkill");
				defaultInterpolatedStringHandler.AppendLiteral(" must be less than ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(4096);
				defaultInterpolatedStringHandler.AppendLiteral("KB");
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			*(int*)pBegin193 = fieldSize193;
			byte* pBegin194 = pCurrData;
			pCurrData += 4;
			pCurrData += this._skillPrepareSpeed.Serialize(pCurrData);
			int fieldSize194 = (int)((long)(pCurrData - pBegin194) - 4L);
			bool flag194 = fieldSize194 > 4194304;
			if (flag194)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Size of field ");
				defaultInterpolatedStringHandler.AppendFormatted("_skillPrepareSpeed");
				defaultInterpolatedStringHandler.AppendLiteral(" must be less than ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(4096);
				defaultInterpolatedStringHandler.AppendLiteral("KB");
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			*(int*)pBegin194 = fieldSize194;
			byte* pBegin195 = pCurrData;
			pCurrData += 4;
			pCurrData += this._breathRecoverSpeed.Serialize(pCurrData);
			int fieldSize195 = (int)((long)(pCurrData - pBegin195) - 4L);
			bool flag195 = fieldSize195 > 4194304;
			if (flag195)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Size of field ");
				defaultInterpolatedStringHandler.AppendFormatted("_breathRecoverSpeed");
				defaultInterpolatedStringHandler.AppendLiteral(" must be less than ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(4096);
				defaultInterpolatedStringHandler.AppendLiteral("KB");
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			*(int*)pBegin195 = fieldSize195;
			byte* pBegin196 = pCurrData;
			pCurrData += 4;
			pCurrData += this._stanceRecoverSpeed.Serialize(pCurrData);
			int fieldSize196 = (int)((long)(pCurrData - pBegin196) - 4L);
			bool flag196 = fieldSize196 > 4194304;
			if (flag196)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Size of field ");
				defaultInterpolatedStringHandler.AppendFormatted("_stanceRecoverSpeed");
				defaultInterpolatedStringHandler.AppendLiteral(" must be less than ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(4096);
				defaultInterpolatedStringHandler.AppendLiteral("KB");
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			*(int*)pBegin196 = fieldSize196;
			byte* pBegin197 = pCurrData;
			pCurrData += 4;
			pCurrData += this._mobilityRecoverSpeed.Serialize(pCurrData);
			int fieldSize197 = (int)((long)(pCurrData - pBegin197) - 4L);
			bool flag197 = fieldSize197 > 4194304;
			if (flag197)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Size of field ");
				defaultInterpolatedStringHandler.AppendFormatted("_mobilityRecoverSpeed");
				defaultInterpolatedStringHandler.AppendLiteral(" must be less than ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(4096);
				defaultInterpolatedStringHandler.AppendLiteral("KB");
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			*(int*)pBegin197 = fieldSize197;
			byte* pBegin198 = pCurrData;
			pCurrData += 4;
			pCurrData += this._changeTrickProgressAddValue.Serialize(pCurrData);
			int fieldSize198 = (int)((long)(pCurrData - pBegin198) - 4L);
			bool flag198 = fieldSize198 > 4194304;
			if (flag198)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Size of field ");
				defaultInterpolatedStringHandler.AppendFormatted("_changeTrickProgressAddValue");
				defaultInterpolatedStringHandler.AppendLiteral(" must be less than ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(4096);
				defaultInterpolatedStringHandler.AppendLiteral("KB");
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			*(int*)pBegin198 = fieldSize198;
			byte* pBegin199 = pCurrData;
			pCurrData += 4;
			pCurrData += this._power.Serialize(pCurrData);
			int fieldSize199 = (int)((long)(pCurrData - pBegin199) - 4L);
			bool flag199 = fieldSize199 > 4194304;
			if (flag199)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Size of field ");
				defaultInterpolatedStringHandler.AppendFormatted("_power");
				defaultInterpolatedStringHandler.AppendLiteral(" must be less than ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(4096);
				defaultInterpolatedStringHandler.AppendLiteral("KB");
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			*(int*)pBegin199 = fieldSize199;
			byte* pBegin200 = pCurrData;
			pCurrData += 4;
			pCurrData += this._maxPower.Serialize(pCurrData);
			int fieldSize200 = (int)((long)(pCurrData - pBegin200) - 4L);
			bool flag200 = fieldSize200 > 4194304;
			if (flag200)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Size of field ");
				defaultInterpolatedStringHandler.AppendFormatted("_maxPower");
				defaultInterpolatedStringHandler.AppendLiteral(" must be less than ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(4096);
				defaultInterpolatedStringHandler.AppendLiteral("KB");
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			*(int*)pBegin200 = fieldSize200;
			byte* pBegin201 = pCurrData;
			pCurrData += 4;
			pCurrData += this._powerCanReduce.Serialize(pCurrData);
			int fieldSize201 = (int)((long)(pCurrData - pBegin201) - 4L);
			bool flag201 = fieldSize201 > 4194304;
			if (flag201)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Size of field ");
				defaultInterpolatedStringHandler.AppendFormatted("_powerCanReduce");
				defaultInterpolatedStringHandler.AppendLiteral(" must be less than ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(4096);
				defaultInterpolatedStringHandler.AppendLiteral("KB");
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			*(int*)pBegin201 = fieldSize201;
			byte* pBegin202 = pCurrData;
			pCurrData += 4;
			pCurrData += this._useRequirement.Serialize(pCurrData);
			int fieldSize202 = (int)((long)(pCurrData - pBegin202) - 4L);
			bool flag202 = fieldSize202 > 4194304;
			if (flag202)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Size of field ");
				defaultInterpolatedStringHandler.AppendFormatted("_useRequirement");
				defaultInterpolatedStringHandler.AppendLiteral(" must be less than ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(4096);
				defaultInterpolatedStringHandler.AppendLiteral("KB");
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			*(int*)pBegin202 = fieldSize202;
			byte* pBegin203 = pCurrData;
			pCurrData += 4;
			pCurrData += this._currInnerRatio.Serialize(pCurrData);
			int fieldSize203 = (int)((long)(pCurrData - pBegin203) - 4L);
			bool flag203 = fieldSize203 > 4194304;
			if (flag203)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Size of field ");
				defaultInterpolatedStringHandler.AppendFormatted("_currInnerRatio");
				defaultInterpolatedStringHandler.AppendLiteral(" must be less than ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(4096);
				defaultInterpolatedStringHandler.AppendLiteral("KB");
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			*(int*)pBegin203 = fieldSize203;
			byte* pBegin204 = pCurrData;
			pCurrData += 4;
			pCurrData += this._costBreathAndStance.Serialize(pCurrData);
			int fieldSize204 = (int)((long)(pCurrData - pBegin204) - 4L);
			bool flag204 = fieldSize204 > 4194304;
			if (flag204)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Size of field ");
				defaultInterpolatedStringHandler.AppendFormatted("_costBreathAndStance");
				defaultInterpolatedStringHandler.AppendLiteral(" must be less than ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(4096);
				defaultInterpolatedStringHandler.AppendLiteral("KB");
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			*(int*)pBegin204 = fieldSize204;
			byte* pBegin205 = pCurrData;
			pCurrData += 4;
			pCurrData += this._costBreath.Serialize(pCurrData);
			int fieldSize205 = (int)((long)(pCurrData - pBegin205) - 4L);
			bool flag205 = fieldSize205 > 4194304;
			if (flag205)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Size of field ");
				defaultInterpolatedStringHandler.AppendFormatted("_costBreath");
				defaultInterpolatedStringHandler.AppendLiteral(" must be less than ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(4096);
				defaultInterpolatedStringHandler.AppendLiteral("KB");
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			*(int*)pBegin205 = fieldSize205;
			byte* pBegin206 = pCurrData;
			pCurrData += 4;
			pCurrData += this._costStance.Serialize(pCurrData);
			int fieldSize206 = (int)((long)(pCurrData - pBegin206) - 4L);
			bool flag206 = fieldSize206 > 4194304;
			if (flag206)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Size of field ");
				defaultInterpolatedStringHandler.AppendFormatted("_costStance");
				defaultInterpolatedStringHandler.AppendLiteral(" must be less than ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(4096);
				defaultInterpolatedStringHandler.AppendLiteral("KB");
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			*(int*)pBegin206 = fieldSize206;
			byte* pBegin207 = pCurrData;
			pCurrData += 4;
			pCurrData += this._costMobility.Serialize(pCurrData);
			int fieldSize207 = (int)((long)(pCurrData - pBegin207) - 4L);
			bool flag207 = fieldSize207 > 4194304;
			if (flag207)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Size of field ");
				defaultInterpolatedStringHandler.AppendFormatted("_costMobility");
				defaultInterpolatedStringHandler.AppendLiteral(" must be less than ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(4096);
				defaultInterpolatedStringHandler.AppendLiteral("KB");
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			*(int*)pBegin207 = fieldSize207;
			byte* pBegin208 = pCurrData;
			pCurrData += 4;
			pCurrData += this._skillCostTricks.Serialize(pCurrData);
			int fieldSize208 = (int)((long)(pCurrData - pBegin208) - 4L);
			bool flag208 = fieldSize208 > 4194304;
			if (flag208)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Size of field ");
				defaultInterpolatedStringHandler.AppendFormatted("_skillCostTricks");
				defaultInterpolatedStringHandler.AppendLiteral(" must be less than ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(4096);
				defaultInterpolatedStringHandler.AppendLiteral("KB");
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			*(int*)pBegin208 = fieldSize208;
			byte* pBegin209 = pCurrData;
			pCurrData += 4;
			pCurrData += this._effectDirection.Serialize(pCurrData);
			int fieldSize209 = (int)((long)(pCurrData - pBegin209) - 4L);
			bool flag209 = fieldSize209 > 4194304;
			if (flag209)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Size of field ");
				defaultInterpolatedStringHandler.AppendFormatted("_effectDirection");
				defaultInterpolatedStringHandler.AppendLiteral(" must be less than ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(4096);
				defaultInterpolatedStringHandler.AppendLiteral("KB");
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			*(int*)pBegin209 = fieldSize209;
			byte* pBegin210 = pCurrData;
			pCurrData += 4;
			pCurrData += this._effectDirectionCanChange.Serialize(pCurrData);
			int fieldSize210 = (int)((long)(pCurrData - pBegin210) - 4L);
			bool flag210 = fieldSize210 > 4194304;
			if (flag210)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Size of field ");
				defaultInterpolatedStringHandler.AppendFormatted("_effectDirectionCanChange");
				defaultInterpolatedStringHandler.AppendLiteral(" must be less than ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(4096);
				defaultInterpolatedStringHandler.AppendLiteral("KB");
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			*(int*)pBegin210 = fieldSize210;
			byte* pBegin211 = pCurrData;
			pCurrData += 4;
			pCurrData += this._gridCost.Serialize(pCurrData);
			int fieldSize211 = (int)((long)(pCurrData - pBegin211) - 4L);
			bool flag211 = fieldSize211 > 4194304;
			if (flag211)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Size of field ");
				defaultInterpolatedStringHandler.AppendFormatted("_gridCost");
				defaultInterpolatedStringHandler.AppendLiteral(" must be less than ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(4096);
				defaultInterpolatedStringHandler.AppendLiteral("KB");
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			*(int*)pBegin211 = fieldSize211;
			byte* pBegin212 = pCurrData;
			pCurrData += 4;
			pCurrData += this._prepareTotalProgress.Serialize(pCurrData);
			int fieldSize212 = (int)((long)(pCurrData - pBegin212) - 4L);
			bool flag212 = fieldSize212 > 4194304;
			if (flag212)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Size of field ");
				defaultInterpolatedStringHandler.AppendFormatted("_prepareTotalProgress");
				defaultInterpolatedStringHandler.AppendLiteral(" must be less than ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(4096);
				defaultInterpolatedStringHandler.AppendLiteral("KB");
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			*(int*)pBegin212 = fieldSize212;
			byte* pBegin213 = pCurrData;
			pCurrData += 4;
			pCurrData += this._specificGridCount.Serialize(pCurrData);
			int fieldSize213 = (int)((long)(pCurrData - pBegin213) - 4L);
			bool flag213 = fieldSize213 > 4194304;
			if (flag213)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Size of field ");
				defaultInterpolatedStringHandler.AppendFormatted("_specificGridCount");
				defaultInterpolatedStringHandler.AppendLiteral(" must be less than ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(4096);
				defaultInterpolatedStringHandler.AppendLiteral("KB");
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			*(int*)pBegin213 = fieldSize213;
			byte* pBegin214 = pCurrData;
			pCurrData += 4;
			pCurrData += this._genericGridCount.Serialize(pCurrData);
			int fieldSize214 = (int)((long)(pCurrData - pBegin214) - 4L);
			bool flag214 = fieldSize214 > 4194304;
			if (flag214)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Size of field ");
				defaultInterpolatedStringHandler.AppendFormatted("_genericGridCount");
				defaultInterpolatedStringHandler.AppendLiteral(" must be less than ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(4096);
				defaultInterpolatedStringHandler.AppendLiteral("KB");
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			*(int*)pBegin214 = fieldSize214;
			byte* pBegin215 = pCurrData;
			pCurrData += 4;
			pCurrData += this._canInterrupt.Serialize(pCurrData);
			int fieldSize215 = (int)((long)(pCurrData - pBegin215) - 4L);
			bool flag215 = fieldSize215 > 4194304;
			if (flag215)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Size of field ");
				defaultInterpolatedStringHandler.AppendFormatted("_canInterrupt");
				defaultInterpolatedStringHandler.AppendLiteral(" must be less than ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(4096);
				defaultInterpolatedStringHandler.AppendLiteral("KB");
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			*(int*)pBegin215 = fieldSize215;
			byte* pBegin216 = pCurrData;
			pCurrData += 4;
			pCurrData += this._interruptOdds.Serialize(pCurrData);
			int fieldSize216 = (int)((long)(pCurrData - pBegin216) - 4L);
			bool flag216 = fieldSize216 > 4194304;
			if (flag216)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Size of field ");
				defaultInterpolatedStringHandler.AppendFormatted("_interruptOdds");
				defaultInterpolatedStringHandler.AppendLiteral(" must be less than ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(4096);
				defaultInterpolatedStringHandler.AppendLiteral("KB");
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			*(int*)pBegin216 = fieldSize216;
			byte* pBegin217 = pCurrData;
			pCurrData += 4;
			pCurrData += this._canSilence.Serialize(pCurrData);
			int fieldSize217 = (int)((long)(pCurrData - pBegin217) - 4L);
			bool flag217 = fieldSize217 > 4194304;
			if (flag217)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Size of field ");
				defaultInterpolatedStringHandler.AppendFormatted("_canSilence");
				defaultInterpolatedStringHandler.AppendLiteral(" must be less than ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(4096);
				defaultInterpolatedStringHandler.AppendLiteral("KB");
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			*(int*)pBegin217 = fieldSize217;
			byte* pBegin218 = pCurrData;
			pCurrData += 4;
			pCurrData += this._silenceOdds.Serialize(pCurrData);
			int fieldSize218 = (int)((long)(pCurrData - pBegin218) - 4L);
			bool flag218 = fieldSize218 > 4194304;
			if (flag218)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Size of field ");
				defaultInterpolatedStringHandler.AppendFormatted("_silenceOdds");
				defaultInterpolatedStringHandler.AppendLiteral(" must be less than ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(4096);
				defaultInterpolatedStringHandler.AppendLiteral("KB");
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			*(int*)pBegin218 = fieldSize218;
			byte* pBegin219 = pCurrData;
			pCurrData += 4;
			pCurrData += this._canCastWithBrokenBodyPart.Serialize(pCurrData);
			int fieldSize219 = (int)((long)(pCurrData - pBegin219) - 4L);
			bool flag219 = fieldSize219 > 4194304;
			if (flag219)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Size of field ");
				defaultInterpolatedStringHandler.AppendFormatted("_canCastWithBrokenBodyPart");
				defaultInterpolatedStringHandler.AppendLiteral(" must be less than ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(4096);
				defaultInterpolatedStringHandler.AppendLiteral("KB");
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			*(int*)pBegin219 = fieldSize219;
			byte* pBegin220 = pCurrData;
			pCurrData += 4;
			pCurrData += this._addPowerCanBeRemoved.Serialize(pCurrData);
			int fieldSize220 = (int)((long)(pCurrData - pBegin220) - 4L);
			bool flag220 = fieldSize220 > 4194304;
			if (flag220)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Size of field ");
				defaultInterpolatedStringHandler.AppendFormatted("_addPowerCanBeRemoved");
				defaultInterpolatedStringHandler.AppendLiteral(" must be less than ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(4096);
				defaultInterpolatedStringHandler.AppendLiteral("KB");
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			*(int*)pBegin220 = fieldSize220;
			byte* pBegin221 = pCurrData;
			pCurrData += 4;
			pCurrData += this._skillType.Serialize(pCurrData);
			int fieldSize221 = (int)((long)(pCurrData - pBegin221) - 4L);
			bool flag221 = fieldSize221 > 4194304;
			if (flag221)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Size of field ");
				defaultInterpolatedStringHandler.AppendFormatted("_skillType");
				defaultInterpolatedStringHandler.AppendLiteral(" must be less than ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(4096);
				defaultInterpolatedStringHandler.AppendLiteral("KB");
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			*(int*)pBegin221 = fieldSize221;
			byte* pBegin222 = pCurrData;
			pCurrData += 4;
			pCurrData += this._effectCountCanChange.Serialize(pCurrData);
			int fieldSize222 = (int)((long)(pCurrData - pBegin222) - 4L);
			bool flag222 = fieldSize222 > 4194304;
			if (flag222)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Size of field ");
				defaultInterpolatedStringHandler.AppendFormatted("_effectCountCanChange");
				defaultInterpolatedStringHandler.AppendLiteral(" must be less than ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(4096);
				defaultInterpolatedStringHandler.AppendLiteral("KB");
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			*(int*)pBegin222 = fieldSize222;
			byte* pBegin223 = pCurrData;
			pCurrData += 4;
			pCurrData += this._canCastInDefend.Serialize(pCurrData);
			int fieldSize223 = (int)((long)(pCurrData - pBegin223) - 4L);
			bool flag223 = fieldSize223 > 4194304;
			if (flag223)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Size of field ");
				defaultInterpolatedStringHandler.AppendFormatted("_canCastInDefend");
				defaultInterpolatedStringHandler.AppendLiteral(" must be less than ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(4096);
				defaultInterpolatedStringHandler.AppendLiteral("KB");
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			*(int*)pBegin223 = fieldSize223;
			byte* pBegin224 = pCurrData;
			pCurrData += 4;
			pCurrData += this._hitDistribution.Serialize(pCurrData);
			int fieldSize224 = (int)((long)(pCurrData - pBegin224) - 4L);
			bool flag224 = fieldSize224 > 4194304;
			if (flag224)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Size of field ");
				defaultInterpolatedStringHandler.AppendFormatted("_hitDistribution");
				defaultInterpolatedStringHandler.AppendLiteral(" must be less than ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(4096);
				defaultInterpolatedStringHandler.AppendLiteral("KB");
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			*(int*)pBegin224 = fieldSize224;
			byte* pBegin225 = pCurrData;
			pCurrData += 4;
			pCurrData += this._canCastOnLackBreath.Serialize(pCurrData);
			int fieldSize225 = (int)((long)(pCurrData - pBegin225) - 4L);
			bool flag225 = fieldSize225 > 4194304;
			if (flag225)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Size of field ");
				defaultInterpolatedStringHandler.AppendFormatted("_canCastOnLackBreath");
				defaultInterpolatedStringHandler.AppendLiteral(" must be less than ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(4096);
				defaultInterpolatedStringHandler.AppendLiteral("KB");
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			*(int*)pBegin225 = fieldSize225;
			byte* pBegin226 = pCurrData;
			pCurrData += 4;
			pCurrData += this._canCastOnLackStance.Serialize(pCurrData);
			int fieldSize226 = (int)((long)(pCurrData - pBegin226) - 4L);
			bool flag226 = fieldSize226 > 4194304;
			if (flag226)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Size of field ");
				defaultInterpolatedStringHandler.AppendFormatted("_canCastOnLackStance");
				defaultInterpolatedStringHandler.AppendLiteral(" must be less than ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(4096);
				defaultInterpolatedStringHandler.AppendLiteral("KB");
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			*(int*)pBegin226 = fieldSize226;
			byte* pBegin227 = pCurrData;
			pCurrData += 4;
			pCurrData += this._costBreathOnCast.Serialize(pCurrData);
			int fieldSize227 = (int)((long)(pCurrData - pBegin227) - 4L);
			bool flag227 = fieldSize227 > 4194304;
			if (flag227)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Size of field ");
				defaultInterpolatedStringHandler.AppendFormatted("_costBreathOnCast");
				defaultInterpolatedStringHandler.AppendLiteral(" must be less than ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(4096);
				defaultInterpolatedStringHandler.AppendLiteral("KB");
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			*(int*)pBegin227 = fieldSize227;
			byte* pBegin228 = pCurrData;
			pCurrData += 4;
			pCurrData += this._costStanceOnCast.Serialize(pCurrData);
			int fieldSize228 = (int)((long)(pCurrData - pBegin228) - 4L);
			bool flag228 = fieldSize228 > 4194304;
			if (flag228)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Size of field ");
				defaultInterpolatedStringHandler.AppendFormatted("_costStanceOnCast");
				defaultInterpolatedStringHandler.AppendLiteral(" must be less than ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(4096);
				defaultInterpolatedStringHandler.AppendLiteral("KB");
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			*(int*)pBegin228 = fieldSize228;
			byte* pBegin229 = pCurrData;
			pCurrData += 4;
			pCurrData += this._canUseMobilityAsBreath.Serialize(pCurrData);
			int fieldSize229 = (int)((long)(pCurrData - pBegin229) - 4L);
			bool flag229 = fieldSize229 > 4194304;
			if (flag229)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Size of field ");
				defaultInterpolatedStringHandler.AppendFormatted("_canUseMobilityAsBreath");
				defaultInterpolatedStringHandler.AppendLiteral(" must be less than ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(4096);
				defaultInterpolatedStringHandler.AppendLiteral("KB");
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			*(int*)pBegin229 = fieldSize229;
			byte* pBegin230 = pCurrData;
			pCurrData += 4;
			pCurrData += this._canUseMobilityAsStance.Serialize(pCurrData);
			int fieldSize230 = (int)((long)(pCurrData - pBegin230) - 4L);
			bool flag230 = fieldSize230 > 4194304;
			if (flag230)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Size of field ");
				defaultInterpolatedStringHandler.AppendFormatted("_canUseMobilityAsStance");
				defaultInterpolatedStringHandler.AppendLiteral(" must be less than ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(4096);
				defaultInterpolatedStringHandler.AppendLiteral("KB");
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			*(int*)pBegin230 = fieldSize230;
			byte* pBegin231 = pCurrData;
			pCurrData += 4;
			pCurrData += this._castCostNeiliAllocation.Serialize(pCurrData);
			int fieldSize231 = (int)((long)(pCurrData - pBegin231) - 4L);
			bool flag231 = fieldSize231 > 4194304;
			if (flag231)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Size of field ");
				defaultInterpolatedStringHandler.AppendFormatted("_castCostNeiliAllocation");
				defaultInterpolatedStringHandler.AppendLiteral(" must be less than ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(4096);
				defaultInterpolatedStringHandler.AppendLiteral("KB");
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			*(int*)pBegin231 = fieldSize231;
			byte* pBegin232 = pCurrData;
			pCurrData += 4;
			pCurrData += this._acceptPoisonResist.Serialize(pCurrData);
			int fieldSize232 = (int)((long)(pCurrData - pBegin232) - 4L);
			bool flag232 = fieldSize232 > 4194304;
			if (flag232)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Size of field ");
				defaultInterpolatedStringHandler.AppendFormatted("_acceptPoisonResist");
				defaultInterpolatedStringHandler.AppendLiteral(" must be less than ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(4096);
				defaultInterpolatedStringHandler.AppendLiteral("KB");
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			*(int*)pBegin232 = fieldSize232;
			byte* pBegin233 = pCurrData;
			pCurrData += 4;
			pCurrData += this._makePoisonResist.Serialize(pCurrData);
			int fieldSize233 = (int)((long)(pCurrData - pBegin233) - 4L);
			bool flag233 = fieldSize233 > 4194304;
			if (flag233)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Size of field ");
				defaultInterpolatedStringHandler.AppendFormatted("_makePoisonResist");
				defaultInterpolatedStringHandler.AppendLiteral(" must be less than ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(4096);
				defaultInterpolatedStringHandler.AppendLiteral("KB");
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			*(int*)pBegin233 = fieldSize233;
			byte* pBegin234 = pCurrData;
			pCurrData += 4;
			pCurrData += this._canCriticalHit.Serialize(pCurrData);
			int fieldSize234 = (int)((long)(pCurrData - pBegin234) - 4L);
			bool flag234 = fieldSize234 > 4194304;
			if (flag234)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Size of field ");
				defaultInterpolatedStringHandler.AppendFormatted("_canCriticalHit");
				defaultInterpolatedStringHandler.AppendLiteral(" must be less than ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(4096);
				defaultInterpolatedStringHandler.AppendLiteral("KB");
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			*(int*)pBegin234 = fieldSize234;
			byte* pBegin235 = pCurrData;
			pCurrData += 4;
			pCurrData += this._canCostNeiliAllocationEffect.Serialize(pCurrData);
			int fieldSize235 = (int)((long)(pCurrData - pBegin235) - 4L);
			bool flag235 = fieldSize235 > 4194304;
			if (flag235)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Size of field ");
				defaultInterpolatedStringHandler.AppendFormatted("_canCostNeiliAllocationEffect");
				defaultInterpolatedStringHandler.AppendLiteral(" must be less than ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(4096);
				defaultInterpolatedStringHandler.AppendLiteral("KB");
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			*(int*)pBegin235 = fieldSize235;
			byte* pBegin236 = pCurrData;
			pCurrData += 4;
			pCurrData += this._consummateLevelRelatedMainAttributesHitValues.Serialize(pCurrData);
			int fieldSize236 = (int)((long)(pCurrData - pBegin236) - 4L);
			bool flag236 = fieldSize236 > 4194304;
			if (flag236)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Size of field ");
				defaultInterpolatedStringHandler.AppendFormatted("_consummateLevelRelatedMainAttributesHitValues");
				defaultInterpolatedStringHandler.AppendLiteral(" must be less than ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(4096);
				defaultInterpolatedStringHandler.AppendLiteral("KB");
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			*(int*)pBegin236 = fieldSize236;
			byte* pBegin237 = pCurrData;
			pCurrData += 4;
			pCurrData += this._consummateLevelRelatedMainAttributesAvoidValues.Serialize(pCurrData);
			int fieldSize237 = (int)((long)(pCurrData - pBegin237) - 4L);
			bool flag237 = fieldSize237 > 4194304;
			if (flag237)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Size of field ");
				defaultInterpolatedStringHandler.AppendFormatted("_consummateLevelRelatedMainAttributesAvoidValues");
				defaultInterpolatedStringHandler.AppendLiteral(" must be less than ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(4096);
				defaultInterpolatedStringHandler.AppendLiteral("KB");
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			*(int*)pBegin237 = fieldSize237;
			byte* pBegin238 = pCurrData;
			pCurrData += 4;
			pCurrData += this._consummateLevelRelatedMainAttributesPenetrations.Serialize(pCurrData);
			int fieldSize238 = (int)((long)(pCurrData - pBegin238) - 4L);
			bool flag238 = fieldSize238 > 4194304;
			if (flag238)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Size of field ");
				defaultInterpolatedStringHandler.AppendFormatted("_consummateLevelRelatedMainAttributesPenetrations");
				defaultInterpolatedStringHandler.AppendLiteral(" must be less than ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(4096);
				defaultInterpolatedStringHandler.AppendLiteral("KB");
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			*(int*)pBegin238 = fieldSize238;
			byte* pBegin239 = pCurrData;
			pCurrData += 4;
			pCurrData += this._consummateLevelRelatedMainAttributesPenetrationResists.Serialize(pCurrData);
			int fieldSize239 = (int)((long)(pCurrData - pBegin239) - 4L);
			bool flag239 = fieldSize239 > 4194304;
			if (flag239)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Size of field ");
				defaultInterpolatedStringHandler.AppendFormatted("_consummateLevelRelatedMainAttributesPenetrationResists");
				defaultInterpolatedStringHandler.AppendLiteral(" must be less than ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(4096);
				defaultInterpolatedStringHandler.AppendLiteral("KB");
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			*(int*)pBegin239 = fieldSize239;
			byte* pBegin240 = pCurrData;
			pCurrData += 4;
			pCurrData += this._skillAlsoAsFiveElements.Serialize(pCurrData);
			int fieldSize240 = (int)((long)(pCurrData - pBegin240) - 4L);
			bool flag240 = fieldSize240 > 4194304;
			if (flag240)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Size of field ");
				defaultInterpolatedStringHandler.AppendFormatted("_skillAlsoAsFiveElements");
				defaultInterpolatedStringHandler.AppendLiteral(" must be less than ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(4096);
				defaultInterpolatedStringHandler.AppendLiteral("KB");
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			*(int*)pBegin240 = fieldSize240;
			byte* pBegin241 = pCurrData;
			pCurrData += 4;
			pCurrData += this._innerInjuryImmunity.Serialize(pCurrData);
			int fieldSize241 = (int)((long)(pCurrData - pBegin241) - 4L);
			bool flag241 = fieldSize241 > 4194304;
			if (flag241)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Size of field ");
				defaultInterpolatedStringHandler.AppendFormatted("_innerInjuryImmunity");
				defaultInterpolatedStringHandler.AppendLiteral(" must be less than ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(4096);
				defaultInterpolatedStringHandler.AppendLiteral("KB");
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			*(int*)pBegin241 = fieldSize241;
			byte* pBegin242 = pCurrData;
			pCurrData += 4;
			pCurrData += this._outerInjuryImmunity.Serialize(pCurrData);
			int fieldSize242 = (int)((long)(pCurrData - pBegin242) - 4L);
			bool flag242 = fieldSize242 > 4194304;
			if (flag242)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Size of field ");
				defaultInterpolatedStringHandler.AppendFormatted("_outerInjuryImmunity");
				defaultInterpolatedStringHandler.AppendLiteral(" must be less than ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(4096);
				defaultInterpolatedStringHandler.AppendLiteral("KB");
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			*(int*)pBegin242 = fieldSize242;
			byte* pBegin243 = pCurrData;
			pCurrData += 4;
			pCurrData += this._poisonAffectThreshold.Serialize(pCurrData);
			int fieldSize243 = (int)((long)(pCurrData - pBegin243) - 4L);
			bool flag243 = fieldSize243 > 4194304;
			if (flag243)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Size of field ");
				defaultInterpolatedStringHandler.AppendFormatted("_poisonAffectThreshold");
				defaultInterpolatedStringHandler.AppendLiteral(" must be less than ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(4096);
				defaultInterpolatedStringHandler.AppendLiteral("KB");
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			*(int*)pBegin243 = fieldSize243;
			byte* pBegin244 = pCurrData;
			pCurrData += 4;
			pCurrData += this._lockDistance.Serialize(pCurrData);
			int fieldSize244 = (int)((long)(pCurrData - pBegin244) - 4L);
			bool flag244 = fieldSize244 > 4194304;
			if (flag244)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Size of field ");
				defaultInterpolatedStringHandler.AppendFormatted("_lockDistance");
				defaultInterpolatedStringHandler.AppendLiteral(" must be less than ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(4096);
				defaultInterpolatedStringHandler.AppendLiteral("KB");
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			*(int*)pBegin244 = fieldSize244;
			byte* pBegin245 = pCurrData;
			pCurrData += 4;
			pCurrData += this._resistOfAllPoison.Serialize(pCurrData);
			int fieldSize245 = (int)((long)(pCurrData - pBegin245) - 4L);
			bool flag245 = fieldSize245 > 4194304;
			if (flag245)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Size of field ");
				defaultInterpolatedStringHandler.AppendFormatted("_resistOfAllPoison");
				defaultInterpolatedStringHandler.AppendLiteral(" must be less than ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(4096);
				defaultInterpolatedStringHandler.AppendLiteral("KB");
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			*(int*)pBegin245 = fieldSize245;
			byte* pBegin246 = pCurrData;
			pCurrData += 4;
			pCurrData += this._makePoisonTarget.Serialize(pCurrData);
			int fieldSize246 = (int)((long)(pCurrData - pBegin246) - 4L);
			bool flag246 = fieldSize246 > 4194304;
			if (flag246)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Size of field ");
				defaultInterpolatedStringHandler.AppendFormatted("_makePoisonTarget");
				defaultInterpolatedStringHandler.AppendLiteral(" must be less than ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(4096);
				defaultInterpolatedStringHandler.AppendLiteral("KB");
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			*(int*)pBegin246 = fieldSize246;
			byte* pBegin247 = pCurrData;
			pCurrData += 4;
			pCurrData += this._acceptPoisonTarget.Serialize(pCurrData);
			int fieldSize247 = (int)((long)(pCurrData - pBegin247) - 4L);
			bool flag247 = fieldSize247 > 4194304;
			if (flag247)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Size of field ");
				defaultInterpolatedStringHandler.AppendFormatted("_acceptPoisonTarget");
				defaultInterpolatedStringHandler.AppendLiteral(" must be less than ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(4096);
				defaultInterpolatedStringHandler.AppendLiteral("KB");
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			*(int*)pBegin247 = fieldSize247;
			byte* pBegin248 = pCurrData;
			pCurrData += 4;
			pCurrData += this._certainCriticalHit.Serialize(pCurrData);
			int fieldSize248 = (int)((long)(pCurrData - pBegin248) - 4L);
			bool flag248 = fieldSize248 > 4194304;
			if (flag248)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Size of field ");
				defaultInterpolatedStringHandler.AppendFormatted("_certainCriticalHit");
				defaultInterpolatedStringHandler.AppendLiteral(" must be less than ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(4096);
				defaultInterpolatedStringHandler.AppendLiteral("KB");
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			*(int*)pBegin248 = fieldSize248;
			byte* pBegin249 = pCurrData;
			pCurrData += 4;
			pCurrData += this._mindMarkCount.Serialize(pCurrData);
			int fieldSize249 = (int)((long)(pCurrData - pBegin249) - 4L);
			bool flag249 = fieldSize249 > 4194304;
			if (flag249)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Size of field ");
				defaultInterpolatedStringHandler.AppendFormatted("_mindMarkCount");
				defaultInterpolatedStringHandler.AppendLiteral(" must be less than ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(4096);
				defaultInterpolatedStringHandler.AppendLiteral("KB");
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			*(int*)pBegin249 = fieldSize249;
			byte* pBegin250 = pCurrData;
			pCurrData += 4;
			pCurrData += this._canFightBackWithHit.Serialize(pCurrData);
			int fieldSize250 = (int)((long)(pCurrData - pBegin250) - 4L);
			bool flag250 = fieldSize250 > 4194304;
			if (flag250)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Size of field ");
				defaultInterpolatedStringHandler.AppendFormatted("_canFightBackWithHit");
				defaultInterpolatedStringHandler.AppendLiteral(" must be less than ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(4096);
				defaultInterpolatedStringHandler.AppendLiteral("KB");
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			*(int*)pBegin250 = fieldSize250;
			byte* pBegin251 = pCurrData;
			pCurrData += 4;
			pCurrData += this._inevitableHit.Serialize(pCurrData);
			int fieldSize251 = (int)((long)(pCurrData - pBegin251) - 4L);
			bool flag251 = fieldSize251 > 4194304;
			if (flag251)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Size of field ");
				defaultInterpolatedStringHandler.AppendFormatted("_inevitableHit");
				defaultInterpolatedStringHandler.AppendLiteral(" must be less than ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(4096);
				defaultInterpolatedStringHandler.AppendLiteral("KB");
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			*(int*)pBegin251 = fieldSize251;
			byte* pBegin252 = pCurrData;
			pCurrData += 4;
			pCurrData += this._attackCanPursue.Serialize(pCurrData);
			int fieldSize252 = (int)((long)(pCurrData - pBegin252) - 4L);
			bool flag252 = fieldSize252 > 4194304;
			if (flag252)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Size of field ");
				defaultInterpolatedStringHandler.AppendFormatted("_attackCanPursue");
				defaultInterpolatedStringHandler.AppendLiteral(" must be less than ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(4096);
				defaultInterpolatedStringHandler.AppendLiteral("KB");
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			*(int*)pBegin252 = fieldSize252;
			byte* pBegin253 = pCurrData;
			pCurrData += 4;
			pCurrData += this._combatSkillDataEffectList.Serialize(pCurrData);
			int fieldSize253 = (int)((long)(pCurrData - pBegin253) - 4L);
			bool flag253 = fieldSize253 > 4194304;
			if (flag253)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Size of field ");
				defaultInterpolatedStringHandler.AppendFormatted("_combatSkillDataEffectList");
				defaultInterpolatedStringHandler.AppendLiteral(" must be less than ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(4096);
				defaultInterpolatedStringHandler.AppendLiteral("KB");
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			*(int*)pBegin253 = fieldSize253;
			byte* pBegin254 = pCurrData;
			pCurrData += 4;
			pCurrData += this._criticalOdds.Serialize(pCurrData);
			int fieldSize254 = (int)((long)(pCurrData - pBegin254) - 4L);
			bool flag254 = fieldSize254 > 4194304;
			if (flag254)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Size of field ");
				defaultInterpolatedStringHandler.AppendFormatted("_criticalOdds");
				defaultInterpolatedStringHandler.AppendLiteral(" must be less than ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(4096);
				defaultInterpolatedStringHandler.AppendLiteral("KB");
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			*(int*)pBegin254 = fieldSize254;
			byte* pBegin255 = pCurrData;
			pCurrData += 4;
			pCurrData += this._stanceCostByEffect.Serialize(pCurrData);
			int fieldSize255 = (int)((long)(pCurrData - pBegin255) - 4L);
			bool flag255 = fieldSize255 > 4194304;
			if (flag255)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Size of field ");
				defaultInterpolatedStringHandler.AppendFormatted("_stanceCostByEffect");
				defaultInterpolatedStringHandler.AppendLiteral(" must be less than ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(4096);
				defaultInterpolatedStringHandler.AppendLiteral("KB");
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			*(int*)pBegin255 = fieldSize255;
			byte* pBegin256 = pCurrData;
			pCurrData += 4;
			pCurrData += this._breathCostByEffect.Serialize(pCurrData);
			int fieldSize256 = (int)((long)(pCurrData - pBegin256) - 4L);
			bool flag256 = fieldSize256 > 4194304;
			if (flag256)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Size of field ");
				defaultInterpolatedStringHandler.AppendFormatted("_breathCostByEffect");
				defaultInterpolatedStringHandler.AppendLiteral(" must be less than ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(4096);
				defaultInterpolatedStringHandler.AppendLiteral("KB");
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			*(int*)pBegin256 = fieldSize256;
			byte* pBegin257 = pCurrData;
			pCurrData += 4;
			pCurrData += this._powerAddRatio.Serialize(pCurrData);
			int fieldSize257 = (int)((long)(pCurrData - pBegin257) - 4L);
			bool flag257 = fieldSize257 > 4194304;
			if (flag257)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Size of field ");
				defaultInterpolatedStringHandler.AppendFormatted("_powerAddRatio");
				defaultInterpolatedStringHandler.AppendLiteral(" must be less than ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(4096);
				defaultInterpolatedStringHandler.AppendLiteral("KB");
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			*(int*)pBegin257 = fieldSize257;
			byte* pBegin258 = pCurrData;
			pCurrData += 4;
			pCurrData += this._powerReduceRatio.Serialize(pCurrData);
			int fieldSize258 = (int)((long)(pCurrData - pBegin258) - 4L);
			bool flag258 = fieldSize258 > 4194304;
			if (flag258)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Size of field ");
				defaultInterpolatedStringHandler.AppendFormatted("_powerReduceRatio");
				defaultInterpolatedStringHandler.AppendLiteral(" must be less than ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(4096);
				defaultInterpolatedStringHandler.AppendLiteral("KB");
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			*(int*)pBegin258 = fieldSize258;
			byte* pBegin259 = pCurrData;
			pCurrData += 4;
			pCurrData += this._poisonAffectProduceValue.Serialize(pCurrData);
			int fieldSize259 = (int)((long)(pCurrData - pBegin259) - 4L);
			bool flag259 = fieldSize259 > 4194304;
			if (flag259)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Size of field ");
				defaultInterpolatedStringHandler.AppendFormatted("_poisonAffectProduceValue");
				defaultInterpolatedStringHandler.AppendLiteral(" must be less than ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(4096);
				defaultInterpolatedStringHandler.AppendLiteral("KB");
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			*(int*)pBegin259 = fieldSize259;
			byte* pBegin260 = pCurrData;
			pCurrData += 4;
			pCurrData += this._canReadingOnMonthChange.Serialize(pCurrData);
			int fieldSize260 = (int)((long)(pCurrData - pBegin260) - 4L);
			bool flag260 = fieldSize260 > 4194304;
			if (flag260)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Size of field ");
				defaultInterpolatedStringHandler.AppendFormatted("_canReadingOnMonthChange");
				defaultInterpolatedStringHandler.AppendLiteral(" must be less than ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(4096);
				defaultInterpolatedStringHandler.AppendLiteral("KB");
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			*(int*)pBegin260 = fieldSize260;
			byte* pBegin261 = pCurrData;
			pCurrData += 4;
			pCurrData += this._medicineEffect.Serialize(pCurrData);
			int fieldSize261 = (int)((long)(pCurrData - pBegin261) - 4L);
			bool flag261 = fieldSize261 > 4194304;
			if (flag261)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Size of field ");
				defaultInterpolatedStringHandler.AppendFormatted("_medicineEffect");
				defaultInterpolatedStringHandler.AppendLiteral(" must be less than ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(4096);
				defaultInterpolatedStringHandler.AppendLiteral("KB");
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			*(int*)pBegin261 = fieldSize261;
			byte* pBegin262 = pCurrData;
			pCurrData += 4;
			pCurrData += this._xiangshuInfectionDelta.Serialize(pCurrData);
			int fieldSize262 = (int)((long)(pCurrData - pBegin262) - 4L);
			bool flag262 = fieldSize262 > 4194304;
			if (flag262)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Size of field ");
				defaultInterpolatedStringHandler.AppendFormatted("_xiangshuInfectionDelta");
				defaultInterpolatedStringHandler.AppendLiteral(" must be less than ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(4096);
				defaultInterpolatedStringHandler.AppendLiteral("KB");
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			*(int*)pBegin262 = fieldSize262;
			byte* pBegin263 = pCurrData;
			pCurrData += 4;
			pCurrData += this._healthDelta.Serialize(pCurrData);
			int fieldSize263 = (int)((long)(pCurrData - pBegin263) - 4L);
			bool flag263 = fieldSize263 > 4194304;
			if (flag263)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Size of field ");
				defaultInterpolatedStringHandler.AppendFormatted("_healthDelta");
				defaultInterpolatedStringHandler.AppendLiteral(" must be less than ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(4096);
				defaultInterpolatedStringHandler.AppendLiteral("KB");
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			*(int*)pBegin263 = fieldSize263;
			byte* pBegin264 = pCurrData;
			pCurrData += 4;
			pCurrData += this._weaponSilenceFrame.Serialize(pCurrData);
			int fieldSize264 = (int)((long)(pCurrData - pBegin264) - 4L);
			bool flag264 = fieldSize264 > 4194304;
			if (flag264)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Size of field ");
				defaultInterpolatedStringHandler.AppendFormatted("_weaponSilenceFrame");
				defaultInterpolatedStringHandler.AppendLiteral(" must be less than ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(4096);
				defaultInterpolatedStringHandler.AppendLiteral("KB");
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			*(int*)pBegin264 = fieldSize264;
			byte* pBegin265 = pCurrData;
			pCurrData += 4;
			pCurrData += this._silenceFrame.Serialize(pCurrData);
			int fieldSize265 = (int)((long)(pCurrData - pBegin265) - 4L);
			bool flag265 = fieldSize265 > 4194304;
			if (flag265)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Size of field ");
				defaultInterpolatedStringHandler.AppendFormatted("_silenceFrame");
				defaultInterpolatedStringHandler.AppendLiteral(" must be less than ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(4096);
				defaultInterpolatedStringHandler.AppendLiteral("KB");
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			*(int*)pBegin265 = fieldSize265;
			byte* pBegin266 = pCurrData;
			pCurrData += 4;
			pCurrData += this._currAgeDelta.Serialize(pCurrData);
			int fieldSize266 = (int)((long)(pCurrData - pBegin266) - 4L);
			bool flag266 = fieldSize266 > 4194304;
			if (flag266)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Size of field ");
				defaultInterpolatedStringHandler.AppendFormatted("_currAgeDelta");
				defaultInterpolatedStringHandler.AppendLiteral(" must be less than ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(4096);
				defaultInterpolatedStringHandler.AppendLiteral("KB");
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			*(int*)pBegin266 = fieldSize266;
			byte* pBegin267 = pCurrData;
			pCurrData += 4;
			pCurrData += this._goneMadInAllBreak.Serialize(pCurrData);
			int fieldSize267 = (int)((long)(pCurrData - pBegin267) - 4L);
			bool flag267 = fieldSize267 > 4194304;
			if (flag267)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Size of field ");
				defaultInterpolatedStringHandler.AppendFormatted("_goneMadInAllBreak");
				defaultInterpolatedStringHandler.AppendLiteral(" must be less than ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(4096);
				defaultInterpolatedStringHandler.AppendLiteral("KB");
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			*(int*)pBegin267 = fieldSize267;
			byte* pBegin268 = pCurrData;
			pCurrData += 4;
			pCurrData += this._makeLoveRateOnMonthChange.Serialize(pCurrData);
			int fieldSize268 = (int)((long)(pCurrData - pBegin268) - 4L);
			bool flag268 = fieldSize268 > 4194304;
			if (flag268)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Size of field ");
				defaultInterpolatedStringHandler.AppendFormatted("_makeLoveRateOnMonthChange");
				defaultInterpolatedStringHandler.AppendLiteral(" must be less than ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(4096);
				defaultInterpolatedStringHandler.AppendLiteral("KB");
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			*(int*)pBegin268 = fieldSize268;
			byte* pBegin269 = pCurrData;
			pCurrData += 4;
			pCurrData += this._canAutoHealOnMonthChange.Serialize(pCurrData);
			int fieldSize269 = (int)((long)(pCurrData - pBegin269) - 4L);
			bool flag269 = fieldSize269 > 4194304;
			if (flag269)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Size of field ");
				defaultInterpolatedStringHandler.AppendFormatted("_canAutoHealOnMonthChange");
				defaultInterpolatedStringHandler.AppendLiteral(" must be less than ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(4096);
				defaultInterpolatedStringHandler.AppendLiteral("KB");
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			*(int*)pBegin269 = fieldSize269;
			byte* pBegin270 = pCurrData;
			pCurrData += 4;
			pCurrData += this._happinessDelta.Serialize(pCurrData);
			int fieldSize270 = (int)((long)(pCurrData - pBegin270) - 4L);
			bool flag270 = fieldSize270 > 4194304;
			if (flag270)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Size of field ");
				defaultInterpolatedStringHandler.AppendFormatted("_happinessDelta");
				defaultInterpolatedStringHandler.AppendLiteral(" must be less than ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(4096);
				defaultInterpolatedStringHandler.AppendLiteral("KB");
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			*(int*)pBegin270 = fieldSize270;
			byte* pBegin271 = pCurrData;
			pCurrData += 4;
			pCurrData += this._teammateCmdCanUse.Serialize(pCurrData);
			int fieldSize271 = (int)((long)(pCurrData - pBegin271) - 4L);
			bool flag271 = fieldSize271 > 4194304;
			if (flag271)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Size of field ");
				defaultInterpolatedStringHandler.AppendFormatted("_teammateCmdCanUse");
				defaultInterpolatedStringHandler.AppendLiteral(" must be less than ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(4096);
				defaultInterpolatedStringHandler.AppendLiteral("KB");
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			*(int*)pBegin271 = fieldSize271;
			byte* pBegin272 = pCurrData;
			pCurrData += 4;
			pCurrData += this._mixPoisonInfinityAffect.Serialize(pCurrData);
			int fieldSize272 = (int)((long)(pCurrData - pBegin272) - 4L);
			bool flag272 = fieldSize272 > 4194304;
			if (flag272)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Size of field ");
				defaultInterpolatedStringHandler.AppendFormatted("_mixPoisonInfinityAffect");
				defaultInterpolatedStringHandler.AppendLiteral(" must be less than ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(4096);
				defaultInterpolatedStringHandler.AppendLiteral("KB");
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			*(int*)pBegin272 = fieldSize272;
			byte* pBegin273 = pCurrData;
			pCurrData += 4;
			pCurrData += this._attackRangeMaxAcupoint.Serialize(pCurrData);
			int fieldSize273 = (int)((long)(pCurrData - pBegin273) - 4L);
			bool flag273 = fieldSize273 > 4194304;
			if (flag273)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Size of field ");
				defaultInterpolatedStringHandler.AppendFormatted("_attackRangeMaxAcupoint");
				defaultInterpolatedStringHandler.AppendLiteral(" must be less than ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(4096);
				defaultInterpolatedStringHandler.AppendLiteral("KB");
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			*(int*)pBegin273 = fieldSize273;
			byte* pBegin274 = pCurrData;
			pCurrData += 4;
			pCurrData += this._maxMobilityPercent.Serialize(pCurrData);
			int fieldSize274 = (int)((long)(pCurrData - pBegin274) - 4L);
			bool flag274 = fieldSize274 > 4194304;
			if (flag274)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Size of field ");
				defaultInterpolatedStringHandler.AppendFormatted("_maxMobilityPercent");
				defaultInterpolatedStringHandler.AppendLiteral(" must be less than ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(4096);
				defaultInterpolatedStringHandler.AppendLiteral("KB");
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			*(int*)pBegin274 = fieldSize274;
			byte* pBegin275 = pCurrData;
			pCurrData += 4;
			pCurrData += this._makeMindDamage.Serialize(pCurrData);
			int fieldSize275 = (int)((long)(pCurrData - pBegin275) - 4L);
			bool flag275 = fieldSize275 > 4194304;
			if (flag275)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Size of field ");
				defaultInterpolatedStringHandler.AppendFormatted("_makeMindDamage");
				defaultInterpolatedStringHandler.AppendLiteral(" must be less than ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(4096);
				defaultInterpolatedStringHandler.AppendLiteral("KB");
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			*(int*)pBegin275 = fieldSize275;
			byte* pBegin276 = pCurrData;
			pCurrData += 4;
			pCurrData += this._acceptMindDamage.Serialize(pCurrData);
			int fieldSize276 = (int)((long)(pCurrData - pBegin276) - 4L);
			bool flag276 = fieldSize276 > 4194304;
			if (flag276)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Size of field ");
				defaultInterpolatedStringHandler.AppendFormatted("_acceptMindDamage");
				defaultInterpolatedStringHandler.AppendLiteral(" must be less than ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(4096);
				defaultInterpolatedStringHandler.AppendLiteral("KB");
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			*(int*)pBegin276 = fieldSize276;
			byte* pBegin277 = pCurrData;
			pCurrData += 4;
			pCurrData += this._hitAddByTempValue.Serialize(pCurrData);
			int fieldSize277 = (int)((long)(pCurrData - pBegin277) - 4L);
			bool flag277 = fieldSize277 > 4194304;
			if (flag277)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Size of field ");
				defaultInterpolatedStringHandler.AppendFormatted("_hitAddByTempValue");
				defaultInterpolatedStringHandler.AppendLiteral(" must be less than ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(4096);
				defaultInterpolatedStringHandler.AppendLiteral("KB");
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			*(int*)pBegin277 = fieldSize277;
			byte* pBegin278 = pCurrData;
			pCurrData += 4;
			pCurrData += this._avoidAddByTempValue.Serialize(pCurrData);
			int fieldSize278 = (int)((long)(pCurrData - pBegin278) - 4L);
			bool flag278 = fieldSize278 > 4194304;
			if (flag278)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Size of field ");
				defaultInterpolatedStringHandler.AppendFormatted("_avoidAddByTempValue");
				defaultInterpolatedStringHandler.AppendLiteral(" must be less than ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(4096);
				defaultInterpolatedStringHandler.AppendLiteral("KB");
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			*(int*)pBegin278 = fieldSize278;
			byte* pBegin279 = pCurrData;
			pCurrData += 4;
			pCurrData += this._ignoreEquipmentOverload.Serialize(pCurrData);
			int fieldSize279 = (int)((long)(pCurrData - pBegin279) - 4L);
			bool flag279 = fieldSize279 > 4194304;
			if (flag279)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Size of field ");
				defaultInterpolatedStringHandler.AppendFormatted("_ignoreEquipmentOverload");
				defaultInterpolatedStringHandler.AppendLiteral(" must be less than ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(4096);
				defaultInterpolatedStringHandler.AppendLiteral("KB");
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			*(int*)pBegin279 = fieldSize279;
			byte* pBegin280 = pCurrData;
			pCurrData += 4;
			pCurrData += this._canCostEnemyUsableTricks.Serialize(pCurrData);
			int fieldSize280 = (int)((long)(pCurrData - pBegin280) - 4L);
			bool flag280 = fieldSize280 > 4194304;
			if (flag280)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Size of field ");
				defaultInterpolatedStringHandler.AppendFormatted("_canCostEnemyUsableTricks");
				defaultInterpolatedStringHandler.AppendLiteral(" must be less than ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(4096);
				defaultInterpolatedStringHandler.AppendLiteral("KB");
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			*(int*)pBegin280 = fieldSize280;
			byte* pBegin281 = pCurrData;
			pCurrData += 4;
			pCurrData += this._ignoreArmor.Serialize(pCurrData);
			int fieldSize281 = (int)((long)(pCurrData - pBegin281) - 4L);
			bool flag281 = fieldSize281 > 4194304;
			if (flag281)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Size of field ");
				defaultInterpolatedStringHandler.AppendFormatted("_ignoreArmor");
				defaultInterpolatedStringHandler.AppendLiteral(" must be less than ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(4096);
				defaultInterpolatedStringHandler.AppendLiteral("KB");
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			*(int*)pBegin281 = fieldSize281;
			byte* pBegin282 = pCurrData;
			pCurrData += 4;
			pCurrData += this._unyieldingFallen.Serialize(pCurrData);
			int fieldSize282 = (int)((long)(pCurrData - pBegin282) - 4L);
			bool flag282 = fieldSize282 > 4194304;
			if (flag282)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Size of field ");
				defaultInterpolatedStringHandler.AppendFormatted("_unyieldingFallen");
				defaultInterpolatedStringHandler.AppendLiteral(" must be less than ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(4096);
				defaultInterpolatedStringHandler.AppendLiteral("KB");
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			*(int*)pBegin282 = fieldSize282;
			byte* pBegin283 = pCurrData;
			pCurrData += 4;
			pCurrData += this._normalAttackPrepareFrame.Serialize(pCurrData);
			int fieldSize283 = (int)((long)(pCurrData - pBegin283) - 4L);
			bool flag283 = fieldSize283 > 4194304;
			if (flag283)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Size of field ");
				defaultInterpolatedStringHandler.AppendFormatted("_normalAttackPrepareFrame");
				defaultInterpolatedStringHandler.AppendLiteral(" must be less than ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(4096);
				defaultInterpolatedStringHandler.AppendLiteral("KB");
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			*(int*)pBegin283 = fieldSize283;
			byte* pBegin284 = pCurrData;
			pCurrData += 4;
			pCurrData += this._canCostUselessTricks.Serialize(pCurrData);
			int fieldSize284 = (int)((long)(pCurrData - pBegin284) - 4L);
			bool flag284 = fieldSize284 > 4194304;
			if (flag284)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Size of field ");
				defaultInterpolatedStringHandler.AppendFormatted("_canCostUselessTricks");
				defaultInterpolatedStringHandler.AppendLiteral(" must be less than ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(4096);
				defaultInterpolatedStringHandler.AppendLiteral("KB");
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			*(int*)pBegin284 = fieldSize284;
			byte* pBegin285 = pCurrData;
			pCurrData += 4;
			pCurrData += this._defendSkillCanAffect.Serialize(pCurrData);
			int fieldSize285 = (int)((long)(pCurrData - pBegin285) - 4L);
			bool flag285 = fieldSize285 > 4194304;
			if (flag285)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Size of field ");
				defaultInterpolatedStringHandler.AppendFormatted("_defendSkillCanAffect");
				defaultInterpolatedStringHandler.AppendLiteral(" must be less than ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(4096);
				defaultInterpolatedStringHandler.AppendLiteral("KB");
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			*(int*)pBegin285 = fieldSize285;
			byte* pBegin286 = pCurrData;
			pCurrData += 4;
			pCurrData += this._assistSkillCanAffect.Serialize(pCurrData);
			int fieldSize286 = (int)((long)(pCurrData - pBegin286) - 4L);
			bool flag286 = fieldSize286 > 4194304;
			if (flag286)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Size of field ");
				defaultInterpolatedStringHandler.AppendFormatted("_assistSkillCanAffect");
				defaultInterpolatedStringHandler.AppendLiteral(" must be less than ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(4096);
				defaultInterpolatedStringHandler.AppendLiteral("KB");
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			*(int*)pBegin286 = fieldSize286;
			byte* pBegin287 = pCurrData;
			pCurrData += 4;
			pCurrData += this._agileSkillCanAffect.Serialize(pCurrData);
			int fieldSize287 = (int)((long)(pCurrData - pBegin287) - 4L);
			bool flag287 = fieldSize287 > 4194304;
			if (flag287)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Size of field ");
				defaultInterpolatedStringHandler.AppendFormatted("_agileSkillCanAffect");
				defaultInterpolatedStringHandler.AppendLiteral(" must be less than ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(4096);
				defaultInterpolatedStringHandler.AppendLiteral("KB");
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			*(int*)pBegin287 = fieldSize287;
			byte* pBegin288 = pCurrData;
			pCurrData += 4;
			pCurrData += this._allMarkChangeToMind.Serialize(pCurrData);
			int fieldSize288 = (int)((long)(pCurrData - pBegin288) - 4L);
			bool flag288 = fieldSize288 > 4194304;
			if (flag288)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Size of field ");
				defaultInterpolatedStringHandler.AppendFormatted("_allMarkChangeToMind");
				defaultInterpolatedStringHandler.AppendLiteral(" must be less than ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(4096);
				defaultInterpolatedStringHandler.AppendLiteral("KB");
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			*(int*)pBegin288 = fieldSize288;
			byte* pBegin289 = pCurrData;
			pCurrData += 4;
			pCurrData += this._mindMarkChangeToFatal.Serialize(pCurrData);
			int fieldSize289 = (int)((long)(pCurrData - pBegin289) - 4L);
			bool flag289 = fieldSize289 > 4194304;
			if (flag289)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Size of field ");
				defaultInterpolatedStringHandler.AppendFormatted("_mindMarkChangeToFatal");
				defaultInterpolatedStringHandler.AppendLiteral(" must be less than ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(4096);
				defaultInterpolatedStringHandler.AppendLiteral("KB");
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			*(int*)pBegin289 = fieldSize289;
			byte* pBegin290 = pCurrData;
			pCurrData += 4;
			pCurrData += this._canCast.Serialize(pCurrData);
			int fieldSize290 = (int)((long)(pCurrData - pBegin290) - 4L);
			bool flag290 = fieldSize290 > 4194304;
			if (flag290)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Size of field ");
				defaultInterpolatedStringHandler.AppendFormatted("_canCast");
				defaultInterpolatedStringHandler.AppendLiteral(" must be less than ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(4096);
				defaultInterpolatedStringHandler.AppendLiteral("KB");
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			*(int*)pBegin290 = fieldSize290;
			byte* pBegin291 = pCurrData;
			pCurrData += 4;
			pCurrData += this._inevitableAvoid.Serialize(pCurrData);
			int fieldSize291 = (int)((long)(pCurrData - pBegin291) - 4L);
			bool flag291 = fieldSize291 > 4194304;
			if (flag291)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Size of field ");
				defaultInterpolatedStringHandler.AppendFormatted("_inevitableAvoid");
				defaultInterpolatedStringHandler.AppendLiteral(" must be less than ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(4096);
				defaultInterpolatedStringHandler.AppendLiteral("KB");
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			*(int*)pBegin291 = fieldSize291;
			byte* pBegin292 = pCurrData;
			pCurrData += 4;
			pCurrData += this._powerEffectReverse.Serialize(pCurrData);
			int fieldSize292 = (int)((long)(pCurrData - pBegin292) - 4L);
			bool flag292 = fieldSize292 > 4194304;
			if (flag292)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Size of field ");
				defaultInterpolatedStringHandler.AppendFormatted("_powerEffectReverse");
				defaultInterpolatedStringHandler.AppendLiteral(" must be less than ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(4096);
				defaultInterpolatedStringHandler.AppendLiteral("KB");
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			*(int*)pBegin292 = fieldSize292;
			byte* pBegin293 = pCurrData;
			pCurrData += 4;
			pCurrData += this._featureBonusReverse.Serialize(pCurrData);
			int fieldSize293 = (int)((long)(pCurrData - pBegin293) - 4L);
			bool flag293 = fieldSize293 > 4194304;
			if (flag293)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Size of field ");
				defaultInterpolatedStringHandler.AppendFormatted("_featureBonusReverse");
				defaultInterpolatedStringHandler.AppendLiteral(" must be less than ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(4096);
				defaultInterpolatedStringHandler.AppendLiteral("KB");
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			*(int*)pBegin293 = fieldSize293;
			byte* pBegin294 = pCurrData;
			pCurrData += 4;
			pCurrData += this._wugFatalDamageValue.Serialize(pCurrData);
			int fieldSize294 = (int)((long)(pCurrData - pBegin294) - 4L);
			bool flag294 = fieldSize294 > 4194304;
			if (flag294)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Size of field ");
				defaultInterpolatedStringHandler.AppendFormatted("_wugFatalDamageValue");
				defaultInterpolatedStringHandler.AppendLiteral(" must be less than ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(4096);
				defaultInterpolatedStringHandler.AppendLiteral("KB");
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			*(int*)pBegin294 = fieldSize294;
			byte* pBegin295 = pCurrData;
			pCurrData += 4;
			pCurrData += this._canRecoverHealthOnMonthChange.Serialize(pCurrData);
			int fieldSize295 = (int)((long)(pCurrData - pBegin295) - 4L);
			bool flag295 = fieldSize295 > 4194304;
			if (flag295)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Size of field ");
				defaultInterpolatedStringHandler.AppendFormatted("_canRecoverHealthOnMonthChange");
				defaultInterpolatedStringHandler.AppendLiteral(" must be less than ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(4096);
				defaultInterpolatedStringHandler.AppendLiteral("KB");
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			*(int*)pBegin295 = fieldSize295;
			byte* pBegin296 = pCurrData;
			pCurrData += 4;
			pCurrData += this._takeRevengeRateOnMonthChange.Serialize(pCurrData);
			int fieldSize296 = (int)((long)(pCurrData - pBegin296) - 4L);
			bool flag296 = fieldSize296 > 4194304;
			if (flag296)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Size of field ");
				defaultInterpolatedStringHandler.AppendFormatted("_takeRevengeRateOnMonthChange");
				defaultInterpolatedStringHandler.AppendLiteral(" must be less than ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(4096);
				defaultInterpolatedStringHandler.AppendLiteral("KB");
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			*(int*)pBegin296 = fieldSize296;
			byte* pBegin297 = pCurrData;
			pCurrData += 4;
			pCurrData += this._consummateLevelBonus.Serialize(pCurrData);
			int fieldSize297 = (int)((long)(pCurrData - pBegin297) - 4L);
			bool flag297 = fieldSize297 > 4194304;
			if (flag297)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Size of field ");
				defaultInterpolatedStringHandler.AppendFormatted("_consummateLevelBonus");
				defaultInterpolatedStringHandler.AppendLiteral(" must be less than ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(4096);
				defaultInterpolatedStringHandler.AppendLiteral("KB");
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			*(int*)pBegin297 = fieldSize297;
			byte* pBegin298 = pCurrData;
			pCurrData += 4;
			pCurrData += this._neiliDelta.Serialize(pCurrData);
			int fieldSize298 = (int)((long)(pCurrData - pBegin298) - 4L);
			bool flag298 = fieldSize298 > 4194304;
			if (flag298)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Size of field ");
				defaultInterpolatedStringHandler.AppendFormatted("_neiliDelta");
				defaultInterpolatedStringHandler.AppendLiteral(" must be less than ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(4096);
				defaultInterpolatedStringHandler.AppendLiteral("KB");
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			*(int*)pBegin298 = fieldSize298;
			byte* pBegin299 = pCurrData;
			pCurrData += 4;
			pCurrData += this._canMakeLoveSpecialOnMonthChange.Serialize(pCurrData);
			int fieldSize299 = (int)((long)(pCurrData - pBegin299) - 4L);
			bool flag299 = fieldSize299 > 4194304;
			if (flag299)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Size of field ");
				defaultInterpolatedStringHandler.AppendFormatted("_canMakeLoveSpecialOnMonthChange");
				defaultInterpolatedStringHandler.AppendLiteral(" must be less than ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(4096);
				defaultInterpolatedStringHandler.AppendLiteral("KB");
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			*(int*)pBegin299 = fieldSize299;
			byte* pBegin300 = pCurrData;
			pCurrData += 4;
			pCurrData += this._healAcupointSpeed.Serialize(pCurrData);
			int fieldSize300 = (int)((long)(pCurrData - pBegin300) - 4L);
			bool flag300 = fieldSize300 > 4194304;
			if (flag300)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Size of field ");
				defaultInterpolatedStringHandler.AppendFormatted("_healAcupointSpeed");
				defaultInterpolatedStringHandler.AppendLiteral(" must be less than ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(4096);
				defaultInterpolatedStringHandler.AppendLiteral("KB");
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			*(int*)pBegin300 = fieldSize300;
			byte* pBegin301 = pCurrData;
			pCurrData += 4;
			pCurrData += this._maxChangeTrickCount.Serialize(pCurrData);
			int fieldSize301 = (int)((long)(pCurrData - pBegin301) - 4L);
			bool flag301 = fieldSize301 > 4194304;
			if (flag301)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Size of field ");
				defaultInterpolatedStringHandler.AppendFormatted("_maxChangeTrickCount");
				defaultInterpolatedStringHandler.AppendLiteral(" must be less than ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(4096);
				defaultInterpolatedStringHandler.AppendLiteral("KB");
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			*(int*)pBegin301 = fieldSize301;
			byte* pBegin302 = pCurrData;
			pCurrData += 4;
			pCurrData += this._convertCostBreathAndStance.Serialize(pCurrData);
			int fieldSize302 = (int)((long)(pCurrData - pBegin302) - 4L);
			bool flag302 = fieldSize302 > 4194304;
			if (flag302)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Size of field ");
				defaultInterpolatedStringHandler.AppendFormatted("_convertCostBreathAndStance");
				defaultInterpolatedStringHandler.AppendLiteral(" must be less than ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(4096);
				defaultInterpolatedStringHandler.AppendLiteral("KB");
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			*(int*)pBegin302 = fieldSize302;
			byte* pBegin303 = pCurrData;
			pCurrData += 4;
			pCurrData += this._personalitiesAll.Serialize(pCurrData);
			int fieldSize303 = (int)((long)(pCurrData - pBegin303) - 4L);
			bool flag303 = fieldSize303 > 4194304;
			if (flag303)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Size of field ");
				defaultInterpolatedStringHandler.AppendFormatted("_personalitiesAll");
				defaultInterpolatedStringHandler.AppendLiteral(" must be less than ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(4096);
				defaultInterpolatedStringHandler.AppendLiteral("KB");
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			*(int*)pBegin303 = fieldSize303;
			byte* pBegin304 = pCurrData;
			pCurrData += 4;
			pCurrData += this._finalFatalDamageMarkCount.Serialize(pCurrData);
			int fieldSize304 = (int)((long)(pCurrData - pBegin304) - 4L);
			bool flag304 = fieldSize304 > 4194304;
			if (flag304)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Size of field ");
				defaultInterpolatedStringHandler.AppendFormatted("_finalFatalDamageMarkCount");
				defaultInterpolatedStringHandler.AppendLiteral(" must be less than ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(4096);
				defaultInterpolatedStringHandler.AppendLiteral("KB");
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			*(int*)pBegin304 = fieldSize304;
			byte* pBegin305 = pCurrData;
			pCurrData += 4;
			pCurrData += this._infinityMindMarkProgress.Serialize(pCurrData);
			int fieldSize305 = (int)((long)(pCurrData - pBegin305) - 4L);
			bool flag305 = fieldSize305 > 4194304;
			if (flag305)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Size of field ");
				defaultInterpolatedStringHandler.AppendFormatted("_infinityMindMarkProgress");
				defaultInterpolatedStringHandler.AppendLiteral(" must be less than ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(4096);
				defaultInterpolatedStringHandler.AppendLiteral("KB");
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			*(int*)pBegin305 = fieldSize305;
			byte* pBegin306 = pCurrData;
			pCurrData += 4;
			pCurrData += this._combatSkillAiScorePower.Serialize(pCurrData);
			int fieldSize306 = (int)((long)(pCurrData - pBegin306) - 4L);
			bool flag306 = fieldSize306 > 4194304;
			if (flag306)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Size of field ");
				defaultInterpolatedStringHandler.AppendFormatted("_combatSkillAiScorePower");
				defaultInterpolatedStringHandler.AppendLiteral(" must be less than ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(4096);
				defaultInterpolatedStringHandler.AppendLiteral("KB");
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			*(int*)pBegin306 = fieldSize306;
			byte* pBegin307 = pCurrData;
			pCurrData += 4;
			pCurrData += this._normalAttackChangeToUnlockAttack.Serialize(pCurrData);
			int fieldSize307 = (int)((long)(pCurrData - pBegin307) - 4L);
			bool flag307 = fieldSize307 > 4194304;
			if (flag307)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Size of field ");
				defaultInterpolatedStringHandler.AppendFormatted("_normalAttackChangeToUnlockAttack");
				defaultInterpolatedStringHandler.AppendLiteral(" must be less than ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(4096);
				defaultInterpolatedStringHandler.AppendLiteral("KB");
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			*(int*)pBegin307 = fieldSize307;
			byte* pBegin308 = pCurrData;
			pCurrData += 4;
			pCurrData += this._attackBodyPartOdds.Serialize(pCurrData);
			int fieldSize308 = (int)((long)(pCurrData - pBegin308) - 4L);
			bool flag308 = fieldSize308 > 4194304;
			if (flag308)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Size of field ");
				defaultInterpolatedStringHandler.AppendFormatted("_attackBodyPartOdds");
				defaultInterpolatedStringHandler.AppendLiteral(" must be less than ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(4096);
				defaultInterpolatedStringHandler.AppendLiteral("KB");
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			*(int*)pBegin308 = fieldSize308;
			byte* pBegin309 = pCurrData;
			pCurrData += 4;
			pCurrData += this._changeDurability.Serialize(pCurrData);
			int fieldSize309 = (int)((long)(pCurrData - pBegin309) - 4L);
			bool flag309 = fieldSize309 > 4194304;
			if (flag309)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Size of field ");
				defaultInterpolatedStringHandler.AppendFormatted("_changeDurability");
				defaultInterpolatedStringHandler.AppendLiteral(" must be less than ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(4096);
				defaultInterpolatedStringHandler.AppendLiteral("KB");
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			*(int*)pBegin309 = fieldSize309;
			byte* pBegin310 = pCurrData;
			pCurrData += 4;
			pCurrData += this._equipmentBonus.Serialize(pCurrData);
			int fieldSize310 = (int)((long)(pCurrData - pBegin310) - 4L);
			bool flag310 = fieldSize310 > 4194304;
			if (flag310)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Size of field ");
				defaultInterpolatedStringHandler.AppendFormatted("_equipmentBonus");
				defaultInterpolatedStringHandler.AppendLiteral(" must be less than ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(4096);
				defaultInterpolatedStringHandler.AppendLiteral("KB");
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			*(int*)pBegin310 = fieldSize310;
			byte* pBegin311 = pCurrData;
			pCurrData += 4;
			pCurrData += this._equipmentWeight.Serialize(pCurrData);
			int fieldSize311 = (int)((long)(pCurrData - pBegin311) - 4L);
			bool flag311 = fieldSize311 > 4194304;
			if (flag311)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Size of field ");
				defaultInterpolatedStringHandler.AppendFormatted("_equipmentWeight");
				defaultInterpolatedStringHandler.AppendLiteral(" must be less than ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(4096);
				defaultInterpolatedStringHandler.AppendLiteral("KB");
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			*(int*)pBegin311 = fieldSize311;
			byte* pBegin312 = pCurrData;
			pCurrData += 4;
			pCurrData += this._rawCreateEffectList.Serialize(pCurrData);
			int fieldSize312 = (int)((long)(pCurrData - pBegin312) - 4L);
			bool flag312 = fieldSize312 > 4194304;
			if (flag312)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Size of field ");
				defaultInterpolatedStringHandler.AppendFormatted("_rawCreateEffectList");
				defaultInterpolatedStringHandler.AppendLiteral(" must be less than ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(4096);
				defaultInterpolatedStringHandler.AppendLiteral("KB");
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			*(int*)pBegin312 = fieldSize312;
			byte* pBegin313 = pCurrData;
			pCurrData += 4;
			pCurrData += this._jiTrickAsWeaponTrickCount.Serialize(pCurrData);
			int fieldSize313 = (int)((long)(pCurrData - pBegin313) - 4L);
			bool flag313 = fieldSize313 > 4194304;
			if (flag313)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Size of field ");
				defaultInterpolatedStringHandler.AppendFormatted("_jiTrickAsWeaponTrickCount");
				defaultInterpolatedStringHandler.AppendLiteral(" must be less than ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(4096);
				defaultInterpolatedStringHandler.AppendLiteral("KB");
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			*(int*)pBegin313 = fieldSize313;
			byte* pBegin314 = pCurrData;
			pCurrData += 4;
			pCurrData += this._uselessTrickAsJiTrickCount.Serialize(pCurrData);
			int fieldSize314 = (int)((long)(pCurrData - pBegin314) - 4L);
			bool flag314 = fieldSize314 > 4194304;
			if (flag314)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Size of field ");
				defaultInterpolatedStringHandler.AppendFormatted("_uselessTrickAsJiTrickCount");
				defaultInterpolatedStringHandler.AppendLiteral(" must be less than ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(4096);
				defaultInterpolatedStringHandler.AppendLiteral("KB");
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			*(int*)pBegin314 = fieldSize314;
			byte* pBegin315 = pCurrData;
			pCurrData += 4;
			pCurrData += this._equipmentPower.Serialize(pCurrData);
			int fieldSize315 = (int)((long)(pCurrData - pBegin315) - 4L);
			bool flag315 = fieldSize315 > 4194304;
			if (flag315)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Size of field ");
				defaultInterpolatedStringHandler.AppendFormatted("_equipmentPower");
				defaultInterpolatedStringHandler.AppendLiteral(" must be less than ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(4096);
				defaultInterpolatedStringHandler.AppendLiteral("KB");
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			*(int*)pBegin315 = fieldSize315;
			byte* pBegin316 = pCurrData;
			pCurrData += 4;
			pCurrData += this._healFlawSpeed.Serialize(pCurrData);
			int fieldSize316 = (int)((long)(pCurrData - pBegin316) - 4L);
			bool flag316 = fieldSize316 > 4194304;
			if (flag316)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Size of field ");
				defaultInterpolatedStringHandler.AppendFormatted("_healFlawSpeed");
				defaultInterpolatedStringHandler.AppendLiteral(" must be less than ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(4096);
				defaultInterpolatedStringHandler.AppendLiteral("KB");
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			*(int*)pBegin316 = fieldSize316;
			byte* pBegin317 = pCurrData;
			pCurrData += 4;
			pCurrData += this._unlockSpeed.Serialize(pCurrData);
			int fieldSize317 = (int)((long)(pCurrData - pBegin317) - 4L);
			bool flag317 = fieldSize317 > 4194304;
			if (flag317)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Size of field ");
				defaultInterpolatedStringHandler.AppendFormatted("_unlockSpeed");
				defaultInterpolatedStringHandler.AppendLiteral(" must be less than ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(4096);
				defaultInterpolatedStringHandler.AppendLiteral("KB");
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			*(int*)pBegin317 = fieldSize317;
			byte* pBegin318 = pCurrData;
			pCurrData += 4;
			pCurrData += this._flawBonusFactor.Serialize(pCurrData);
			int fieldSize318 = (int)((long)(pCurrData - pBegin318) - 4L);
			bool flag318 = fieldSize318 > 4194304;
			if (flag318)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Size of field ");
				defaultInterpolatedStringHandler.AppendFormatted("_flawBonusFactor");
				defaultInterpolatedStringHandler.AppendLiteral(" must be less than ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(4096);
				defaultInterpolatedStringHandler.AppendLiteral("KB");
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			*(int*)pBegin318 = fieldSize318;
			byte* pBegin319 = pCurrData;
			pCurrData += 4;
			pCurrData += this._canCostShaTricks.Serialize(pCurrData);
			int fieldSize319 = (int)((long)(pCurrData - pBegin319) - 4L);
			bool flag319 = fieldSize319 > 4194304;
			if (flag319)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Size of field ");
				defaultInterpolatedStringHandler.AppendFormatted("_canCostShaTricks");
				defaultInterpolatedStringHandler.AppendLiteral(" must be less than ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(4096);
				defaultInterpolatedStringHandler.AppendLiteral("KB");
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			*(int*)pBegin319 = fieldSize319;
			byte* pBegin320 = pCurrData;
			pCurrData += 4;
			pCurrData += this._defenderDirectFinalDamageValue.Serialize(pCurrData);
			int fieldSize320 = (int)((long)(pCurrData - pBegin320) - 4L);
			bool flag320 = fieldSize320 > 4194304;
			if (flag320)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Size of field ");
				defaultInterpolatedStringHandler.AppendFormatted("_defenderDirectFinalDamageValue");
				defaultInterpolatedStringHandler.AppendLiteral(" must be less than ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(4096);
				defaultInterpolatedStringHandler.AppendLiteral("KB");
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			*(int*)pBegin320 = fieldSize320;
			byte* pBegin321 = pCurrData;
			pCurrData += 4;
			pCurrData += this._normalAttackRecoveryFrame.Serialize(pCurrData);
			int fieldSize321 = (int)((long)(pCurrData - pBegin321) - 4L);
			bool flag321 = fieldSize321 > 4194304;
			if (flag321)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Size of field ");
				defaultInterpolatedStringHandler.AppendFormatted("_normalAttackRecoveryFrame");
				defaultInterpolatedStringHandler.AppendLiteral(" must be less than ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(4096);
				defaultInterpolatedStringHandler.AppendLiteral("KB");
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			*(int*)pBegin321 = fieldSize321;
			byte* pBegin322 = pCurrData;
			pCurrData += 4;
			pCurrData += this._finalGoneMadInjury.Serialize(pCurrData);
			int fieldSize322 = (int)((long)(pCurrData - pBegin322) - 4L);
			bool flag322 = fieldSize322 > 4194304;
			if (flag322)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Size of field ");
				defaultInterpolatedStringHandler.AppendFormatted("_finalGoneMadInjury");
				defaultInterpolatedStringHandler.AppendLiteral(" must be less than ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(4096);
				defaultInterpolatedStringHandler.AppendLiteral("KB");
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			*(int*)pBegin322 = fieldSize322;
			byte* pBegin323 = pCurrData;
			pCurrData += 4;
			pCurrData += this._attackerDirectFinalDamageValue.Serialize(pCurrData);
			int fieldSize323 = (int)((long)(pCurrData - pBegin323) - 4L);
			bool flag323 = fieldSize323 > 4194304;
			if (flag323)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Size of field ");
				defaultInterpolatedStringHandler.AppendFormatted("_attackerDirectFinalDamageValue");
				defaultInterpolatedStringHandler.AppendLiteral(" must be less than ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(4096);
				defaultInterpolatedStringHandler.AppendLiteral("KB");
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			*(int*)pBegin323 = fieldSize323;
			byte* pBegin324 = pCurrData;
			pCurrData += 4;
			pCurrData += this._canCostTrickDuringPreparingSkill.Serialize(pCurrData);
			int fieldSize324 = (int)((long)(pCurrData - pBegin324) - 4L);
			bool flag324 = fieldSize324 > 4194304;
			if (flag324)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Size of field ");
				defaultInterpolatedStringHandler.AppendFormatted("_canCostTrickDuringPreparingSkill");
				defaultInterpolatedStringHandler.AppendLiteral(" must be less than ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(4096);
				defaultInterpolatedStringHandler.AppendLiteral("KB");
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			*(int*)pBegin324 = fieldSize324;
			byte* pBegin325 = pCurrData;
			pCurrData += 4;
			pCurrData += this._validItemList.Serialize(pCurrData);
			int fieldSize325 = (int)((long)(pCurrData - pBegin325) - 4L);
			bool flag325 = fieldSize325 > 4194304;
			if (flag325)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Size of field ");
				defaultInterpolatedStringHandler.AppendFormatted("_validItemList");
				defaultInterpolatedStringHandler.AppendLiteral(" must be less than ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(4096);
				defaultInterpolatedStringHandler.AppendLiteral("KB");
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			*(int*)pBegin325 = fieldSize325;
			byte* pBegin326 = pCurrData;
			pCurrData += 4;
			pCurrData += this._acceptDamageCanAdd.Serialize(pCurrData);
			int fieldSize326 = (int)((long)(pCurrData - pBegin326) - 4L);
			bool flag326 = fieldSize326 > 4194304;
			if (flag326)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Size of field ");
				defaultInterpolatedStringHandler.AppendFormatted("_acceptDamageCanAdd");
				defaultInterpolatedStringHandler.AppendLiteral(" must be less than ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(4096);
				defaultInterpolatedStringHandler.AppendLiteral("KB");
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			*(int*)pBegin326 = fieldSize326;
			byte* pBegin327 = pCurrData;
			pCurrData += 4;
			pCurrData += this._makeDamageCanReduce.Serialize(pCurrData);
			int fieldSize327 = (int)((long)(pCurrData - pBegin327) - 4L);
			bool flag327 = fieldSize327 > 4194304;
			if (flag327)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Size of field ");
				defaultInterpolatedStringHandler.AppendFormatted("_makeDamageCanReduce");
				defaultInterpolatedStringHandler.AppendLiteral(" must be less than ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(4096);
				defaultInterpolatedStringHandler.AppendLiteral("KB");
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			*(int*)pBegin327 = fieldSize327;
			byte* pBegin328 = pCurrData;
			pCurrData += 4;
			pCurrData += this._normalAttackGetTrickCount.Serialize(pCurrData);
			int fieldSize328 = (int)((long)(pCurrData - pBegin328) - 4L);
			bool flag328 = fieldSize328 > 4194304;
			if (flag328)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Size of field ");
				defaultInterpolatedStringHandler.AppendFormatted("_normalAttackGetTrickCount");
				defaultInterpolatedStringHandler.AppendLiteral(" must be less than ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(4096);
				defaultInterpolatedStringHandler.AppendLiteral("KB");
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			*(int*)pBegin328 = fieldSize328;
			return (int)((long)(pCurrData - pData));
		}

		// Token: 0x06002869 RID: 10345 RVA: 0x001EE144 File Offset: 0x001EC344
		public unsafe int Deserialize(byte* pData)
		{
			this._id = *(int*)pData;
			byte* pCurrData = pData + 4;
			pCurrData += 4;
			pCurrData += this._maxStrength.Deserialize(pCurrData);
			pCurrData += 4;
			pCurrData += this._maxDexterity.Deserialize(pCurrData);
			pCurrData += 4;
			pCurrData += this._maxConcentration.Deserialize(pCurrData);
			pCurrData += 4;
			pCurrData += this._maxVitality.Deserialize(pCurrData);
			pCurrData += 4;
			pCurrData += this._maxEnergy.Deserialize(pCurrData);
			pCurrData += 4;
			pCurrData += this._maxIntelligence.Deserialize(pCurrData);
			pCurrData += 4;
			pCurrData += this._recoveryOfStance.Deserialize(pCurrData);
			pCurrData += 4;
			pCurrData += this._recoveryOfBreath.Deserialize(pCurrData);
			pCurrData += 4;
			pCurrData += this._moveSpeed.Deserialize(pCurrData);
			pCurrData += 4;
			pCurrData += this._recoveryOfFlaw.Deserialize(pCurrData);
			pCurrData += 4;
			pCurrData += this._castSpeed.Deserialize(pCurrData);
			pCurrData += 4;
			pCurrData += this._recoveryOfBlockedAcupoint.Deserialize(pCurrData);
			pCurrData += 4;
			pCurrData += this._weaponSwitchSpeed.Deserialize(pCurrData);
			pCurrData += 4;
			pCurrData += this._attackSpeed.Deserialize(pCurrData);
			pCurrData += 4;
			pCurrData += this._innerRatio.Deserialize(pCurrData);
			pCurrData += 4;
			pCurrData += this._recoveryOfQiDisorder.Deserialize(pCurrData);
			pCurrData += 4;
			pCurrData += this._minorAttributeFixMaxValue.Deserialize(pCurrData);
			pCurrData += 4;
			pCurrData += this._minorAttributeFixMinValue.Deserialize(pCurrData);
			pCurrData += 4;
			pCurrData += this._resistOfHotPoison.Deserialize(pCurrData);
			pCurrData += 4;
			pCurrData += this._resistOfGloomyPoison.Deserialize(pCurrData);
			pCurrData += 4;
			pCurrData += this._resistOfColdPoison.Deserialize(pCurrData);
			pCurrData += 4;
			pCurrData += this._resistOfRedPoison.Deserialize(pCurrData);
			pCurrData += 4;
			pCurrData += this._resistOfRottenPoison.Deserialize(pCurrData);
			pCurrData += 4;
			pCurrData += this._resistOfIllusoryPoison.Deserialize(pCurrData);
			pCurrData += 4;
			pCurrData += this._displayAge.Deserialize(pCurrData);
			pCurrData += 4;
			pCurrData += this._neiliProportionOfFiveElements.Deserialize(pCurrData);
			pCurrData += 4;
			pCurrData += this._weaponMaxPower.Deserialize(pCurrData);
			pCurrData += 4;
			pCurrData += this._weaponUseRequirement.Deserialize(pCurrData);
			pCurrData += 4;
			pCurrData += this._weaponAttackRange.Deserialize(pCurrData);
			pCurrData += 4;
			pCurrData += this._armorMaxPower.Deserialize(pCurrData);
			pCurrData += 4;
			pCurrData += this._armorUseRequirement.Deserialize(pCurrData);
			pCurrData += 4;
			pCurrData += this._hitStrength.Deserialize(pCurrData);
			pCurrData += 4;
			pCurrData += this._hitTechnique.Deserialize(pCurrData);
			pCurrData += 4;
			pCurrData += this._hitSpeed.Deserialize(pCurrData);
			pCurrData += 4;
			pCurrData += this._hitMind.Deserialize(pCurrData);
			pCurrData += 4;
			pCurrData += this._hitCanChange.Deserialize(pCurrData);
			pCurrData += 4;
			pCurrData += this._hitChangeEffectPercent.Deserialize(pCurrData);
			pCurrData += 4;
			pCurrData += this._avoidStrength.Deserialize(pCurrData);
			pCurrData += 4;
			pCurrData += this._avoidTechnique.Deserialize(pCurrData);
			pCurrData += 4;
			pCurrData += this._avoidSpeed.Deserialize(pCurrData);
			pCurrData += 4;
			pCurrData += this._avoidMind.Deserialize(pCurrData);
			pCurrData += 4;
			pCurrData += this._avoidCanChange.Deserialize(pCurrData);
			pCurrData += 4;
			pCurrData += this._avoidChangeEffectPercent.Deserialize(pCurrData);
			pCurrData += 4;
			pCurrData += this._penetrateOuter.Deserialize(pCurrData);
			pCurrData += 4;
			pCurrData += this._penetrateInner.Deserialize(pCurrData);
			pCurrData += 4;
			pCurrData += this._penetrateResistOuter.Deserialize(pCurrData);
			pCurrData += 4;
			pCurrData += this._penetrateResistInner.Deserialize(pCurrData);
			pCurrData += 4;
			pCurrData += this._neiliAllocationAttack.Deserialize(pCurrData);
			pCurrData += 4;
			pCurrData += this._neiliAllocationAgile.Deserialize(pCurrData);
			pCurrData += 4;
			pCurrData += this._neiliAllocationDefense.Deserialize(pCurrData);
			pCurrData += 4;
			pCurrData += this._neiliAllocationAssist.Deserialize(pCurrData);
			pCurrData += 4;
			pCurrData += this._happiness.Deserialize(pCurrData);
			pCurrData += 4;
			pCurrData += this._maxHealth.Deserialize(pCurrData);
			pCurrData += 4;
			pCurrData += this._healthCost.Deserialize(pCurrData);
			pCurrData += 4;
			pCurrData += this._moveSpeedCanChange.Deserialize(pCurrData);
			pCurrData += 4;
			pCurrData += this._attackerHitStrength.Deserialize(pCurrData);
			pCurrData += 4;
			pCurrData += this._attackerHitTechnique.Deserialize(pCurrData);
			pCurrData += 4;
			pCurrData += this._attackerHitSpeed.Deserialize(pCurrData);
			pCurrData += 4;
			pCurrData += this._attackerHitMind.Deserialize(pCurrData);
			pCurrData += 4;
			pCurrData += this._attackerAvoidStrength.Deserialize(pCurrData);
			pCurrData += 4;
			pCurrData += this._attackerAvoidTechnique.Deserialize(pCurrData);
			pCurrData += 4;
			pCurrData += this._attackerAvoidSpeed.Deserialize(pCurrData);
			pCurrData += 4;
			pCurrData += this._attackerAvoidMind.Deserialize(pCurrData);
			pCurrData += 4;
			pCurrData += this._attackerPenetrateOuter.Deserialize(pCurrData);
			pCurrData += 4;
			pCurrData += this._attackerPenetrateInner.Deserialize(pCurrData);
			pCurrData += 4;
			pCurrData += this._attackerPenetrateResistOuter.Deserialize(pCurrData);
			pCurrData += 4;
			pCurrData += this._attackerPenetrateResistInner.Deserialize(pCurrData);
			pCurrData += 4;
			pCurrData += this._attackHitType.Deserialize(pCurrData);
			pCurrData += 4;
			pCurrData += this._makeDirectDamage.Deserialize(pCurrData);
			pCurrData += 4;
			pCurrData += this._makeBounceDamage.Deserialize(pCurrData);
			pCurrData += 4;
			pCurrData += this._makeFightBackDamage.Deserialize(pCurrData);
			pCurrData += 4;
			pCurrData += this._makePoisonLevel.Deserialize(pCurrData);
			pCurrData += 4;
			pCurrData += this._makePoisonValue.Deserialize(pCurrData);
			pCurrData += 4;
			pCurrData += this._attackerHitOdds.Deserialize(pCurrData);
			pCurrData += 4;
			pCurrData += this._attackerFightBackHitOdds.Deserialize(pCurrData);
			pCurrData += 4;
			pCurrData += this._attackerPursueOdds.Deserialize(pCurrData);
			pCurrData += 4;
			pCurrData += this._makedInjuryChangeToOld.Deserialize(pCurrData);
			pCurrData += 4;
			pCurrData += this._makedPoisonChangeToOld.Deserialize(pCurrData);
			pCurrData += 4;
			pCurrData += this._makeDamageType.Deserialize(pCurrData);
			pCurrData += 4;
			pCurrData += this._canMakeInjuryToNoInjuryPart.Deserialize(pCurrData);
			pCurrData += 4;
			pCurrData += this._makePoisonType.Deserialize(pCurrData);
			pCurrData += 4;
			pCurrData += this._normalAttackWeapon.Deserialize(pCurrData);
			pCurrData += 4;
			pCurrData += this._normalAttackTrick.Deserialize(pCurrData);
			pCurrData += 4;
			pCurrData += this._extraFlawCount.Deserialize(pCurrData);
			pCurrData += 4;
			pCurrData += this._attackCanBounce.Deserialize(pCurrData);
			pCurrData += 4;
			pCurrData += this._attackCanFightBack.Deserialize(pCurrData);
			pCurrData += 4;
			pCurrData += this._makeFightBackInjuryMark.Deserialize(pCurrData);
			pCurrData += 4;
			pCurrData += this._legSkillUseShoes.Deserialize(pCurrData);
			pCurrData += 4;
			pCurrData += this._attackerFinalDamageValue.Deserialize(pCurrData);
			pCurrData += 4;
			pCurrData += this._defenderHitStrength.Deserialize(pCurrData);
			pCurrData += 4;
			pCurrData += this._defenderHitTechnique.Deserialize(pCurrData);
			pCurrData += 4;
			pCurrData += this._defenderHitSpeed.Deserialize(pCurrData);
			pCurrData += 4;
			pCurrData += this._defenderHitMind.Deserialize(pCurrData);
			pCurrData += 4;
			pCurrData += this._defenderAvoidStrength.Deserialize(pCurrData);
			pCurrData += 4;
			pCurrData += this._defenderAvoidTechnique.Deserialize(pCurrData);
			pCurrData += 4;
			pCurrData += this._defenderAvoidSpeed.Deserialize(pCurrData);
			pCurrData += 4;
			pCurrData += this._defenderAvoidMind.Deserialize(pCurrData);
			pCurrData += 4;
			pCurrData += this._defenderPenetrateOuter.Deserialize(pCurrData);
			pCurrData += 4;
			pCurrData += this._defenderPenetrateInner.Deserialize(pCurrData);
			pCurrData += 4;
			pCurrData += this._defenderPenetrateResistOuter.Deserialize(pCurrData);
			pCurrData += 4;
			pCurrData += this._defenderPenetrateResistInner.Deserialize(pCurrData);
			pCurrData += 4;
			pCurrData += this._acceptDirectDamage.Deserialize(pCurrData);
			pCurrData += 4;
			pCurrData += this._acceptBounceDamage.Deserialize(pCurrData);
			pCurrData += 4;
			pCurrData += this._acceptFightBackDamage.Deserialize(pCurrData);
			pCurrData += 4;
			pCurrData += this._acceptPoisonLevel.Deserialize(pCurrData);
			pCurrData += 4;
			pCurrData += this._acceptPoisonValue.Deserialize(pCurrData);
			pCurrData += 4;
			pCurrData += this._defenderHitOdds.Deserialize(pCurrData);
			pCurrData += 4;
			pCurrData += this._defenderFightBackHitOdds.Deserialize(pCurrData);
			pCurrData += 4;
			pCurrData += this._defenderPursueOdds.Deserialize(pCurrData);
			pCurrData += 4;
			pCurrData += this._acceptMaxInjuryCount.Deserialize(pCurrData);
			pCurrData += 4;
			pCurrData += this._bouncePower.Deserialize(pCurrData);
			pCurrData += 4;
			pCurrData += this._fightBackPower.Deserialize(pCurrData);
			pCurrData += 4;
			pCurrData += this._directDamageInnerRatio.Deserialize(pCurrData);
			pCurrData += 4;
			pCurrData += this._defenderFinalDamageValue.Deserialize(pCurrData);
			pCurrData += 4;
			pCurrData += this._directDamageValue.Deserialize(pCurrData);
			pCurrData += 4;
			pCurrData += this._directInjuryMark.Deserialize(pCurrData);
			pCurrData += 4;
			pCurrData += this._goneMadInjury.Deserialize(pCurrData);
			pCurrData += 4;
			pCurrData += this._healInjurySpeed.Deserialize(pCurrData);
			pCurrData += 4;
			pCurrData += this._healInjuryBuff.Deserialize(pCurrData);
			pCurrData += 4;
			pCurrData += this._healInjuryDebuff.Deserialize(pCurrData);
			pCurrData += 4;
			pCurrData += this._healPoisonSpeed.Deserialize(pCurrData);
			pCurrData += 4;
			pCurrData += this._healPoisonBuff.Deserialize(pCurrData);
			pCurrData += 4;
			pCurrData += this._healPoisonDebuff.Deserialize(pCurrData);
			pCurrData += 4;
			pCurrData += this._fleeSpeed.Deserialize(pCurrData);
			pCurrData += 4;
			pCurrData += this._maxFlawCount.Deserialize(pCurrData);
			pCurrData += 4;
			pCurrData += this._canAddFlaw.Deserialize(pCurrData);
			pCurrData += 4;
			pCurrData += this._flawLevel.Deserialize(pCurrData);
			pCurrData += 4;
			pCurrData += this._flawLevelCanReduce.Deserialize(pCurrData);
			pCurrData += 4;
			pCurrData += this._flawCount.Deserialize(pCurrData);
			pCurrData += 4;
			pCurrData += this._maxAcupointCount.Deserialize(pCurrData);
			pCurrData += 4;
			pCurrData += this._canAddAcupoint.Deserialize(pCurrData);
			pCurrData += 4;
			pCurrData += this._acupointLevel.Deserialize(pCurrData);
			pCurrData += 4;
			pCurrData += this._acupointLevelCanReduce.Deserialize(pCurrData);
			pCurrData += 4;
			pCurrData += this._acupointCount.Deserialize(pCurrData);
			pCurrData += 4;
			pCurrData += this._addNeiliAllocation.Deserialize(pCurrData);
			pCurrData += 4;
			pCurrData += this._costNeiliAllocation.Deserialize(pCurrData);
			pCurrData += 4;
			pCurrData += this._canChangeNeiliAllocation.Deserialize(pCurrData);
			pCurrData += 4;
			pCurrData += this._canGetTrick.Deserialize(pCurrData);
			pCurrData += 4;
			pCurrData += this._getTrickType.Deserialize(pCurrData);
			pCurrData += 4;
			pCurrData += this._attackBodyPart.Deserialize(pCurrData);
			pCurrData += 4;
			pCurrData += this._weaponEquipAttack.Deserialize(pCurrData);
			pCurrData += 4;
			pCurrData += this._weaponEquipDefense.Deserialize(pCurrData);
			pCurrData += 4;
			pCurrData += this._armorEquipAttack.Deserialize(pCurrData);
			pCurrData += 4;
			pCurrData += this._armorEquipDefense.Deserialize(pCurrData);
			pCurrData += 4;
			pCurrData += this._attackRangeForward.Deserialize(pCurrData);
			pCurrData += 4;
			pCurrData += this._attackRangeBackward.Deserialize(pCurrData);
			pCurrData += 4;
			pCurrData += this._moveCanBeStopped.Deserialize(pCurrData);
			pCurrData += 4;
			pCurrData += this._canForcedMove.Deserialize(pCurrData);
			pCurrData += 4;
			pCurrData += this._mobilityCanBeRemoved.Deserialize(pCurrData);
			pCurrData += 4;
			pCurrData += this._mobilityCostByEffect.Deserialize(pCurrData);
			pCurrData += 4;
			pCurrData += this._moveDistance.Deserialize(pCurrData);
			pCurrData += 4;
			pCurrData += this._jumpPrepareFrame.Deserialize(pCurrData);
			pCurrData += 4;
			pCurrData += this._bounceInjuryMark.Deserialize(pCurrData);
			pCurrData += 4;
			pCurrData += this._skillHasCost.Deserialize(pCurrData);
			pCurrData += 4;
			pCurrData += this._combatStateEffect.Deserialize(pCurrData);
			pCurrData += 4;
			pCurrData += this._changeNeedUseSkill.Deserialize(pCurrData);
			pCurrData += 4;
			pCurrData += this._changeDistanceIsMove.Deserialize(pCurrData);
			pCurrData += 4;
			pCurrData += this._replaceCharHit.Deserialize(pCurrData);
			pCurrData += 4;
			pCurrData += this._canAddPoison.Deserialize(pCurrData);
			pCurrData += 4;
			pCurrData += this._canReducePoison.Deserialize(pCurrData);
			pCurrData += 4;
			pCurrData += this._reducePoisonValue.Deserialize(pCurrData);
			pCurrData += 4;
			pCurrData += this._poisonCanAffect.Deserialize(pCurrData);
			pCurrData += 4;
			pCurrData += this._poisonAffectCount.Deserialize(pCurrData);
			pCurrData += 4;
			pCurrData += this._costTricks.Deserialize(pCurrData);
			pCurrData += 4;
			pCurrData += this._jumpMoveDistance.Deserialize(pCurrData);
			pCurrData += 4;
			pCurrData += this._combatStateToAdd.Deserialize(pCurrData);
			pCurrData += 4;
			pCurrData += this._combatStatePower.Deserialize(pCurrData);
			pCurrData += 4;
			pCurrData += this._breakBodyPartInjuryCount.Deserialize(pCurrData);
			pCurrData += 4;
			pCurrData += this._bodyPartIsBroken.Deserialize(pCurrData);
			pCurrData += 4;
			pCurrData += this._maxTrickCount.Deserialize(pCurrData);
			pCurrData += 4;
			pCurrData += this._maxBreathPercent.Deserialize(pCurrData);
			pCurrData += 4;
			pCurrData += this._maxStancePercent.Deserialize(pCurrData);
			pCurrData += 4;
			pCurrData += this._extraBreathPercent.Deserialize(pCurrData);
			pCurrData += 4;
			pCurrData += this._extraStancePercent.Deserialize(pCurrData);
			pCurrData += 4;
			pCurrData += this._moveCostMobility.Deserialize(pCurrData);
			pCurrData += 4;
			pCurrData += this._defendSkillKeepTime.Deserialize(pCurrData);
			pCurrData += 4;
			pCurrData += this._bounceRange.Deserialize(pCurrData);
			pCurrData += 4;
			pCurrData += this._mindMarkKeepTime.Deserialize(pCurrData);
			pCurrData += 4;
			pCurrData += this._skillMobilityCostPerFrame.Deserialize(pCurrData);
			pCurrData += 4;
			pCurrData += this._canAddWug.Deserialize(pCurrData);
			pCurrData += 4;
			pCurrData += this._hasGodWeaponBuff.Deserialize(pCurrData);
			pCurrData += 4;
			pCurrData += this._hasGodArmorBuff.Deserialize(pCurrData);
			pCurrData += 4;
			pCurrData += this._teammateCmdRequireGenerateValue.Deserialize(pCurrData);
			pCurrData += 4;
			pCurrData += this._teammateCmdEffect.Deserialize(pCurrData);
			pCurrData += 4;
			pCurrData += this._flawRecoverSpeed.Deserialize(pCurrData);
			pCurrData += 4;
			pCurrData += this._acupointRecoverSpeed.Deserialize(pCurrData);
			pCurrData += 4;
			pCurrData += this._mindMarkRecoverSpeed.Deserialize(pCurrData);
			pCurrData += 4;
			pCurrData += this._injuryAutoHealSpeed.Deserialize(pCurrData);
			pCurrData += 4;
			pCurrData += this._canRecoverBreath.Deserialize(pCurrData);
			pCurrData += 4;
			pCurrData += this._canRecoverStance.Deserialize(pCurrData);
			pCurrData += 4;
			pCurrData += this._fatalDamageValue.Deserialize(pCurrData);
			pCurrData += 4;
			pCurrData += this._fatalDamageMarkCount.Deserialize(pCurrData);
			pCurrData += 4;
			pCurrData += this._canFightBackDuringPrepareSkill.Deserialize(pCurrData);
			pCurrData += 4;
			pCurrData += this._skillPrepareSpeed.Deserialize(pCurrData);
			pCurrData += 4;
			pCurrData += this._breathRecoverSpeed.Deserialize(pCurrData);
			pCurrData += 4;
			pCurrData += this._stanceRecoverSpeed.Deserialize(pCurrData);
			pCurrData += 4;
			pCurrData += this._mobilityRecoverSpeed.Deserialize(pCurrData);
			pCurrData += 4;
			pCurrData += this._changeTrickProgressAddValue.Deserialize(pCurrData);
			pCurrData += 4;
			pCurrData += this._power.Deserialize(pCurrData);
			pCurrData += 4;
			pCurrData += this._maxPower.Deserialize(pCurrData);
			pCurrData += 4;
			pCurrData += this._powerCanReduce.Deserialize(pCurrData);
			pCurrData += 4;
			pCurrData += this._useRequirement.Deserialize(pCurrData);
			pCurrData += 4;
			pCurrData += this._currInnerRatio.Deserialize(pCurrData);
			pCurrData += 4;
			pCurrData += this._costBreathAndStance.Deserialize(pCurrData);
			pCurrData += 4;
			pCurrData += this._costBreath.Deserialize(pCurrData);
			pCurrData += 4;
			pCurrData += this._costStance.Deserialize(pCurrData);
			pCurrData += 4;
			pCurrData += this._costMobility.Deserialize(pCurrData);
			pCurrData += 4;
			pCurrData += this._skillCostTricks.Deserialize(pCurrData);
			pCurrData += 4;
			pCurrData += this._effectDirection.Deserialize(pCurrData);
			pCurrData += 4;
			pCurrData += this._effectDirectionCanChange.Deserialize(pCurrData);
			pCurrData += 4;
			pCurrData += this._gridCost.Deserialize(pCurrData);
			pCurrData += 4;
			pCurrData += this._prepareTotalProgress.Deserialize(pCurrData);
			pCurrData += 4;
			pCurrData += this._specificGridCount.Deserialize(pCurrData);
			pCurrData += 4;
			pCurrData += this._genericGridCount.Deserialize(pCurrData);
			pCurrData += 4;
			pCurrData += this._canInterrupt.Deserialize(pCurrData);
			pCurrData += 4;
			pCurrData += this._interruptOdds.Deserialize(pCurrData);
			pCurrData += 4;
			pCurrData += this._canSilence.Deserialize(pCurrData);
			pCurrData += 4;
			pCurrData += this._silenceOdds.Deserialize(pCurrData);
			pCurrData += 4;
			pCurrData += this._canCastWithBrokenBodyPart.Deserialize(pCurrData);
			pCurrData += 4;
			pCurrData += this._addPowerCanBeRemoved.Deserialize(pCurrData);
			pCurrData += 4;
			pCurrData += this._skillType.Deserialize(pCurrData);
			pCurrData += 4;
			pCurrData += this._effectCountCanChange.Deserialize(pCurrData);
			pCurrData += 4;
			pCurrData += this._canCastInDefend.Deserialize(pCurrData);
			pCurrData += 4;
			pCurrData += this._hitDistribution.Deserialize(pCurrData);
			pCurrData += 4;
			pCurrData += this._canCastOnLackBreath.Deserialize(pCurrData);
			pCurrData += 4;
			pCurrData += this._canCastOnLackStance.Deserialize(pCurrData);
			pCurrData += 4;
			pCurrData += this._costBreathOnCast.Deserialize(pCurrData);
			pCurrData += 4;
			pCurrData += this._costStanceOnCast.Deserialize(pCurrData);
			pCurrData += 4;
			pCurrData += this._canUseMobilityAsBreath.Deserialize(pCurrData);
			pCurrData += 4;
			pCurrData += this._canUseMobilityAsStance.Deserialize(pCurrData);
			pCurrData += 4;
			pCurrData += this._castCostNeiliAllocation.Deserialize(pCurrData);
			pCurrData += 4;
			pCurrData += this._acceptPoisonResist.Deserialize(pCurrData);
			pCurrData += 4;
			pCurrData += this._makePoisonResist.Deserialize(pCurrData);
			pCurrData += 4;
			pCurrData += this._canCriticalHit.Deserialize(pCurrData);
			pCurrData += 4;
			pCurrData += this._canCostNeiliAllocationEffect.Deserialize(pCurrData);
			pCurrData += 4;
			pCurrData += this._consummateLevelRelatedMainAttributesHitValues.Deserialize(pCurrData);
			pCurrData += 4;
			pCurrData += this._consummateLevelRelatedMainAttributesAvoidValues.Deserialize(pCurrData);
			pCurrData += 4;
			pCurrData += this._consummateLevelRelatedMainAttributesPenetrations.Deserialize(pCurrData);
			pCurrData += 4;
			pCurrData += this._consummateLevelRelatedMainAttributesPenetrationResists.Deserialize(pCurrData);
			pCurrData += 4;
			pCurrData += this._skillAlsoAsFiveElements.Deserialize(pCurrData);
			pCurrData += 4;
			pCurrData += this._innerInjuryImmunity.Deserialize(pCurrData);
			pCurrData += 4;
			pCurrData += this._outerInjuryImmunity.Deserialize(pCurrData);
			pCurrData += 4;
			pCurrData += this._poisonAffectThreshold.Deserialize(pCurrData);
			pCurrData += 4;
			pCurrData += this._lockDistance.Deserialize(pCurrData);
			pCurrData += 4;
			pCurrData += this._resistOfAllPoison.Deserialize(pCurrData);
			pCurrData += 4;
			pCurrData += this._makePoisonTarget.Deserialize(pCurrData);
			pCurrData += 4;
			pCurrData += this._acceptPoisonTarget.Deserialize(pCurrData);
			pCurrData += 4;
			pCurrData += this._certainCriticalHit.Deserialize(pCurrData);
			pCurrData += 4;
			pCurrData += this._mindMarkCount.Deserialize(pCurrData);
			pCurrData += 4;
			pCurrData += this._canFightBackWithHit.Deserialize(pCurrData);
			pCurrData += 4;
			pCurrData += this._inevitableHit.Deserialize(pCurrData);
			pCurrData += 4;
			pCurrData += this._attackCanPursue.Deserialize(pCurrData);
			pCurrData += 4;
			pCurrData += this._combatSkillDataEffectList.Deserialize(pCurrData);
			pCurrData += 4;
			pCurrData += this._criticalOdds.Deserialize(pCurrData);
			pCurrData += 4;
			pCurrData += this._stanceCostByEffect.Deserialize(pCurrData);
			pCurrData += 4;
			pCurrData += this._breathCostByEffect.Deserialize(pCurrData);
			pCurrData += 4;
			pCurrData += this._powerAddRatio.Deserialize(pCurrData);
			pCurrData += 4;
			pCurrData += this._powerReduceRatio.Deserialize(pCurrData);
			pCurrData += 4;
			pCurrData += this._poisonAffectProduceValue.Deserialize(pCurrData);
			pCurrData += 4;
			pCurrData += this._canReadingOnMonthChange.Deserialize(pCurrData);
			pCurrData += 4;
			pCurrData += this._medicineEffect.Deserialize(pCurrData);
			pCurrData += 4;
			pCurrData += this._xiangshuInfectionDelta.Deserialize(pCurrData);
			pCurrData += 4;
			pCurrData += this._healthDelta.Deserialize(pCurrData);
			pCurrData += 4;
			pCurrData += this._weaponSilenceFrame.Deserialize(pCurrData);
			pCurrData += 4;
			pCurrData += this._silenceFrame.Deserialize(pCurrData);
			pCurrData += 4;
			pCurrData += this._currAgeDelta.Deserialize(pCurrData);
			pCurrData += 4;
			pCurrData += this._goneMadInAllBreak.Deserialize(pCurrData);
			pCurrData += 4;
			pCurrData += this._makeLoveRateOnMonthChange.Deserialize(pCurrData);
			pCurrData += 4;
			pCurrData += this._canAutoHealOnMonthChange.Deserialize(pCurrData);
			pCurrData += 4;
			pCurrData += this._happinessDelta.Deserialize(pCurrData);
			pCurrData += 4;
			pCurrData += this._teammateCmdCanUse.Deserialize(pCurrData);
			pCurrData += 4;
			pCurrData += this._mixPoisonInfinityAffect.Deserialize(pCurrData);
			pCurrData += 4;
			pCurrData += this._attackRangeMaxAcupoint.Deserialize(pCurrData);
			pCurrData += 4;
			pCurrData += this._maxMobilityPercent.Deserialize(pCurrData);
			pCurrData += 4;
			pCurrData += this._makeMindDamage.Deserialize(pCurrData);
			pCurrData += 4;
			pCurrData += this._acceptMindDamage.Deserialize(pCurrData);
			pCurrData += 4;
			pCurrData += this._hitAddByTempValue.Deserialize(pCurrData);
			pCurrData += 4;
			pCurrData += this._avoidAddByTempValue.Deserialize(pCurrData);
			pCurrData += 4;
			pCurrData += this._ignoreEquipmentOverload.Deserialize(pCurrData);
			pCurrData += 4;
			pCurrData += this._canCostEnemyUsableTricks.Deserialize(pCurrData);
			pCurrData += 4;
			pCurrData += this._ignoreArmor.Deserialize(pCurrData);
			pCurrData += 4;
			pCurrData += this._unyieldingFallen.Deserialize(pCurrData);
			pCurrData += 4;
			pCurrData += this._normalAttackPrepareFrame.Deserialize(pCurrData);
			pCurrData += 4;
			pCurrData += this._canCostUselessTricks.Deserialize(pCurrData);
			pCurrData += 4;
			pCurrData += this._defendSkillCanAffect.Deserialize(pCurrData);
			pCurrData += 4;
			pCurrData += this._assistSkillCanAffect.Deserialize(pCurrData);
			pCurrData += 4;
			pCurrData += this._agileSkillCanAffect.Deserialize(pCurrData);
			pCurrData += 4;
			pCurrData += this._allMarkChangeToMind.Deserialize(pCurrData);
			pCurrData += 4;
			pCurrData += this._mindMarkChangeToFatal.Deserialize(pCurrData);
			pCurrData += 4;
			pCurrData += this._canCast.Deserialize(pCurrData);
			pCurrData += 4;
			pCurrData += this._inevitableAvoid.Deserialize(pCurrData);
			pCurrData += 4;
			pCurrData += this._powerEffectReverse.Deserialize(pCurrData);
			pCurrData += 4;
			pCurrData += this._featureBonusReverse.Deserialize(pCurrData);
			pCurrData += 4;
			pCurrData += this._wugFatalDamageValue.Deserialize(pCurrData);
			pCurrData += 4;
			pCurrData += this._canRecoverHealthOnMonthChange.Deserialize(pCurrData);
			pCurrData += 4;
			pCurrData += this._takeRevengeRateOnMonthChange.Deserialize(pCurrData);
			pCurrData += 4;
			pCurrData += this._consummateLevelBonus.Deserialize(pCurrData);
			pCurrData += 4;
			pCurrData += this._neiliDelta.Deserialize(pCurrData);
			pCurrData += 4;
			pCurrData += this._canMakeLoveSpecialOnMonthChange.Deserialize(pCurrData);
			pCurrData += 4;
			pCurrData += this._healAcupointSpeed.Deserialize(pCurrData);
			pCurrData += 4;
			pCurrData += this._maxChangeTrickCount.Deserialize(pCurrData);
			pCurrData += 4;
			pCurrData += this._convertCostBreathAndStance.Deserialize(pCurrData);
			pCurrData += 4;
			pCurrData += this._personalitiesAll.Deserialize(pCurrData);
			pCurrData += 4;
			pCurrData += this._finalFatalDamageMarkCount.Deserialize(pCurrData);
			pCurrData += 4;
			pCurrData += this._infinityMindMarkProgress.Deserialize(pCurrData);
			pCurrData += 4;
			pCurrData += this._combatSkillAiScorePower.Deserialize(pCurrData);
			pCurrData += 4;
			pCurrData += this._normalAttackChangeToUnlockAttack.Deserialize(pCurrData);
			pCurrData += 4;
			pCurrData += this._attackBodyPartOdds.Deserialize(pCurrData);
			pCurrData += 4;
			pCurrData += this._changeDurability.Deserialize(pCurrData);
			pCurrData += 4;
			pCurrData += this._equipmentBonus.Deserialize(pCurrData);
			pCurrData += 4;
			pCurrData += this._equipmentWeight.Deserialize(pCurrData);
			pCurrData += 4;
			pCurrData += this._rawCreateEffectList.Deserialize(pCurrData);
			pCurrData += 4;
			pCurrData += this._jiTrickAsWeaponTrickCount.Deserialize(pCurrData);
			pCurrData += 4;
			pCurrData += this._uselessTrickAsJiTrickCount.Deserialize(pCurrData);
			pCurrData += 4;
			pCurrData += this._equipmentPower.Deserialize(pCurrData);
			pCurrData += 4;
			pCurrData += this._healFlawSpeed.Deserialize(pCurrData);
			pCurrData += 4;
			pCurrData += this._unlockSpeed.Deserialize(pCurrData);
			pCurrData += 4;
			pCurrData += this._flawBonusFactor.Deserialize(pCurrData);
			pCurrData += 4;
			pCurrData += this._canCostShaTricks.Deserialize(pCurrData);
			pCurrData += 4;
			pCurrData += this._defenderDirectFinalDamageValue.Deserialize(pCurrData);
			pCurrData += 4;
			pCurrData += this._normalAttackRecoveryFrame.Deserialize(pCurrData);
			pCurrData += 4;
			pCurrData += this._finalGoneMadInjury.Deserialize(pCurrData);
			pCurrData += 4;
			pCurrData += this._attackerDirectFinalDamageValue.Deserialize(pCurrData);
			pCurrData += 4;
			pCurrData += this._canCostTrickDuringPreparingSkill.Deserialize(pCurrData);
			pCurrData += 4;
			pCurrData += this._validItemList.Deserialize(pCurrData);
			pCurrData += 4;
			pCurrData += this._acceptDamageCanAdd.Deserialize(pCurrData);
			pCurrData += 4;
			pCurrData += this._makeDamageCanReduce.Deserialize(pCurrData);
			pCurrData += 4;
			pCurrData += this._normalAttackGetTrickCount.Deserialize(pCurrData);
			return (int)((long)(pCurrData - pData));
		}

		// Token: 0x040006C9 RID: 1737
		[CollectionObjectField(false, true, false, false, false)]
		private int _id;

		// Token: 0x040006CA RID: 1738
		[CollectionObjectField(false, true, false, false, false)]
		private SpecialEffectList _maxStrength;

		// Token: 0x040006CB RID: 1739
		[CollectionObjectField(false, true, false, false, false)]
		private SpecialEffectList _maxDexterity;

		// Token: 0x040006CC RID: 1740
		[CollectionObjectField(false, true, false, false, false)]
		private SpecialEffectList _maxConcentration;

		// Token: 0x040006CD RID: 1741
		[CollectionObjectField(false, true, false, false, false)]
		private SpecialEffectList _maxVitality;

		// Token: 0x040006CE RID: 1742
		[CollectionObjectField(false, true, false, false, false)]
		private SpecialEffectList _maxEnergy;

		// Token: 0x040006CF RID: 1743
		[CollectionObjectField(false, true, false, false, false)]
		private SpecialEffectList _maxIntelligence;

		// Token: 0x040006D0 RID: 1744
		[CollectionObjectField(false, true, false, false, false)]
		private SpecialEffectList _recoveryOfStance;

		// Token: 0x040006D1 RID: 1745
		[CollectionObjectField(false, true, false, false, false)]
		private SpecialEffectList _recoveryOfBreath;

		// Token: 0x040006D2 RID: 1746
		[CollectionObjectField(false, true, false, false, false)]
		private SpecialEffectList _moveSpeed;

		// Token: 0x040006D3 RID: 1747
		[CollectionObjectField(false, true, false, false, false)]
		private SpecialEffectList _recoveryOfFlaw;

		// Token: 0x040006D4 RID: 1748
		[CollectionObjectField(false, true, false, false, false)]
		private SpecialEffectList _castSpeed;

		// Token: 0x040006D5 RID: 1749
		[CollectionObjectField(false, true, false, false, false)]
		private SpecialEffectList _recoveryOfBlockedAcupoint;

		// Token: 0x040006D6 RID: 1750
		[CollectionObjectField(false, true, false, false, false)]
		private SpecialEffectList _weaponSwitchSpeed;

		// Token: 0x040006D7 RID: 1751
		[CollectionObjectField(false, true, false, false, false)]
		private SpecialEffectList _attackSpeed;

		// Token: 0x040006D8 RID: 1752
		[CollectionObjectField(false, true, false, false, false)]
		private SpecialEffectList _innerRatio;

		// Token: 0x040006D9 RID: 1753
		[CollectionObjectField(false, true, false, false, false)]
		private SpecialEffectList _recoveryOfQiDisorder;

		// Token: 0x040006DA RID: 1754
		[CollectionObjectField(false, true, false, false, false)]
		private SpecialEffectList _minorAttributeFixMaxValue;

		// Token: 0x040006DB RID: 1755
		[CollectionObjectField(false, true, false, false, false)]
		private SpecialEffectList _minorAttributeFixMinValue;

		// Token: 0x040006DC RID: 1756
		[CollectionObjectField(false, true, false, false, false)]
		private SpecialEffectList _resistOfHotPoison;

		// Token: 0x040006DD RID: 1757
		[CollectionObjectField(false, true, false, false, false)]
		private SpecialEffectList _resistOfGloomyPoison;

		// Token: 0x040006DE RID: 1758
		[CollectionObjectField(false, true, false, false, false)]
		private SpecialEffectList _resistOfColdPoison;

		// Token: 0x040006DF RID: 1759
		[CollectionObjectField(false, true, false, false, false)]
		private SpecialEffectList _resistOfRedPoison;

		// Token: 0x040006E0 RID: 1760
		[CollectionObjectField(false, true, false, false, false)]
		private SpecialEffectList _resistOfRottenPoison;

		// Token: 0x040006E1 RID: 1761
		[CollectionObjectField(false, true, false, false, false)]
		private SpecialEffectList _resistOfIllusoryPoison;

		// Token: 0x040006E2 RID: 1762
		[CollectionObjectField(false, true, false, false, false)]
		private SpecialEffectList _resistOfAllPoison;

		// Token: 0x040006E3 RID: 1763
		[CollectionObjectField(false, true, false, false, false)]
		private SpecialEffectList _personalitiesAll;

		// Token: 0x040006E4 RID: 1764
		[CollectionObjectField(false, true, false, false, false)]
		private SpecialEffectList _displayAge;

		// Token: 0x040006E5 RID: 1765
		[CollectionObjectField(false, true, false, false, false)]
		private SpecialEffectList _neiliProportionOfFiveElements;

		// Token: 0x040006E6 RID: 1766
		[CollectionObjectField(false, true, false, false, false)]
		private SpecialEffectList _skillAlsoAsFiveElements;

		// Token: 0x040006E7 RID: 1767
		[CollectionObjectField(false, true, false, false, false)]
		private SpecialEffectList _weaponMaxPower;

		// Token: 0x040006E8 RID: 1768
		[CollectionObjectField(false, true, false, false, false)]
		private SpecialEffectList _weaponUseRequirement;

		// Token: 0x040006E9 RID: 1769
		[CollectionObjectField(false, true, false, false, false)]
		private SpecialEffectList _weaponAttackRange;

		// Token: 0x040006EA RID: 1770
		[CollectionObjectField(false, true, false, false, false)]
		private SpecialEffectList _armorMaxPower;

		// Token: 0x040006EB RID: 1771
		[CollectionObjectField(false, true, false, false, false)]
		private SpecialEffectList _armorUseRequirement;

		// Token: 0x040006EC RID: 1772
		[CollectionObjectField(false, true, false, false, false)]
		private SpecialEffectList _equipmentPower;

		// Token: 0x040006ED RID: 1773
		[CollectionObjectField(false, true, false, false, false)]
		private SpecialEffectList _ignoreEquipmentOverload;

		// Token: 0x040006EE RID: 1774
		[CollectionObjectField(false, true, false, false, false)]
		private SpecialEffectList _equipmentBonus;

		// Token: 0x040006EF RID: 1775
		[CollectionObjectField(false, true, false, false, false)]
		private SpecialEffectList _hitStrength;

		// Token: 0x040006F0 RID: 1776
		[CollectionObjectField(false, true, false, false, false)]
		private SpecialEffectList _hitTechnique;

		// Token: 0x040006F1 RID: 1777
		[CollectionObjectField(false, true, false, false, false)]
		private SpecialEffectList _hitSpeed;

		// Token: 0x040006F2 RID: 1778
		[CollectionObjectField(false, true, false, false, false)]
		private SpecialEffectList _hitMind;

		// Token: 0x040006F3 RID: 1779
		[CollectionObjectField(false, true, false, false, false)]
		private SpecialEffectList _hitAddByTempValue;

		// Token: 0x040006F4 RID: 1780
		[CollectionObjectField(false, true, false, false, false)]
		private SpecialEffectList _hitCanChange;

		// Token: 0x040006F5 RID: 1781
		[CollectionObjectField(false, true, false, false, false)]
		private SpecialEffectList _hitChangeEffectPercent;

		// Token: 0x040006F6 RID: 1782
		[CollectionObjectField(false, true, false, false, false)]
		private SpecialEffectList _avoidStrength;

		// Token: 0x040006F7 RID: 1783
		[CollectionObjectField(false, true, false, false, false)]
		private SpecialEffectList _avoidTechnique;

		// Token: 0x040006F8 RID: 1784
		[CollectionObjectField(false, true, false, false, false)]
		private SpecialEffectList _avoidSpeed;

		// Token: 0x040006F9 RID: 1785
		[CollectionObjectField(false, true, false, false, false)]
		private SpecialEffectList _avoidMind;

		// Token: 0x040006FA RID: 1786
		[CollectionObjectField(false, true, false, false, false)]
		private SpecialEffectList _avoidAddByTempValue;

		// Token: 0x040006FB RID: 1787
		[CollectionObjectField(false, true, false, false, false)]
		private SpecialEffectList _avoidCanChange;

		// Token: 0x040006FC RID: 1788
		[CollectionObjectField(false, true, false, false, false)]
		private SpecialEffectList _avoidChangeEffectPercent;

		// Token: 0x040006FD RID: 1789
		[CollectionObjectField(false, true, false, false, false)]
		private SpecialEffectList _penetrateOuter;

		// Token: 0x040006FE RID: 1790
		[CollectionObjectField(false, true, false, false, false)]
		private SpecialEffectList _penetrateInner;

		// Token: 0x040006FF RID: 1791
		[CollectionObjectField(false, true, false, false, false)]
		private SpecialEffectList _penetrateResistOuter;

		// Token: 0x04000700 RID: 1792
		[CollectionObjectField(false, true, false, false, false)]
		private SpecialEffectList _penetrateResistInner;

		// Token: 0x04000701 RID: 1793
		[CollectionObjectField(false, true, false, false, false)]
		private SpecialEffectList _consummateLevelBonus;

		// Token: 0x04000702 RID: 1794
		[CollectionObjectField(false, true, false, false, false)]
		private SpecialEffectList _consummateLevelRelatedMainAttributesHitValues;

		// Token: 0x04000703 RID: 1795
		[CollectionObjectField(false, true, false, false, false)]
		private SpecialEffectList _consummateLevelRelatedMainAttributesAvoidValues;

		// Token: 0x04000704 RID: 1796
		[CollectionObjectField(false, true, false, false, false)]
		private SpecialEffectList _consummateLevelRelatedMainAttributesPenetrations;

		// Token: 0x04000705 RID: 1797
		[CollectionObjectField(false, true, false, false, false)]
		private SpecialEffectList _consummateLevelRelatedMainAttributesPenetrationResists;

		// Token: 0x04000706 RID: 1798
		[CollectionObjectField(false, true, false, false, false)]
		private SpecialEffectList _neiliAllocationAttack;

		// Token: 0x04000707 RID: 1799
		[CollectionObjectField(false, true, false, false, false)]
		private SpecialEffectList _neiliAllocationAgile;

		// Token: 0x04000708 RID: 1800
		[CollectionObjectField(false, true, false, false, false)]
		private SpecialEffectList _neiliAllocationDefense;

		// Token: 0x04000709 RID: 1801
		[CollectionObjectField(false, true, false, false, false)]
		private SpecialEffectList _neiliAllocationAssist;

		// Token: 0x0400070A RID: 1802
		[CollectionObjectField(false, true, false, false, false)]
		private SpecialEffectList _happiness;

		// Token: 0x0400070B RID: 1803
		[CollectionObjectField(false, true, false, false, false)]
		private SpecialEffectList _maxHealth;

		// Token: 0x0400070C RID: 1804
		[CollectionObjectField(false, true, false, false, false)]
		private SpecialEffectList _healthCost;

		// Token: 0x0400070D RID: 1805
		[CollectionObjectField(false, true, false, false, false)]
		private SpecialEffectList _moveSpeedCanChange;

		// Token: 0x0400070E RID: 1806
		[CollectionObjectField(false, true, false, false, false)]
		private SpecialEffectList _goneMadInAllBreak;

		// Token: 0x0400070F RID: 1807
		[CollectionObjectField(false, true, false, false, false)]
		private SpecialEffectList _xiangshuInfectionDelta;

		// Token: 0x04000710 RID: 1808
		[CollectionObjectField(false, true, false, false, false)]
		private SpecialEffectList _healthDelta;

		// Token: 0x04000711 RID: 1809
		[CollectionObjectField(false, true, false, false, false)]
		private SpecialEffectList _happinessDelta;

		// Token: 0x04000712 RID: 1810
		[CollectionObjectField(false, true, false, false, false)]
		private SpecialEffectList _currAgeDelta;

		// Token: 0x04000713 RID: 1811
		[CollectionObjectField(false, true, false, false, false)]
		private SpecialEffectList _neiliDelta;

		// Token: 0x04000714 RID: 1812
		[CollectionObjectField(false, true, false, false, false)]
		private SpecialEffectList _makeLoveRateOnMonthChange;

		// Token: 0x04000715 RID: 1813
		[CollectionObjectField(false, true, false, false, false)]
		private SpecialEffectList _takeRevengeRateOnMonthChange;

		// Token: 0x04000716 RID: 1814
		[CollectionObjectField(false, true, false, false, false)]
		private SpecialEffectList _canMakeLoveSpecialOnMonthChange;

		// Token: 0x04000717 RID: 1815
		[CollectionObjectField(false, true, false, false, false)]
		private SpecialEffectList _canReadingOnMonthChange;

		// Token: 0x04000718 RID: 1816
		[CollectionObjectField(false, true, false, false, false)]
		private SpecialEffectList _canAutoHealOnMonthChange;

		// Token: 0x04000719 RID: 1817
		[CollectionObjectField(false, true, false, false, false)]
		private SpecialEffectList _canRecoverHealthOnMonthChange;

		// Token: 0x0400071A RID: 1818
		[CollectionObjectField(false, true, false, false, false)]
		private SpecialEffectList _featureBonusReverse;

		// Token: 0x0400071B RID: 1819
		[CollectionObjectField(false, true, false, false, false)]
		private SpecialEffectList _attackerHitStrength;

		// Token: 0x0400071C RID: 1820
		[CollectionObjectField(false, true, false, false, false)]
		private SpecialEffectList _attackerHitTechnique;

		// Token: 0x0400071D RID: 1821
		[CollectionObjectField(false, true, false, false, false)]
		private SpecialEffectList _attackerHitSpeed;

		// Token: 0x0400071E RID: 1822
		[CollectionObjectField(false, true, false, false, false)]
		private SpecialEffectList _attackerHitMind;

		// Token: 0x0400071F RID: 1823
		[CollectionObjectField(false, true, false, false, false)]
		private SpecialEffectList _attackerAvoidStrength;

		// Token: 0x04000720 RID: 1824
		[CollectionObjectField(false, true, false, false, false)]
		private SpecialEffectList _attackerAvoidTechnique;

		// Token: 0x04000721 RID: 1825
		[CollectionObjectField(false, true, false, false, false)]
		private SpecialEffectList _attackerAvoidSpeed;

		// Token: 0x04000722 RID: 1826
		[CollectionObjectField(false, true, false, false, false)]
		private SpecialEffectList _attackerAvoidMind;

		// Token: 0x04000723 RID: 1827
		[CollectionObjectField(false, true, false, false, false)]
		private SpecialEffectList _attackerPenetrateOuter;

		// Token: 0x04000724 RID: 1828
		[CollectionObjectField(false, true, false, false, false)]
		private SpecialEffectList _attackerPenetrateInner;

		// Token: 0x04000725 RID: 1829
		[CollectionObjectField(false, true, false, false, false)]
		private SpecialEffectList _attackerPenetrateResistOuter;

		// Token: 0x04000726 RID: 1830
		[CollectionObjectField(false, true, false, false, false)]
		private SpecialEffectList _attackerPenetrateResistInner;

		// Token: 0x04000727 RID: 1831
		[CollectionObjectField(false, true, false, false, false)]
		private SpecialEffectList _attackHitType;

		// Token: 0x04000728 RID: 1832
		[CollectionObjectField(false, true, false, false, false)]
		private SpecialEffectList _makeDirectDamage;

		// Token: 0x04000729 RID: 1833
		[CollectionObjectField(false, true, false, false, false)]
		private SpecialEffectList _makeMindDamage;

		// Token: 0x0400072A RID: 1834
		[CollectionObjectField(false, true, false, false, false)]
		private SpecialEffectList _makeBounceDamage;

		// Token: 0x0400072B RID: 1835
		[CollectionObjectField(false, true, false, false, false)]
		private SpecialEffectList _makeFightBackDamage;

		// Token: 0x0400072C RID: 1836
		[CollectionObjectField(false, true, false, false, false)]
		private SpecialEffectList _makePoisonLevel;

		// Token: 0x0400072D RID: 1837
		[CollectionObjectField(false, true, false, false, false)]
		private SpecialEffectList _makePoisonValue;

		// Token: 0x0400072E RID: 1838
		[CollectionObjectField(false, true, false, false, false)]
		private SpecialEffectList _makePoisonResist;

		// Token: 0x0400072F RID: 1839
		[CollectionObjectField(false, true, false, false, false)]
		private SpecialEffectList _makePoisonTarget;

		// Token: 0x04000730 RID: 1840
		[CollectionObjectField(false, true, false, false, false)]
		private SpecialEffectList _attackerHitOdds;

		// Token: 0x04000731 RID: 1841
		[CollectionObjectField(false, true, false, false, false)]
		private SpecialEffectList _attackerFightBackHitOdds;

		// Token: 0x04000732 RID: 1842
		[CollectionObjectField(false, true, false, false, false)]
		private SpecialEffectList _attackerPursueOdds;

		// Token: 0x04000733 RID: 1843
		[CollectionObjectField(false, true, false, false, false)]
		private SpecialEffectList _makedInjuryChangeToOld;

		// Token: 0x04000734 RID: 1844
		[CollectionObjectField(false, true, false, false, false)]
		private SpecialEffectList _makedPoisonChangeToOld;

		// Token: 0x04000735 RID: 1845
		[CollectionObjectField(false, true, false, false, false)]
		private SpecialEffectList _allMarkChangeToMind;

		// Token: 0x04000736 RID: 1846
		[CollectionObjectField(false, true, false, false, false)]
		private SpecialEffectList _mindMarkChangeToFatal;

		// Token: 0x04000737 RID: 1847
		[CollectionObjectField(false, true, false, false, false)]
		private SpecialEffectList _infinityMindMarkProgress;

		// Token: 0x04000738 RID: 1848
		[CollectionObjectField(false, true, false, false, false)]
		private SpecialEffectList _makeDamageCanReduce;

		// Token: 0x04000739 RID: 1849
		[CollectionObjectField(false, true, false, false, false)]
		private SpecialEffectList _ignoreArmor;

		// Token: 0x0400073A RID: 1850
		[CollectionObjectField(false, true, false, false, false)]
		private SpecialEffectList _makeDamageType;

		// Token: 0x0400073B RID: 1851
		[CollectionObjectField(false, true, false, false, false)]
		private SpecialEffectList _canMakeInjuryToNoInjuryPart;

		// Token: 0x0400073C RID: 1852
		[CollectionObjectField(false, true, false, false, false)]
		private SpecialEffectList _makePoisonType;

		// Token: 0x0400073D RID: 1853
		[CollectionObjectField(false, true, false, false, false)]
		private SpecialEffectList _normalAttackWeapon;

		// Token: 0x0400073E RID: 1854
		[CollectionObjectField(false, true, false, false, false)]
		private SpecialEffectList _normalAttackTrick;

		// Token: 0x0400073F RID: 1855
		[CollectionObjectField(false, true, false, false, false)]
		private SpecialEffectList _normalAttackGetTrickCount;

		// Token: 0x04000740 RID: 1856
		[CollectionObjectField(false, true, false, false, false)]
		private SpecialEffectList _normalAttackPrepareFrame;

		// Token: 0x04000741 RID: 1857
		[CollectionObjectField(false, true, false, false, false)]
		private SpecialEffectList _normalAttackRecoveryFrame;

		// Token: 0x04000742 RID: 1858
		[CollectionObjectField(false, true, false, false, false)]
		private SpecialEffectList _unlockSpeed;

		// Token: 0x04000743 RID: 1859
		[CollectionObjectField(false, true, false, false, false)]
		private SpecialEffectList _normalAttackChangeToUnlockAttack;

		// Token: 0x04000744 RID: 1860
		[CollectionObjectField(false, true, false, false, false)]
		private SpecialEffectList _rawCreateEffectList;

		// Token: 0x04000745 RID: 1861
		[CollectionObjectField(false, true, false, false, false)]
		private SpecialEffectList _extraFlawCount;

		// Token: 0x04000746 RID: 1862
		[CollectionObjectField(false, true, false, false, false)]
		private SpecialEffectList _flawBonusFactor;

		// Token: 0x04000747 RID: 1863
		[CollectionObjectField(false, true, false, false, false)]
		private SpecialEffectList _attackCanBounce;

		// Token: 0x04000748 RID: 1864
		[CollectionObjectField(false, true, false, false, false)]
		private SpecialEffectList _attackCanFightBack;

		// Token: 0x04000749 RID: 1865
		[CollectionObjectField(false, true, false, false, false)]
		private SpecialEffectList _attackCanPursue;

		// Token: 0x0400074A RID: 1866
		[CollectionObjectField(false, true, false, false, false)]
		private SpecialEffectList _makeFightBackInjuryMark;

		// Token: 0x0400074B RID: 1867
		[CollectionObjectField(false, true, false, false, false)]
		private SpecialEffectList _legSkillUseShoes;

		// Token: 0x0400074C RID: 1868
		[CollectionObjectField(false, true, false, false, false)]
		private SpecialEffectList _attackerDirectFinalDamageValue;

		// Token: 0x0400074D RID: 1869
		[CollectionObjectField(false, true, false, false, false)]
		private SpecialEffectList _attackerFinalDamageValue;

		// Token: 0x0400074E RID: 1870
		[CollectionObjectField(false, true, false, false, false)]
		private SpecialEffectList _defenderHitStrength;

		// Token: 0x0400074F RID: 1871
		[CollectionObjectField(false, true, false, false, false)]
		private SpecialEffectList _defenderHitTechnique;

		// Token: 0x04000750 RID: 1872
		[CollectionObjectField(false, true, false, false, false)]
		private SpecialEffectList _defenderHitSpeed;

		// Token: 0x04000751 RID: 1873
		[CollectionObjectField(false, true, false, false, false)]
		private SpecialEffectList _defenderHitMind;

		// Token: 0x04000752 RID: 1874
		[CollectionObjectField(false, true, false, false, false)]
		private SpecialEffectList _defenderAvoidStrength;

		// Token: 0x04000753 RID: 1875
		[CollectionObjectField(false, true, false, false, false)]
		private SpecialEffectList _defenderAvoidTechnique;

		// Token: 0x04000754 RID: 1876
		[CollectionObjectField(false, true, false, false, false)]
		private SpecialEffectList _defenderAvoidSpeed;

		// Token: 0x04000755 RID: 1877
		[CollectionObjectField(false, true, false, false, false)]
		private SpecialEffectList _defenderAvoidMind;

		// Token: 0x04000756 RID: 1878
		[CollectionObjectField(false, true, false, false, false)]
		private SpecialEffectList _defenderPenetrateOuter;

		// Token: 0x04000757 RID: 1879
		[CollectionObjectField(false, true, false, false, false)]
		private SpecialEffectList _defenderPenetrateInner;

		// Token: 0x04000758 RID: 1880
		[CollectionObjectField(false, true, false, false, false)]
		private SpecialEffectList _defenderPenetrateResistOuter;

		// Token: 0x04000759 RID: 1881
		[CollectionObjectField(false, true, false, false, false)]
		private SpecialEffectList _defenderPenetrateResistInner;

		// Token: 0x0400075A RID: 1882
		[CollectionObjectField(false, true, false, false, false)]
		private SpecialEffectList _acceptDirectDamage;

		// Token: 0x0400075B RID: 1883
		[CollectionObjectField(false, true, false, false, false)]
		private SpecialEffectList _acceptMindDamage;

		// Token: 0x0400075C RID: 1884
		[CollectionObjectField(false, true, false, false, false)]
		private SpecialEffectList _acceptBounceDamage;

		// Token: 0x0400075D RID: 1885
		[CollectionObjectField(false, true, false, false, false)]
		private SpecialEffectList _acceptFightBackDamage;

		// Token: 0x0400075E RID: 1886
		[CollectionObjectField(false, true, false, false, false)]
		private SpecialEffectList _acceptPoisonLevel;

		// Token: 0x0400075F RID: 1887
		[CollectionObjectField(false, true, false, false, false)]
		private SpecialEffectList _acceptPoisonValue;

		// Token: 0x04000760 RID: 1888
		[CollectionObjectField(false, true, false, false, false)]
		private SpecialEffectList _acceptPoisonResist;

		// Token: 0x04000761 RID: 1889
		[CollectionObjectField(false, true, false, false, false)]
		private SpecialEffectList _acceptPoisonTarget;

		// Token: 0x04000762 RID: 1890
		[CollectionObjectField(false, true, false, false, false)]
		private SpecialEffectList _defenderHitOdds;

		// Token: 0x04000763 RID: 1891
		[CollectionObjectField(false, true, false, false, false)]
		private SpecialEffectList _defenderFightBackHitOdds;

		// Token: 0x04000764 RID: 1892
		[CollectionObjectField(false, true, false, false, false)]
		private SpecialEffectList _defenderPursueOdds;

		// Token: 0x04000765 RID: 1893
		[CollectionObjectField(false, true, false, false, false)]
		private SpecialEffectList _acceptDamageCanAdd;

		// Token: 0x04000766 RID: 1894
		[CollectionObjectField(false, true, false, false, false)]
		private SpecialEffectList _acceptMaxInjuryCount;

		// Token: 0x04000767 RID: 1895
		[CollectionObjectField(false, true, false, false, false)]
		private SpecialEffectList _bouncePower;

		// Token: 0x04000768 RID: 1896
		[CollectionObjectField(false, true, false, false, false)]
		private SpecialEffectList _fightBackPower;

		// Token: 0x04000769 RID: 1897
		[CollectionObjectField(false, true, false, false, false)]
		private SpecialEffectList _directDamageInnerRatio;

		// Token: 0x0400076A RID: 1898
		[CollectionObjectField(false, true, false, false, false)]
		private SpecialEffectList _defenderDirectFinalDamageValue;

		// Token: 0x0400076B RID: 1899
		[CollectionObjectField(false, true, false, false, false)]
		private SpecialEffectList _defenderFinalDamageValue;

		// Token: 0x0400076C RID: 1900
		[CollectionObjectField(false, true, false, false, false)]
		private SpecialEffectList _unyieldingFallen;

		// Token: 0x0400076D RID: 1901
		[CollectionObjectField(false, true, false, false, false)]
		private SpecialEffectList _outerInjuryImmunity;

		// Token: 0x0400076E RID: 1902
		[CollectionObjectField(false, true, false, false, false)]
		private SpecialEffectList _innerInjuryImmunity;

		// Token: 0x0400076F RID: 1903
		[CollectionObjectField(false, true, false, false, false)]
		private SpecialEffectList _directDamageValue;

		// Token: 0x04000770 RID: 1904
		[CollectionObjectField(false, true, false, false, false)]
		private SpecialEffectList _directInjuryMark;

		// Token: 0x04000771 RID: 1905
		[CollectionObjectField(false, true, false, false, false)]
		private SpecialEffectList _goneMadInjury;

		// Token: 0x04000772 RID: 1906
		[CollectionObjectField(false, true, false, false, false)]
		private SpecialEffectList _finalGoneMadInjury;

		// Token: 0x04000773 RID: 1907
		[CollectionObjectField(false, true, false, false, false)]
		private SpecialEffectList _healInjurySpeed;

		// Token: 0x04000774 RID: 1908
		[CollectionObjectField(false, true, false, false, false)]
		private SpecialEffectList _healInjuryBuff;

		// Token: 0x04000775 RID: 1909
		[CollectionObjectField(false, true, false, false, false)]
		private SpecialEffectList _healInjuryDebuff;

		// Token: 0x04000776 RID: 1910
		[CollectionObjectField(false, true, false, false, false)]
		private SpecialEffectList _healPoisonSpeed;

		// Token: 0x04000777 RID: 1911
		[CollectionObjectField(false, true, false, false, false)]
		private SpecialEffectList _healPoisonBuff;

		// Token: 0x04000778 RID: 1912
		[CollectionObjectField(false, true, false, false, false)]
		private SpecialEffectList _healPoisonDebuff;

		// Token: 0x04000779 RID: 1913
		[CollectionObjectField(false, true, false, false, false)]
		private SpecialEffectList _medicineEffect;

		// Token: 0x0400077A RID: 1914
		[CollectionObjectField(false, true, false, false, false)]
		private SpecialEffectList _healFlawSpeed;

		// Token: 0x0400077B RID: 1915
		[CollectionObjectField(false, true, false, false, false)]
		private SpecialEffectList _healAcupointSpeed;

		// Token: 0x0400077C RID: 1916
		[CollectionObjectField(false, true, false, false, false)]
		private SpecialEffectList _fleeSpeed;

		// Token: 0x0400077D RID: 1917
		[CollectionObjectField(false, true, false, false, false)]
		private SpecialEffectList _maxFlawCount;

		// Token: 0x0400077E RID: 1918
		[CollectionObjectField(false, true, false, false, false)]
		private SpecialEffectList _canAddFlaw;

		// Token: 0x0400077F RID: 1919
		[CollectionObjectField(false, true, false, false, false)]
		private SpecialEffectList _flawLevel;

		// Token: 0x04000780 RID: 1920
		[CollectionObjectField(false, true, false, false, false)]
		private SpecialEffectList _flawLevelCanReduce;

		// Token: 0x04000781 RID: 1921
		[CollectionObjectField(false, true, false, false, false)]
		private SpecialEffectList _flawCount;

		// Token: 0x04000782 RID: 1922
		[CollectionObjectField(false, true, false, false, false)]
		private SpecialEffectList _maxAcupointCount;

		// Token: 0x04000783 RID: 1923
		[CollectionObjectField(false, true, false, false, false)]
		private SpecialEffectList _canAddAcupoint;

		// Token: 0x04000784 RID: 1924
		[CollectionObjectField(false, true, false, false, false)]
		private SpecialEffectList _acupointLevel;

		// Token: 0x04000785 RID: 1925
		[CollectionObjectField(false, true, false, false, false)]
		private SpecialEffectList _acupointLevelCanReduce;

		// Token: 0x04000786 RID: 1926
		[CollectionObjectField(false, true, false, false, false)]
		private SpecialEffectList _acupointCount;

		// Token: 0x04000787 RID: 1927
		[CollectionObjectField(false, true, false, false, false)]
		private SpecialEffectList _addNeiliAllocation;

		// Token: 0x04000788 RID: 1928
		[CollectionObjectField(false, true, false, false, false)]
		private SpecialEffectList _costNeiliAllocation;

		// Token: 0x04000789 RID: 1929
		[CollectionObjectField(false, true, false, false, false)]
		private SpecialEffectList _canChangeNeiliAllocation;

		// Token: 0x0400078A RID: 1930
		[CollectionObjectField(false, true, false, false, false)]
		private SpecialEffectList _canGetTrick;

		// Token: 0x0400078B RID: 1931
		[CollectionObjectField(false, true, false, false, false)]
		private SpecialEffectList _getTrickType;

		// Token: 0x0400078C RID: 1932
		[CollectionObjectField(false, true, false, false, false)]
		private SpecialEffectList _attackBodyPart;

		// Token: 0x0400078D RID: 1933
		[CollectionObjectField(false, true, false, false, false)]
		private SpecialEffectList _attackBodyPartOdds;

		// Token: 0x0400078E RID: 1934
		[CollectionObjectField(false, true, false, false, false)]
		private SpecialEffectList _weaponEquipAttack;

		// Token: 0x0400078F RID: 1935
		[CollectionObjectField(false, true, false, false, false)]
		private SpecialEffectList _weaponEquipDefense;

		// Token: 0x04000790 RID: 1936
		[CollectionObjectField(false, true, false, false, false)]
		private SpecialEffectList _armorEquipAttack;

		// Token: 0x04000791 RID: 1937
		[CollectionObjectField(false, true, false, false, false)]
		private SpecialEffectList _armorEquipDefense;

		// Token: 0x04000792 RID: 1938
		[CollectionObjectField(false, true, false, false, false)]
		private SpecialEffectList _equipmentWeight;

		// Token: 0x04000793 RID: 1939
		[CollectionObjectField(false, true, false, false, false)]
		private SpecialEffectList _attackRangeForward;

		// Token: 0x04000794 RID: 1940
		[CollectionObjectField(false, true, false, false, false)]
		private SpecialEffectList _attackRangeBackward;

		// Token: 0x04000795 RID: 1941
		[CollectionObjectField(false, true, false, false, false)]
		private SpecialEffectList _attackRangeMaxAcupoint;

		// Token: 0x04000796 RID: 1942
		[CollectionObjectField(false, true, false, false, false)]
		private SpecialEffectList _moveCanBeStopped;

		// Token: 0x04000797 RID: 1943
		[CollectionObjectField(false, true, false, false, false)]
		private SpecialEffectList _canForcedMove;

		// Token: 0x04000798 RID: 1944
		[CollectionObjectField(false, true, false, false, false)]
		private SpecialEffectList _mobilityCanBeRemoved;

		// Token: 0x04000799 RID: 1945
		[CollectionObjectField(false, true, false, false, false)]
		private SpecialEffectList _mobilityCostByEffect;

		// Token: 0x0400079A RID: 1946
		[CollectionObjectField(false, true, false, false, false)]
		private SpecialEffectList _stanceCostByEffect;

		// Token: 0x0400079B RID: 1947
		[CollectionObjectField(false, true, false, false, false)]
		private SpecialEffectList _breathCostByEffect;

		// Token: 0x0400079C RID: 1948
		[CollectionObjectField(false, true, false, false, false)]
		private SpecialEffectList _moveDistance;

		// Token: 0x0400079D RID: 1949
		[CollectionObjectField(false, true, false, false, false)]
		private SpecialEffectList _lockDistance;

		// Token: 0x0400079E RID: 1950
		[CollectionObjectField(false, true, false, false, false)]
		private SpecialEffectList _jumpPrepareFrame;

		// Token: 0x0400079F RID: 1951
		[CollectionObjectField(false, true, false, false, false)]
		private SpecialEffectList _bounceInjuryMark;

		// Token: 0x040007A0 RID: 1952
		[CollectionObjectField(false, true, false, false, false)]
		private SpecialEffectList _skillHasCost;

		// Token: 0x040007A1 RID: 1953
		[CollectionObjectField(false, true, false, false, false)]
		private SpecialEffectList _combatStateEffect;

		// Token: 0x040007A2 RID: 1954
		[CollectionObjectField(false, true, false, false, false)]
		private SpecialEffectList _changeNeedUseSkill;

		// Token: 0x040007A3 RID: 1955
		[CollectionObjectField(false, true, false, false, false)]
		private SpecialEffectList _changeDistanceIsMove;

		// Token: 0x040007A4 RID: 1956
		[CollectionObjectField(false, true, false, false, false)]
		private SpecialEffectList _replaceCharHit;

		// Token: 0x040007A5 RID: 1957
		[CollectionObjectField(false, true, false, false, false)]
		private SpecialEffectList _canAddPoison;

		// Token: 0x040007A6 RID: 1958
		[CollectionObjectField(false, true, false, false, false)]
		private SpecialEffectList _canReducePoison;

		// Token: 0x040007A7 RID: 1959
		[CollectionObjectField(false, true, false, false, false)]
		private SpecialEffectList _reducePoisonValue;

		// Token: 0x040007A8 RID: 1960
		[CollectionObjectField(false, true, false, false, false)]
		private SpecialEffectList _poisonCanAffect;

		// Token: 0x040007A9 RID: 1961
		[CollectionObjectField(false, true, false, false, false)]
		private SpecialEffectList _poisonAffectCount;

		// Token: 0x040007AA RID: 1962
		[CollectionObjectField(false, true, false, false, false)]
		private SpecialEffectList _poisonAffectThreshold;

		// Token: 0x040007AB RID: 1963
		[CollectionObjectField(false, true, false, false, false)]
		private SpecialEffectList _poisonAffectProduceValue;

		// Token: 0x040007AC RID: 1964
		[CollectionObjectField(false, true, false, false, false)]
		private SpecialEffectList _mixPoisonInfinityAffect;

		// Token: 0x040007AD RID: 1965
		[CollectionObjectField(false, true, false, false, false)]
		private SpecialEffectList _costTricks;

		// Token: 0x040007AE RID: 1966
		[CollectionObjectField(false, true, false, false, false)]
		private SpecialEffectList _jiTrickAsWeaponTrickCount;

		// Token: 0x040007AF RID: 1967
		[CollectionObjectField(false, true, false, false, false)]
		private SpecialEffectList _uselessTrickAsJiTrickCount;

		// Token: 0x040007B0 RID: 1968
		[CollectionObjectField(false, true, false, false, false)]
		private SpecialEffectList _changeDurability;

		// Token: 0x040007B1 RID: 1969
		[CollectionObjectField(false, true, false, false, false)]
		private SpecialEffectList _jumpMoveDistance;

		// Token: 0x040007B2 RID: 1970
		[CollectionObjectField(false, true, false, false, false)]
		private SpecialEffectList _combatStateToAdd;

		// Token: 0x040007B3 RID: 1971
		[CollectionObjectField(false, true, false, false, false)]
		private SpecialEffectList _combatStatePower;

		// Token: 0x040007B4 RID: 1972
		[CollectionObjectField(false, true, false, false, false)]
		private SpecialEffectList _breakBodyPartInjuryCount;

		// Token: 0x040007B5 RID: 1973
		[CollectionObjectField(false, true, false, false, false)]
		private SpecialEffectList _bodyPartIsBroken;

		// Token: 0x040007B6 RID: 1974
		[CollectionObjectField(false, true, false, false, false)]
		private SpecialEffectList _maxTrickCount;

		// Token: 0x040007B7 RID: 1975
		[CollectionObjectField(false, true, false, false, false)]
		private SpecialEffectList _maxBreathPercent;

		// Token: 0x040007B8 RID: 1976
		[CollectionObjectField(false, true, false, false, false)]
		private SpecialEffectList _maxStancePercent;

		// Token: 0x040007B9 RID: 1977
		[CollectionObjectField(false, true, false, false, false)]
		private SpecialEffectList _maxMobilityPercent;

		// Token: 0x040007BA RID: 1978
		[CollectionObjectField(false, true, false, false, false)]
		private SpecialEffectList _extraBreathPercent;

		// Token: 0x040007BB RID: 1979
		[CollectionObjectField(false, true, false, false, false)]
		private SpecialEffectList _extraStancePercent;

		// Token: 0x040007BC RID: 1980
		[CollectionObjectField(false, true, false, false, false)]
		private SpecialEffectList _moveCostMobility;

		// Token: 0x040007BD RID: 1981
		[CollectionObjectField(false, true, false, false, false)]
		private SpecialEffectList _defendSkillKeepTime;

		// Token: 0x040007BE RID: 1982
		[CollectionObjectField(false, true, false, false, false)]
		private SpecialEffectList _bounceRange;

		// Token: 0x040007BF RID: 1983
		[CollectionObjectField(false, true, false, false, false)]
		private SpecialEffectList _mindMarkKeepTime;

		// Token: 0x040007C0 RID: 1984
		[CollectionObjectField(false, true, false, false, false)]
		private SpecialEffectList _mindMarkCount;

		// Token: 0x040007C1 RID: 1985
		[CollectionObjectField(false, true, false, false, false)]
		private SpecialEffectList _skillMobilityCostPerFrame;

		// Token: 0x040007C2 RID: 1986
		[CollectionObjectField(false, true, false, false, false)]
		private SpecialEffectList _canAddWug;

		// Token: 0x040007C3 RID: 1987
		[CollectionObjectField(false, true, false, false, false)]
		private SpecialEffectList _hasGodWeaponBuff;

		// Token: 0x040007C4 RID: 1988
		[CollectionObjectField(false, true, false, false, false)]
		private SpecialEffectList _hasGodArmorBuff;

		// Token: 0x040007C5 RID: 1989
		[CollectionObjectField(false, true, false, false, false)]
		private SpecialEffectList _teammateCmdRequireGenerateValue;

		// Token: 0x040007C6 RID: 1990
		[CollectionObjectField(false, true, false, false, false)]
		private SpecialEffectList _teammateCmdEffect;

		// Token: 0x040007C7 RID: 1991
		[CollectionObjectField(false, true, false, false, false)]
		private SpecialEffectList _teammateCmdCanUse;

		// Token: 0x040007C8 RID: 1992
		[CollectionObjectField(false, true, false, false, false)]
		private SpecialEffectList _flawRecoverSpeed;

		// Token: 0x040007C9 RID: 1993
		[CollectionObjectField(false, true, false, false, false)]
		private SpecialEffectList _acupointRecoverSpeed;

		// Token: 0x040007CA RID: 1994
		[CollectionObjectField(false, true, false, false, false)]
		private SpecialEffectList _mindMarkRecoverSpeed;

		// Token: 0x040007CB RID: 1995
		[CollectionObjectField(false, true, false, false, false)]
		private SpecialEffectList _injuryAutoHealSpeed;

		// Token: 0x040007CC RID: 1996
		[CollectionObjectField(false, true, false, false, false)]
		private SpecialEffectList _canRecoverBreath;

		// Token: 0x040007CD RID: 1997
		[CollectionObjectField(false, true, false, false, false)]
		private SpecialEffectList _canRecoverStance;

		// Token: 0x040007CE RID: 1998
		[CollectionObjectField(false, true, false, false, false)]
		private SpecialEffectList _fatalDamageValue;

		// Token: 0x040007CF RID: 1999
		[CollectionObjectField(false, true, false, false, false)]
		private SpecialEffectList _wugFatalDamageValue;

		// Token: 0x040007D0 RID: 2000
		[CollectionObjectField(false, true, false, false, false)]
		private SpecialEffectList _fatalDamageMarkCount;

		// Token: 0x040007D1 RID: 2001
		[CollectionObjectField(false, true, false, false, false)]
		private SpecialEffectList _finalFatalDamageMarkCount;

		// Token: 0x040007D2 RID: 2002
		[CollectionObjectField(false, true, false, false, false)]
		private SpecialEffectList _canFightBackDuringPrepareSkill;

		// Token: 0x040007D3 RID: 2003
		[CollectionObjectField(false, true, false, false, false)]
		private SpecialEffectList _canFightBackWithHit;

		// Token: 0x040007D4 RID: 2004
		[CollectionObjectField(false, true, false, false, false)]
		private SpecialEffectList _skillPrepareSpeed;

		// Token: 0x040007D5 RID: 2005
		[CollectionObjectField(false, true, false, false, false)]
		private SpecialEffectList _breathRecoverSpeed;

		// Token: 0x040007D6 RID: 2006
		[CollectionObjectField(false, true, false, false, false)]
		private SpecialEffectList _stanceRecoverSpeed;

		// Token: 0x040007D7 RID: 2007
		[CollectionObjectField(false, true, false, false, false)]
		private SpecialEffectList _mobilityRecoverSpeed;

		// Token: 0x040007D8 RID: 2008
		[CollectionObjectField(false, true, false, false, false)]
		private SpecialEffectList _maxChangeTrickCount;

		// Token: 0x040007D9 RID: 2009
		[CollectionObjectField(false, true, false, false, false)]
		private SpecialEffectList _changeTrickProgressAddValue;

		// Token: 0x040007DA RID: 2010
		[CollectionObjectField(false, true, false, false, false)]
		private SpecialEffectList _agileSkillCanAffect;

		// Token: 0x040007DB RID: 2011
		[CollectionObjectField(false, true, false, false, false)]
		private SpecialEffectList _defendSkillCanAffect;

		// Token: 0x040007DC RID: 2012
		[CollectionObjectField(false, true, false, false, false)]
		private SpecialEffectList _assistSkillCanAffect;

		// Token: 0x040007DD RID: 2013
		[CollectionObjectField(false, true, false, false, false)]
		private SpecialEffectList _power;

		// Token: 0x040007DE RID: 2014
		[CollectionObjectField(false, true, false, false, false)]
		private SpecialEffectList _maxPower;

		// Token: 0x040007DF RID: 2015
		[CollectionObjectField(false, true, false, false, false)]
		private SpecialEffectList _powerCanReduce;

		// Token: 0x040007E0 RID: 2016
		[CollectionObjectField(false, true, false, false, false)]
		private SpecialEffectList _powerAddRatio;

		// Token: 0x040007E1 RID: 2017
		[CollectionObjectField(false, true, false, false, false)]
		private SpecialEffectList _powerReduceRatio;

		// Token: 0x040007E2 RID: 2018
		[CollectionObjectField(false, true, false, false, false)]
		private SpecialEffectList _powerEffectReverse;

		// Token: 0x040007E3 RID: 2019
		[CollectionObjectField(false, true, false, false, false)]
		private SpecialEffectList _useRequirement;

		// Token: 0x040007E4 RID: 2020
		[CollectionObjectField(false, true, false, false, false)]
		private SpecialEffectList _currInnerRatio;

		// Token: 0x040007E5 RID: 2021
		[CollectionObjectField(false, true, false, false, false)]
		private SpecialEffectList _costBreathAndStance;

		// Token: 0x040007E6 RID: 2022
		[CollectionObjectField(false, true, false, false, false)]
		private SpecialEffectList _costBreath;

		// Token: 0x040007E7 RID: 2023
		[CollectionObjectField(false, true, false, false, false)]
		private SpecialEffectList _costStance;

		// Token: 0x040007E8 RID: 2024
		[CollectionObjectField(false, true, false, false, false)]
		private SpecialEffectList _costMobility;

		// Token: 0x040007E9 RID: 2025
		[CollectionObjectField(false, true, false, false, false)]
		private SpecialEffectList _skillCostTricks;

		// Token: 0x040007EA RID: 2026
		[CollectionObjectField(false, true, false, false, false)]
		private SpecialEffectList _canCostEnemyUsableTricks;

		// Token: 0x040007EB RID: 2027
		[CollectionObjectField(false, true, false, false, false)]
		private SpecialEffectList _canCostUselessTricks;

		// Token: 0x040007EC RID: 2028
		[CollectionObjectField(false, true, false, false, false)]
		private SpecialEffectList _canCostShaTricks;

		// Token: 0x040007ED RID: 2029
		[CollectionObjectField(false, true, false, false, false)]
		private SpecialEffectList _effectDirection;

		// Token: 0x040007EE RID: 2030
		[CollectionObjectField(false, true, false, false, false)]
		private SpecialEffectList _effectDirectionCanChange;

		// Token: 0x040007EF RID: 2031
		[CollectionObjectField(false, true, false, false, false)]
		private SpecialEffectList _gridCost;

		// Token: 0x040007F0 RID: 2032
		[CollectionObjectField(false, true, false, false, false)]
		private SpecialEffectList _prepareTotalProgress;

		// Token: 0x040007F1 RID: 2033
		[CollectionObjectField(false, true, false, false, false)]
		private SpecialEffectList _specificGridCount;

		// Token: 0x040007F2 RID: 2034
		[CollectionObjectField(false, true, false, false, false)]
		private SpecialEffectList _genericGridCount;

		// Token: 0x040007F3 RID: 2035
		[CollectionObjectField(false, true, false, false, false)]
		private SpecialEffectList _canCriticalHit;

		// Token: 0x040007F4 RID: 2036
		[CollectionObjectField(false, true, false, false, false)]
		private SpecialEffectList _criticalOdds;

		// Token: 0x040007F5 RID: 2037
		[CollectionObjectField(false, true, false, false, false)]
		private SpecialEffectList _certainCriticalHit;

		// Token: 0x040007F6 RID: 2038
		[CollectionObjectField(false, true, false, false, false)]
		private SpecialEffectList _inevitableHit;

		// Token: 0x040007F7 RID: 2039
		[CollectionObjectField(false, true, false, false, false)]
		private SpecialEffectList _inevitableAvoid;

		// Token: 0x040007F8 RID: 2040
		[CollectionObjectField(false, true, false, false, false)]
		private SpecialEffectList _canInterrupt;

		// Token: 0x040007F9 RID: 2041
		[CollectionObjectField(false, true, false, false, false)]
		private SpecialEffectList _interruptOdds;

		// Token: 0x040007FA RID: 2042
		[CollectionObjectField(false, true, false, false, false)]
		private SpecialEffectList _canSilence;

		// Token: 0x040007FB RID: 2043
		[CollectionObjectField(false, true, false, false, false)]
		private SpecialEffectList _silenceOdds;

		// Token: 0x040007FC RID: 2044
		[CollectionObjectField(false, true, false, false, false)]
		private SpecialEffectList _silenceFrame;

		// Token: 0x040007FD RID: 2045
		[CollectionObjectField(false, true, false, false, false)]
		private SpecialEffectList _weaponSilenceFrame;

		// Token: 0x040007FE RID: 2046
		[CollectionObjectField(false, true, false, false, false)]
		private SpecialEffectList _canCastWithBrokenBodyPart;

		// Token: 0x040007FF RID: 2047
		[CollectionObjectField(false, true, false, false, false)]
		private SpecialEffectList _addPowerCanBeRemoved;

		// Token: 0x04000800 RID: 2048
		[CollectionObjectField(false, true, false, false, false)]
		private SpecialEffectList _skillType;

		// Token: 0x04000801 RID: 2049
		[CollectionObjectField(false, true, false, false, false)]
		private SpecialEffectList _effectCountCanChange;

		// Token: 0x04000802 RID: 2050
		[CollectionObjectField(false, true, false, false, false)]
		private SpecialEffectList _canCast;

		// Token: 0x04000803 RID: 2051
		[CollectionObjectField(false, true, false, false, false)]
		private SpecialEffectList _canCastInDefend;

		// Token: 0x04000804 RID: 2052
		[CollectionObjectField(false, true, false, false, false)]
		private SpecialEffectList _hitDistribution;

		// Token: 0x04000805 RID: 2053
		[CollectionObjectField(false, true, false, false, false)]
		private SpecialEffectList _canCastOnLackBreath;

		// Token: 0x04000806 RID: 2054
		[CollectionObjectField(false, true, false, false, false)]
		private SpecialEffectList _canCastOnLackStance;

		// Token: 0x04000807 RID: 2055
		[CollectionObjectField(false, true, false, false, false)]
		private SpecialEffectList _convertCostBreathAndStance;

		// Token: 0x04000808 RID: 2056
		[CollectionObjectField(false, true, false, false, false)]
		private SpecialEffectList _costBreathOnCast;

		// Token: 0x04000809 RID: 2057
		[CollectionObjectField(false, true, false, false, false)]
		private SpecialEffectList _costStanceOnCast;

		// Token: 0x0400080A RID: 2058
		[CollectionObjectField(false, true, false, false, false)]
		private SpecialEffectList _canUseMobilityAsBreath;

		// Token: 0x0400080B RID: 2059
		[CollectionObjectField(false, true, false, false, false)]
		private SpecialEffectList _canUseMobilityAsStance;

		// Token: 0x0400080C RID: 2060
		[CollectionObjectField(false, true, false, false, false)]
		private SpecialEffectList _castCostNeiliAllocation;

		// Token: 0x0400080D RID: 2061
		[CollectionObjectField(false, true, false, false, false)]
		private SpecialEffectList _canCostNeiliAllocationEffect;

		// Token: 0x0400080E RID: 2062
		[CollectionObjectField(false, true, false, false, false)]
		private SpecialEffectList _canCostTrickDuringPreparingSkill;

		// Token: 0x0400080F RID: 2063
		[CollectionObjectField(false, true, false, false, false)]
		private SpecialEffectList _validItemList;

		// Token: 0x04000810 RID: 2064
		[CollectionObjectField(false, true, false, false, false)]
		private SpecialEffectList _combatSkillDataEffectList;

		// Token: 0x04000811 RID: 2065
		[CollectionObjectField(false, true, false, false, false)]
		private SpecialEffectList _combatSkillAiScorePower;

		// Token: 0x04000812 RID: 2066
		public const int FixedSize = 4;

		// Token: 0x04000813 RID: 2067
		public const int DynamicCount = 328;

		// Token: 0x02000A05 RID: 2565
		internal class FixedFieldInfos
		{
			// Token: 0x04002965 RID: 10597
			public const uint Id_Offset = 0U;

			// Token: 0x04002966 RID: 10598
			public const int Id_Size = 4;
		}
	}
}
