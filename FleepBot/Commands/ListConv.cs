﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace FleepBot.Commands
{
    class ListConv : BaseCommand
	{
		public override string command_name { get { return "ListConv"; } }
		public static Regex regex = new Regex(String.Format("^\\{0}listconv(?:\\s+(.+))?$", FleepBot.Program.ADMIN_COMMAND_PREFIX), RegexOptions.IgnoreCase | RegexOptions.Multiline | RegexOptions.Singleline);

		protected override void execute(string convid, string message, string account_id)
		{
			if (!FleepBot.Program.ADMIN_CHATS.Contains(convid.ToLower()))
			{
				FleepBot.Program.SendErrorMessage(convid, "Error: Admin commands not permitted");
				return;
			}

			string filter = regex.Match(message).Groups[1].Value;

			long? sync_horizon = 0;
			bool found = false;
			dynamic conversations = new { };
			do
			{
				dynamic list = FleepBot.Program.ApiPost("api/conversation/list", new { sync_horizon = sync_horizon, ticket = FleepBot.Program.TICKET });
				conversations = list.conversations;
				sync_horizon = list.sync_horizon.Value;

				foreach (dynamic conversation in conversations)
				{
					if (conversation.conversation_id != null && ((string)conversation.topic).ToLower().Contains((filter ?? "").ToLower()))
					{
						FleepBot.Program.SendMessage(convid, String.Format(":::\n{0}", conversation));

						found = true;
						break;
					}
				}

			} while (!found && conversations != null && conversations.HasValues);

			if (!found)
				FleepBot.Program.SendErrorMessage(convid, "Error: could not find conversation.");
		}
	}
}