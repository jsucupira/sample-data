﻿using System.Collections.Generic;

namespace Sampler.Core
{
    public class SamplerOptions
    {
        #region Options enum

        public enum Options
        {
            NoOptions = 0,
            IsUnique,
            OneWord,
            Sentense,
            Paragraph,
            Phrase,
            IsNullable,
            Sequential,
            Email,
            NullValue,
            DefaultValue,
            Phone
        }

        #endregion

        public SamplerOptions()
        {
            PropertyOptions = new Dictionary<string, Options>();
            PropertyDefaults = new Dictionary<PropertiesSettings, Options>();
        }

        public Dictionary<string, Options> PropertyOptions { get; set; }
        public Dictionary<PropertiesSettings, Options> PropertyDefaults { get; set; }
    }
}