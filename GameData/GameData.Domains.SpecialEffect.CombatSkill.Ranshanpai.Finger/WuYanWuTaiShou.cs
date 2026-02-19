using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Character;
using GameData.Domains.CombatSkill;

namespace GameData.Domains.SpecialEffect.CombatSkill.Ranshanpai.Finger;

public class WuYanWuTaiShou : CombatSkillEffectBase
{
	private readonly sbyte[] _requirePersonalities = new sbyte[5] { 0, 1, 2, 3, 4 };

	private const sbyte AddTrickOrMarkNeedPersonalityCount = 3;

	private const sbyte AddTrickOrMarkCount = 2;

	private const short FameMultiplier = 3;

	public WuYanWuTaiShou()
	{
	}

	public WuYanWuTaiShou(CombatSkillKey skillKey)
		: base(skillKey, 7102, -1)
	{
	}

	public override void OnEnable(DataContext context)
	{
		Events.RegisterHandler_CastSkillEnd(OnCastSkillEnd);
	}

	public override void OnDisable(DataContext context)
	{
		Events.UnRegisterHandler_CastSkillEnd(OnCastSkillEnd);
	}

	private void OnCastSkillEnd(DataContext context, int charId, bool isAlly, short skillId, sbyte power, bool interrupted)
	{
		if (charId != base.CharacterId || skillId != base.SkillTemplateId)
		{
			return;
		}
		if (PowerMatchAffectRequire(power) && CalcAbovePersonalityCount() >= 3)
		{
			if (base.IsDirect)
			{
				DomainManager.Combat.AddTrick(context, base.CurrEnemyChar, 20, 2, addedByAlly: false);
			}
			else
			{
				DomainManager.Combat.AppendMindDefeatMark(context, base.CurrEnemyChar, 2, -1);
			}
			RecordFameAction(context);
			ShowSpecialEffectTips(0);
			ShowSpecialEffectTips(1);
		}
		RemoveSelf(context);
	}

	private int CalcAbovePersonalityCount()
	{
		Personalities personalities = CharObj.GetPersonalities();
		Personalities personalities2 = base.CurrEnemyChar.GetCharacter().GetPersonalities();
		int num = 0;
		sbyte[] requirePersonalities = _requirePersonalities;
		foreach (sbyte index in requirePersonalities)
		{
			if (personalities[index] > personalities2[index])
			{
				num++;
			}
		}
		return num;
	}

	private void RecordFameAction(DataContext context)
	{
		GameData.Domains.Character.Character character = base.CombatChar.GetCharacter();
		GameData.Domains.Character.Character character2 = base.CurrEnemyChar.GetCharacter();
		short fameActionId = (short)(41 + ((!base.IsDirect) ? 1 : 0));
		short fameActionId2 = (short)(base.IsDirect ? 52 : 53);
		character.RecordFameAction(context, fameActionId, -1, 3);
		character2.RecordFameAction(context, fameActionId2, -1, 3);
	}
}
