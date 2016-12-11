﻿using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.Drawing;

namespace Server
{
    [CallbackBehavior(UseSynchronizationContext = false)]
    public class MyCallback : IServiceCallBack
    {
        public List<string> Filters = new List<string>();
        public byte[] Result = new byte[200000000];
        public int Progress = 0;
        public bool ImageHere = false;

        public void GetFilters(List<string> filters)
        {
            this.Filters = filters;
        }


        public void GetImage(byte[] image, bool flag)
        {
            Result = image;
            ImageHere = flag;
        }

        public void GetProgress(int progress)
        {
            Progress = progress;
        }
    }
}
