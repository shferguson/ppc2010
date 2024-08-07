﻿using System;
using System.Collections.Generic;
using Umbraco.Core.Models;

namespace PPC_2010.Data
{
    public interface ISermonRepository : IDisposable
    {
        ISermon LoadCurrentSermon(string recordingSession);
        ISermon LoadSermon(int sermonId);
        IEnumerable<ISermon> LoadLastSermons(int count);
        IEnumerable<ISermon> LoadSermonsByPage(int pageNumber, int itemsPerPage);
        IEnumerable<ISermon> LoadAllSermons();
        int GetNumberOfSermons();

        void RefreshSermons();
        void RefreshSermon(int sermonId, bool deleted = false);
        void UpdateSermonSort(IMedia item);
    }
}
