﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace FleepBot.Commands
{
    class IGN
	{
		public static Regex regex = new Regex(String.Format("^<msg><p>\\{0}ign(?:\\s+(.+))?</p></msg>$", FleepBot.Program.COMMAND_PREFIX), RegexOptions.IgnoreCase);

		public static void execute(string convid, string message, string account_id)
		{
			string ign = regex.Match(message).Groups[1].Value;
			bool isGet = String.IsNullOrEmpty(ign);

			if (isGet)
			{
				dynamic memberinfo = FleepBot.Program.ApiPost("api/contact/sync", new { contact_id = account_id, ticket = FleepBot.Program.TICKET });
				string contact_name = memberinfo.contact_name;

				FleepBot.Program.SendMessage(convid, String.Format("Your IGN is {0}", String.IsNullOrEmpty(contact_name) ? String.Format("not set. Please set using the following command, {0}ign _InGameName_", FleepBot.Program.COMMAND_PREFIX) : "*" + contact_name + "*"));
			}
			else
			{
				dynamic memberinfo = FleepBot.Program.ApiPost("api/contact/describe", new { contact_id = account_id, contact_name = ign, ticket = FleepBot.Program.TICKET });
				string contact_name = memberinfo.contact_name;

				if (!String.IsNullOrEmpty(contact_name))
				{
					FleepBot.Program.SendMessage(convid, String.Format("Successfully set your IGN to *{0}*", contact_name));
				}
				else
				{
					FleepBot.Program.SendErrorMessage(convid);
				}
			}
		}
	}
}
