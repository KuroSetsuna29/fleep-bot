﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace FleepBot.Commands
{
    class DefSetup : BaseCommand
	{
		public override string command_name { get { return "DefSetup"; } }
		public static Regex regex = new Regex(String.Format("^\\{0}defsetup(?:\\s+(.+))?$", FleepBot.Program.COMMAND_PREFIX), RegexOptions.IgnoreCase | RegexOptions.Multiline | RegexOptions.Singleline);

		protected override void execute(string convid, string message, string account_id)
		{
			string search = regex.Match(message).Groups[1].Value;

			if (String.IsNullOrWhiteSpace(search))
			{
				FleepBot.Program.SendErrorMessage(convid, String.Format("Error: Please specify a member. Example: {0}defsetup _Member1_ [, _Member2_ ][, _Member3_ ]", FleepBot.Program.COMMAND_PREFIX));
				return;
			}

			List<string> members = search.Split(',', ' ').Select(x => x.ToLower().Trim()).ToList();
			string query = String.Format("select A, B, C, D, E, F, G, H, I, J, K, L, M, N, O, P, Q, R, S, T, U, V, W, X, Y, Z, AA, AB, AC, AD, AE, AF, AG, AH where lower(A) matches '.*({0}).*'", String.Join("|", members));
			Tuple<List<dynamic>, List<dynamic>> stats = FleepBot.Program.GetGoogleSheet(convid, "1Ge91fmZEbNNLzAc4LwoeIqN8w5pdYMwbxgNuAjd25Pk", "720812518", query, 1, "A:AH");

			if (stats == null)
			{
				return;
			}

			if (!stats.Item1.Any(c => c.id.Value == "A" && c.label.Value.Trim() == "Name")
				|| !stats.Item1.Any(c => c.id.Value == "B" && c.label.Value.Trim() == "Hero1")
				|| !stats.Item1.Any(c => c.id.Value == "D" && c.label.Value.Trim() == "Hero2")
				|| !stats.Item1.Any(c => c.id.Value == "F" && c.label.Value.Trim() == "Hero3")
				|| !stats.Item1.Any(c => c.id.Value == "H" && c.label.Value.Trim() == "Hero4")
				|| !stats.Item1.Any(c => c.id.Value == "J" && c.label.Value.Trim() == "Hero5")
				|| !stats.Item1.Any(c => c.id.Value == "L" && c.label.Value.Trim() == "Hero1")
				|| !stats.Item1.Any(c => c.id.Value == "N" && c.label.Value.Trim() == "Hero2")
				|| !stats.Item1.Any(c => c.id.Value == "P" && c.label.Value.Trim() == "Hero3")
				|| !stats.Item1.Any(c => c.id.Value == "R" && c.label.Value.Trim() == "Hero4")
				|| !stats.Item1.Any(c => c.id.Value == "T" && c.label.Value.Trim() == "Hero5")
				|| !stats.Item1.Any(c => c.id.Value == "V" && c.label.Value.Trim() == "Pet 1")
				|| !stats.Item1.Any(c => c.id.Value == "X" && c.label.Value.Trim() == "Pet 2")
				|| !stats.Item1.Any(c => c.id.Value == "Z" && c.label.Value.Trim() == "Buffgrp1")
				|| !stats.Item1.Any(c => c.id.Value == "AA" && c.label.Value.Trim() == "Buffgrp2")
				|| !stats.Item1.Any(c => c.id.Value == "AB" && c.label.Value.Trim() == "SetItemSet")
				|| !stats.Item1.Any(c => c.id.Value == "AC" && c.label.Value.Trim() == "Costume")
				|| !stats.Item1.Any(c => c.id.Value == "AD" && c.label.Value.Trim() == "7*Skill")
				|| !stats.Item1.Any(c => c.id.Value == "AE" && c.label.Value.Trim() == "Rating")
				|| !stats.Item1.Any(c => c.id.Value == "AF" && c.label.Value.Trim() == "Rank")
				|| !stats.Item1.Any(c => c.id.Value == "AG" && c.label.Value.Trim() == "7*Count")
				|| !stats.Item1.Any(c => c.id.Value == "AH" && c.label.Value.Trim() == "HeroQuality"))
			{
				FleepBot.Program.SendErrorMessage(convid);
				return;
			}

			int nameLen = Math.Max((stats.Item2.Max(x => x.c[0].v.Value.ToString().Trim().Length) ?? 0) + 2, 6);
			int hero1Len = Math.Max(7, Math.Max(
								(stats.Item2.Max(x => (x.c[1] != null ? x.c[1].v.Value.ToString().Trim().Length : 0) + (x.c[2] != null ? x.c[2].v.Value.ToString().Trim().Length : 0)) ?? 0) + 3,
								(stats.Item2.Max(x => (x.c[11] != null ? x.c[11].v.Value.ToString().Trim().Length : 0) + (x.c[12] != null ? x.c[12].v.Value.ToString().Trim().Length : 0)) ?? 0) + 3));
			int hero2Len = Math.Max(7, Math.Max(
								(stats.Item2.Max(x => (x.c[3] != null ? x.c[3].v.Value.ToString().Trim().Length : 0) + (x.c[4] != null ? x.c[4].v.Value.ToString().Trim().Length : 0)) ?? 0) + 3,
								(stats.Item2.Max(x => (x.c[13] != null ? x.c[13].v.Value.ToString().Trim().Length : 0) + (x.c[14] != null ? x.c[14].v.Value.ToString().Trim().Length : 0)) ?? 0) + 3));
			int hero3Len = Math.Max(7, Math.Max(
								(stats.Item2.Max(x => (x.c[5] != null ? x.c[5].v.Value.ToString().Trim().Length : 0) + (x.c[6] != null ? x.c[6].v.Value.ToString().Trim().Length : 0)) ?? 0) + 3,
								(stats.Item2.Max(x => (x.c[15] != null ? x.c[15].v.Value.ToString().Trim().Length : 0) + (x.c[16] != null ? x.c[16].v.Value.ToString().Trim().Length : 0)) ?? 0) + 3));
			int hero4Len = Math.Max(7, Math.Max(
								(stats.Item2.Max(x => (x.c[7] != null ? x.c[7].v.Value.ToString().Trim().Length : 0) + (x.c[8] != null ? x.c[8].v.Value.ToString().Trim().Length : 0)) ?? 0) + 3,
								(stats.Item2.Max(x => (x.c[17] != null ? x.c[17].v.Value.ToString().Trim().Length : 0) + (x.c[18] != null ? x.c[18].v.Value.ToString().Trim().Length : 0)) ?? 0) + 3));
			int hero5Len = Math.Max(7, Math.Max(
								(stats.Item2.Max(x => (x.c[9] != null ? x.c[9].v.Value.ToString().Trim().Length : 0) + (x.c[10] != null ? x.c[10].v.Value.ToString().Trim().Length : 0)) ?? 0) + 3,
								(stats.Item2.Max(x => (x.c[19] != null ? x.c[19].v.Value.ToString().Trim().Length : 0) + (x.c[20] != null ? x.c[20].v.Value.ToString().Trim().Length : 0)) ?? 0) + 3));
			int petLen = Math.Max(5, Math.Max(
								(stats.Item2.Max(x => (x.c[21] != null ? x.c[21].v.Value.ToString().Trim().Length : 0) + (x.c[22] != null ? x.c[22].v.Value.ToString().Trim().Length : 0)) ?? 0) + 4,
								(stats.Item2.Max(x => (x.c[23] != null ? x.c[23].v.Value.ToString().Trim().Length : 0) + (x.c[24] != null ? x.c[24].v.Value.ToString().Trim().Length : 0)) ?? 0) + 4));
			int buffLen = Math.Max(10, Math.Max(
								(stats.Item2.Max(x => (x.c[25] != null ? x.c[25].v.Value.ToString().Trim().Length : 0)) ?? 0) + 2,
								(stats.Item2.Max(x => (x.c[26] != null ? x.c[26].v.Value.ToString().Trim().Length : 0)) ?? 0) + 2));
			int setItemLen = Math.Max((stats.Item2.Max(x => (x.c[27] != null ? x.c[27].v.Value.ToString().Trim().Length : 0)) ?? 0) + 2, 9);
			int costumeLen = Math.Max((stats.Item2.Max(x => (x.c[28] != null ? x.c[28].v.Value.ToString().Trim().Length : 0)) ?? 0) + 2, 9);
			int skill7sLen = Math.Max((stats.Item2.Max(x => (x.c[29] != null ? x.c[29].v.Value.ToString().Trim().Length : 0)) ?? 0) + 2, 9);
			int ratingLen = Math.Max((stats.Item2.Max(x => (x.c[30] != null ? x.c[30].v.Value.ToString().Trim().Length : 0)) ?? 0) + 2, 8);
			int rankLen = Math.Max((stats.Item2.Max(x => (x.c[31] != null ? x.c[31].v.Value.ToString().Trim().Length : 0)) ?? 0) + 2, 6);
			int count7sLen = Math.Max((stats.Item2.Max(x => (x.c[32] != null ? x.c[32].v.Value.ToString().Trim().Length : 0)) ?? 0) + 2, 9);
			int scoreLen = Math.Max((stats.Item2.Max(x => (x.c[33] != null ? x.c[33].v.Value.ToString().Trim().Length : 0)) ?? 0) + 2, 7);

			string msg = String.Format("No member(s) found for '{0}'. Check spelling.", search);
			if (stats.Item2.Count > 0)
			{
				msg = String.Format(":::\n{0}{1}{2}{3}{4}{5}{6}{7}{8}{9}{10}{11}{12}{13}{14}\n",
						"Name".PadRight(nameLen),
						"Hero1".PadRight(hero1Len),
						"Hero2".PadRight(hero2Len),
						"Hero3".PadRight(hero3Len),
						"Hero4".PadRight(hero4Len),
						"Hero5".PadRight(hero5Len),
						"Pet".PadRight(petLen),
						"BuffGrp".PadRight(buffLen),
						"SetItem".PadRight(setItemLen),
						"Costume".PadRight(costumeLen),
						"7*Skill".PadRight(skill7sLen),
						"Rating".PadRight(ratingLen),
						"Rank".PadRight(rankLen),
						"7*Count".PadRight(count7sLen),
						"Score".PadRight(scoreLen))
						+ String.Join("\n", stats.Item2.Select(x => String.Format("{0}{1}{2}{3}{4}{5}{6}{7}{8}{9}{10}{11}{12}{13}{14}\n{15}{16}{17}{18}{19}{20}{21}{22}",
						x.c[0].v.Value.ToString().Trim().PadRight(nameLen),
						((x.c[1] != null ? x.c[1].v.Value.ToString().Trim() : "") + "+" + (x.c[2] != null ? x.c[2].v.Value.ToString().Trim() : "")).PadRight(hero1Len),
						((x.c[3] != null ? x.c[3].v.Value.ToString().Trim() : "") + "+" + (x.c[4] != null ? x.c[4].v.Value.ToString().Trim() : "")).PadRight(hero2Len),
						((x.c[5] != null ? x.c[5].v.Value.ToString().Trim() : "") + "+" + (x.c[6] != null ? x.c[6].v.Value.ToString().Trim() : "")).PadRight(hero3Len),
						((x.c[7] != null ? x.c[7].v.Value.ToString().Trim() : "") + "+" + (x.c[8] != null ? x.c[8].v.Value.ToString().Trim() : "")).PadRight(hero4Len),
						((x.c[9] != null ? x.c[9].v.Value.ToString().Trim() : "") + "+" + (x.c[10] != null ? x.c[10].v.Value.ToString().Trim() : "")).PadRight(hero5Len),
						((x.c[22] != null ? x.c[22].v.Value.ToString().Trim() : "") + "* " + (x.c[21] != null ? x.c[21].v.Value.ToString().Trim() : "")).PadRight(petLen),
						(x.c[25] != null ? x.c[25].v.Value.ToString().Trim() : "").PadRight(buffLen),
						(x.c[27] != null ? x.c[27].v.Value.ToString().Trim() : "").PadRight(setItemLen),
						(x.c[28] != null ? x.c[28].v.Value.ToString().Trim() : "").PadRight(costumeLen),
						(x.c[29] != null ? x.c[29].v.Value.ToString().Trim() : "").PadRight(skill7sLen),
						(x.c[30] != null ? x.c[30].v.Value.ToString().Trim() : "").PadRight(ratingLen),
						(x.c[31] != null ? x.c[31].v.Value.ToString().Trim() : "").PadRight(rankLen),
						(x.c[32] != null ? x.c[32].v.Value.ToString().Trim() : "").PadRight(count7sLen),
						(x.c[33] != null ? x.c[33].v.Value.ToString().Trim() : "").PadRight(scoreLen),
						"".PadRight(nameLen),
						((x.c[11] != null ? x.c[11].v.Value.ToString().Trim() : "") + "+" + (x.c[12] != null ? x.c[12].v.Value.ToString().Trim() : "")).PadRight(hero1Len),
						((x.c[13] != null ? x.c[13].v.Value.ToString().Trim() : "") + "+" + (x.c[14] != null ? x.c[14].v.Value.ToString().Trim() : "")).PadRight(hero2Len),
						((x.c[15] != null ? x.c[15].v.Value.ToString().Trim() : "") + "+" + (x.c[16] != null ? x.c[16].v.Value.ToString().Trim() : "")).PadRight(hero3Len),
						((x.c[17] != null ? x.c[17].v.Value.ToString().Trim() : "") + "+" + (x.c[18] != null ? x.c[18].v.Value.ToString().Trim() : "")).PadRight(hero4Len),
						((x.c[19] != null ? x.c[19].v.Value.ToString().Trim() : "") + "+" + (x.c[20] != null ? x.c[20].v.Value.ToString().Trim() : "")).PadRight(hero5Len),
						((x.c[24] != null ? x.c[24].v.Value.ToString().Trim() : "") + "* " + (x.c[23] != null ? x.c[23].v.Value.ToString().Trim() : "")).PadRight(petLen),
						(x.c[26] != null ? x.c[26].v.Value.ToString().Trim() : "").PadRight(buffLen))));
			}

			FleepBot.Program.SendMessage(convid, msg);
		}
	}
}