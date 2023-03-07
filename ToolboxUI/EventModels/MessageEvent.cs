﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToolboxUI.EventModels
{
    public sealed class MessageEvent
    {
        public MessageEvent(string message)
        {
            Message = message;
        }

        public string Message { get; }
    }
}
