using System;
using System.Collections.Generic;
using Config.Common;

namespace Config;

[Serializable]
public class CombatScene : ConfigData<CombatSceneItem, short>
{
	public static class DefKey
	{
		public const short EasterEgg = 42;
	}

	public static class DefValue
	{
		public static CombatSceneItem EasterEgg => Instance[(short)42];
	}

	public static CombatScene Instance = new CombatScene();

	private readonly HashSet<string> RequiredFields = new HashSet<string> { "TemplateId" };

	internal override int ToInt(short value)
	{
		return value;
	}

	internal override short ToTemplateId(int value)
	{
		return (short)value;
	}

	private void CreateItems0()
	{
		_dataArray.Add(new CombatSceneItem(0, new List<string> { "combat_scene_101/combat_scene_101", "combat_scene_101/combat_scene_101_1", "combat_scene_101/combat_scene_101_2" }));
		_dataArray.Add(new CombatSceneItem(1, new List<string> { "combat_scene_102/combat_scene_102", "combat_scene_102/combat_scene_102_1", "combat_scene_102/combat_scene_102_2" }));
		_dataArray.Add(new CombatSceneItem(2, new List<string> { "combat_scene_103/combat_scene_103", "combat_scene_103/combat_scene_103_1", "combat_scene_103/combat_scene_103_2" }));
		_dataArray.Add(new CombatSceneItem(3, new List<string> { "combat_scene_104/combat_scene_104", "combat_scene_104/combat_scene_104_1", "combat_scene_104/combat_scene_104_2" }));
		_dataArray.Add(new CombatSceneItem(4, new List<string> { "combat_scene_105/combat_scene_105", "combat_scene_105/combat_scene_105_1", "combat_scene_105/combat_scene_105_2" }));
		_dataArray.Add(new CombatSceneItem(5, new List<string> { "combat_scene_106/combat_scene_106", "combat_scene_106/combat_scene_106_1", "combat_scene_106/combat_scene_106_2" }));
		_dataArray.Add(new CombatSceneItem(6, new List<string> { "combat_scene_107/combat_scene_107", "combat_scene_107/combat_scene_107_1", "combat_scene_107/combat_scene_107_2" }));
		_dataArray.Add(new CombatSceneItem(7, new List<string> { "combat_scene_108/combat_scene_108", "combat_scene_108/combat_scene_108_1", "combat_scene_108/combat_scene_108_2" }));
		_dataArray.Add(new CombatSceneItem(8, new List<string> { "combat_scene_109/combat_scene_109", "combat_scene_109/combat_scene_109_1", "combat_scene_109/combat_scene_109_2" }));
		_dataArray.Add(new CombatSceneItem(9, new List<string> { "combat_scene_110/combat_scene_110", "combat_scene_110/combat_scene_110_1", "combat_scene_110/combat_scene_110_2" }));
		_dataArray.Add(new CombatSceneItem(10, new List<string> { "combat_scene_112/combat_scene_112", "combat_scene_112/combat_scene_112_1", "combat_scene_112/combat_scene_112_2" }));
		_dataArray.Add(new CombatSceneItem(11, new List<string> { "combat_scene_113/combat_scene_113", "combat_scene_113/combat_scene_113_1", "combat_scene_113/combat_scene_113_2" }));
		_dataArray.Add(new CombatSceneItem(12, new List<string> { "combat_scene_114/combat_scene_114", "combat_scene_114/combat_scene_114_1", "combat_scene_114/combat_scene_114_2" }));
		_dataArray.Add(new CombatSceneItem(13, new List<string> { "combat_scene_115/combat_scene_115", "combat_scene_115/combat_scene_115_1", "combat_scene_115/combat_scene_115_2" }));
		_dataArray.Add(new CombatSceneItem(14, new List<string> { "combat_scene_117/combat_scene_117", "combat_scene_117/combat_scene_117_1", "combat_scene_117/combat_scene_117_2" }));
		_dataArray.Add(new CombatSceneItem(15, new List<string> { "combat_scene_118/combat_scene_118", "combat_scene_118/combat_scene_118_1", "combat_scene_118/combat_scene_118_2" }));
		_dataArray.Add(new CombatSceneItem(16, new List<string> { "combat_scene_119/combat_scene_119", "combat_scene_119/combat_scene_119_1", "combat_scene_119/combat_scene_119_2" }));
		_dataArray.Add(new CombatSceneItem(17, new List<string> { "combat_scene_120/combat_scene_120" }));
		_dataArray.Add(new CombatSceneItem(18, new List<string> { "combat_scene_121/combat_scene_121" }));
		_dataArray.Add(new CombatSceneItem(19, new List<string> { "combat_scene_122/combat_scene_122" }));
		_dataArray.Add(new CombatSceneItem(20, new List<string> { "combat_scene_123/combat_scene_123" }));
		_dataArray.Add(new CombatSceneItem(21, new List<string> { "combat_scene_124/combat_scene_124" }));
		_dataArray.Add(new CombatSceneItem(22, new List<string> { "combat_scene_125/combat_scene_125" }));
		_dataArray.Add(new CombatSceneItem(23, new List<string> { "combat_scene_111/combat_scene_111_0" }));
		_dataArray.Add(new CombatSceneItem(24, new List<string> { "combat_scene_111/combat_scene_111_1" }));
		_dataArray.Add(new CombatSceneItem(25, new List<string> { "combat_scene_111/combat_scene_111_2" }));
		_dataArray.Add(new CombatSceneItem(26, new List<string> { "combat_scene_111/combat_scene_111_3" }));
		_dataArray.Add(new CombatSceneItem(27, new List<string> { "combat_scene_111/combat_scene_111_4" }));
		_dataArray.Add(new CombatSceneItem(28, new List<string> { "combat_scene_111/combat_scene_111_5" }));
		_dataArray.Add(new CombatSceneItem(29, new List<string> { "combat_scene_111/combat_scene_111_6" }));
		_dataArray.Add(new CombatSceneItem(30, new List<string> { "combat_scene_111/combat_scene_111_7" }));
		_dataArray.Add(new CombatSceneItem(31, new List<string> { "combat_scene_111/combat_scene_111_8" }));
		_dataArray.Add(new CombatSceneItem(32, new List<string> { "combat_scene_126/combat_scene_126" }));
		_dataArray.Add(new CombatSceneItem(33, new List<string> { "combat_scene_127/combat_scene_127" }));
		_dataArray.Add(new CombatSceneItem(34, new List<string> { "combat_scene_128/combat_scene_128" }));
		_dataArray.Add(new CombatSceneItem(35, new List<string> { "combat_scene_129/combat_scene_129" }));
		_dataArray.Add(new CombatSceneItem(36, new List<string> { "combat_scene_130/combat_scene_130" }));
		_dataArray.Add(new CombatSceneItem(37, new List<string> { "combat_scene_131/combat_scene_131" }));
		_dataArray.Add(new CombatSceneItem(38, new List<string> { "combat_scene_132/combat_scene_132" }));
		_dataArray.Add(new CombatSceneItem(39, new List<string> { "combat_scene_133/combat_scene_133" }));
		_dataArray.Add(new CombatSceneItem(40, new List<string> { "combat_scene_143/combat_scene_143" }));
		_dataArray.Add(new CombatSceneItem(41, new List<string> { "combat_scene_145/combat_scene_145" }));
		_dataArray.Add(new CombatSceneItem(42, new List<string> { "combat_scene_146/combat_scene_146" }));
		_dataArray.Add(new CombatSceneItem(43, new List<string> { "combat_scene_147/combat_scene_147" }));
		_dataArray.Add(new CombatSceneItem(44, new List<string> { "combat_scene_148/combat_scene_148" }));
		_dataArray.Add(new CombatSceneItem(45, new List<string> { "combat_scene_155/combat_scene_155_lei" }));
		_dataArray.Add(new CombatSceneItem(46, new List<string> { "combat_scene_155/combat_scene_155_shui" }));
		_dataArray.Add(new CombatSceneItem(47, new List<string> { "combat_scene_155/combat_scene_155_feng" }));
		_dataArray.Add(new CombatSceneItem(48, new List<string> { "combat_scene_155/combat_scene_155_huo" }));
		_dataArray.Add(new CombatSceneItem(49, new List<string> { "combat_scene_155/combat_scene_155" }));
		_dataArray.Add(new CombatSceneItem(50, new List<string> { "combat_scene_156/combat_scene_156" }));
		_dataArray.Add(new CombatSceneItem(51, new List<string> { "combat_scene_151/combat_scene_151" }));
		_dataArray.Add(new CombatSceneItem(52, new List<string> { "combat_scene_149/combat_scene_149" }));
		_dataArray.Add(new CombatSceneItem(53, new List<string> { "combat_scene_152/combat_scene_152" }));
		_dataArray.Add(new CombatSceneItem(54, new List<string> { "combat_scene_150/combat_scene_150" }));
	}

	public override void Init()
	{
		base.Init();
		_dataArray = new List<CombatSceneItem>(55);
		CreateItems0();
	}
}
