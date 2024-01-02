﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ShadowRando
{
	public static class Nukkoro2
	{
		static readonly Encoding shift_jis = Encoding.GetEncoding(932);
		public static Dictionary<string, Nukkoro2Stage> ReadFile(string path)
		{
			Dictionary<string, Nukkoro2Stage> result = new Dictionary<string, Nukkoro2Stage>();
			string[] data = File.ReadAllLines(path, shift_jis);
			Nukkoro2Stage stage = null;
			for (int i = 0; i < data.Length; i++)
			{
				string line = data[i].Split('#')[0];
				int endbracket;
				if (line.Length > 0 && line[0] == '[' && (endbracket = line.IndexOf(']')) != -1)
				{
					stage = new Nukkoro2Stage();
					try
					{
						result.Add(line.Substring(1, endbracket - 1), stage);
					}
					catch { }
				}
				else if (!string.IsNullOrWhiteSpace(line))
				{
					string[] split = line.Split(new[] { " : " }, StringSplitOptions.None);
					if (split.Length == 2)
					{
						switch (split[0])
						{
							case "PLAYER":
								{
									string[] sp2 = split[1].Split(' ');
									stage.Player.Add(int.Parse(sp2[0]), new Nukkoro2Player(sp2));
								}
								break;
							case "STARTSPD":
								{
									string[] sp2 = split[1].Split(' ');
									stage.StartSpeed.Add(int.Parse(sp2[0]), new Nukkoro2Vector(int.Parse(sp2[1]), int.Parse(sp2[2]), int.Parse(sp2[3])));
								}
								break;
							case "STARTDEMO":
								stage.StartDemo = int.Parse(split[1]);
								break;
							case "RANK_H":
								stage.RankHero = new Nukkoro2Rank(split[1]);
								break;
							case "RANK_D":
								stage.RankDark = new Nukkoro2Rank(split[1]);
								break;
							case "RANK_N":
								stage.RankNeutral = new Nukkoro2Rank(split[1]);
								break;
							case "GOALEVENTPOS_H":
								stage.GoalEventPosHero = new Nukkoro2Vector(split[1]);
								break;
							case "GOALEVENTPOS_D":
								stage.GoalEventPosDark = new Nukkoro2Vector(split[1]);
								break;
							case "GOALEVENTPOS_N":
								stage.GoalEventPosNeutral = new Nukkoro2Vector(split[1]);
								break;
							case "GOALEVENTPOS_X":
								stage.GoalEventPosExpert = new Nukkoro2Vector(split[1]);
								break;
							case "MISSIONCOUNT_H":
								stage.MissionCountHero = new Nukkoro2Mission(split[1]);
								break;
							case "MISSIONCOUNT_D":
								stage.MissionCountDark = new Nukkoro2Mission(split[1]);
								break;
							case "MISSIONCOUNT_N":
								stage.MissionCountNeutral = new Nukkoro2Mission(split[1]);
								break;
							case "MISSIONCOUNT_HARD":
								stage.MissionCountExpert = new Nukkoro2Mission(split[1]);
								break;
							case "MIPMAPK":
								stage.MipmapK = int.Parse(split[1]);
								break;
						}
					}
				}
			}
			return result;

		}

		public static void WriteFile(string path, Dictionary<string, Nukkoro2Stage> map)
		{
			using (var fs = File.Create(path))
			using (var sw = new StreamWriter(fs, shift_jis))
			{
				foreach (var stage in map)
				{
					sw.WriteLine("[{0}]", stage.Key);
					foreach (var pl in stage.Value.Player)
						sw.WriteLine("PLAYER : {0} {1}", pl.Key, pl.Value);
					foreach (var pl in stage.Value.StartSpeed)
						sw.WriteLine("STARTSPD : {0} {1}", pl.Key, pl.Value);
					if (stage.Value.StartDemo != 0)
						sw.WriteLine("STARTDEMO : {0}", stage.Value.StartDemo);
					if (stage.Value.RankHero != null)
						sw.WriteLine("RANK_H : {0}", stage.Value.RankHero);
					if (stage.Value.RankDark != null)
						sw.WriteLine("RANK_D : {0}", stage.Value.RankDark);
					if (stage.Value.RankNeutral != null)
						sw.WriteLine("RANK_N : {0}", stage.Value.RankNeutral);
					if (stage.Value.GoalEventPosHero != null)
						sw.WriteLine("GOALEVENTPOS_H : {0}", stage.Value.GoalEventPosHero);
					if (stage.Value.GoalEventPosDark != null)
						sw.WriteLine("GOALEVENTPOS_D : {0}", stage.Value.GoalEventPosDark);
					if (stage.Value.GoalEventPosNeutral != null)
						sw.WriteLine("GOALEVENTPOS_N : {0}", stage.Value.GoalEventPosNeutral);
					if (stage.Value.GoalEventPosExpert != null)
						sw.WriteLine("GOALEVENTPOS_X : {0}", stage.Value.GoalEventPosExpert);
					if (stage.Value.MissionCountHero != null)
						sw.WriteLine("MISSIONCOUNT_H : {0}", stage.Value.MissionCountHero);
					if (stage.Value.MissionCountDark != null)
						sw.WriteLine("MISSIONCOUNT_D : {0}", stage.Value.MissionCountDark);
					if (stage.Value.MissionCountNeutral != null)
						sw.WriteLine("MISSIONCOUNT_N : {0}", stage.Value.MissionCountNeutral);
					if (stage.Value.MissionCountExpert != null)
						sw.WriteLine("MISSIONCOUNT_X : {0}", stage.Value.MissionCountExpert);
					if (stage.Value.MipmapK != 0)
						sw.WriteLine("MIPMAPK : {0}", stage.Value.MipmapK);
					sw.WriteLine();
				}
			}
		}
	}

	public class Nukkoro2Stage
	{
		public Dictionary<int, Nukkoro2Player> Player { get; set; } = new Dictionary<int, Nukkoro2Player>();
		public Dictionary<int, Nukkoro2Vector> StartSpeed { get; set; } = new Dictionary<int, Nukkoro2Vector>();
		public int StartDemo { get; set; }
		public Nukkoro2Rank RankHero { get; set; }
		public Nukkoro2Rank RankDark { get; set; }
		public Nukkoro2Rank RankNeutral { get; set; }
		public Nukkoro2Vector GoalEventPosHero { get; set; }
		public Nukkoro2Vector GoalEventPosDark { get; set; }
		public Nukkoro2Vector GoalEventPosNeutral { get; set; }
		public Nukkoro2Vector GoalEventPosExpert { get; set; }
		public Nukkoro2Mission MissionCountHero { get; set; }
		public Nukkoro2Mission MissionCountDark { get; set; }
		public Nukkoro2Mission MissionCountNeutral { get; set; }
		public Nukkoro2Mission MissionCountExpert { get; set; }
		public int MipmapK { get; set; }
	}

	public class Nukkoro2Player
	{
		public Nukkoro2Vector Position { get; set; }
		public Nukkoro2Vector Rotation { get; set; }

		public Nukkoro2Player() { }

		public Nukkoro2Player(string[] data)
		{
			Position = new Nukkoro2Vector(int.Parse(data[1]), int.Parse(data[2]), int.Parse(data[3]));
			Rotation = new Nukkoro2Vector(int.Parse(data[4]), int.Parse(data[5]), int.Parse(data[6]));
		}

		public override string ToString() => $"{Position} {Rotation}";
	}

	public class Nukkoro2Vector
	{
		public int X { get; set; }
		public int Y { get; set; }
		public int Z { get; set; }

		public Nukkoro2Vector() { }

		public Nukkoro2Vector(int x, int y, int z)
		{
			X = x;
			Y = y;
			Z = z;
		}

		public Nukkoro2Vector(string data)
		{
			string[] split = data.Split(' ');
			X = int.Parse(split[0]);
			Y = int.Parse(split[1]);
			Z = int.Parse(split[2]);
		}

		public override string ToString() => $"{X} {Y} {Z}";
	}

	public class Nukkoro2Rank
	{
		public int A { get; set; }
		public int B { get; set; }
		public int C { get; set; }
		public int D { get; set; }

		public Nukkoro2Rank() { }

		public Nukkoro2Rank(string data)
		{
			string[] split = data.Split(' ');
			A = int.Parse(split[0]);
			B = int.Parse(split[1]);
			C = int.Parse(split[2]);
			D = int.Parse(split[3]);
		}

		public override string ToString() => $"{A} {B} {C} {D}";
	}

	public class Nukkoro2Mission
	{
		public int Success { get; set; }
		public int Failure { get; set; }

		public Nukkoro2Mission() { }

		public Nukkoro2Mission(string data)
		{
			string[] split = data.Split(' ');
			Success = int.Parse(split[0]);
			try
			{
				Failure = int.Parse(split[1]);
			} catch (FormatException)
			{
				Failure = int.Parse(split[2]);
			}
		}

		public override string ToString() => $"{Success} {Failure}";
	}
}
