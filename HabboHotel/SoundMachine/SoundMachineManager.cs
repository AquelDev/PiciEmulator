using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Butterfly.Core;

namespace Butterfly.HabboHotel.SoundMachine
{
    class SoundMachineManager
    {
        private Dictionary<uint, SoundTemplate> sounds;

        internal void Initialize()
        {
            DateTime start = DateTime.Now;
            sounds = SoundTemplateFactory.GetTemplates();

            TimeSpan timeSpent = DateTime.Now - start;

            Logging.WriteLine("Sound templates -> READY! (" + timeSpent.Seconds + " s, " + timeSpent.Milliseconds + " ms)");
        }

        internal SoundTemplate GetSound(uint soundID)
        {
            SoundTemplate template;
            if (sounds.TryGetValue(soundID, out template))
                return template;

            return template;
        }
    }
}
