﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Timers;

namespace FleepBot.Commands
{
    class Remind : BaseCommand
	{
		public class Reminder
		{
			public string ID = Guid.NewGuid().ToString("N");
			public string User = "";
			public TimeSpan Delay = new TimeSpan();
			public string ConvID = "";
			public string Message = "";
			public string HangoutsChat = "";
			private Timer timer = new Timer();
			private DateTime _ends = new DateTime();

			public Reminder(string user, TimeSpan delay, string convid, string message)
			{
				this.User = user;
				this.Delay = delay;
				this.ConvID = convid;
				this.Message = message;
				
				timer = new Timer(delay.TotalMilliseconds);
				timer.Elapsed += new ElapsedEventHandler(execute);
				timer.Enabled = true;
				timer.AutoReset = false;
				_ends = DateTime.Now.Add(delay);

				FleepBot.Program.REMINDERS.Add(this);
			}

			public Reminder(string user, TimeSpan delay, string convid, string message, string hangoutsChat)
			{
				this.User = user;
				this.Delay = delay;
				this.ConvID = convid;
				this.Message = message;
				this.HangoutsChat = hangoutsChat;

				timer = new Timer(delay.TotalMilliseconds);
				timer.Elapsed += new ElapsedEventHandler(execute);
				timer.Enabled = true;
				timer.AutoReset = false;
				_ends = DateTime.Now.Add(delay);

				FleepBot.Program.REMINDERS.Add(this);
			}

			public TimeSpan TimeLeft
			{
				get { return _ends - DateTime.Now; }
			}

			public string TimeLeftAsString
			{
				get { return TimeLeft.ToString("dd\\.hh\\:mm\\:ss"); }
			}

			public void Remove()
			{
				timer.Enabled = false;
				FleepBot.Program.REMINDERS.Remove(this);
			}

			private void execute(object source, ElapsedEventArgs e)
			{
				Remove();
				FleepBot.Program.SendMessage(ConvID, String.Format("From {0}:\n:::\n{1}", User, Message));

				if (!String.IsNullOrWhiteSpace(HangoutsChat))
				{
					FleepBot.Program.SendHangouts(HangoutsChat, Message);
				}
			}
		}

		public override string command_name { get { return "Remind"; } }
		public static Regex regex = new Regex(String.Format("^\\{0}remind(?:\\s+((?:(\\d+)d)?(?:(\\d+)h)?(?:(\\d+)m)?(?:(\\d+)s)?))?(?:\\s+(.+))?$", FleepBot.Program.COMMAND_PREFIX), RegexOptions.IgnoreCase | RegexOptions.Multiline | RegexOptions.Singleline);

		protected override void execute(string convid, string message, string account_id)
		{
			string delay = regex.Match(message).Groups[1].Value;
			string days = regex.Match(message).Groups[2].Value ?? "0";
			string hours = regex.Match(message).Groups[3].Value ?? "0";
			string minutes = regex.Match(message).Groups[4].Value ?? "0";
			string seconds = regex.Match(message).Groups[5].Value ?? "0";
			string msg = regex.Match(message).Groups[6].Value;

			days = String.IsNullOrEmpty(days) ? "0" : days;
			hours = String.IsNullOrEmpty(hours) ? "0" : hours;
			minutes = String.IsNullOrEmpty(minutes) ? "0" : minutes;
			seconds = String.IsNullOrEmpty(seconds) ? "0" : seconds;

			if (String.IsNullOrWhiteSpace(delay) || String.IsNullOrWhiteSpace(msg))
			{
				FleepBot.Program.SendErrorMessage(convid, String.Format("Error: Please specify a valid delay. Example: {0}remind 1h30m This message will show after 1 hour 30 minutes", FleepBot.Program.COMMAND_PREFIX));
				return;
			}

			TimeSpan timespan = new TimeSpan(int.Parse(days), int.Parse(hours), int.Parse(minutes), int.Parse(seconds));
			string contact_name = FleepBot.Program.GetUserName(account_id);

			Reminder r = new Reminder(contact_name, timespan, convid, msg);

			FleepBot.Program.SendMessage(convid, String.Format("Set reminder for {0}", timespan.ToString("dd\\.hh\\:mm\\:ss")));
        }
	}
}