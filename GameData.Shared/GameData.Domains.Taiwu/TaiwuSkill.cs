namespace GameData.Domains.Taiwu;

public interface TaiwuSkill
{
	sbyte GetBookPageReadingProgress(byte index);

	void SetBookPageReadingProgress(byte index, sbyte progress);

	sbyte[] GetAllBookPageReadingProgress();
}
