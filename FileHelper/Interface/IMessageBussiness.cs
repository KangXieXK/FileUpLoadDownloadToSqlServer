﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileHelper
{
    public interface IMessageBussiness:IModelBussiness
    {
        IMessageModel Work(IMessageModel messageModel);

    }
}
