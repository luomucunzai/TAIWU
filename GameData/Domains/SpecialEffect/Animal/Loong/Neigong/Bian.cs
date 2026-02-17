using System;
using System.Linq;
using GameData.Combat.Math;
using GameData.Common;
using GameData.Domains.Character;
using GameData.Domains.CombatSkill;

namespace GameData.Domains.SpecialEffect.Animal.Loong.Neigong
{
	// Token: 0x020005FE RID: 1534
	public class Bian : AnimalEffectBase
	{
		// Token: 0x060044FF RID: 17663 RVA: 0x00270FFE File Offset: 0x0026F1FE
		public Bian()
		{
		}

		// Token: 0x06004500 RID: 17664 RVA: 0x00271008 File Offset: 0x0026F208
		public Bian(CombatSkillKey skillKey) : base(skillKey)
		{
		}

		// Token: 0x06004501 RID: 17665 RVA: 0x00271013 File Offset: 0x0026F213
		public override void OnEnable(DataContext context)
		{
			base.OnEnable(context);
			base.CreateAffectedData(64, EDataModifyType.AddPercent, -1);
			base.CreateAffectedData(65, EDataModifyType.AddPercent, -1);
			base.CreateAffectedData(100, EDataModifyType.AddPercent, -1);
			base.CreateAffectedData(101, EDataModifyType.AddPercent, -1);
		}

		// Token: 0x06004502 RID: 17666 RVA: 0x0027104C File Offset: 0x0026F24C
		public override int GetModifyValue(AffectedDataKey dataKey, int currModifyValue)
		{
			bool flag = dataKey.CharId != base.CharacterId;
			bool flag2 = flag;
			if (!flag2)
			{
				ushort fieldId = dataKey.FieldId;
				bool flag3 = fieldId - 64 <= 1 || fieldId - 100 <= 1;
				flag2 = !flag3;
			}
			bool flag4 = flag2;
			int result;
			if (flag4)
			{
				result = 0;
			}
			else
			{
				int currDate = DomainManager.World.GetCurrDate();
				result = (from record in base.CurrEnemyChar.GetCharacter().GetFameActionRecords()
				where record.EndDate > currDate
				where record.Value < 0
				select (int)(record.Value * record.Value)).Sum() / 10;
			}
			return result;
		}

		// Token: 0x04001464 RID: 5220
		private const int EffectDivisor = 10;
	}
}
